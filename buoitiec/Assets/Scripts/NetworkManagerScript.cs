using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {
	
	public static string gameTypeName = "Buoi_tiec_cua_Huy";
	public static string gameName = "Huy's Party";
	public GameObject playerPrefab;
	public Transform spawnPosition;

	float btnX;
	float btnY;
	float btnW;
	float btnH;

	string msg;
	HostData[] hostData;
	int numPlayerPerRoom = 2;

	private GameObject myPlayer = null;
	int playerGroup = 0;

	// Use this for initialization
	void Start () {
		btnX = Screen.width * 0.1f;
		btnY = Screen.height * 0.1f;
		btnW = Screen.width * 0.2f;
		btnH = Screen.height * 0.2f;
		msg = "None";
	}

	void startServer(int id){
		Network.InitializeServer(numPlayerPerRoom, 25002, ! Network.HavePublicAddress());
		MasterServer.RegisterHost(gameTypeName, gameName+ " "+ id, "Test Unity");
	}

	void OnServerInitialized(){
		msg = "Server initialized!";
		// connected, invalidate hostData
		hostData = null;
		spawnPlayer();
	}

	void OnConnectedToServer(){
		msg = "Connected to server";
		// connected, invalidate hostData
		hostData = null;
		spawnPlayer();
	}

	void OnMasterServerEvent(MasterServerEvent mse){
		if (mse == MasterServerEvent.RegistrationSucceeded){
			msg = "Server Registered!";
		}
	}

	void spawnPlayer(){
		if (myPlayer != null)
			Destroy(myPlayer);
		
		myPlayer = (GameObject) Network.Instantiate(playerPrefab, spawnPosition.position, 
		                                            Quaternion.identity, playerGroup);
	}
	
	IEnumerator getServerList(){
		MasterServer.RequestHostList(NetworkManagerScript.gameTypeName);
		yield return new WaitForSeconds(1.5f);
		hostData = MasterServer.PollHostList();
	}
	
	void Update(){

	}


	// Update is called once per frame
	void OnGUI () {
		if (!Network.isServer && !Network.isClient){

			// start of the game
			if(GUI.Button(new Rect(btnX, btnY,btnW,btnH), "Check Room")){
				msg = "Check Room!";
				StartCoroutine("getServerList");
			}

			if (hostData != null){
				
				int i =0;
				for (i=0; i<hostData.Length; i++){
					if (GUI.Button(new Rect(btnX + btnW, btnY + btnH*i, btnW, btnH), hostData[i].gameName)){
						Network.Connect(hostData[i]);

					}
				}
				
				// create own room
				if (GUI.Button(new Rect(btnX + btnW, btnY + btnH*i, btnW, btnH), "Create Own Room")){
					startServer(hostData.Length);
				}
			}

		}

		// Already in game
		else{
			if(GUI.Button(new Rect(btnX, btnY,btnW,btnH), "Disconnect")){
				Network.RemoveRPCs(myPlayer.GetComponent<NetworkView>().viewID);
				Network.Destroy(myPlayer.GetComponent<NetworkView>().viewID);
				myPlayer = null;
				Network.Disconnect();
				msg = "Disconnected!";
				hostData = null;
//				networkView.RPC("setMessage", RPCMode.All, "S2C: Server here!");
			}
		}

		GUI.Label(new Rect(btnX, btnY + btnH, btnW, btnH), msg);

	}
	
	[RPC]
	void setMessage(string newMsg){
		msg = newMsg;
	}
}
