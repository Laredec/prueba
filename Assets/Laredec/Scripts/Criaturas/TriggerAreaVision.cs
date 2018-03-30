using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriggerAreaVision : MonoBehaviour {

    public GameObject objetivo;
    public bool persiguiendo_ok;
    private Vector3 posTorre;
    // public GameObject[] vCercanosGO = new GameObject[0];
    private float t0ActualizarArea = 0;

    void Start()
    {
       // vCercanosGO = new GameObject[0];
        posTorre = GetComponentInParent<MoverGO>().posFinal; //hacemos esto para que coincida
        posTorre.z = 0;
    }


    void FixedUpdate()
    {
        TriggerAtaque triggerAtaque = transform.parent.GetComponentInChildren<TriggerAtaque>();
        // if(!triggerAtaque.soyEnemigo_ok)

        if (persiguiendo_ok  &&  triggerAtaque.objetivo == null)
        {
            if (objetivo != null)
            {
               // if(triggerAtaque.soyEnemigo_ok)
               // DatosAccionesMultijugador.MensajeNuevoRecibido("ENTRA EN 3 ");
                GetComponentInParent<MoverGO>().posFinal = new Vector2 (objetivo.transform.position.x, objetivo.transform.position.y);
                GetComponentInParent<MoverGO>().enabled = true;
            }
           /* else if (!triggerAtaque.soyEnemigo_ok)
            {
                persiguiendo_ok = false;
               // ComprobarAlguienAPerseguir();
            }*/
        }

        if (!objetivo && transform.parent.GetComponentInChildren<TriggerAtaque>() && !transform.parent.GetComponentInChildren<TriggerAtaque>().soyTower_ok && transform.parent.GetComponentInChildren<TriggerAtaque>().objetivo==null && !GetComponent<ModificarRadio>().enabled/* && !transform.parent.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok*/)
        {
            // DatosAccionesMultijugador.MensajeNuevoRecibido("ENTRA EN 1 ");
            transform.parent.GetComponentInChildren<Atacar>().ActivarColliders();

        }


    }

    public void BuscarSiguientePersonajeASeguir()
    {
        GameObject[] Personajes = GameObject.FindGameObjectsWithTag("PERSONAJES");
        float distancia = 1000;
        GameObject pj = null;

        foreach (GameObject Personaje in Personajes)
            if ((Personaje.layer == 10 && transform.parent.gameObject.layer== 9 || Personaje.layer == 9 && transform.parent.gameObject.layer == 10)// Es decir si es enemigo  y yo aliado o al contrario
                && Vector3.Distance(Personaje.transform.position, transform.parent.position) < distancia) 
            {
                distancia = Vector3.Distance(Personaje.transform.position, transform.parent.position);
                pj = Personaje;
            }

        ComprobarSiPerseguir(pj);
    }

    void ComprobarSiPerseguir(Collider2D other)
    {
        objetivo = other.gameObject;
        persiguiendo_ok = true;
        GetComponentInParent<MoverGO>().posFinal = other.gameObject.transform.position;
        GetComponentInParent<MoverGO>().t0 = Time.realtimeSinceStartup;
        GetComponentInParent<MoverGO>().pos0 = transform.parent.position;
        if(!transform.parent.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok)
           EnviarCambioObjetivo(other.name);
    }


    void ComprobarSiPerseguir(GameObject GO)
    {
        Debug.Log("Actúa el comprobar perseguir de GameObject");
        objetivo = GO;
        persiguiendo_ok = true;
        GetComponentInParent<MoverGO>().posFinal = gameObject.transform.position;
        GetComponentInParent<MoverGO>().t0 = Time.realtimeSinceStartup;
        GetComponentInParent<MoverGO>().pos0 = transform.parent.position;
        if (!transform.parent.GetComponentInChildren<TriggerAtaque>().soyEnemigo_ok && GO)
            EnviarCambioObjetivo(GO.name);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entra en Trigger Vision de "+transform.parent.name +" el señor: "+ other.name);
        //  GroupResize(ref vCercanosGO, vCercanosGO.Length+1);
        //  vCercanosGO[vCercanosGO.Length - 1] = other.gameObject;
        TriggerAtaque triggerAtaque = transform.parent.GetComponentInChildren<TriggerAtaque>();

            if (!persiguiendo_ok  &&  triggerAtaque.objetivo == null  &&  !triggerAtaque.soyEnemigo_ok)//si no estoy ya persiguiendo a alguien entonces métete a perseguir a alguien
            {
                ComprobarSiPerseguir(other);
            }
    }


    void OnTriggerExit2D(Collider2D other)//SI SALE DE LA colision es que lo han empujado o el otro se ha movido o muerto, asi que hay que volver a seguirlo.
    {
        Debug.Log("sale de trigger vision");
       // GroupEliminar(ref vCercanosGO, other.gameObject);

        if (persiguiendo_ok)
        {
            TriggerAtaque triggerAtaque = transform.parent.gameObject.GetComponentInChildren<TriggerAtaque>();
            Atacar atacar = transform.parent.gameObject.GetComponentInChildren<Atacar>();
            if (objetivo == other.gameObject  &&  !triggerAtaque.soyEnemigo_ok)
            {
                persiguiendo_ok = false;
                triggerAtaque.objetivo = null;
                atacar.enemigoGO = null;
                atacar.atacando_ok = false;
                atacar.ActivarColliders();
                // ComprobarAlguienAPerseguir();
            }

        }
    }



  /*  void ReactivarMovimiento()
    {
        TriggerAtaque triggerAtaque = transform.parent.GetComponentInChildren<TriggerAtaque>();

        if (!triggerAtaque.soyEnemigo_ok)
        {
            GetComponentInParent<MoverGO>().enabled = true;
            GetComponentInParent<MoverGO>().t0 = Time.realtimeSinceStartup;
           // GetComponentInParent<SeguirPersonaje>().siguiendoOk = true;
            EnviarPerdidaObjetivo();
        }
        else
        {

        }
    }*/


  /*  public void GroupResize(ref GameObject[] group, int size)
    {
        GameObject[] temp = new GameObject[size];
        for (int c = 0; c < Mathf.Min(size, group.Length); c++)
        {
            temp[c] = group[c];
        }
        group = temp;
    }



    public void GroupEliminar(ref GameObject[] group, GameObject gameObject)
    {
        for (int i=0; i<group.Length; i++)
        {
            if (gameObject.name == group[i].name)
                for (int j = i; j < group.Length - 1; j++)
                    group[j] = group[j + 1];
        }

        GroupResize(ref group, group.Length - 1);
    }


    public void GroupEliminar(ref GameObject[] group, int numEliminar)
    {
        for (int j = numEliminar; j < group.Length - 1; j++)
            group[j] = group[j + 1];

        GroupResize(ref group, group.Length - 1);
    }*/


 /*   public void ComprobarAlguienAPerseguir()
    {
        if (vCercanosGO.Length > 0 && !persiguiendo_ok)
        {
            objetivo = null;

            while (vCercanosGO.Length > 0 && !persiguiendo_ok)
            {
                if (vCercanosGO[0] == null)
                {
                    GroupEliminar(ref vCercanosGO, 0);
                }
                else
                {
                    objetivo = vCercanosGO[0];
                    persiguiendo_ok = true;
                    GetComponentInParent<MoverGO>().enabled = true;
                    GetComponentInParent<MoverGO>().posFinal = objetivo.transform.position;
                    GetComponentInParent<MoverGO>().t0 = Time.realtimeSinceStartup;
                    GetComponentInParent<MoverGO>().pos0 = transform.parent.position;
                    EnviarCambioObjetivo(objetivo.name);
                    GetComponentInParent<SeguirPersonaje>().siguiendoOk = false;
                }
            }

            if (!persiguiendo_ok)
                ReactivarMovimiento();
        }
    }



    public void ComprobarAlguienAPerseguir(string nameExcluir)
    {
        if (vCercanosGO.Length > 0 && !persiguiendo_ok)
        {
            objetivo = null;
            int acc = 0;

            while (vCercanosGO.Length > acc && !persiguiendo_ok)
            {
                if (vCercanosGO[acc] == null)
                {
                    GroupEliminar(ref vCercanosGO, acc);
                }
                else
                {
                    if (vCercanosGO[acc].name != nameExcluir)
                    {
                        objetivo = vCercanosGO[acc];
                        persiguiendo_ok = true;
                        GetComponentInParent<MoverGO>().enabled = true;
                        GetComponentInParent<MoverGO>().posFinal = objetivo.transform.position;
                        GetComponentInParent<MoverGO>().t0 = Time.realtimeSinceStartup;
                        GetComponentInParent<MoverGO>().pos0 = transform.parent.position;
                        EnviarCambioObjetivo(objetivo.name);
                        GetComponentInParent<SeguirPersonaje>().siguiendoOk = false;
                    }
                    else
                    {
                        acc++;
                    }
                }

            }

            if (!persiguiendo_ok)
                ReactivarMovimiento();
        }
    }
    */

    public void EnviarCambioObjetivo (string name)
    {
        int index = name.IndexOf("Enemigo");
        string nombreSinEnemigo = name.Substring(0, index);
        string strPacket = "F" + transform.parent.name + "Enemigo" + "/" + nombreSinEnemigo;
        if (GameManager.Instance  &&  !GameManager.Instance.debugOk)
            GameManager.Instance.EnviarPaquete(strPacket, true);
    }


    public void EnviarPerdidaObjetivo()
    {
       
        string strPacket = "G" + transform.parent.name + "Enemigo";
        if (GameManager.Instance && !GameManager.Instance.debugOk)
            GameManager.Instance.EnviarPaquete(strPacket, true);
    }


}
