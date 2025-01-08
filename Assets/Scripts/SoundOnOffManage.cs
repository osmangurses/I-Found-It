using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnOffManage : MonoBehaviour
{
    public string soundStatu;
    public GameObject onObject, OffObject;
    private void Awake()
    {
        if (PlayerPrefs.GetInt(soundStatu,1)==1)
        {
            onObject.SetActive(true);
            OffObject.SetActive(false);
        }
        else
        {
            onObject.SetActive(false);
            OffObject.SetActive(true);
        }
    }
    public void SendStatuToManager()
    {
        if (onObject.activeSelf)
        {
            PlayerPrefs.SetInt(soundStatu,0);
            onObject.SetActive(false);
            OffObject.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt(soundStatu, 1);
            onObject.SetActive(true);
            OffObject.SetActive(false);
        }
    }
}
