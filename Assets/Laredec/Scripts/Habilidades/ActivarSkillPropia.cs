using UnityEngine;
using System.Collections;

public class ActivarSkillPropia : MonoBehaviour
{
    public int manaCost;
    ///<summary> 
    ///Explicacion de numMetodoAparicion
    /// El 0 es el que aparece en un círculo cerca de Akiles
    /// El 1 es el que aparece en la pared más cercana de donde se encuentra Akiles
    /// El 2 es el que aparece en una pared al azar y todas las tropas siguientes de esta misma tanda de tropas aparecen cerca de donde se ha puesto la primera
    /// El 3 posicionarse detrás del rival Akiles
    /// El 4 es posicionarse entre Akiles y la Torre más cercana a la distancia de ataque de la criatura
    /// EL 5 es que aparece justo donde está Akiles (Se utiliza para torres sobretodo)
    ///</summary>


    public int numMetodoAparicion;

    private float wLimitante = 7f;
    private float hLimitante = 13f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activar()
    {
        GameManager gameManager = GameManager.Instance;
        GameObject jugador = null;

        string strSkill = transform.name; //nombre del GameObject que contiene el numero de la skill de este pj
        string numSkill = "-1";
        if (strSkill.Length > 0)
        {
            numSkill = strSkill.Remove(0, 3);// strSkill.Substring(strSkill.Length - 1, 1);
        }

        if (gameManager && numSkill != "-1")
        {
            jugador = gameManager.jugadorPropio;
        }

        if (jugador)
        {
            Debug.Log("Hab" + numSkill.ToString());
            GameObject habGO = jugador.transform.Find("Skills").Find("Hab" + numSkill.ToString()).gameObject;
            if (habGO)
            {
                SkillSpawn skillSpawn = habGO.GetComponent<SkillSpawn>();
                SkillArma skillArma = habGO.GetComponent<SkillArma>();
                SkillHechizo skillHechizo = habGO.GetComponent<SkillHechizo>();
                if (skillSpawn)
                {
                    Debug.Log("numMetodoAparicion: " + numMetodoAparicion);
                    Vector2[] vRands = MetodoAparacion(numMetodoAparicion, skillSpawn, jugador, habGO);

                    Debug.Log("Valores de los Rands");

                    if (numMetodoAparicion != 0  && numMetodoAparicion != 4)
                        for (int i = 0; i < vRands.Length; i++) //Luego se añade la posicion del jugador, por eso la quito aquí.
                        {
                            vRands[i].x -= jugador.transform.position.x;
                            vRands[i].y -= jugador.transform.position.y;
                            Debug.Log("De el elemento: " + i.ToString() + " lA x vale: " + vRands[i].x + " y la Y vale : " + vRands[i].y);
                        }

                    DatosAccionesMultijugador.MensajeNuevoRecibido("habGO if");

                    habGO.GetComponentInChildren<SkillSpawn>().AplicarSkill(jugador, vRands);
                    EnviarSkillEnemigo(vRands);
                    DatosAccionesMultijugador.MensajeNuevoRecibido("habGO if pasa");
                }
                else if (skillArma)
                {
                    DatosAccionesMultijugador.MensajeNuevoRecibido("habGO else if");

                    habGO.GetComponentInChildren<SkillArma>().AplicarSkill();
                    EnviarSkillEnemigo(null);
                    DatosAccionesMultijugador.MensajeNuevoRecibido("habGO else if pasa");

                }
                else if (skillHechizo)
                {
                    DatosAccionesMultijugador.MensajeNuevoRecibido("activando skill hechizo");
                    habGO.GetComponentInChildren<SkillHechizo>().AplicarSkill();
                    EnviarSkillEnemigo(null);
                    DatosAccionesMultijugador.MensajeNuevoRecibido("activando skill hechizo pasa");
                }

                /////AQUI habran else if las otras
            }   //fin if (habGO)
            else
                DatosAccionesMultijugador.MensajeNuevoRecibido("habGO else");


        }


    }

    Vector2[] MetodoAparacion(int numMetodo, SkillSpawn skillSpawn, GameObject jugador, GameObject habGO)
    {
        Vector2[] vRands = new Vector2[skillSpawn.cantCriaturas];

        Debug.Log("metodo de aparicion: " + numMetodo);
        switch (numMetodo)
        {
            case 0:
                vRands = ValoresAlAzarEnUnCirculo(vRands, skillSpawn.distMinSpawn, skillSpawn.distMaxSpawn);
                break;

            case 1:
                vRands = ComprobarPuntoParedCercana(jugador.transform.position, vRands);
                break;

            case 2:
                vRands = ValoresAlAzarEnUnCirculo(vRands, 0, 10);
                vRands = ComprobarPuntoParedCercana(new Vector3(vRands[0].x, vRands[0].y, jugador.transform.position.z), vRands);
                break;

            case 3:
                vRands = PosicionarseDetrasRival(habGO, vRands);
                break;

            case 4:
                bool torreOk = false;
                vRands = PosicionarseEntreTorreYAkiles(habGO, vRands, ref torreOk);

                if (!torreOk) // en caso de que no hayan torres, que se haga el default
                {
                    vRands = ValoresAlAzarEnUnCirculo(vRands, skillSpawn.distMinSpawn, skillSpawn.distMaxSpawn);
                }

                break;

            case 5:
                vRands[0] = new Vector2(GameManager.Instance.jugadorPropio.transform.position.x+0.01f, GameManager.Instance.jugadorPropio.transform.position.y);
                break;

            default:
                vRands = ValoresAlAzarEnUnCirculo(vRands, skillSpawn.distMinSpawn, skillSpawn.distMaxSpawn);
                break;
        }

        Debug.Log("vRands[0]: " + vRands[0]);

        return vRands;
    }


    public Vector2[] PosicionarseDetrasRival(GameObject habGO, Vector2[] vRands)
    {
        Transform PositionjugadorRival = GameManager.Instance.jugadorEnemigo.GetComponent<MoverGO>().referenciaGO.transform;
        float distFijaAlRival = 0;

        if (habGO.GetComponentInChildren<SkillSpawn>()) //Este if es para añadir la distancia de ataque
        {
            int id = habGO.GetComponentInChildren<SkillSpawn>().Criatura.transform.GetChild(1).GetChild(1).GetComponent<Atacar>().IDPredeterminada;
            distFijaAlRival = GameObject.Find("Datos").GetComponent<AtributosTropas>().statsIniciales[id].rangeAttack[0];
        }


        vRands[0] = new Vector2(PositionjugadorRival.position.x, PositionjugadorRival.position.y);
        vRands[0] += distFijaAlRival * 1.5f * (-GameManager.Instance.jugadorEnemigo.GetComponent<MoverGO>().direccion);


        return vRands;
    }





    public Vector2[] PosicionarseEntreTorreYAkiles(GameObject habGO, Vector2[] vRands, ref bool torreOk)
    {
        Debug.Log("entra en PosicionarseEntreTorreYAkiles");

        GameObject torre = TorreMasCercana();

        if (!torre || torre == null)
        {
            Debug.Log("NO HAY TORRE");
            torreOk = false;
            return vRands;
        }
        else
        {
            torreOk = true;
            float distFijaAlRival = 0;

            if (habGO.GetComponentInChildren<SkillSpawn>()) //Este if es para añadir la distancia de ataque
                distFijaAlRival = GameObject.Find("Datos").GetComponent<AtributosTropas>().statsIniciales[habGO.GetComponentInChildren<SkillSpawn>().Criatura.transform.GetChild(1).GetChild(1).GetComponent<Atacar>().IDPredeterminada].rangeAttack[0];

            Vector3 positionTorreRival = torre.transform.position;
            Vector3 posAkiles = GameManager.Instance.jugadorPropio.transform.position;

            vRands[0] = (positionTorreRival - posAkiles).normalized * ((positionTorreRival - posAkiles).magnitude - distFijaAlRival);
        }



        return vRands;
    }







    public GameObject TorreMasCercana()
    {
        int layerEnemigo = 10;
        float distMinim = 1000f;
        GameObject torre = null;

        foreach (GameObject personaje in GameObject.FindGameObjectsWithTag("PERSONAJES"))
            if (personaje != GameManager.Instance.jugadorPropio  &&  personaje != GameManager.Instance.jugadorEnemigo  &&
                personaje.layer == layerEnemigo && personaje.GetComponentInChildren<Atacar>().soyTower_ok)
            {
                Debug.Log("entra en foreach: " + personaje);
                float x1 = GameManager.Instance.jugadorPropio.transform.position.x;
                float y1 = GameManager.Instance.jugadorPropio.transform.position.y;
                float x2 = personaje.transform.position.x;
                float y2 = personaje.transform.position.y;

                Vector2 vecDif = new Vector2((x2 - x1), (y2 - y1));
                Debug.Log("vecDif.magnitude: " + vecDif.magnitude);
                Debug.Log("distMinim: " + distMinim);

                if (vecDif.magnitude < distMinim)
                {
                    Debug.Log("ENTRA EN EL IF");
                    distMinim = vecDif.magnitude;
                    torre = personaje;
                }
                //Se añade al vector de personajes de arriba
            }

        return torre;
    }



    public Vector2[] ValoresAlAzarEnUnCirculo(Vector2[] vRands, float min, float max)
    {
        for (int i = 0; i < vRands.Length; i++)  //Obtenemos los rands en un círculo cercano al personaje
        {
            vRands[i] = Random.insideUnitCircle * (min - max);
            vRands[i] += vRands[i].normalized * min;
        }

        return vRands;
    }



    public Vector2[] ComprobarPuntoParedCercana(Vector3 tPunto, Vector2[] vRands)
    {
        //derecha
        float distanciaMinima = Vector3.Distance(tPunto, new Vector3(wLimitante, tPunto.y, tPunto.z));

        for (int i = 0; i < vRands.Length; i++)
            vRands[i] = new Vector2(wLimitante, tPunto.y + Random.Range(-0.8f, 0.8f));


        //izquierda
        if (distanciaMinima >= Vector3.Distance(tPunto, new Vector3(-wLimitante, tPunto.y, tPunto.z)))
        {
            for (int i = 0; i < vRands.Length; i++)
                vRands[i] = new Vector2(-wLimitante, tPunto.y + Random.Range(-0.8f, 0.8f));
            distanciaMinima = Vector3.Distance(tPunto, new Vector3(-wLimitante, tPunto.y, tPunto.z));
        }

        //arriba
        if (distanciaMinima >= Vector3.Distance(tPunto, new Vector3(tPunto.x, hLimitante, tPunto.z)))
        {
            for (int i = 0; i < vRands.Length; i++)
                vRands[i] = new Vector2(tPunto.x + Random.Range(-0.8f, 0.8f), hLimitante);
            distanciaMinima = Vector3.Distance(tPunto, new Vector3(tPunto.x, hLimitante, tPunto.z));
        }


        //abajo
        if (distanciaMinima >= Vector3.Distance(tPunto, new Vector3(tPunto.x, -hLimitante, tPunto.z)))
        {
            for (int i = 0; i < vRands.Length; i++)
                vRands[i] = new Vector2(tPunto.x + Random.Range(-0.8f, 0.8f), -hLimitante);
            distanciaMinima = Vector3.Distance(tPunto, new Vector3(tPunto.x, -hLimitante, tPunto.z));
        }


        return vRands;
    }


    public void EnviarSkillEnemigo(Vector2[] vRands)
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager && !gameManager.debugOk)
        {
            string strSkill = transform.name; //nombre del GameObject que contiene el numero de la skill de este pj
            string numSkill = "-1";
            if (strSkill.Length > 0)
            {
                int lonNumero = strSkill.Length - (strSkill.IndexOf("Hab") + 3);

                numSkill = strSkill.Substring(strSkill.Length - lonNumero, lonNumero); //SERGIO COGER LOS ULTIMOS DESPUES DE HAB
            }

            string strRands = "";
            if (vRands != null)
                for (int i = 0; i < vRands.Length; i++)
                    strRands += "/" + (-vRands[i].x).ToString() + "&" + (-vRands[i].y).ToString();

            if (gameManager && numSkill != "-1")
                gameManager.EnviarPaquete("B" + numSkill + strRands, true);
        }
    }

}
