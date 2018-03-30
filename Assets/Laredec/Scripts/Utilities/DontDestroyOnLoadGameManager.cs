using UnityEngine;
using System.Collections;

public class DontDestroyOnLoadGameManager : MonoBehaviour
{
    private static DontDestroyOnLoadGameManager dontDestroy;
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
    }

}
