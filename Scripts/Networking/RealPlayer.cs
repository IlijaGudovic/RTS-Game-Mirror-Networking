using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class RealPlayer : NetworkBehaviour
{

    [SerializeField] private LayerMask BuildingBlockPlayer = new LayerMask();
    [SerializeField] private float buildingRangeLimit = 9f;

    [SerializeField] private Building[] availableBuildings = new Building[0];

    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    [SerializeField] private List<Building> myBuildings = new List<Building>();

    [SyncVar(hook = nameof(onResourcesChange))] public int resources = 500;

    public event Action<int> clientResourcesChanged;

    public Color teamColor;

    

    public List<Unit> getMyUnits()
    {
        return (myUnits);
    }


    public List<Building> getMyBuildings()
    {
        return (myBuildings);
    }


    public bool CanPlaceBuildingBool(BoxCollider buildingBoxCollider, Vector3 spawnPostion)
    {

        if (Physics.CheckBox(spawnPostion + buildingBoxCollider.center, buildingBoxCollider.size / 2, Quaternion.identity, BuildingBlockPlayer))
        {
            return false;
        }

        foreach (Building building in myBuildings)
        {
            if ((spawnPostion - building.transform.position).sqrMagnitude < buildingRangeLimit * buildingRangeLimit)
            {
                return true;
            }
        }

        return false;

    }


    #region Server

    public override void OnStartServer()
    {
        //Dodaje komandu na event
        Unit.serverOnUnitSpawn += ServerHendleSpawnedUnit;
        Unit.serverOnUnitDespawn += ServerHendleDespawnedUnit;

        Building.serverOnBuildingSpawn += ServerHendleSpawnedBuild;
        Building.serverOnBuildingDespawn += ServerHendleDespawnedBuild;
    }

    public override void OnStopServer()
    {
        Unit.serverOnUnitSpawn -= ServerHendleSpawnedUnit;
        Unit.serverOnUnitDespawn -= ServerHendleDespawnedUnit;

        Building.serverOnBuildingSpawn -= ServerHendleSpawnedBuild;
        Building.serverOnBuildingDespawn -= ServerHendleDespawnedBuild;
    }


    //Buildings
    private void ServerHendleSpawnedBuild(Building building)
    {

        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myBuildings.Add(building);

    }

    private void ServerHendleDespawnedBuild(Building building)
    {

        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myBuildings.Remove(building);

    }


    [Command]
    public void tryPlaceBuild(int buildingId, Vector3 spawnPostion)
    {

        Building buildingToPlace = null;

        foreach (Building building in availableBuildings)
        {

            if (building.getId() == buildingId)
            {

                buildingToPlace = building;

                break;

            }

        }

        if (buildingToPlace == null)
        {
            return;
        }

        if (resources < buildingToPlace.getPrice())
        {
            return;
        }

        BoxCollider buildingBoxCollider = buildingToPlace.GetComponent<BoxCollider>();

        if (CanPlaceBuildingBool(buildingBoxCollider, spawnPostion) == false)
        {
            return;
        }

        GameObject spawnedBuilding = Instantiate(buildingToPlace.gameObject, spawnPostion, buildingToPlace.transform.rotation);

        NetworkServer.Spawn(spawnedBuilding, connectionToClient);

        resources -= buildingToPlace.getPrice();

    }


    //Units
    private void ServerHendleSpawnedUnit(Unit unit)
    {

        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Add(unit);

    }

    private void ServerHendleDespawnedUnit(Unit unit)
    {

        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Remove(unit);

    }

    #endregion


    #region Client

    private void onResourcesChange(int oldResources, int newResources)
    {
        clientResourcesChanged?.Invoke(newResources);
    }

    public override void OnStartAuthority()
    {

        if (NetworkServer.active)
        {
            return;
        }

        Unit.authorityOnUnitSpawn += authorityHendleSpawnedUnit;
        Unit.authorityOnUnitDespawn += authorityHendleDespawnedUnit;

        Building.authorityOnBuildingSpawn += authorityHendleSpawnedBuild;
        Building.authorityOnBuildingDespawn += authorityHendleDespawnedBuild;

    }


    public override void OnStopClient()
    {

        if (!isClientOnly || !hasAuthority)
        {
            return;
        }

        Unit.authorityOnUnitSpawn -= authorityHendleSpawnedUnit;
        Unit.authorityOnUnitDespawn -= authorityHendleDespawnedUnit;

        Building.authorityOnBuildingSpawn -= authorityHendleSpawnedBuild;
        Building.authorityOnBuildingDespawn -= authorityHendleDespawnedBuild;

    }

    //Units
    private void authorityHendleSpawnedUnit(Unit unit)
    {

        myUnits.Add(unit);

    }

    private void authorityHendleDespawnedUnit(Unit unit)
    {

        myUnits.Remove(unit);

    }

    //Buildings
    private void authorityHendleSpawnedBuild(Building building)
    {

        myBuildings.Add(building);

    }

    private void authorityHendleDespawnedBuild(Building building)
    {

        myBuildings.Remove(building);

    }

    #endregion


}
