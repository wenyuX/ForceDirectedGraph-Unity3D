using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivityMouse = 500f;
    public float sensitivetyKeyBoard = 0.1f;
    public float sensitivetyMouseWheel = 10f;
    public Vector3 objPos = Vector3.zero;
    public Vector3 offsetPos;

    private void Start()
    {
        transform.position = new Vector3(0,0,-50);
        offsetPos = transform.position - objPos;
    }

    void Update()
    {
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            this.GetComponent<Camera>().fieldOfView = this.GetComponent<Camera>().fieldOfView - Input.GetAxis("Mouse ScrollWheel") * sensitivetyMouseWheel;
        }
        
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(objPos,Vector3.up, Input.GetAxis("Mouse X") * sensitivityMouse);
            transform.RotateAround(objPos, transform.right, Input.GetAxis("Mouse Y") * sensitivityMouse);
        }
        
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * sensitivetyKeyBoard, 0, 0);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(0, Input.GetAxis("Vertical") * sensitivetyKeyBoard, 0);
        }
    }
    
}