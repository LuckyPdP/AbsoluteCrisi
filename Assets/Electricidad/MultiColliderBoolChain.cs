using UnityEngine;
using UnityEngine.Events;

public class MultiColliderBoolChain : MonoBehaviour
{
    //Poner en el Padre
    [Header("Estado")]
    public bool activo = false;

    [Header("Eventos")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    private int contactosActivos = 0;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        ActualizarVisual();
    }

    public void RegistrarContacto(bool entrando)
    {
        if (entrando)
            contactosActivos++;
        else
            contactosActivos--;

        contactosActivos = Mathf.Max(0, contactosActivos);

        if (contactosActivos > 0 && !activo)
            Activar();
        else if (contactosActivos == 0 && activo)
            Desactivar();
    }

    void Activar()
    {
        activo = true;
        OnActivated?.Invoke();
        ActualizarVisual();
        Debug.Log(name + " ACTIVADO");
    }

    void Desactivar()
    {
        activo = false;
        OnDeactivated?.Invoke();
        ActualizarVisual();
        Debug.Log(name + " DESACTIVADO");
    }

    void ActualizarVisual()
    {
        if (rend != null)
            rend.material.color = activo ? Color.green : Color.red;
    }
}