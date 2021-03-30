using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class UnitSelectionManager : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask = new LayerMask();

    [SerializeField] private RectTransform selectionAreBox = null;
    private Vector2 startMousePosition;

    private RealPlayer player;

    private Camera mainCamera;

    public List<Unit> selectedUnits { get; } = new List<Unit>();


    public void clientHandleGameover(string neverMind)
    {
        enabled = false;
    }

    private void Start()
    {

        mainCamera = Camera.main;

        Unit.authorityOnUnitDespawn += authorityHandleUnitDespawn;

        GameOverHandler.ClientOnGameover += clientHandleGameover;

    }

    private void OnDestroy()
    {

        Unit.authorityOnUnitDespawn -= authorityHandleUnitDespawn;

        GameOverHandler.ClientOnGameover -= clientHandleGameover;

    }

    private void authorityHandleUnitDespawn(Unit unit)
    {
        selectedUnits.Remove(unit);
    }

    private void Update()
    {

        if (player ==  null)
        {
            player = NetworkClient.connection.identity.GetComponent<RealPlayer>();
        }


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {

            startSelectedArea();

        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {

            CleareSelectionArea();

        }
        else if (Mouse.current.leftButton.isPressed)
        {

            updateSelectedArea();

        }

    }


    private void startSelectedArea()
    {

        startMousePosition = Mouse.current.position.ReadValue();

        //selectionAreBox.gameObject.SetActive(true);

        selectionAreBox.sizeDelta = new Vector2(0, 0);

        if (Keyboard.current.shiftKey.isPressed)
        {
            return;
        }

        foreach (Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Deselect();
        }

        selectedUnits.Clear();

    }


    private void updateSelectedArea()
    {

        if (!selectionAreBox.gameObject.activeInHierarchy)
        {
            if (selectionAreBox.sizeDelta.magnitude > 0.5)
            {
                selectionAreBox.gameObject.SetActive(true);
            }
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float widht = startMousePosition.x - mousePosition.x;
        float height = startMousePosition.y - mousePosition.y;


        selectionAreBox.sizeDelta = new Vector2(Mathf.Abs(widht), Mathf.Abs(height));

        selectionAreBox.anchoredPosition = startMousePosition - new Vector2(widht / 2, height / 2);


    }


    private void CleareSelectionArea()
    {

        selectionAreBox.gameObject.SetActive(false);

        if (selectionAreBox.sizeDelta.magnitude < 0.5)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {

                if (hit.collider.TryGetComponent<Unit>(out Unit unit))
                {

                    if (unit.hasAuthority)
                    {

                        if (!selectedUnits.Contains(unit))
                        {
                            selectedUnits.Add(unit); 
                        }
                        
                    }

                    foreach (Unit selectedUnit in selectedUnits)
                    {
                        selectedUnit.Select();
                    }

                }
                else
                {

                    foreach (Unit selectedUnit in selectedUnits)
                    {
                        selectedUnit.Deselect();
                    }

                    selectedUnits.Clear();

                }

            }

            return;

        }

        Vector2 minRange = selectionAreBox.anchoredPosition - (selectionAreBox.sizeDelta / 2);
        Vector2 maxRange = selectionAreBox.anchoredPosition + (selectionAreBox.sizeDelta / 2);

        foreach (Unit unit in player.getMyUnits())
        {

            if (selectedUnits.Contains(unit))
            {
                continue;
            }

            Vector3 unitScreanPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (unitScreanPosition.x > minRange.x && unitScreanPosition.x < maxRange.x)
            {

                if (unitScreanPosition.y > minRange.y && unitScreanPosition.y < maxRange.y)
                {

                    selectedUnits.Add(unit);
                    unit.Select();

                }

            }

        }


    }

}
