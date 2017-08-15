using UnityEngine;
using SocketIO;

public class ServerConnecting : MonoBehaviour {
	
	void Awake () {
        string ip = PlayerPrefs.GetString("ip");
        string port = PlayerPrefs.GetString("port");
        /*string ip = "127.0.0.1";
        string port = "7701";*/
        GetComponent<SocketIOComponent>().url = "ws://" + ip + ":" + port + "/socket.io/?EIO=4&transport=websocket";
    }
}
