using UnityEngine;

public class SeguirRaton : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float sensibilidad = 0.5f; // Cuánto se mueve la cámara
    public float suavidad = 5f; // Qué tan suave es el movimiento (Lerp)

    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos la posición central de la cámara
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Obtenemos la posición del ratón en la pantalla (de 0 a 1)
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

        // Calculamos la nueva posición objetivo
        Vector3 posicionObjetivo = new Vector3(
            posicionInicial.x + (mouseX * sensibilidad),
            posicionInicial.y + (mouseY * sensibilidad),
            posicionInicial.z
        );

        // Movemos la cámara suavemente hacia el objetivo
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * suavidad);
    }
}