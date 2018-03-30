using UnityEngine;
using System.Collections;

public class DontDestroyOnLoadDatos : MonoBehaviour
{
    private static DontDestroyOnLoadDatos dontDestroy;

    // Use this for initialization
    void Start()
    {
        if (dontDestroy == null)
        {
            dontDestroy = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else if (dontDestroy != this)
        {
            Destroy(gameObject);
        }

        PlayerPrefs.GetInt("runa0", 2);
    }

}
