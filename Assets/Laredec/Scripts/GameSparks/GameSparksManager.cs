using UnityEngine;
using System.Collections;
using GameSparks.Api.Responses;
using GameSparks.Core;
using GameSparks.Api.Messages;
using System.Collections.Generic;
using GameSparks.RT;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using GameSparks.Api.Requests;

using System.Text;
using System.Linq;



/// <summary>
/// Game sparks manager.
/// Created by Sean Durkan for GameSparks Inc 2016 cc
/// </summary>
public class GameSparksManager : MonoBehaviour {

	/// <summary>The GameSparks Manager singleton</summary>
	private static GameSparksManager sInstance = null;
    public static int eloRival;

    public bool matchedOk = false;

	/// <summary>This method will return the current instance of this class </summary>
	public static GameSparksManager Instance()
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
        Debug.Log("NUEVO GAMESPARKSMANAGER");
        if (sInstance == null)
        {
            sInstance = this;
            transform.SetParent(null);
            DestruirPersonajesYProyectiles();
            DontDestroyOnLoad(gameObject);
        }
        else if (sInstance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("QUITANDO OnMatchFound DESTROY");
    }



    private RTSessionInfo tempRTSessionInfo;

    /// <summary>
    /// This is called when the player receives the MatchFoundMessage.
    /// It is used to print the match details to the lobby and to keep a copy of the match details temporaly
    /// </summary>
    /// <param name="_message">matchfoundmessage</param>
    public void OnMatchFound(GameSparks.Api.Messages.MatchFoundMessage _message)
    {
        if (!matchedOk)
        {
            matchedOk = true;
            Debug.Log("Match Found!...");
            tempRTSessionInfo = new RTSessionInfo(_message); // we'll store the match data untill we need to create an RT session instance
                                                             //matchmakingBttn.gameObject.SetActive(false);

            GameSparksManager.Instance().StartNewRTSession(tempRTSessionInfo);


            StringBuilder sBuilder = new StringBuilder();
            sBuilder.AppendLine("Match Found...");
            sBuilder.AppendLine("Host URL:" + _message.Host);
            sBuilder.AppendLine("Port:" + _message.Port);
            sBuilder.AppendLine("Access Token:" + _message.AccessToken);
            sBuilder.AppendLine("MatchId:" + _message.MatchId);
            sBuilder.AppendLine("Opponents:" + _message.Participants.Count());
            sBuilder.AppendLine("_________________");
            sBuilder.AppendLine(); // we'll leave a space between the playerlist and the match data
            foreach (GameSparks.Api.Messages.MatchFoundMessage._Participant player in _message.Participants)
            {
                sBuilder.AppendLine("Player:" + player.PeerId + " User Name:" + player.DisplayName); // add the player number and the display name to the list
            }
            //matchDetails.text = sBuilder.ToString (); // set the string to be the playerlist field

            //ChangeScene.LoadScene("GameScene");
        }
    }



    /*private void OnEnable()
    {
        GetComponent<GameSparksUnity>().Start(); ////CAMBIAR
    }*/

    public GameSparksRTUnity gameSparksRTUnity;
	public GameSparksRTUnity GetRTSession()
    {
		return gameSparksRTUnity;
	}
	private RTSessionInfo sessionInfo;
	public RTSessionInfo GetSessionInfo(){
		return sessionInfo;
	}
	private ChatManager chatManager; // this is a refrence to the chat-manager so that any packets that contain chat-messages can be sent to the chat-manager

	#region Login & Registration
	public delegate void AuthCallback(AuthenticationResponse _authresp2);
	public delegate void RegCallback(RegistrationResponse _authResp);
	/// <summary>
	/// Sends an authentication request or registration request to GS.
	/// </summary>
	/// <param name="_callback1">Auth-Response</param>
	/// <param name="_callback2">Registration-Response</param>
	public void AuthenticateUser (string _userName, string _password, RegCallback _regcallback, AuthCallback _authcallback)
	{
		new GameSparks.Api.Requests.RegistrationRequest()
		// this login method first attempts a registration //
		// if the player is not new, we will be able to tell as the registrationResponse has a bool 'NewPlayer' which we can check //
		// for this example we use the user-name was the display name also //
			.SetDisplayName(_userName)
			.SetUserName(_userName)
			.SetPassword(_password)
			.Send((regResp) => {
				if(!regResp.HasErrors){ // if we get the response back with no errors then the registration was successful
					Debug.Log("GSM| Registration Successful..."); 
					_regcallback(regResp);
				}else{
					// if we receive errors in the response, then the first thing we check is if the player is new or not
					if(!(bool)regResp.NewPlayer) // player already registered, lets authenticate instead
					{
						Debug.LogWarning("GSM| Existing User, Switching to Authentication");
						new GameSparks.Api.Requests.AuthenticationRequest()
							.SetUserName(_userName)
							.SetPassword(_password)
							.Send((authResp) => {
								if(!authResp.HasErrors){
									Debug.Log("Authentication Successful...");
									_authcallback(authResp);
								}else{
									Debug.LogWarning("GSM| Error Authenticating User \n"+authResp.Errors.JSON);
								}
							});
					}else{
						Debug.LogWarning("GSM| Error Authenticating User \n"+regResp.Errors.JSON); // if there is another error, then the registration must have failed
					}
				}
			});
	}
	#endregion

	bool isConnected, isAuthenticated;

	void Start(){

//		GS.GameSparksAuthenticated += (isAv)=>{
//			Debug.Log (">>> GS AUTHENTICATED <<<  "+isAv);
//			isAuthenticated = true;
//		};
//
//		GS.GameSparksAvailable += (isCon)=>{
//			Debug.Log (">>> GS AVAILABLE "+isCon+" <<<");
//			isConnected = isCon;
//			if(!isCon){
//				isAuthenticated = false;
//			}
//		};


	}


    //	void Update(){
    //			
    //		if(isAuthenticated){
    //			Debug.Log (">>> IS AUTHENTICATED <<<");
    //		}
    //		else if(isConnected){
    //			Debug.Log (">>> IS AVAILABLE <<<");
    //		}
    //	}

    #region Matchmaking Request
    /// <summary>
    /// This will request a match between as many players you have set in the match.
    /// When the max number of players is found each player will receive the MatchFound message
    /// </summary>
    /// 
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
                if (gsData.GetInt("skill").HasValue)
                {
                    skill = (int)gsData.GetInt("skill").Value;
                }
                playerId = gsData.GetString("playerId");
                float[] playerCoords = gsData.GetGSData("location").GetFloatList("coordinates").ToArray();
                equipo = gsData.GetString("equipo");
            }

            public void Print()
            {
                Debug.Log("PlayerId:" + playerId + ", Skill:" + skill + ", Equipo:" + equipo);
            }
        }

        public PendingMatchDetails(GSData gsData)
        {
            pendingMatchID = gsData.GetString("id");
            matchShortCode = gsData.GetString("matchShortCode");
            if (gsData.GetInt("skill").HasValue)
            {
                skill = (int)gsData.GetInt("skill").Value;
            }
            foreach (GSData playerGS in gsData.GetGSDataList("matchedPlayers"))
            {
                pendingMatchPlayers.Add(new PendingMatchPlayer(playerGS));
            }

        }


        public void Print()
        {
            Debug.Log("Match ID:" + pendingMatchID + ", ShortCode:" + matchShortCode);
            foreach (PendingMatchPlayer mplyr in pendingMatchPlayers)
            {
                mplyr.Print();
            }
        }
    }


    public void FindPlayers()
    {

        Debug.Log ("GSM| Attempting Matchmaking...");
		new GameSparks.Api.Requests.MatchmakingRequest()
            .SetMatchShortCode ("1v1") // set the shortcode to be the same as the one we created in the first tutorial
			.SetSkill (0) // in// this case we want anyone to be able to join so the skill is set to zero by default
            .SetParticipantData(new GSRequestData().AddNumber("elo", IniciarDatos.Instance.datos.elo))
			.Send ((response) => 
            {
				if(response.HasErrors){ // check for errors
					Debug.LogError("GSM| MatchMaking Error \n"+response.Errors.JSON);
				}
                else
                {
                    Debug.LogWarning(response.JSONString);
                    new GameSparks.Api.Requests.FindPendingMatchesRequest()
                    .SetMatchShortCode("1v1")
                    .Send((resp2) =>
                    {
                        if (!resp2.HasErrors)
                        {
                            Debug.LogWarning(resp2.JSONString);
                            List<PendingMatchDetails> pendingMatchList = new List<PendingMatchDetails>();
                            if (resp2.BaseData.GetGSDataList("pendingMatches") != null)
                            {
                                Debug.Log("resp2");
                                foreach (GSData gs in resp2.BaseData.GetGSDataList("pendingMatches"))
                                {
                                    Debug.Log("pendingMatchList number " + pendingMatchList.Count);
                                   
                                    long eloRivalAux = gs.GetGSDataList("matchedPlayers")[0].GetGSData("participantData").GetNumber("elo").Value;

                                    pendingMatchList.Add(new PendingMatchDetails(gs));
                                    if (true)//NINGUNA CONDICION DE MOMENTO (CONDICIONES PUEDEN SER POR EJEMPLO PERTENENCIA A DOS O MAS GRUPOS DISTINTOS DE PERSONAS QUE NO PUEDEN JUGAR ENTRE SI, EQUIPO ROJO VS AZUL VS AMARILLO ETC)
                                    {
                                        new GameSparks.Api.Requests.JoinPendingMatchRequest()
                                        .SetMatchShortCode("1v1")
                                        .SetPendingMatchId(pendingMatchList[pendingMatchList.Count-1].pendingMatchID)
                                        .Send((resp3) =>
                                        {
                                            if (!resp3.HasErrors)
                                            {
                                                eloRival = (int)eloRivalAux;
                                                Debug.Log("GAME JOINED");
                                            }
                                            else
                                                Debug.LogError("No Match JOINED...");
                                        });
                                    }
                                }
                            }
                        }
                    });
                }
            });



	}
    #endregion



    public void CerrarSala()
    {
        if (Instance())
            if (Instance().GetRTSession())
            {
                Debug.Log("RT Session: " + GameSparksManager.Instance().GetRTSession());

                Instance().GetRTSession().Disconnect();
                Destroy(Instance().GetComponent<GameSparksRTUnity>());
                DestroyImmediate(gameObject);
            }
        /*else
        { 
            GS.Instance.Disconnect();
            Destroy(GameSparksManager.Instance().gameObject);
            Instantiate(gameSparksManagerPR);
        }*/

    }



    public void CancelMatchMaking()
    {
        new GameSparks.Api.Requests.MatchmakingRequest()
            .SetMatchShortCode("1v1") // set the shortcode to be the same as the one we created in the first tutorial
            .SetAction("cancel")
            .Send((response4) =>
            {
                /*if (response.HasErrors)
                { // check for errors
                    Debug.LogError("GSM| MatchMaking Cancel Error \n" + response.Errors.JSON);
                }
                else
                {
                    Debug.LogWarning(response.JSONString);
                }*/

            });
            

    }




	public void StartNewRTSession(RTSessionInfo _info)
    {
		if (true) {
			Debug.Log ("GSM| Creating New RT Session Instance...");
			sessionInfo = _info;
			gameSparksRTUnity = this.gameObject.AddComponent<GameSparksRTUnity> (); // Adds the RT script to the game
			// In order to create a new RT game we need a 'FindMatchResponse' //
			// This would usually come from the server directly after a sucessful FindMatchRequest //
			// However, in our case, we want the game to be created only when the first player decides using a button //
			// therefore, the details from the response is passed in from the gameInfo and a mock-up of a FindMatchResponse //
			// is passed in. In normal operation this mock-response may not be needed //
			GSRequestData mockedResponse = new GSRequestData ().AddNumber ("port", (double)_info.GetPortID ()).AddString ("host", _info.GetHostURL ()).AddString ("accessToken", _info.GetAccessToken ()); // construct a dataset from the game-details
			FindMatchResponse response = new FindMatchResponse (mockedResponse); // create a match-response from that data and pass it into the game-config
			// So in the game-config method we pass in the response which gives the instance its connection settings //
			// In this example i use a lambda expression to pass in actions for 
			// OnPlayerConnect, OnPlayerDisconnect, OnReady and OnPacket actions //
			// These methods are self-explanitory, but the important one is the OnPacket Method //
			// this gets called when a packet is received //

			gameSparksRTUnity.Configure (response, 
				(peerId) => {
					OnPlayerConnectedToGame (peerId);
				},
				(peerId) => {
					OnPlayerDisconnected (peerId);
				},
				(ready) => {
					OnRTReady (ready);
				},
				(packet) => {
					OnPacketReceived (packet);
				});
			gameSparksRTUnity.Connect (); // when the config is set, connect the game
           
//		Debug.LogError (_info.GetAccessToken()+"|"+_info.GetMatchID());
		} else
        {
            gameSparksRTUnity.Disconnect(); // when the config is set, connect the game
            StartNewRTSession(_info);
            Debug.LogError ("Session Already Started");
		}

    }

    private void OnPlayerConnectedToGame(int _peerId)
    {
		Debug.Log ("GSM| Player "+_peerId+" Connected");
	}

	private void OnPlayerDisconnected(int _peerId)
    {
		Debug.Log ("GSM| Player "+_peerId+" Disconnected");
		GameManager.Instance.OnOpponentDisconnected (_peerId);
	}

	private void OnRTReady(bool _isReady){
		if (_isReady) {
			Debug.Log ("GSM| RT Session Connected...");
        }
    }



    IEnumerator SendPackets ()
    {
        //for (int i = 1; i <= 150; i++) {
            GameSparksRTUnity.Instance.SendData (101, GameSparksRT.DeliveryIntent.RELIABLE, new RTData ().SetInt (1, 12));
            yield return new WaitForSeconds (2f);
        //}
    }

	private void OnPacketReceived(RTPacket _packet)
    {
        //Debug.Log("PAQUETE: " + _packet.ToString());
        //Debug.LogWarning(_packet.ToString());

		if (GameManager.Instance != null) {
            GameManager.Instance.PacketReceived(_packet.PacketSize);
		}
		
		switch (_packet.OpCode)
        {
		// op-code 1 refers to any chat-messages being received by a player //
		// from here, we will send them to the chat-manager //
		//case 1:

		//	break;

		case 2:
                // contains information about the rotation, positiona and 'isInvincible' bool
                GameManager.Instance.OnRealTimeMessageReceived(_packet.Data.GetString(1));
			break;
//		case 3:
//			// contains information about the id of the shell that needs to be created
//			// so that the sender and recipient a corresonding id for the shell
//			GameController.Instance ().InstantiateOpponentShells(_packet);
//			break;
//		case 4:
//			// contains information about the position and rotation of the opponent shells
//			GameController.Instance ().UpdateOpponentShells (_packet);
//			break;
//		case 5:
//			// contains information about the shell that hit, the player it hit, and the owner of the shell
//			GameController.Instance ().RegisterOpponentCollision (_packet);
//			break;
//
//		// REGISTER NETWORK INFO //
		case 100:
                //Debug.Log ("GSM| Loading Level...");
                //Debug.Log("entra en SyncClock");
                SyncClock(_packet);
                //GameObject.Find("Canvas Inicio").GetComponent<GestionarMenuCarga>().ComenzarPartida();
                break;
		case 101:
              /*  Debug.Log("HA LLEGADO EL NUMERO " + _packet.Data.GetFloat(1).ToString());
                Debug.Log(((float)_packet.Data.GetFloat(1)).ToString());

                if (_packet.Data.GetFloat(1) % 2 < 1)
                    IniciarDatos.Instance.datos.Equipo = "ROJO";
                else
                    IniciarDatos.Instance.datos.Equipo = "AZUL";
                    */ //ESTE PAQUETE SIRVE PARA AUTOASIGNAR EQUIPOS DESDE EL LADO DEL SERVIDOR PERO EN ESTE PROYECTO NO ES NECESARIO


                break;
		case 102:
                GameManager.Instance.OnOpponentDisconnected(0);
                break;
		}
	}





    private DateTime serverClock;
    private int timeDelta, latency, roundTrip;

    private bool clockStarted;
    private DateTime endTime;


    /// <summary>
    /// Calculates the time-difference between the client and server
    /// </summary>
    public void CalculateTimeDelta(RTPacket _packet)
    {
        // calculate the time taken from the packet to be sent from the client and then for the server to return it //
        roundTrip = (int)((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds - _packet.Data.GetLong(1).Value);
        latency = roundTrip / 2; // the latency is half the round-trip time
                                 // calculate teh server-delta from the server time time minus the current time 
        int serverDelta = (int)(_packet.Data.GetLong(2).Value - (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
        timeDelta = serverDelta + latency; // the time-delta is the server-delta plus the latency

    }

    /// <summary>
    /// Syncs the local clock to server-time
    /// </summary>
    /// <param name="_packet">Packet.</param>
    public void SyncClock(RTPacket _packet)
    {
        DateTime dateNow = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc); // get the current time
        serverClock = dateNow.AddMilliseconds(_packet.Data.GetLong(1).Value + timeDelta).ToLocalTime(); // adjust current time to match clock from server
        //Debug.Log("clockStarted: " + clockStarted);
        
        // Debug.Log("serverClock: " + serverClock);
        DateTime startClock = DateTime.Now.AddSeconds(-Time.realtimeSinceStartup); //clock when program started
        if (!clockStarted)
        {
            // make sure that we only calculate the endtime once
            //Debug.Log("serverClock: " + serverClock);
            //Debug.Log("startClock: " + startClock);
            DatosAccionesMultijugador.t0Room = Time.realtimeSinceStartup;
            //Debug.Log(DatosAccionesMultijugador.t0Room);


            clockStarted = true;
            GameManager.Instance.CreateQuickGame();

            //ChangeScene.LoadScene("Juego");
        }
        // set the timer each time a new update from the server comes in

    }


    public void DestruirPersonajesYProyectiles()
    {
        foreach (GameObject personaje in GameObject.FindGameObjectsWithTag("PERSONAJES"))
            GameObject.Destroy(personaje);

        foreach (GameObject proyectil in GameObject.FindGameObjectsWithTag("Proyectil"))
            GameObject.Destroy(proyectil);
    }


}







public class RTSessionInfo
{
	private string hostURL;
	public string GetHostURL(){	return this.hostURL;	}
	private string acccessToken;
	public string GetAccessToken(){	return this.acccessToken;	}
	private int portID;
	public int GetPortID(){	return this.portID;	}
	private string matchID;
	public string GetMatchID(){	return this.matchID;	}

	private List<RTPlayer> playerList = new List<RTPlayer> ();
	public List<RTPlayer> GetPlayerList(){
		return playerList;
	}

	/// <summary>
	/// Creates a new RTSession object which is held untill a new RT session is created
	/// </summary>
	/// <param name="_message">Message.</param>
	public RTSessionInfo (MatchFoundMessage _message){
		portID = (int)_message.Port;
		hostURL = _message.Host;
		acccessToken = _message.AccessToken;
		matchID = _message.MatchId;
		// we loop through each participant and get thier peerId and display name //
		foreach(MatchFoundMessage._Participant p in _message.Participants){
			playerList.Add(new RTPlayer(p.DisplayName, p.Id, (int)p.PeerId));
		}
	}

	public class RTPlayer
	{
		public RTPlayer(string _displayName, string _id, int _peerId){
			this.displayName = _displayName;
			this.id = _id;
			this.peerID = _peerId;
		}

		public string displayName;
		public string id;
		public int peerID;
		public bool isOnline;
	}

    

}


