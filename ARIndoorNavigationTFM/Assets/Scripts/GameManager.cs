using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject CameraAr;
    public static GameManager instance { get; private set; }
    public event Action OnMainMenu;
    public event Action OnItemsMenu;

    private void Awake(){
        if (instance != null && instance != this){
            Debug.Log("Destruimos el objeto GAMEMANAGERRRR");
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("La posicion actual del singleton es de: " + ControllerPlayer.instance.positionPlayer);
        Debug.Log("El edificio seleccionado es " + ControllerSelectBuiding.instance.edificio_seleccionado);
        if (CameraAr != null){
            
             Vector3 postionMapa = CameraAr.transform.position;
             postionMapa.x = ControllerPlayer.instance.positionPlayer.x;
             postionMapa.y = ControllerPlayer.instance.positionPlayer.y;
             postionMapa.z = ControllerPlayer.instance.positionPlayer.z;

             CameraAr.transform.position = postionMapa;

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

}