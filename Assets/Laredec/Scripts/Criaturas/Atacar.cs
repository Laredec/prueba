using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Atacar : MonoBehaviour{
   
    public GameObject enemigoGO;
    public Atacar AtaqueEnemigo;
    public float t0ultimoAtaque = 0;
    public bool atacando_ok = false;
    public bool soyTower_ok;
    public bool enviarMensaje_ok;
    public bool ataqueSoyEnemigo_ok = false;
    public bool atacarNadaMasEntrar_Ok = false;
    public Stats stats;
    public bool areaOK = false; //considerar meterlo en stats en un futuro (false por defecto)
    public float anguloArea = 360f; //si 360 en area, si otra cosa en cono (en caso de ser areaOk == true)
    public float radioArea = 5f;

    public int IDPredeterminada;

    public bool ataqueCriticoOk; //este bool indica si el ataque actual ha sido critico

    public GameObject indicadorDanyo;
    public GameObject indicadorEscudo;


    //VARIABLES QUE LAS RELLENARÁ DATOSACCIONMULTIJUGADOR CUANDO LAS RECIBAS DEL ENEMIGO
    public float ENEMIGOdanyo;
    public float ENEMIGOdanyoEscudo;


    public bool muertoOk;



    // Use this for initialization
    void Start ()
    {
       /// atacarNadaMasEntrar_Ok = false;
      //  ataqueSoyEnemigo_ok = false;
      //  atacando_ok=false;
       // t0ultimoAtaque = 0;

        /*if(soyTower_ok)
        {
            if (colorBola == IniciarDatos.Instance.datos.Equipo)
            {
                transform.parent.gameObject.layer = 9;
                transform.parent.transform.GetChild(1).gameObject.layer = 11;
            }
            else
            {
                transform.parent.gameObject.layer = 10;
                transform.parent.transform.GetChild(1).gameObject.layer = 12;
                transform.parent.transform.GetChild(1).GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok = true;
            }
        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*if (transform.parent.GetChild(0).GetChild(0) && transform.parent.GetChild(0).GetChild(0).GetComponent<Text>())
            transform.parent.GetChild(0).GetChild(0).GetComponent<Text>().text = transform.parent.gameObject.name;//AtaqueEnemigo.stats.health.ToString();
            */
        if (GameManager.gameEndedOk)
        {
            /*if (transform.parent.GetComponentInChildren<Animator>()  &&  IDPredeterminada == 5)
            {
                if (stats.melee)
                    transform.parent.GetComponentInChildren<Animator>().SetBool("disparandoOk", false);
                else
                    transform.parent.GetComponentInChildren<Animator>().SetBool("disparandoOk", true);
            }*/
            this.enabled = false;
        }



        if (enemigoGO != null)
        {
            if (atacarNadaMasEntrar_Ok && !GetComponent<TriggerAtaque>().soyEnemigo_ok || atacando_ok && Time.realtimeSinceStartup - t0ultimoAtaque > (1.0f / (stats.attackSpeed+stats.ComprobarHabilidadesAumentoAttackSpeed())) && !GetComponent<TriggerAtaque>().soyEnemigo_ok || GetComponent<TriggerAtaque>().soyEnemigo_ok && ataqueSoyEnemigo_ok)
            {
                if (atacarNadaMasEntrar_Ok)
                {
                    atacarNadaMasEntrar_Ok = false;
                    GetComponent<Atacar>().t0ultimoAtaque = Time.realtimeSinceStartup;
                }
                 else
                    t0ultimoAtaque += (1.0f / (stats.attackSpeed+ stats.ComprobarHabilidadesAumentoAttackSpeed()));


                AtaqueEnemigo = enemigoGO.GetComponentInChildren<Atacar>();

                AnimacionesAtaque();


                GestionarAtaque();
                ataqueSoyEnemigo_ok = false;

            }
        }
        else if(atacando_ok)//si ha desaparecido o muerto por que otro le golpea
        {
            atacando_ok = false;
            ActivarColliders();
        }

    }

    void GestionarAtaque()
    {
        //añadir if area bla bla else esto de aqui abajo
        GameObject[] vEnemigos = BuscarEnemigosAtacar();

        if (enviarMensaje_ok)
        {
            for (int i=0; i<vEnemigos.Length; i++)
                EnviarRivalPrefab(stats.prefabProyectil.name, enemigoGO, transform.parent.gameObject);
        }

        //añadir a esta funcion GO Target ya que se envia a enemigoGO siempre ahoram ismo
        for (int i = 0; i < vEnemigos.Length; i++)
            CrearProyectil(stats.CalcularCriticoProyectil(), stats.CalcularDanyoProyectil(), vEnemigos[i]);
    }


    GameObject[] BuscarEnemigosAtacar()
    {
        GameObject[] vEnemigos;
        if (!areaOK)
        {
            Debug.Log("areaOk = false");
            vEnemigos = new GameObject[1];
            vEnemigos[0] = enemigoGO;
            Debug.Log("enemigoGO= " + enemigoGO);
        }
        else
        {
            Debug.Log("areaOk = true");
            Debug.Log("stats.rangeAttack: " + stats.rangeAttack);

            vEnemigos = ComprobarPersonajesDentroRadio(radioArea, enemigoGO);
        }

        Debug.Log("vEnemigos[0]: " + vEnemigos[0]);


        return vEnemigos;
    }


    GameObject[] ComprobarPersonajesDentroRadio(float radio, GameObject target, bool enemigos_ok = true, float anguloApertura = 360)
    {
        int layerEnemigo = 10;
        int layerAliado = 9;

        GameObject[] personajesDentro = new GameObject[1];
        personajesDentro[0] = target;

        float xAtacante = transform.parent.position.x;
        float yAtacante = transform.parent.position.y;
        float xTarget = target.transform.position.x;
        float yTarget = target.transform.position.y;

        Debug.Log("xAtacante, yAtacante: " + xAtacante + ", " + yAtacante);
        Debug.Log("xTarget, yTarget: " + xTarget + ", " + yTarget);
        Vector2 vecDifTarget = new Vector2((xTarget - xAtacante), (yTarget - yAtacante));
        Debug.Log("vecDifTarget: " + vecDifTarget);
        

        foreach (GameObject personaje in GameObject.FindGameObjectsWithTag("PERSONAJES"))
            if (target != personaje  &&  (enemigos_ok && personaje.layer == layerEnemigo ||
                !enemigos_ok && personaje.layer == layerAliado) )
            {
                Debug.Log("enemigos_ok: " + enemigos_ok);
                Debug.Log("anguloApertura: " + anguloApertura);

                Debug.Log("entra aqui");
                float x2 = personaje.transform.position.x;
                float y2 = personaje.transform.position.y;

                Debug.Log("xAtacante, yAtacante: " + xAtacante + ", " + yAtacante);
                Debug.Log("xTarget, yTarget: " + xTarget + ", " + yTarget);
                Vector2 vecDif = new Vector2((x2 - xAtacante), (y2 - yAtacante));
                Debug.Log("vecDif: " + vecDif);

                float anguloVecDif = Vector2.Angle(vecDif, vecDifTarget);
                Debug.Log("anguloVecDif: " + anguloVecDif);

                

                if (Mathf.Abs(anguloVecDif) < anguloApertura / 2  &&  vecDif.magnitude < radio)                //Si dentro de RAdio
                {
                    Debug.Log("AÑADIENDO PERSONAJE " + personaje);
                    GameObject[] aux = (personajesDentro != null) ? personajesDentro.Clone() as GameObject[] : null;
                    if (aux != null)
                    {
                        personajesDentro = new GameObject[personajesDentro.Length + 1];

                        for (int i = 0; i < aux.Length; i++)
                        {
                            personajesDentro[i] = aux[i];
                        }
                        personajesDentro[personajesDentro.Length - 1] = personaje;
                    }
                    else
                    {
                        personajesDentro = new GameObject[1];
                        personajesDentro[0] = personaje;
                    }
                }
                //Se añade al vector de personajes de arriba
            }

        return personajesDentro;
    }



    void CrearProyectil(float critico,float physicalDamage, GameObject targetGO)
    {
        GameObject proyectil = Instantiate(stats.prefabProyectil, transform.parent.position, new Quaternion(0, 0, 0, 0)) as GameObject;
        if (transform.parent.GetComponentInChildren<Buffos>()) //PARA METERLE LOS ATRIBUTOS ADECUADOS AL PROYECTIL
        {
            transform.parent.GetComponentInChildren<Buffos>().proyectilAtaque = proyectil;
            stats.ComprobarBufProyectil();
        }

        MoverGOProyectil mover = proyectil.GetComponent<MoverGOProyectil>();
        TriggerAccionProyectil accion = proyectil.GetComponent<TriggerAccionProyectil>();

        accion.physicalDamage = physicalDamage;
        accion.critico = critico;
        accion.GoPadreCreadorDeEsteProyectil = transform.parent.gameObject;
        accion.enviarMensaje_ok = enviarMensaje_ok;

        if (GetComponent<TriggerAtaque>().soyEnemigo_ok)
            proyectil.layer = 12;
        else
            proyectil.layer = 11;

        mover.GOSeguir = targetGO;
        mover.t0SegundosActualizar = Time.realtimeSinceStartup;
        mover.vel = stats.speedProyectil;
        mover.posFinal = targetGO.transform.position;
        mover.t0 = Time.realtimeSinceStartup;
        mover.pos0 = transform.parent.position;
    }




    public void AnimacionesAtaque()
    {
        Animator animator = transform.parent.GetComponentInChildren<Animator>();
        if (animator)
            animator.SetTrigger("atacar");
    }



    void EnviarMensajeRivalMuerte()
    {
        string nombreEnemigo;
        int index = enemigoGO.name.IndexOf("E");
        if (index > -1)
            nombreEnemigo = enemigoGO.name.Substring(0, index);
        else
            nombreEnemigo = enemigoGO.name;

        string strPacket = "D" + nombreEnemigo;
        GameManager.Instance.EnviarPaquete(strPacket, true);
       // DatosAccionesMultijugador.MensajeNuevoRecibido("paquete env: " + strPacket);
    }


    void EnviarMensajeRivalAtaque(float danyo, bool criticoOk,float danyoEscudo) //o
    {
        if (GameManager.Instance && !GameManager.Instance.debugOk)
        {
            string nombre = transform.parent.gameObject.name;
            string nombreEnemigo;

            int index = enemigoGO.name.IndexOf("E");
            if (index > -1)
                nombreEnemigo = enemigoGO.name.Substring(0, index);
            else
                nombreEnemigo = enemigoGO.name;

            //Debug.Log(criticoOk);
            int criticoInt = criticoOk ? 1 : 0;


            string strPacket = "C" + nombre + "/" + nombreEnemigo + "|" + danyo.ToString() + "%" + criticoInt.ToString()+"$"+danyoEscudo;
            GameManager.Instance.EnviarPaquete(strPacket, true);
           // DatosAccionesMultijugador.MensajeNuevoRecibido("paquete env: " + strPacket);
        }

    }

    void EnviarRivalPrefab(string nombreSkill, GameObject tropa, GameObject atacante)
    {
            GameManager gameManager = GameManager.Instance;
            string nombrePj;
            int index = tropa.name.IndexOf("E");
            if (index > -1)
                nombrePj = tropa.name.Substring(0, index);
            else
                nombrePj = tropa.name;

            string paquete = "M" + nombrePj + "|" + nombreSkill + "+" + true+"%"+atacante.name;

            if (gameManager)
                gameManager.EnviarPaquete(paquete, true);


    }


    float ObtenerMultiplicador(float probCritico)
    {
        float multiplicador = 1f;

        if (Random.Range(0, 100) < probCritico)
        {
            multiplicador = 2;
            ataqueCriticoOk = true;
        }
        else
            ataqueCriticoOk = false;

        

        return multiplicador;
    }


    void ComprobarGanar(GameObject enemigoGO)
    {
        if (GameManager.Instance)
            if (enemigoGO.name == GameManager.Instance.jugadorEnemigo.name)
                GameManager.Instance.Ganar();
    }


    public void InstanciarIndicadorDanyo(float danyo, bool criticoOk)
    {
        if (indicadorDanyo  &&  danyo > 0)
        {
            GameObject nuevoIndicador = Instantiate(indicadorDanyo, indicadorDanyo.transform.position, indicadorDanyo.transform.rotation) as GameObject;
            nuevoIndicador.transform.SetParent(indicadorDanyo.transform.parent);
            nuevoIndicador.transform.position = indicadorDanyo.transform.position;
            nuevoIndicador.transform.localScale = indicadorDanyo.transform.localScale;

            nuevoIndicador.gameObject.SetActive(true);
            nuevoIndicador.GetComponent<Text>().text = "-" + danyo.ToString();
            if (criticoOk)
            {
                nuevoIndicador.GetComponent<Outline>().effectDistance *= 1.5f;
                nuevoIndicador.GetComponent<RectTransform>().sizeDelta *= 1.5f;
            }

        }
    }


    public void InstanciarIndicadorEscudo(float danyo, bool criticoOk)
    {
        if (indicadorEscudo  &&  danyo > 0)
        {
            Debug.Log("ENTRAA danyo 2: " + danyo);

            GameObject nuevoIndicador = Instantiate(indicadorEscudo, indicadorEscudo.transform.position, indicadorEscudo.transform.rotation) as GameObject;
            nuevoIndicador.transform.SetParent(indicadorEscudo.transform.parent);
            nuevoIndicador.transform.position = indicadorEscudo.transform.position;
            nuevoIndicador.transform.localScale = indicadorEscudo.transform.localScale;

            nuevoIndicador.gameObject.SetActive(true);
            nuevoIndicador.GetComponent<Text>().text = "-" + danyo.ToString();

            if (criticoOk)
            {
                nuevoIndicador.GetComponent<Outline>().effectDistance *= 1.5f;
                nuevoIndicador.GetComponent<RectTransform>().sizeDelta *= 1.5f;
            }

        }
    }



    public void ActivarColliders()
    {
        ReinicioDeAtaqueYArea();



        if (!transform.parent.GetComponent<InputMovimiento>())
        {
            GetComponent<ModificarRadio>().enabled = true;

            if (!GetComponent<TriggerAtaque>().soyTower_ok)
            {
                transform.parent.GetChild(2).GetComponent<ModificarRadio>().enabled = true;
                transform.parent.GetComponentInChildren<TriggerAreaVision>().BuscarSiguientePersonajeASeguir();
            }

            transform.parent.GetComponentInChildren<TriggerAtaque>().BuscarSiguientePersonajeAAtacar();

        }
        else
        {
            transform.parent.GetComponentInChildren<TriggerAtaque>().BuscarSiguientePersonajeAAtacar();

        }


    }


    public void ReinicioDeAtaqueYArea()
    {
        GetComponent<TriggerAtaque>().objetivo = null;
        enemigoGO = null;
        atacando_ok = false;
        if (!GetComponent<TriggerAtaque>().soyTower_ok)
        {
            //Debug.Log("Entra en el reinicio ataque la parte de area: ");
            transform.parent.GetComponentInChildren<TriggerAreaVision>().persiguiendo_ok = false;
            transform.parent.GetComponentInChildren<TriggerAreaVision>().objetivo = null;
        }

    }

}
