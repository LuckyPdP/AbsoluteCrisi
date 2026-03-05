using UnityEngine;
using UnityEngine.SceneManagement;
public class cambio_escena : MonoBehaviour
{
    public string Samplescene;

    public void CambiarEscena()
    {
        SceneManager.LoadScene(Samplescene);
    }

}
