using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerBlockRoute : MonoBehaviour
{

    public bool changeManual = false;
    // Patrón Singleton
    public static ControllerBlockRoute instance { get; private set; }

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
        }else{
            instance = this;
        }

        // El objeto Gamemanager tiene que persistir en todas las escenas
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateBlockRoute(){
        Debug.Log("Actualziamos el BlockRoute");
        changeManual = !changeManual;
    }
}