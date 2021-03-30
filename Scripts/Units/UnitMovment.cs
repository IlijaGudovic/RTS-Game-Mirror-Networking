using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovment : NetworkBehaviour
{

    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Targeter targeterScript = null;

    [SerializeField] private float chaceRange = 10;


    [ServerCallback]
    private void Update()
    {

        Targetable target = targeterScript.getTarget();

        if (target != null)
        {

            if ((target.transform.position - transform.position).sqrMagnitude > chaceRange * chaceRange)
            {
                agent.SetDestination(target.transform.position);
            }
            else if (agent.hasPath)
            {
                agent.ResetPath();
            }

            return;

        }

        if (agent.hasPath)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                agent.ResetPath();
            }
        }

    }

    #region Server

    [Command]
    public void CmdMove(Vector3 whereToGo)
    {

        ServerMove(whereToGo);

    }

    [Server]
    public void ServerMove(Vector3 whereToGo)
    {

        targeterScript.cleareTarget();

        if (NavMesh.SamplePosition(whereToGo, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

    }

    #endregion












}
