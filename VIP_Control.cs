using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VIP_Control : MonoBehaviour
{
    int needExp;//讀需求點數的數值

    [SerializeField] GameObject VIPExpBar;//VIP頁面的經驗值條
    [SerializeField] GameObject VIPLvUpAnim;//升等動畫

    [SerializeField] Image VIPLvPicOld;//舊等級數字
    [SerializeField] Image VIPLvPicNew;//新等級數字
    [SerializeField] Image VIPLvUp_Num;//升等數字
    [SerializeField] Image[] VIPMask = new Image[8];//等級欄

    [SerializeField] Text VIPTextOnPlayerPanel;//玩家設定上的文字
    [SerializeField] Text VIPText;//大廳上的文字
    [SerializeField] Sprite[] VIPLv_Num = new Sprite[9];//數字

    int[] needPoint = new int[7] {30,90,300,3000,6000,8000,9000};//需求點數

    public static bool VIPLevelUp;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i <= 7; i++)
        {
            if ((PhotonClient.newPhotonClient.iVIPLv == i))//等級的那一欄
            {
                VIPMask[i].color = new Color(0, 0, 0, 0.4f);//顏色加暗
                VIPLvPicOld.sprite = VIPLv_Num[PhotonClient.newPhotonClient.iVIPLv];//舊等級數字
                if (i <= 7) VIPLvPicNew.sprite = VIPLv_Num[PhotonClient.newPhotonClient.iVIPLv + 1];//新等級數字
            }
            else VIPMask[i].color = new Color(0, 0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        VIPText.text = PhotonClient.newPhotonClient.iVIPLv.ToString();//主畫面上的Lv
        if (PhotonClient.newPhotonClient.iVIPLv < 7)
        {
            needExp = needPoint[PhotonClient.newPhotonClient.iVIPLv];//點數需求
            VIPExpBar.transform.localPosition = new Vector3((850 * PhotonClient.newPhotonClient.iVIPExp / needExp - 425), 0, 0);//移動進度條

            if (VIPLevelUp==true)//現在有的點數符合升級點數
            {
                VIPLvUpAnim.SetActive(true);//升等動畫
                VIPLvUp_Num.sprite = VIPLv_Num[PhotonClient.newPhotonClient.iVIPLv];//升等數字           
                VIPLevelUp = false;
            }

        }else if (PhotonClient.newPhotonClient.iVIPLv == 7)//VIP滿等
        {
            VIPExpBar.transform.localPosition = new Vector3(425, 0, 0);//移動進度條
        }
        VIPTextOnPlayerPanel.text = "VIP "+PhotonClient.newPhotonClient.iVIPLv.ToString();//PlayerPanel上的VIPLv 
    }

    public void VIPLvMask()//會員等級遮罩
    {
        for (int i = 0; i <= 7; i++)
        {
            if ((PhotonClient.newPhotonClient.iVIPLv == i))//等級的那一欄
            {
                VIPMask[i].color = new Color(0, 0, 0, 0.4f);//顏色加暗
                VIPLvPicOld.sprite = VIPLv_Num[PhotonClient.newPhotonClient.iVIPLv];//舊等級數字
                if(i<=7)VIPLvPicNew.sprite = VIPLv_Num[PhotonClient.newPhotonClient.iVIPLv + 1];//新等級數字
            }
            else VIPMask[i].color = new Color(0, 0, 0, 0);
        }
    }
}