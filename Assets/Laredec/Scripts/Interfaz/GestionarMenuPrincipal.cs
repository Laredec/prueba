using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GestionarMenuPrincipal : MonoBehaviour
{

    // Use this for initialization

    public void Awake()
    {
        #if !UNITY_EDITOR && !UNITY_IPHONE && !UNITY_ANDROID
            Screen.SetResolution(300, 533, false);
        #endif
    }

    void Start ()
    {


    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}


    public void ComenzarBusquedaOponente()
    {
        GameSparksManager.Instance().FindPlayers();
        ChangeScene.LoadScene("Cargando Escenario");
        //gameManager.nombrePropio = nombreInput.text;
    }


 


    public void SalirPartida()
    {
        if (GameSparksManager.Instance())
            GameSparksManager.Instance().CerrarSala();
        SceneManager.LoadScene("MenuPrincipal");
    }
}
