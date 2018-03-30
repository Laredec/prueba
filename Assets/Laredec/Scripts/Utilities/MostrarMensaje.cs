using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarMensaje : MonoBehaviour {

    public Text mensajeGO;
    static private Text text;

	// Use this for initialization
	void Start ()
    {
        text = mensajeGO;
        text.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    static public void Mostrar(string mensaje)
    {
        if (text)
        {
            text.enabled = true;
            text.text = mensaje;
        }
    }


}
