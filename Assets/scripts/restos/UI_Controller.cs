using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller instance;

    private void Awake()
    {
            instance = this;
    }

    [SerializeField] TMP_Text interactionText;

    public void EnableInteractionText(string message)
    {
        interactionText.text = message + " (E)";
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
