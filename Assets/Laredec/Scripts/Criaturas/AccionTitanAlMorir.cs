using UnityEngine;
using System.Collections;

public class AccionTitanAlMorir : MonoBehaviour {
    public GameObject BolaGO;

    void OnDestroy()
    {
        GameObject bola=Instantiate(BolaGO,transform.position,transform.rotation) as GameObject;
        bola.GetComponent<Rigidbody2D>().velocity=(((Vector2)(GetComponent<MoverGO>().pos0)) - GetComponent<MoverGO>().posFinal).normalized *(-(GetComponent<Stats>().movSpeed+ GetComponent<Stats>().ComprobarHabilidadesAumentoMovSpeed())); //Darle velocidad a la bola
        bola.layer = transform.GetChild(1).gameObject.layer;//ponemos el mismo layer que tenga el Atacar de esta misma tropa
    }
}

