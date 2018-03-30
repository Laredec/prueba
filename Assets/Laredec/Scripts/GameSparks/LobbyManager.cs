using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;
using GameSparks.Api.Responses;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using GameSparks.RT;
using System;
using System.IO;
using System.Security.Cryptography;



public class LobbyManager : MonoBehaviour {

	/*public Text userId, connectionStatus;
	public Button matchmakingBttn;
	public Text matchDetails;
	public GameObject matchDetailsPanel;*/

    public Button /*findPendingMatchButton,*/ matchButton;

    static public bool registeredOk = false;


    static private LobbyManager sInstance;


    public static LobbyManager Instance()
    {
        if (sInstance != null)
        {
            return sInstance; // return the singleton if the instance has been setup
        }
        else
        { // otherwise return an error
          //Debug.LogError ("GSM| GameSparksManager Not Initialized...");
        }
        return null;
    }

    void Awake()
    {
        if (sInstance == null)
        {
            Debug.Log("NUEVO LOBBY MANAGER");
            
                Debug.Log("AÑADIENDO ON MATCH FOUND");

            IniciarLobbyManager();
            sInstance = this;
        }
        else if (sInstance != this)
        {
        }

    }



    // Use this for initialization
    void IniciarLobbyManager () {

		//userId.text = "No User Logged In..."; // we wont start with a user logged in so lets show this also
		//connectionStatus.text = "No Connection..."; // we wont immediatly have connection, so at the start of the lobby we will set the connection status to show this
		GS.GameSparksAvailable += (isAvailable) => {
			if(isAvailable)
            {
                Debug.Log("GameSparks Connected...");
               // if (connectionStatus)
				//    connectionStatus.text = "GameSparks Connected...";
                if(!registeredOk)
                    RegisterPlayerBttn(GenerateDeviceUniqueIdentifier(), "1234");
            }
            else
            {
                // if (connectionStatus)
                //    connectionStatus.text = "GameSparks Disconnected...";
                Debug.Log("GameSparks Disconnected...");

            }
        };
        // only the login panel and login button is needed at the start of the scene, so disable any other objects //
        //matchDetailsPanel.SetActive(false);
        //matchmakingBttn.gameObject.SetActive (false);

        GameSparks.Api.Messages.MatchFoundMessage.Listener += GameSparksManager.Instance().OnMatchFound;

        //findPendingMatchButton.gameObject.SetActive(false);
        //matchButton.gameObject.SetActive(false);



        // from the  matchmaking button we will call the FindPlayers method //
        /*matchButton.onClick.AddListener (() => {
			GameSparksManager.Instance().FindPlayers();
			//matchDetails.text = "Searching For Players...";
		});*/
        // this listener will update the text in the playerlist field if no match was found //
        GameSparks.Api.Messages.MatchNotFoundMessage.Listener  = (message) => {
			//matchDetails.text = "No Match Found...";
		};
		// this is the MatchFoundMessage listener. We are going to reference a method in this class to be called when this listener is called //
		

		// this is a listener for the StartGameBttn. Onclick, we will will pass the stored RTSessionInfo to the GameSparksManager to create a new RT session //
		





        /*findPendingMatchButton.onClick.AddListener(() => {


            
        });*/



        /*matchButton.onClick.AddListener(() => {

            new GameSparks.Api.Requests.MatchmakingRequest()
                .SetMatchShortCode("pending_test")
                .SetSkill(1)
                .Send((resp1) => {

                    if(!resp1.HasErrors)
                    {
                        Debug.LogWarning(resp1.JSONString);
                    }
                    else
                    {
                        Debug.LogWarning(resp1.JSONString); 
                    }

                });

        });*/

	}



    public void RegisterPlayerBttn(string userName, string pass)
    {
        Debug.Log("Registering Player...");
        new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(userName)
            .SetUserName(userName)
            .SetPassword(pass)
            .Send((response) => {

                if (!response.HasErrors)
                {
                    Debug.Log("Player Registered \n User Name: " + response.DisplayName);
                    registeredOk = true;
                    GameSparksManager.Instance().AuthenticateUser(userName, pass, OnRegistration, OnAuthentication);
                }
                else
                {
                    Debug.Log("Error Registering Player... \n " + response.Errors.JSON.ToString());
                    registeredOk = true;
                    GameSparksManager.Instance().AuthenticateUser(userName, pass, OnRegistration, OnAuthentication);
                }

            });

    }





    public class PendingMatchDetails
    {
        public string pendingMatchID;
        public string matchShortCode;
        public int skill;
        public List<PendingMatchPlayer> pendingMatchPlayers = new List<PendingMatchPlayer>();

        public class PendingMatchPlayer
        {

            public string equipo;
            public string playerId;
            public int skill;

            public PendingMatchPlayer(GSData gsData)
            {
                if(gsData.GetInt("skill").HasValue)
                {
                    skill = (int)gsData.GetInt("skill").Value;
                }
                playerId = gsData.GetString("playerId");
                float[] playerCoords = gsData.GetGSData("location").GetFloatList("coordinates").ToArray();
                equipo = gsData.GetString("equipo");
            }

            public void Print()
            {
                Debug.Log("PlayerId:"+playerId+", Skill:"+skill+", Equipo:"+ equipo);
            }
        }



        public PendingMatchDetails(GSData gsData)
        {
            pendingMatchID = gsData.GetString("id");
            matchShortCode = gsData.GetString("matchShortCode");
            if(gsData.GetInt("skill").HasValue)
            {
                skill = (int)gsData.GetInt("skill").Value;
            }
            foreach(GSData playerGS in gsData.GetGSDataList("matchedPlayers"))
            {
                pendingMatchPlayers.Add(new PendingMatchPlayer(playerGS));
            }
            
        }


        public void Print()
        {
            Debug.Log("Match ID:"+pendingMatchID+", ShortCode:"+matchShortCode);
            foreach(PendingMatchPlayer mplyr in pendingMatchPlayers)
            {
                mplyr.Print();
            }
        }
    }

    public void ComenzarBusqueda()
    {
        //GameManager.Instance.Comen();
    }








	/// <summary>
	/// this is called when a player is registered
	/// </summary>
	/// <param name="_resp">Resp.</param>
	private void OnRegistration(RegistrationResponse _resp){
        /*userId.text = "User ID: "+_resp.UserId;
		connectionStatus.text = "New User Registered...";
		matchmakingBttn.gameObject.SetActive (true);
		matchDetailsPanel.SetActive (true);*/
        Debug.Log(_resp.UserId + " registered");

//        matchLobbyPanel.SetActive(true);
//        findPendingMatchButton.gameObject.SetActive(true);
//        matchButton.gameObject.SetActive(true);
	}
	/// <summary>
	/// This is called when a player is authenticated
	/// </summary>
	/// <param name="_resp">Resp.</param>
	private void OnAuthentication(AuthenticationResponse _resp)
    {
        /*userId.text = "User ID: "+_resp.UserId;
		connectionStatus.text = "User Authenticated...";
		matchmakingBttn.gameObject.SetActive (true);
		matchDetailsPanel.SetActive (true);*/
        Debug.Log(_resp.UserId + " authenticated");
        //        matchLobbyPanel.SetActive(true);
        //        findPendingMatchButton.gameObject.SetActive(true);
        //        matchButton.gameObject.SetActive(true);
    }





#if UNITY_ANDROID
    static private string GetMd5Hash(string input)
    {
        if (input == "")
        {
            return "";
        }

        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            stringBuilder.Append(data[i].ToString("x2"));
        }

        return stringBuilder.ToString();
    }
#endif



    static private string GenerateDeviceUniqueIdentifier()
    {
#if UNITY_EDITOR
        return SystemInfo.deviceUniqueIdentifier+1;
#elif !UNITY_ANDROID
        return SystemInfo.deviceUniqueIdentifier;
#else
        try

        {
            string id = "";
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass contextClass = new AndroidJavaClass("android.content.Context");
            string TELEPHONY_SERVICE = contextClass.GetStatic<string>("TELEPHONY_SERVICE");
            AndroidJavaObject telephonyService = activity.Call<AndroidJavaObject>("getSystemService", TELEPHONY_SERVICE);
            bool noPermission = false;

            try
            {
                id = telephonyService.Call<string>("getDeviceId");
            }
            catch
            {
                noPermission = true;
            }

            if (id == null)
            {
                id = "";
            }

            if (noPermission)
            {
                AndroidJavaClass settingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
                string ANDROID_ID = settingsSecure.GetStatic<string>("ANDROID_ID");
                AndroidJavaObject contentResolver = activity.Call<AndroidJavaObject>("getContentResolver");
                id = settingsSecure.CallStatic<string>("getString", contentResolver, ANDROID_ID);
                if (id == null)
                {
                    id = "";
                }
            }

            if (id == "")
            {
                string mac = "00000000000000000000000000000000";
                try
                {
                    StreamReader reader = new StreamReader("/sys/class/net/wlan0/address");
                    mac = reader.ReadLine();
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                id = mac.Replace(":", "");
            }

            return GetMd5Hash(id);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return GetMd5Hash("00000000000000000000000000000000");
        }
#endif
    }






    // Update is called once per frame
    void Update () {
	
	}
}
