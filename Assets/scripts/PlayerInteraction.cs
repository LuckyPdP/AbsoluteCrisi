using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float InteractRange = 2;
    Interactable currentInteractable;
    Interactable newInteractable;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void FixedUpdate()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if(Physics.Raycast(ray, out hit, InteractRange))
        {
            if(hit.collider.tag == "Interactable")
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
