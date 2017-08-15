using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class ServerBrowser : MonoBehaviour {

    public string url;
    public Transform browserContent;

    public GameObject serverUIInfo;

    bool loginActive = true;
    bool loaded = false;

    void Update () {
        loginActive = GetComponent<LoginController>().loginUi.activeSelf;
        if (!loginActive && !loaded)
        {
            StartCoroutine(getServers());

            loaded = true;
        }
	}

    IEnumerator getServers()
    {
        WWW w = new WWW(url);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
        }
        else
        {
            if (w.text != "" && w.text != "null")
            {
                JSONNode j = JSON.Parse(w.text);
                for (int i = 0; i < j.Count; i++)
                {
                    StartCoroutine(ServerElement(j[i]));
                }
            }
            else
            {
                print("Something went wrong! Couldn't find any server.");
            }
        }
    }

    IEnumerator ServerElement(JSONNode server)
    {
        string name = server["name"];
        string description = server["description"];
        string ip = server["ip"];
        string port = server["port"];

        Ping p = new Ping(ip);
        while (!p.isDone)
        {
            yield return p;
        }
        CreateServerElement(name, description, p.time.ToString(), ip, port);
    }

    void CreateServerElement (string name, string description, string ping, string ip, string port)
    {

        GameObject ui = Instantiate(serverUIInfo) as GameObject;
        ui.transform.SetParent(browserContent);

        Transform t = ui.transform;

        t.GetChild(0).GetComponent<ConnectButton>().ip = ip;
        t.GetChild(0).GetComponent<ConnectButton>().port = port;

        t.GetChild(1).GetComponent<Text>().text = name;
        t.GetChild(2).GetComponent<Text>().text = description;

        t.GetChild(3).GetComponent<Text>().text = ping;
    }
}
