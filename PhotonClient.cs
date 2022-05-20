using UnityEngine.UI;
using UnityEngine;
using System;
using System.Security;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;//  (注一) 引用 PHOTON CLIENT 元件
using Protocol;// (注二)引用Protocol.dll 定義

public class PhotonClient : MonoBehaviour, IPhotonPeerListener// (注三) 物件繼承
{
    #region//定義
    public bool Out;

    //string ServerAddress = "210.64.126.67:5055";// (注四) 宣告IP位置 跟 PORT 都是5055//外網
    string ServerAddress = "10.0.0.145:5055";// (注四) 宣告IP位置 跟 PORT 都是5055////內網
    string ServerApplication = "GBTServer";// (注五) 傳送目標 SERVER 名稱
    public static PhotonClient newPhotonClient;
    public bool OpenLoadingPanel;//打開跑進度頁面

    [SerializeField] GameObject DisConnect_Panel;//無連線頁面
    GameObject DisConnect_Panel_Clone;//生成無連線頁面

    internal protected PhotonPeer peer;// 宣告傳輸物件
    public bool ServerConnected;
    public bool getGSCed;
    public bool isBetSent;

    bool ReConnectWork;
    
    // 取得Server回傳值用的變數
    public bool LoginStatus;
    public string getMemberID = "";//帳號
    public string getGameID = "";//帳號//加密前
    public string getNickname = "";//暱稱
    public string getFBKey = "";
    public string getP8Key = "";
    public int getUserMoney = 0;//金幣

    public int iLV = 0;//等級
    public int iVIPLv;//會員等級
    public int iEXP = 0;//經驗值
    public int iVIPTotalExp = 0;//會員點數
    public int iVIPExp = 0;//會員點數
    public int iBankSave = 0;//撲滿存了多少錢
    int[] VIPNeedExp = new int[8] { 0, 30, 180, 480, 1380, 4380, 10380, 19380 };

    public int iLestLV = -1;// 前一盤是否為關卡
    public int iLVNow = 0;// 送過來資訊的這盤是否為關卡
    public int iLVNum;
    public int iAllFG = -1;
    public int iBet = 0;

    //public int iLevelUpBonus = 999;//升等獎勵
    public int JP_Money;// 中JP的錢
    public int Line_Money;//一般盤, 中獎金額
    public int Bonus_Money;//BonusGame直接給的錢
    public int JP_Pool;//JP獎池的錢
    public int TimeNum;//乘倍
    public int BonusNum;//BonusGame的數值

    //每日登入
    public int RealMoney;//實際上的錢
    public int DailyMoney;//登入獎金
    public int iGetLoginRewardMoney;//登入獎領了多少
    public int iGetLoginReward;//每日登入獎勵是否開啟

    //信箱
    public int Mail_Num;
    public int Mail_Money;
    public string Mail_ID;
    public string Mail_Name;
    public string Mail_Title;
    public string Mail_Info;
    public bool SendSuccess = true;
    public int SendSuccessCode;

    public int Test;

    [HideInInspector] public int i5FreeNum = 0;
    [HideInInspector] public int i4FreeNum = 0;
    [HideInInspector] public int i3FreeNum = 0;
    [HideInInspector] public int i5Jokerum = 0;
    [HideInInspector] public int i5ANum = 0;
    [HideInInspector] public int i5KNum = 0;
    [HideInInspector] public int i5QNum = 0;
    [HideInInspector] public int i5JNum = 0;
    [HideInInspector] public int i510Num = 0;
    [HideInInspector] public int i59Num = 0;
    [HideInInspector] public int iPoor = 0;

    [HideInInspector] public int[] islot3x5 = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };//15格數字
    [HideInInspector] public int[] iLine25 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//25線

    [HideInInspector] public string szMSG;
    [HideInInspector] public string szName;
    [HideInInspector] public string szAwards;
    [HideInInspector] public int iMoney;
    [HideInInspector] public int iType;
  
    public string LoginResult = "";// 登入失敗的回傳字串
    #endregion

    void Awake() {
        if (Out == true) ServerAddress = "210.64.126.67:5055";
        else if(Out==false) ServerAddress = "10.0.0.145:5055";
        DontDestroyOnLoad(gameObject); }
    void Start()
    {
        newPhotonClient = this;       
        this.ServerConnected = false;// 呼叫連線
        this.LoginStatus = false;
        if (this.ServerConnected == false)
        {
            this.ServerConnected = false;
            this.LoginStatus = false;
            this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            this.Connect();
        }
    }
    internal virtual void Connect()// (注六)  虛擬繼承涵式 連線開始
    {
        try { this.peer.Connect(ServerAddress, this.ServerApplication); }
        catch (SecurityException se) { this.DebugReturn(0, "Connection Failed. " + se.ToString()); }
    }
    void Update()// (注七)  虛擬繼承更新 這會一直跑進來
    {
        this.peer.Service();// 固定要呼叫的方法
        if (this.ServerConnected)
        {
            if (ReConnectWork == true)//重新連線過後
            {
                this.LogIn();//重新登入
                this.GetPlayerInfo();//重新傳玩家資料
                ReConnectWork = false;
            }
        }
    }
    public void DebugReturn(DebugLevel level, string message) { Debug.Log(message); }
    public void OnOperationResponse(OperationResponse operationResponse)// (注八) 連線後 收命令的地方
    {
        // display operationCode
        this.DebugReturn(0, string.Format("OperationResult:" + operationResponse.OperationCode.ToString()));
        switch (operationResponse.OperationCode)
        {
            #region//PlayerInfo
            case (byte)Command.PWC_UserIDLogin://6
                {
                    if (operationResponse.ReturnCode == 1)  // if success// 取得Server的回傳值
                    {
                        print("UserIDLogin");
                        OpenLoadingPanel = true;
                        
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        getGameID = Convert.ToString(operationResponse.Parameters[1]);
                        getNickname = Convert.ToString(operationResponse.Parameters[3]);
                        getFBKey = Convert.ToString(operationResponse.Parameters[4]);
                        getP8Key = Convert.ToString(operationResponse.Parameters[5]);
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[6]);
                        RealMoney = getUserMoney;
                        iLV = Convert.ToInt32(operationResponse.Parameters[9]);
                        iEXP = Convert.ToInt32(operationResponse.Parameters[10]);
                        iGetLoginRewardMoney = Convert.ToInt32(operationResponse.Parameters[15]);// 登入領了多少 登入獎勵 0 是今天領過了 或是活動未開啟
                        DailyMoney = Convert.ToInt32(operationResponse.Parameters[18]);    // 登入獎領是多少
                        iGetLoginReward = Convert.ToInt32(operationResponse.Parameters[20]);//每日登入獎勵是否開啟
                        iVIPLv = Convert.ToInt32(operationResponse.Parameters[22]); //VIP等級1 - 10  0 是 1喔
                        iVIPTotalExp = Convert.ToInt32(operationResponse.Parameters[23]);
                        iBankSave = Convert.ToInt32(operationResponse.Parameters[24]);

                        iVIPExp = iVIPTotalExp - VIPNeedExp[iVIPLv]; //VIP總經驗值
                        LoginStatus = true;
                        if (iGetLoginRewardMoney != 0) { getUserMoney = getUserMoney - DailyMoney; Gift_Control.HaveGift = true; }
                        if (getNickname == "NewUser0001") LogOut();
                    }
                    else
                    {
                        print(operationResponse.ReturnCode);
                        if (operationResponse.ReturnCode == -99) {
                            if ((getP8Key != "") && (getP8Key != "0")) Creat("", getP8Key);
                            else if ((getFBKey != "") && (getFBKey != "0"))  Creat(getFBKey, "");
                            LoginSys.NickName = true;
                        }                       
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_UserFBLogin://7
                {
                    if (operationResponse.ReturnCode == 1)  // if success// 取得Server的回傳值
                    {
                        print("UserIDLogin");
                        OpenLoadingPanel = true;

                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        getGameID = Convert.ToString(operationResponse.Parameters[1]);
                        getNickname = Convert.ToString(operationResponse.Parameters[3]);
                        getFBKey = Convert.ToString(operationResponse.Parameters[4]);
                        getP8Key = Convert.ToString(operationResponse.Parameters[5]);
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[6]);
                        RealMoney = getUserMoney;
                        iLV = Convert.ToInt32(operationResponse.Parameters[9]);
                        iEXP = Convert.ToInt32(operationResponse.Parameters[10]);
                        iGetLoginRewardMoney = Convert.ToInt32(operationResponse.Parameters[15]);// 登入領了多少 登入獎勵 0 是今天領過了 或是活動未開啟
                        DailyMoney = Convert.ToInt32(operationResponse.Parameters[18]);    // 登入獎領是多少
                        iGetLoginReward = Convert.ToInt32(operationResponse.Parameters[20]);//每日登入獎勵是否開啟

                        iVIPLv = Convert.ToInt32(operationResponse.Parameters[22]); //VIP等級1 - 10  0 是 1喔
                        iVIPTotalExp = Convert.ToInt32(operationResponse.Parameters[23]);
                        iVIPExp = iVIPTotalExp - VIPNeedExp[iVIPLv]; //VIP總經驗值
                        LoginStatus = true;
                        if (iGetLoginRewardMoney != 0) { getUserMoney = getUserMoney - DailyMoney; Gift_Control.HaveGift = true; }
                    }
                }
                break;
            case (byte)Command.PWC_UserP8Login://8
                {
                    if (operationResponse.ReturnCode == 1)  // if success// 取得Server的回傳值
                    {
                        print("UserIDLogin");
                        OpenLoadingPanel = true;

                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        getGameID = Convert.ToString(operationResponse.Parameters[1]);
                        getNickname = Convert.ToString(operationResponse.Parameters[3]);
                        getFBKey = Convert.ToString(operationResponse.Parameters[4]);
                        getP8Key = Convert.ToString(operationResponse.Parameters[5]);
                        print(getP8Key);
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[6]);
                        RealMoney = getUserMoney;
                        iLV = Convert.ToInt32(operationResponse.Parameters[9]);
                        iEXP = Convert.ToInt32(operationResponse.Parameters[10]);
                        iGetLoginRewardMoney = Convert.ToInt32(operationResponse.Parameters[15]);// 登入領了多少 登入獎勵 0 是今天領過了 或是活動未開啟
                        DailyMoney = Convert.ToInt32(operationResponse.Parameters[18]);    // 登入獎領是多少
                        iGetLoginReward = Convert.ToInt32(operationResponse.Parameters[20]);//每日登入獎勵是否開啟

                        iVIPLv = Convert.ToInt32(operationResponse.Parameters[22]); //VIP等級1 - 10  0 是 1喔
                        iVIPTotalExp = Convert.ToInt32(operationResponse.Parameters[23]);
                        iVIPExp = iVIPTotalExp - VIPNeedExp[iVIPLv]; //VIP總經驗值
                        LoginStatus = true;
                        if (iGetLoginRewardMoney != 0) { getUserMoney = getUserMoney - DailyMoney; Gift_Control.HaveGift = true; }
                    }
                }break;
            case (byte)Command.PWC_UserCreate://9
                {
                    if (operationResponse.ReturnCode == 1)  // if success// 取得Server的回傳值
                    {
                        print("UserCreate");                       
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        getGameID = Convert.ToString(operationResponse.Parameters[1]);
                        getNickname = Convert.ToString(operationResponse.Parameters[3]);
                        getFBKey = Convert.ToString(operationResponse.Parameters[4]);
                        getP8Key = Convert.ToString(operationResponse.Parameters[5]);
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[6]);
                        RealMoney = getUserMoney;
                        //if ((getFBKey == "") || (getFBKey == "0"))
                        //{
                        //if ((getP8Key == "") || (getP8Key == "0")) {
                        //PlayerPrefs.SetString("ID", getMemberID); PlayerPrefs.SetInt("haveID", 1);
                        //}}

                        iLV = Convert.ToInt32(operationResponse.Parameters[9]);
                        iEXP = Convert.ToInt32(operationResponse.Parameters[10]);
                        iGetLoginRewardMoney = Convert.ToInt32(operationResponse.Parameters[15]);// 登入領了多少 登入獎勵 0 是今天領過了 或是活動未開啟
                        DailyMoney = Convert.ToInt32(operationResponse.Parameters[18]);    // 登入獎領是多少
                        iGetLoginReward = Convert.ToInt32(operationResponse.Parameters[20]);//每日登入獎勵是否開啟

                        iVIPLv = Convert.ToInt32(operationResponse.Parameters[22]); //VIP等級1 - 10  0 是 1喔
                        iVIPTotalExp = Convert.ToInt32(operationResponse.Parameters[23]);
                        iVIPExp = iVIPTotalExp - VIPNeedExp[iVIPLv]; //VIP總經驗值
                        LoginStatus = true;
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_PlayerInfo://22
                {
                    if (operationResponse.ReturnCode == 1)  // if success// 取得Server的回傳值
                    {
                        print("PlayerInfo");                      
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        getGameID = Convert.ToString(operationResponse.Parameters[1]);
                        getNickname = Convert.ToString(operationResponse.Parameters[3]);
                        getFBKey = Convert.ToString(operationResponse.Parameters[4]);
                        getP8Key = Convert.ToString(operationResponse.Parameters[5]);
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[6]);
                        RealMoney = getUserMoney;
                        iLV = Convert.ToInt32(operationResponse.Parameters[9]);
                        iEXP = Convert.ToInt32(operationResponse.Parameters[10]);
                        iGetLoginRewardMoney = Convert.ToInt32(operationResponse.Parameters[15]);// 登入領了多少 登入獎勵 0 是今天領過了 或是活動未開啟
                        DailyMoney = Convert.ToInt32(operationResponse.Parameters[18]);    // 登入獎領是多少
                        iGetLoginReward = Convert.ToInt32(operationResponse.Parameters[20]);//每日登入獎勵是否開啟
                        iVIPLv = Convert.ToInt32(operationResponse.Parameters[22]); //VIP等級1 - 10  0 是 1喔
                        iVIPTotalExp = Convert.ToInt32(operationResponse.Parameters[23]);
                        iBankSave = Convert.ToInt32(operationResponse.Parameters[24]);
                        iVIPExp = iVIPTotalExp - VIPNeedExp[iVIPLv]; //VIP總經驗值
                        LoginStatus = true;
                        if (iGetLoginRewardMoney != 0) { getUserMoney = getUserMoney - DailyMoney; Gift_Control.HaveGift = true; }
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                    }
                }
                break;
            case (byte)Command.PWC_LevelUp://23
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        // 取得Server的回傳值
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);
                        iLV = Convert.ToInt32(operationResponse.Parameters[2]);
                        iEXP = Convert.ToInt32(operationResponse.Parameters[3]);
                        //iLevelUpBonus = Convert.ToInt32(operationResponse.Parameters[4]);
                        Lv_Control.LevelUp = true;
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_AWardsPost://26取得公告
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        // 取得Server的回傳值
                        szMSG = Convert.ToString(operationResponse.Parameters[0]);
                        szName = Convert.ToString(operationResponse.Parameters[1]);
                        szAwards = Convert.ToString(operationResponse.Parameters[2]);
                        iMoney = Convert.ToInt32(operationResponse.Parameters[3]);
                        iType = Convert.ToInt16(operationResponse.Parameters[4]);
                        Award_Control.ShowAward();
                    }
                }
                break;
            case (byte)Command.PWC_SetNickName://30
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        getNickname = Convert.ToString(operationResponse.Parameters[1]);
                        print("getNickname" + getNickname);
                        PlayerPrefs.SetString("ID", getMemberID); PlayerPrefs.SetInt("haveID", 1);
                        if (PlayerPrefs.GetInt("SceneNum") == 0)OpenLoadingPanel = true;
                    }else {NickName.MistakeNickName();print("MistakeNickName"); }
                }
                break;
            case (byte)Command.PWC_GoolgePurchases://36
                {
                    getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                    getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);
                    int getMoney = Convert.ToInt32(operationResponse.Parameters[2]);
                    iVIPLv = Convert.ToInt32(operationResponse.Parameters[3]);
                    if (iVIPLv > PlayerPrefs.GetInt("VIPLv")) VIP_Control.VIPLevelUp = true;
                    PlayerPrefs.SetInt("VIPLv", iVIPLv);
                    iVIPTotalExp = Convert.ToInt32(operationResponse.Parameters[4]);
                    iVIPExp = iVIPTotalExp - VIPNeedExp[iVIPLv];
                }
                break;
            #endregion
            #region//遊戲
            case (byte)Command.PWC_CookGirl_Info://38
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("CookGirl");                        
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[1]);// cookGirlJPMoney
                        for (int i = 0; i < 25; i++) { iLine25[i] = 0; }
                    }
                    else { Debug.Log("get GSC failed"); }
                }
                break;
            case (byte)Command.PWC_CookGirl_Result://40
                {
                    if (operationResponse.ReturnCode == 1)  // if success// 取得Server的回傳值    
                    {
                        print("CookGirl_Result");                                    
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);// 玩家ID
                        SlotTurnSys.GetSlotInfo();

                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);// 玩家金錢
                        iBet = Convert.ToInt32(operationResponse.Parameters[2]);
                        iAllFG = Convert.ToInt32(operationResponse.Parameters[3]);// 全盤獎(0.沒有 1.JP 2Free 3上菜)
                        iLVNow = Convert.ToInt32(operationResponse.Parameters[4]);
                        iLVNum = Convert.ToInt32(operationResponse.Parameters[5]);// 關咖數量 取得時事總數量 關卡玩一盤 就會減一                      
                        Line_Money = Convert.ToInt32(operationResponse.Parameters[6]);// 一班轉盤的錢                       
                        JP_Money = Convert.ToInt32(operationResponse.Parameters[7]);// 取得JP1的錢

                        if (iLVNow == 3) PrizeSys.SpecialLevel(iLVNow, iLVNum);
                        if (iAllFG > 0 && iAllFG < 3) PrizeSys.SpecialLevel(iAllFG, iLVNum);
                        if (Line_Money != 0) PrizeSys.PrizeAnimation(Line_Money);

                        #region // 15輪的花色

                        islot3x5[0] = Convert.ToInt32(operationResponse.Parameters[8]);
                        islot3x5[1] = Convert.ToInt32(operationResponse.Parameters[9]);
                        islot3x5[2] = Convert.ToInt32(operationResponse.Parameters[10]);
                        islot3x5[3] = Convert.ToInt32(operationResponse.Parameters[11]);
                        islot3x5[4] = Convert.ToInt32(operationResponse.Parameters[12]);
                        islot3x5[5] = Convert.ToInt32(operationResponse.Parameters[13]);
                        islot3x5[6] = Convert.ToInt32(operationResponse.Parameters[14]);
                        islot3x5[7] = Convert.ToInt32(operationResponse.Parameters[15]);
                        islot3x5[8] = Convert.ToInt32(operationResponse.Parameters[16]);
                        islot3x5[9] = Convert.ToInt32(operationResponse.Parameters[17]);
                        islot3x5[10] = Convert.ToInt32(operationResponse.Parameters[18]);
                        islot3x5[11] = Convert.ToInt32(operationResponse.Parameters[19]);
                        islot3x5[12] = Convert.ToInt32(operationResponse.Parameters[20]);
                        islot3x5[13] = Convert.ToInt32(operationResponse.Parameters[21]);
                        islot3x5[14] = Convert.ToInt32(operationResponse.Parameters[22]);

                        if (islot3x5[2] == 10) { islot3x5[7] = 11; islot3x5[12] = 12; }
                        if (islot3x5[4] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[14] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[4] = 11; islot3x5[9] = 12; }
                            }
                            else islot3x5[4] = 12;
                        }
                        if (islot3x5[14] >= 10)
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[4] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[9] = 10; islot3x5[14] = 11; }
                            }
                            else islot3x5[14] = 10;
                        }

                        // 25條
                        iLine25[0] = Convert.ToInt32(operationResponse.Parameters[23]);
                        iLine25[1] = Convert.ToInt32(operationResponse.Parameters[24]);
                        iLine25[2] = Convert.ToInt32(operationResponse.Parameters[25]);
                        iLine25[3] = Convert.ToInt32(operationResponse.Parameters[26]);
                        iLine25[4] = Convert.ToInt32(operationResponse.Parameters[27]);
                        iLine25[5] = Convert.ToInt32(operationResponse.Parameters[28]);
                        iLine25[6] = Convert.ToInt32(operationResponse.Parameters[29]);
                        iLine25[7] = Convert.ToInt32(operationResponse.Parameters[30]);
                        iLine25[8] = Convert.ToInt32(operationResponse.Parameters[31]);
                        iLine25[9] = Convert.ToInt32(operationResponse.Parameters[32]);
                        iLine25[10] = Convert.ToInt32(operationResponse.Parameters[33]);
                        iLine25[11] = Convert.ToInt32(operationResponse.Parameters[34]);
                        iLine25[12] = Convert.ToInt32(operationResponse.Parameters[35]);
                        iLine25[13] = Convert.ToInt32(operationResponse.Parameters[36]);
                        iLine25[14] = Convert.ToInt32(operationResponse.Parameters[37]);
                        iLine25[15] = Convert.ToInt32(operationResponse.Parameters[38]);
                        iLine25[16] = Convert.ToInt32(operationResponse.Parameters[39]);
                        iLine25[17] = Convert.ToInt32(operationResponse.Parameters[40]);
                        iLine25[18] = Convert.ToInt32(operationResponse.Parameters[41]);
                        iLine25[19] = Convert.ToInt32(operationResponse.Parameters[42]);
                        iLine25[20] = Convert.ToInt32(operationResponse.Parameters[43]);
                        iLine25[21] = Convert.ToInt32(operationResponse.Parameters[44]);
                        iLine25[22] = Convert.ToInt32(operationResponse.Parameters[45]);
                        iLine25[23] = Convert.ToInt32(operationResponse.Parameters[46]);
                        iLine25[24] = Convert.ToInt32(operationResponse.Parameters[47]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[48]);//iJP                        
                        #endregion
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_MagicGirl_Info://42
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("MagicGirl");
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[1]);
                    }
                }
                break;
            case (byte)Command.PWC_MagicGirl_Result://44
                {
                    if (operationResponse.ReturnCode == 1)  // if success// 取得Server的回傳值 
                    {
                        print("MagicGirl_Result");                                           
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);// 玩家ID
                        SlotTurnSys.GetSlotInfo();

                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);// 玩家金錢
                        iBet = Convert.ToInt32(operationResponse.Parameters[2]);
                        iAllFG = Convert.ToInt32(operationResponse.Parameters[3]);// 全盤獎(0.沒有 1.JP 2Free 3上菜)
                        iLVNow = Convert.ToInt32(operationResponse.Parameters[4]);
                        iLVNum = Convert.ToInt32(operationResponse.Parameters[5]);// 關咖數量 取得時事總數量 關卡玩一盤 就會減一                     
                        Line_Money = Convert.ToInt32(operationResponse.Parameters[6]);// 一班轉盤的錢                 
                        JP_Money = Convert.ToInt32(operationResponse.Parameters[7]);// 取得JP1的錢

                        if (iLVNow == 3) PrizeSys.SpecialLevel(iLVNow, iLVNum);
                        if (iAllFG > 0 && iAllFG < 3) PrizeSys.SpecialLevel(iAllFG, iLVNum);
                        if (Line_Money != 0) PrizeSys.PrizeAnimation(Line_Money);

                        #region // 15輪的花色
                        islot3x5[0] = Convert.ToInt32(operationResponse.Parameters[8]);
                        islot3x5[1] = Convert.ToInt32(operationResponse.Parameters[9]);
                        islot3x5[2] = Convert.ToInt32(operationResponse.Parameters[10]);
                        islot3x5[3] = Convert.ToInt32(operationResponse.Parameters[11]);
                        islot3x5[4] = Convert.ToInt32(operationResponse.Parameters[12]);
                        islot3x5[5] = Convert.ToInt32(operationResponse.Parameters[13]);
                        islot3x5[6] = Convert.ToInt32(operationResponse.Parameters[14]);
                        islot3x5[7] = Convert.ToInt32(operationResponse.Parameters[15]);
                        islot3x5[8] = Convert.ToInt32(operationResponse.Parameters[16]);
                        islot3x5[9] = Convert.ToInt32(operationResponse.Parameters[17]);
                        islot3x5[10] = Convert.ToInt32(operationResponse.Parameters[18]);
                        islot3x5[11] = Convert.ToInt32(operationResponse.Parameters[19]);
                        islot3x5[12] = Convert.ToInt32(operationResponse.Parameters[20]);
                        islot3x5[13] = Convert.ToInt32(operationResponse.Parameters[21]);
                        islot3x5[14] = Convert.ToInt32(operationResponse.Parameters[22]);

                        if (islot3x5[2] == 10) { islot3x5[7] = 11; islot3x5[12] = 12; }
                        if (islot3x5[4] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[14] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[4] = 11; islot3x5[9] = 12; }
                            }
                            else islot3x5[4] = 12;
                        }
                        if (islot3x5[14] >= 10)
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[4] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[9] = 10; islot3x5[14] = 11; }
                            }
                            else islot3x5[14] = 10;
                        }

                        // 25條
                        iLine25[0] = Convert.ToInt32(operationResponse.Parameters[23]);
                        iLine25[1] = Convert.ToInt32(operationResponse.Parameters[24]);
                        iLine25[2] = Convert.ToInt32(operationResponse.Parameters[25]);
                        iLine25[3] = Convert.ToInt32(operationResponse.Parameters[26]);
                        iLine25[4] = Convert.ToInt32(operationResponse.Parameters[27]);
                        iLine25[5] = Convert.ToInt32(operationResponse.Parameters[28]);
                        iLine25[6] = Convert.ToInt32(operationResponse.Parameters[29]);
                        iLine25[7] = Convert.ToInt32(operationResponse.Parameters[30]);
                        iLine25[8] = Convert.ToInt32(operationResponse.Parameters[31]);
                        iLine25[9] = Convert.ToInt32(operationResponse.Parameters[32]);
                        iLine25[10] = Convert.ToInt32(operationResponse.Parameters[33]);

                        iLine25[11] = Convert.ToInt32(operationResponse.Parameters[34]);
                        iLine25[12] = Convert.ToInt32(operationResponse.Parameters[35]);
                        iLine25[13] = Convert.ToInt32(operationResponse.Parameters[36]);
                        iLine25[14] = Convert.ToInt32(operationResponse.Parameters[37]);
                        iLine25[15] = Convert.ToInt32(operationResponse.Parameters[38]);
                        iLine25[16] = Convert.ToInt32(operationResponse.Parameters[39]);
                        iLine25[17] = Convert.ToInt32(operationResponse.Parameters[40]);
                        iLine25[18] = Convert.ToInt32(operationResponse.Parameters[41]);
                        iLine25[19] = Convert.ToInt32(operationResponse.Parameters[42]);
                        iLine25[20] = Convert.ToInt32(operationResponse.Parameters[43]);
                        iLine25[21] = Convert.ToInt32(operationResponse.Parameters[44]);
                        iLine25[22] = Convert.ToInt32(operationResponse.Parameters[45]);
                        iLine25[23] = Convert.ToInt32(operationResponse.Parameters[46]);
                        iLine25[24] = Convert.ToInt32(operationResponse.Parameters[47]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[48]);
                        #endregion
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_ComeCat_Info://46
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("LuckyCat");//目前CLIENT 不用收這些數值 這些數值 是還要多少次才開獎                      
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[1]);// cookGirlJPMoney
                        for (int i = 0; i < 25; i++) { iLine25[i] = 0; }
                    }
                    else { Debug.Log("get GSC failed"); }
                }
                break;
            case (byte)Command.PWC_ComeCat_Result://48
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("LuckyCat_Result");// 取得Server的回傳值                                               
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);// 玩家ID
                        SlotTurnSys.GetSlotInfo();

                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);// 玩家金錢
                        iBet = Convert.ToInt32(operationResponse.Parameters[2]);
                        iAllFG = Convert.ToInt32(operationResponse.Parameters[3]);// 全盤獎(0.沒有 1.JP 2Free 3上菜)
                        iLVNow = Convert.ToInt32(operationResponse.Parameters[4]);
                        iLVNum = Convert.ToInt32(operationResponse.Parameters[5]);// 關咖數量 取得時事總數量 關卡玩一盤 就會減一                      
                        Line_Money = Convert.ToInt32(operationResponse.Parameters[6]);// 一班轉盤的錢                       
                        JP_Money = Convert.ToInt32(operationResponse.Parameters[7]);// 取得JP1的錢             
                        TimeNum = Convert.ToInt32(operationResponse.Parameters[49]);// 乘倍

                        if (iLestLV >= 0) { iLestLV = iLestLV - 1; }
                        if (iLVNow == 3) PrizeSys.SpecialLevel(iLVNow, iLVNum);
                        if (iLVNow == 2) { if (iLVNum == 1) { iLestLV = 1; } }
                        if (iAllFG > 0 && iAllFG < 3) PrizeSys.SpecialLevel(iAllFG, iLVNum);
                        if (Line_Money != 0) PrizeSys.PrizeAnimation(Line_Money);

                        #region // 15輪的花色

                        islot3x5[0] = Convert.ToInt32(operationResponse.Parameters[8]);
                        islot3x5[1] = Convert.ToInt32(operationResponse.Parameters[9]);
                        islot3x5[2] = Convert.ToInt32(operationResponse.Parameters[10]);
                        islot3x5[3] = Convert.ToInt32(operationResponse.Parameters[11]);
                        islot3x5[4] = Convert.ToInt32(operationResponse.Parameters[12]);
                        islot3x5[5] = Convert.ToInt32(operationResponse.Parameters[13]);
                        islot3x5[6] = Convert.ToInt32(operationResponse.Parameters[14]);
                        islot3x5[7] = Convert.ToInt32(operationResponse.Parameters[15]);
                        islot3x5[8] = Convert.ToInt32(operationResponse.Parameters[16]);
                        islot3x5[9] = Convert.ToInt32(operationResponse.Parameters[17]);
                        islot3x5[10] = Convert.ToInt32(operationResponse.Parameters[18]);
                        islot3x5[11] = Convert.ToInt32(operationResponse.Parameters[19]);
                        islot3x5[12] = Convert.ToInt32(operationResponse.Parameters[20]);
                        islot3x5[13] = Convert.ToInt32(operationResponse.Parameters[21]);
                        islot3x5[14] = Convert.ToInt32(operationResponse.Parameters[22]);

                        if (islot3x5[3] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[8] >= 10)
                            {
                                if (islot3x5[13] >= 10) { islot3x5[3] = 10; islot3x5[8] = 11; islot3x5[13] = 12; }
                                else { islot3x5[3] = 11; islot3x5[8] = 12; }
                            }
                            else islot3x5[3] = 12;
                        }
                        if (islot3x5[13] >= 10)
                        {
                            if (islot3x5[8] >= 10)
                            {
                                if (islot3x5[3] >= 10) { islot3x5[3] = 10; islot3x5[8] = 11; islot3x5[13] = 12; }
                                else { islot3x5[8] = 10; islot3x5[13] = 11; }
                            }
                            else islot3x5[13] = 10;
                        }


                        if (islot3x5[4] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[14] >= 10) { islot3x5[4] = 13; islot3x5[9] = 14; islot3x5[14] = 15; }
                                else { islot3x5[4] = 14; islot3x5[9] = 15; }
                            }
                            else islot3x5[4] = 15;
                        }
                        if (islot3x5[14] >= 10)
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[4] >= 10) { islot3x5[4] = 13; islot3x5[9] = 14; islot3x5[14] = 15; }
                                else { islot3x5[9] = 13; islot3x5[14] = 14; }
                            }
                            else islot3x5[14] = 13;
                        }


                        // 25條
                        iLine25[0] = Convert.ToInt32(operationResponse.Parameters[23]);
                        iLine25[1] = Convert.ToInt32(operationResponse.Parameters[24]);
                        iLine25[2] = Convert.ToInt32(operationResponse.Parameters[25]);
                        iLine25[3] = Convert.ToInt32(operationResponse.Parameters[26]);
                        iLine25[4] = Convert.ToInt32(operationResponse.Parameters[27]);
                        iLine25[5] = Convert.ToInt32(operationResponse.Parameters[28]);
                        iLine25[6] = Convert.ToInt32(operationResponse.Parameters[29]);
                        iLine25[7] = Convert.ToInt32(operationResponse.Parameters[30]);
                        iLine25[8] = Convert.ToInt32(operationResponse.Parameters[31]);
                        iLine25[9] = Convert.ToInt32(operationResponse.Parameters[32]);
                        iLine25[10] = Convert.ToInt32(operationResponse.Parameters[33]);
                        iLine25[11] = Convert.ToInt32(operationResponse.Parameters[34]);
                        iLine25[12] = Convert.ToInt32(operationResponse.Parameters[35]);
                        iLine25[13] = Convert.ToInt32(operationResponse.Parameters[36]);
                        iLine25[14] = Convert.ToInt32(operationResponse.Parameters[37]);
                        iLine25[15] = Convert.ToInt32(operationResponse.Parameters[38]);
                        iLine25[16] = Convert.ToInt32(operationResponse.Parameters[39]);
                        iLine25[17] = Convert.ToInt32(operationResponse.Parameters[40]);
                        iLine25[18] = Convert.ToInt32(operationResponse.Parameters[41]);
                        iLine25[19] = Convert.ToInt32(operationResponse.Parameters[42]);
                        iLine25[20] = Convert.ToInt32(operationResponse.Parameters[43]);
                        iLine25[21] = Convert.ToInt32(operationResponse.Parameters[44]);
                        iLine25[22] = Convert.ToInt32(operationResponse.Parameters[45]);
                        iLine25[23] = Convert.ToInt32(operationResponse.Parameters[46]);
                        iLine25[24] = Convert.ToInt32(operationResponse.Parameters[47]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[48]);//iJP       


                        #endregion
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_EgyptKing_Info://50
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("EgyptGirl");//目前CLIENT 不用收這些數值 這些數值 是還要多少次才開獎                      
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[1]);// cookGirlJPMoney
                        for (int i = 0; i < 25; i++) { iLine25[i] = 0; }
                    }
                    else { Debug.Log("get GSC failed"); }
                }
                break;
            case (byte)Command.PWC_EgyptKing_Result://52
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("EgyptGirl_Result");// 取得Server的回傳值                                               
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);// 玩家ID
                        SlotTurnSys.GetSlotInfo();

                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);// 玩家金錢
                        iBet = Convert.ToInt32(operationResponse.Parameters[2]);
                        iAllFG = Convert.ToInt32(operationResponse.Parameters[3]);// 全盤獎(0.沒有 1.JP 2Free 3上菜)
                        iLVNow = Convert.ToInt32(operationResponse.Parameters[4]);
                        iLVNum = Convert.ToInt32(operationResponse.Parameters[5]);// 關咖數量 取得時事總數量 關卡玩一盤 就會減一                      
                        Line_Money = Convert.ToInt32(operationResponse.Parameters[6]);// 一班轉盤的錢                       
                        JP_Money = Convert.ToInt32(operationResponse.Parameters[7]);// 取得JP1的錢             
                        TimeNum = Convert.ToInt32(operationResponse.Parameters[49]);// 乘倍

                        if (iLestLV >= 0) { iLestLV = iLestLV - 1; }
                        if (iLVNow == 3) PrizeSys.SpecialLevel(iLVNow, iLVNum);
                        if (iLVNow == 2) { if (iLVNum == 1) { iLestLV = 1; } }
                        if (iAllFG > 0 && iAllFG < 3) PrizeSys.SpecialLevel(iAllFG, iLVNum);
                        if (Line_Money != 0) PrizeSys.PrizeAnimation(Line_Money);

                        #region // 15輪的花色

                        islot3x5[0] = Convert.ToInt32(operationResponse.Parameters[8]);
                        islot3x5[1] = Convert.ToInt32(operationResponse.Parameters[9]);
                        islot3x5[2] = Convert.ToInt32(operationResponse.Parameters[10]);
                        islot3x5[3] = Convert.ToInt32(operationResponse.Parameters[11]);
                        islot3x5[4] = Convert.ToInt32(operationResponse.Parameters[12]);
                        islot3x5[5] = Convert.ToInt32(operationResponse.Parameters[13]);
                        islot3x5[6] = Convert.ToInt32(operationResponse.Parameters[14]);
                        islot3x5[7] = Convert.ToInt32(operationResponse.Parameters[15]);
                        islot3x5[8] = Convert.ToInt32(operationResponse.Parameters[16]);
                        islot3x5[9] = Convert.ToInt32(operationResponse.Parameters[17]);
                        islot3x5[10] = Convert.ToInt32(operationResponse.Parameters[18]);
                        islot3x5[11] = Convert.ToInt32(operationResponse.Parameters[19]);
                        islot3x5[12] = Convert.ToInt32(operationResponse.Parameters[20]);
                        islot3x5[13] = Convert.ToInt32(operationResponse.Parameters[21]);
                        islot3x5[14] = Convert.ToInt32(operationResponse.Parameters[22]);

                        if (islot3x5[3] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[8] >= 10)
                            {
                                if (islot3x5[13] >= 10) { islot3x5[3] = 10; islot3x5[8] = 11; islot3x5[13] = 12; }
                                else { islot3x5[3] = 11; islot3x5[8] = 12; }
                            }
                            else islot3x5[3] = 12;
                        }
                        if (islot3x5[13] >= 10)
                        {
                            if (islot3x5[8] >= 10)
                            {
                                if (islot3x5[3] >= 10) { islot3x5[3] = 10; islot3x5[8] = 11; islot3x5[13] = 12; }
                                else { islot3x5[8] = 10; islot3x5[13] = 11; }
                            }
                            else islot3x5[13] = 10;
                        }


                        if (islot3x5[4] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[14] >= 10) { islot3x5[4] = 13; islot3x5[9] = 14; islot3x5[14] = 15; }
                                else { islot3x5[4] = 14; islot3x5[9] = 15; }
                            }
                            else islot3x5[4] = 15;
                        }
                        if (islot3x5[14] >= 10)
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[4] >= 10) { islot3x5[4] = 13; islot3x5[9] = 14; islot3x5[14] = 15; }
                                else { islot3x5[9] = 13; islot3x5[14] = 14; }
                            }
                            else islot3x5[14] = 13;
                        }


                        // 25條
                        iLine25[0] = Convert.ToInt32(operationResponse.Parameters[23]);
                        iLine25[1] = Convert.ToInt32(operationResponse.Parameters[24]);
                        iLine25[2] = Convert.ToInt32(operationResponse.Parameters[25]);
                        iLine25[3] = Convert.ToInt32(operationResponse.Parameters[26]);
                        iLine25[4] = Convert.ToInt32(operationResponse.Parameters[27]);
                        iLine25[5] = Convert.ToInt32(operationResponse.Parameters[28]);
                        iLine25[6] = Convert.ToInt32(operationResponse.Parameters[29]);
                        iLine25[7] = Convert.ToInt32(operationResponse.Parameters[30]);
                        iLine25[8] = Convert.ToInt32(operationResponse.Parameters[31]);
                        iLine25[9] = Convert.ToInt32(operationResponse.Parameters[32]);
                        iLine25[10] = Convert.ToInt32(operationResponse.Parameters[33]);
                        iLine25[11] = Convert.ToInt32(operationResponse.Parameters[34]);
                        iLine25[12] = Convert.ToInt32(operationResponse.Parameters[35]);
                        iLine25[13] = Convert.ToInt32(operationResponse.Parameters[36]);
                        iLine25[14] = Convert.ToInt32(operationResponse.Parameters[37]);
                        iLine25[15] = Convert.ToInt32(operationResponse.Parameters[38]);
                        iLine25[16] = Convert.ToInt32(operationResponse.Parameters[39]);
                        iLine25[17] = Convert.ToInt32(operationResponse.Parameters[40]);
                        iLine25[18] = Convert.ToInt32(operationResponse.Parameters[41]);
                        iLine25[19] = Convert.ToInt32(operationResponse.Parameters[42]);
                        iLine25[20] = Convert.ToInt32(operationResponse.Parameters[43]);
                        iLine25[21] = Convert.ToInt32(operationResponse.Parameters[44]);
                        iLine25[22] = Convert.ToInt32(operationResponse.Parameters[45]);
                        iLine25[23] = Convert.ToInt32(operationResponse.Parameters[46]);
                        iLine25[24] = Convert.ToInt32(operationResponse.Parameters[47]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[48]);//iJP       


                        #endregion
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_AlsDream_Info://54
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("AlsDream");//目前CLIENT 不用收這些數值 這些數值 是還要多少次才開獎
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[1]);
                        for (int i = 0; i < 25; i++) { iLine25[i] = 0; }
                    }
                }
                break;
            case (byte)Command.PWC_AlsDream_Result://56
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {                   
                        print("AlsDream_Result");// 取得Server的回傳值                  
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);// 玩家ID
                        SlotTurnSys.GetSlotInfo();
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);// 玩家金錢
                        iBet = Convert.ToInt32(operationResponse.Parameters[2]);                      
                        iAllFG = Convert.ToInt32(operationResponse.Parameters[3]);// 全盤獎(0.沒有 1.JP 2Free 3上菜)
                        iLVNow = Convert.ToInt32(operationResponse.Parameters[4]);                    
                        iLVNum = Convert.ToInt32(operationResponse.Parameters[5]); // 關咖數量 取得時事總數量 關卡玩一盤 就會減一                     
                        Line_Money = Convert.ToInt32(operationResponse.Parameters[6]);// 一班轉盤的錢                        
                        JP_Money = Convert.ToInt32(operationResponse.Parameters[7]);// 取得JP1的錢

                        if (iLVNow == 3) PrizeSys.SpecialLevel(iLVNow, iLVNum);
                        if (iAllFG > 0 && iAllFG < 3) PrizeSys.SpecialLevel(iAllFG, iLVNum);
                        if (Line_Money != 0) PrizeSys.PrizeAnimation(Line_Money);

                        // 15輪的花色
                        islot3x5[0] = Convert.ToInt32(operationResponse.Parameters[8]);
                        islot3x5[1] = Convert.ToInt32(operationResponse.Parameters[9]);
                        islot3x5[2] = Convert.ToInt32(operationResponse.Parameters[10]);
                        islot3x5[3] = Convert.ToInt32(operationResponse.Parameters[11]);
                        islot3x5[4] = Convert.ToInt32(operationResponse.Parameters[12]);
                        islot3x5[5] = Convert.ToInt32(operationResponse.Parameters[13]);
                        islot3x5[6] = Convert.ToInt32(operationResponse.Parameters[14]);
                        islot3x5[7] = Convert.ToInt32(operationResponse.Parameters[15]);
                        islot3x5[8] = Convert.ToInt32(operationResponse.Parameters[16]);
                        islot3x5[9] = Convert.ToInt32(operationResponse.Parameters[17]);
                        islot3x5[10] = Convert.ToInt32(operationResponse.Parameters[18]);
                        islot3x5[11] = Convert.ToInt32(operationResponse.Parameters[19]);
                        islot3x5[12] = Convert.ToInt32(operationResponse.Parameters[20]);
                        islot3x5[13] = Convert.ToInt32(operationResponse.Parameters[21]);
                        islot3x5[14] = Convert.ToInt32(operationResponse.Parameters[22]);
                        // 15條
                        iLine25[0] = Convert.ToInt32(operationResponse.Parameters[23]);
                        iLine25[1] = Convert.ToInt32(operationResponse.Parameters[24]);
                        iLine25[2] = Convert.ToInt32(operationResponse.Parameters[25]);
                        iLine25[3] = Convert.ToInt32(operationResponse.Parameters[26]);
                        iLine25[4] = Convert.ToInt32(operationResponse.Parameters[27]);
                        iLine25[23] = Convert.ToInt32(operationResponse.Parameters[28]);
                        iLine25[24] = Convert.ToInt32(operationResponse.Parameters[29]);
                        iLine25[22] = Convert.ToInt32(operationResponse.Parameters[30]);
                        iLine25[21] = Convert.ToInt32(operationResponse.Parameters[31]);
                        iLine25[13] = Convert.ToInt32(operationResponse.Parameters[32]);
                        iLine25[14] = Convert.ToInt32(operationResponse.Parameters[33]);

                        iLine25[19] = Convert.ToInt32(operationResponse.Parameters[34]);
                        iLine25[20] = Convert.ToInt32(operationResponse.Parameters[35]);
                        iLine25[11] = Convert.ToInt32(operationResponse.Parameters[36]);
                        iLine25[12] = Convert.ToInt32(operationResponse.Parameters[37]);

                        BonusNum = 3-Convert.ToInt32(operationResponse.Parameters[38]);  // 1-4種茶壺
                        TimeNum = Convert.ToInt32(operationResponse.Parameters[39]);  // 實際取得倍數	
                        Bonus_Money = Convert.ToInt32(operationResponse.Parameters[40]);//取得金額
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[41]);

                        if (islot3x5[4] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[14] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[4] = 11; islot3x5[9] = 12; }
                            }
                            else islot3x5[4] = 12;
                        }
                        if (islot3x5[14] >= 10)
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[4] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[9] = 10; islot3x5[14] = 11; }
                            }
                            else islot3x5[14] = 10;
                        }
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            case (byte)Command.PWC_GreenDay_Info://58
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("GreenDay");//目前CLIENT 不用收這些數值 這些數值 是還要多少次才開獎
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[1]);
                        for (int i = 0; i < 25; i++) { iLine25[i] = 0; }
                    }
                }
                break;
            case (byte)Command.PWC_GreenDay_Result://60
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        print("GreenDay_Result");// 取得Server的回傳值                  
                        getMemberID = Convert.ToString(operationResponse.Parameters[0]);// 玩家ID
                        SlotTurnSys.GetSlotInfo();
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);// 玩家金錢
                        iBet = Convert.ToInt32(operationResponse.Parameters[2]);
                        iAllFG = Convert.ToInt32(operationResponse.Parameters[3]);// 全盤獎(0.沒有 1.JP 2Free 3上菜)
                        iLVNow = Convert.ToInt32(operationResponse.Parameters[4]);
                        iLVNum = Convert.ToInt32(operationResponse.Parameters[5]); // 關咖數量 取得時事總數量 關卡玩一盤 就會減一                     
                        Line_Money = Convert.ToInt32(operationResponse.Parameters[6]);// 一班轉盤的錢                        
                        JP_Money = Convert.ToInt32(operationResponse.Parameters[7]);// 取得JP1的錢

                        if (iLVNow == 3) PrizeSys.SpecialLevel(iLVNow, iLVNum);
                        if (iAllFG > 0 && iAllFG < 3) PrizeSys.SpecialLevel(iAllFG, iLVNum);
                        if (Line_Money != 0) PrizeSys.PrizeAnimation(Line_Money);

                        // 15輪的花色
                        islot3x5[0] = Convert.ToInt32(operationResponse.Parameters[8]);
                        islot3x5[1] = Convert.ToInt32(operationResponse.Parameters[9]);
                        islot3x5[2] = Convert.ToInt32(operationResponse.Parameters[10]);
                        islot3x5[3] = Convert.ToInt32(operationResponse.Parameters[11]);
                        islot3x5[4] = Convert.ToInt32(operationResponse.Parameters[12]);
                        islot3x5[5] = Convert.ToInt32(operationResponse.Parameters[13]);
                        islot3x5[6] = Convert.ToInt32(operationResponse.Parameters[14]);
                        islot3x5[7] = Convert.ToInt32(operationResponse.Parameters[15]);
                        islot3x5[8] = Convert.ToInt32(operationResponse.Parameters[16]);
                        islot3x5[9] = Convert.ToInt32(operationResponse.Parameters[17]);
                        islot3x5[10] = Convert.ToInt32(operationResponse.Parameters[18]);
                        islot3x5[11] = Convert.ToInt32(operationResponse.Parameters[19]);
                        islot3x5[12] = Convert.ToInt32(operationResponse.Parameters[20]);
                        islot3x5[13] = Convert.ToInt32(operationResponse.Parameters[21]);
                        islot3x5[14] = Convert.ToInt32(operationResponse.Parameters[22]);
                        // 15條
                        iLine25[0] = Convert.ToInt32(operationResponse.Parameters[23]);
                        iLine25[1] = Convert.ToInt32(operationResponse.Parameters[24]);
                        iLine25[2] = Convert.ToInt32(operationResponse.Parameters[25]);
                        iLine25[3] = Convert.ToInt32(operationResponse.Parameters[26]);
                        iLine25[4] = Convert.ToInt32(operationResponse.Parameters[27]);
                        iLine25[23] = Convert.ToInt32(operationResponse.Parameters[28]);
                        iLine25[24] = Convert.ToInt32(operationResponse.Parameters[29]);
                        iLine25[22] = Convert.ToInt32(operationResponse.Parameters[30]);
                        iLine25[21] = Convert.ToInt32(operationResponse.Parameters[31]);
                        iLine25[13] = Convert.ToInt32(operationResponse.Parameters[32]);
                        iLine25[14] = Convert.ToInt32(operationResponse.Parameters[33]);

                        iLine25[19] = Convert.ToInt32(operationResponse.Parameters[34]);
                        iLine25[20] = Convert.ToInt32(operationResponse.Parameters[35]);
                        iLine25[11] = Convert.ToInt32(operationResponse.Parameters[36]);
                        iLine25[12] = Convert.ToInt32(operationResponse.Parameters[37]);

                        BonusNum = 3 - Convert.ToInt32(operationResponse.Parameters[38]);  // 1-4種茶壺
                        TimeNum = Convert.ToInt32(operationResponse.Parameters[39]);  // 實際取得倍數	
                        Bonus_Money = Convert.ToInt32(operationResponse.Parameters[40]);//取得金額
                        JP_Pool = Convert.ToInt32(operationResponse.Parameters[41]);

                        if (islot3x5[4] >= 10)//Wild的頭身腳
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[14] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[4] = 11; islot3x5[9] = 12; }
                            }
                            else islot3x5[4] = 12;
                        }
                        if (islot3x5[14] >= 10)
                        {
                            if (islot3x5[9] >= 10)
                            {
                                if (islot3x5[4] >= 10) { islot3x5[4] = 10; islot3x5[9] = 11; islot3x5[14] = 12; }
                                else { islot3x5[9] = 10; islot3x5[14] = 11; }
                            }
                            else islot3x5[14] = 10;
                        }
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                }
                break;
            #endregion


            case (byte)Command.CWP_Mail_Send://62
                {
                    SendSuccessCode = 62;
                    print(operationResponse.ReturnCode);
                    if (operationResponse.ReturnCode != 1)
                    {
                        SendSuccess = false;
                    }
                }
                break;

            case (byte)Command.PWC_Mail_Info://64
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        Mail_Num = Convert.ToInt32(operationResponse.Parameters[0]);//資料筆數 一封信12筆
                        if (Mail_Num != 0)
                        {
                            Mail_ID = Convert.ToString(operationResponse.Parameters[(byte)(Mail_Num/12)]);
                            string Send_ID = Convert.ToString(operationResponse.Parameters[(byte)(Mail_Num/12+1)]);
                            string Get_ID = Convert.ToString(operationResponse.Parameters[(byte)(Mail_Num/12+2)]);
                            Mail_Name = Convert.ToString(operationResponse.Parameters[(byte)(Mail_Num/12+3)]);
                            string Get_Name=Convert.ToString(operationResponse.Parameters[(byte)(Mail_Num/12+4)]);
                            Mail_Title = Convert.ToString(operationResponse.Parameters[(byte)(Mail_Num/12+5)]);
                            Mail_Info = Convert.ToString(operationResponse.Parameters[(byte)(Mail_Num/12+6)]);
                            Mail_Money = Convert.ToInt32(operationResponse.Parameters[(byte)(Mail_Num/12+7)]);
                            int Event_Num = Convert.ToInt32(operationResponse.Parameters[(byte)(Mail_Num/12+8)]);
                            int ifGet = Convert.ToInt32(operationResponse.Parameters[(byte)(Mail_Num/12+9)]);
                            DateTime datatime = Convert.ToDateTime(operationResponse.Parameters[(byte)(Mail_Num/12+10)]);
                        }
                    }
                }
                break;
            case (byte)Command.PWC_Mail_Get://65
                {
                    {
                        if (operationResponse.ReturnCode == 1)  // if success
                        {
                            Mail_Money = Convert.ToInt32(operationResponse.Parameters[0]); // 取得多少
                            getUserMoney = Convert.ToInt32(operationResponse.Parameters[1]);// 現在玩家身上多少
                        }
                    }
                }
                break;
            case (byte)Command.PWC_Mail_Send://66
                {
                    if (operationResponse.ReturnCode == 1)  // if success
                    {
                        SendSuccessCode = 66;
                        getUserMoney = Convert.ToInt32(operationResponse.Parameters[0]);// 現在玩家身上多少
                        SendSuccess = true;
                    }
                }
                break;
        }
    }
    public void OnStatusChanged(StatusCode statusCode)// (注九)  建立連線後 狀況改變的涵式 基本上也算繼曾 IPhotonPeerListener 虛擬涵式
    {
        // 連線狀態更改的回傳
        this.DebugReturn(0, string.Format("PeerStatusCallback: {0}", statusCode));
        switch (statusCode)
        {
            case StatusCode.Connect:
                this.ServerConnected = true;
                DisConnect_Panel.SetActive(false);
                LoginResult = "連線中";
                break;
            case StatusCode.Disconnect:
                this.LoginStatus = false;
                this.ServerConnected = false;
                DisConnect_Panel_Clone = (GameObject)Instantiate(DisConnect_Panel, GameObject.FindGameObjectWithTag("Canvas").transform);
                DisConnect_Panel_Clone.SetActive(true);
                LoginResult = "中斷連線";
                break;
            case StatusCode.DisconnectByServer:
                this.LoginStatus = false;
                this.ServerConnected = false;
                DisConnect_Panel_Clone = (GameObject)Instantiate(DisConnect_Panel, GameObject.FindGameObjectWithTag("Canvas").transform);
                DisConnect_Panel_Clone.SetActive(true);
                LoginResult = "重連線中";
                break;
            case StatusCode.TimeoutDisconnect:
                this.LoginStatus = false;
                this.ServerConnected = false;
                DisConnect_Panel_Clone = (GameObject)Instantiate(DisConnect_Panel, GameObject.FindGameObjectWithTag("Canvas").transform);
                DisConnect_Panel_Clone.SetActive(true);
                LoginResult = "重連線中";
                break;
            case StatusCode.DisconnectByServerUserLimit:
                this.LoginStatus = false;
                this.ServerConnected = false;
                DisConnect_Panel_Clone = (GameObject)Instantiate(DisConnect_Panel, GameObject.FindGameObjectWithTag("Canvas").transform);
                DisConnect_Panel_Clone.SetActive(true);
                LoginResult = "重連線中";
                break;
            case StatusCode.DisconnectByServerLogic:
                this.LoginStatus = false;
                this.ServerConnected = false;
                DisConnect_Panel_Clone = (GameObject)Instantiate(DisConnect_Panel, GameObject.FindGameObjectWithTag("Canvas").transform);
                DisConnect_Panel_Clone.SetActive(true);
                LoginResult = "重連線中";
                break;
        }
    }
    public void OnEvent(EventData eventData) { throw new System.NotImplementedException(); }

    public void Creat(string FBKey, string P8Key)//創帳號
    {
        if (this.ServerConnected)
        {
            print("FB" + FBKey);
            print("P8" + P8Key);
            var parameter = new Dictionary<byte, object> { { 0, FBKey }, { 1, P8Key } };
            this.peer.OpCustom((byte)(byte)Command.CWP_UserEasyCreate, parameter, false);
        }
    }
    public void LogIn()
    {
        if (this.ServerConnected == true)
        {
            getMemberID = PlayerPrefs.GetString("ID");
            print("LogIn");
            var parameter = new Dictionary<byte, object> { { 0, getMemberID }, { 1, "1" } };
            this.peer.OpCustom((byte)Command.CWP_UserIDLogin, parameter, false);
        }
    }
    public void LogOut()//登出
    {
        if (this.ServerConnected)
        {
            this.peer.OpCustom((byte)Command.CWP_UserLogout, null, false);
            PlayerPrefs.DeleteAll();//清紀錄
        }
    }
    public void ReConnect()//斷線重連
    {
        if (this.ServerConnected == false)
        {
            this.ServerConnected = false;
            this.LoginStatus = false;
            this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            this.Connect();
            ReConnectWork = true;
        }
    }
    public void SetUserNickName(string getNickname)//設暱稱
    {      
        var parameter = new Dictionary<byte, object> { { 0, getMemberID }, { 1, getNickname } };
        this.peer.OpCustom((byte)(byte)Command.CWP_SetNickName, parameter, false);
    }

    public void FBLogIn(string Account)//FB登入
    {
        if (this.ServerConnected == true)
        {
            getFBKey = Account;
            print("FBLogIn");
            var parameter = new Dictionary<byte, object> { { 0, Account }, { 1, "1" } };
            this.peer.OpCustom((byte)(byte)Command.CWP_UserFBLogin, parameter, false);
        }
    }
    public void P8LogIn(string Account)//P8登入
    {
        if (this.ServerConnected == true)
        {
            getP8Key = Account;
            print("P8LogIn");
            var parameter = new Dictionary<byte, object> { { 0, Account }, { 1, "1" } };
            this.peer.OpCustom((byte)(byte)Command.CWP_UserP8Login, parameter, false);
        }
    }
    public void LinkFB(string Account)//連結FB
    {
        if (this.ServerConnected == true)
        {
            getFBKey = Account;
            var parameter = new Dictionary<byte, object> { { 0, getMemberID }, { 1, getFBKey } };
            this.peer.OpCustom((byte)(byte)Command.CWP_SetFBKey, parameter, false);
        }
    }

    public void GetPlayerInfo()//讀取玩家資訊
    {
        if (this.ServerConnected) { this.peer.OpCustom((byte)(byte)Command.CWP_PlayerInfo, null, false); }
    }
    public void GetSCG(int GameNum)//讀取遊戲資訊
    {
        if (this.ServerConnected)
        {
            switch (GameNum)
            {
                case 3: this.peer.OpCustom((byte)Command.CWP_CookGirl_Info, null, false); break;
                case 4: this.peer.OpCustom((byte)Command.CWP_MagicGirl_Info, null, false); break;
                case 5: this.peer.OpCustom((byte)Command.CWP_ComeCat_Info, null, false); break;
                case 6: this.peer.OpCustom((byte)Command.CWP_EgyptKing_Info, null, false); break;
                case 7: this.peer.OpCustom((byte)Command.CWP_GreenDay_Info, null, false);break;
                case 8: this.peer.OpCustom((byte)Command.CWP_AlsDream_Info, null, false); break;
            }
            getGSCed = true;
        }
    }
    public void SendBet(int GameNum)//下注
    {
        if (this.ServerConnected)
        {
            var parameter = new Dictionary<byte, object> { { 0, getMemberID }, { 1, iBet }, { 2, Test } };
            switch (GameNum)
            {
                case 3: this.peer.OpCustom((byte)Command.CWP_CookGirl_Bet, parameter, false); break;
                case 4: this.peer.OpCustom((byte)Command.CWP_MagicGirl_Bet, parameter, false); break;
                case 5: this.peer.OpCustom((byte)Command.CWP_ComeCat_Bet, parameter, false); break;
                case 6: this.peer.OpCustom((byte)Command.CWP_EgyptKing_Bet, parameter, false); break;
                case 7: this.peer.OpCustom((byte)Command.CWP_GreenDay_Bet, parameter, false); break;
                case 8: this.peer.OpCustom((byte)Command.CWP_AlsDream_Bet, parameter, false); break;
            }
            isBetSent = true;
        }
    }

    public void CheckMail(){
        var parameter = new Dictionary<byte, object> { { 0, 0 }, { 1, 10 }};
        this.peer.OpCustom((byte)Command.CWP_Mail_Info, parameter, false);}//61
    public void SendMail(int money, string nick, string title, string info)
    {
        var parameter = new Dictionary<byte, object>{{0, money},{1, nick },{2, title},{3, info}};
        this.peer.OpCustom((byte)Command.CWP_Mail_Send, parameter, false);//62
    }
    public void GetMail(string id)
    {
        var parameter = new Dictionary<byte, object>{{0,id},};
        this.peer.OpCustom((byte)Command.CWP_Mail_Get, parameter, false);//63
    }

    public void CheckID(string memberID)//檢查此帳號是否有(但是沒有登入) 創帳號時用
    {
        var parameter = new Dictionary<byte, object> { { 0, memberID }, { 1, 1 } }; //帳號 密碼
        this.peer.OpCustom((byte)(byte)Command.CWP_UserCheckID, parameter, false);
    }
    public void SetGooglePurchase(string memberID, string PurchasesToken, string ProductID, string OrderID)
    {
        var parameter = new Dictionary<byte, object> { { 0, memberID }, { 1, PurchasesToken }, { 2, ProductID }, { 3, OrderID } };
        this.peer.OpCustom((byte)(byte)Command.CWP_GoolgePurchases, parameter, false);
    }
    public void GetBonus()//要求領取轉速獎勵
    {
        if (this.ServerConnected) { this.peer.OpCustom((byte)(byte)Command.CWP_RollBonusGet, null, false); }
    }

    void OnDestroy() { if (this.ServerConnected) { if (this.ServerConnected == true) { this.peer.Disconnect(); } } }
    void OnGUI()
    {
        /*
        GUI.Label(new Rect(30, 250, 400, 20), "L1_" + iLine25[0]);
        GUI.Label(new Rect(30, 270, 400, 20), "L2_" + iLine25[1]);
        GUI.Label(new Rect(30, 290, 400, 20), "L3_" + iLine25[2]);
        GUI.Label(new Rect(30, 310, 400, 20), "L4_" + iLine25[3]);
        GUI.Label(new Rect(30, 330, 400, 20), "L5_" + iLine25[4]);
        GUI.Label(new Rect(30, 350, 400, 20), "L6_" + iLine25[5]);
        GUI.Label(new Rect(30, 370, 400, 20), "L7_" + iLine25[6]);
        GUI.Label(new Rect(30, 390, 400, 20), "L8_" + iLine25[7]);
        GUI.Label(new Rect(30, 410, 400, 20), "L9_" + iLine25[8]);
        GUI.Label(new Rect(30, 430, 400, 20), "L10_" + iLine25[9]);
        GUI.Label(new Rect(30, 450, 400, 20), "L11_" + iLine25[10]);
        GUI.Label(new Rect(30, 470, 400, 20), "L12_" + iLine25[11]);
        GUI.Label(new Rect(30, 490, 400, 20), "L13_" + iLine25[12]);
        GUI.Label(new Rect(30, 510, 400, 20), "L14_" + iLine25[13]);
        GUI.Label(new Rect(30, 530, 400, 20), "L15_" + iLine25[14]);
        GUI.Label(new Rect(30, 550, 400, 20), "L16_" + iLine25[15]);
        GUI.Label(new Rect(30, 570, 400, 20), "L17_" + iLine25[16]);
        GUI.Label(new Rect(30, 590, 400, 20), "L18_" + iLine25[17]);
        GUI.Label(new Rect(30, 610, 400, 20), "L19_" + iLine25[18]);
        GUI.Label(new Rect(30, 630, 400, 20), "L20_" + iLine25[19]);
        GUI.Label(new Rect(30, 650, 400, 20), "L21_" + iLine25[20]);
        GUI.Label(new Rect(30, 670, 400, 20), "L22_" + iLine25[21]);
        GUI.Label(new Rect(30, 690, 400, 20), "L23_" + iLine25[22]);
        GUI.Label(new Rect(30, 710, 400, 20), "L24_" + iLine25[23]);
        GUI.Label(new Rect(30, 730, 400, 20), "L25_" + iLine25[24]);

        if (GUI.Button(new Rect(150, 250, 40, 20), "斷線"))// (注13) 
        {
            if (this.ServerConnected)
            {
                if (this.ServerConnected == true)
                {
                    this.peer.Disconnect();
                }
            }
        }

        GUI.Label(new Rect(90, 250, 400, 20), this.LoginResult);
        GUI.Label(new Rect(90, 270, 400, 20), "Your Nickname : " + getNickname);
        GUI.Label(new Rect(90, 290, 400, 20), "Your Money : " + getUserMoney);
        GUI.Label(new Rect(90, 310, 400, 20), "Your ID : " + getMemberID);
        GUI.Label(new Rect(90, 330, 400, 20), "目前等級 " + iLV);
        GUI.Label(new Rect(90, 350, 400, 20), "經驗值 : " + iEXP);

        GUI.Label(new Rect(90, 370, 400, 20), "一般線獎 : " + Line_Money);
        GUI.Label(new Rect(90, 390, 400, 20), "是否轉到關卡 : " + iAllFG);
        GUI.Label(new Rect(90, 410, 400, 20), "現在關卡 : " + iLVNow);
        GUI.Label(new Rect(90, 430, 400, 20), "關卡數量 : " + iLVNum);

        GUI.Label(new Rect(90, 450, 400, 20), "" + islot3x5[0]);
        GUI.Label(new Rect(110, 450, 400, 20), "" + islot3x5[1]);
        GUI.Label(new Rect(130, 450, 400, 20), "" + islot3x5[2]);
        GUI.Label(new Rect(150, 450, 400, 20), "" + islot3x5[3]);
        GUI.Label(new Rect(170, 450, 400, 20), "" + islot3x5[4]);
        GUI.Label(new Rect(90, 470, 400, 20), "" + islot3x5[5]);
        GUI.Label(new Rect(110, 470, 400, 20), "" + islot3x5[6]);
        GUI.Label(new Rect(130, 470, 400, 20), "" + islot3x5[7]);
        GUI.Label(new Rect(150, 470, 400, 20), "" + islot3x5[8]);
        GUI.Label(new Rect(170, 470, 400, 20), "" + islot3x5[9]);
        GUI.Label(new Rect(90, 490, 400, 20), "" + islot3x5[10]);
        GUI.Label(new Rect(110, 490, 400, 20), "" + islot3x5[11]);
        GUI.Label(new Rect(130, 490, 400, 20), "" + islot3x5[12]);
        GUI.Label(new Rect(150, 490, 400, 20), "" + islot3x5[13]);
        GUI.Label(new Rect(170, 490, 400, 20), "" + islot3x5[14]);       */
    }
    public void TestPrize(int Prize) { Test = Prize; }
}