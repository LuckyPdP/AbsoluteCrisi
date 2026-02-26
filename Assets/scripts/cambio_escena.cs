using UnityEngine;
using UnityEngine.SceneManagement;
public class cambio_escena : MonoBehaviour
{
    public string Samplescene;

    void Cambiarjuego()
    {
        SceneManager.LoadScene(Samplescene);
    }

}
