using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FitCanvas : MonoBehaviour
{
    [SerializeField] Text Award_Up;
    [SerializeField] Text Award_Down;

    float x, y;

	// Use this for initialization
	void Start () {
        x = Screen.width;
        y = Screen.height;
        if (x / y <= 1.4f)
        {
            Award_Up.gameObject.SetActive(true);
            Award_Down.gameObject.SetActive(false);
        }
        else
        {
            Award_Up.gameObject.SetActive(false);
            Award_Down.gameObject.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Award_Up.text = Award_Down.text;
    }
}
