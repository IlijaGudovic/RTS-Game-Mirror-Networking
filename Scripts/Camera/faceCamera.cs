using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceCamera : MonoBehaviour
{

    private Transform mainCamTransfrom = null;


    private void Start()
    {
        mainCamTransfrom = Camera.main.transform;
    }


    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamTransfrom.rotation * Vector3.forward, mainCamTransfrom.rotation * Vector3.up);
    }

}
