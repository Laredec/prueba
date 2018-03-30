using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Stats : MonoBehaviour{

    public int ID;
    public string name;
    public float movSpeed;
    public string iconName;
    public string bigIcon;
    public float size;
    public float health;
    public float healthInicial;
    public float armor;
    public float attackSpeed;
    public float rangeAttack;
    public float rangeVision;
    public float armorPenetration;
    public float critico;
    public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        public float sizeProyectil;
    public int groupID;
    public int[] groupsStrong;//variable que dice contra quienes es fuerte esta unidad
        public float[] strongDMG;//Esta variable nos dice cuanto de fuerte es la variable de arriba
    public int[] groupsIgnore;
 /*   public bool charge;
        public float chargeDMG;
        public float chargeExtraSpeed;
        public float segTimeToCharge;*/
    public float physicalDamage;
    public float magicalDamage;
    /// <summary>
    /// VALORES INTERNOS
    /// </summary>
    public float escudoActual;
    [HideInInspector]
    public bool stun_ok;
    [HideInInspector]
    public float timeStun;
    [HideInInspector]
    public float t0Stun;

    public float physicalDamageModificate;
    public float armorModificate;
    public float attackSpeedModificate;
    public float movSpeedModificate;
    public float criticoModificate;

    void Start()
    {
        healthInicial = health;
    }


    void Update()
    {
        if (stun_ok)
        {
            GetComponentInChildren<Atacar>().enabled = false;
            GetComponentInChildren<TriggerAtaque>().enabled = false;

            if (Time.realtimeSinceStartup - t0Stun > timeStun)
            {
                if (gameObject.name.Substring(0, 1) != "0")
                {
                    GetComponentInChildren<Atacar>().enabled = true;
                    GetComponentInChildren<TriggerAtaque>().enabled = true;
                }
                GetComponentInChildren<Atacar>().t0ultimoAtaque += timeStun;
                stun_ok = false;
                GetComponentInChildren<MoverGO>().t0 = Time.realtimeSinceStartup;
            }
            GetComponentInChildren<Animator>().SetBool("stuneandoseOk", true);
        }
        else
            GetComponentInChildren<Animator>().SetBool("stuneandoseOk", false);


    }

    public float ComprobarGroupStrong(int IDEnemigo)
    {
        for(int i=0;i<groupsStrong.Length;i++)
        {
            if (groupsStrong[i] == IDEnemigo)
                return strongDMG[i];
        }

        return 1f;
    }


    /*    public bool ComprobarParada()
        {
            int numRand = Random.Range(0, 99);

            if (numRand >= parry)
                return false;
            else
                return true;
        }*/


    /*  public bool ComprobarBloqueo()
      {
          int numRand = Random.Range(0, 99);

          if (numRand >= block)
              return false;
          else
              return true;
      }*/


        
    public bool ComprobarCritico(float crit)
    {
        int numRand = Random.Range(0, 99);

        if (numRand >= crit)
            return false;
        else
            return true;
    }


    public float CalcularDanyoProyectil()
    {
        float damageFinal;

        damageFinal = physicalDamage;

        if (ID == 5)// Es decir si soy Akiles
            damageFinal += ComprobarHabilidadesAumentoPhysicalDamage();

        if (damageFinal <= 0)//condicion de que el daño físico no puede ser menor que 0
            damageFinal = 1;

        return damageFinal;
    }


    public float CalcularCriticoProyectil()
    {
        float criticoFinal;

        criticoFinal = critico;

        if (ID == 5)// Es decir si soy Akiles
            criticoFinal += ComprobarHabilidadesAumentoCritico();

        if (criticoFinal < 0)//condicion de que el crítico no puede ser menor que 0
            criticoFinal = 0;

        return criticoFinal;
    }



    public float CalcularDanyo(float armaduraEnemigo, int groupIDEnemigo,bool critico_Ok,float phyDam)
    {
        float damageFinal;
        float multiplicador;

        damageFinal = phyDam - ComprobarPenetracionArmadura(armaduraEnemigo);

        if (damageFinal <= 0)//condicion de que el daño físico no puede ser menor que 0
            damageFinal = 1;

        multiplicador = ComprobarGroupStrong(groupIDEnemigo);
        damageFinal = multiplicador * damageFinal;

        if(critico_Ok)
            damageFinal = 2 * damageFinal;


        return damageFinal;
    }

    public void Curar(float cura)
    {
        health += cura;

        if (health + cura > healthInicial)
            health = healthInicial;
    }

    public void ComprobarModificacionBuffos()
    {
        physicalDamageModificate = 0;
        armorModificate = 0;
        attackSpeedModificate = 0;
        movSpeedModificate = 0;
        criticoModificate = 0;

        if (GetComponentInChildren<Buffos>())
            for (int i = 0; nombreHabilidad[i] != null; i++)
            {
                GetComponentInChildren<Buffos>().Invoke("AumentaPhysicalDamage" + nombreHabilidad[i], 0f);
                GetComponentInChildren<Buffos>().Invoke("AumentaArmor" + nombreHabilidad[i], 0f);
                GetComponentInChildren<Buffos>().Invoke("AumentaCritico" + nombreHabilidad[i], 0f);
                GetComponentInChildren<Buffos>().Invoke("AumentaAttackSpeed" + nombreHabilidad[i], 0f);
                GetComponentInChildren<Buffos>().Invoke("AumentaMovSpeed" + nombreHabilidad[i], 0f);
            }

    }

    public float ComprobarHabilidadesAumentoPhysicalDamage()
    {
        float dañoDevuelto;

        dañoDevuelto = physicalDamage + physicalDamageModificate;

        dañoDevuelto += ComprobarHabilidades("physicalDamage"); // esto es para los buffos de todas las tropas en general, no buffos únicamente de Akiles

        if (dañoDevuelto <= 0)
            dañoDevuelto = 0;


        return dañoDevuelto;
    }

    public float ComprobarHabilidadesAumentoArmor()
    {
        float armorDevuelto;

        armorDevuelto = armor + armorModificate;

        armorDevuelto += ComprobarHabilidades("armor"); // esto es para los buffos de todas las tropas en general, no buffos únicamente de Akiles
        if (armorDevuelto <= 0)
            armorDevuelto = 0;

        return armorDevuelto;
    }



    public float ComprobarHabilidadesAumentoCritico()
    {
        float criticoDevuelto;


        criticoDevuelto = critico +criticoModificate; //En caso de ser 0 significa que Akiles no tiene ningun Buf

        criticoDevuelto += ComprobarHabilidades("critico"); // esto es para los buffos de todas las tropas en general, no buffos únicamente de Akiles

        if (criticoDevuelto <= 0)
            criticoDevuelto = 0;

        return criticoDevuelto;
    }



    public float ComprobarHabilidadesAumentoAttackSpeed()
    {
        float attackSpeedDevuelta;


        attackSpeedDevuelta = attackSpeed + attackSpeedModificate;

        attackSpeedDevuelta += ComprobarHabilidades("attackSpeed"); // esto es para los buffos de todas las tropas en general, no buffos únicamente de Akiles

        if (attackSpeedDevuelta <= 0)
            attackSpeedDevuelta = 0;

        return attackSpeedDevuelta;
    }


    public float ComprobarHabilidadesAumentoMovSpeed()
    {
        float movSpeedDevuelta;

        movSpeedDevuelta = movSpeed + movSpeedModificate;

        movSpeedDevuelta += ComprobarHabilidades("movSpeed"); // esto es para los buffos de todas las tropas en general, no buffos únicamente de Akiles

        if (movSpeedDevuelta <= 0)
            movSpeedDevuelta = 0;

        return movSpeedDevuelta;
    }


    public void ComprobarBufProyectil()
    {
        if (GetComponentInChildren<Buffos>())
            for (int i = 0; nombreHabilidad[i] != null; i++)
            {
                GetComponentInChildren<Buffos>().Invoke("AñadirBufProyectil" + nombreHabilidad[i], 0f);
                GetComponentInChildren<Buffos>().Invoke("DisminuirUso" + nombreHabilidad[i], 0f);
            }
                

    }

    public float UsoEscudo(float damage)
    {
        float damageReal = damage;

        if(escudoActual>0)
        {
            damageReal = damage - escudoActual;
            if (damageReal < 0)
                damageReal = 0;

            ////escudoActual = escudoActual - damage; //el escudo se baja despues
            ////if (escudoActual < 0)
                ////escudoActual = 0;

        }

        return damageReal;

    }




    public float ComprobarPenetracionArmadura(float armaduraEnemigo)
    {
        if (armaduraEnemigo - armorPenetration > 0)
            return armaduraEnemigo - armorPenetration;
        else
            return 0f;
    }



    public bool ComprobarSiIgnoramos(int groupIDEnemigo)
    {
        for(int i =0; i< groupsIgnore.Length; i++)
        {
            if(groupsIgnore[i]== groupIDEnemigo)
                return true;
        }

        return false;
    }


    ///<summary>
    ///Aqui están los stats que tienen que ver con las habilidades
    /// </summary>

    public string[] nombreHabilidad = new string[25];
    public string[] statAumento = new string[25];
    public float[] statAumentovalue = new float[25];
    public float[] statAumentoDuracion = new float[25];
    public float[] statAumentoT0 = new float[25];


    public void AnyadirStatHabilidad(string nombre, string statAumenta, float value, float tiempo, float t0)
    {
        //EnviarRivalStat(nombre, statAumenta, value, tiempo, t0);

        if (GetComponentInChildren<Buffos>())
        {
            GetComponentInChildren<Buffos>().Invoke("Añadir" + nombre, -1f);
            Debug.LogWarning("Añadir" + nombre);
        }

        int acc = 0;
        for (int i = 0; nombreHabilidad[i] != null; i++)
            acc++;

        nombreHabilidad[acc] = nombre;
        statAumento[acc] = statAumenta;
        statAumentovalue[acc] = value;
        statAumentoDuracion[acc] = tiempo;
        statAumentoT0[acc] = t0;

        Invoke("EliminarHabilidadesPorDuracion", (t0 + tiempo + 0.1f) - Time.realtimeSinceStartup);

        if (GetComponentInChildren<Buffos>())
            GetComponentInChildren<Buffos>().Invoke(nombre, 0f);

        Invoke("ComprobarModificacionBuffos",0f);

    }


 /*   public void EnviarRivalStat(string nombre, string statAumenta, float value, float tiempo, float t0)
    {
        //NOMBRE 

    }*/

    public void EliminarHabilidadesPorDuracion()
    {
        for (int i = 0; nombreHabilidad[i] != null; i++)
            if (ComprobarTiempoBorrar(i))
            {
                EliminarHabilidad(i);
                i--;
            }
    }


    public void EliminarHabilidadesPorNombre(string nombreHab,string nombreHabActual="none")
    {
        for (int i = 0; nombreHabilidad[i] != null; i++)
            if (nombreHabilidad[i] == nombreHab && nombreHabilidad[i]!=nombreHabActual)
            {
                EliminarHabilidad(i);
                i--;
            }
    }

    public void EliminarHabilidadesPorStatAumenta(string stat, string nombreHabActual = "none")
    {
        for (int i = 0; nombreHabilidad[i] != null; i++)
            if (statAumento[i] == stat && nombreHabilidad[i] != nombreHabActual)
            {
                EliminarHabilidad(i);
                i--;
            }
    }

    public void EliminarHabilidad(int id)
    {
        if (GetComponentInChildren<Buffos>())
        {
            GetComponentInChildren<Buffos>().Invoke("Eliminar" + nombreHabilidad[id], 0f);
            Debug.LogWarning("Eliminar" + nombreHabilidad[id]);
        }

        for (int i = id; nombreHabilidad[i] != null; i++)
        {
            if (nombreHabilidad[i + 1] != null)
            {
                nombreHabilidad[i] = nombreHabilidad[i + 1];
                statAumento[i] = statAumento[i + 1];
                statAumentovalue[i] = statAumentovalue[i + 1];
                statAumentoDuracion[i] = statAumentoDuracion[i + 1];
                statAumentoT0[i] = statAumentoT0[i + 1];
            }
            else
            {
                nombreHabilidad[i] = null;//ESTO ES PARA ELIMiNAR LA ULTIMA HABILIDAD Y QUE NO ESTË REPETIDA, PUESTO QUE LA ULTIMA SE HA PUESTO EN LA ULTIMA -1
                statAumento[i] = null;
                statAumentovalue[i] = 0;
                statAumentoDuracion[i] = 0;
                statAumentoT0[i] = 0;
            }
        }
        Invoke("ComprobarModificacionBuffos", 0f);

    }

    public bool ComprobarTiempoBorrar(int id)
    {
        bool borrar = false;

        if (Time.realtimeSinceStartup - statAumentoT0[id] > statAumentoDuracion[id])
            borrar = true;

        return borrar;
    }


    public float ComprobarHabilidades(string statBuscado)
    {
        float aumento = 0;
        for (int i = 0; nombreHabilidad[i] != null  && nombreHabilidad[i] != ""; i++)
        {
            if (statAumento[i] == statBuscado)
                aumento += statAumentovalue[i];
        }
        return aumento;
    }


    public bool ComprobarHabilidadesPorNombre(string nombreBuscado)
    {
        bool existe = false;

        if(nombreHabilidad.Length>0)
            for (int i = 0; nombreHabilidad[i] != null; i++)
            {
                if (nombreHabilidad[i] == nombreBuscado)
                    existe = true;
            }

        return existe;
    }


}