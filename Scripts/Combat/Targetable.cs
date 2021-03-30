using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targetable : NetworkBehaviour
{

    [SerializeField] private Transform aimPoint = null;

    public Transform getAimAtPoint()
    {
        return aimPoint;
    }

}
