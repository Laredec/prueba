using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActualizarNombre : MonoBehaviour
{
    public bool propioOk;
    public Text text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager)
        {
            if (propioOk)
            {
                if (text.text != gameManager.nombrePropio)
                    text.text = gameManager.nombrePropio;
            }
            else
                if (text.text != gameManager.nombreEnemigo)
                text.text = gameManager.nombreEnemigo;
        }

	}


}
