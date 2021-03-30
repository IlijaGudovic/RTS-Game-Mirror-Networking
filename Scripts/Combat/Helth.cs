using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Helth : NetworkBehaviour
{


    [SerializeField] private int maxHp = 100;

    [SyncVar(hook = nameof(hpUpdate))]
    private int currentHp;

    public event Action serverOnDie;

    public event Action<int, int> clientOnHpUpdate;


    #region Server

    public override void OnStartServer()
    {

        currentHp = maxHp;

        UnitBase.serverOnPlayerDie += ServerHandlePlayerId;

    }

    public override void OnStopServer()
    {

        UnitBase.serverOnPlayerDie -= ServerHandlePlayerId;

    }

    [Server]
    private void ServerHandlePlayerId(int id)
    {

        if (connectionToClient.connectionId != id)
        {
            return;
        }

        dealDamage(currentHp);

    }


    [Server]
    public void dealDamage(int damage)
    {

        if (currentHp == 0)
        {
            return;
        }

        currentHp = Mathf.Max(currentHp - damage, 0);

        if (currentHp != 0)
        {
            return;
        }

        serverOnDie?.Invoke();

    }

    #endregion



    #region Client

    private void hpUpdate(int oldHp, int newHp)
    {

        clientOnHpUpdate(newHp, maxHp);

    }

    #endregion



}
