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

    // Para evitar disparar OnActivated/OnDeactivated más de una vez
    private bool estadoAnterior = false;

    //   Unity Lifecycle  

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

        // Cada nodo arranca su propio loop de comprobación
        StartCoroutine(LoopComprobacion());
    }

    //   Loop continuo  

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
            // Iniciar activación con delay si no hay una ya en marcha
            if (rutinaActivacion == null)
                rutinaActivacion = StartCoroutine(ActivacionConDelay());
        }
        else if (!debeEstarActivo && activo)
        {
            // Apagar inmediatamente
            CancelarActivacionYApagar();
        }
    }

    //   Triggers (solo registran vecinos, no propagan)  

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

        // Al separar físicamente sí forzamos evaluación inmediata
        if (activo) CancelarActivacionYApagar();
    }

    //   Activación / Desactivación  

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

        // Actualizar estado y visual primero
        activo = true;
        ActualizarVisual();
        rutinaActivacion = null;

        // Si es el destino, esperar a que el resto del camino también esté activo
        if (esDestino)
        {
            // Timeout de seguridad para evitar espera infinita
            float tiempoEspera = 0f;
            float timeout = delayActivacion * 10f + 2f;

            while (!TodosLosNodosDelCaminoActivos())
            {
                tiempoEspera += Time.deltaTime;
                if (tiempoEspera >= timeout) break; // evitar loop infinito
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

    //   BFS auxiliares  

    /// <summary>
    /// Recorre el grafo desde este nodo hacia atrás buscando una fuente.
    /// No depende de que los vecinos estén "activos", solo de que estén conectados.
    /// </summary>
    bool EsAlcanzableDesdefuente()
    {
        // Si este nodo no está snapeado (y no es fuente/destino fijo), no puede conducir
        var drag = GetComponent<DragAndSnapMulti>();
        if (drag != null && drag.currentTarget == null) return false;

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
                    // Cada vecino también debe estar snapeado para conducir
                    var vDrag = v.GetComponent<DragAndSnapMulti>();
                    if (vDrag != null && vDrag.currentTarget == null) continue;

                    visitados.Add(v);
                    cola.Enqueue(v);
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Para el destino final: comprueba que todos los nodos alcanzables
    /// desde la fuente ya tienen activo = true (visual actualizado).
    /// </summary>
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

    //   API Pública  

    public void NotificarMovimiento()
    {
        // Ya no es necesario llamar PropagateFromSources,
        // el loop se encarga. Pero se puede forzar una evaluación inmediata:
        if (!esFuente) EvaluarConexion();
    }

    

}