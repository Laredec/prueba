using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificarLayer : MonoBehaviour {

    public int LayerEnemigo;
    public int LayerAliado;

    public float radioCollider;

    private TriggerAtaque tA;

    private bool ensancharCollider_ok = false;
    private float aumentoRadio = 0.1f;

    // Use this for initialization
    void Start ()
    {
        tA = transform.parent.parent.GetChild(1).GetComponent<TriggerAtaque>();

        if (transform.parent.parent.gameObject.name.IndexOf("Enemigo") != -1)
            gameObject.layer =  LayerEnemigo;
        else
            gameObject.layer = LayerAliado;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!tA.objetivo && !GetComponent<CircleCollider2D>().enabled)
        {
            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().radius = 0.01f;
            ensancharCollider_ok = true;
        }
        else if (tA.objetivo)
            GetComponent<CircleCollider2D>().enabled = false;



        if(ensancharCollider_ok)
        {
            GetComponent<CircleCollider2D>().radius += aumentoRadio;

            if (GetComponent<CircleCollider2D>().radius>= radioCollider)
            {
                ensancharCollider_ok = false;
                GetComponent<CircleCollider2D>().radius = radioCollider;
            }

        }

    }


}
