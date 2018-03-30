using UnityEngine;
using System.Collections;

public class ModificarApagadoPantalla : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Modificar(0); //0 es nunca
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void Modificar(int value)
    {
        if(value==0)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        else
            Screen.sleepTimeout = value;
    }
}
