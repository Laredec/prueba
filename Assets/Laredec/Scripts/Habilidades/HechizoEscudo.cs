using UnityEngine;
using System.Collections;

public class HechizoEscudo : MonoBehaviour {
    public GameObject GOPadre;
    public float destroyEscudo = 4f;
    public float danyoSoportaEscudo = 40f;

    void Start()
    {
        GOPadre.GetComponentInChildren<Atacar>().stats.escudoActual = danyoSoportaEscudo;
        Invoke("QuitarEscudo", destroyEscudo);
    }

    void Update()
    {
        if (GOPadre && GOPadre.GetComponentInChildren<Atacar>().stats.escudoActual <= 0)
            QuitarEscudo();
    }

    void QuitarEscudo()
    {
        GOPadre.GetComponentInChildren<Atacar>().stats.escudoActual = 0;
        Destroy(this.gameObject);
    }
}
