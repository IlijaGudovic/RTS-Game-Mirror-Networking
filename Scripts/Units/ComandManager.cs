using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComandManager : MonoBehaviour
{


    [SerializeField] private UnitSelectionManager selectScript = null;

    private Camera mainCamera;

    [SerializeField] private LayerMask layerMask = new LayerMask();

    private void Start()
    {

        selectScript.selectedUnits.Clear();

        mainCamera = Camera.main;

        GameOverHandler.ClientOnGameover += clientHandleGameover;

    }

    private void OnDestroy()
    {
        GameOverHandler.ClientOnGameover -= clientHandleGameover;
    }

    public void clientHandleGameover(string neverMind)
    {
        enabled = false;
    }

    private void Update()
    {

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
           
                if (hit.collider.gameObject.TryGetComponent<Targetable>(out Targetable newTarget))
                {

                    if (newTarget.hasAuthority)
                    {

                        tryMove(hit.point);
                        return;

                    }

                    tryTarget(newTarget);
                    return;

                }

                tryMove(hit.point);

            }

        }

    }


    private void tryTarget(Targetable target)
    {

        foreach (Unit unit in selectScript.selectedUnits)
        {

            unit.getTargeterScript().CmdSetTarget(target.gameObject);

        }

    }


    private void tryMove(Vector3 desiredPosition)
    {

        foreach (Unit unit in selectScript.selectedUnits)
        {

            unit.getMovmentScript().CmdMove(desiredPosition);

        }

    }





}
