using UnityEngine;

public class ColliderRelay : MonoBehaviour
{
    //Este hay que ponerlo en los hijos
    private FastBoolChain padre;

    void Awake()
    {
        padre = GetComponentInParent<FastBoolChain>();
    }

    void OnTriggerEnter(Collider other)
    {
        FastBoolChain otro = other.GetComponentInParent<FastBoolChain>();

        if (otro != null && otro != padre)
        {
            padre.NotificarCambioEstado(otro, otro.activo);
            otro.OnActivated.AddListener(() => padre.NotificarCambioEstado(otro, true));
            otro.OnDeactivated.AddListener(() => padre.NotificarCambioEstado(otro, false));
        }
    }

    void OnTriggerExit(Collider other)
    {
        FastBoolChain otro = other.GetComponentInParent<FastBoolChain>();

        if (otro != null && otro != padre)
        {
            padre.NotificarCambioEstado(otro, false);
        }
    }
}