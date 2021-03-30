using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ResourcesDisplay : MonoBehaviour
{

    [SerializeField] private TMP_Text resourcesText = null;

    private RealPlayer player = null;

    private void Update()
    {

        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RealPlayer>();

            if (player != null)
            {

                clientHendleResourcesUpdate(player.resources);

                player.clientResourcesChanged += clientHendleResourcesUpdate;

            }
        }

    }

    private void OnDestroy()
    {
        player.clientResourcesChanged -= clientHendleResourcesUpdate;
    }

    private void clientHendleResourcesUpdate(int resources)
    {
        resourcesText.text = "Resources: " + resources.ToString(); 
    }

}
