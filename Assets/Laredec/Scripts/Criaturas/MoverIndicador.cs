using UnityEngine;
using System.Collections;

public class MoverIndicador : MonoBehaviour
{
    static public Vector3 vel; //son static para que sean comun a todos 
    static public float duracion = 2;
    private float t0;

	void Awake ()
    {
        vel = new Vector3(0, 0.02f, 0);
        duracion = 1;
        Destroy(gameObject, duracion); //destruimos el objeto en duracion segundos
    }

    void FixedUpdate ()
    {
        gameObject.transform.position += vel; //movemos el objeto segun la velocidad (60 veces por segundo)
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
	}
}
