using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriggerAccionProyectil : MonoBehaviour {

    //public bool parada_ok;
    //public bool bloqueo_ok;
    public float critico;
    public float physicalDamage;
    //public float danyoEscudo;
    public GameObject GoPadreCreadorDeEsteProyectil;
    public bool enviarMensaje_ok;

    private Atacar AtaqueEnemigo;
    private Atacar AtaqueAliado;

    public GameObject prefabExplosion;

    public bool accionConstanteAlColisionar;
    public GameObject prefabColisionConstante;

    public Efecto efectoPrefab;

    [HideInInspector]
    public string[] nombreHabilidad = new string[25];
    [HideInInspector]
    public string[] statAumento = new string[25];
    [HideInInspector]
    public float[] statAumentovalue = new float[25];
    [HideInInspector]
    public float[] statAumentoDuracion = new float[25];
    [HideInInspector]
    public float[] statAumentoT0 = new float[25];


    void AñadirBuffs()
    {
        GameObject enemigo = GetComponent<MoverGOProyectil>().GOSeguir;

        for (int i = 0; nombreHabilidad[i]!=""; i++)
        {
            enemigo.GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(nombreHabilidad[i], statAumento[i], statAumentovalue[i], statAumentoDuracion[i], Time.realtimeSinceStartup);
            if (enviarMensaje_ok)
                EnviarRivalStat(nombreHabilidad[i],statAumento[i],statAumentovalue[i],statAumentoDuracion[i],enemigo);
        }
    }

    void EnviarRivalStat(string nombre, string tipo, float value, float duracion, GameObject pj,bool enemigo_ok=true)
    {
        string nombrePj;
        int index = pj.name.IndexOf("E");
        if (index > -1)
            nombrePj = pj.name.Substring(0, index);
        else
            nombrePj = pj.name;


        string paquete = "K" + nombrePj + "/" + nombre + "|" + tipo + "@" + value.ToString() + "&" + duracion.ToString() + "%" + (Time.realtimeSinceStartup - DatosAccionesMultijugador.t0Room).ToString() + "+" + enemigo_ok;
    }


    void OnTriggerEnter2D(Collider2D other)
    {

       if(accionConstanteAlColisionar && other.gameObject.tag=="PERSONAJES"/* && other.gameObject.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok*/) 
        {
            GameObject efecto = Instantiate(prefabColisionConstante, other.gameObject.transform.position, prefabColisionConstante.transform.rotation, other.gameObject.transform);       
            
            if(other.gameObject.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok)
            {
                ComprobarMuerteEnemigo(other.gameObject);
            }
        }
 


        if (other.gameObject.name == GetComponent<MoverGOProyectil>().GOSeguir.name)
        {
            if (enviarMensaje_ok)
            {
                if(efectoPrefab)
                    efectoPrefab.Activar();
                AñadirBuffs();
                ComprobarMuerteEnemigo();
            }

            if (prefabExplosion)
                Instantiate(prefabExplosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }



    void EnviarMensajeRivalAtaque(float danyo, bool criticoOk, float danyoEscudo, GameObject enemigoGO) //o
    {
        
        if (GameManager.Instance && !GameManager.Instance.debugOk)
        {
            string nombre = GoPadreCreadorDeEsteProyectil.name;
            string nombreEnemigo;

            int index = enemigoGO.name.IndexOf("E");
            if (index > -1)
                nombreEnemigo = enemigoGO.name.Substring(0, index);
            else
                nombreEnemigo = enemigoGO.name;

            int criticoInt = criticoOk ? 1 : 0;
            string bla;
            float ble = 2;
           
            string strPacket = "C" + nombre + "/" + nombreEnemigo + "|" + danyo.ToString() + "%" + criticoInt.ToString() + "$" + danyoEscudo.ToString();
            Debug.Log("strPacket: " + strPacket);
            GameManager.Instance.EnviarPaquete(strPacket, true);
        }

    }



    void ComprobarMuerteEnemigo(GameObject enemigo=null)
    {
        if(enemigo==null)
            enemigo = GetComponent<MoverGOProyectil>().GOSeguir;

        AtaqueEnemigo = enemigo.GetComponentInChildren<Atacar>();

        if (AtaqueAliado == null && GoPadreCreadorDeEsteProyectil != null)
            AtaqueAliado = GoPadreCreadorDeEsteProyectil.GetComponentInChildren<Atacar>();
        else
            AtaqueAliado = AtaqueEnemigo; //Puesto que solo se utiliza para usar las función internas de Stats no hay problema con que si no existe el aliado se utilice el stats del Enemigo

        float damageRealizado = 0;
        float damageEscudoRealizado = 0;
        float damageSinEscudo;
        bool ataqueCriticoOk;


         ataqueCriticoOk = AtaqueAliado.stats.ComprobarCritico(critico); //Esta función realmente la hace el aliado, le pasamos los valores del aliado, el problema es que el aliado que lanzó el ataque puede estar muerto, por eso utilizo el Stats del enemigo

         damageSinEscudo = AtaqueAliado.stats.CalcularDanyo(AtaqueEnemigo.stats.armor+ AtaqueEnemigo.stats.ComprobarHabilidadesAumentoArmor(), AtaqueEnemigo.stats.groupID, ataqueCriticoOk,physicalDamage);//Esta función realmente la hace el aliado, le pasamos los valores del aliado, el problema es que el aliado que lanzó el ataque puede estar muerto, por eso utilizo el Stats del enemigo
    
         damageRealizado = AtaqueEnemigo.stats.UsoEscudo(damageSinEscudo); //aplicamos el escudo del rival

         damageEscudoRealizado = damageSinEscudo - damageRealizado;


       AtaqueEnemigo.InstanciarIndicadorDanyo(damageRealizado, ataqueCriticoOk);

        if (damageEscudoRealizado > 0)
        {
            Debug.Log("ENTRAA damageEscudoRealizado: " + damageEscudoRealizado);
            AtaqueEnemigo.stats.escudoActual -= damageEscudoRealizado;
            AtaqueEnemigo.InstanciarIndicadorEscudo (damageEscudoRealizado, ataqueCriticoOk);
        }

        AtaqueEnemigo.stats.health -= damageRealizado;

        EnviarMensajeRivalAtaque(damageRealizado, ataqueCriticoOk, damageEscudoRealizado, enemigo);

        if (AtaqueEnemigo.stats.health <= 0)
            {
                if (AtaqueEnemigo.transform.parent.GetComponentInChildren<Animator>())
                    AtaqueEnemigo.transform.parent.GetComponentInChildren<Animator>().SetTrigger("morir");
                if (AtaqueAliado != null)
                    AtaqueAliado.atacando_ok = false;
                if (enemigo.transform.GetChild(0).GetChild(0)  && enemigo.transform.GetChild(0).GetChild(0).GetComponent<Text>())
                    enemigo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";

                EnviarMensajeRivalMuerte(enemigo);

                ComprobarGanar(enemigo);

                string nombreEnemigoAux = enemigo.name;

                    Destroy(enemigo, 0.2f);
                    enemigo.GetComponent<CircleCollider2D>().enabled = false;


               if (AtaqueAliado)
                 AtaqueAliado.ActivarColliders();
            }
            else
            {
            if (enemigo.transform.GetChild(0).GetChild(0) && enemigo.transform.GetChild(0).GetChild(0).GetComponent<Text>())
                enemigo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = AtaqueEnemigo.stats.health.ToString();
            }
    }



    void EnviarMensajeRivalMuerte(GameObject enemigo)
    {
        string nombreEnemigo;
        int index = enemigo.name.IndexOf("E");
        if (index > -1)
            nombreEnemigo = enemigo.name.Substring(0, index);
        else
            nombreEnemigo = enemigo.name;

        string strPacket = "D" + nombreEnemigo;
        GameManager.Instance.EnviarPaquete(strPacket, true);
        DatosAccionesMultijugador.MensajeNuevoRecibido("paquete env: " + strPacket);
    }


    void ComprobarGanar(GameObject enemigoGO)
    {
        if (GameManager.Instance)
            if (enemigoGO.name == GameManager.Instance.jugadorEnemigo.name)
                GameManager.Instance.Ganar();
    }


  /*  void ComprobarGanarPerder(GameObject enemigoGO)
    {
        FaseJuego fase = GameObject.Find("Suelo").GetComponent<FaseJuego>();

        if (enemigoGO.GetComponentInChildren<Atacar>().soyTower_ok && (enemigoGO.name.IndexOf("Base") != -1))//si el rival es una torre && es una Base 
        {
            if (enemigoGO.GetComponentInChildren<Atacar>().colorBola != colorBola)
            {
                DatosAccionesMultijugador.MensajeNuevoRecibido("GANA");

                GameManager.Instance.Ganar();
            }
            else
            {
                DatosAccionesMultijugador.MensajeNuevoRecibido("PIERDE");

                GameManager.Instance.Perder();
            }
        }
        else if (enemigoGO.GetComponentInChildren<Atacar>().soyTower_ok)
        {
            if (enemigoGO.GetComponentInChildren<Atacar>().colorBola == "ROJO")
            {
                if (!fase.puntoTactico2.activeInHierarchy)
                {
                    fase.puntoTactico2.SetActive(true);

                    if (enemigoGO.GetComponentInChildren<Atacar>().colorBola != IniciarDatos.Instance.datos.Equipo)
                    {
                        DatosAccionesMultijugador.AliadoHaCapturado();

                        if (GameManager.Instance != null)
                            GameManager.Instance.EnviarPaquete("F2/" + (Time.realtimeSinceStartup - DatosAccionesMultijugador.t0Room).ToString() + "|2", true);
                    }
                }
            }
            else
            {
                if (!fase.puntoTactico0.activeInHierarchy)
                {
                    fase.puntoTactico0.SetActive(true);

                    if (enemigoGO.GetComponentInChildren<Atacar>().colorBola != IniciarDatos.Instance.datos.Equipo)
                    {
                        DatosAccionesMultijugador.AliadoHaCapturado();

                        if (GameManager.Instance != null)
                            GameManager.Instance.EnviarPaquete("F2/" + (Time.realtimeSinceStartup - DatosAccionesMultijugador.t0Room).ToString() + "|0", true);
                    }

                }
            }
        }

    }*/


}
