using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Building : NetworkBehaviour
{

    [SerializeField] private GameObject buildingPriview = null;

    [SerializeField] private Sprite icon = null;
    [SerializeField] private int id = 0;
    [SerializeField] private int price = 100;

    public static event Action<Building> serverOnBuildingSpawn;
    public static event Action<Building> serverOnBuildingDespawn;

    public static event Action<Building> authorityOnBuildingSpawn;
    public static event Action<Building> authorityOnBuildingDespawn;

    [SerializeField] private Renderer renderMatirial = null;

    public Sprite getIcon()
    {
        return icon;
    }

    public int getId()
    {
        return id;
    }

    public int getPrice()
    {
        return price;
    }

    public GameObject getBuildingPreview()
    {
        return buildingPriview;
    }


    #region Server

    private void Start()
    {
        if (!hasAuthority)
        {
            renderMatirial.material.color = Color.black;
        }
    }

    public override void OnStartServer()
    {
        serverOnBuildingSpawn?.Invoke(this);

    }

    public override void OnStopServer()
    {
        serverOnBuildingDespawn?.Invoke(this);
    }


    #endregion



    #region Client

    public override void OnStartAuthority()
    {

        authorityOnBuildingSpawn?.Invoke(this);

    }

    public override void OnStopClient()
    {

        if (!hasAuthority)
        {
            return;
        }

        authorityOnBuildingDespawn?.Invoke(this);

    }


    #endregion


}
