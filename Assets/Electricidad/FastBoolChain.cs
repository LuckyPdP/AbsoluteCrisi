using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class FastBoolChain : MonoBehaviour
{
    [Header("Estado")]
    public bool activo = false;

    [Header("Fuente independiente (no necesita vecino)")]
    public bool esFuente = false;

    [Header("Delay Activación")]
    public float delayActivacion = 0.5f;

    [Header("Eventos")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    private HashSet<FastBoolChain> vecinosEnRango = new HashSet<FastBoolChain>();
    private Coroutine rutinaActivacion;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (esFuente) Activar();
        ActualizarVisual();
    }

    void OnTriggerEnter(Collider other)
    {
        FastBoolChain vecino = other.GetComponent<FastBoolChain>();
        if (vecino == null) return;

        vecinosEnRango.Add(vecino);
        vecino.vecinosEnRango.Add(this); // Registro mutuo

        EvaluarEstado();
        vecino.EvaluarEstado(); // El vecino también evalúa
    }

    void OnTriggerExit(Collider other)
    {
        FastBoolChain vecino = other.GetComponent<FastBoolChain>();
        if (vecino == null) return;

        vecinosEnRango.Remove(vecino);
        vecino.vecinosEnRango.Remove(this); // Limpieza mutua

        EvaluarEstado();
        vecino.EvaluarEstado(); // El vecino también reevalúa
    }

    public void EvaluarEstado()
    {
        if (esFuente) return;

        bool hayVecinoActivo = HayVecinoActivo();

        if (hayVecinoActivo && !activo)
        {
            if (rutinaActivacion == null)
                rutinaActivacion = StartCoroutine(ActivacionConDelay());
        }
        else if (!hayVecinoActivo)
        {
            if (rutinaActivacion != null)
            {
                StopCoroutine(rutinaActivacion);
                rutinaActivacion = null;
            }

            if (activo) Desactivar();
        }
    }

    bool HayVecinoActivo()
    {
        foreach (var v in vecinosEnRango)
            if (v != null && v.activo) return true;
        return false;
    }

    IEnumerator ActivacionConDelay()
    {
        yield return new WaitForSeconds(delayActivacion);

        if (HayVecinoActivo() && !activo)
            Activar();

        rutinaActivacion = null;
    }

    void Activar()
    {
        activo = true;
        ActualizarVisual();
        OnActivated?.Invoke();

        // Notificar a vecinos en rango que este nodo se activó
        foreach (var v in vecinosEnRango)
            if (v != null && !v.activo && !v.esFuente)
                v.EvaluarEstado();
    }

    void Desactivar()
    {
        activo = false;
        ActualizarVisual();
        OnDeactivated?.Invoke();

        // Notificar a vecinos que este nodo se desactivó
        foreach (var v in vecinosEnRango)
            if (v != null && !v.esFuente)
                v.EvaluarEstado();
    }

    void ActualizarVisual()
    {
        if (rend != null)
            rend.material.color = activo ? Color.green : Color.red;
    }
}