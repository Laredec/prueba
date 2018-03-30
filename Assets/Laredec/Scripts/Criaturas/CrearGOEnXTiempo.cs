using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearGOEnXTiempo : MonoBehaviour {

    public GameObject GO;
    public float tiempoEspera;
    public bool destruirGOActual_ok;
    public bool conservarPadre_ok;

    public GameObject GOChild;

    // Use this for initialization
    void Start () {
        if (transform.GetChild(0).GetComponent<Animator>())
            tiempoEspera = transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        Invoke("CrearGameObject",tiempoEspera);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    void CrearGameObject()
    {
        GameObject nuevoGO;

        if (GOChild!=null)
        {
            nuevoGO = GOChild/*Create*/;
            nuevoGO.SetActive(true);
            /*  nuevoGO.transform.position = transform.position;
              nuevoGO.transform.rotation = transform.rotation;
              nuevoGO.transform.localScale = transform.localScale;*/
            //nuevoGO.name = gameObject.name;
            //Debug.Log("Ahora se llama "+ gameObject.name);

            //nuevoGO.layer = gameObject.layer;
            //nuevoGO.tag = gameObject.tag;
            Animator referenciaAnim = transform.GetChild(0).GetComponent<Animator>();
            if (referenciaAnim && referenciaAnim.gameObject.activeInHierarchy)
            {
                referenciaAnim.transform.SetParent(GOChild.transform);
                referenciaAnim.transform.SetAsLastSibling();
            }

            nuevoGO.transform.SetParent(null);
        }
        else
        {
            nuevoGO = Instantiate(GO);
        }


        if (conservarPadre_ok && transform.parent != null)
            nuevoGO.transform.SetParent(transform.parent);

        if (destruirGOActual_ok)
            Destroy(gameObject);
    }


}
