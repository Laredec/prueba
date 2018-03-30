using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class ChangeScene : MonoBehaviour
{
    public string nombreEscena = "";
    public bool chargeOnStart = false;

    public GameObject[] vEscenas;


    public void Update()
    {
        if (chargeOnStart)
        {
            LoadScene(nombreEscena);
            chargeOnStart = false;
        }

    }



    public void Start()
    {
        if (GameObject.Find("Datos") && GameObject.Find("Datos").GetComponent<ChangeScene>().vEscenas != null)
            vEscenas = GameObject.Find("Datos").GetComponent<ChangeScene>().vEscenas;

    }

    static public void LoadScene(string sceneName)
    {
        IniciarDatos.Instance.GetComponent<ChangeScene>()._LoadScene(sceneName);
    }

    public void _LoadScene(string sceneName)
    {
        bool escenaCargada_Ok = false;

        GameObject[] escenas = GameObject.FindGameObjectsWithTag("Escena");
        if (escenas != null)
            foreach (GameObject escena in escenas)
                Destroy(escena);

        for (int i = 0; i < vEscenas.Length; i++)
        {
            if (vEscenas[i] != null && sceneName == vEscenas[i].name)
            {
                Instantiate(vEscenas[i]);
                escenaCargada_Ok = true;
            }
        }

        if (!escenaCargada_Ok)
        {
            IEAnyadirYCargarEscenaConLoading(sceneName);
            escenaCargada_Ok = true;

        }

    }


    public void IEAnyadirYCargarEscenaConLoading(string scene)
    {
        ////AutoLogin.ActivarLoading(scene);
        /*yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        */

        AnyadirEscenaAlArray(Resources.Load("BigBox/Escenas/PrefabEscenas/" + scene) as GameObject);
        Instantiate(vEscenas[vEscenas.Length - 1]);
        //Debug.Log("Cargando escena NUEVA: " + scene);

        for (int i = 0; i < vEscenas.Length; i++)
            Debug.Log(vEscenas[i].name);
        //yield return new WaitForEndOfFrame();
        ////AutoLogin.DesactivarLoading();
        //yield return null;
    }


    public void AnyadirEscenaAlArray(GameObject nuevaEscena)
    {
        GameObject[] vAuxGO = (GameObject[])vEscenas.Clone();
        vEscenas = new GameObject[vAuxGO.Length + 1];
        for (int i = 0; i < vAuxGO.Length; i++)
        {
            vEscenas[i] = vAuxGO[i];
        }
        vEscenas[vAuxGO.Length] = nuevaEscena;
    }

}
