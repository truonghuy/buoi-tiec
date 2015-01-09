using UnityEngine;
using System.Collections;

public class NetworkClientScript : MonoBehaviour {

	float btnX;
	float btnY;
	float btnW;
	float btnH;

	bool refreshing;
	string msg;
	HostData[] hostData;
	
	// Use this for initialization
	void Start () {
		btnX = Screen.width * 0.1f;
		btnY = Screen.height * 0.1f;
		btnW = Screen.width * 0.2f;
		btnH = Screen.height * 0.2f;
		refreshing = false;
		msg = "No";
		hostData = null;
	}
	
	void getServerList(){
		MasterServer.RequestHostList(NetworkManagerScript.gameTypeName);
		refreshing = true;
	}

	void Update(){
		if (refreshing){
			if (MasterServer.PollHostList().Length > 0){
				refreshing = false;
				msg = "Found server!";
				hostData = MasterServer.PollHostList();
			}
		}
	}

	// Update is called once per frame
	void OnGUI () {

		// not connected
		if (!Network.isClient){
			if(GUI.Button(new Rect(btnX, btnY,btnW,btnH), "Get Server")){
				Debug.Log("Get Server!");
				getServerList();
			}

			if (hostData != null){
				for (int i=0; i<hostData.Length; i++){
					if (GUI.Button(new Rect(btnX + btnW, btnY + btnH*i, btnW, btnH), hostData[i].gameName)){
						Network.Connect(hostData[i]);
					}
				}
			}
		}
		// connected here
		else{
			// create buttons to send message
			if (GUI.Button(new Rect(btnX, btnY,btnW,btnH), "Msg to Server")){
				networkView.RPC("setMessage", RPCMode.All, "C2S: Client here!");
			}
		}

		GUI.Label(new Rect(btnX, btnY + btnH, btnW, btnH), msg);
	}
	[RPC]
	void setMessage(string newMsg){
		msg = newMsg;
	}
}
