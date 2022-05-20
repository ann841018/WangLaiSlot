using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bet_Control : MonoBehaviour {

    [SerializeField] Text BetText;

    public static int SwitchNum;
    public static int[] BetNum = new int[11] { 10,20,30,40,50,100,200,300,400,500,1000 };//單線注額
    public static int Line;//幾線25or50
    public int BetRange = 0;

	// Use this for initialization
	void Start ()
    {
        SwitchNum = 0;
        if (PlayerPrefs.GetInt("SceneNum") == 7|| PlayerPrefs.GetInt("SceneNum") == 8) Line = 15;else Line = 25;
        PhotonClient.newPhotonClient.iBet = BetNum[SwitchNum]*Line;
        BetText.text = "總注 " + (BetNum[SwitchNum] * Line).ToString("#,##0");
    }
	
	// Update is called once per frame
	void Update () {}

    public void ChangeBetNum()
    {
        BetRange = 7 - PhotonClient.newPhotonClient.iVIPLv;
        SwitchNum = SwitchNum + 1;
        if (SwitchNum >= 11-BetRange) SwitchNum = 0;
        PhotonClient.newPhotonClient.iBet = BetNum[SwitchNum]*Line;
        BetText.text = "總注 " + (BetNum[SwitchNum] * Line).ToString("#,##0");
    }
}
