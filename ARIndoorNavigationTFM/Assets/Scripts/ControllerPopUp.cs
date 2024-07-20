using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CotrollerPopUp : MonoBehaviour
{
    public GameObject popup;
    // Patrón Singleton
    public static CotrollerPopUp instance { get; private set; }

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
        }else{
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ShowHidePopUp(){
        popup.SetActive(!popup.activeSelf);
    }

    public void HidePopUp(){
        popup.SetActive(false);
    }
}