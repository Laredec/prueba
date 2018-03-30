using UnityEngine;
using System.Collections;

public class ModificarRadio : MonoBehaviour
{
    public float metros;
    private float formularadio = 1f;
    public MoverGO moverGO = null;
    private bool boolDesactivar = false;

    // Use this for initialization
    void Start()
    {
        boolDesactivar = false;
    }

    void OnEnable()
    {
        boolDesactivar = false;
    }

    void Update()
    {
        if (boolDesactivar)
        {
            Component[] colliders1;
            colliders1 = GetComponents(typeof(CircleCollider2D));
            foreach (CircleCollider2D circle in colliders1)
            {
                //circle.radius -= 0.0001f;
                circle.radius = metros * formularadio;
                boolDesactivar = false;
                this.enabled = false;
            }

        }
        else if (!boolDesactivar)
        {
           // Debug.Log("Se mete en booldesactivar False");
            if (moverGO != null)
            {
                if (Time.realtimeSinceStartup - moverGO.t0 > moverGO.tEspera)
                {
                    formularadio = 1f;
                    Component[] colliders;
                    colliders = GetComponents(typeof(CircleCollider2D));
                    foreach (CircleCollider2D circle in colliders)
                    {
                        //   if (circle.isTrigger)
                        circle.enabled = true;
                        boolDesactivar = true;
                        circle.radius = 0.001f;
                       // Debug.Log("Pone valor");


                    }
                }
            }
            else
            {
                formularadio = 1f;
                Component[] colliders;
                colliders = GetComponents(typeof(CircleCollider2D));
                foreach (CircleCollider2D circle in colliders)
                {
                    // if (circle.isTrigger)
                    circle.enabled = true;
                    boolDesactivar = true;
                    circle.radius = 0.001f;
                }

            }
        }
    }

}
