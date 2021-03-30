using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CustomNetworkManager : NetworkManager
{

    [SerializeField] private GameObject spawnerPrefab = null;

    [SerializeField] private GameOverHandler gameoverScript = null;



    public override void OnServerAddPlayer(NetworkConnection conn)
    {

        base.OnServerAddPlayer(conn);

        GameObject spawnedUnit = Instantiate(spawnerPrefab, conn.identity.transform.position, Quaternion.identity);

        NetworkServer.Spawn(spawnedUnit, conn);

    }



    public override void OnServerSceneChanged(string sceneName)
    {

        if (SceneManager.GetActiveScene().name.StartsWith("Main"))
        {

            GameOverHandler gameOverHandler = Instantiate(gameoverScript);

            NetworkServer.Spawn(gameOverHandler.gameObject);

        }

    }


    //Custom Host/Join

    [SerializeField] private InputField ipText;

    public void HostGame()
    {

        NetworkManager.singleton.StartHost();

    }

    public void JoinGame()
    {

        string serverAdress = ipText.text;

        NetworkManager.singleton.networkAddress = serverAdress;

        NetworkManager.singleton.StartClient();

    }

  

    



}
