using UnityEngine;
using System.Collections;

public class SeguirPersonaje : MonoBehaviour
{
    public GameObject personaje;
    public float distMin = 3;
    public float distMax = 5;

    private float ultimoTiempoUpdate;
    const float frecuenciaUpdate = 1.0f/60;

    public bool siguiendoOk;

    // Use this for initialization
    void Start ()
    {
        siguiendoOk = true;
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 difPos = personaje.transform.position - transform.position;
        TriggerAreaVision triggerAreaVision = GetComponentInChildren<TriggerAreaVision>();

        if (triggerAreaVision)
            if (siguiendoOk)
                //if (Time.realtimeSinceStartup - ultimoTiempoUpdate > frecuenciaUpdate)
                //{
                  //  ultimoTiempoUpdate = Time.realtimeSinceStartup - (Time.realtimeSinceStartup - ultimoTiempoUpdate) % frecuenciaUpdate + frecuenciaUpdate; //este calculo lo hago así por si la diferencia entre tActual y tUpdate es demasiado grande (cogemos el tUpdate mas proximo sin pasarse al tiempo actual y sumamos la frecuencia)

                    if (difPos.magnitude > distMax)
                    {
                        MoverGO moverGO = GetComponent<MoverGO>();
                        moverGO.posFinal = personaje.transform.position - difPos.normalized * distMin;
                        moverGO.t0 = Time.realtimeSinceStartup;
                    }
                //}

	}
}
