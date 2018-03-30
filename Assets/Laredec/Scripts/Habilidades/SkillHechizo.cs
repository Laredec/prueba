using UnityEngine;
using System.Collections;

public class SkillHechizo : MonoBehaviour
{
    public GameObject jugador;
    public int numSkillHechizo;
//    public float areaSkill;
    public GameObject HechizoStun;
    public const int layerEnemigo = 10;
    public GameObject escudoGO;
    public GameObject sustanciaX;
    public GameObject fraseMotivacional;
    public GameObject bolaDeNieve;
    public GameObject cohete;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AplicarSkill()
    {
        GameManager gameManager = GameManager.Instance;
        if (jugador)
        {
            switch (numSkillHechizo)
            {
                case 0:
                    GameObject hechizo = Instantiate(HechizoStun, jugador.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                    hechizo.transform.SetParent(transform);
                    if (jugador.layer == layerEnemigo) // Si el jugador es el enemigo
                    {
                        hechizo.GetComponent<CircleCollider2D>().enabled = false;//atonto el collider para que no envie mensaje y porque no es necesario tener colisiones de más
                    }
                    break;

                case 1:
                    //primero borramos los posibles hijos
                    for (int i = 0; i < transform.childCount; i++)
                        Destroy(transform.GetChild(0).gameObject);

                    GameObject escudo = Instantiate(escudoGO, jugador.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                    escudo.transform.SetParent(transform);
                    escudo.transform.position = new Vector3(escudo.transform.position.x, escudo.transform.position.y, escudo.transform.position.z - 1);
                    escudo.GetComponent<HechizoEscudo>().GOPadre = jugador;
                    break;

                case 2:
                  //  GameObject sustanX = Instantiate(sustanciaX, jugador.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                   // sustanX.transform.SetParent(transform);
                   // sustanX.transform.position = new Vector3(sustanX.transform.position.x, sustanX.transform.position.y, sustanX.transform.position.z - 1);
                    jugador.GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(jugador.GetComponentInChildren<Buffos>().sustanciaX.nombre, "hechizo", 1, jugador.GetComponentInChildren<Buffos>().sustanciaX.duracion, Time.realtimeSinceStartup);
                    break;

                case 3:
                   // GameObject fraseMotiv = Instantiate(fraseMotivacional, jugador.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                   // fraseMotiv.transform.SetParent(transform);
                   // fraseMotiv.transform.position = new Vector3(fraseMotiv.transform.position.x, fraseMotiv.transform.position.y, fraseMotiv.transform.position.z - 1);
                    jugador.GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(jugador.GetComponentInChildren<Buffos>().fraseMotivacional.nombre,"hechizo",1, jugador.GetComponentInChildren<Buffos>().fraseMotivacional.duracion, Time.realtimeSinceStartup);
                    break;

                case 4:
                    //GameObject bolNieve = Instantiate(bolaDeNieve, jugador.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                   // bolNieve.transform.SetParent(transform);
                   // bolNieve.transform.position = new Vector3(bolNieve.transform.position.x, bolNieve.transform.position.y, bolNieve.transform.position.z - 1);
                    jugador.GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(jugador.GetComponentInChildren<Buffos>().bolaDeNieve.nombre, "hechizo", 1, jugador.GetComponentInChildren<Buffos>().bolaDeNieve.duracion, Time.realtimeSinceStartup);
                    break;

                case 5:
                    //GameObject cohe = Instantiate(cohete, jugador.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                   // cohe.transform.SetParent(transform);
                   // cohe.transform.position = new Vector3(cohe.transform.position.x, cohe.transform.position.y, cohe.transform.position.z - 1);
                    jugador.GetComponentInChildren<Atacar>().stats.AnyadirStatHabilidad(jugador.GetComponentInChildren<Buffos>().cohete.nombre , "hechizo", 1, jugador.GetComponentInChildren<Buffos>().cohete.duracion, Time.realtimeSinceStartup);

                    break;

                default:
                    break;
            }

        }
    }

}
