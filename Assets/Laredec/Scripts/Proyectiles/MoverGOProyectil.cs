using UnityEngine;
using System.Collections;

public class MoverGOProyectil : MonoBehaviour
{
    public float vel; //cantidad de pixeles que se desplaza por segundo
    public Vector2 posFinal; //punto al que se dirige
    public bool actualizarPosinicial = false;

    public float t0;
    public Vector2 pos0;
    public float tEspera; //este es el tiempo en seg de espera entre que la pones y se mueve (aparece transparente como el CR)
    public GameObject meshGO;
    public bool meshInvertidoOk = false;

    public float toleranciaMovimiento = 0.1f;
     
    public Rigidbody2D rigidBody;

    public GameObject GOSeguir;
    private float segundosActualizar=0.01f;
    [HideInInspector]
    public float t0SegundosActualizar;


    void Start()
    {
        //DatosAccionesMultijugador.MensajeNuevoRecibido("Tiene que perseguir a : " + GOSeguir.name);
        //DatosAccionesMultijugador.MensajeNuevoRecibido("La velocidad que lleva es  : " + vel);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (GOSeguir == null) //Si no existe la persona que estabamos atacando, pues destruye la flecha
            Destroy(this.gameObject);

        if (Time.realtimeSinceStartup - t0SegundosActualizar > segundosActualizar)
        {
            t0SegundosActualizar = Time.realtimeSinceStartup;
            if(GOSeguir)
                posFinal = GOSeguir.transform.position;
        }

        posFinal = new Vector2(posFinal.x, posFinal.y);

        if (((Vector2)transform.localPosition - posFinal).magnitude >= toleranciaMovimiento && Time.timeScale > 0)
        {
            Vector2 direccion = (posFinal - pos0).normalized;

            float tiempoTranscurrido = Time.realtimeSinceStartup - (t0 + tEspera);
            float desplazamientoActual = tiempoTranscurrido * vel;
          //  Vector3 posActual = pos0 + direccion * desplazamientoActual;
            float desplazamientoTotal = (posFinal - pos0).magnitude;

            if (meshGO && GOSeguir)
            {
                Vector3 posicionMirar = new Vector3(GOSeguir.transform.position.x, GOSeguir.transform.position.y, meshGO.transform.position.z);
                Debug.Log("ROTANDO PROYECTIL");
                meshGO.transform.LookAt(posicionMirar, Vector3.back);
            }

            if (tiempoTranscurrido > 0)
            {
              
                if (desplazamientoActual < desplazamientoTotal)
                {
                    rigidBody.velocity = vel * direccion;
                }
                else
                {
                    rigidBody.velocity = Vector2.zero;

                }

                if (actualizarPosinicial)
                {
                    t0 = Time.realtimeSinceStartup;
                    pos0 = transform.localPosition;
                    tEspera = 0;
                }
            }
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }

    } //fin del update


}