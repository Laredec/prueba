using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrearMazo : MonoBehaviour
{
    private Button[] vCartas;
    public bool[] vSeleccionadasOk;
    // Use this for initialization
    public int cartasSeleccionadas
    {
        get
        {
            int num = 0;
            for (int i = 0; i < vSeleccionadasOk.Length; i++)
                if (vSeleccionadasOk[i])
                    num++;
            return num;
        }
    }


    void Awake ()
    {
        vCartas = transform.GetComponentsInChildren<Button>();
        vSeleccionadasOk = new bool[vCartas.Length];
        CargarCartas();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ActualizarMarcos();
        ComprobarMazoCompleto();

	}

    public void SeleccionarCarta(int id)
    {
        vSeleccionadasOk[id] = !vSeleccionadasOk[id];

    }

    void ActualizarMarcos()
    {
        for (int i=0; i<vCartas.Length; i++)
        {
            vCartas[i].transform.parent.GetComponent<Image>().enabled = vSeleccionadasOk[i];
        }
    }


    void ComprobarMazoCompleto()
    {
        DesactivarCartasSeleccionadas(cartasSeleccionadas >= 8);
    }



    void DesactivarCartasSeleccionadas(bool ok = true)
    {
        for (int i=0; i<vCartas.Length; i++)
        {
            vCartas[i].interactable = ok ? vSeleccionadasOk[i] : true;
        }
    }


    public void GuardarCartasYSalir()
    {
        DatosMovil datos = IniciarDatos.Instance.datos;
        datos.vCartasSeleccionadasOk = new bool[vSeleccionadasOk.Length];

        for (int i = 0; i < Mathf.Min(vSeleccionadasOk.Length, datos.vCartasSeleccionadasOk.Length); i++)
        {
            datos.vCartasSeleccionadasOk[i] = vSeleccionadasOk[i];
        }

        IniciarDatos.Instance.GetComponent<GestionarXML>().Guardar_Datos();
        ChangeScene.LoadScene("MenuPrincipal");
    }


    public void CargarCartas()
    {
        DatosMovil datos = IniciarDatos.Instance.datos;

        for (int i = 0; i < Mathf.Min(vSeleccionadasOk.Length, datos.vCartasSeleccionadasOk.Length); i++)
        {
            vSeleccionadasOk[i] = datos.vCartasSeleccionadasOk[i];
        }

    }



}
