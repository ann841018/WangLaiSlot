using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift_AnimationControl : MonoBehaviour {

	// Use this for initialization
	void Start (){}
	
	// Update is called once per frame
	void Update () {
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetBool("Open", true);//打開動畫
    }
}
