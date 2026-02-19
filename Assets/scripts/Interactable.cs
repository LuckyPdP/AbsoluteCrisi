using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string Message;

    public UnityEvent onInteraction;


    private void Start()
    {
        this.transform.tag = "Interactable";
    }

    public void Interact()
    {
        Debug.Log("interaction");
        onInteraction.Invoke();
    }


}
