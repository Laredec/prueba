using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkilesMejoras : MonoBehaviour {
    public bool[] mejoras_ok = new bool[5];

	// Use this for initialization
	void Start ()
    {
        OnInvocacion();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}


    public void OnInvocacion()
    {
        //MEJORA 1 
        if (mejoras_ok[1]) { }
        //MEJORA 2
        if (mejoras_ok[2]) { }

        //MEJORA 3
        if (mejoras_ok[3]) { }

    }

    public void OnMuerte()
    {


    }

    public void OnContinuo()
    {
        //MEJORA 4
        if (mejoras_ok[4]) { }

        //MEJORA 5 <20%
        if (mejoras_ok[5]) { }

    }


}
