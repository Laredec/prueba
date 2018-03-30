using UnityEngine;
using System.Collections;

public class MoverGO : MonoBehaviour
{
    public Animator animator;
    public CircleCollider2D collider;
    public Vector2 posFinal; //punto al que se dirige
    public bool actualizarPosinicial=false;

    public float t0;
    public Vector2 pos0;
    public float tEspera; //este es el tiempo en seg de espera entre que la pones y se mueve (aparece transparente como el CR)
    public float toleranciaMovimiento = 0.01f;
    public float tiempoToleranciaAnimacion = 0.1f;

    public GameObject referenciaGO;
     
    public float wLimitantes = 14;
    public float hLimitantes = 26;

    private float ultimoTiempoMoviendose;

    public float ultimoTiempoUpdate;
    public float frecuenciaUpdate = 1.0f / 60;

    public Rigidbody2D rigidBody;

    private float t0EnviarMov;
    private float tEsperaEnvioMov = 0.01f;

    [HideInInspector]
    public Vector2 direccion;

    // Use this for initialization
    void Start ()
    {
        direccion = Vector2.down;

        t0 = Time.realtimeSinceStartup;
        pos0 = transform.position;
        posFinal = new Vector2(transform.position.x, transform.position.y);

        if(name!="0" && name != "0Enemigo")
         InvokeRepeating("EnviarMovimientoRival", tEsperaEnvioMov, tEsperaEnvioMov);
    }


    // Update is called once per frame
    void Update()
    {
        bool stunOk = false;
        if (GetComponentInChildren<Atacar>().stats)
         stunOk = GetComponentInChildren<Atacar>().stats.stun_ok;

        if (GameManager.gameEndedOk) //para que dejen de moverse al acabar la partida
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.Sleep();
            this.enabled = false;
            animator.SetBool("moviendoseOk", false);
        }

        rigidBody.angularVelocity = 0;

        ultimoTiempoUpdate = Time.realtimeSinceStartup - (Time.realtimeSinceStartup - ultimoTiempoUpdate) % frecuenciaUpdate + frecuenciaUpdate; //este calculo lo hago así por si la diferencia entre tActual y tUpdate es demasiado grande (cogemos el tUpdate mas proximo sin pasarse al tiempo actual y sumamos la frecuencia)

        if (((Vector2)pos0 - posFinal).magnitude >= toleranciaMovimiento && !GameManager.gameEndedOk  &&  !stunOk)
        {
            direccion = (posFinal - pos0).normalized;
            float tiempoTranscurrido = Time.realtimeSinceStartup - (t0 + tEspera);

            float desplazamientoActual = tiempoTranscurrido * (GetComponentInChildren<Atacar>().stats.movSpeed + GetComponentInChildren<Atacar>().stats.ComprobarHabilidadesAumentoMovSpeed());
            Vector3 posActual = pos0 + direccion * desplazamientoActual;
            float desplazamientoTotal = (posFinal - pos0).magnitude;

            

            if (tiempoTranscurrido > 0)
            {
                ultimoTiempoMoviendose = Time.realtimeSinceStartup;

                if (!animator.GetBool("moviendoseOk"))
                    animator.SetBool("moviendoseOk", true);

                
                if (desplazamientoActual < desplazamientoTotal)
                {

                    if (referenciaGO  /*&&  gameObject != GameManager.Instance.jugadorPropio*/) //rotamos
                    {
                        Debug.Log("ROTAMOS MOVERGO");
                        referenciaGO.transform.LookAt(transform.position + new Vector3(direccion.x, direccion.y, 0), Vector3.back);
                    }

                    rigidBody.velocity = (GetComponentInChildren<Atacar>().stats.movSpeed+ GetComponentInChildren<Atacar>().stats.ComprobarHabilidadesAumentoMovSpeed()) * direccion;
                }
                else
                {
                    
                    transform.position = new Vector3(posFinal.x, posFinal.y, transform.position.z);
                    pos0 = transform.position;

                    rigidBody.velocity = Vector2.zero;
                }


                if (actualizarPosinicial)
                {
                    t0 = Time.realtimeSinceStartup;
                    pos0 = transform.position;
                    tEspera = 0;
                }
            }

        }
        else
        {
            Debug.Log("entra en el else");
            rigidBody.velocity = Vector2.zero;     

            transform.position = new Vector3(posFinal.x, posFinal.y, transform.position.z);
            pos0 = transform.position;

            if (Time.realtimeSinceStartup - ultimoTiempoMoviendose >= tiempoToleranciaAnimacion  ||  stunOk)
            {
                animator.SetBool("moviendoseOk", false);
            }
        }
       
            if (transform.position.x < -wLimitantes / 2 + collider.radius)
            {
                transform.position = new Vector3(-wLimitantes / 2 + collider.radius, transform.position.y, transform.position.z);
                posFinal = transform.position;
            }
            if (transform.position.x > wLimitantes / 2 - collider.radius)
            {
                transform.position = new Vector3(wLimitantes / 2 - collider.radius, transform.position.y, transform.position.z);
                posFinal = transform.position;
            }
            if (transform.position.y < -hLimitantes / 2 + collider.radius)
            {
                transform.position = new Vector3(transform.position.x, -hLimitantes / 2 + collider.radius, transform.position.z);
                posFinal = transform.position;
            }
            if (transform.position.y > hLimitantes / 2 - collider.radius)
            {
                transform.position = new Vector3(transform.position.x, hLimitantes / 2 - collider.radius, transform.position.z);
                posFinal = transform.position;
            }



    } //fin del update



    void EnviarMovimientoRival()
    {
    if (GetComponentInChildren<TriggerAtaque>() && !GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok && !GetComponentInChildren<TriggerAtaque>().soyTower_ok)
        if (GameManager.Instance != null)
        {
            string strPacket = "";
            if (GetComponentInChildren<TriggerAreaVision>() && GetComponentInChildren<TriggerAreaVision>().objetivo != null && GetComponentInChildren<TriggerAreaVision>().objetivo.GetComponentInChildren<Atacar>() && !GetComponentInChildren<TriggerAreaVision>().objetivo.GetComponentInChildren<Atacar>().muertoOk)
            {
                string enemigoName = GetComponentInChildren<TriggerAreaVision>().objetivo.name;
                int index = enemigoName.IndexOf("E");
                if (index > -1)
                    enemigoName = enemigoName.Substring(0, index);

                strPacket = "J" + name + "/" + transform.position.x + "|" + transform.position.y + "@" + transform.position.z + "&" + enemigoName;
            }
            else
            {
                strPacket = "J" + name + "/" + transform.position.x + "|" + transform.position.y + "@" + transform.position.z + "&none";
            }
            GameManager.Instance.EnviarPaquete(strPacket, false);
        }
    }


}
