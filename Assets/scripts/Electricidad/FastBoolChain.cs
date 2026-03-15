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

    [Header("Batería central (destino final)")]
    public bool esDestino = false;

    [Header("Ignorar Snap (conduce sin estar snapeado)")]
    public bool ignorarSnap = false; // <--- NUEVO

    [Header("Delay Activación")]
    public float delayActivacion = 0.5f;

    [Header("Intervalo de comprobación (segundos)")]
    public float intervaloCheck = 0.1f;

    [Header("Eventos")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    private HashSet<FastBoolChain> vecinosEnRango = new HashSet<FastBoolChain>();
    private Coroutine rutinaActivacion;
    private Renderer rend;

    private bool estadoAnterior = false;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        ActualizarVisual();
    }

    void Start()
    {
        if (esFuente)
        {
            activo = true;
            ActualizarVisual();
        }

        StartCoroutine(LoopComprobacion());
    }

    IEnumerator LoopComprobacion()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloCheck);

            if (!esFuente)
                EvaluarConexion();
        }
    }

    void EvaluarConexion()
    {
        bool debeEstarActivo = EsAlcanzableDesdefuente();

        if (debeEstarActivo && !activo)
        {
            if (rutinaActivacion == null)
                rutinaActivacion = StartCoroutine(ActivacionConDelay());
        }
        else if (!debeEstarActivo && activo)
        {
            CancelarActivacionYApagar();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        FastBoolChain vecino = other.GetComponent<FastBoolChain>();
        if (vecino == null) return;
        vecinosEnRango.Add(vecino);
        vecino.vecinosEnRango.Add(this);
    }

    void OnTriggerExit(Collider other)
    {
        FastBoolChain vecino = other.GetComponent<FastBoolChain>();
        if (vecino == null) return;
        vecinosEnRango.Remove(vecino);
        vecino.vecinosEnRango.Remove(this);

        if (activo) CancelarActivacionYApagar();
    }

    void CancelarActivacionYApagar()
    {
        if (rutinaActivacion != null)
        {
            StopCoroutine(rutinaActivacion);
            rutinaActivacion = null;
        }
        if (activo) Desactivar();
    }

    IEnumerator ActivacionConDelay()
    {
        yield return new WaitForSeconds(delayActivacion);

        if (!EsAlcanzableDesdefuente())
        {
            rutinaActivacion = null;
            yield break;
        }

        activo = true;
        ActualizarVisual();
        rutinaActivacion = null;

        if (esDestino)
        {
            float tiempoEspera = 0f;
            float timeout = delayActivacion * 10f + 2f;

            while (!TodosLosNodosDelCaminoActivos())
            {
                tiempoEspera += Time.deltaTime;
                if (tiempoEspera >= timeout) break;
                yield return null;
            }
        }

        OnActivated?.Invoke();
    }

    void Activar()
    {
        activo = true;
        ActualizarVisual();
        OnActivated?.Invoke();
    }

    void Desactivar()
    {
        activo = false;
        ActualizarVisual();
        OnDeactivated?.Invoke();
    }

    void ActualizarVisual()
    {
        if (rend != null)
            rend.material.color = activo ? Color.green : Color.red;
    }

    bool EsAlcanzableDesdefuente()
    {
        var drag = GetComponent<DragAndSnapMulti>();
        // Si no ignora snap y no está snapeado, no conduce
        if (drag != null && drag.currentTarget == null && !ignorarSnap) return false;

        HashSet<FastBoolChain> visitados = new HashSet<FastBoolChain>();
        Queue<FastBoolChain> cola = new Queue<FastBoolChain>();
        cola.Enqueue(this);
        visitados.Add(this);

        while (cola.Count > 0)
        {
            var actual = cola.Dequeue();
            if (actual.esFuente) return true;

            foreach (var v in actual.vecinosEnRango)
            {
                if (v != null && !visitados.Contains(v))
                {
                    var vDrag = v.GetComponent<DragAndSnapMulti>();
                    // Igual para cada vecino: si ignora snap, pasa siempre
                    if (vDrag != null && vDrag.currentTarget == null && !v.ignorarSnap) continue;

                    visitados.Add(v);
                    cola.Enqueue(v);
                }
            }
        }
        return false;
    }

    bool TodosLosNodosDelCaminoActivos()
    {
        FastBoolChain[] todos = FindObjectsByType<FastBoolChain>(FindObjectsSortMode.None);

        HashSet<FastBoolChain> alcanzados = new HashSet<FastBoolChain>();
        Queue<FastBoolChain> cola = new Queue<FastBoolChain>();

        foreach (var n in todos)
            if (n.esFuente) { alcanzados.Add(n); cola.Enqueue(n); }

        while (cola.Count > 0)
        {
            var actual = cola.Dequeue();
            foreach (var v in actual.vecinosEnRango)
                if (v != null && !alcanzados.Contains(v))
                { alcanzados.Add(v); cola.Enqueue(v); }
        }

        foreach (var n in alcanzados)
            if (!n.activo) return false;

        return true;
    }

    public void NotificarMovimiento()
    {
        if (!esFuente) EvaluarConexion();
    }
}