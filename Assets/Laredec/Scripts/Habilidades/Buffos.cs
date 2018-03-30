using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffos : MonoBehaviour
{

    /// Variables de contacto con otras clases
    [HideInInspector]
    public bool soyEnemigo_ok; //esta variables se utiliza para comprobar si eres aliado o enemigo.
    [HideInInspector]
    public GameObject proyectilAtaque; //Esta variable se utiliza cada vez que se lanza un proyectil para definir valores, se usa tanto localmente como remotamente
    [HideInInspector]
    public GameObject PjObjectivo;// Esta variable es para indicar el objetivo del ataque, se usa solo desde el lado del Enemigo.


    /// Variables de uso en este script, arrastrar GO en el Editor
    public Atacar atacar;
    public TriggerAtaque triggerAtaque;
    public GameObject Skills;
    [HideInInspector]
    public Stats stats;
    [HideInInspector]
    public GameManager gameManager;


    ///Clases de los buffs
    public Hacha hacha;
    [Space(10, order = 0)]

    public LanzaHacha lanzaHacha;
    [Space(10, order = 1)]

    public Gorro gorro;
    [Space(10, order = 2)]

    public GolpeDobleGorro golpeDobleGorro;
    [Space(10, order = 3)]

    public GorroHielo gorroHielo;
    [Space(10, order = 4)]

    public BuffInicialGorroHieloTropa buffInicialGorroHielo;
    [Space(10, order = 5)]

    public RelentizacionGorroHieloTropa relentizacionGorroHielo;
    [Space(10, order = 6)]

    public CuboAgua cuboAgua;
    [Space(10, order = 7)]

    public LanzaCuboAgua lanzaCuboAgua;
    [Space(10, order = 8)]

    public FraseMotivacional fraseMotivacional;
    [Space(10, order = 9)]

    public BufFraseMotivacionalTropa bufFraseMotivacional;
    [Space(10, order = 10)]

    public SustanciaX sustanciaX;
    [Space(10, order = 11)]

    public ReyDelTrueno reyDelTrueno;
    [Space(10, order = 12)]

    public BolaDeNieve bolaDeNieve;
    [Space(10, order = 13)]

    public Cohete cohete;
    [Space(10, order = 14)]

    public ExplosionTrampaGigantes explosionTrampaGigantes;
    [Space(10, order = 15)]

    public Proyectiles proyectiles;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if (!stats)
        {
            stats = atacar.stats;
            soyEnemigo_ok = atacar.GetComponent<TriggerAtaque>().soyEnemigo_ok;
            gameManager = GameManager.Instance;

        }
    }


    //ARMA HACHA
    [Serializable]
    public struct Hacha
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        // public int size;
        public float attackSpeed;
        public float rangeAttack;
        public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        // public float sizeProyectil;
        public float physicalDamage;
        public float duracion;
    }

    public void AñadirHachaAkiles()
    {
        stats = atacar.stats;
        stats.EliminarHabilidadesPorStatAumenta("arma", hacha.nombre);//Como el Invoke se hace al final de la iteración aquí se elimina el propio arma También, por eso lo añado de nuevo
        stats.EliminarHabilidadesPorStatAumenta("complementoArma");
    }

    public void AumentaPhysicalDamageHachaAkiles()
    {
        stats.physicalDamageModificate += hacha.physicalDamage;
    }


    public void AumentaAttackSpeedHachaAkiles()
    {
        stats.attackSpeedModificate += hacha.attackSpeed;
    }

    public void HachaAkiles()
    {
        stats.melee = hacha.melee;
        stats.prefabProyectil = hacha.prefabProyectil;
        stats.speedProyectil = hacha.speedProyectil;

        GetComponent<ModificarRadio>().metros = hacha.rangeAttack;
        GetComponent<ModificarRadio>().enabled = true;
        //Debug.Log(hacha.rangeAttack);
        atacar.enabled = true;
        triggerAtaque.enabled = true;
    }

    public void EliminarHachaAkiles()
    {
        stats.AnyadirStatHabilidad(lanzaHacha.nombre, lanzaHacha.tipo, lanzaHacha.physicalDamage, lanzaHacha.duracion, Time.realtimeSinceStartup); //Añadimos el Lanza Hacha
    }


    //COMPLEMENTO ARMA LANZA HACHA

    [Serializable]
    public struct LanzaHacha
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        //public int size;
        public float attackSpeed;
        //public float rangeVision;
        public float rangeAttack;
        public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        //public float sizeProyectil;
        public float physicalDamage;
        public float duracion;
        public float cantUsos;
        [HideInInspector]
        public float usosBase;

    }



    public void AumentaPhysicalDamageLanzaHachaAkiles()
    {
        stats.physicalDamageModificate += lanzaHacha.physicalDamage;
    }


    public void DisminuirUsoLanzaHachaAkiles()
    {
        lanzaHacha.cantUsos--;
        if (lanzaHacha.cantUsos <= 0)
            stats.EliminarHabilidadesPorNombre(lanzaHacha.nombre); //AQUI SE ACABA DE UTILIZAR POR LO QUE ELIMINAMOS EL STAT

    }

    public void AumentaAttackSpeedLanzaHachaAkiles()
    {
        stats.attackSpeedModificate += lanzaHacha.attackSpeed;
    }


    public void LanzaHachaAkiles()
    {
        lanzaHacha.usosBase = lanzaHacha.cantUsos;
        stats.melee = false;
        stats.prefabProyectil = lanzaHacha.prefabProyectil;
        stats.speedProyectil = lanzaHacha.speedProyectil;

        GetComponent<ModificarRadio>().metros = lanzaHacha.rangeAttack;
        GetComponent<ModificarRadio>().enabled = true;
    }

    public void EliminarLanzaHachaAkiles()
    {
        Skills.transform.Find("Hab" + lanzaHacha.idSkill.ToString()).GetComponent<SkillArma>().DesAplicarSkill();
        atacar.enabled = false;
        triggerAtaque.enabled = false;
        lanzaHacha.cantUsos = lanzaHacha.usosBase;

    }



    //ARMA GORRO FUEGO 
    [Serializable]
    public struct Gorro
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        //public int size;
        public float attackSpeed;
        public float rangeAttack;
        public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        //public float sizeProyectil;
        public float physicalDamage;
        public float duracion;
    }


    public void AñadirGorroAkiles()
    {
        stats = atacar.stats;
        stats.EliminarHabilidadesPorStatAumenta("arma", gorro.nombre);
        stats.EliminarHabilidadesPorStatAumenta("complementoArma");
        stats.AnyadirStatHabilidad(golpeDobleGorro.nombre, golpeDobleGorro.tipo, golpeDobleGorro.rangeAttack, golpeDobleGorro.duracion, Time.realtimeSinceStartup);
    }

    public void AumentaPhysicalDamageGorroAkiles()
    {
        stats.physicalDamageModificate += gorro.physicalDamage;
    }
    public void AumentaAttackSpeedGorroAkiles()
    {
        stats.attackSpeedModificate += gorro.attackSpeed;
    }

    public void GorroAkiles()
    {
        stats.melee = gorro.melee;
        stats.prefabProyectil = gorro.prefabProyectil;
        stats.speedProyectil = gorro.speedProyectil;

        GetComponent<ModificarRadio>().metros = gorro.rangeAttack;
        Debug.Log(gorro.rangeAttack);
        GetComponent<ModificarRadio>().enabled = true;

        atacar.enabled = true;
        triggerAtaque.enabled = true;
    }

    public void EliminarGorroAkiles()
    {
        Skills.transform.Find("Hab" + gorro.idSkill.ToString()).GetComponent<SkillArma>().DesAplicarSkill();
        stats.EliminarHabilidadesPorNombre(golpeDobleGorro.nombre); //En caso de que no se haya utilizado lo eliminamos ahora
        atacar.enabled = false;
        triggerAtaque.enabled = false;
    }



    //GOLPE DOBLE GORRO FUEGO
    [Serializable]
    public struct GolpeDobleGorro
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        //public int size;
        public float attackSpeed;
        public float rangeAttack;
        public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        //public float sizeProyectil;
        public float physicalDamage;
        public float duracion;
        public float cantUsos;
        [HideInInspector]
        public float usosBase;
    }


    public void AumentaPhysicalDamageGolpeDobleGorroAkiles()
    {
        stats.physicalDamageModificate += golpeDobleGorro.physicalDamage;
    }

    public void DisminuirUsoGolpeDobleGorroAkiles()
    {
        golpeDobleGorro.cantUsos--;
        if (golpeDobleGorro.cantUsos <= 0)
            stats.EliminarHabilidadesPorNombre(golpeDobleGorro.nombre); //AQUI SE ACABA DE UTILIZAR POR LO QUE ELIMINAMOS EL STAT
    }

    public void AumentaAttackSpeedGolpeDobleGorroAkiles()
    {
        stats.attackSpeedModificate += golpeDobleGorro.attackSpeed;
    }

    public void GolpeDobleGorroAkiles()
    {
        golpeDobleGorro.usosBase = golpeDobleGorro.cantUsos;
        stats.melee = golpeDobleGorro.melee;
        stats.prefabProyectil = golpeDobleGorro.prefabProyectil;
        stats.speedProyectil = golpeDobleGorro.speedProyectil;

        //GetComponent<ModificarRadio>().metros = golpeDobleGorro.rangeAttack;
        //GetComponent<ModificarRadio>().enabled = true;

        atacar.enabled = true;
        triggerAtaque.enabled = true;

    }

    public void EliminarGolpeDobleGorroAkiles()
    {
        stats.melee = gorro.melee;
        stats.prefabProyectil = gorro.prefabProyectil;
        stats.speedProyectil = gorro.speedProyectil;
        golpeDobleGorro.cantUsos = golpeDobleGorro.usosBase;

    }


    //ARMA GORRO HIELO 
    [Serializable]
    public struct GorroHielo
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        //public int size;
        public float attackSpeed;
        public float rangeAttack;
        public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        //public float sizeProyectil;
        public float physicalDamage;
        public float duracion;
        public float radio;
    }


    public void AñadirGorroHieloAkiles()
    {
        stats = atacar.stats;

        stats.EliminarHabilidadesPorStatAumenta("arma", gorroHielo.nombre);
        stats.EliminarHabilidadesPorStatAumenta("complementoArma");

        if (!soyEnemigo_ok)
        {

            GameObject[] dentro = ComprobarPersonajesDentroRadio(gorroHielo.radio);

            if (dentro != null)
                for (int i = 0; i < dentro.Length; i++)
                {
                    for (int j = 0; j < buffInicialGorroHielo.tipo.Length; j++)
                    {
                        dentro[i].GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(buffInicialGorroHielo.nombre[j], buffInicialGorroHielo.tipo[j], buffInicialGorroHielo.value[j], buffInicialGorroHielo.duracion[j], Time.realtimeSinceStartup);
                        EnviarRivalStat(buffInicialGorroHielo.nombre[j], buffInicialGorroHielo.tipo[j], buffInicialGorroHielo.value[j], buffInicialGorroHielo.duracion[j], dentro[i]);
                    }
                }
        }
    }

    public void AñadirBufProyectilGorroHieloAkiles()
    {
        TriggerAccionProyectil accion = proyectilAtaque.GetComponent<TriggerAccionProyectil>();

        for (int i = 0; i < relentizacionGorroHielo.tipo.Length; i++)
        {
            accion.nombreHabilidad[i] = relentizacionGorroHielo.nombre[i];
            accion.statAumento[i] = relentizacionGorroHielo.tipo[i];
            accion.statAumentovalue[i] = relentizacionGorroHielo.value[i];
            accion.statAumentoDuracion[i] = relentizacionGorroHielo.duracion[i];
            accion.statAumentoT0[i] = Time.realtimeSinceStartup;
        }
    }


    public void AumentaPhysicalDamageGorroHieloAkiles()
    {
        stats.physicalDamageModificate += gorroHielo.physicalDamage;
    }

    public void AumentaAttackSpeedGorroHieloAkiles()
    {
        stats.attackSpeedModificate += gorroHielo.attackSpeed;
    }

    public void GorroHieloAkiles()
    {
        stats.melee = gorroHielo.melee;
        GetComponent<ModificarRadio>().metros = gorroHielo.rangeAttack;

        GetComponent<ModificarRadio>().enabled = true;

        atacar.enabled = true;
        triggerAtaque.enabled = true;

    }

    public void EliminarGorroHieloAkiles()
    {
        Skills.transform.Find("Hab" + gorroHielo.idSkill.ToString()).GetComponent<SkillArma>().DesAplicarSkill();
        atacar.enabled = false;
        triggerAtaque.enabled = false;
    }


    //RELENTIZACION GORRO HIELO 
    [Serializable]
    public struct RelentizacionGorroHieloTropa
    {
        public int idSkill;
        public string[] nombre;
        public string[] tipo;
        public float[] value;
        public float[] duracion;
    }


    //Relentizacion/Daño Inicial Gorro hielo
    [Serializable]
    public struct BuffInicialGorroHieloTropa
    {
        public int idSkill;
        public string[] nombre;
        public string[] tipo;
        public float[] value;
        public float[] duracion;
    }


    //ARMA CUBO DE AGUA
    [Serializable]
    public struct CuboAgua
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        public float healSpeed;
        public float rangeHeal;
        public float lifeHeal;
        public float movSpeed;
        public float duracion;
        public GameObject prefabEstetico;
    }


    public void AñadirCuboAguaAkiles()
    {
        stats = atacar.stats;

        stats.EliminarHabilidadesPorStatAumenta("arma", cuboAgua.nombre);
        stats.EliminarHabilidadesPorStatAumenta("complementoArma");

        atacar.enabled = false;
        triggerAtaque.enabled = false;

    }

    public void AumentaMovSpeedCuboAguaAkiles()
    {
        stats.movSpeedModificate += cuboAgua.movSpeed;
    }

    public void CuboAguaAkiles()
    {
        if (!soyEnemigo_ok)
        {
            InvokeRepeating("CurarCuboAgua", 0f, cuboAgua.healSpeed);
        }
    }

    public void EliminarCuboAguaAkiles()
    {
        stats.AnyadirStatHabilidad(lanzaCuboAgua.nombre, lanzaCuboAgua.tipo, lanzaCuboAgua.physicalDamage, lanzaCuboAgua.duracion, Time.realtimeSinceStartup); //Añadimos el Lanza Cubo

        CancelInvoke("CurarCuboAgua");
    }


    //COMPLEMENTO ARMA LANZA CUBO AGUA
    [Serializable]
    public struct LanzaCuboAgua
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        //public int size;
        public float attackSpeed;
        //public float rangeVision;
        public float rangeAttack;
        public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        //public float sizeProyectil;
        public float physicalDamage;
        public float duracion;
        public float cantUsos;
        [HideInInspector]
        public float usosBase;
    }



    public void AumentaPhysicalDamageLanzaCuboAguaAkiles()
    {
        stats.physicalDamageModificate += lanzaCuboAgua.physicalDamage;

    }

    public void DisminuirUsoLanzaCuboAguaAkiles()
    {
        lanzaCuboAgua.cantUsos--;
        if (lanzaCuboAgua.cantUsos <= 0)
            stats.EliminarHabilidadesPorNombre(lanzaCuboAgua.nombre); //AQUI SE ACABA DE UTILIZAR POR LO QUE ELIMINAMOS EL STAT
    }


    public void AumentaAttackSpeedLanzaCuboAguaAkiles()
    {
        stats.attackSpeedModificate += lanzaCuboAgua.attackSpeed;
    }


    public void LanzaCuboAguaAkiles()
    {
        lanzaCuboAgua.usosBase = lanzaCuboAgua.cantUsos;
        stats.melee = lanzaCuboAgua.melee;
        stats.prefabProyectil = lanzaCuboAgua.prefabProyectil;
        stats.speedProyectil = lanzaCuboAgua.speedProyectil;

        GetComponent<ModificarRadio>().metros = lanzaCuboAgua.rangeAttack;
        GetComponent<ModificarRadio>().enabled = true;

        atacar.enabled = true;
        triggerAtaque.enabled = true;

    }


    public void EliminarLanzaCuboAguaAkiles()
    {
        Skills.transform.Find("Hab" + lanzaCuboAgua.idSkill.ToString()).GetComponent<SkillArma>().DesAplicarSkill();
        atacar.enabled = false;
        triggerAtaque.enabled = false;
        lanzaCuboAgua.cantUsos = lanzaCuboAgua.usosBase;
    }


    //HABILIDAD FRASE MOTIVACIONAL
    [Serializable]
    public struct FraseMotivacional
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        public float radio;
        public float duracion;
    }


    public void FraseMotivacionalAkiles()
    {
        if (!soyEnemigo_ok)
        {

            GameObject[] dentro = ComprobarPersonajesDentroRadio(fraseMotivacional.radio, false);

            for (int i = 0; i < dentro.Length; i++)
            {
                for (int j = 0; j < bufFraseMotivacional.tipo.Length; j++)
                {
                    dentro[i].GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(bufFraseMotivacional.nombre[j], bufFraseMotivacional.tipo[j], bufFraseMotivacional.value[j], bufFraseMotivacional.duracion[j], Time.realtimeSinceStartup);
                    EnviarRivalStat(bufFraseMotivacional.nombre[j], bufFraseMotivacional.tipo[j], bufFraseMotivacional.value[j], bufFraseMotivacional.duracion[j], dentro[i]);
                }
            }
        }
    }


    //BUF FRASE MOTIVACIONAL
    [Serializable]
    public struct BufFraseMotivacionalTropa
    {
        public int idSkill;
        public string[] nombre;
        public string[] tipo;
        public float[] value;
        public float[] duracion;
    }



    // Hechizo SUSTANCIA X
    [Serializable]
    public struct SustanciaX
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        public GameObject prefabProyectil;
        public float speedProyectil;
        public float physicalDamage;
        public float duracion;

        public float rangeFinal;
        public float physicalDamageFinal;
        public GameObject prefabDamageFinal;
    }


    public void SustanciaXAkiles()
    {
        if (!soyEnemigo_ok)
        {
            GameObject enemigo = BuscarPersonajeMasCercano(true);
            GameObject proyectil = CrearPrefabATropa(sustanciaX.prefabProyectil, enemigo);
            EnviarRivalPrefab(sustanciaX.nombre, enemigo);
            if (proyectil.GetComponent<TriggerAccionProyectil>())
            {
                proyectil.GetComponent<TriggerAccionProyectil>().physicalDamage = sustanciaX.physicalDamage;
                proyectil.GetComponent<TriggerAccionProyectil>().enviarMensaje_ok = true;
                proyectil.GetComponent<Efecto>().nombre = "EfectoPrefab" + sustanciaX.nombre;
                proyectil.GetComponent<Efecto>().tipo = 1;
            }
        }
    }


    public void InstanciarPrefabRivalSustanciaXAkiles()
    {
        GameObject proyectil = CrearPrefabATropa(sustanciaX.prefabProyectil, PjObjectivo);
        proyectil.transform.position = gameManager.jugadorEnemigo.transform.position;

    }


    public void InstanciarPrefabRivalEfectoPrefabSustanciaXAkiles()
    {
        GameObject proyectil = CrearPrefabATropa(sustanciaX.prefabDamageFinal, PjObjectivo);
        if(proyectil.GetComponent<Efecto>())
        proyectil.GetComponent<Efecto>().tipo = 2;
    }

    public void EfectoPrefabSustanciaXAkiles()
    {
        if (!soyEnemigo_ok)
        {
            GameObject[] dentro = ComprobarPersonajesDentroRadio(sustanciaX.rangeFinal);

            if(dentro!=null)
            for (int i = 0; i < dentro.Length; i++)
            {
                Debug.LogWarning("Es afectado por SustanciaX: " + dentro[i].name);
                GameObject proyectil = CrearPrefabATropa(sustanciaX.prefabDamageFinal, dentro[i]); // esto sería el efecto de cuando sufren daño por el círculo, puesto que el círuclo saldría en ambos jugadores automático al llegar el proyectil
                if (proyectil.GetComponent<TriggerAccionProyectil>())
                {
                    proyectil.GetComponent<TriggerAccionProyectil>().physicalDamage = sustanciaX.physicalDamageFinal;
                    proyectil.GetComponent<TriggerAccionProyectil>().enviarMensaje_ok = true;
                    proyectil.GetComponent<Efecto>().tipo = 2;
                }
                EnviarRivalPrefab("EfectoPrefab" + sustanciaX.nombre, dentro[i]);
            }
        }

    }


    //Arma Rey del trueno
    [Serializable]
    public struct ReyDelTrueno
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        public float attackSpeed;
        public float rangeAttack;
        public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        public float physicalDamage;
        public float duracion;
        public float velTick;
        public int cantPjMinTick;
        public int cantPjMaxTick;
    }


    public void AñadirReyDelTruenoAkiles()
    {
        stats = atacar.stats;

        stats.EliminarHabilidadesPorStatAumenta("arma", reyDelTrueno.nombre);
        stats.EliminarHabilidadesPorStatAumenta("complementoArma");
    }


    public void ReyDelTruenoAkiles()
    {
        if (!soyEnemigo_ok)
        {
            InvokeRepeating("reyTruenoTickBase", 0f, reyDelTrueno.velTick);
        }
    }

    public void EliminarReyDelTruenoAkiles()
    {
        if (!soyEnemigo_ok)
        {
            reyTruenoTick(3, 60f); //cuando termina tiene que lanzar 3 rayos entre todos los personajes rivales
        }

        Skills.transform.Find("Hab" + reyDelTrueno.idSkill.ToString()).GetComponent<SkillArma>().DesAplicarSkill();

        CancelInvoke("reyTruenoTickBase");
    }

    public void InstanciarPrefabRivalReyDelTruenoAkiles()
    {
        GameObject proyectil = CrearPrefabATropa(reyDelTrueno.prefabProyectil, PjObjectivo);
        proyectil.transform.position = gameManager.jugadorEnemigo.transform.position;
    }


    // Hechizo Bola de Nieve
    [Serializable]
    public struct BolaDeNieve
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        public GameObject prefabProyectil;
        public float speedProyectil;
        public float physicalDamage;
        public int numVectorGameManager;
        public int cantCriaturas;
        public float duracion;
        [HideInInspector]
        public GameObject CriaturaObjetivo;
    }




    public void BolaDeNieveAkiles()
    {
        if (!soyEnemigo_ok)
        {
            bolaDeNieve.CriaturaObjetivo = BuscarPersonajeMasCercano(true);
            GameObject proyectil = CrearPrefabATropa(bolaDeNieve.prefabProyectil, bolaDeNieve.CriaturaObjetivo);
            if (proyectil.GetComponent<TriggerAccionProyectil>())
            {
                proyectil.GetComponent<TriggerAccionProyectil>().physicalDamage = bolaDeNieve.physicalDamage;
                proyectil.GetComponent<TriggerAccionProyectil>().enviarMensaje_ok = true;
                proyectil.GetComponent<Efecto>().nombre = "EfectoPrefab" + bolaDeNieve.nombre;
                proyectil.GetComponent<Efecto>().tipo = 1;
            }
            EnviarRivalPrefab(bolaDeNieve.nombre, bolaDeNieve.CriaturaObjetivo);
        }
    }

    public void InstanciarPrefabRivalBolaDeNieveAkiles()
    {
        GameObject proyectil = CrearPrefabATropa(bolaDeNieve.prefabProyectil, PjObjectivo);
        proyectil.transform.position = gameManager.jugadorEnemigo.transform.position;
    }


    public void EfectoPrefabBolaDeNieveAkiles()
    {
        if (!soyEnemigo_ok)
        {
            Vector2[] posiciones = CrearPosicionesCriaturas(bolaDeNieve.CriaturaObjetivo, 1, 5);

            CrearSpawn(posiciones, gameManager.prefabsPersonajes[bolaDeNieve.numVectorGameManager]);
            EnviarCriaturasCreadasEnemigo(posiciones, bolaDeNieve.numVectorGameManager);
        }
    }


    // Hechizo Cohete
    [Serializable]
    public struct Cohete
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        public GameObject prefabProyectil;
        public float speedProyectil;
        public float physicalDamage;
        public float duracion;
    }



    public void CoheteAkiles()
    {
        if (!soyEnemigo_ok)
        {
            GameObject enemigo = gameManager.jugadorEnemigo; //BuscarPersonajeMasCercano(true);
            GameObject proyectil = CrearPrefabATropa(cohete.prefabProyectil, enemigo);
            if (proyectil.GetComponent<TriggerAccionProyectil>())
            {

                proyectil.GetComponent<TriggerAccionProyectil>().physicalDamage = cohete.physicalDamage;
                proyectil.GetComponent<TriggerAccionProyectil>().enviarMensaje_ok = true;
            }
            EnviarRivalPrefab(cohete.nombre, enemigo);
        }
    }

    public void InstanciarPrefabRivalCoheteAkiles()
    {
        GameObject proyectil = CrearPrefabATropa(cohete.prefabProyectil, PjObjectivo);
        proyectil.transform.position = gameManager.jugadorEnemigo.transform.position;
    }


    // Hechizo Trampa para Gigantes Explosion
    [Serializable]
    public struct ExplosionTrampaGigantes
    {
        public int idSkill;
        public string nombre;
        public string tipo;
        public GameObject prefabExplosion;
        public float radio;
        public float physicalDamage;
        public float duracion;
    }

    public void ExplosionTrampaGigantesAkiles()
    {
        if (!soyEnemigo_ok)
        {
            Instantiate(explosionTrampaGigantes.prefabExplosion, PjObjectivo.transform);

            string paquete = "N" + explosionTrampaGigantes.nombre + "|" + PjObjectivo.transform.position.x + "+" + PjObjectivo.transform.position.y + "%" + PjObjectivo.transform.position.z;
            gameManager.EnviarPaquete(paquete, true);

            GameObject[] dentro = ComprobarPersonajesDentroRadio(explosionTrampaGigantes.radio);

            for (int i = 0; i < dentro.Length; i++)
            {
                Debug.LogWarning("La explosión de la trampa para gigante actua sobre : " + dentro[i].name);
                GameObject proyectil = CrearPrefabATropa(proyectiles.defaultP, dentro[i]);
                if (proyectil.GetComponent<TriggerAccionProyectil>())
                {
                    proyectil.GetComponent<TriggerAccionProyectil>().physicalDamage = explosionTrampaGigantes.physicalDamage;
                    proyectil.GetComponent<TriggerAccionProyectil>().enviarMensaje_ok = true;
                }
                EnviarRivalPrefab(proyectiles.defaultP.name, dentro[i]);

            }
        }
    }

    public void InstanciarPrefabRivalExplosionTrampaGigantesAkiles()
    {
        Instantiate(explosionTrampaGigantes.prefabExplosion, PjObjectivo.transform.position,PjObjectivo.transform.rotation);
    }


    //Proyectiles
    [Serializable]
    public struct Proyectiles
    {
        public GameObject bolaDeFuego;
        public GameObject defaultP;
        public GameObject bolaDeHielo;
        public GameObject FlechaTorre;
    }


    void SeleccionarProyectilBolaDeFuego()
      {
        proyectilAtaque = proyectiles.bolaDeFuego;
      }


    void SeleccionarProyectilDefault()
    {
        proyectilAtaque = proyectiles.defaultP;
    }

    void SeleccionarProyectilBolaDeHielo()
    {
        proyectilAtaque = proyectiles.bolaDeHielo;
    }

    void SeleccionarProyectilFlecha()
    {
        proyectilAtaque = proyectiles.bolaDeHielo;
    }


    void SeleccionarProyectilFlechaTorre()
    {
        proyectilAtaque = proyectiles.FlechaTorre;
    }

    //FUNCIONES UTILES
    Vector2[] CrearPosicionesCriaturas(GameObject criatura, float radio, int numCriaturas) // La Criatura es donde se tienen que spawnear
    {
        Vector2[] posiciones = new Vector2[numCriaturas];

        for (int i = 0; i < numCriaturas; i++)
        {
            float anguloCreacion = 2 * Mathf.PI * i / numCriaturas;
            float x = criatura.transform.position.x + radio * Mathf.Cos(anguloCreacion);
            float y = criatura.transform.position.y + radio * Mathf.Sin(anguloCreacion);
            posiciones[i] = new Vector2(x - gameManager.jugadorPropio.transform.position.x, y-gameManager.jugadorPropio.transform.position.y );
        }



        return posiciones;
    }


    GameObject[] ComprobarPersonajesDentroRadio(float radio, bool enemigos_ok = true, float anguloApertura = 360)
    {
        int layerEnemigo = 10;
        int layerAliado = 9;

        GameObject[] personajesDentro = null;

        foreach (GameObject personaje in GameObject.FindGameObjectsWithTag("PERSONAJES"))
            if (enemigos_ok && personaje.layer == layerEnemigo ||
                !enemigos_ok && personaje.layer == layerAliado)
            {
                float x1 = transform.parent.position.x;
                float y1 = transform.parent.position.y;
                float x2 = personaje.transform.position.x;
                float y2 = personaje.transform.position.y;

                Vector2 vecDif = new Vector2((x2 - x1), (y2 - y1));
                float anguloVecDif = Vector2.Angle(vecDif, Vector2.up);
                if (Mathf.Abs(anguloVecDif) < anguloApertura / 2 && vecDif.magnitude < radio)                //Si dentro de RAdio
                {
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


    void EnviarRivalStat(string nombre, string tipo, float value, float duracion, GameObject pj, bool enemigo_ok = true)
    {
        if (!soyEnemigo_ok)
        {
            string nombrePj;
            int index = pj.name.IndexOf("E");
            if (index > -1)
                nombrePj = pj.name.Substring(0, index);
            else
                nombrePj = pj.name;


            string paquete = "K" + nombrePj + "/" + nombre + "|" + tipo + "@" + value.ToString() + "&" + duracion.ToString() + "%" + (Time.realtimeSinceStartup - DatosAccionesMultijugador.t0Room).ToString() + "+" + enemigo_ok;

            if (gameManager)
                gameManager.EnviarPaquete(paquete, true);

        }
    }

    public void CurarCuboAgua()
    {
        GameObject[] dentro = ComprobarPersonajesDentroRadio(cuboAgua.rangeHeal,false);

        for (int i = 0; i < dentro.Length; i++)
        {
            Debug.Log("Se cura a la unidad: " + dentro[i].name);
            dentro[i].GetComponentInChildren<Atacar>().stats.Curar(cuboAgua.lifeHeal);
            EnviarAñadirCuracionRival(cuboAgua.lifeHeal, dentro[i]);
        }
    }



    void EnviarAñadirCuracionRival(float cura, GameObject pj)
    {
        if (!soyEnemigo_ok)
        {
            string nombrePj;
            int index = pj.name.IndexOf("E");
            if (index > -1)
                nombrePj = pj.name.Substring(0, index);
            else
                nombrePj = pj.name;


            string paquete = "L" + nombrePj + "/" + cura.ToString();

            if (gameManager)
                gameManager.EnviarPaquete(paquete, true);

        }
    }

    void reyTruenoTickBase()
    {
        reyTruenoTick(UnityEngine.Random.Range(reyDelTrueno.cantPjMinTick, reyDelTrueno.cantPjMaxTick), reyDelTrueno.rangeAttack);
    }


    void reyTruenoTick(int cantEnemigos, float range)
    {
        GameObject[] dentro = ComprobarPersonajesDentroRadio(range);

        if(dentro!=null)
        for (int i = 0; i < cantEnemigos && i < dentro.Length; i++)
        {
            Debug.Log("Se lanza rayo a la unidad: "+dentro[i].name);
            GameObject proyectil= CrearPrefabATropa(reyDelTrueno.prefabProyectil, dentro[i]);
                if (proyectil.GetComponent<TriggerAccionProyectil>())
                {

                    proyectil.GetComponent<TriggerAccionProyectil>().physicalDamage = reyDelTrueno.physicalDamage;
                }

            EnviarRivalPrefab(reyDelTrueno.nombre,  dentro[i]);
        }
    }


    GameObject CrearPrefabATropa(GameObject prefab, GameObject objetivoGO, bool posicionInicialAkiles=true)
    {
        GameObject proyectil = Instantiate(prefab);
        if(posicionInicialAkiles)
            proyectil.transform.position = gameManager.jugadorPropio.transform.position;

        MoverGOProyectil mover = proyectil.GetComponentInChildren<MoverGOProyectil>();
        if (mover != null)
        {
            mover.GOSeguir = objetivoGO;
            mover.t0SegundosActualizar = Time.realtimeSinceStartup;
            if(mover.vel==0)
                mover.vel = stats.speedProyectil;
            mover.posFinal = objetivoGO.transform.position;
            mover.t0 = Time.realtimeSinceStartup;
            mover.pos0 = transform.parent.position;
        }

        TriggerAccionProyectil accion = proyectil.GetComponent<TriggerAccionProyectil>();

        if (accion)
        {
            accion.GoPadreCreadorDeEsteProyectil = transform.parent.gameObject;
            accion.enviarMensaje_ok = false;
        }

        if (GetComponent<TriggerAtaque>().soyEnemigo_ok)
            proyectil.layer = 12;
        else
            proyectil.layer = 11;


        return proyectil;
    }


    void EnviarRivalPrefab(string nombreSkill, GameObject tropa,  bool enemigo_ok = true)
    {
        if (!soyEnemigo_ok)
        {
            string nombrePj;
            int index = tropa.name.IndexOf("E");
            if (index > -1)
                nombrePj = tropa.name.Substring(0, index);
            else
                nombrePj = tropa.name;


            string paquete = "M" + nombrePj + "|" + nombreSkill + "+" + enemigo_ok;

            if (gameManager)
                gameManager.EnviarPaquete(paquete, true);

        }
    }


    GameObject BuscarPersonajeMasCercano(bool enemigo_ok = true)
    {
        float radio = 60f;// 60 es un valor para asegurarse a coger a todos los personajes en juego

        GameObject[] dentro = ComprobarPersonajesDentroRadio(radio);
        GameObject personajeCercano;

        personajeCercano = dentro[0];

        float x1 = transform.parent.position.x;
        float y1 = transform.parent.position.y;
        float x2 = dentro[0].transform.position.x;
        float y2 = dentro[0].transform.position.y;


        Vector2 vecDif = new Vector2((x2 - x1), (y2 - y1));
        float distanciaMinima = vecDif.magnitude;


        for (int i = 0; i < dentro.Length; i++)
        {
            x2 = dentro[i].transform.position.x;
            y2 = dentro[i].transform.position.y;

            vecDif = new Vector2((x2 - x1), (y2 - y1));

            if (distanciaMinima > vecDif.magnitude)
            {
                distanciaMinima = vecDif.magnitude;
                personajeCercano = dentro[i];
            }

        }

        return personajeCercano;
    }



    public void EnviarCriaturasCreadasEnemigo(Vector2[] vRands,int numCriaturaSpawn) // El numcriatura hace referencia al número en el vector del GameManager 
    {
        string strRands = "";
        if (vRands != null)
            for (int i = 0; i < vRands.Length; i++)
                strRands += "/" + (-vRands[i].x).ToString() + "&" + (-vRands[i].y).ToString();

        if (gameManager)
            gameManager.EnviarPaquete("I" + gameManager.jugadorPropio.name + "|" + numCriaturaSpawn.ToString() + strRands, true);
    }

    public void CrearSpawn(Vector2[] vRands,GameObject Criatura)
    {
        int layerJugador = 0;
        int layerTrigger = 0;

        if (gameManager)
        {
            //jugador = gameManager.jugadorPropio;
            layerJugador = GameManager.layerAliado;
            layerTrigger = GameManager.layerTriggerAliado;
        }

        if (gameManager.jugadorPropio)
            for (int i = 0; i < vRands.Length; i++)
            {
                Vector3 posicionCriatura = new Vector3(gameManager.jugadorPropio.transform.position.x + vRands[i].x, gameManager.jugadorPropio.transform.position.y + vRands[i].y, gameManager.jugadorPropio.transform.position.z);
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
                {
                    nuevoGO.GetComponentInChildren<Atacar>().stats = nuevoGO.AddComponent<Stats>();
                }

                GameObject.Find("Datos").GetComponent<AtributosTropas>().CrearNuevo(/*ID del bicho*/nuevoGO.GetComponentInChildren<Atacar>().IDPredeterminada,/*GameObject padre*/nuevoGO,/*Nivel del bicho*/0);


                if (nuevoGO.GetComponentInChildren<TriggerAtaque>())
                    nuevoGO.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok = false;
                if (nuevoGO.GetComponentInChildren<Atacar>())
                    nuevoGO.GetComponentInChildren<Atacar>().enviarMensaje_ok = true;
                SkillSpawn.numCriaturasPropiasCreadas++;
                nuevoGO.name = SkillSpawn.numCriaturasPropiasCreadas.ToString();

                nuevoGO.layer = layerJugador;
                if (nuevoGO.GetComponentInChildren<TriggerAtaque>())
                    nuevoGO.GetComponentInChildren<TriggerAtaque>().gameObject.layer = layerTrigger;
                if (nuevoGO.GetComponentInChildren<TriggerAreaVision>())
                    nuevoGO.GetComponentInChildren<TriggerAreaVision>().gameObject.layer = layerTrigger;

                if (nuevoGO.GetComponentInChildren<TriggerAreaCreacionEstructuras>())
                    nuevoGO.GetComponentInChildren<TriggerAreaCreacionEstructuras>().jugador = gameManager.jugadorPropio;


            }

    }



}
