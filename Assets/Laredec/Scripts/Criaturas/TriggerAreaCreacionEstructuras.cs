using UnityEngine;
using System.Collections;

public class TriggerAreaCreacionEstructuras : MonoBehaviour {

    //[HideInInspector]
    public GameObject jugador;
    public GameObject[] ActivarGO;

    void Start()
    {
        transform.parent.transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y, jugador.transform.position.z);
        for (int i = 0; i < ActivarGO.Length; i++)
            ActivarGO[i].SetActive(false);
        transform.parent.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
    }

    void Update()
    {
        if ((transform.parent.position - jugador.transform.position).sqrMagnitude > 1.5 )
        {
            for (int i = 0; i < ActivarGO.Length; i++)
                ActivarGO[i].SetActive(true);

            transform.parent.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;

            Destroy(gameObject);
        }
    }
}
