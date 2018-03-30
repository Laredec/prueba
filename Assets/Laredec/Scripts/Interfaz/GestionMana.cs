using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionMana : MonoBehaviour
{
    public float t0InicioMana;
    public float manaInicial;
    public float manaPorSegundo = 1f;
    public float manaMax = 10f;

    public Text textoManaActual;
    [HideInInspector]
    public float manaActual;
    public Image barraMana;

    static public GestionMana sInstance;

    private void Awake()
    {
        sInstance = this;
    }


    void Start()
    {
        t0InicioMana = DatosAccionesMultijugador.t0Room;
        manaInicial = 0;
        barraMana.fillAmount = 0;
    }


    public static GestionMana Instance
    {
        get
        {
            return sInstance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance && !GameManager.gameEndedOk)
        {
            manaActual = Mathf.Min(manaInicial + (Time.realtimeSinceStartup - t0InicioMana) * manaPorSegundo, manaMax);
            barraMana.fillAmount = Mathf.Repeat(manaActual, 1);
            textoManaActual.text = Mathf.Floor(manaActual).ToString();
        }
    }


    public bool GastarMana(int cantMana)
    {
        bool gastadoOk = false;
        float manaActual = Mathf.Min(manaInicial + (Time.realtimeSinceStartup - t0InicioMana) * manaPorSegundo, manaMax);

        if (cantMana <= manaActual)
        {
            manaInicial = manaActual - cantMana;
            t0InicioMana = Time.realtimeSinceStartup;
            gastadoOk = true;
        }

        return gastadoOk;
    }


    
}
