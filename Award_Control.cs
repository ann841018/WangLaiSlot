using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Award_Control : MonoBehaviour {
        
    [SerializeField] Text Award_Text;//跑馬燈
    public static bool CanShowAward;
    string GameName;
    int MinX = -2000;
    float time;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (CanShowAward == true)
        {
            if (PhotonClient.newPhotonClient.szAwards == "CookGirl") GameName = "中華料理娘";
            else if (PhotonClient.newPhotonClient.szAwards == "MagicGirl") GameName = "奇幻魔術師";
            else if (PhotonClient.newPhotonClient.szAwards == "ComeCat") GameName = "招財御守喵";
            else if (PhotonClient.newPhotonClient.szAwards == "EgyptKing") GameName = "埃及公主";
            else if (PhotonClient.newPhotonClient.szAwards == "AlsDream") GameName = "綠野仙蹤";
            Award_Text.text = "恭喜玩家："+PhotonClient.newPhotonClient.szName + " 在" + GameName + "　贏得彩金 " + PhotonClient.newPhotonClient.iMoney.ToString("#,##0");
            time = time + Time.deltaTime;
            if (time < 20)
            {
                if (Award_Text.gameObject.transform.localPosition.x <= MinX) { Award_Text.gameObject.transform.localPosition = new Vector2(-MinX+700, Award_Text.gameObject.transform.localPosition.y); }
                Award_Text.gameObject.transform.Translate(-3, 0, 0);
            }else if (time >= 20) { Award_Text.text = " "; CanShowAward = false; }
        }else if (CanShowAward == false){time = 0;}
    }

    public static void ShowAward()
    {
        CanShowAward = true;
    }
}
