using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class DatosMovil
{
    public int victorias;
    public int derrotas;

    public int idIdioma;//0=ingles, 1=español

    public int softCurreny;
    public int hardCurrency;
    public int elo;

    public bool musicaOk;
    public bool sonidoOk;

    public bool[] vCartasSeleccionadasOk;

    public void Constructor(int numTropas)
    {
        victorias = 0;
        derrotas = 0;

        idIdioma = 0;

        softCurreny = 10000;
        hardCurrency = 2000;
        elo = 0;
        
        musicaOk = true;
        sonidoOk = true;
        vCartasSeleccionadasOk = new bool[24];
        for (int i = 0; i < 8; i++)
            vCartasSeleccionadasOk[i] = true;
    }

}
