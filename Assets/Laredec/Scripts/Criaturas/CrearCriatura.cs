using UnityEngine;
using System.Collections;

public class CrearCriatura : MonoBehaviour {
    public float tiempoSpawn;
    public float t0UltimoSpawn;
    public float distMaxSpawn;
    public float distMinSpawn;
    public int cantCriaturas = 1;
    public int numCriaturaSpawn;
    public ReproducirSonido sonidoCreacion;

    private GameObject Criatura;
    private GameObject towerPadre;

    public float wLimitante = 4.68f * 2;
    public float hLimitante = 7.12f * 2;


    // Use this for initialization
    void Start()
       {
        towerPadre = transform.parent.gameObject;
        Criatura = GameManager.Instance.prefabsPersonajes[numCriaturaSpawn];
        t0UltimoSpawn = Time.realtimeSinceStartup- tiempoSpawn/2;
       }


    // Update is called once per frame
    void Update() {
        if (!GetComponent<TriggerAtaque>().soyEnemigo_ok && Time.realtimeSinceStartup - t0UltimoSpawn >tiempoSpawn)
            NuevoSpawn();
    }



    public void NuevoSpawn()
    {
        t0UltimoSpawn = Time.realtimeSinceStartup;
        Vector2[] vRands = new Vector2[cantCriaturas];
        for (int i = 0; i < vRands.Length; i++)  //obtenemos los rands
        {
            vRands[i] = Random.insideUnitCircle * (distMaxSpawn - distMinSpawn);
            vRands[i] += vRands[i].normalized * distMinSpawn;

            Vector3 posicionCriatura = new Vector3(towerPadre.transform.position.x + vRands[i].x, towerPadre.transform.position.y + vRands[i].y, 0);

            float radioCriatura;
            if (Criatura.GetComponent<CrearGOEnXTiempo>() && Criatura.GetComponent<CrearGOEnXTiempo>().GOChild)
                radioCriatura = Criatura.GetComponent<CrearGOEnXTiempo>().GOChild.GetComponentInChildren<CircleCollider2D>().radius;
            else
                radioCriatura = Criatura.GetComponentInChildren<CircleCollider2D>().radius;

     
        }

        CrearSpawn(vRands);
        EnviarSkillEnemigo(vRands);
        //sonidoCreacion.Reproducir();
    }


    public void EnviarSkillEnemigo(Vector2[] vRands)
    {
        GameManager gameManager = GameManager.Instance;

        string strRands = "";
        if (vRands != null)
            for (int i = 0; i < vRands.Length; i++)
                strRands += "/" + (-vRands[i].x).ToString() + "&" + (-vRands[i].y).ToString();

        if (gameManager)
            gameManager.EnviarPaquete("I" + transform.parent.gameObject.name + "|" + numCriaturaSpawn.ToString() +strRands, true);
    }

    public void CrearSpawn(Vector2[] vRands)
    {
        GameManager gameManager = GameManager.Instance;
        int layerJugador = 0;
        int layerTrigger = 0;

        if (gameManager)
        {
                //jugador = gameManager.jugadorPropio;
                layerJugador = GameManager.layerAliado;
                layerTrigger = GameManager.layerTriggerAliado;
        }

        if (towerPadre)
            for (int i = 0; i < vRands.Length; i++)
            {
                Vector3 posicionCriatura = new Vector3(towerPadre.transform.position.x + vRands[i].x, towerPadre.transform.position.y + vRands[i].y, towerPadre.transform.position.z);
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
                    nuevoGO.GetComponentInChildren<TriggerAreaCreacionEstructuras>().jugador = towerPadre;


            }

    }


}
