using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efecto : MonoBehaviour
{
    [HideInInspector]
    public string nombre;
    [HideInInspector]
    public int tipo;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (tipo == 2 && GetComponent<TriggerAccionProyectil>().enviarMensaje_ok == true)
        {
            GetComponent<MoverGOProyectil>().vel = 10f; //para que llegue instantáneamente. No quiero modificar la posición directamente por si da problemas para  la detección del collider
        }
    }


    public void Activar()
    {
        GameManager.Instance.jugadorPropio.GetComponentInChildren<Buffos>().Invoke(nombre,0f);
    }

}
