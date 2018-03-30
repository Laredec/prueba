using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaExplosion : MonoBehaviour
{
    int layerEnemigo = 10;
    public float timeDestroy;
    public GameObject prefabExplosion;

    // Use this for initialization
    void Start ()
    {
        if (GetComponent<TriggerAtaque>().soyEnemigo_ok && GetComponent<TriggerAtaque>().soyTower_ok)
            Destroy(transform.parent.gameObject, timeDestroy);
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="PERSONAJES" && other.gameObject.layer==layerEnemigo && !GetComponent<TriggerAtaque>().soyEnemigo_ok)
        {
            GameManager.Instance.jugadorPropio.GetComponentInChildren<Buffos>().PjObjectivo = transform.parent.gameObject; //el objetivo donde explota es justo encima mía
            GameManager.Instance.jugadorPropio.GetComponentInChildren<Buffos>().Invoke("ExplosionTrampaGigantesAkiles",0f);
            Destroy(transform.parent.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

}
