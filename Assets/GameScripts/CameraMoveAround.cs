using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveAround : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 120f;

    void Update()

    {
        Vector3 pos = transform.position;

     /*   if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }*/

        if (Input.GetKey("d")|| Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)  || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

       // float scroll = Input.GetAxis("Mouse ScrollWheel");
       // Camera.main.orthographicSize -= scroll * scrollSpeed * 100f * Time.deltaTime; // Camera.main.orthographicSize

        // pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        // pos.y = Mathf.Clamp(pos.y, minY, maxY);
       // pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;

    }
}
