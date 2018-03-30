using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;
using GameSparks.RT;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// Chat manager.
/// Created by Sean Durkan for GameSparks Inc 2016 cc
/// </summary>
public class ChatManager : MonoBehaviour {

	public GameObject chatWindow;
	public Button chatToogleBttn;
	private bool isChatWindowOpen;

	public InputField messageInput;
	public Dropdown recipientOption;
	public Button sendMessageBttn;

	public Text chatLogOutput;
	public int elementsInChatLog = 7; 
	private Queue<string> chatLog = new Queue<string>();

	// Use this for initialization
	void Start () {

		chatLogOutput.text = string.Empty; // we want to clear the chat log at the start of the game in case there is any debug text in there
		chatWindow.SetActive (false); // we dont want the chat window to show at the start of the level so we disable it here
		chatToogleBttn.onClick.AddListener (ToogleChatWindow); // assign the toggle-button to the chat-toggle method which will enable and disable the chat window

		// we need to setup the drop-down menu so that it lists the correct names for each player //
		foreach (RTSessionInfo.RTPlayer player in GameSparksManager.Instance().GetSessionInfo().GetPlayerList()) {
			// we dont want to add the option to send a message to ourselves, so we will use our own peerId to exclude this option; we will only be able to send messages to others //
			if (player.peerID != GameSparksManager.Instance().GetRTSession().PeerId) {
				recipientOption.options.Add (new Dropdown.OptionData () { text = player.displayName });
			}
		}
		sendMessageBttn.onClick.AddListener (SendMessage); // we add a listener to the send message button so we can choose what happens when this button is clicked
	}

	/// <summary>
	/// This method will check the player the message is being sent to, and then construct an 
	/// RTData packet and send the packet with the current player's message to the chosen player
	/// </summary>
	private void SendMessage(){

		if (messageInput.text != string.Empty) { // first check to see if there is any message to sent
			// for all RT-data we are sending, we use and instance of the RTData object //
			// this is a disposable object, so we wrap it in this using statement to make sure it is return to the pool //
			using (RTData data = RTData.Get ()) { 
				data.SetString (1, messageInput.text); // we add the message data to the RTPacket at key '1', so we know how to key it when the packet is receieved
				data.SetString(2, DateTime.Now.ToString()); // we are also going to send the time at which the user sent this message

				UpdateChatLog ("Me", messageInput.text, DateTime.Now.ToString ()); // we will update the chat-log for the current user to display the message they just sent

				messageInput.text = string.Empty; // and we clear the message window

				if (recipientOption.options [recipientOption.value].text == "To All") { // we check to see if the packet is sent to all players
					Debug.Log ("Sending Message to All Players... \n" + messageInput.text);
					// for this example we are sending RTData, but there are other methods for sending data we will look at later //
					// the first parameter we use is the op-code. This is used to index the type of data being send, and so we can identify to ourselves which packet this is when it is receieved //
					// the second parameter is the delivery intent. The intent we are using here is 'reliable', which means it will be send via TCP. This is because we arent concerned about //
					// speed when it comes to these chat messages, but we very much want to make sure the whole packet is receieved //
					// the final parameter is the RTData object itself // 
					GameManager.Instance.SendRTData(1, GameSparks.RT.GameSparksRT.DeliveryIntent.RELIABLE, data);
				} else {
					// if we are not sending the message to all players, then we need to search through the players we wish to send it to, so we can get their peerId //
					foreach (RTSessionInfo.RTPlayer player in GameSparksManager.Instance().GetSessionInfo().GetPlayerList()) {
						if (recipientOption.options [recipientOption.value].text == player.displayName) { // check the display name matching the player
							Debug.Log ("Sending Message to " + player.displayName + " ... \n" + messageInput.text);
                            // all methods for sending packets have the option to send to a specific player //
                            // if this option is left out, it will send to all players //
                            // in order to send to a specific player(s) you will need to create an array of ints corresponding to the player's peerId (what we called playerNo in the last tutorial)
                            GameManager.Instance.SendRTData(1, GameSparks.RT.GameSparksRT.DeliveryIntent.RELIABLE, data, new int[] { player.peerID });
						}
					}
				}
			}
		} else {
			Debug.Log ("Not Chat Message To Send...");
		}
	}




	/// <summary>
	/// This is called from the GameSparksManager class.
	/// It send any packets with op-code '1' to the chat manager to the chat manager can parse the nessesary details out for display in the chat log window
	/// </summary>
	/// <param name="_data">Data.</param>
	public void OnMessageReceived(RTPacket _packet)
    {
		Debug.Log ("Message Received...\n"+_packet.Data.GetString(1)); // the RTData we sent the message with used an index '1' so we can parse the data back using the same index
		// we need the display name of the sender. We get this by using the packet sender id and comparing that to the peerId of the player //
		foreach (RTSessionInfo.RTPlayer player in GameSparksManager.Instance().GetSessionInfo().GetPlayerList())
        {
			if (player.peerID == _packet.Sender) {
				// we want to get the message and time and print those to the local users chat-log //
				UpdateChatLog (player.displayName, _packet.Data.GetString (1), _packet.Data.GetString (2));
			}
		}
	}


	/// <summary>
	/// This is called by the chatToggleBttn and it will enable/disable the chat window
	/// </summary>
	private void ToogleChatWindow(){
		isChatWindowOpen = !isChatWindowOpen;
		if (isChatWindowOpen) {
			chatWindow.SetActive (true); // toggle the gameobject on and off
			chatToogleBttn.transform.GetComponentInChildren<Text>().text = "End Chat"; // set the text on the button to show the chat window is on or off
		} else {
			chatWindow.SetActive (false);
			chatToogleBttn.transform.GetComponentInChildren<Text>().text = "Start Chat";
		}
	}


	/// <summary>
	/// This method will update the current users chat log and print the new log to the screen.
	/// </summary>
	/// <param name="_sender">The name (display-name) of the sender</param>
	/// <param name="_message">the body of the message</param>
	/// <param name="_date">the date of the message</param>
	private void UpdateChatLog(string _sender, string _message, string _date){
		// In this example the message we want to display is formatted so that we can distinguish each part of the message when //
		// it is displayed, all the information is clearly visible //
		chatLog.Enqueue("<b>"+_sender+"</b>\n<color=black>"+_message+"</color>"+"\n<i>"+_date+"</i>");
		if (chatLog.Count > elementsInChatLog){ // if we have exceeded the amount of messages in the log then remove the top message
			chatLog.Dequeue ();
		}
		chatLogOutput.text = string.Empty; // we need to clear the log otherwise we always get the same messages repeating
		foreach(string logEntry in chatLog.ToArray()){ // go through each chat item and add the entry to the output log
			chatLogOutput.text += logEntry+"\n";
		}
	}
}



