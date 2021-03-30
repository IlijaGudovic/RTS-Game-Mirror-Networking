using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UnitSpawner : NetworkBehaviour , IPointerClickHandler
{

    [SerializeField] private Unit unitPrefab = null;
    [SerializeField] private Transform spawnPoint = null;

    [SerializeField] private Helth helth = null;

    [Header("Settings:")]
    [SerializeField] private TMP_Text remainingUnitText = null;
    [SerializeField] private Image unitProgressImage = null;
    [SerializeField] private int maxUnitsInQue = 5;
    [SerializeField] private float spawnMoveRange = 7f;
    [SerializeField] private float unitSpawnDuration = 3f;

    [SyncVar(hook = nameof(clientUpdateQue))] private int queuedUnits;
    [SyncVar] private float unitTimer;

    private float unitProgressImageVelocity;

    private void Update()
    {

        if (isServer)
        {
            produceUnits();
        }

        if (isClient)
        {
            updateTimerDispaly();
        }

    }

    #region Server


    public override void OnStartServer()
    {
        helth.serverOnDie += serverHendleDie;
    }

    public override void OnStopServer()
    {
        helth.serverOnDie -= serverHendleDie;
    }

    [Server]
    private void produceUnits()
    {

        if (queuedUnits == 0)
        {
            return;
        }

        unitTimer += Time.deltaTime;

        if (unitTimer >= unitSpawnDuration)
        {

            unitTimer = 0;

            GameObject spawnedUnit = Instantiate(unitPrefab.gameObject, spawnPoint.position, Quaternion.identity);

            NetworkServer.Spawn(spawnedUnit, connectionToClient);

            Vector3 spawnOffset = Random.insideUnitSphere * spawnMoveRange;
            spawnOffset.y = spawnPoint.position.y;

            spawnedUnit.GetComponent<UnitMovment>().ServerMove(spawnedUnit.transform.position + spawnOffset);

            queuedUnits--;

        }

    }

    [Server]
    private void serverHendleDie()
    {
        NetworkServer.Destroy(gameObject);
    }


    [Command]
    private void cmdSpawnUnit()
    {

        if (queuedUnits == maxUnitsInQue)
        {
            return;
        }

        int resources = connectionToClient.identity.GetComponent<RealPlayer>().resources;

        RealPlayer player = connectionToClient.identity.GetComponent<RealPlayer>();

        if (resources < unitPrefab.resourcesCost)
        {
            return;
        }

        player.resources -= unitPrefab.resourcesCost;

        queuedUnits++;

    }

    #endregion



    #region Client

    private void updateTimerDispaly()
    {

        float newProgres = unitTimer / unitSpawnDuration;

        if (newProgres < unitProgressImage.fillAmount)
        {
            unitProgressImage.fillAmount = newProgres;
        }
        else
        {

            unitProgressImage.fillAmount = Mathf.SmoothDamp(unitProgressImage.fillAmount, newProgres,ref unitProgressImageVelocity, 0.1f);

        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (!hasAuthority)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            cmdSpawnUnit();
        }

    }

    private void clientUpdateQue(int oldUnits, int newUnits)
    {

        remainingUnitText.text = newUnits.ToString();

    }

    #endregion







}
