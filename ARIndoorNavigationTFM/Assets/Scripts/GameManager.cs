using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject CameraAr;
    [SerializeField] private Text position;
    public event Action OnMainMenu;
    public event Action OnItemsMenu;

    // Patrón Singleton
    public static GameManager instance { get; private set; }

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("La posicion actual del singleton es de: " + ControllerPlayer.instance.positionPlayer);
        if (CameraAr != null){
            
             Vector3 postionMapa = CameraAr.transform.position;
             postionMapa.x = ControllerPlayer.instance.positionPlayer.x;
             postionMapa.z = ControllerPlayer.instance.positionPlayer.z;

             CameraAr.transform.position = postionMapa;

             position.text = postionMapa.ToString();

        }
        MainMenu();
    }

    public void MainMenu(){
        OnMainMenu?.Invoke();
        Debug.Log("Main Menu Activated");
    }

    public void ItemsMenu(){
        OnItemsMenu?.Invoke();
        Debug.Log("Items Menu Activated");
    }

    // public void CloseApp(){
    //     Application.Quit();
    // }
}