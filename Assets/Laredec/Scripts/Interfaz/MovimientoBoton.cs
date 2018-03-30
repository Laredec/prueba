using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovimientoBoton : MonoBehaviour {

    public Vector3 posfinal;
    public float velocidad = 50;
    public float tolerancia = 3f;
    private bool primeraVezCambio=false;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (this.transform.localPosition.x <= posfinal.x + tolerancia && this.transform.localPosition.x >= posfinal.x - tolerancia && this.transform.localPosition.y <= posfinal.y + tolerancia && this.transform.localPosition.y >= posfinal.y - tolerancia)
        {
            if (transform.parent.GetComponent<BotonesPrincipales>().botonSelec == int.Parse(this.gameObject.name) && !primeraVezCambio)
            {
                primeraVezCambio = true;
                BotonesPrincipales botPrin = transform.parent.GetComponent<BotonesPrincipales>();
                this.transform.localPosition = new Vector3(0, -250, 0);
                posfinal = new Vector3(0, 0, 0);
                Sprite aux = GetComponent<Image>().sprite;
                GetComponent<Image>().sprite = botPrin.imagenes[botPrin.botonSelec];
                botPrin.imagenes[botPrin.botonSelec] = aux;
                velocidad = 100;
                DesactivarTodaslasPantallasMenosSeleccion(botPrin.botonSelec);
            }
            else if (transform.parent.GetComponent<BotonesPrincipales>().botonSelecAnterior == int.Parse(this.gameObject.name) && !primeraVezCambio)
            {
                primeraVezCambio = true;
                BotonesPrincipales botPrin = transform.parent.GetComponent<BotonesPrincipales>();
                Sprite aux = GetComponent<Image>().sprite;
                GetComponent<Image>().sprite = botPrin.imagenes[botPrin.botonSelecAnterior];
                botPrin.imagenes[botPrin.botonSelecAnterior] = aux;
                velocidad =50;

                transform.localPosition = new Vector3(botPrin.posBoton[botPrin.botonSelec, int.Parse(this.gameObject.name)].x, -30, 0);
                posfinal = botPrin.posBoton[botPrin.botonSelec, int.Parse(this.gameObject.name)];
            }
            else
            {
                BotonesPrincipales botPrin = transform.parent.GetComponent<BotonesPrincipales>();
                botPrin.ActivarBotones(true,1);

                primeraVezCambio = false;
                this.enabled = false;
            }
        }
        else
        {
            Vector3 vectorHaciaObjetivo = this.transform.localPosition - posfinal;
            vectorHaciaObjetivo = new Vector3(-vectorHaciaObjetivo.x, -vectorHaciaObjetivo.y, 0);
            vectorHaciaObjetivo.Normalize();
            this.transform.Translate(vectorHaciaObjetivo * Time.deltaTime * velocidad);
        }

    }

   void DesactivarTodaslasPantallasMenosSeleccion(int selec)
    {
        for(int i=0;i< transform.parent.parent.GetChild(0).GetChildCount();i++)
        {
            transform.parent.parent.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        transform.parent.parent.GetChild(0).GetChild(selec).gameObject.SetActive(true);
    }

}
