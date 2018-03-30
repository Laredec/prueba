using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyadirCirculoColor : MonoBehaviour {

    public GameObject[] circulo = new GameObject[2];

	// Use this for initialization
	void Start () {
		if(transform.gameObject.name.IndexOf("Enemigo")!=-1)
        {
            GameObject nuevo= Instantiate(circulo[1], transform) as GameObject;
            nuevo.transform.localPosition = new Vector3(0, 0, -0.05f);
        }
        else
        {
            GameObject nuevo = Instantiate(circulo[0], transform) as GameObject;
            nuevo.transform.localPosition = new Vector3(0, 0, -0.05f);
        }
        GetComponent<AnyadirCirculoColor>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
