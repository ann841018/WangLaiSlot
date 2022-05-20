using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyCat_WildGame_Control : MonoBehaviour {

    [SerializeField] GameObject Wild_Pic;
    [SerializeField] GameObject TimeStr;
    [SerializeField] Image TimeNum;
    [SerializeField] GameObject[] Wild_Panel = new GameObject[4];
    [SerializeField] GameObject [] Slot_Line = new GameObject[4];
    [SerializeField] Sprite[] Time_Num = new Sprite[5];

    int RawNum;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {
        if (SlotTurnSys.PlayWildAnim == true)
        {
            if (SlotTurnSys.CanTurn == false)
            {

                for (int i = 3; i >= 0; i--)
                {
                    if (i == PhotonClient.newPhotonClient.iLVNum)
                    {
                        Wild_Panel[i].SetActive(true);Slot_Line[i].SetActive(false);
                        Wild_Pic.transform.localPosition = new Vector2(280 * (i) - 492, 25);
                    }else { Wild_Panel[i].SetActive(false); Slot_Line[i].SetActive(true); }
                }
            }
            else if (SlotTurnSys.CanTurn == true)
            {
                for (int i = 3; i >= 0; i--)
                {
                    if (i == PhotonClient.newPhotonClient.iLVNum)
                    {
                        if (i == 3) { Wild_Pic.SetActive(true); Wild_Pic.transform.localPosition = new Vector2(348, 25); }
                        Wild_Panel[i].SetActive(true);Slot_Line[i].SetActive(false);
                        if (SlotTurnSys.AutoPlay == true) { Wild_Pic.transform.localPosition = Vector2.Lerp(Wild_Pic.transform.localPosition, new Vector2(280 * (i)-492, 25), Time.deltaTime * 4f); }
                        if (SlotTurnSys.AutoPlay == false) { Wild_Pic.transform.localPosition = Vector2.Lerp(Wild_Pic.transform.localPosition, new Vector2(280 * (i)-492, 25), Time.deltaTime * 1.5f); }
                    }
                    else { Wild_Panel[i].SetActive(false); Slot_Line[i].SetActive(true); }
                }
                if (PhotonClient.newPhotonClient.iLVNow == 0 && PhotonClient.newPhotonClient.islot3x5[0] != 10) { SlotTurnSys.PlayWildAnim = false; Slot_Line[0].SetActive(true); }
            }
        }
        else if (SlotTurnSys.PlayWildAnim == false)
        {
            if (PhotonClient.newPhotonClient.iLVNow == 2 || PhotonClient.newPhotonClient.iLestLV >= 0){
                for (int i = 0; i < 4; i++){
                    if (PhotonClient.newPhotonClient.islot3x5[i] >= 10 && PhotonClient.newPhotonClient.islot3x5[i + 1] >= 10){
                        Wild_Panel[i].SetActive(true); Slot_Line[i].SetActive(false); Wild_Pic.SetActive(true);
                        Wild_Pic.transform.localPosition = new Vector2(280 * (i) - 492, 25);
                        TimeStr.SetActive(true);
                        TimeNum.sprite = Time_Num[PhotonClient.newPhotonClient.TimeNum - 1];}
                    else { Wild_Panel[i].SetActive(false); Slot_Line[i].SetActive(true); }
                }
            }else if (PhotonClient.newPhotonClient.iLVNum == 0){
                if (PhotonClient.newPhotonClient.iLestLV == -1){Wild_Pic.SetActive(false); TimeStr.SetActive(false);
                    for (int i = 0; i < 4; i++) { Wild_Panel[i].SetActive(false); Slot_Line[i].SetActive(true); }
                }
            }
        }
    }
}
