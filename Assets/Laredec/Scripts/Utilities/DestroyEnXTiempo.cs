using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnXTiempo : MonoBehaviour {
    public float tiempo;
	// Use this for initialization
	void Start () {
        Destroy(gameObject,tiempo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
