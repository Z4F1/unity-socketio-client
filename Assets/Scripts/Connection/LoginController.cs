using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class LoginController : MonoBehaviour {

    public GameObject loginUi;

    public InputField user;
    public InputField pass;
    public Toggle remember;

    public Text feedBack;

    public string url;

    private void Awake()
    {
        if(PlayerPrefs.GetString("rememberme") == "True")
        {
            user.text = PlayerPrefs.GetString("name");
            pass.text = PlayerPrefs.GetString("password");
            remember.isOn = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartLogin();
        }
    }

    // Use this for initialization
    public void StartLogin () {
        StartCoroutine(Login(user.text, pass.text));
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        WWW w = new WWW(url, form);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            feedBack.text = "Something went wrong with the connection!";
            print(w.error);
        }else
        {
            if(w.text != "" && w.text != "null")
            {
                UserInfo user = JsonUtility.FromJson<UserInfo>(w.text);
                feedBack.text = "Logging you in right now!";

                PlayerPrefs.SetString("unique_id", user.unique_id);
                PlayerPrefs.SetString("name", username);

                print(remember.isOn.ToString());

                if (remember.isOn)
                {
                    PlayerPrefs.SetString("password", password);
                    PlayerPrefs.SetString("rememberme", remember.isOn.ToString());
                }else
                {
                    PlayerPrefs.SetString("rememberme", remember.isOn.ToString());
                }
                yield return new WaitForSeconds(.6f);
                loginUi.SetActive(false);
            }
            else
            {
                feedBack.text = "Username or/and password is wrong!";
            }
        }
    }


}
