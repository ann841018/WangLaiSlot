using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Control : MonoBehaviour {

    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Upper_Panel;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {if (Input.GetKeyDown(KeyCode.Escape)){Panel.SetActive(false); Upper_Panel.SetActive(true); } }

    public void OpenUrl()
    {
        Application.OpenURL("http://webservice.pfamily.com.tw/");
    }
}
