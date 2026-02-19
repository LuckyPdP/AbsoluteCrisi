using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
//using UnityEditor.Timeline;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject playerbody;
    private float xrotacion;
    public float sensibilidad = 20;
    private float yrotacion;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;


        transform.localRotation = Quaternion.Euler(xrotacion, 0, 0);
     //   transform.localRotation = Quaternion.Euler(xrotacion, yrotacion, 0);

        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;
        xrotacion -= mouseY;
        //   yrotacion += mouseX;
        xrotacion = Mathf.Clamp(xrotacion, -90, 90);
        playerbody.transform.Rotate(Vector3.up * mouseX);

    }
}
