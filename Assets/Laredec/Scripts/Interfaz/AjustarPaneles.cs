using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AjustarPaneles : MonoBehaviour
{
    public Scrollbar scrollbar;
    public ScrollRect scrollRect;
    public RectTransform contentRT;
    public float velAjuste = 0.001f;
    public float tolerancia = 1f;

    public bool suavizandoOk = false;
    public bool desclicadoOk = false;

    // Use this for initialization
    void Start ()
    {
        contentRT.anchoredPosition = new Vector2(-1600, contentRT.anchoredPosition.y);
        scrollbar.value = 0.5f;
	}

    // Update is called once per frame
    void FixedUpdate()
    {

#if !UNITY_EDITOR
                if (Input.touchCount > 0)
                {
                    //Debug.Log("Input.touchCount: " + Input.touchCount);

                    Touch touch = Input.GetTouch(0);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            desclicadoOk = false;
                            break;

                        case TouchPhase.Moved:
                            desclicadoOk = false;
                            break;

                        case TouchPhase.Ended:
                            desclicadoOk = true;
                            break;
                    }
                }
#else
        if (Input.GetMouseButton(0))
            desclicadoOk = false;
        else
            desclicadoOk = true;
#endif



        if (desclicadoOk && scrollRect.velocity.magnitude < tolerancia)
            suavizandoOk = true;
        else
            suavizandoOk = false;

        if (suavizandoOk)
        {
            float valueToRound = Mathf.Round(scrollbar.value * 4) / 4;
            if (Mathf.Abs(scrollbar.value - valueToRound) > velAjuste)
            {
                Debug.Log("valueToRound: " + valueToRound);
                float signo = (valueToRound - scrollbar.value > 0) ? 1 : -1;
                Debug.Log(scrollRect.velocity.x / contentRT.sizeDelta.x / 2f);

                scrollbar.value += velAjuste * signo;
            }
            else
            {
                scrollbar.value = valueToRound;
                suavizandoOk = false;
            }
        }

    }
}
