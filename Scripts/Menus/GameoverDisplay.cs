using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameoverDisplay : MonoBehaviour
{

    [SerializeField] private TMP_Text winnerNameText = null;

    [SerializeField] private GameObject gameoverPanel = null;

    private void Start()
    {

        GameOverHandler.ClientOnGameover += clientHandleGameover;

    }


    private void OnDestroy()
    {

        GameOverHandler.ClientOnGameover -= clientHandleGameover;

    }


    public void leaveGame()
    {

        if (NetworkServer.active && NetworkClient.isConnected)
        {

            NetworkManager.singleton.StopHost();

        }
        else
        {

            NetworkManager.singleton.StopClient();

        }

    }


    private void clientHandleGameover(string Winner)
    {

        winnerNameText.text = Winner + " Hes Won!";

        gameoverPanel.gameObject.SetActive(true);

    }

}
