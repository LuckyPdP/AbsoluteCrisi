using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class boton : MonoBehaviour
{
    public string Samplescene;

    public void Empezar()
    {
        SceneManager.LoadScene(Samplescene);
    }

    public void salir()
    {
        Application.Quit();

    }
}
