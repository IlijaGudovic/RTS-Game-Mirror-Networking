using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private Building building = null;
    [SerializeField] private Image icon = null;
    [SerializeField] private TMP_Text priceText = null;

    [SerializeField] private LayerMask groundMask = new LayerMask();

    private Camera mainCamera;
    private RealPlayer playerScript;
    [SerializeField] private GameObject buildignPriview;
    private Renderer buildingRendera;

    private BoxCollider BuildingColider;


   

    private void Start()
    {

        mainCamera = Camera.main;

        icon.sprite = building.getIcon();
        priceText.text = building.getPrice().ToString();
        icon.sprite = building.getIcon();

        BuildingColider = building.GetComponent<BoxCollider>();

    }

    private void Update()
    {

        if (playerScript == null)
        {
            playerScript = NetworkClient.connection.identity.GetComponent<RealPlayer>();
        }

        if (buildignPriview == null)
        {
            return;
        }

        updatePrieviewInstance();

    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (playerScript.resources < building.getPrice())
        {
            return;
        }

        buildignPriview = Instantiate(building.getBuildingPreview());

        buildingRendera = buildignPriview.GetComponentInChildren<Renderer>();

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (buildignPriview == null)
        {
            return;
        }


        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray , out RaycastHit hit , Mathf.Infinity, groundMask))
        {

            //Place Building

            playerScript.tryPlaceBuild(building.getId(), hit.point);

        }

        Destroy(buildignPriview);

    }


    private void updatePrieviewInstance()
    {

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {

            buildignPriview.transform.position = hit.point;

            if (buildignPriview.activeSelf != true)
            {
                buildignPriview.SetActive(true);
            }


            //Color

            Color newColor = playerScript.CanPlaceBuildingBool(BuildingColider, hit.point) ? Color.green : Color.red;

            buildingRendera.material.color = newColor; 

        }

    }


}
