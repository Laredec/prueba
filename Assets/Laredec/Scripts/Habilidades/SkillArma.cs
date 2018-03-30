using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillArma : MonoBehaviour
{
    public GameObject jugador;
   // public bool priorizaCampeonesOk;
    public float rango = 2;
 /*   public int poderAtaqueExtra = 20;
    public float velocidadAtaque = 1; //ataques por segundo
    public int velocidadMovimientoExtra = 0;
    public float probCriticoExtra = 0;*/
    public float duracion = 5;
    public GameObject indicadorArmaActivada;
    public GameObject indicadorArmaDesactivada;

    private float t0;
    private bool skillActivaOk = false;

    public bool melee_ok;
 /*   public GameObject Proyectil;
    public float speedProyectil;*/

    private bool melee_okAnterior;
    private GameObject ProyectilAnterior;
    private float speedProyectilAnterior;

    public GameObject armaGO;
    public GameObject[] armaDesactivarGO;
    public SkillArma[] otraArma;

    public bool armaEquipadaOk = false;

    public string nombreHab;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (skillActivaOk) //hacemos esto por si desactivamos en otro lado con stun
        {
            armaGO.SetActive(true);
            for(int i=0;i< armaDesactivarGO.Length;i++)
              armaDesactivarGO[i].SetActive(false);

            Atacar atacar = jugador.GetComponentInChildren<Atacar>();
            TriggerAtaque triggerAtaque = jugador.GetComponentInChildren<TriggerAtaque>();
            if (!atacar.stats.stun_ok)
            {
                atacar.enabled = true;
                triggerAtaque.enabled = true;
            }
        }
        else
        {
            //EliminarStat();
            armaGO.SetActive(false);
        }




        /*if (skillActivaOk && Time.realtimeSinceStartup - t0 > duracion)
            DesAplicarSkill();*/
	}




    public void AplicarSkill()
    {
        jugador.GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(nombreHab, "arma", rango, duracion, Time.realtimeSinceStartup);

        armaEquipadaOk = true;

        for(int i=0 ; i<otraArma.Length ; i++)
            if (otraArma[i].armaEquipadaOk)
                otraArma[i].DesAplicarSkill();

        skillActivaOk = true;

        InstanciarIndicadorArmaActivada();
    }


    public void DesAplicarSkill()
    {
            armaEquipadaOk = false;

            skillActivaOk = false;

            InstanciarIndicadorArmaDesactivada();
    }


    public void InstanciarIndicadorArmaActivada ()
    {
        if (indicadorArmaActivada)
        {
            GameObject nuevoIndicador = Instantiate(indicadorArmaActivada, indicadorArmaActivada.transform.position, indicadorArmaActivada.transform.rotation) as GameObject;
            nuevoIndicador.transform.SetParent(indicadorArmaActivada.transform.parent);
            nuevoIndicador.transform.position = indicadorArmaActivada.transform.position;
            nuevoIndicador.transform.localScale = indicadorArmaActivada.transform.localScale;

            nuevoIndicador.gameObject.SetActive(true);
        }
    }


    public void InstanciarIndicadorArmaDesactivada()
    {
        if (indicadorArmaDesactivada)
        {
            GameObject nuevoIndicador = Instantiate(indicadorArmaDesactivada, indicadorArmaDesactivada.transform.position, indicadorArmaDesactivada.transform.rotation) as GameObject;
            nuevoIndicador.transform.SetParent(indicadorArmaDesactivada.transform.parent);
            nuevoIndicador.transform.position = indicadorArmaDesactivada.transform.position;
            nuevoIndicador.transform.localScale = indicadorArmaDesactivada.transform.localScale;

            nuevoIndicador.gameObject.SetActive(true);
        }
    }

}



