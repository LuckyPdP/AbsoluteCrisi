using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activar_Puente : MonoBehaviour
{
    private bool Pulsado = false;
    public GameObject Puente;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void pulsarBoton()
    {
        if (Pulsado == false)
        {
            Pulsado = true;
            Puente.SetActive(true);
        }
       
    }
    public void quitarPuerta()
    {
        if (Pulsado == false)
        {
            Pulsado = true;
            Puente.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
