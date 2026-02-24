using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    [Header("Movimientos")]
    public float speed;
    public Rigidbody rb;
    float inputHorizontal;
    float inputVertical;
    Vector3 movedir;


    [Header("Saltos")]
    public float fuerzaSalto;
    public Transform OrigenPruebaSuelo;
    public float RadioEsferaSuelo;
    [SerializeField] bool tocandoSuelo;
    public LayerMask EsSuelo;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        comprobarSuelo();

        MyInputs();
        speedlimi();


      //  if (Input.GetKeyDown(KeyCode.Space))
      //  {
      //      jump();
//
      //  }



    }


    void comprobarSuelo()
    {
        tocandoSuelo = Physics.CheckSphere(OrigenPruebaSuelo.position, RadioEsferaSuelo, EsSuelo);
    }


    void speedlimi ()
    {
        Vector3 FlatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (FlatVelocity.magnitude > speed ) 
        {
            Vector3 limitVelocity = FlatVelocity.normalized * speed;
            rb.linearVelocity = new Vector3(limitVelocity.x, rb.linearVelocity.y, limitVelocity.z);
        
        }

    }

    private void jump()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);

    }

    private void FixedUpdate()
    {

        movePlayer();

    }

    void MyInputs()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        if (tocandoSuelo == true && Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        } 

    }

    void movePlayer()
    {
        // Leer entrada del teclado
        float moverX = Input.GetAxis("Horizontal"); // A/D o flechas 
        float moverZ = Input.GetAxis("Vertical");   // W/S o flechas 

        // Crear vector de movimiento
        Vector3 movimiento = new Vector3(moverX, 0f, moverZ);

        // Mover el objeto
        //transform.Translate(movimiento * speed * Time.deltaTime);


        rb.AddForce(movedir.normalized * speed, ForceMode.Force);
        movedir = transform.right * inputHorizontal + transform.forward * inputVertical;

        rb.linearVelocity = movedir.normalized * speed + transform.up * rb.linearVelocity.y;


        //new Vector3(inputHorizontal, 0, inputVertical);

    }

}
