using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriggerAtaque : MonoBehaviour {

    public bool soyTower_ok;
    public GameObject objetivo;
    [HideInInspector]
    public Vector3 posTorre;
    public bool soyEnemigo_ok = false;
   // public GameObject[] vCercanosGO = new GameObject[0];
    public Sprite[] vSprites;
    public SpriteRenderer spriteRenderer;
    public float modificacionXTowerCollider=0.0001f;

    /* void OnEnable()
     {
         Debug.Log("entra en on enable");
       //  if (vCercanosGO.Length>0)
       //      ComprobarSiAtacar(vCercanosGO[0]);  
     }*/


    void Update()
    {
        if(objetivo && transform.parent.GetComponentInChildren<MoverGO>() && transform.parent.GetComponentInChildren<MoverGO>().referenciaGO)
        {
            Vector3 direccion = (objetivo.transform.position - transform.parent.position).normalized;
            Debug.Log("Rotando en TriggerAtaque");
            transform.parent.GetComponentInChildren<MoverGO>().referenciaGO.transform.LookAt(transform.position + new Vector3(direccion.x, direccion.y, 0), Vector3.back);
            PararMovimiento();

        }

        if (spriteRenderer  &&  vSprites.Length == 2)
        {
            if (soyEnemigo_ok)
                spriteRenderer.sprite = vSprites[1];
            else
                spriteRenderer.sprite = vSprites[0];

        }
        

        if (GameManager.gameEndedOk)
        {
            this.enabled = false;
        }


        bool activarMov = false;
        if (objetivo && objetivo.GetComponentInChildren<Atacar>() && objetivo.GetComponentInChildren<Atacar>().muertoOk)
        {
            objetivo = null;
            if (GetComponent<CircleCollider2D>().enabled == false)
            {
                GetComponent<Atacar>().ActivarColliders();
            }
            if (!soyTower_ok)
                activarMov = true; //hacemos esto para que entre en el siguiente if
            else
            {
                GetComponent<Atacar>().ActivarColliders();
            }
        }


        if (!soyTower_ok)
        {
            if (activarMov && objetivo == null)
            {
                GetComponent<Atacar>().ActivarColliders();

            }
        }

        //PRUEBA STAY2D 
      /*  if(!soyEnemigo_ok && !soyTower_ok && GetComponent<Atacar>().stats.rangeAttack< Vector2.Distance(new Vector2(objetivo.transform.position.x, objetivo.transform.position.y),new Vector2(transform.parent.position.x, transform.parent.position.y)))
            GetComponent<Atacar>().ActivarColliders();
            */

        /*if (soyTower_ok && objetivo==null && !soyEnemigo_ok)
        {
            GetComponent<ModificarRadio>().metros = GetComponent<CircleCollider2D>().radius;
            GetComponent<ModificarRadio>().enabled = true;
        }*/

        if (GetComponent<Atacar>().stats.ID == 1 && objetivo==null) // ES DECIR SI SOY LA TORRE QUE DISPARA UNICAMENTE
        {
            transform.parent.position = new Vector3(transform.parent.position.x+modificacionXTowerCollider, transform.parent.position.y, transform.parent.position.z);
            modificacionXTowerCollider *= -1;
        }
        
    }


    void ComprobarSiAtacar(GameObject otherGameObject)
    {
        if (objetivo == null)//es decir si no estoy atacando ya a alguien
        {
                objetivo = otherGameObject;
                if (!soyEnemigo_ok)
                    ComenzarAtaque();
                else
                    PararMovimiento();
        }

    }

  void OnTriggerEnter2D(Collider2D other)
  {
        Debug.Log("entra en el enter");

      //  GroupResize(ref vCercanosGO, vCercanosGO.Length + 1);
      //  vCercanosGO[vCercanosGO.Length - 1] = other.gameObject;

        if (this.enabled)
            ComprobarSiAtacar(other.gameObject);
  }


    void OnTriggerExit2D(Collider2D other)//SI SALE DE LA colision es que lo han empujado o el otro se ha movido, a si que hay que volver a seguirlo.
    {
        Debug.Log("entra en el exit");
     //   GroupEliminar(ref vCercanosGO, other.gameObject);
      //  GroupResize(ref vCercanosGO, vCercanosGO.Length - 1);

        if (objetivo != null && objetivo.name == other.gameObject.name)
        {
          //  ComprobarAlguienAAtacar();
            GetComponent<Atacar>().ActivarColliders();
        }
    }



    public void BuscarSiguientePersonajeAAtacar()
    {
        GameObject[] Personajes = GameObject.FindGameObjectsWithTag("PERSONAJES");
        float distancia = GetComponent<ModificarRadio>().metros;
        GameObject pj = null;

        foreach (GameObject Personaje in Personajes)
            if (Personaje.layer == 10 && Vector3.Distance(Personaje.transform.position, transform.parent.position) < distancia) // Es decir si es enemigo
            {
                distancia = Vector3.Distance(Personaje.transform.position, transform.parent.position); //esto es para que coja el más cercano
                pj = Personaje;
            }


        if(pj)
        ComprobarSiAtacar(pj);
    }



    void ComenzarAtaque()
    {
        Debug.Log("entra en comenzar ataque");
        if (!soyTower_ok)
        {
            transform.parent.GetComponentInChildren<TriggerAreaVision>().persiguiendo_ok = false;
            transform.parent.GetComponentInChildren<TriggerAreaVision>().objetivo = null;
        }

        GetComponent<Atacar>().enemigoGO = objetivo;

        if (Time.realtimeSinceStartup - GetComponent<Atacar>().t0ultimoAtaque > (1.0f / (GetComponent<Atacar>().stats.attackSpeed+ GetComponent<Atacar>().stats.ComprobarHabilidadesAumentoAttackSpeed())))
        {
            GetComponent<Atacar>().atacarNadaMasEntrar_Ok = true;
        }

        GetComponent<Atacar>().atacando_ok = true;
        GetComponent<Atacar>().enviarMensaje_ok = true;
        PararMovimiento();
    }


    public void PararMovimiento()
    {
        if (!soyTower_ok) // SI NO ERES TORRE DESACTIVAS EL MOVERTE
        {
            //GetComponentInParent<MoverGO>().enabled = false;
            GetComponentInParent<MoverGO>().rigidBody.velocity = Vector2.zero;
            GetComponentInParent<MoverGO>().posFinal = transform.parent.position;

            Animator animator = transform.parent.GetComponentInChildren<Animator>();
            if (animator)
                animator.SetBool("moviendoseOk", false);
        }
    }


 /*   public void GroupResize(ref GameObject[] group, int size)
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
        //Debug.Log(group.Length);
        for (int i = 0; i < group.Length; i++)
        {
            if (gameObject.name == group[i].name)
                for (int j = i; j < group.Length - 1; j++)
                    group[j] = group[j + 1];
        }

        
    }


    public void GroupEliminar(ref GameObject[] group, int numEliminar)
    {
        for (int j = numEliminar; j < group.Length - 1; j++)
            group[j] = group[j + 1];

        GroupResize(ref group, group.Length - 1);
    }

    */



  /*  public void ComprobarAlguienAAtacar()
    {
        objetivo = null;
        GetComponent<Atacar>().enemigoGO = null;
        GetComponent<Atacar>().atacando_ok = false;
        if (!soyTower_ok)
        {
            transform.parent.GetComponentInChildren<TriggerAreaVision>().persiguiendo_ok = false;
            transform.parent.GetComponentInChildren<TriggerAreaVision>().objetivo = null;
        }

        while (vCercanosGO.Length > 0 && objetivo == null)
        {
            if (vCercanosGO[0] == null)
            {
                GroupEliminar(ref vCercanosGO, 0);
            }
            else
            {
                objetivo = vCercanosGO[0];
                if (!soyEnemigo_ok)
                {
                    GetComponent<Atacar>().enemigoGO = objetivo;
                    GetComponent<Atacar>().atacando_ok = true;
                    GetComponent<Atacar>().enviarMensaje_ok = true;
                    GetComponent<Atacar>().t0ultimoAtaque = Time.realtimeSinceStartup;

                }


            }
        }

       // if (objetivo == null && !soyTower_ok)
        //    transform.parent.gameObject.GetComponentInChildren<TriggerAreaVision>().ComprobarAlguienAPerseguir();
    }



    public void ComprobarAlguienAAtacar (string nameExcluir)
    {
        objetivo = null;
        GetComponent<Atacar>().enemigoGO = null;
        GetComponent<Atacar>().atacando_ok = false;
        if (!soyTower_ok)
        {
            transform.parent.GetComponentInChildren<TriggerAreaVision>().persiguiendo_ok = false;
            transform.parent.GetComponentInChildren<TriggerAreaVision>().objetivo = null;
        }
        int acc = 0;

        while (vCercanosGO.Length > acc && objetivo == null)
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
                    if(!soyEnemigo_ok)
                    {
                        GetComponent<Atacar>().enemigoGO = objetivo;
                        GetComponent<Atacar>().atacando_ok = true;
                        GetComponent<Atacar>().enviarMensaje_ok = true;
                        GetComponent<Atacar>().t0ultimoAtaque = Time.realtimeSinceStartup;

                    }
                }
                else
                {
                    acc++;
                }
            }

        }

      //  if (objetivo == null && !soyTower_ok)//Porque si soy una torre no tengo área de vision, solo de ataque
      //      transform.parent.gameObject.GetComponentInChildren<TriggerAreaVision>().ComprobarAlguienAPerseguir(nameExcluir);
    }
*/

}
