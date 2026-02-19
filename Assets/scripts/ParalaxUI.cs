using UnityEngine;

public class ParalaxUI : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    [Tooltip("Cantidad máxima de píxeles que se moverá la imagen.")]
    public float cantidadMovimiento = 50f;
    public float suavidad = 5f;

    [Tooltip("Activa esto si quieres que el fondo se mueva en dirección opuesta al ratón (efecto cámara).")]
    public bool invertirMovimiento = true;

    private RectTransform rectTransform;
    private Vector2 posicionInicial;

    void Start()
    {
        // Obtenemos el componente RectTransform de esta imagen
        rectTransform = GetComponent<RectTransform>();

        // Guardamos su posición anclada inicial
        posicionInicial = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Calculamos la posición del ratón respecto al centro (va de -0.5 a 0.5)
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

        // Determinamos la dirección (normal o invertida)
        float multiplicador = invertirMovimiento ? -1f : 1f;

        // Calculamos a dónde debería ir la imagen
        Vector2 posicionObjetivo = new Vector2(
            posicionInicial.x + (mouseX * cantidadMovimiento * multiplicador),
            posicionInicial.y + (mouseY * cantidadMovimiento * multiplicador)
        );

        // Movemos el RectTransform suavemente
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, posicionObjetivo, Time.deltaTime * suavidad);
    }
}
