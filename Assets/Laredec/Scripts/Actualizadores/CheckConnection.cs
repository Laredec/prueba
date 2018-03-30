using UnityEngine;
using System;
using System.Net;
using System.Collections;

public class CheckConnection : MonoBehaviour
{

    void Update()
    {
        checkifonline();


    }


    public void checkifonline()
    {
    #if UNITY_ANDROID
        //DatosAccionesMultijugador.MensajeNuevoRecibido(Application.internetReachability.ToString() + "|" + Application.internetReachability.ToString());
        if ( Application.internetReachability == NetworkReachability.NotReachable)
        {
            DatosAccionesMultijugador.MensajeNuevoRecibido("sin conexion");
            GameManager.desconectadoOk = true;
            GameManager.Instance.ConexionPerdida();
        }

        if (GameManager.desconectadoOk) //si te has desconectado y ya tienes internet o datos
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                DatosAccionesMultijugador.MensajeNuevoRecibido("cerrando sala al reconectar 1");
                Debug.Log("cerrando sala al reconectar 1");
                GameSparksManager.Instance().CerrarSala();
                GameManager.desconectadoOk = false;
                DatosAccionesMultijugador.MensajeNuevoRecibido("cerrando sala al reconectar 2");
            }


    #endif


    }






    public static bool HasConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = new WebClient().OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    bool Check(string URL)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Timeout = 5000;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK) return true;
            else return false;
        }
        catch
        {
            return false;
        }
    }



}
