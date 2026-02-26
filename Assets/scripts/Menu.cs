using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    
    public void Jugar()
    {
        SceneManager.LoadScene("Casa y pasillo");
        
    }

    
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
