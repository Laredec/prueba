using UnityEngine;
using System.Collections;

public class OnEscape : MonoBehaviour
{
    public GameObject canvasInicioGO;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)  &&  Application.platform == RuntimePlatform.Android)
        {    
            if (canvasInicioGO  && canvasInicioGO.activeSelf)
            {
                if (GameSparksManager.Instance())
                    GameSparksManager.Instance().CerrarSala();
                ChangeScene.LoadScene("MenuPrincipal");
            }
            else
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
        }

    }//fin update

}