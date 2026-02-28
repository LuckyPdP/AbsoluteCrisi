using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuPausa2D : MonoBehaviour
{

    public GameObject MenuDePausa;
    public string Samplescene;
    public string MenudeInicio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
    }


    private bool juegoPausado = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausa();

            }


        }


    }

    public void Pausa()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        MenuDePausa.SetActive(true);
    //    Cursor.visible = true;
   //     Cursor.visible = true;
    //    Cursor.lockState = CursorLockMode.None;

    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        MenuDePausa.SetActive(false);
     //   Cursor.lockState = CursorLockMode.None;
     //   Cursor.lockState = CursorLockMode.Locked;
     //   Cursor.visible = false;
        juegoPausado = false;

    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Samplescene);
    }

    public void MenuDeInicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenudeInicio);
    }

    public void quit()
    {

        Application.Quit();

    }
}
