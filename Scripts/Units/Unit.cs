using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class Unit : NetworkBehaviour
{

    public int resourcesCost = 50;

    [SerializeField] private Targeter targeterScript = null;

    [SerializeField] private UnitMovment movmentScript = null;
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    public static event Action<Unit> serverOnUnitSpawn;
    public static event Action<Unit> serverOnUnitDespawn;

    public static event Action<Unit> authorityOnUnitSpawn;
    public static event Action<Unit> authorityOnUnitDespawn;

    [SerializeField] private Helth helth = null;

    [SerializeField] private GameObject effect = null;


    public UnitMovment getMovmentScript()
    {
        return movmentScript;
    }

    public Targeter getTargeterScript()
    {
        return targeterScript;
    }


    #region Server

    public override void OnStartServer()
    {

        serverOnUnitSpawn?.Invoke(this);

        helth.serverOnDie += serverHendleDie;

    }

    public override void OnStopServer()
    {

        serverOnUnitDespawn?.Invoke(this);

        helth.serverOnDie -= serverHendleDie;

    }


    [Server]
    private void serverHendleDie()
    {

        GameObject spawnedEffect = Instantiate(effect, gameObject.GetComponent<Targetable>().getAimAtPoint().transform.position, Quaternion.identity);

        NetworkServer.Spawn(spawnedEffect, connectionToClient);

        NetworkServer.Destroy(gameObject);
    }

    #endregion


    #region Clinet



    public override void OnStartAuthority()
    {

        authorityOnUnitSpawn?.Invoke(this);

    }

    public override void OnStopClient()
    {

        if ( !hasAuthority)
        {
            return;
        }

        authorityOnUnitDespawn?.Invoke(this);

    }



    [Client]
    public void Select()
    {
        if (hasAuthority)
        {
            onSelected?.Invoke();
        }
        
    }


    [Client]
    public void Deselect()
    {
        if (hasAuthority)
        {
            onDeselected?.Invoke();
        }
    }

    #endregion

}
