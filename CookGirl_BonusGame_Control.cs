using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookGirl_BonusGame_Control : MonoBehaviour {

    [SerializeField] Text CountDown_Text;
    [SerializeField] Text FreeCount_Text;
    [SerializeField] AudioSource CountDown_Sound;

    [SerializeField] GameObject Slot_Panel;
    [SerializeField] GameObject Bonus_Panel;
    [SerializeField] GameObject FreeSpin_Panel;
    [SerializeField] GameObject FreeSpin_Square;
    [SerializeField] GameObject FreeSpin_Btn;
    [SerializeField] GameObject Wild_Panel;
    [SerializeField] GameObject Wild_Pic;
    [SerializeField] GameObject[] Num_Pic = new GameObject[3];
    [SerializeField] Button[] Bun = new Button[3];

    static float time = -1;
    public static bool CanCountDown;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        int BonusNum = 3;
        if (CanCountDown == true)
        {
            time = time - Time.deltaTime;
            if (time > 0)
            {
                for (int i = 9; i > 0; i--)
                {
                    if (time >= i && time < i + 0.1f) CountDown_Sound.Play();
                }
                CountDown_Text.text = ((int)time).ToString();//倒數的數字
                FreeSpin_Btn.SetActive(true);
                if (PhotonClient.newPhotonClient.iLVNum != 0) FreeCount_Text.text = "免費";
                FreeSpin_Btn.GetComponent<Button>().enabled = false;
                for (int i = 0; i < 3; i++) { Num_Pic[i].GetComponent<Text>().text = PhotonClient.newPhotonClient.iLVNum.ToString(); }//抽到的輪數是server傳來的
            }
            else if (time <= 0 && time > -1)
            {
                for (int i = 0; i < BonusNum; i++)
                {
                    if (i == 0)
                    {
                        Bun[i].GetComponent<Gift_AnimationControl>().enabled = true;//播動畫
                        Num_Pic[i].SetActive(true);//顯示數字
                    }
                    else
                    {
                        Bun[i].GetComponent<Animator>().enabled = false; //另外兩個動畫停掉
                        Bun[i].enabled = false;//按鈕不能按
                    }
                }
                StopCountDown();//停止倒數
            }if (time < -2){CanCountDown = false;}
        }
        else if (CanCountDown == false)
        {
            for (int i = 0; i < BonusNum; i++)
            {
                Bun[i].GetComponent<Gift_AnimationControl>().enabled = false;
                Bun[i].GetComponent<Animator>().enabled = true;
                Bun[i].enabled = true;
                Num_Pic[i].SetActive(false);
            }
            time = 0;
            if (PhotonClient.newPhotonClient.iLVNum != 0) FreeCount_Text.text = "免費 " + PhotonClient.newPhotonClient.iLVNum.ToString();
            if (SlotTurnSys.TimeAuto == true) {Auto_Control.AutoNum[Auto_Control.SwitchNum] = Auto_Control.AutoNum[Auto_Control.SwitchNum] + PhotonClient.newPhotonClient.iLVNum; SlotTurnSys.AutoPlay = true; SlotTurnSys.TimeAuto = false;}                     
            if ((PlayerPrefs.GetInt("SceneNum") != 5)&&(PlayerPrefs.GetInt("SceneNum") != 6)){ Wild_Pic.SetActive(true);Wild_Panel.SetActive(true);}
            if (PlayerPrefs.GetInt("SceneNum") == 4) Wild_Pic.GetComponentInChildren<Animator>().SetInteger("Wild", 2);
            Slot_Panel.SetActive(false); Bonus_Panel.SetActive(false);
            FreeSpin_Panel.SetActive(true);FreeSpin_Square.SetActive(true);FreeSpin_Btn.SetActive(true);           
            FreeSpin_Btn.GetComponent<Button>().enabled = true;
        }
	}

    public static void StartCountDown()//開始倒數
    {
        CanCountDown = true;
        time = 10;
    }
    public void StopCountDown(){time = -1;}//停止倒數
}

