using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using GameSparks.Core;
using GameSparks.RT;

public class GameManager : MonoBehaviour
{
    public bool debugOk = false;

    public const int layerAliado = 9;
    public const int layerEnemigo = 10;
    public const int layerTriggerAliado = 11;
    public const int layerTriggerEnemigo = 12;

    static GameManager sInstance = null;

    const int QuickGameOpponents = 1;
    const int GameVariant = 0;

    // room setup progress
    static private float mRoomSetupProgress = 0.0f;
    // speed of the "fake progress" (to keep the player happy)
    // during room setup
    const float FakeProgressSpeed = 20.0f;
    const float MaxFakeProgress = 50.0f;
    static float mRoomSetupStartTime = 0.0f;

    //[HideInInspector]
    public GameObject jugadorPropio;
    //[HideInInspector]
    public GameObject jugadorEnemigo;

    static public bool desconectadoOk;

    static public bool gameEndedOk = false;

    [HideInInspector]
    public byte[] mPosPacket = new byte[10]; //variable de envio de datos


    [HideInInspector]
    public string idEnemigo; //de momento no se usa
    //[HideInInspector]
    public string nombreEnemigo;

    [HideInInspector]
    public string idPropia; //de momento no se usa
    //[HideInInspector]
    public string nombrePropio;

    public GameObject canvasInicioGO;
    public GameObject panelEsperaGO;
    
    public GameObject porcentajeCargadoGO;

    public GameObject EscenarioGO;

    public GameObject canvasGameGO;
    public GameObject panelVictoriaGO;
    public GameObject panelDerrotaGO;
    public GameObject panelSalirGO;
    public GameObject panelVSGO;
    public GameObject botonSurrenderGO;

    public Text mensajeFinalPartida;

    //publics para comenzar partida
    public GameObject[] prefabsPersonajes;

    public Transform[] posiciones;

    public bool cerrarSalaOk = false;
    public float t0CerrarSala;

    public ReproducirSonido sonidoGanar;
    public ReproducirSonido sonidoPerder;

    public Sprite barraAzulVida, barraRojaVida;

    public void OnEnable()
    {
        Debug.Log("Todo correcto");
        canvasInicioGO.SetActive(true); //Inicializamos esto por si quedan cambiados en la escena
        EscenarioGO.SetActive(false);

        cerrarSalaOk = false;
        IniciarInstance();

        if (debugOk)
            ComenzarPartida();
    }


    public void Update()
    {
        if (panelEsperaGO)
            if (panelEsperaGO.activeSelf)
            {
                DatosAccionesMultijugador.MensajeNuevoRecibido("RoomSetupProgress: " + RoomSetupProgress);
                porcentajeCargadoGO.GetComponent<Text>().text = Mathf.RoundToInt(RoomSetupProgress).ToString() + " %";
            }

        if (cerrarSalaOk && Time.realtimeSinceStartup - t0CerrarSala > 1f && SceneManager.GetActiveScene().name == "Juego")
        {
            Debug.Log("cerrando sala en cerrarSalaOk");
            if (GameSparksManager.Instance())
                GameSparksManager.Instance().CerrarSala();
        }

        if (jugadorPropio)
        {
            jugadorPropio.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Image>().sprite = barraAzulVida;
            float amountVidaPropia = jugadorPropio.GetComponentInChildren<Atacar>().stats.health / GameObject.Find("Datos").GetComponent<AtributosTropas>().statsIniciales[jugadorPropio.GetComponentInChildren<Atacar>().stats.ID].health[0];
            jugadorPropio.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = amountVidaPropia;
        }
        if (jugadorEnemigo)
        {
            jugadorEnemigo.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Image>().sprite = barraRojaVida;
            float amountVidaEnemigo = jugadorEnemigo.GetComponentInChildren<Atacar>().stats.health / GameObject.Find("Datos").GetComponent<AtributosTropas>().statsIniciales[jugadorEnemigo.GetComponentInChildren<Atacar>().stats.ID].health[0];
            jugadorEnemigo.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = amountVidaEnemigo;
        }
    }

   


    public static GameManager Instance
    {
        get
        {
            return sInstance;
        }
    }




    private void IniciarInstance()
    {
        Debug.Log("INICIANDO INSTANCE");
        sInstance = this;
        mRoomSetupStartTime = Time.realtimeSinceStartup;
        mRoomSetupProgress = 0.0f;
        gameEndedOk = false;
        cerrarSalaOk = false;
        desconectadoOk = false;
    }


    public void CreateQuickGame()
    {
        Debug.LogWarning("Creating Game");
        ////IniciarInstance();

        ComenzarPartida();
        //INSTANCIANDO JUGADORES AQUI


        //DatosAccionesMultijugador.MensajeNuevoRecibido(exclusiveBitMask.ToString());

        DatosAccionesMultijugador.sInstance = new DatosAccionesMultijugador();

    }




    public float RoomSetupProgress
    {
        get
        {
            float fakeProgress = (Time.realtimeSinceStartup - mRoomSetupStartTime) * FakeProgressSpeed;
            if (fakeProgress > MaxFakeProgress)
            {
                fakeProgress = MaxFakeProgress;
            }
            float progress = mRoomSetupProgress + fakeProgress;
            return progress < 99.0f ? progress : 99.0f;
        }
        
    }



    public void OnLeftRoom()
    {
        //DatosAccionesMultijugador.MensajeNuevoRecibido("OnLeftRoom");
        sInstance = null;
    }

    public void OnRoomSetupProgress(float percent)
    { 
        mRoomSetupProgress = percent*2.5f;
        mRoomSetupStartTime = Time.realtimeSinceStartup;

        Debug.Log("bla");
        if (mRoomSetupProgress > 50)
        {
            panelEsperaGO.GetComponent<ReproducirSonido>().Reproducir();
        }
        //AjustarT0Room.empezarOk = true;

    }


    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            DatosAccionesMultijugador.t0Room = Time.realtimeSinceStartup;
            EnviarPaquete("E" + nombrePropio, false);
            //SceneManager.LoadScene("Juego");
        }
        else
        {
            Debug.Log("Room no conectada");
        }

    }


   


    public void OnApplicationPause(bool paused)
    {
        /*if (!paused  &&  SceneManager.GetActiveScene().name == "Juego"  &&  !debugOk)
        {
            MostrarMensaje.Mostrar("Has abandonado la partida");
            Debug.Log("partida abandonada");
            Perder();
        }*/
    }


    

  

    public void Perder()
    {
        //Time.timeScale = 0;
        if (panelDerrotaGO)
            panelDerrotaGO.SetActive(true);
        if (panelSalirGO)
            panelSalirGO.SetActive(false);
        if (botonSurrenderGO)
            botonSurrenderGO.SetActive(false);

        Debug.Log("derrota");

        cerrarSalaOk = true;
        t0CerrarSala = Time.realtimeSinceStartup;

        gameEndedOk = true;
        //Destroy(this);
        if (jugadorPropio)
            jugadorPropio.SetActive(false);//si existe el jugador porque no le ha dado tiempo a ser destruido, lo atontamos

        sonidoPerder.Reproducir();
        GameObject.Find("Music").GetComponent<AudioSource>().Pause();
    }



    public void Ganar()
    {
        //Time.timeScale = 0;


        if (panelVictoriaGO)
            panelVictoriaGO.SetActive(true);
        if (panelSalirGO)
            panelSalirGO.SetActive(false);
        if (botonSurrenderGO)
            botonSurrenderGO.SetActive(false);

        Debug.Log("victoria");
        cerrarSalaOk = true;
        t0CerrarSala = Time.realtimeSinceStartup;

        gameEndedOk = true;
        //Destroy(this);
        if (jugadorEnemigo)
            jugadorEnemigo.SetActive(false);//si existe el jugador porque no le ha dado tiempo a ser destruido, lo atontamos
        sonidoGanar.Reproducir();
        GameObject.Find("Music").GetComponent<AudioSource>().Pause();
    }



    public void ConexionPerdida()
    {
       
        MostrarMensaje.Mostrar("Has perdido la conexion");
        //Time.timeScale = 0;
        if (panelDerrotaGO)
            panelDerrotaGO.SetActive(true);
        if (panelSalirGO)
            panelSalirGO.SetActive(false);
        if (botonSurrenderGO)
            botonSurrenderGO.SetActive(false);

        gameEndedOk = true;
        //Destroy(this)
        sonidoPerder.Reproducir();
        GameObject.Find("Music").GetComponent<AudioSource>().Pause();

    }



    public void EnviarPosicion (Vector3 pos)
    {
        if (!debugOk)
        {
            string strPacket = "A" + pos.x.ToString() + "/" + pos.y.ToString() + "|";
            EnviarPaquete(strPacket, false);
        }
    }



    public void OnRealTimeMessageReceived(string strPacket)
    {
        string codigoPacket = strPacket.Substring(0, 1);
        //DatosAccionesMultijugador.MensajeNuevoRecibido(strPacket);

        if (codigoPacket == "A") //mover pj
        {
            //DatosAccionesMultijugador.MensajeNuevoRecibido("1" + strPacket);
            int index = strPacket.IndexOf("/");
            int index2 = strPacket.IndexOf("|");
            float posX = float.Parse(strPacket.Substring(1, index - 1));

            
           // DatosAccionesMultijugador.MensajeNuevoRecibido("1.05" + strPacket.Substring(index + 1, index2 - index - 1));

            float posY = float.Parse(strPacket.Substring(index + 1, index2 - index - 2));

            //DatosAccionesMultijugador.MensajeNuevoRecibido("1.1" + strPacket.Substring(index2 + 1));

            //DatosAccionesMultijugador.MensajeNuevoRecibido("1.2" + strPacket);

            MoverGO moverGO = jugadorEnemigo.GetComponent<MoverGO>();
            if (moverGO.referenciaGO  /*&&  gameObject != GameManager.Instance.jugadorPropio*/) //rotamos
            {
                Debug.Log("ROTAMOS MOVERGO 1");
                Debug.Log("moverGO.referenciaGO.transform.position.x: " + moverGO.referenciaGO.transform.position.x);
                Debug.Log("moverGO.referenciaGO.transform.position.y: " + moverGO.referenciaGO.transform.position.y);
                Debug.Log("posX: " + posX);
                Debug.Log("posY: " + posY);

                moverGO.referenciaGO.transform.LookAt(new Vector3(posX, posY, moverGO.referenciaGO.transform.position.z), Vector3.back);

            }

            //jugadorEnemigo.GetComponent<MoverGO>().pos0 = new Vector3(posX, posY, 0);
            jugadorEnemigo.GetComponent<MoverGO>().t0 = Time.realtimeSinceStartup;
            jugadorEnemigo.GetComponent<MoverGO>().posFinal = new Vector3(posX, posY, 0);
        }

        else if (codigoPacket == "B") //recibes una Skill del enemigo
        {
            DatosAccionesMultijugador.MensajeNuevoRecibido("1" + strPacket);
            int indexAux = strPacket.IndexOf("/");
            Debug.Log("strPacket:" + strPacket);
            Debug.Log("indexAux:" + indexAux);

            int numAtaque = (indexAux > -1) ? int.Parse (strPacket.Substring(1,indexAux -1) ) : int.Parse(strPacket.Substring(1)); 
            Debug.Log("numAtaque: " + numAtaque);
            //DatosAccionesMultijugador.MensajeNuevoRecibido("numAtaque: " + numAtaque.ToString());
            
            GameObject habGO = jugadorEnemigo.transform.Find("Skills").Find("Hab" + numAtaque.ToString()).gameObject;
           // DatosAccionesMultijugador.MensajeNuevoRecibido("skillGO lo pasa");
            
            if (habGO)
            {
                SkillSpawn skillSpawn = habGO.GetComponent<SkillSpawn>();
                SkillArma skillArma = habGO.GetComponent<SkillArma>();
                SkillHechizo skillHechizo = habGO.GetComponent<SkillHechizo>();
                

                if (skillSpawn)
                {
                    //DatosAccionesMultijugador.MensajeNuevoRecibido("SkillSpawn if");
                    skillSpawn.AplicarSkill(false, ObtenerVRands(strPacket));
                    //DatosAccionesMultijugador.MensajeNuevoRecibido("SkillSpawn pasa");
                }
                else if (skillArma)
                {
                    skillArma.AplicarSkill();
                }
                else if (skillHechizo)
                {
                    skillHechizo.AplicarSkill();
                }
            }
            /*else
                DatosAccionesMultijugador.MensajeNuevoRecibido("!skillGO else");*/

        } //fin paquete de skill
        

        else if (codigoPacket == "C") //recibes un ataque del enemigo
        {
            int index = strPacket.IndexOf("/");
            int index2 = strPacket.IndexOf("|");
            int index3 = strPacket.IndexOf("%");
            int index4 = strPacket.IndexOf("$");

            string nombreEnemigo = strPacket.Substring(1, index - 1);
            string nombre = strPacket.Substring(index + 1, index2 - index - 1);
            float danyo = float.Parse(strPacket.Substring(index2 + 1, index3 - index2 - 1));
            int criticoInt = int.Parse(strPacket.Substring(index3 + 1, index4 - index3 - 1));
            float danyoEscudo = float.Parse(strPacket.Substring(index4 + 1));
            bool criticoOk = (criticoInt > 0) ? true : false;

            Debug.Log("strPacket: " + strPacket);

            Debug.Log("ENTRAA danyoEscudo 4: " + danyoEscudo);

            GameObject datosGO = GameObject.Find("Datos");
            DatosAccionesMultijugador datosaccionesMultijugador = datosGO.GetComponent<DatosAccionesMultijugador>();
            //DatosAccionesMultijugador.MensajeNuevoRecibido("nombre " + nombre + "nombreenemigo" + nombreEnemigo);

            datosaccionesMultijugador.ComenzarAtacar(nombre, nombreEnemigo, danyo, criticoOk, danyoEscudo);
            //DatosAccionesMultijugador.MensajeNuevoRecibido("El danyo es " + danyo.ToString());
        }

        else if (codigoPacket == "D") //recibes tu muerte de parte del enemigo
        {
            string nombre = strPacket.Substring(1);

           // DatosAccionesMultijugador.MensajeNuevoRecibido("nombreenemigo" + nombre);

            GameObject[] aux= GameObject.FindGameObjectsWithTag("PERSONAJES");

           

            GameObject datosGO = GameObject.Find("Datos");
            DatosAccionesMultijugador datosaccionesMultijugador = datosGO.GetComponent<DatosAccionesMultijugador>();
            datosaccionesMultijugador.Destruir(nombre);
        }

        else if (codigoPacket == "E") //nombre
        {
            //DatosAccionesMultijugador.MensajeNuevoRecibido("strPacket: " + strPacket.ToString());

            nombreEnemigo = "";
            if (strPacket.Length>1)
                nombreEnemigo = strPacket.Substring(1);
        }
        else if (codigoPacket == "F") //cambio de objetivo en ciratura
        {
           /* DatosAccionesMultijugador.MensajeNuevoRecibido("paquete F 1: " + strPacket);

            int index = strPacket.IndexOf("/");
            string nombrePerseguidor = strPacket.Substring(1, index - 1);
            string nombreObjetivo = strPacket.Substring(index + 1);
            DatosAccionesMultijugador.MensajeNuevoRecibido("paquete F 2: " + nombrePerseguidor + "/" + nombreObjetivo);

            TriggerAreaVision triggerAreaVision=null;
            MoverGO moverGO = null;
            DatosAccionesMultijugador.MensajeNuevoRecibido("perseguidor: " + nombrePerseguidor);
            GameObject perseguidor = GameObject.Find(nombrePerseguidor);
            DatosAccionesMultijugador.MensajeNuevoRecibido("ENCONTRADO perseguidor: " + perseguidor.name);

            DatosAccionesMultijugador.MensajeNuevoRecibido("objetivo: " + nombreObjetivo);
            GameObject objetivo = GameObject.Find(nombreObjetivo);
            DatosAccionesMultijugador.MensajeNuevoRecibido("ENCONTRADO objetivo: " + objetivo.name);

            if (perseguidor.GetComponentInChildren<TriggerAreaVision>())
               triggerAreaVision = perseguidor.GetComponentInChildren<TriggerAreaVision>();
            if(perseguidor.GetComponentInChildren<MoverGO>())
             moverGO = perseguidor.GetComponentInChildren<MoverGO>();

            DatosAccionesMultijugador.MensajeNuevoRecibido("paquete F 3: " + strPacket);

            perseguidor.GetComponentInChildren<Atacar>().ActivarColliders();
            if (triggerAreaVision)
            {
                triggerAreaVision.persiguiendo_ok = true;
                triggerAreaVision.objetivo = objetivo;
            }
            if (moverGO)
            {
                moverGO.posFinal = objetivo.gameObject.transform.position;
                moverGO.t0 = Time.realtimeSinceStartup;
            }
            DatosAccionesMultijugador.MensajeNuevoRecibido("paquete F 4: " + "sale");*/
        }
        else if (codigoPacket == "G") //cambio de objetivo en ciratura
        {
            /*DatosAccionesMultijugador.MensajeNuevoRecibido("paquete G: " + strPacket);
            string nombre = strPacket.Substring(1);
            GameObject criatura = GameObject.Find(nombre);
            MoverGO moverGO = criatura.GetComponentInChildren<MoverGO>();

            criatura.GetComponentInChildren<Atacar>().ActivarColliders();

            moverGO.enabled = true;
            moverGO.t0 = Time.realtimeSinceStartup;*/

        }
        else if (codigoPacket == "H") //ActivarStun
        {
            //DatosAccionesMultijugador.MensajeNuevoRecibido("paquete H: " + strPacket);
            int index = strPacket.IndexOf("/");
            int index2 = strPacket.IndexOf("|");

            string nombre = strPacket.Substring(1, index - 1);
            float timeStun = float.Parse(strPacket.Substring(index + 1, index2 - index - 1));
            float t0Stun = float.Parse(strPacket.Substring(index2 + 1));

            GameObject datosGO = GameObject.Find("Datos");
            DatosAccionesMultijugador datosaccionesMultijugador = datosGO.GetComponent<DatosAccionesMultijugador>();
            datosaccionesMultijugador.Stunear(nombre, timeStun, t0Stun);
        }
        else if (codigoPacket == "I") //Spawn Criature
        {
            Debug.Log("El paquete llega así: " + strPacket);
            int index = strPacket.IndexOf("|");
            int index2 = strPacket.IndexOf("/");

            string name = strPacket.Substring(1, index - 1)+"Enemigo";

            int numCriaturaSpawn = int.Parse(strPacket.Substring(index + 1, index2 - index - 1));
            Vector2[] vRands = ObtenerVRands(strPacket);

            GameObject datosGO = GameObject.Find("Datos");
            DatosAccionesMultijugador datosaccionesMultijugador = datosGO.GetComponent<DatosAccionesMultijugador>();
            datosaccionesMultijugador.SpawnCriature(name, prefabsPersonajes[numCriaturaSpawn], vRands);
        }
        else if (codigoPacket == "J") //actualizar posicion (para coordinar)
        {
            int index = strPacket.IndexOf("/");
            int index2 = strPacket.IndexOf("|");
            int index3 = strPacket.IndexOf("@");
            int index4 = strPacket.IndexOf("&");

            // DatosAccionesMultijugador.MensajeNuevoRecibido("strPacket E2: " + strPacket);

            string nombreEnemigo = strPacket.Substring(1, index - 1);
            float posx = float.Parse(strPacket.Substring(index + 1, index2 - index - 1));
            float posy = float.Parse(strPacket.Substring(index2 + 1, index3 - index2 - 1));
            float posz = float.Parse(strPacket.Substring(index3 + 1, index4 - index3 - 1));
            string nameTriggerArea = strPacket.Substring(index4 + 1);
            //DatosAccionesMultijugador.MensajeNuevoRecibido("strPacket E3: " + strPacket);

            GameObject datosGO = GameObject.Find("Datos");
            DatosAccionesMultijugador datosaccionesMultijugador = datosGO.GetComponent<DatosAccionesMultijugador>();
            //DatosAccionesMultijugador.MensajeNuevoRecibido("strPacket E4: " + strPacket);

            datosaccionesMultijugador.Mover(nombreEnemigo, posx, posy, posz, nameTriggerArea);
          //  DatosAccionesMultijugador.MensajeNuevoRecibido("strPacket E5: " + strPacket);
        }
        else if (codigoPacket == "K") //AñadirStat
        {
            int index = strPacket.IndexOf("/");
            int index2 = strPacket.IndexOf("|");
            int index3 = strPacket.IndexOf("@");
            int index4 = strPacket.IndexOf("&");
            int index5 = strPacket.IndexOf("%");
            int index6 = strPacket.IndexOf("+");

            // DatosAccionesMultijugador.MensajeNuevoRecibido("strPacket E2: " + strPacket);

            string nombreEnemigo = strPacket.Substring(1, index - 1);
            string nombre = strPacket.Substring(index + 1, index2 - index - 1);
            string tipo = strPacket.Substring(index2 + 1, index3 - index2 - 1);
            float value = float.Parse(strPacket.Substring(index3 + 1, index4 - index3 - 1));
            float duracion = float.Parse(strPacket.Substring(index4 + 1, index5 - index4 - 1));
            float t0 = float.Parse(strPacket.Substring(index5 + 1, index6 - index5 - 1))+DatosAccionesMultijugador.t0Room; //Le sumamos el T0Room de este jugador puesto que antes se lo habíamos restado
            string aliado_ok = strPacket.Substring(index6 + 1); //Este Aliado_OK para la otra persona es Enemigo_OK


            GameObject Pj = GameObject.Find(nombreEnemigo);
            if(aliado_ok=="0")
                Pj = GameObject.Find(nombreEnemigo+"Enemigo");

            Pj.GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(nombre,tipo,value,duracion,t0);

        }
        else if (codigoPacket == "L") //AñadirCuracion
        {
            int index = strPacket.IndexOf("/");

            string nombreEnemigo = strPacket.Substring(1, index - 1);
            float curacion = float.Parse(strPacket.Substring(index + 1));


            GameObject Pj = GameObject.Find(nombreEnemigo + "Enemigo");

            Pj.GetComponentInChildren<Atacar>().stats.Curar(curacion);

        }
        else if (codigoPacket == "M") //Añadir Prefab estético
        {
            int index = strPacket.IndexOf("|");
            int index2 = strPacket.IndexOf("+");
            int index3 = strPacket.IndexOf("%");

            string nombreEnemigo = strPacket.Substring(1, index - 1);
            string nombreSkill =strPacket.Substring(index + 1, index2 - index - 1);
            string aliado_ok = "";
            string nombreAliado = "";

            GameObject PjLanzadorProyectil=null;

            GameObject Pj = GameObject.Find(nombreEnemigo);


            if (index3 == -1)
            {
                aliado_ok = strPacket.Substring(index2 + 1); //Este Aliado_OK para la otra persona es Enemigo_OK

                if (aliado_ok == "0")
                    Pj = GameObject.Find(nombreEnemigo + "Enemigo");

                jugadorEnemigo.GetComponentInChildren<Buffos>().PjObjectivo = Pj;
                Debug.LogWarning("NOmbrePersonaje: " + Pj.name);
                jugadorEnemigo.GetComponentInChildren<Buffos>().Invoke("InstanciarPrefabRival" + nombreSkill, 0f);
            }
            else
            {
                aliado_ok = strPacket.Substring(index2 + 1, index3 - index2 - 1); //Este Aliado_OK para la otra persona es Enemigo_OK

                if (aliado_ok == "0")
                {
                    Pj = GameObject.Find(nombreEnemigo + "Enemigo");
                    nombreAliado = strPacket.Substring(index3 + 1);
                    PjLanzadorProyectil = GameObject.Find(nombreAliado);
                }
                else
                {
                    Pj = GameObject.Find(nombreEnemigo );
                    nombreAliado = strPacket.Substring(index3 + 1);
                    PjLanzadorProyectil = GameObject.Find(nombreAliado + "Enemigo");

                }



                //Debug.LogWarning("Envia el usuario "+ PjLanzadorProyectil.name+"  el proyectil "+nombreSkill+" al jugador "+Pj.name);

                jugadorPropio.GetComponentInChildren<Buffos>().proyectilAtaque = jugadorPropio.GetComponentInChildren<Buffos>().proyectiles.defaultP; //por defecto ponemos el Default por si no existe
                //jugadorPropio.GetComponentInChildren<Buffos>().Invoke("SeleccionarProyectil"+nombreSkill,0f);
                //PjLanzadorProyectil.GetComponentInChildren<Atacar>().AnimacionesAtaque();
                //CrearPrefabATropa(jugadorPropio.GetComponentInChildren<Buffos>().proyectilAtaque, Pj, PjLanzadorProyectil);
                StartCoroutine(LanzarProyectil(nombreSkill, PjLanzadorProyectil, Pj));
            }


            

        }
        else if (codigoPacket == "N") //Añadir Prefab estético en una posición
        {
            int index = strPacket.IndexOf("|");
            int index2 = strPacket.IndexOf("+");
            int index3 = strPacket.IndexOf("%");

            string nombreSkill = strPacket.Substring(1, index - 1);
            float posX = float.Parse(strPacket.Substring(index + 1, index2 - index - 1));
            float posY = float.Parse(strPacket.Substring(index2 + 1, index3 - index2 - 1));
            float posZ = float.Parse(strPacket.Substring(index3 + 1));

            GameObject auxGO=Instantiate(new GameObject()) as GameObject;
            Destroy(auxGO, 2f);
            jugadorEnemigo.GetComponentInChildren<Buffos>().PjObjectivo = auxGO;//Es únicamente para tomar la posición
            jugadorEnemigo.GetComponentInChildren<Buffos>().PjObjectivo.transform.position = new Vector3(-posX,-posY,posZ);
            jugadorEnemigo.GetComponentInChildren<Buffos>().Invoke("InstanciarPrefabRival" + nombreSkill, 0f);
        }
    }

    IEnumerator LanzarProyectil(string nombreSkill, GameObject PjLanzadorProyectil, GameObject Pj)
    {
        yield return jugadorPropio.GetComponentInChildren<Buffos>().StartCoroutine("SeleccionarProyectil" + nombreSkill);
        if (PjLanzadorProyectil)
        {
            PjLanzadorProyectil.GetComponentInChildren<Atacar>().AnimacionesAtaque();
            CrearPrefabATropa(jugadorPropio.GetComponentInChildren<Buffos>().proyectilAtaque, Pj, PjLanzadorProyectil);
        }
    }



    void CrearPrefabATropa(GameObject prefab, GameObject objetivoGO, GameObject lanzador)
    {
        GameObject proyectil = Instantiate(prefab, lanzador.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;

        MoverGOProyectil mover = proyectil.GetComponentInChildren<MoverGOProyectil>();
        if (mover != null)
        {
            mover.GOSeguir = objetivoGO;
            mover.t0SegundosActualizar = Time.realtimeSinceStartup;
            mover.vel = lanzador.GetComponentInChildren<Atacar>().stats.speedProyectil;
            mover.posFinal = objetivoGO.transform.position;
            mover.t0 = Time.realtimeSinceStartup;
            mover.pos0 = lanzador.transform.position;
        }

        TriggerAccionProyectil accion = proyectil.GetComponent<TriggerAccionProyectil>();

        if (accion)
        {
            accion.GoPadreCreadorDeEsteProyectil = lanzador;
            accion.enviarMensaje_ok = false;
        }

        if (lanzador.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok)
            proyectil.layer = 12;
        else
            proyectil.layer = 11;
        //nuevoRayo.GetComponent<>().soyEnemigo_ok=soyEnemigo_ok;
    }



    Vector2[] ObtenerVRands (string str)
    {

        string[] vStrAux = str.Split('/');
        Vector2[] vRands = new Vector2[vStrAux.Length-1];
        string aux = "";


        for (int i=0; i<vRands.Length; i++)//quitamos el 0 que es lo que no vale
        {
            vRands[i].x = float.Parse(vStrAux[i+1].Split('&')[0]);
            vRands[i].y = float.Parse(vStrAux[i+1].Split('&')[1]);
            aux += vRands[i].x.ToString() + vRands[i].y.ToString();
        }

        //DatosAccionesMultijugador.MensajeNuevoRecibido(aux);
        return vRands;
    }

    public void EnviarPaquete (string paquete, bool reliableOk)
    { 
        ////if (!GameObject.FindWithTag("IA"))
        {
            RTData data = new RTData();
            data.SetString(1, paquete);
            if (reliableOk)
                SendRTData(2, GameSparksRT.DeliveryIntent.RELIABLE, data);
            else
                SendRTData(2, GameSparksRT.DeliveryIntent.UNRELIABLE_SEQUENCED, data);
        }
    }




    public void ComenzarPartida()
    {
        Debug.Log("Comenzar partida");

        canvasInicioGO.SetActive(false);
        EscenarioGO.SetActive(true);


        jugadorPropio = InstanciarJugador(posiciones[0], true);

        
        jugadorEnemigo = InstanciarJugador(posiciones[1], false);

        GameObject referencia;
        referencia = jugadorPropio.GetComponent<MoverGO>().referenciaGO;
        if (referencia)
            referencia.transform.LookAt(jugadorEnemigo.transform, Vector3.back);

        referencia = jugadorEnemigo.GetComponent<MoverGO>().referenciaGO;
        if (referencia)
            referencia.transform.LookAt(jugadorPropio.transform, Vector3.back);



        jugadorEnemigo.GetComponent<InputMovimiento>().enabled = false; //desactivamos el inputMovimiento del enemigo (su movimiento ira mediante paquetes)

        Camera.main.transform.localScale = Vector3.one;

    }


    public GameObject InstanciarJugador(Transform startPoint, bool propioOk)
    {
        GameObject jugador = null;
        jugador = Instantiate(prefabsPersonajes[0], startPoint.localPosition, startPoint.localRotation) as GameObject; //rotancion debe ser 0

        jugador.GetComponentInChildren<Atacar>().stats = jugador.AddComponent<Stats>(); //new Stats(); // ESTO ES NECESARIO, PUESTO QUE AL INSTANCIAR UN GO NO SE INSTANCIAN LAS CLASES QUE TIENE DENTRO, AUNQUE TENGA NEW'S DENTRO
        jugador.GetComponentInChildren<Atacar>().IDPredeterminada = 5;

        GameObject.Find("Datos").GetComponent<AtributosTropas>().CrearNuevo(/*ID del bicho*/jugador.GetComponentInChildren<Atacar>().IDPredeterminada,/*GameObject padre*/jugador,/*Nivel del bicho*/0);

        if (propioOk)
        {
            Atacar atacar = jugador.GetComponentInChildren<Atacar>();
            TriggerAtaque triggerAtaque = jugador.GetComponentInChildren<TriggerAtaque>();

            atacar.enviarMensaje_ok = true;
            triggerAtaque.soyEnemigo_ok = false;

            jugador.layer = layerAliado;
            jugador.GetComponentInChildren<TriggerAtaque>().gameObject.layer = layerTriggerAliado;
            if (jugador.GetComponent<MoverGO>())
                if (jugador.GetComponent<MoverGO>().referenciaGO)
                    jugador.GetComponent<MoverGO>().referenciaGO.layer = layerAliado;
            //jugador.GetComponentInChildren<TriggerAreaVision>().gameObject.layer = layerTriggerAliado;  //los jugadores no tienen triggerareavision
            jugador.name = "0";
        }
        else
        {
            Atacar atacar = jugador.GetComponentInChildren<Atacar>();
            TriggerAtaque triggerAtaque = jugador.GetComponentInChildren<TriggerAtaque>();

            atacar.enviarMensaje_ok = false;
            triggerAtaque.soyEnemigo_ok = true;
            
            jugador.layer = layerEnemigo;
            jugador.GetComponentInChildren<TriggerAtaque>().gameObject.layer = layerTriggerEnemigo;
            if (jugador.GetComponent<MoverGO>())
                if (jugador.GetComponent<MoverGO>().referenciaGO)
                    jugador.GetComponent<MoverGO>().referenciaGO.layer = layerEnemigo;
            //jugador.GetComponentInChildren<TriggerAreaVision>().gameObject.layer = layerTriggerEnemigo;
            jugador.name = "0Enemigo";
        }



        return jugador;
    }







    /// <summary>
    /// Sends a unix timestamp in milliseconds to the server
    /// </summary>
    private IEnumerator SendTimeStamp()
    {

        // send a packet with our current time first //
        using (RTData data = RTData.Get())
        {
            data.SetLong(1, (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds); // get the current time as unix timestamp
            SendRTData(101, GameSparks.RT.GameSparksRT.DeliveryIntent.UNRELIABLE, data, new int[] { 0 }); // send to peerId -> 0, which is the server
        }
        yield return new WaitForSeconds(1f); // wait 1 second
        StartCoroutine(SendTimeStamp()); // send the timestamp again
    }


    private int packetSize_sent;

    /// <summary>
    /// Sends RTData and records the packet size
    /// </summary>
    /// <param name="_opcode">Opcode.</param>
    /// <param name="_intent">Intent.</param>
    /// <param name="_data">Data.</param>
    /// <param name="_targetPeers">Target peers.</param>
    public void SendRTData(int _opcode, GameSparksRT.DeliveryIntent _intent, RTData _data, int[] _targetPeers)
    {
        packetSize_sent = GameSparksManager.Instance().GetRTSession().SendData(_opcode, _intent, _data, _targetPeers);
    }
    /// <summary>
    /// Sends RTData to all players
    /// </summary>
    /// <param name="_opcode">Opcode.</param>
    /// <param name="_intent">Intent.</param>
    /// <param name="_data">Data.</param>
    public void SendRTData(int _opcode, GameSparksRT.DeliveryIntent _intent, RTData _data)
    {
        if (GameSparksManager.Instance())
            packetSize_sent = GameSparksManager.Instance().GetRTSession().SendData(_opcode, _intent, _data);
    }

    private int packetSize_incoming;
    /// <summary>
    /// Records the incoming packet size
    /// </summary>
    /// <param name="_packetSize">Packet size.</param>
    /// 

    public void PacketReceived(int _packetSize)
    {
        packetSize_incoming = _packetSize;
    }



    public void OnOpponentDisconnected(int _peerId)
    {
        Ganar();
        Debug.Log("hola");
    }



    public void SalirPartida()
    {
        if (Instance)
        {
            Debug.Log("entra en cerrar sala de Salir Partida");
            if (GameSparksManager.Instance())
                GameSparksManager.Instance().CerrarSala();
        }
        //GameManager.Instance.cerrarSalaOk = true;//esto no debe estar asi
        ChangeScene.LoadScene("MenuPrincipal");
    }




}
