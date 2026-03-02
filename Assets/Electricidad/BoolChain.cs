using UnityEngine;
using UnityEngine.Events;

public class BoolChainEvent : MonoBehaviour
{
    [Header("Estado")]
    public bool activo = false;

    [Header("Eventos")]
    public UnityEvent OnActivated;

    private int contactosActivos = 0;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        ActualizarVisual();
    }

    private void OnTriggerEnter(Collider other)
    {
        BoolChainEvent otro = other.GetComponent<BoolChainEvent>();

        if (otro != null && otro.activo)
        {
            contactosActivos++;
            Activar();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BoolChainEvent otro = other.GetComponent<BoolChainEvent>();

        if (otro != null && otro.activo)
        {
            contactosActivos--;
            if (contactosActivos <= 0)
            {
                Desactivar();
            }
        }
    }

    void Activar()
    {
        if (!activo)
        {
            activo = true;
            OnActivated?.Invoke();
            ActualizarVisual();
            Debug.Log(gameObject.name + " ACTIVADO");
        }
    }

    void Desactivar()
    {
        if (activo)
        {
            activo = false;
            ActualizarVisual();
            Debug.Log(gameObject.name + " DESACTIVADO");
        }
    }

    void ActualizarVisual()
    {
        if (rend != null)
        {
            rend.material.color = activo ? Color.green : Color.red;
        }
    }
}