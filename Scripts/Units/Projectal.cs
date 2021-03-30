using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectal : NetworkBehaviour
{

    [SerializeField] private Rigidbody rb = null;

    [SerializeField] private float destroyAfter = 5f;
    [SerializeField] private float launchSpeed = 10f;

    [SerializeField] private int damage = 40;

    private void Start()
    {

        rb.velocity = transform.forward * launchSpeed;

    }

    public override void OnStartServer()
    {
        Invoke(nameof(selfDestroy), destroyAfter);
    }



    [ServerCallback]
    public void OnTriggerEnter(Collider target)
    {

        if (target.TryGetComponent<NetworkIdentity>(out NetworkIdentity identity))
        {

            if (identity.connectionToClient == connectionToClient) //Self triger
            {
                return;
            }

            if (target.TryGetComponent<Helth>(out Helth hp))
            {

                hp.dealDamage(damage);

                selfDestroy();

            }

        }
        
    }


    [Server]
    public void selfDestroy()
    {

        NetworkServer.Destroy(gameObject);

    }


}
