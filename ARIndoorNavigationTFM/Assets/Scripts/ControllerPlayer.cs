using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerPlayer : MonoBehaviour
{

    public Vector3 positionPlayer;
    public Vector3 rotationPlayer;
    // Patrón Singleton
    public static ControllerPlayer instance { get; private set; }

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
        }else{
            instance = this;
        }

        // El objeto Gamemanager tiene que persistir en todas las escenas
        DontDestroyOnLoad(gameObject);
    }
}