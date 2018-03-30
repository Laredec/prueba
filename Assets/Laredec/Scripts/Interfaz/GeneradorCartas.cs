using UnityEngine;
using System.Collections;

public class GeneradorCartas : MonoBehaviour {
    public GameObject[] vCartasExistentes;
    public ActivarSkillPropia[] vBotones;

    public int[] vNumCarta;
    public int indiceUltimaCarta = 0;

    void Awake()
    {
        //vNumCarta = GenerarVIntRandomSinRepeticion(8);
        vBotones = transform.parent.GetComponentsInChildren<ActivarSkillPropia>();

    }
    // Use this for initialization
    void Start ()
    {
        CargarValoresCartas();
        for (int i=0; i<4; i++)
            GenerarCartaNueva(i, false);

    }


    public void CargarValoresCartas()
    {
        DatosMovil datos = IniciarDatos.Instance.datos;

        vNumCarta = new int[8];
        int acc = 0;
        for (int i = 0; i < datos.vCartasSeleccionadasOk.Length && acc < 8; i++)
            if (datos.vCartasSeleccionadasOk[i])
            {
                vNumCarta[acc] = i;
                acc++;
            }
        vNumCarta = ArrayModifier.GenerarVIntRandomSinRepeticion(vNumCarta);
    }


    public void ReCargarValoresCartas()
    {
        vNumCarta = ArrayModifier.GenerarVIntRandomSinRepeticion(vNumCarta, 4);
    }


    // Update is called once per frame
    void Update ()
    {
       
    }


    public void GenerarCartaNueva(int idBotonCartaPulsada, bool intercambiarOk = true)
    {
        int idCarta = vNumCarta[indiceUltimaCarta];
        GameObject nuevaCarta=Instantiate(vCartasExistentes[idCarta]) as GameObject;
        nuevaCarta.name = vCartasExistentes[idCarta].name;
        nuevaCarta.transform.SetParent(transform);
        nuevaCarta.transform.localPosition = new Vector3(-180 + 80 * idBotonCartaPulsada, -80, 0);
        nuevaCarta.transform.localScale = new Vector3(1, 1, 1);
        nuevaCarta.transform.SetSiblingIndex(idBotonCartaPulsada);

        if (intercambiarOk)
            ArrayModifier.IntercambiarInts(ref vNumCarta[idBotonCartaPulsada], ref vNumCarta[indiceUltimaCarta]);


        indiceUltimaCarta++;
        if (indiceUltimaCarta >= 8)
        {
            indiceUltimaCarta = 4;
            ReCargarValoresCartas();
        }


    }





    /*int[] GenerarVIntRandomSinRepeticion(int[] vInt)
    {
        int[] vAux = GenerarVIntRandomSinRepeticion(vInt.Length);
        int[] vIntClon = vInt.Clone() as int[];
        for (int i=0; i<vInt.Length; i++)
        {
            Debug.Log("vAux[" + i + "]:" + vAux[i]);
            vIntClon[i] = vInt[vAux[i]];
        }

        return vIntClon;
    }*/



   
}
