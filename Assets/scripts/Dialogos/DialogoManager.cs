using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogoManager : MonoBehaviour
{
    // public AudioSource Voces;

    [Header("Control del Jugador")]
    public MonoBehaviour controladorJugador;
    public MonoBehaviour Cameramovement;


    [System.Serializable]
    public class Decision
    {
        public string textoBoton;
        public int indiceDestino;
    }

    [System.Serializable]
    public class Frase
    {
        [TextArea] public string texto;
        public Sprite retrato;
        public Decision[] opciones;
    }


    public Image personajeImagen;

    public TextMeshProUGUI DialogueText;
    public GameObject panelBotones;
    public Button[] botonesUI;
    public Frase[] conversacion;

    private int index = 0;
    public float dialogueSpeed;
    private bool escribiendo = false;

    void Start()
    {
        panelBotones.SetActive(false);
        MostrarFrase();
    }

    public void IniciarDialogo()
    {
        panelDialogo.SetActive(true);

        if (controladorJugador != null)
            controladorJugador.enabled = false;

        if (Cameramovement != null)
            Cameramovement.enabled = false;

        index = 0;
        MostrarFrase();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (escribiendo)
            {
                StopAllCoroutines();
                DialogueText.text = conversacion[index].texto;
                escribiendo = false;

                if (conversacion[index].opciones.Length > 0)
                    MostrarOpciones();
            }
            else if (conversacion[index].opciones.Length == 0)
            {
                SiguienteFrase();
            }
        }
    }

    


    public GameObject panelDialogo;

    public void SiguienteFrase()
    {
        if (index < conversacion.Length - 1)
        {
            index++;
            MostrarFrase();
        }
        else
        {
            FinalizarDialogo();
        }
    }

    void FinalizarDialogo()
    {
        panelDialogo.SetActive(false);

        if (controladorJugador != null)
            controladorJugador.enabled = true;

        if (Cameramovement != null)
            Cameramovement.enabled = true;

        index = 0;
    }


    void MostrarFrase()
    {
        StopAllCoroutines();

        if (personajeImagen != null)
        {
            personajeImagen.sprite = conversacion[index].retrato;
            personajeImagen.gameObject.SetActive(conversacion[index].retrato != null);
        }

        StartCoroutine(EscribirFrase());
    }

    IEnumerator EscribirFrase()
    {
        //  Voces.Play();
        escribiendo = true;
        DialogueText.text = "";
        panelBotones.SetActive(false);

        foreach (char c in conversacion[index].texto.ToCharArray())
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }

        escribiendo = false;
        if (conversacion[index].opciones.Length > 0)
        {
            MostrarOpciones();
        }
    }

    void MostrarOpciones()
    {
        panelBotones.SetActive(true);
        for (int i = 0; i < botonesUI.Length; i++)
        {
            if (i < conversacion[index].opciones.Length)
            {
                botonesUI[i].gameObject.SetActive(true);
                Decision decision = conversacion[index].opciones[i];
                botonesUI[i].GetComponentInChildren<TextMeshProUGUI>().text = decision.textoBoton;

                botonesUI[i].onClick.RemoveAllListeners();
                botonesUI[i].onClick.AddListener(() => SeleccionarOpcion(decision.indiceDestino));
            }
            else
            {
                botonesUI[i].gameObject.SetActive(false);
            }
        }
    }

    public void SeleccionarOpcion(int destino)
    {
        index = destino;
        MostrarFrase();
    }
}