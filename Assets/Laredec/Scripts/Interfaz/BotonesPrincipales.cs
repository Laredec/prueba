using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BotonesPrincipales : MonoBehaviour {
    public Vector3[,] posBoton = new Vector3[5,5];
    public Sprite[] imagenes = new Sprite[5];

    [HideInInspector]
    public int botonSelecAnterior = -1;
    public int botonSelec=2;

    // Use this for initialization
    void Start () {
        ActivarBotones(true,0);

        posBoton[0, 0] = new Vector3(0, 0, 0);
        posBoton[0, 1] = new Vector3(0, 0, 0);
        posBoton[0, 2] = new Vector3(0, 0, 0);
        posBoton[0, 3] = new Vector3(0, 0, 0);
        posBoton[0, 4] = new Vector3(0, 0, 0);

        posBoton[1, 0] = new Vector3(-135, 0, 0);
        posBoton[1, 1] = new Vector3(0, 0, 0);
        posBoton[1, 2] = new Vector3(0, 0, 0);
        posBoton[1, 3] = new Vector3(0, 0, 0);
        posBoton[1, 4] = new Vector3(0, 0, 0);

        posBoton[2, 0] = new Vector3(-135, 0, 0);
        posBoton[2, 1] = new Vector3(-90, 0, 0);
        posBoton[2, 2] = new Vector3(0, 0, 0);
        posBoton[2, 3] = new Vector3(0, 0, 0);
        posBoton[2, 4] = new Vector3(0, 0, 0);

        posBoton[3, 0] = new Vector3(-135, 0, 0);
        posBoton[3, 1] = new Vector3(-90, 0, 0);
        posBoton[3, 2] = new Vector3(-90, 0, 0);
        posBoton[3, 3] = new Vector3(0, 0, 0);
        posBoton[3, 4] = new Vector3(0, 0, 0);

        posBoton[4, 0] = new Vector3(-135, 0, 0);
        posBoton[4, 1] = new Vector3(-90, 0, 0);
        posBoton[4, 2] = new Vector3(-90, 0, 0);
        posBoton[4, 3] = new Vector3(-90, 0, 0);
        posBoton[4, 4] = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update () {
      
    }


    public void CambiarPosicionBotones(int boton)
    {
        if (!ComprobarMovimientoBotones(0))
        {
            botonSelecAnterior = botonSelec;
            if (boton == botonSelec)
                return;

            botonSelec = boton;

            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(i).gameObject.GetComponent<MovimientoBoton>().posfinal = posBoton[botonSelec, i];
                transform.GetChild(i).gameObject.GetComponent<MovimientoBoton>().enabled = true;
            }

            transform.GetChild(botonSelec).gameObject.GetComponent<MovimientoBoton>().posfinal = new Vector3(transform.GetChild(botonSelec).localPosition.x, transform.GetChild(botonSelec).localPosition.y, 0);

            transform.GetChild(botonSelecAnterior).gameObject.GetComponent<MovimientoBoton>().posfinal = new Vector3(0, -250, 0);
            transform.GetChild(botonSelecAnterior).gameObject.GetComponent<MovimientoBoton>().velocidad = 100;
            ActivarBotones(false,0);
        }
    }


    public void ActivarBotones(bool valor,int cant)
    {
        if (!ComprobarMovimientoBotones(cant))
        {
            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(i).GetComponentInChildren<Button>().enabled = valor;
              //  transform.GetChild(i).GetChild(0).GetComponent<Image>().raycastTarget = valor;
            }

            transform.GetChild(botonSelec).GetComponentInChildren<Button>().enabled = false;
            //transform.GetChild(botonSelec).GetComponentInChildren<Image>().enabled = false;

        }
    }


    bool ComprobarMovimientoBotones(int cant)
    {
        int acc = 0;
        for (int i = 0; i < 5; i++)
        {
            if (transform.GetChild(i).GetComponentInChildren<MovimientoBoton>().enabled == true)
            {
                acc++;
                if(acc>cant)
                    return true;
            }
        }

        return false;
    }

}
