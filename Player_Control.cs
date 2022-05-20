using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Control : MonoBehaviour
{
    #region//定義
    [SerializeField] int SceneNum;

    //Top裡顯示的數值
    [SerializeField] Image Player_Pic;
    [SerializeField] Text Money_Text;

    //Player_Panel裡顯示的數值
    [SerializeField] Image PlayerPic_Player;//頭像
    [SerializeField] Text NickName_Player;//暱稱
    [SerializeField] Text Money_Player;//金幣
    string[] ProductID = new string[7] { "wanglaislot.coin3000", "wanglaislot.coin15000", "wanglaislot.coin30000", "wanglaislot.coin60000", "wanglaislot.coin150000", "wanglaislot.coin300000", "wanglaislot.coin168000" };

    //Buy_Panel
    [SerializeField] Text[] BuyMoney_Text = new Text[6];//顯示加乘後多得的金幣的數值
    
    [SerializeField] GameObject LeaveGame_Panel;//離開遊戲
    GameObject PhotonClient_GameObject;

    int[] getMoney = new int[7] { 3000, 15000, 30000, 60000, 150000, 300000,168000 };//金幣商品
    int[] VIPmoreMoney = new int[8] {100,125,150,175,200,225,250,300};//會員加成%數

    //撲滿
    [SerializeField] Text Piggy_Money;//撲滿有多少錢
    [SerializeField] Button Piggy;

    //解鎖關卡
    public int LV;//需要LV
    public int VIP;//需要VIP
    [SerializeField] Text Lock_LV;//等級解鎖
    [SerializeField] Text Lock_VIP;//等級解鎖
    [SerializeField] GameObject Lock_L;//等級解鎖
    [SerializeField] GameObject Lock_V;//等級解鎖

    //玩家頭像
    [SerializeField] Image[] Player_New_Pic;
    [SerializeField] GameObject[] Player_Lock;

    #endregion
    // Use this for initialization
    void Start()
    {
        PhotonClient_GameObject = GameObject.FindGameObjectWithTag("PhotonClient");
        if (PhotonClient_GameObject == null){SceneManager.LoadScene(0);}//切場景 
        PlayerPrefs.SetInt("SceneNum", SceneNum);
        if (PlayerPrefs.GetInt("SceneNum") == 1)
        {
            GetPlayerInfo();
            if (PhotonClient.newPhotonClient.iLV >= LV) { Lock_L.SetActive(false); }
            if (PhotonClient.newPhotonClient.iVIPLv >= VIP) { Lock_V.SetActive(false); }
        }
        if (PlayerPrefs.GetInt("Player_Pic") != 0)
        {
            Player_Pic.sprite = Player_New_Pic[PlayerPrefs.GetInt("Player_Pic")].sprite;
            PlayerPic_Player.sprite = Player_New_Pic[PlayerPrefs.GetInt("Player_Pic")].sprite;
        }
        SlotTurnSys.PlayWildAnim = false;
        ChangeMoney();
    }

    // Update is called once per frame
    void Update()
    {
        //Player_Pic.sprite = PlayerPic_Player.sprite;
        Money_Text.text = PhotonClient.newPhotonClient.getUserMoney.ToString("#,##0");
        NickName_Player.text = PhotonClient.newPhotonClient.getNickname;
        Money_Player.text = PhotonClient.newPhotonClient.getUserMoney.ToString("#,##0");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameObject.FindGameObjectWithTag("Panel").name == "Upper_Panel")
            {
                if (PlayerPrefs.GetInt("SceneNum") >= 3) { SceneManager.LoadScene(1); }//切場景
                else if (PlayerPrefs.GetInt("SceneNum") == 1) { LeaveGame_Panel.SetActive(true); }//離開遊戲
            }
        }
        if (Input.GetKeyDown(KeyCode.Home)) { LeaveGame_Panel.SetActive(true); }//離開遊戲
    }
     
    public void ChangeMoney()//不同會員等級可以得到的金幣加乘不同
    {
        for(int i = 0;i<6;i++)
        {
            BuyMoney_Text[i].text = "+ "+(getMoney[i] * (VIPmoreMoney[PhotonClient.newPhotonClient.iVIPLv] -100) / 100).ToString("#,##0");
        }
    }

    public void Buy_Money(int i)//買金幣 //之後跟server要
    {
        PhotonClient.newPhotonClient.SetGooglePurchase(PhotonClient.newPhotonClient.getMemberID, PlayerPrefs.GetString("tokenID"), ProductID[i], PlayerPrefs.GetString("OrderID"));
    }

    public void Lock(int i)//等級1會員2
    {
        if (i == 1){if (PhotonClient.newPhotonClient.iLV >= LV) { Lock_L.SetActive(false);}}
        if (i == 2){if (PhotonClient.newPhotonClient.iVIPLv >= VIP) { Lock_V.SetActive(false);}}
    }

    public void GetPlayerInfo()//讀取資訊(for撲滿)
    {
        PhotonClient.newPhotonClient.GetPlayerInfo();
        if (PhotonClient.newPhotonClient.iBankSave < 168000) { Piggy_Money.text = PhotonClient.newPhotonClient.iBankSave.ToString("#,##0") + "/168,000"; Piggy.interactable=false; }
        else if (PhotonClient.newPhotonClient.iBankSave >= 168000){PhotonClient.newPhotonClient.iBankSave = 168000;Piggy_Money.text = "168,000/168,000"; Piggy.interactable = true; }
    }

    public void Change_Panel()
    {
        for (int i = 0; i < 12; i++)
        {
            if (PlayerPrefs.GetInt("Player_UnLock" + i) == 1)
            {
                Player_New_Pic[i].color = Color.white;
                Player_Lock[i].SetActive(false);
            }
        }
    }

    public void UnLock_Pic(int i)
    {
        Player_Lock[i].SetActive(false);
        //PlayerPrefs.SetInt("Player_UnLock"+i, 1);
    }

    public void Change_Pic(int i)
    {
        Player_Pic.sprite = gameObject.GetComponent<Image>().sprite;
        PlayerPic_Player.sprite = gameObject.GetComponent<Image>().sprite;
        //Player_Pic.sprite = Player_New_Pic[i].sprite;
        //PlayerPic_Player.sprite = Player_New_Pic[i].sprite;
        PlayerPrefs.SetInt("Player_Pic", i);
    }
}
