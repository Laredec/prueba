using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelarBusqueda : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        
        
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void Cancelar()
    {
        Debug.Log("Cerrando sala en CancelarBusqueda");
        if (GameSparksManager.Instance())
            GameSparksManager.Instance().CerrarSala();
        ChangeScene.LoadScene("MenuPrincipal");
    }



}

