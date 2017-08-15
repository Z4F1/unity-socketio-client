using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectManual : MonoBehaviour {

    public GameObject serverBrowserUI;

    public InputField ipInput;
    public InputField portInput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConnectToServer();
        }
    }

    public void UseIpScreen(bool active)
    {
        serverBrowserUI.SetActive(!active);
    }

    public void ConnectToServer()
    {
        string ip = ipInput.text;
        string port = portInput.text;

        PlayerPrefs.SetString("ip", ip);
        PlayerPrefs.SetString("port", port);
        SceneManager.LoadScene(1);
    }

}
