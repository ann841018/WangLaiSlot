using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_Control : MonoBehaviour
{

    public static bool OpenMusic = true;
    public static bool OpenSound = true;

    [SerializeField] GameObject MusicObject;//音樂
    [SerializeField] GameObject SoundObject;//音效
    [SerializeField] GameObject LvUpAudio;//升等
    [SerializeField] GameObject VIPLvUpAudio;//VIP升等
    [SerializeField] GameObject ReelStopAudio;//停輪音效
    [SerializeField] GameObject CoinAudio;//Gift
    [SerializeField] GameObject LineAudio;//Line


    //Toggle開關
    [SerializeField] GameObject MusicObjectOpen;
    [SerializeField] GameObject MusicObjectClose;
    [SerializeField] GameObject SoundObjectOpen;
    [SerializeField] GameObject SoundObjectClose;

    // Use this for initialization
    void Start()
    {
        MusicObjectOpen.SetActive(OpenMusic);
        MusicObjectClose.SetActive(!OpenMusic);
        MusicObject.SetActive(OpenMusic);
        SoundObjectOpen.SetActive(OpenSound);
        SoundObjectClose.SetActive(!OpenSound);
        SoundObject.SetActive(OpenSound);
        LvUpAudio.SetActive(OpenSound);
        VIPLvUpAudio.SetActive(OpenSound);
        ReelStopAudio.SetActive(OpenSound);
        if (PlayerPrefs.GetInt("SceneNum") == 1) CoinAudio.GetComponent<AudioSource>().enabled = OpenSound;
    }

    // Update is called once per frame
    void Update()
    { if (PlayerPrefs.GetInt("SceneNum") == 1) LineAudio.SetActive(false); }

    public void Music()
    {
        OpenMusic = !OpenMusic;
        MusicObjectOpen.SetActive(OpenMusic);
        MusicObjectClose.SetActive(!OpenMusic);
        MusicObject.SetActive(OpenMusic);
    }

    public void Sound()
    {
        OpenSound = !OpenSound;
        SoundObjectOpen.SetActive(OpenSound);
        SoundObjectClose.SetActive(!OpenSound);
        SoundObject.SetActive(OpenSound);
        LvUpAudio.SetActive(OpenSound);
        VIPLvUpAudio.SetActive(OpenSound);
        ReelStopAudio.SetActive(OpenSound);
        if (PlayerPrefs.GetInt("SceneNum") == 1) CoinAudio.GetComponent<AudioSource>().enabled = OpenSound;
    }
}
