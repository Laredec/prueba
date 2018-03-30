using UnityEngine;
using System.Collections;

public class InputMovimiento : MonoBehaviour
{
    public float distMinMovDedo;
    public float distMaxMovDedo;
    public MoverGO moverGO;

    private Vector2 posTouch0;
    private Vector2 difPosTouch;
    private bool moviendoseOk;
    private Vector2 dir;
    public GameObject joyStick;
    public float distanciaJoystick = 28f;
    private Vector3 pos0Joystick;

    // Use this for initialization
    private void Awake()
    {
        if (!joyStick)
            joyStick = GameObject.FindGameObjectWithTag("JoyStick");
        if (joyStick)
            pos0Joystick = joyStick.transform.localPosition;
    }

    void Start () {
        moviendoseOk = false;

    }

    // Update is called once per frame


    void FixedUpdate()
    {
        
    }

    void OnDrawGizmos()
    {
        Vector2 posTouch0World = Camera.main.ScreenToWorldPoint(posTouch0);
        Vector2 difPosTouchWorld = Camera.main.ScreenToWorldPoint(difPosTouch);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere (posTouch0, 2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(posTouch0 + difPosTouch, 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(posTouch0, posTouch0 + difPosTouch);

    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            moverGO.t0 = Time.realtimeSinceStartup;
            moverGO.pos0 = transform.position;
            posTouch0 = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            difPosTouch = (Vector2)Input.mousePosition - posTouch0;

            if (difPosTouch.magnitude > distMaxMovDedo)
            {
                posTouch0 += difPosTouch.normalized * (difPosTouch.magnitude - distMaxMovDedo); //si te pasas del radio maximo reajustamos la posicion virtual del joistick
                difPosTouch = (Vector2)Input.mousePosition - posTouch0; //volvemos a calcular el vector direccion
            }


            if (difPosTouch.magnitude > distMinMovDedo)
            {
                moviendoseOk = true;
                dir = (GetComponentInChildren<Atacar>().stats.movSpeed + GetComponentInChildren<Atacar>().stats.ComprobarHabilidadesAumentoMovSpeed()) * Time.deltaTime * ((Vector2)Input.mousePosition - posTouch0).normalized;
            }
            else
                moviendoseOk = false;

        }

        if (!Input.GetMouseButton(0))
        {
            moviendoseOk = false;
        }




        if (moviendoseOk)
        {
            Debug.Log("dir: " + dir);
            moverGO.posFinal = (Vector2)moverGO.transform.position + dir;

            MoverJoystick(dir);
            if (GameManager.Instance)
                GameManager.Instance.EnviarPosicion(-moverGO.posFinal); //tiene signo negativo ya que el otro jugador tiene las coordenadas invertidas (ambos comienzan en la parte de abajo)
        }
        else
        {
            MoverJoystick(Vector3.zero);

            moverGO.posFinal = transform.position;

        }


    }



    void MoverJoystick(Vector3 dirMov)
    {

       // Debug.Log("Moviendo JoyStick a: " + dir);
        joyStick.transform.localPosition = pos0Joystick + dirMov.normalized * distanciaJoystick;
    }

}
