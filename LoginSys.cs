using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : MonoBehaviour
{
    [SerializeField] GameObject Main_Panel;//主要頁面
    [SerializeField] GameObject P8_Panel;//創帳號頁面
    [SerializeField] GameObject Creat_Panel;//創帳號頁面

    public static bool NickName;
    public static bool StartGame_Bool;
    public bool OpenPanel;
    
    // Use this for initialization
    void Start () {PlayerPrefs.SetInt("SceneNum", 0);}
	
	// Update is called once per frame
	void Update ()
    {
        if (OpenPanel==true)
        {
            Main_Panel.SetActive(false);
            P8_Panel.SetActive(true);
            OpenPanel = false;
        }
        if (NickName == true) { OpenNickName(); }
        if (StartGame_Bool == true) { StartGame(); }
    }

    public void StartGame()
    {
        if (PlayerPrefs.GetInt("haveID") == 0)//沒有帳號
        {
            if (PhotonClient.newPhotonClient.ServerConnected)
            {
                Creat_Panel.SetActive(true); //創帳號頁面
                PhotonClient.newPhotonClient.Creat("", "");
                StartGame_Bool = false;
            }
        }
        else if (PlayerPrefs.GetInt("haveID") == 1) { PhotonClient.newPhotonClient.LogIn(); StartGame_Bool = false; }//有帳號 登入
    }

    public void OpenNickName()
    {
        if (PhotonClient.newPhotonClient.ServerConnected) Creat_Panel.SetActive(true);//創帳號頁面
        NickName = false;
    }

    public void Open()
    {
        OpenPanel = true;
    }
}