using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class UnitBase : NetworkBehaviour
{


    [SerializeField] private Helth helth = null;

    public static event Action<UnitBase> serverOnUnitBaseSpawn;
    public static event Action<UnitBase> serverOnUnitBaseDespawn;

    public static event Action<int> serverOnPlayerDie;

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x , Camera.main.transform.position.y, transform.position.z - 13);
    }

    #region Server

    public override void OnStartServer()
    {

        helth.serverOnDie += serverHendleDie;

        serverOnUnitBaseSpawn?.Invoke(this);

    }

    public override void OnStopServer()
    {

        helth.serverOnDie -= serverHendleDie;

        serverOnUnitBaseDespawn?.Invoke(this);

    }

    [Server]
    private void serverHendleDie()
    {

        serverOnPlayerDie?.Invoke(connectionToClient.connectionId);

        NetworkServer.Destroy(gameObject);

    }

    #endregion


    #region Client

    #endregion

}
