using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectButton : MonoBehaviour {

    public string ip = "127.0.0.1";
    public string port = "7701";

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ConnectToServer);
    }

    void ConnectToServer()
    {
        PlayerPrefs.SetString("ip", ip);
        PlayerPrefs.SetString("port", port);
        SceneManager.LoadScene(1);
    }
}
