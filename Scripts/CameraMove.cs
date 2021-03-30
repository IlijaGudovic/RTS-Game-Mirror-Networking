using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private bool doMovment = true; // lock screan

    public float panSpeed = 30f;

    public float panBorderFicknes = 10f;

    public float scrollSpeed = 5f;

    [Header("Camera Limits")]
    private Vector3 spawnPosition;
    public Vector2 zoomRange;
    public Vector2 horizontalRange;
    public Vector2 verticalRange;


    private void Start()
    {
        spawnPosition = transform.position;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {

            doMovment = !doMovment;

        }


        if (!doMovment)
        {

            return;

        }


        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderFicknes)
        {

            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);

        }


        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderFicknes)
        {

            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);

        }


        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderFicknes)
        {

            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);

        }


        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderFicknes)
        {

            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);



        }


        float scrolle = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        pos.y -= scrollSpeed * scrolle * 1000 * Time.deltaTime;

        //Clamp positions
        pos.y = Mathf.Clamp(pos.y, spawnPosition.y - zoomRange.x, spawnPosition.y + zoomRange.y); //zoom

        pos.z = Mathf.Clamp(pos.z, spawnPosition.z - verticalRange.x, spawnPosition.z + verticalRange.y); //vertical

        pos.x = Mathf.Clamp(pos.x, spawnPosition.x - horizontalRange.x, spawnPosition.x + horizontalRange.y); //Horizontal

        transform.position = pos;


    }
}
