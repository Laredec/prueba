using UnityEngine;
using System.Collections;

public class ExplosionBolaTitan : MonoBehaviour {
    public int tiempoExplosion;
    public int tiempoDestruccion;
    public int danyoExplosion;

	// Use this for initialization
	void Start () {
        Invoke("ExplosionBola",tiempoExplosion);
        Destroy(this.gameObject, tiempoDestruccion);
    }

       void ExplosionBola()
       {
          GetComponent<ModificarRadio>().enabled = true;
       }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.IndexOf("Enemigo")!=-1)
        {
            float danyoReal=other.gameObject.GetComponentInChildren<Atacar>().stats.UsoEscudo(danyoExplosion);
            float damageEscudo = danyoExplosion - danyoReal;

            EnviarMensajeRivalAtaque(other.gameObject.name, danyoReal, false, damageEscudo);
        }
    }


    void EnviarMensajeRivalAtaque(string nombreEnemigoaux,float danyo, bool criticoOk, float danyoEscudo) //o
    {
        if (GameManager.Instance && !GameManager.Instance.debugOk)
        {
            string nombre = "-1";
            string nombreEnemigo="";
            int index = nombreEnemigoaux.IndexOf("E");
            if (index > -1)
                nombreEnemigo = nombreEnemigoaux.Substring(0, index);
            else
                nombreEnemigo = nombreEnemigoaux;

            int criticoInt = criticoOk ? 1 : 0;

            string strPacket = "C" + nombre + "/" + nombreEnemigo + "|" + danyo.ToString() + "%" + criticoInt.ToString() + "$" + danyoEscudo;
            GameManager.Instance.EnviarPaquete(strPacket, true);
          //  DatosAccionesMultijugador.MensajeNuevoRecibido("paquete env: " + strPacket);
        }

    }
}
