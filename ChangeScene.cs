using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int SceneNum;

	// Use this for initialization
	void Start ()
    {
        PlayerPrefs.SetInt("SceneNum", SceneNum);
        if (SceneNum!=1&& SceneNum != 9&&SceneNum != 10) PhotonClient.newPhotonClient.GetSCG(SceneNum);
        else if (SceneNum == 9) PhotonClient.newPhotonClient.GetSCG(7);
        else if (SceneNum == 10) PhotonClient.newPhotonClient.GetSCG(3);
        PhotonClient.newPhotonClient.getGSCed = false;
        PhotonClient.newPhotonClient.isBetSent = false;
        SceneManager.LoadScene(SceneNum);//切場景
    }
}
