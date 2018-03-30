using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class DatosAccionesMultijugador : MonoBehaviour {

    static public Text mensajerec;

    static public float t0Room;
    // Use this for initialization
    static public DatosAccionesMultijugador sInstance = null;



    public static DatosAccionesMultijugador Instance
    {
        get
        {
            return sInstance;
        }
    }




    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
         
    }

    public static void MensajeNuevoRecibido(string mensaje)
    {
        GameObject mensajeMultiplayerGO = GameObject.Find("Mensaje Multiplayer");
        if (mensajeMultiplayerGO)
        {
            mensajerec = mensajeMultiplayerGO.GetComponent<Text>();
            if (mensajerec)
                mensajerec.text += mensaje;
        } 
            
    }



    public void ComenzarAtacar(string nombre, string nombreEnemigo, float danyo, bool criticoOk,float danyoEscudo)
    {
        GameObject AtacadoGO = null;

        GameObject[] Personajes = GameObject.FindGameObjectsWithTag("PERSONAJES");

        foreach (GameObject Personaje in Personajes)
            if (Personaje.name == nombre)
                AtacadoGO = Personaje;

        GameObject AtacanteGO = null;
        AtacanteGO = GameObject.Find(nombreEnemigo + "Enemigo");

        //DatosAccionesMultijugador.MensajeNuevoRecibido("nombre: " + nombre + "nombreEnemigo: " + nombreEnemigo);

        //////
        /*if (AtacanteGO != null)//SI EXISTE EN MI JUEGO
        {
            if (!AtacanteGO.GetComponentInChildren<Atacar>().soyTower_ok)
            {
                AtacanteGO.GetComponentInChildren<TriggerAreaVision>().persiguiendo_ok = false;
                AtacanteGO.GetComponentInChildren<TriggerAreaVision>().objetivo = null;
            }
               AtacanteGO.GetComponentInChildren<Atacar>().enemigoGO = AtacadoGO;
               AtacanteGO.GetComponentInChildren<TriggerAtaque>().objetivo = AtacadoGO;
               //AtacanteGO.GetComponentInChildren<TriggerAtaque>().PararMovimiento();
        }*/
        ///aplicamos el daño aunque no hayan proyectiles

            if (danyoEscudo > 0)
            {
                AtacadoGO.GetComponentInChildren<Atacar>().stats.escudoActual -= danyoEscudo;
                Debug.Log("ENTRAA danyoEscudo 3: " + danyoEscudo);

            AtacadoGO.GetComponentInChildren<Atacar>().InstanciarIndicadorEscudo(danyoEscudo, criticoOk);
            }

            AtacadoGO.GetComponentInChildren<Atacar>().stats.health -= danyo;
            AtacadoGO.GetComponentInChildren<Atacar>().InstanciarIndicadorDanyo(danyo, criticoOk);

            if (AtacadoGO.GetComponentInChildren<Atacar>().stats.health <= 0)
            {
                if (AtacadoGO.GetComponentInChildren<Animator>())
                    AtacadoGO.GetComponentInChildren<Animator>().SetTrigger("morir");


                ComprobarPerder(AtacadoGO); //de momento lo quitamos

            if (!AtacadoGO.GetComponentInChildren<Atacar>().soyTower_ok)
                Destroy(AtacadoGO, 0.2f);
            else
                Destroy(AtacadoGO);

        }
        else
            {
                if (AtacadoGO  &&  AtacadoGO.transform.GetChild(0).GetChild(0)  && AtacadoGO.transform.GetChild(0).GetChild(0).GetComponent<Text>()  && AtacadoGO.transform.GetChild(1)  && AtacadoGO.transform.GetChild(1).GetComponent<Atacar>()  && AtacadoGO.transform.GetChild(1).GetComponent<Atacar>().stats) //comprobante para que no pete
                    AtacadoGO.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = AtacadoGO.transform.GetChild(1).GetComponent<Atacar>().stats.health.ToString();
            }
       //}
    }




    void ComprobarPerder(GameObject obj)
    {
        if (GameManager.Instance  &&  !GameManager.Instance.debugOk)
            if (obj.name == "0"/*GameManager.Instance.jugadorPropio.name*/)
            {
                Debug.Log("entra en comprobar perder if");
                GameManager.Instance.Perder();
            }
    }




    public void Destruir(string nombre)
    {
        GameObject muertoGO = null;

        GameObject[] Personajes = GameObject.FindGameObjectsWithTag("PERSONAJES");

        foreach (GameObject Personaje in Personajes)
            if (Personaje.name == nombre)
                muertoGO = Personaje;

        ComprobarPerder(muertoGO);

        if (muertoGO != null /*&& !muertoGO.GetComponentInChildren<Atacar>().soyTower_ok*/)
            Destroy(muertoGO, 0.2f);
    }


    public void Stunear(string nombre,float timeStun,float t0Stun)
    {
        GameObject StunGO = null;

        GameObject[] Personajes = GameObject.FindGameObjectsWithTag("PERSONAJES");

        foreach (GameObject Personaje in Personajes)
            if (Personaje.name == nombre)
                StunGO = Personaje;

        if (StunGO != null)
        {
            StunGO.GetComponentInChildren<Atacar>().stats.stun_ok = true;
            StunGO.GetComponentInChildren<Atacar>().stats.timeStun = timeStun;
            StunGO.GetComponentInChildren<Atacar>().stats.t0Stun = /*t0Stun*/ Time.realtimeSinceStartup;
        }

           
    }

    public void Mover(string nombreEnemigo, float x, float y, float z, string nameTriggerArea)
    {
        GameObject EnemigoGO;

      //  Debug.Log("nombreEnemigo: "+ nombreEnemigo+ " x:" +x + " y:"+y+" z:"+z+"nameTriggerarea"+ nameTriggerArea);
        EnemigoGO = GameObject.Find(nombreEnemigo + "Enemigo");
       // Debug.Log(EnemigoGO);

        if (EnemigoGO != null && nameTriggerArea != "none" && EnemigoGO.GetComponentInChildren<TriggerAtaque>() && EnemigoGO.GetComponentInChildren<TriggerAtaque>().objetivo!= GameObject.Find(nameTriggerArea))
        {
            EnemigoGO.GetComponentInChildren<TriggerAtaque>().objetivo = null;//Puesto que tienes seleccionado uno que no es.
            //EnemigoGO.GetComponentInChildren<Atacar>().objetivo
        //    Debug.Log("Se mete en el TriggerArea+ "+ GameObject.Find(nameTriggerArea));
            EnemigoGO.GetComponentInChildren<TriggerAreaVision>().objetivo = GameObject.Find(nameTriggerArea);
            if (EnemigoGO.GetComponentInChildren<TriggerAreaVision>().objetivo != null)
                EnemigoGO.GetComponentInChildren<TriggerAreaVision>().persiguiendo_ok = true;
        }

       /* if (EnemigoGO != null && nameTriggerArea != "none" && EnemigoGO.GetComponentInChildren<TriggerAtaque>() && EnemigoGO.GetComponentInChildren<TriggerAtaque>().objetivo!=null && EnemigoGO.GetComponentInChildren<TriggerAtaque>().objetivo != GameObject.Find(nameTriggerArea))
        {
            Debug.Log("Se mete en el TriggerAtaque");

            EnemigoGO.GetComponentInChildren<TriggerAtaque>().objetivo = GameObject.Find(nameTriggerArea);
        }*/


        if (EnemigoGO != null && Time.realtimeSinceStartup - EnemigoGO.GetComponentInChildren<MoverGO>().t0 > EnemigoGO.GetComponentInChildren<MoverGO>().tEspera)
        {
        //    Debug.Log("Se mete en el CalculoPosicion");

            if ((new Vector3(-x, -y, z) - EnemigoGO.transform.localPosition).magnitude > 2)
            {
                EnemigoGO.GetComponentInChildren<MoverGO>().t0 = Time.realtimeSinceStartup;
                EnemigoGO.GetComponentInChildren<MoverGO>().pos0 = new Vector3(-x, -y, z);
                EnemigoGO.GetComponentInChildren<MoverGO>().transform.localPosition = new Vector3(-x, -y, z);
            }
            else
            {
            }
        }
    }


    public void SpawnCriature(string nameEnemigo, GameObject Criatura, Vector2[] vRands)
    {
        GameManager gameManager = GameManager.Instance;
        int layerJugador = 0;
        int layerTrigger = 0;
        GameObject towerPadre = GameObject.Find(nameEnemigo);

        if (gameManager)
        {
            layerJugador = GameManager.layerEnemigo;
            layerTrigger = GameManager.layerTriggerEnemigo;
        }

        for (int i = 0; i < vRands.Length; i++)
        {
            Debug.Log(towerPadre.name);
            Debug.Log(nameEnemigo);
            Debug.Log(vRands);
            Debug.Log(towerPadre.transform.position.x);
            Debug.Log(towerPadre.transform.position.y);
            Debug.Log(towerPadre.transform.position.z);
            Debug.Log(vRands[i].x);
            Debug.Log(vRands[i].y);

            Vector3 posicionCriatura = new Vector3(towerPadre.transform.position.x + vRands[i].x, towerPadre.transform.position.y + vRands[i].y, towerPadre.transform.position.z);
            GameObject nuevoGO = Instantiate(Criatura) as GameObject;
            nuevoGO.transform.position = posicionCriatura;
            nuevoGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            Debug.Log("Añade "+ nuevoGO.name);

            if (nuevoGO.GetComponent<CrearGOEnXTiempo>() && nuevoGO.GetComponent<CrearGOEnXTiempo>().GOChild)
            {
                nuevoGO = nuevoGO.GetComponent<CrearGOEnXTiempo>().GOChild;
            }


            if (nuevoGO.GetComponentInChildren<Atacar>())
            {
                nuevoGO.GetComponentInChildren<Atacar>().stats = nuevoGO.AddComponent<Stats>();
                Debug.Log("Añade componente Stat");

            }



            GameObject.Find("Datos").GetComponent<AtributosTropas>().CrearNuevo(/*ID del bicho*/nuevoGO.GetComponentInChildren<Atacar>().IDPredeterminada,/*GameObject padre*/nuevoGO,/*Nivel del bicho*/0);

            if (nuevoGO.GetComponentInChildren<TriggerAtaque>())
                nuevoGO.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok = true;
            if (nuevoGO.GetComponentInChildren<Atacar>())
                nuevoGO.GetComponentInChildren<Atacar>().enviarMensaje_ok = false;
            SkillSpawn.numCriaturasEnemigasCreadas++;
            nuevoGO.name = SkillSpawn.numCriaturasEnemigasCreadas.ToString() + "Enemigo";
          
            /* if (nuevoGO.GetComponent<SeguirPersonaje>())
             {
                 SeguirPersonaje seguirPersonaje = nuevoGO.GetComponent<SeguirPersonaje>();
                 if (seguirPersonaje)
                     seguirPersonaje.personaje = towerPadre;
             }*/

            nuevoGO.layer = layerJugador;
            if (nuevoGO.GetComponentInChildren<TriggerAtaque>())
                nuevoGO.GetComponentInChildren<TriggerAtaque>().gameObject.layer = layerTrigger;
            if (nuevoGO.GetComponentInChildren<TriggerAreaVision>())
                nuevoGO.GetComponentInChildren<TriggerAreaVision>().gameObject.layer = layerTrigger;

            if (nuevoGO.GetComponentInChildren<TriggerAreaCreacionEstructuras>())
                nuevoGO.GetComponentInChildren<TriggerAreaCreacionEstructuras>().jugador = towerPadre;
            DatosAccionesMultijugador.MensajeNuevoRecibido("Añade 2");
        }
       // DatosAccionesMultijugador.MensajeNuevoRecibido("Añade 3");

    }
}
