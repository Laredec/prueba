using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayModifier : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    static public int[] GenerarVIntRandomSinRepeticion(int[] vInt, int inicio = 0, int tam = -1) //genera 'tam' numeros aleatorios sin repetir desde la posicion 'inicio' 
    {
        if (tam == -1)
            tam = vInt.Length - inicio;

        int[] vIntClon = vInt.Clone() as int[];

        int[] vAux = GenerarVIntRandomSinRepeticion(tam);

        for (int i = 0; i < tam; i++)
        {
            vIntClon[i + inicio] = vInt[inicio + vAux[i]];
        }

        return vIntClon;
    }




    static public int[] GenerarVIntRandomSinRepeticion(int tam) //genera 'tam' ints sin repeticion y los coloca en un int[]
    {
        int[] vInt = new int[tam];

        for (int i = 0; i < tam; i++)
        {
            int acc = 0;
            vInt[i] = Random.Range(0, tam - i);
            //Debug.Log("random inicial: " + vInt[i]);

            int[] vIntAux = OrdenarVInt(vInt, i);

            for (int j = 0; j < i; j++)
            {
                //Debug.Log("vIntAux["  + j + "]: " + vIntAux[j]);
                if (vInt[i] >= vIntAux[j])
                {
                    vInt[i]++;
                }
            }
            //vInt[i] += acc;


        }

        return vInt;
    }


    static public int[] OrdenarVInt(int[] vInt, int tamOrdenacion = -1, bool ascendenteOk = true) //ordena un int[] de forma ascendente o descendente desde el elemento 0 hasta el tamOrdenacion
    {
        int[] vIntOrdenado = vInt.Clone() as int[];

        if (tamOrdenacion == -1)
            tamOrdenacion = vIntOrdenado.Length;

        for (int i = 0; i < tamOrdenacion; i++)
        {
            for (int j = i + 1; j < tamOrdenacion; j++)
            {
                if (ascendenteOk)
                {
                    if (vIntOrdenado[j] < vIntOrdenado[i])
                    {
                        IntercambiarInts(ref vIntOrdenado[j], ref vIntOrdenado[i]);
                    }
                }
                else
                {
                    if (vIntOrdenado[j] > vIntOrdenado[i])
                    {
                        IntercambiarInts(ref vIntOrdenado[j], ref vIntOrdenado[i]);
                    }
                }
            }
        }


        return vIntOrdenado;
    }


    static public void IntercambiarInts(ref int a, ref int b) //intercambia dos ints por referencia
    {
        int aux = a;
        a = b;
        b = aux;
    }

    static public int GenerarIntConExcepcion(int tam, int exc) //genera un numero Random entre 0 y tam que no puede ser exc
    {
        int ran = Random.Range(0, tam - 1);
        if (ran >= exc)
            ran++;

        return ran;
    }



}
