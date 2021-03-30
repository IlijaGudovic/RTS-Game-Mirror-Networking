using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{

    public Targetable target;


    public Targetable getTarget()
    {
        return target;
    }


    [Command]
    public void CmdSetTarget(GameObject targetGameObj)
    {

        if (targetGameObj.TryGetComponent<Targetable>(out Targetable newTarget))
        {

            target = newTarget;

        }

    }


    [Server]
    public void cleareTarget()
    {

        target = null;

    }


}
