using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SimpleJSON;

[Serializable]
public class UserInfo
{
    public string username;
    public string unique_id;

    public GameObject go;

    public Vector3 positon;
    public float rotation;

    public UserInfo(string unique, GameObject g, string name, Vector3 pos, float rot)
    {
        unique_id = unique;
        username = name;
        positon = pos;
        rotation = rot;
        go = g;
    }

    public void UpdateTransform(Vector3 pos, float rot)
    {
        positon = pos;
        rotation = rot;
    }
}

public class PlayerUpdate : MonoBehaviour {

    private SocketIOComponent socket;
    public Transform player;
    public bool updated = false;

    public GameObject others;

    public List<UserInfo> userList = new List<UserInfo>();

    public void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("open", OnOpen);
        socket.On("online-users", OnUserUpdate);
        socket.On("socket-position", OnSocketPos);
        socket.On("error", OnError);
        socket.On("close", OnClose);

        StartCoroutine(Starter());
    }

    private void FixedUpdate()
    {

        for (int i = 0; i < userList.Count; i++)
        {
            GameObject g = userList[i].go;
            Vector3 pos = userList[i].positon;

            g.transform.position = pos;
            g.transform.rotation = Quaternion.Euler(0, userList[i].rotation, 0);

            userList[i].go = g;
        }

    }

    IEnumerator Starter()
    {
        yield return new WaitForSeconds(0.1f);

        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("name", PlayerPrefs.GetString("name"));
        j.AddField("unique_id", PlayerPrefs.GetString("unique_id"));

        socket.Emit("user-info", j);
    }

    public void UpdateMyTransform(Vector3 pos, float rot)
    {
        string unique_id = PlayerPrefs.GetString("unique_id");

        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("id", unique_id);
        j.AddField("x", pos.x);
        j.AddField("y", pos.y);
        j.AddField("z", pos.z);
        j.AddField("rot", rot);

        socket.Emit("updatePlayer", j);
    }

    public void OnUserUpdate(SocketIOEvent e)
    {
        JSONNode j = JSON.Parse(e.data.ToString());

        if(PlayerPrefs.GetString("unique_id") != j["id"].Value)
        {
            float x = float.Parse(j["x"].Value);
            float y = float.Parse(j["y"].Value);
            float z = float.Parse(j["z"].Value);

            bool exist = false;
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].unique_id == j["id"].Value)
                {
                    if (j["online"].Value == "false")
                    {
                        Destroy(userList[i].go.gameObject);
                        userList.RemoveAt(i);
                    }

                    userList[i].UpdateTransform(new Vector3(x, y, z), float.Parse(j["rot"].Value));
                    exist = true;
                }
            }

            if (!exist)
            {
                GameObject g = Instantiate(others);
                g.transform.name = j["name"].Value;
                userList.Add(new UserInfo(j["id"].Value, g, j["name"].Value, new Vector3(x, y, z), float.Parse(j["rot"].Value)));
            }
        }
    }

    public void OnSocketPos(SocketIOEvent e)
    {
        JSONNode j = JSON.Parse(e.data.ToString());

        float x = float.Parse(j["x"].Value);
        float y = float.Parse(j["y"].Value);
        float z = float.Parse(j["z"].Value);
        float rot = float.Parse(j["rot"].Value);

        print("looking");

        player.position = new Vector3(x, y, z);
        player.rotation = Quaternion.AngleAxis(rot, Vector3.up);

        updated = true;
    }

    #region Standard SocketIOevents

    public void OnOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void OnBoop(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);
    }

    public void OnError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void OnClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }
    #endregion
}
