using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackPot_Control : MonoBehaviour {

    [SerializeField] Text JP_Text;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {
        JP_Text.text = PhotonClient.newPhotonClient.JP_Pool.ToString("#,##0");
    }
}
