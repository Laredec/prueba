using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class IniciarDatos : MonoBehaviour
{
    static IniciarDatos sInstance = null;
    static public bool primeraVez;
    //private ConectarGooglePlay conectarGooglePlay;

    public DatosMovil datos = new DatosMovil();


    public static IniciarDatos Instance
    {
        get
        {
            return sInstance;
        }
    }



    void Awake() 
     {
        Debug.Log("CARGANDO LECTURA DE DATOS MOVIL" + Time.realtimeSinceStartup);
        if (sInstance == null)
         {
            sInstance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            Construir();
            //conectarGooglePlay = gameObject.GetComponent<ConectarGooglePlay>();
         }
         else if (sInstance != this)
         {
             Destroy(gameObject);
         }
        Debug.Log("LECTURA DE DATOS MOVIL CARGADA" + Time.realtimeSinceStartup);

    }


    void LateUpdate()
    {
      /*  if (GestionarXML.cargadoOk)
        {
            GestionarXML.cargadoOk = false;
        }
        */
        //GetComponent<GestionarXML>().guardarOk = true;

    }


    public void Construir()
    {
        datos.Constructor(10);
    }

}