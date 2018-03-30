using UnityEngine;
using System.Collections;

public class SkillSpawn : MonoBehaviour
{
    public GameObject jugador;
    public GameObject Criatura;
    public int cantCriaturas;
    public bool seguirEnemigoOk;

    public float distMinSpawn;
    public float distMaxSpawn;
    static public int numCriaturasPropiasCreadas = 0;
    static public int numCriaturasEnemigasCreadas = 0;

    public float wLimitante = 4.68f * 2;
    public float hLimitante = 7.12f * 2;


    // Use this for initialization
    void Start ()
    {
        numCriaturasPropiasCreadas = 0;
        numCriaturasEnemigasCreadas = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}


    public void AplicarSkillDebug (bool propioOk)
    {
        Vector2[] vRands = new Vector2[1];
        vRands[0] = new Vector2(0.5f, 0.5f);
        AplicarSkill(propioOk, vRands);
    }


    public void AplicarSkill(bool propioOk, Vector2[] vRands)
    {
        GameManager gameManager = GameManager.Instance;

       
        int layerJugador = 0;
        int layerTrigger = 0;
        Debug.Log("AplicarSkill");

        if (gameManager)
        {
            if (propioOk)
            {
                jugador = gameManager.jugadorPropio;
                layerJugador = GameManager.layerAliado;
                layerTrigger = GameManager.layerTriggerAliado;
            }
            else
            {
                Debug.Log("propioOk else");

                jugador = gameManager.jugadorEnemigo;
                layerJugador = GameManager.layerEnemigo;
                layerTrigger = GameManager.layerTriggerEnemigo;
            }
        }

       

        if (jugador)
            for (int i=0; i < cantCriaturas; i++)
            {
                
                //Debug.Log(vRands[i]);

                Vector3 posicionCriatura = new Vector3(jugador.transform.position.x + vRands[i].x, jugador.transform.position.y + vRands[i].y, jugador.transform.position.z);

                float radioCriatura;
                if(Criatura.GetComponent<CrearGOEnXTiempo>() && Criatura.GetComponent<CrearGOEnXTiempo>().GOChild)
                    radioCriatura = Criatura.GetComponent<CrearGOEnXTiempo>().GOChild.GetComponentInChildren<CircleCollider2D>().radius;
                else
                    radioCriatura = Criatura.GetComponentInChildren<CircleCollider2D>().radius;

                /* 
                if (posicionCriatura.x < -wLimitante / 2 + radioCriatura)
                {
                    posicionCriatura.x = -wLimitante / 2 + radioCriatura;
                    vRands[i].x = posicionCriatura.x - jugador.transform.position.x;
                }
                if (posicionCriatura.x > wLimitante / 2 - radioCriatura)
                {
                    posicionCriatura.x = wLimitante / 2 - radioCriatura;
                    vRands[i].x = posicionCriatura.x - jugador.transform.position.x;
                }
                if (posicionCriatura.y < -hLimitante / 2 + radioCriatura)
                {
                    posicionCriatura.y = -hLimitante / 2 + radioCriatura;
                    vRands[i].y = posicionCriatura.y - jugador.transform.position.y;
                }
                if (posicionCriatura.y > hLimitante / 2 - radioCriatura)
                {
                    posicionCriatura.y = hLimitante / 2 - radioCriatura;
                    vRands[i].y = posicionCriatura.y - jugador.transform.position.y;
                }*/

                GameObject nuevoGO = Instantiate(Criatura) as GameObject;
                nuevoGO.transform.position = posicionCriatura;
                nuevoGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

                if (nuevoGO.GetComponent<CrearGOEnXTiempo>() && nuevoGO.GetComponent<CrearGOEnXTiempo>().GOChild)
                {
                    nuevoGO = nuevoGO.GetComponent<CrearGOEnXTiempo>().GOChild;
                }

                nuevoGO.transform.position = posicionCriatura;
                nuevoGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

                if (nuevoGO.GetComponentInChildren<Atacar>())
                    nuevoGO.GetComponentInChildren<Atacar>().stats = nuevoGO.AddComponent<Stats>();

                GameObject.Find("Datos").GetComponent<AtributosTropas>().CrearNuevo(/*ID del bicho*/nuevoGO.GetComponentInChildren<Atacar>().IDPredeterminada,/*GameObject padre*/nuevoGO,/*Nivel del bicho*/0);

                if (propioOk)
                {
                    if(nuevoGO.GetComponentInChildren<TriggerAtaque>())
                        nuevoGO.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok = false;
                    if(nuevoGO.GetComponentInChildren<Atacar>())
                        nuevoGO.GetComponentInChildren<Atacar>().enviarMensaje_ok = true;
                    numCriaturasPropiasCreadas++;
                    nuevoGO.name = numCriaturasPropiasCreadas.ToString();
                    //DatosAccionesMultijugador.MensajeNuevoRecibido("if nuevoGO.name: " + nuevoGO.name);
                }
                else
                {
                    if (nuevoGO.GetComponentInChildren<TriggerAtaque>())
                        nuevoGO.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok = true;
                    if(nuevoGO.GetComponentInChildren<Atacar>())
                        nuevoGO.GetComponentInChildren<Atacar>().enviarMensaje_ok = false;
                    numCriaturasEnemigasCreadas++;
                    nuevoGO.name = numCriaturasEnemigasCreadas.ToString() + "Enemigo";
                   // DatosAccionesMultijugador.MensajeNuevoRecibido("else nuevoGO.name: " + nuevoGO.name);
                }


               /* if (nuevoGO.GetComponent<SeguirPersonaje>())
                {
                    SeguirPersonaje seguirPersonaje = nuevoGO.GetComponent<SeguirPersonaje>();
                    if (seguirPersonaje)
                    {
                        if (!seguirEnemigoOk)
                            seguirPersonaje.personaje = jugador;
                        else
                            seguirPersonaje.personaje = gameManager.jugadorEnemigo;
                    }
                }*/

                nuevoGO.layer = layerJugador;
                if (nuevoGO.GetComponentInChildren<TriggerAtaque>())
                    nuevoGO.GetComponentInChildren<TriggerAtaque>().gameObject.layer = layerTrigger;
                if(nuevoGO.GetComponentInChildren<TriggerAreaVision>())
                    nuevoGO.GetComponentInChildren<TriggerAreaVision>().gameObject.layer = layerTrigger;

                if (nuevoGO.GetComponentInChildren<TriggerAreaCreacionEstructuras>())
                    nuevoGO.GetComponentInChildren<TriggerAreaCreacionEstructuras>().jugador = jugador;
                }
        
    }


    void Spawn()
    {

    }
}
