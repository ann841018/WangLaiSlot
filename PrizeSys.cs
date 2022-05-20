using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeSys : MonoBehaviour
{
    [SerializeField] GameObject[] Prize = new GameObject[5];//大獎
    [SerializeField] GameObject[] LevelAnim = new GameObject[2];//特殊關卡
    [SerializeField] GameObject PrizePanel;//大獎
    [SerializeField] GameObject LevelPanel;//關卡
    [SerializeField] GameObject BonusPanel;//抽蒸籠
    [SerializeField] GameObject FreeSpin_Square;

    [SerializeField] AudioSource Win_Sound;
    [SerializeField] AudioSource Stage_Sound;
    [SerializeField] Text Win_text;//大獎文字

    public static int PrizeNum = -1;//大獎編號0Big 1Mega 2Epic 3Ultra 4Jackpot 
    public static float time;
    bool PlaySound;

    void Start () {}// Use this for initialization
	
	// Update is called once per frame
	void FixedUpdate () {
        if (SlotTurnSys.CanTurn == true){PlaySound = true;}
        else if (SlotTurnSys.CanTurn == false)
        {
            if (PhotonClient.newPhotonClient.iLVNum == 0&& (PhotonClient.newPhotonClient.Bonus_Money == 0)) { FreeSpin_Square.SetActive(false); }//如果不是關卡
            if (PhotonClient.newPhotonClient.iAllFG == 3) { if (PlaySound == true){Stage_Sound.Play();PlaySound = false; }}
            if (Alice_BonusGame_Control.Bonus_Prize == true)
            {
                if ((time < 0.5f) && (time >= 0))time = time + Time.deltaTime;                   
                if (time >= 0.5f){
                    PlayPrizeAnimation(PhotonClient.newPhotonClient.BonusNum);
                    time = -1;
                }
            }
            if (SlotTurnSys.time > SlotTurnSys.PlayAnimWait)//等動畫播完
            {
                if (PrizeNum >= 0) { PlayPrizeAnimation(PrizeNum); }//大獎動畫
                if (SlotTurnSys.PlayBonusGame == true)//Bonus動畫
                {
                    if (SlotTurnSys.time < (SlotTurnSys.PlayAnimWait + 0.8f))
                    {
                        if (SlotTurnSys.time < (SlotTurnSys.PlayAnimWait + 0.2f)) Stage_Sound.Play();
                        LevelPanel.SetActive(true);//動畫
                        LevelAnim[0].SetActive(true);//Bonus Game動畫

                    }
                    if (SlotTurnSys.time > (SlotTurnSys.PlayAnimWait + 1.5f))//等動畫播完
                    {
                        CookGirl_BonusGame_Control.StartCountDown();//開始倒數
                        BonusPanel.SetActive(true);//Bonus Game頁面
                        FreeSpin_Square.SetActive(true);//特效框
                        SlotTurnSys.PlayBonusGame = false;
                    }
                }
            }
        }
    }

    public static void SpecialLevel(int Level, int LevelCount)// 全盤獎(0.沒有 1.JP 2Free 3上菜)
    {
        switch (Level)
        {
            case 1://JPWin
                if (SlotTurnSys.AutoPlay == true) { if (SlotTurnSys.CanTurn == false) { Spin_Control.StopAuto = true; SlotTurnSys.AutoPlay = false; } }
                PrizeNum = 4;
                break;
            case 2://BonusGame
                SlotTurnSys.PlayBonusGame = true;
                break;
            case 3://Wild
                SlotTurnSys.PlayWildAnim = true;
                break;
        }
    }
    public static void PrizeAnimation(int GetMoney)//判斷是哪一個大獎
    {
        if (GetMoney / (Bet_Control.BetNum[Bet_Control.SwitchNum] * Bet_Control.Line) >= 50) { PrizeNum = 3; }
        else if (GetMoney / (Bet_Control.BetNum[Bet_Control.SwitchNum] * Bet_Control.Line) >= 30) { PrizeNum = 2; }
        else if (GetMoney / (Bet_Control.BetNum[Bet_Control.SwitchNum] * Bet_Control.Line) >= 20) { PrizeNum = 1; }
        else if (GetMoney / (Bet_Control.BetNum[Bet_Control.SwitchNum] * Bet_Control.Line) >= 10) { PrizeNum = 0; }
    }
    public void PlayPrizeAnimation(int i)//播大獎動畫
    {
        for (int j = 0; j < 5; j++) { if (j != i) Prize[j].SetActive(false); }
        if (PhotonClient.newPhotonClient.Bonus_Money == 0)
        {
            if (i != 4) { StartCoroutine(Money_Str(i, PhotonClient.newPhotonClient.Line_Money)); }//數值切換
            else if (i == 4) { StartCoroutine(Money_Str(i, PhotonClient.newPhotonClient.JP_Money)); }//數值切換
        }else if (PhotonClient.newPhotonClient.Bonus_Money != 0) StartCoroutine(Money_Str(i, PhotonClient.newPhotonClient.Bonus_Money));//Bonus直接送錢
        PrizePanel.SetActive(true);
        Prize[i].SetActive(true);
        print(PhotonClient.newPhotonClient.Bonus_Money);
        Win_Sound.Play();
        PrizeNum = -1;
    }
    private IEnumerator Money_Str(int Prize, int GetMoney)
    {
        if (SlotTurnSys.AutoPlay == true)
        {
            SetMoneyStr(GetMoney);//設定進度條資訊
        }
        else if (SlotTurnSys.AutoPlay == false)
        {
            int i = 0;//顯示出來的數值
            while (i < GetMoney)
            {
                if (Prize != 4)
                {
                    if (i < GetMoney * 0.9) i = (int)(GetMoney * 0.9);
                    else if (i < GetMoney)
                    {
                        if ((GetMoney * 0.01) % 100 == 0) i = i + (int)(GetMoney * 0.01);
                        else if ((GetMoney - i) % 100 == 0) i = i + 100;
                        else if ((GetMoney - i) % 10 == 0) i = i + 10;
                        else i = i + 1;
                    }//顯示出來的數值
                }else if (Prize == 4)
                {
                    if (i < GetMoney - 1000) i = GetMoney - 1000;
                    else if (i < GetMoney) {
                        if ((GetMoney - i) % 10 == 0) i = i + 10;
                        else i = i + 1;
                    }else if (i > GetMoney) i = GetMoney;
                    //顯示出來的數值
                }
                SetMoneyStr(i);//設定進度條資訊
                yield return new WaitForEndOfFrame();//一幀一幀跑
            }
            if (i > GetMoney) i = GetMoney;
            SetMoneyStr(i);//設定進度條資訊
        }
    }
    void SetMoneyStr(int Money) { Win_text.text = Money.ToString("#,##0"); }
}
