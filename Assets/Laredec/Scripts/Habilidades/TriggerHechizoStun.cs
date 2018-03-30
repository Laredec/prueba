using UnityEngine;
using System.Collections;

public class TriggerHechizoStun : MonoBehaviour {

    public const int layerEnemigo = 10;
    public float timeStun=2f;
    public float destroyStun = 4f;

    void Start()
    {
        Destroy(this.gameObject, destroyStun);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == layerEnemigo  &&  other.gameObject.tag!= "Proyectil")
        {
            Stunear(other.gameObject);
            EnviarMensajeParalizado(other.gameObject);
        }
    }

    void EnviarMensajeParalizado(GameObject GO)
    {
        string strPacket = "H" + TomarNombreEnemigo(GO.name) + "/" + GO.GetComponentInChildren<Atacar>().stats.timeStun.ToString() + "|" + GO.GetComponentInChildren<Atacar>().stats.t0Stun.ToString();
        GameManager.Instance.EnviarPaquete(strPacket, true);
    }



    void Stunear(GameObject GO)
    {
        GO.GetComponentInChildren<Atacar>().stats.stun_ok = true;
        GO.GetComponentInChildren<Atacar>().stats.timeStun = timeStun;
        GO.GetComponentInChildren<Atacar>().stats.t0Stun = Time.realtimeSinceStartup;
    }




    string TomarNombreEnemigo(string nombre)
    {
        string nombreEnemigo = "";

        int index = nombre.IndexOf("E");
        if (index > -1)
            nombreEnemigo = nombre.Substring(0, index);
        else
            nombreEnemigo = nombre;

        return nombreEnemigo;
    }

}
