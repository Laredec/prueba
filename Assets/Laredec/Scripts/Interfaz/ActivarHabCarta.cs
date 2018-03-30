using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActivarHabCarta : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        int manaCost = GetComponentInParent<GeneradorCartas>().vBotones[int.Parse(name)].GetComponent<ActivarSkillPropia>().manaCost;
        GameObject boton = GetComponentInParent<GeneradorCartas>().vBotones[int.Parse(name)].gameObject;
        GetComponentInChildren<Button>().interactable = (GestionMana.Instance.manaActual >= manaCost);
        GetComponent<Image>().color = (GestionMana.Instance.manaActual >= manaCost) ? Color.white : new Color(1, 1, 1, 0.5f);
    }


    public void ActivarHab()
    {
        int numCarta = int.Parse(name);    
        GeneradorCartas generadorCartas = GetComponentInParent<GeneradorCartas>();
        int indiceBotonCarta= transform.GetSiblingIndex();
        int manaCost = generadorCartas.vBotones[numCarta].GetComponent<ActivarSkillPropia>().manaCost;
        GameObject boton = generadorCartas.vBotones[numCarta].gameObject;


        if (GestionMana.Instance.manaActual >= manaCost)
        {
            GestionMana.Instance.GastarMana(manaCost);
            boton.GetComponent<Button>().onClick.Invoke();
            DestroyImmediate(this.gameObject);
        }

        generadorCartas.GenerarCartaNueva(indiceBotonCarta);
    }

}
