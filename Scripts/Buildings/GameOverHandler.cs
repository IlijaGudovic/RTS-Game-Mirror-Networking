using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;


public class GameOverHandler : NetworkBehaviour
{

    public static event Action ServerOnGameover;
    public static event Action<string> ClientOnGameover;


    #region Server

    public List<UnitBase> bases = new List<UnitBase>();

    public override void OnStartServer()
    {

        UnitBase.serverOnUnitBaseSpawn += ServerHandleBaseSpawn;
        UnitBase.serverOnUnitBaseDespawn += ServerHandleBaseDespawn;

    }

    public override void OnStopServer()
    {

        UnitBase.serverOnUnitBaseSpawn -= ServerHandleBaseSpawn;
        UnitBase.serverOnUnitBaseDespawn -= ServerHandleBaseDespawn;

    }

    [Server]
    private void ServerHandleBaseSpawn(UnitBase unitBase)
    {

        bases.Add(unitBase);

    }

    [Server]
    private void ServerHandleBaseDespawn(UnitBase unitBase)
    {

        bases.Remove(unitBase);

        if (bases.Count != 1)
        {
            return;
        }

        int playerId = bases[0].connectionToClient.connectionId;

        RpcGameOver("Player " + playerId);

        ServerOnGameover?.Invoke();

    }

    #endregion



    #region Client

    [ClientRpc]
    private void RpcGameOver(string winner)
    {

        ClientOnGameover?.Invoke(winner);

    }

    #endregion





}
