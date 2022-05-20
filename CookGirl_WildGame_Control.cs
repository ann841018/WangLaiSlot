using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookGirl_WildGame_Control : MonoBehaviour {

    [SerializeField] GameObject Wild_Pic;
    [SerializeField] GameObject[] Wild_Panel = new GameObject[5];
    int y = 35;

    // Use this for initialization
    void Start () {  if (PlayerPrefs.GetInt("SceneNum") == 8) { y = 0; } }
	
	// Update is called once per frame
	void Update () {
        if (SlotTurnSys.PlayWildAnim == true)
        {
            if (SlotTurnSys.CanTurn == false)
            {
                for (int i = 4; i >= 0; i--)
                {
                    if (i == PhotonClient.newPhotonClient.iLVNum)
                    {
                        Wild_Panel[i].SetActive(true);
                       
                        Wild_Pic.transform.localPosition = new Vector2(280 * (i) - 560, y);
                    }else { Wild_Panel[i].SetActive(false); }
                }
            }
            else if (SlotTurnSys.CanTurn == true)
            {
                for (int i = 4; i >= 0; i--)
                {
                    if (i == PhotonClient.newPhotonClient.iLVNum)
                    {
                        if (i == 4) { Wild_Pic.SetActive(true); Wild_Pic.transform.localPosition = new Vector2(560, y); }
                        Wild_Pic.GetComponentInChildren<Animator>().SetInteger("Wild", 1);
                        Wild_Panel[i].SetActive(true);
                        if (SlotTurnSys.AutoPlay == true){Wild_Pic.transform.localPosition = Vector2.Lerp(Wild_Pic.transform.localPosition, new Vector2(280 * (i) - 560, y), Time.deltaTime * 4f);}
                        if (SlotTurnSys.AutoPlay == false){Wild_Pic.transform.localPosition = Vector2.Lerp(Wild_Pic.transform.localPosition, new Vector2(280 * (i) - 560, y), Time.deltaTime * 1.5f);}
                    }else Wild_Panel[i].SetActive(false);
                }
                if (PhotonClient.newPhotonClient.iLVNow == 0 && PhotonClient.newPhotonClient.islot3x5[0] != 10) { SlotTurnSys.PlayWildAnim = false; }
            }
        }
        else if (SlotTurnSys.PlayWildAnim == false)
        {
            if (Wild_Pic.transform.localPosition.x==-560) Wild_Pic.SetActive(false);
            if (SlotTurnSys.CanTurn == false) { if (PhotonClient.newPhotonClient.iLVNum == 0) Wild_Pic.SetActive(false); }
            Wild_Panel[0].SetActive(false);
            Wild_Pic.transform.localPosition = new Vector2(0, y);
        }
    }
}
