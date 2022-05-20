using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift_Control : MonoBehaviour {

    public static bool HaveGift;

    [SerializeField] GameObject Wheel_Panel;
    [SerializeField] GameObject Shine;
    [SerializeField] Text LoginMoney;
    [SerializeField] Text[] WheelMoney = new Text[12];

    int[,] VIPGet = new int[8, 12]{
        { 300, 400, 500, 600, 800, 900,1100,1200,1400,1500,1600,1700}, { 400, 500, 600, 700, 900,1100,1300,1400,1600,2000,2200,2300},
        { 500, 600, 700, 800,1000,1200,1800,2000,2200,2300,2400,2500}, {1000,1100,1200,1300,1400,1500,1600,1700,2000,2500,2700,3000},
        {1100,1200,1300,1400,1500,1700,2000,2200,2500,2800,3000,3300}, {1600,1800,2100,2400,2500,2800,3000,3300,3600,3900,4000,5000},
        {2500,3500,4000,4500,5000,5500,6000,6500,7500,8000,9000,10000},{3000,4000,4500,5000,6000,6500,7000,8000,9000,10000,15000,30000}};
    int[] Wheel_Num = new int[12] {1,11,3,9,5,7,2,10,4,8,6,0};
    int GetNum;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < 12; i++) {
            WheelMoney[i].text = VIPGet[PhotonClient.newPhotonClient.iVIPLv, i].ToString("#,##0");
            if (VIPGet[PhotonClient.newPhotonClient.iVIPLv, i] == PhotonClient.newPhotonClient.iGetLoginRewardMoney) GetNum = Wheel_Num[i];
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (HaveGift == true){Wheel_Panel.SetActive(true);}
    }

    public void Stop()
    {
        Shine.GetComponent<Animator>().SetInteger("Wheel_Num", GetNum);
        LoginMoney.text = PhotonClient.newPhotonClient.iGetLoginRewardMoney.ToString("#,##0");
    }

    public void GetMoney()
    {
        PhotonClient.newPhotonClient.getUserMoney= PhotonClient.newPhotonClient.RealMoney;
        HaveGift = false;
    }
}
