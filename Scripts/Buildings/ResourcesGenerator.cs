using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourcesGenerator : NetworkBehaviour
{

    [SerializeField] private Helth helth = null;

    [SerializeField] private int incomingResources = 10;
    [SerializeField] private float interval = 2;

    private float timer;
    public RealPlayer player = null;

    public override void OnStartServer()
    {
        timer = interval;
        player = connectionToClient.identity.GetComponent<RealPlayer>();

        helth.serverOnDie += serverHendelDie;
        GameOverHandler.ServerOnGameover += serverHendelOnGameOver;

    }

    public override void OnStopServer()
    {

        helth.serverOnDie -= serverHendelDie;
        GameOverHandler.ServerOnGameover -= serverHendelOnGameOver;

    }


    [ServerCallback]
    private void Update()
    {

        timer -= Time.deltaTime;

        if (timer <= 0)
        {

            timer = interval;

            player.resources += incomingResources;

        }

    }

    private void serverHendelDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void serverHendelOnGameOver()
    {
        enabled = false;
    }

}
