using UnityEngine;
using UnityEngine.SceneManagement;
public class cambio_escena : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Cambiarjuego()
    {
        SceneManager.LoadScene("Casa y pasillo");
    }

     


}
