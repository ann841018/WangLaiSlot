using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTurnSysS : MonoBehaviour
{
    #region//宣告變數
    [SerializeField] GameObject[] Line = new GameObject[25];//線圖案
    [SerializeField] GameObject[] FiveSlot = new GameObject[5];//輪物件
    [SerializeField] GameObject[,] SlotTurn_Image = new GameObject[5, 12];//五輪
    [SerializeField] GameObject Line_Sound;
    [SerializeField] Button Spin;

    [SerializeField] Animator MoneyFly;
    [SerializeField] AudioSource Stop_Sound;
    [SerializeField] AudioClip ReelStop_Sound;
    [SerializeField] AudioClip FiveReelStop_Sound;

    [SerializeField] Sprite[] SlotSprite = new Sprite[16];//輪盤圖案

    int MaxJ;//連線是幾個3 4 5 
    int MoveSize;//輪轉的速度
    int[] PlayAnimNum = new int[5];//播動畫的數值
    int[] SlotTurn_Num = new int[5];//五個輪
    int[,,] SlotTurnNum = new int[5, 5, 12]{
    {{5,6,5,4,7,8,2,9,9,0,8,7},{2,7,5,9,6,4,8,7,8,0,9,9},{6,8,7,2,6,4,8,9,0,8,8,4},{10,11,12,4,8,2,8,5,8,0,9,7},{13,14,15,9,6,7,5,7,9,0,8,3}},
    {{4,8,5,3,7,4,2,8,6,1,7,4},{2,9,4,7,6,3,8,4,9,8,2,6},{4,9,5,8,2,7,4,6,9,1,8,5},{10,11,12,8,2,6,9,3,8,5,7,2},{13,14,15,9,8,2,9,3,8,1,6,3}},
    {{6,5,4,7,8,2,9,7,4,6,7,5},{7,5,9,6,4,8,7,5,6,2,9,5},{8,7,2,6,4,9,5,2,7,8,4,9},{10,11,12,4,8,2,8,9,4,7,3,8},{13,14,15,9,6,7,7,9,7,8,3,6}},
    {{2,3,4,5,6,7,8,9,2,3,4,5},{5,6,7,8,9,2,3,4,5,6,7,8},{3,4,5,6,7,8,9,2,3,4,5,6},{5,6,7,8,9,2,3,4,5,6,7,8},{2,3,4,5,6,7,2,3,4,5,6,7}},
    {{2,2,2,3,3,3,4,4,4,5,5,5},{6,6,6,7,7,7,8,8,8,9,9,9},{4,4,4,5,5,5,6,6,6,7,7,7},{8,8,8,9,9,9,2,2,2,3,3,3},{3,3,3,4,4,4,5,5,5,6,6,6}}};
    float[] newY = new float[5];
    float[] ChangeSlotTime = new float[10];//輪軸旋轉時間設定
    bool PlaySound;

    Animator[] PlayAnim = new Animator[15];
    #endregion
    void Start()// Use this for initialization
    {
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 12; i++)
            {
                SlotTurn_Image[j, i] = FiveSlot[j].transform.GetChild(i).gameObject;
                SlotTurn_Image[j, i].GetComponent<Image>().sprite = SlotSprite[SlotTurnNum[Random.Range(0, 3), 0, i]];
            }
        }
        if (PlayerPrefs.GetInt("SceneNum") == 5)
        {
            SlotTurnNum = new int[5, 5, 12]{
    {{5,6,5,4,7,8,2,9,9,0,8,7},{8,7,8,0,9,9,5,8,7,3,9,8},{6,8,7,2,6,4,8,9,0,8,8,4},{10,11,12,4,8,2,8,5,8,0,9,7},{13,14,15,9,6,7,5,7,9,0,8,3}},
    {{4,8,5,3,7,4,2,8,6,1,7,4},{3,8,4,9,8,2,6,7,4,9,8,3},{4,9,5,8,2,7,4,6,9,1,8,5},{10,11,12,8,2,6,9,3,8,5,7,2},{13,14,15,9,8,2,9,3,8,1,6,3}},
    {{9,6,5,4,7,8,2,9,7,4,6,7},{7,5,9,6,4,8,7,5,6,2,9,5},{3,6,8,7,2,6,4,9,5,2,7,8},{10,11,12,4,8,2,8,5,7,9,4,7},{13,14,15,9,6,7,5,7,9,7,8,3}},
    {{2,3,4,5,6,7,8,9,2,3,4,5},{6,7,8,9,2,3,4,5,6,7,8,9},{2,3,4,5,6,7,8,9,2,3,4,5},{6,7,8,9,2,3,4,5,6,7,8,9},{2,3,4,5,6,7,8,9,2,3,4,5}},
    {{2,2,2,3,3,3,4,4,4,5,5,5},{6,6,6,7,7,7,8,8,8,9,9,9},{4,4,4,5,5,5,6,6,6,7,7,7},{8,8,8,9,9,9,2,2,2,3,3,3},{3,3,3,4,4,4,5,5,5,6,6,6}}};
        }
        else if (PlayerPrefs.GetInt("SceneNum") == 6)
        {
            SlotTurnNum = new int[5, 5, 12] {
    {{5,6,5,4,7,8,2,9,9,0,8,7},{7,5,9,6,4,8,7,8,0,9,9,5},{3,6,8,7,2,6,4,8,9,0,8,8},{10,11,12,4,8,2,8,5,8,0,9,7},{13,14,15,9,6,7,5,7,9,0,8,3}},
    {{4,8,5,3,7,4,2,8,6,1,7,4},{6,3,8,4,9,8,2,6,7,4,9,8},{1,8,5,7,3,6,5,9,3,8,6,0},{10,11,12,8,2,6,9,3,8,5,7,2},{13,14,15,9,8,2,9,3,8,1,6,3}},
    {{9,6,5,4,7,8,2,9,7,4,6,7},{8,7,5,6,2,9,5,8,0,3,9,8},{3,6,8,7,2,6,4,9,5,2,7,8},{10,11,12,4,8,2,8,5,7,9,4,7},{13,14,15,9,6,7,5,7,9,7,8,3}},
    {{2,3,4,5,6,7,8,9,2,3,4,5},{6,7,8,9,2,3,4,5,6,7,8,9},{2,3,4,5,6,7,8,9,2,3,4,5},{6,7,8,9,2,3,4,5,6,7,8,9},{2,3,4,5,6,7,8,9,2,3,4,5}},
    {{2,2,2,3,3,3,4,4,4,5,5,5},{6,6,6,7,7,7,8,8,8,9,9,9},{4,4,4,5,5,5,6,6,6,7,7,7},{8,8,8,9,9,9,2,2,2,3,3,3},{3,3,3,4,4,4,5,5,5,6,6,6}}};
        }
    }
    void FixedUpdate()// Update is called once per frame
    {
        if (SlotTurnSys.CanTurn == true)//正在轉
        {
            Line_Sound.SetActive(false);
            for (int i = 0; i < 25; i++)
            {
                if (PhotonClient.newPhotonClient.iLine25[i] != 0)
                {
                    Line[i].SetActive(false);//隱藏25線
                    MoneyFly.SetBool("MoneyFly", false);
                    for (int j = 0; j < 15; j++)
                    {
                        if (PlayAnim[j].enabled == true)
                        {
                            PlayAnim[j].SetInteger("SlotImage", -1);
                            //PlayAnim[j].enabled = false;
                        }
                    }
                }
            }
            if (SlotTurnSys.AutoPlay == false)//沒有開自動
            {
                if (Spin_Control.StopAuto == false)//沒按停止鍵
                {
                    SlotTurnSys.PlayAnimWait = 4f; MoveSize = -50;
                    Stop_Sound.clip = FiveReelStop_Sound;
                    ChangeSlotTime = new float[10] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 1.5f, 2f, 2.5f, 3f };
                }
                else if (Spin_Control.StopAuto == true)//按停止鍵
                {
                    SlotTurnSys.PlayAnimWait = 2f; MoveSize = -60;
                    Stop_Sound.clip = ReelStop_Sound;
                    if (SlotTurnSys.time <= 1.5f) { ChangeSlotTime = new float[10] { 1f, 1f, 1f, 1f, 1f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f }; }
                    else if (SlotTurnSys.time > 1.5f && SlotTurnSys.time <= 2f) { ChangeSlotTime = new float[10] { 1f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 2f, 2f, 2f }; }
                    else if (SlotTurnSys.time > 2f && SlotTurnSys.time <= 2.5f) { ChangeSlotTime = new float[10] { 1f, 1.5f, 2f, 2f, 2f, 2.5f, 1.5f, 2f, 2.5f, 2.5f }; }
                    else if (SlotTurnSys.time > 2.5f) { ChangeSlotTime = new float[10] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 1.5f, 2f, 2.5f, 3f }; }
                }
            }
            else if (SlotTurnSys.AutoPlay == true)//開自動
            {
                SlotTurnSys.PlayAnimWait = 1.5f; MoveSize = -60;//移動速度加快
                Stop_Sound.clip = ReelStop_Sound;
                ChangeSlotTime = new float[10] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 1f, 1f, 1f, 1f, 1f };
            }
            if (SlotTurnSys.CanChangeSlotImage == false)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (SlotTurn_Image[j, i].transform.localPosition.y <= -450)
                        {
                            SlotTurn_Image[j, i].transform.rotation = new Quaternion(0, 0, 0, 0);
                            if (i != 11) SlotTurn_Image[j, i].transform.localPosition = new Vector2(0, SlotTurn_Image[j, i + 1].transform.localPosition.y + 225);//換行
                            else if (i == 11) SlotTurn_Image[j, i].transform.localPosition = new Vector2(0, SlotTurn_Image[j, 0].transform.localPosition.y + 275);
                        }
                        SlotTurn_Image[j, i].transform.position = new Vector3(SlotTurn_Image[j, i].transform.position.x, SlotTurn_Image[j, i].transform.position.y + MoveSize, 0);//移動
                    }
                }
                PlaySound = true;
            }
            if (SlotTurnSys.CanChangeSlotImage == true)//換成Server給的結果
            {
                if (SlotTurnSys.ChangeFakeSlotImage == true) { if (SlotTurnSys.time > 0.5f) ChangeTurnImage(); else if (SlotTurnSys.time < 0.5f) { for (int j = 0; j < 15; j++) PlayAnim[j].enabled = false; } }
                SlotTurnSys.time = SlotTurnSys.time + Time.deltaTime;
                if (SlotTurnSys.time > ChangeSlotTime[0]) { if (PlaySound == true) { Stop_Sound.Play(); PlaySound = false; } }
                for (int j = 0; j < 5; j++){for (int i = 0; i < 12; i++) { 
                    if (SlotTurn_Image[j, i].transform.localPosition.y < -225){
                    SlotTurn_Image[j, i].transform.eulerAngles = new Vector3(0, 0, 0);
                    if (i != 11) SlotTurn_Image[j, i].transform.localPosition = new Vector2(SlotTurn_Image[j, i].transform.localPosition.x, SlotTurn_Image[j, i + 1].transform.localPosition.y + 225);//換行
                    else if (i == 11) SlotTurn_Image[j, i].transform.localPosition = new Vector2(SlotTurn_Image[j, i].transform.localPosition.x, SlotTurn_Image[j, 0].transform.localPosition.y + 275);}
                    if (SlotTurnSys.time <= ChangeSlotTime[j]) { SlotTurn_Image[j, i].transform.position = new Vector2(SlotTurn_Image[j, i].transform.position.x, SlotTurn_Image[j, i].transform.position.y + MoveSize); }//移動
                    if (j != 4)
                        {
                            if (SlotTurnSys.time > ChangeSlotTime[j] && SlotTurnSys.time <= ChangeSlotTime[j + 6]) { ChangeImageToCorrect(j, i);
                            }//換圖 
                            else if (SlotTurnSys.time >= ChangeSlotTime[5]) { SlotTurnSys.CanTurn = false; }
                        }
                    else if (j == 4)
                        {
                            if (SlotTurnSys.time > ChangeSlotTime[j] && SlotTurnSys.time <= ChangeSlotTime[5]) { ChangeImageToCorrect(j, i); }//換圖 
                            else if (SlotTurnSys.time >= ChangeSlotTime[5]) { SlotTurnSys.CanTurn = false; }
                        }
                }}
            }
        }
        else if (SlotTurnSys.CanTurn == false)//沒有在轉
        {

            SlotTurnSys.time = SlotTurnSys.time + Time.deltaTime;
            SlotTurnSys.CanChangeSlotImage = false;
            if (SlotTurnSys.AutoPlay == false) { if (Spin_Control.StopAuto == true) Spin_Control.StopAuto = false; }
            if (SlotTurnSys.PlayBonusGame == true) { if (SlotTurnSys.AutoPlay == true) { if (SlotTurnSys.CanTurn == false) { SlotTurnSys.TimeAuto = true; SlotTurnSys.AutoPlay = false; } } }
            if (Auto_Control.AutoNum[Auto_Control.SwitchNum] == 0) SlotTurnSys.AutoPlay = false;
            for (int i = 0; i < 25; i++)
            {
                if (PhotonClient.newPhotonClient.iLine25[i] != 0)//如果有連線
                {
                    if (PhotonClient.newPhotonClient.iLine25[i] % 3 == 1) MaxJ = 5;
                    else if (PhotonClient.newPhotonClient.iLine25[i] % 3 == 2) MaxJ = 4;
                    else if (PhotonClient.newPhotonClient.iLine25[i] % 3 == 0) MaxJ = 3;
                    Line[i].SetActive(true);
                    Line_Sound.SetActive(true);
                    switch (i)//25線
                    {
                        case 0:
                            PlayAnimNum = new int[] { 0, 1, 2, 3, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 1:
                            PlayAnimNum = new int[] { 5, 6, 7, 8, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[7]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 2:
                            PlayAnimNum = new int[] { 10, 11, 12, 13, 14 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 3:
                            PlayAnimNum = new int[] { 0, 6, 12, 8, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 4:
                            PlayAnimNum = new int[] { 10, 6, 2, 8, 14 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 5:
                            PlayAnimNum = new int[] { 5, 1, 2, 3, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 6:
                            PlayAnimNum = new int[] { 5, 11, 12, 13, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 7:
                            PlayAnimNum = new int[] { 0, 1, 7, 13, 14 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[7]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 8:
                            PlayAnimNum = new int[] { 10, 11, 7, 3, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[7]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 9:
                            PlayAnimNum = new int[] { 5, 1, 7, 13, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[7]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 10:
                            PlayAnimNum = new int[] { 5, 11, 7, 3, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[7]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 11:
                            PlayAnimNum = new int[] { 0, 6, 7, 8, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[7]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 12:
                            PlayAnimNum = new int[] { 10, 6, 7, 8, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[7]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 13:
                            PlayAnimNum = new int[] { 0, 6, 2, 8, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 14:
                            PlayAnimNum = new int[] { 10, 6, 12, 8, 14 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 15:
                            PlayAnimNum = new int[] { 5, 6, 2, 8, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 16:
                            PlayAnimNum = new int[] { 5, 6, 12, 8, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[8] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 17:
                            PlayAnimNum = new int[] { 0, 1, 12, 3, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 18:
                            PlayAnimNum = new int[] { 10, 11, 2, 13, 14 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 19:
                            PlayAnimNum = new int[] { 0, 11, 12, 13, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 20:
                            PlayAnimNum = new int[] { 10, 1, 2, 3, 14 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 21:
                            PlayAnimNum = new int[] { 5, 1, 12, 3, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 22:
                            PlayAnimNum = new int[] { 5, 11, 2, 13, 9 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[5] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[5]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[9] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 23:
                            PlayAnimNum = new int[] { 0, 11, 2, 13, 4 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[0] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[0]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[2]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[13] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[4] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;
                        case 24:
                            PlayAnimNum = new int[] { 10, 1, 12, 3, 14 }; PlayAnim[PlayAnimNum[0]] = SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[1]] = SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[2]] = SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(); PlayAnim[PlayAnimNum[3]] = SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(); PlayAnim[PlayAnimNum[4]] = SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>();
                            for (int j = 0; j < MaxJ; j++) { PlayAnim[PlayAnimNum[j]].enabled = true; if (PhotonClient.newPhotonClient.islot3x5[10] < 10) PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[10]); else PlayAnim[PlayAnimNum[j]].SetInteger("SlotImage", PhotonClient.newPhotonClient.islot3x5[12]); }
                            if (MaxJ >= 4)
                            {
                                if (PhotonClient.newPhotonClient.islot3x5[3] >= 10) { PlayAnim[PlayAnimNum[3]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[3]].enabled = false; }
                                if (MaxJ == 5) { if (PhotonClient.newPhotonClient.islot3x5[14] >= 13) { PlayAnim[PlayAnimNum[4]].SetInteger("SlotImage", -1); PlayAnim[PlayAnimNum[4]].enabled = false; } }
                            }
                            break;

                    }

                }
            }
            if (SlotTurnSys.time > SlotTurnSys.PlayAnimWait)//等動畫播完
            {
                PlayAnim = new Animator[15] { SlotTurn_Image[0, SlotTurn_Num[0]].GetComponent<Animator>(), SlotTurn_Image[1, SlotTurn_Num[1]].GetComponent<Animator>(), SlotTurn_Image[2, SlotTurn_Num[2]].GetComponent<Animator>(), SlotTurn_Image[3, SlotTurn_Num[3]].GetComponent<Animator>(), SlotTurn_Image[4, SlotTurn_Num[4]].GetComponent<Animator>(), SlotTurn_Image[0, (SlotTurn_Num[0] + 1) % 12].GetComponent<Animator>(), SlotTurn_Image[1, (SlotTurn_Num[1] + 1) % 12].GetComponent<Animator>(), SlotTurn_Image[2, (SlotTurn_Num[2] + 1) % 12].GetComponent<Animator>(), SlotTurn_Image[3, (SlotTurn_Num[3] + 1) % 12].GetComponent<Animator>(), SlotTurn_Image[4, (SlotTurn_Num[4] + 1) % 12].GetComponent<Animator>(), SlotTurn_Image[0, (SlotTurn_Num[0] + 2) % 12].GetComponent<Animator>(), SlotTurn_Image[1, (SlotTurn_Num[1] + 2) % 12].GetComponent<Animator>(), SlotTurn_Image[2, (SlotTurn_Num[2] + 2) % 12].GetComponent<Animator>(), SlotTurn_Image[3, (SlotTurn_Num[3] + 2) % 12].GetComponent<Animator>(), SlotTurn_Image[4, (SlotTurn_Num[4] + 2) % 12].GetComponent<Animator>() };
                for (int i = 0; i < 25; i++) { Line[i].SetActive(false); } //線關掉
                if (SlotTurnSys.PlayBonusGame == true)//Bonus動畫
                {
                    if (SlotTurnSys.time > (SlotTurnSys.PlayAnimWait + 1.5f)) { ChangeTurnImage(); }//等動畫播完
                }
            }
            else if (SlotTurnSys.PlayBonusGame == true) Spin.enabled = false;
        }
    }

    public static void GetSlotInfo() { SlotTurnSys.CanChangeSlotImage = true; }
    public static void StartTurn()//開始轉
    {
        SlotTurnSys.time = 0;
        SlotTurnSys.CanTurn = true;
    }
    void ChangeTurnImage()//切換輪轉圖
    {
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 12; i++)
            {
                if (PhotonClient.newPhotonClient.iAllFG == 2)
                {
                    if (SlotTurnSys.CanTurn == false)
                    {
                        SlotTurn_Image[j, i].GetComponent<Image>().sprite = SlotSprite[SlotTurnNum[4, 0, i]];
                    }
                }
                else
                {
                    if (PhotonClient.newPhotonClient.iLVNow == 0)
                    {
                        if (PhotonClient.newPhotonClient.islot3x5[0] == 10)
                        {
                            SlotTurn_Image[j, i].GetComponent<Image>().sprite = SlotSprite[SlotTurnNum[3, j, i]];
                        }
                        else
                        {
                            SlotTurn_Image[j, i].GetComponent<Image>().sprite = SlotSprite[SlotTurnNum[Random.Range(0, 3), j, i]];
                            SlotTurn_Image[j, i].transform.eulerAngles = new Vector3(0, 0, 0);
                            if (SlotTurn_Num[j] + 1 - i < 11) { SlotTurn_Image[j, i].transform.localPosition = new Vector2(0, (SlotTurn_Num[j] + 1 - i) * 225); }
                        }
                    }
                    else if (PhotonClient.newPhotonClient.iLVNow == 2)
                    {
                        SlotTurn_Image[j, i].GetComponent<Image>().sprite = SlotSprite[SlotTurnNum[4, j, i]];
                    }
                    else if (PhotonClient.newPhotonClient.iLVNow == 3)
                    {
                        SlotTurn_Image[j, i].GetComponent<Image>().sprite = SlotSprite[SlotTurnNum[3, j, i]];
                    }
                }
            }
        }
        SlotTurnSys.ChangeFakeSlotImage = false;
    }
    void ChangeImageToCorrect(int j, int i)
    {
        if (SlotTurn_Image[j, i].transform.localPosition.y > 338 && SlotTurn_Image[j, i].transform.localPosition.y <= 568)
        {
            SlotTurn_Num[j] = i;
            if (i < 10)
            {
                if (PhotonClient.newPhotonClient.islot3x5[j + 0] != -1) SlotTurn_Image[j, i].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 0]];
                if (PhotonClient.newPhotonClient.islot3x5[j + 5] != -1) SlotTurn_Image[j, i + 1].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 5]];
                if (PhotonClient.newPhotonClient.islot3x5[j + 10] != -1) SlotTurn_Image[j, i + 2].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 10]];
            }
            else if (i == 10)
            {
                if (PhotonClient.newPhotonClient.islot3x5[j + 0] != -1) SlotTurn_Image[j, 10].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 0]];
                if (PhotonClient.newPhotonClient.islot3x5[j + 5] != -1) SlotTurn_Image[j, 11].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 5]];
                if (PhotonClient.newPhotonClient.islot3x5[j + 10] != -1) SlotTurn_Image[j, 0].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 10]];
            }
            else if (i == 11)
            {
                if (PhotonClient.newPhotonClient.islot3x5[j + 0] != -1) SlotTurn_Image[j, 11].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 0]];
                if (PhotonClient.newPhotonClient.islot3x5[j + 5] != -1) SlotTurn_Image[j, 0].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 5]];
                if (PhotonClient.newPhotonClient.islot3x5[j + 10] != -1) SlotTurn_Image[j, 1].GetComponent<Image>().sprite = SlotSprite[PhotonClient.newPhotonClient.islot3x5[j + 10]];
            }
        }
    }
}
