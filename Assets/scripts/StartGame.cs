using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public AudioSource sonidoStart;
    public Animator fade;

    public void StartButton()
    {
        sonidoStart.Play();
        fade.SetTrigger("FadeOut");

        Invoke("CargarNivel", 1.5f); // espera 1.5 segundos
    }

    void CargarNivel()
    {
        SceneManager.LoadScene("Nivel1");
    }
}
