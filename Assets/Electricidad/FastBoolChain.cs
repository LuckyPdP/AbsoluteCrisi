using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class FastBoolChain : MonoBehaviour
{
    [Header("Estado")]
    public bool activo = false;

    [Header("Delay Activación")]
    public float delayActivacion = 0.5f;

    [Header("Eventos")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    private HashSet<FastBoolChain> vecinosActivos = new HashSet<FastBoolChain>();
    private Coroutine rutinaActivacion;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        ActualizarVisual();
    }

    public void NotificarCambioEstado(FastBoolChain vecino, bool estado)
    {
        if (estado)
            vecinosActivos.Add(vecino);
        else
            vecinosActivos.Remove(vecino);

        EvaluarEstado();
    }

    void EvaluarEstado()
    {
        bool hayConexion = vecinosActivos.Count > 0;

        if (hayConexion && !activo)
        {
            // Iniciar delay si no está ya en proceso
            if (rutinaActivacion == null)
                rutinaActivacion = StartCoroutine(ActivacionConDelay());
        }
        else if (!hayConexion)
        {
            // Cancelar activación pendiente
            if (rutinaActivacion != null)
            {
                StopCoroutine(rutinaActivacion);
                rutinaActivacion = null;
            }

            // Si estaba activo, desactivar inmediatamente
            if (activo)
                Desactivar();
        }
    }

    IEnumerator ActivacionConDelay()
    {
        yield return new WaitForSeconds(delayActivacion);

        // Confirmar que sigue habiendo conexión
        if (vecinosActivos.Count > 0)
        {
            activo = true;
            OnActivated?.Invoke();
            ActualizarVisual();
        }

        rutinaActivacion = null;
    }

    void Desactivar()
    {
        activo = false;
        OnDeactivated?.Invoke();
        ActualizarVisual();
    }

    void ActualizarVisual()
    {
        if (rend != null)
            rend.material.color = activo ? Color.green : Color.red;
    }
}