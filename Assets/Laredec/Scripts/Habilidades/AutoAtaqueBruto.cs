using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAtaqueBruto : MonoBehaviour
{
    private int indiceAtaque = 0;
    Atacar atacar;
    public float anguloAtaque = 90f;


	// Use this for initialization
	void Awake ()
    {
        atacar = GetComponent<Atacar>();
    }

	
	// Update is called once per frame
	void Update ()
    {
        Animator animator = transform.parent.GetComponentInChildren<Animator>();

        switch (animator.GetInteger("numAtaque"))
        {
            case 1:
                atacar.areaOK = true;
                atacar.anguloArea = anguloAtaque;
                break;
            case 2:
                atacar.areaOK = true;
                atacar.anguloArea = anguloAtaque;
                break;
            case 3:
                atacar.areaOK = true;
                atacar.anguloArea = 360f;
                break;

        }

    }
}
