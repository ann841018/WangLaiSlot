using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogOut : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        PlayerPrefs.DeleteAll();
        NickName.WrongNickName = false;
        NickName.OK = false;
        PhotonClient.newPhotonClient.LogOut();
        PhotonClient.newPhotonClient.getNickname = "";
        PlayerPrefs.SetInt("SceneNum", 0);
        SceneManager.LoadScene(2);//切場景
    }
	
	// Update is called once per frame
	void Update () {}
}
