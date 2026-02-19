using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovimiento : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
