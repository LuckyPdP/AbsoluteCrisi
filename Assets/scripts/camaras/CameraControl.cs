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

// interaccion dialogo y objetos:
    public float InteractRange = 2;
    Interactable currentInteractable;
    Interactable newInteractable;



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


        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }

    }


    //Mas interaccion con los objetos:

    private void FixedUpdate()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, InteractRange))
        {
            if (hit.collider.tag == "Interactable")
            {
                newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable != null)
                {
                    SetNewCurrenInteractable(newInteractable);
                }

                else
                {
                    DissabelCurrenInteractable();
                }

            }

            else
            {
                DissabelCurrenInteractable();
            }
        }

        else
        {
            DissabelCurrenInteractable();
        }
    }

    void SetNewCurrenInteractable(Interactable newInteractable)
    {


        currentInteractable = newInteractable;
        //UI_Controller.instance.EnableInteractionText(currentInteractable.Message);
        Debug.Log(currentInteractable.Message);

        if (currentInteractable.TryGetComponent<Outline>(out Outline outlineMat))
        {
            outlineMat.OutlineWidth = 3;
        }
    }

    void DissabelCurrenInteractable()
    {
        //UI_Controller.instance.DisableInteractionText();

        if (currentInteractable)
        {
            if (currentInteractable.TryGetComponent<Outline>(out Outline outlineMat))
            {
                outlineMat.OutlineWidth = 0;
            }

            currentInteractable = null;
        }
    }



}
