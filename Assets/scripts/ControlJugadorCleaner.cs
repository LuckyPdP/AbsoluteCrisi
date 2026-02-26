using UnityEngine;

public class ControlJugadorCleaner : MonoBehaviour
{
    [Header("Movimientos")]
    public float speed = 6f;
    private Rigidbody rb;
    float inputHorizontal;
    float inputVertical;
    Vector3 moveDir;

    [Header("Saltos")]
    public float fuerzaSalto = 7f;
    public Transform OrigenPruebaSuelo;
    public float RadioEsferaSuelo = 0.2f;
    [SerializeField] bool tocandoSuelo;
    public LayerMask EsSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true; // Evita que se caiga
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        comprobarSuelo();
        MyInputs();

        if (tocandoSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        SpeedLimit();
    }

    void comprobarSuelo()
    {
        tocandoSuelo = Physics.CheckSphere(
            OrigenPruebaSuelo.position,
            RadioEsferaSuelo,
            EsSuelo
        );
    }

    void MyInputs()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        moveDir = transform.right * inputHorizontal + transform.forward * inputVertical;
    }

    void MovePlayer()
    {
        Vector3 velocity = moveDir.normalized * speed;
        velocity.y = rb.linearVelocity.y; // mantiene gravedad natural

        rb.linearVelocity = velocity;
    }

    void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limited = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }
}