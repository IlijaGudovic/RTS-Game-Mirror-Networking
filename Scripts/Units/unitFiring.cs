using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class unitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeterScript = null;

    [SerializeField] private GameObject projectalPrefab = null;
    [SerializeField] private Transform firePoint = null;

    [SerializeField] private float fireRate = 5f;
    [SerializeField] private float fireRange = 1f;

    [SerializeField] private float rotationSpeed = 20f;

    private float lastFireTime;


    [ServerCallback]
    private void Update()
    {

        if (targeterScript.getTarget() == null)
        {
            return;
        }

        if (canFireOnTarget())
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targeterScript.getTarget().transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / fireRate) + lastFireTime)
        {

            Quaternion projectalRotation = Quaternion.LookRotation(targeterScript.getTarget().getAimAtPoint().position - firePoint.transform.position);

            GameObject newPorjectal = Instantiate(projectalPrefab, firePoint.position, projectalRotation);

            NetworkServer.Spawn(newPorjectal, connectionToClient);

            lastFireTime = Time.time;

        }

    }


    [Server]
    private bool canFireOnTarget()
    {

        return (targeterScript.getTarget().transform.position - transform.position).sqrMagnitude > fireRange * fireRange;

    }

}
