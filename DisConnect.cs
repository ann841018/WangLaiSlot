using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisConnect : MonoBehaviour {
    
    // Use this for initialization
    void Start ()
    {
        PhotonClient.newPhotonClient.ReConnect();
        SceneManager.LoadScene(1);//切場景
    }
	
	// Update is called once per frame
	void Update () {}
}
