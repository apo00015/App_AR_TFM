using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectBuiding : MonoBehaviour
{

    public int edificio_seleccionado;
   
    // Patrón Singleton
    public static ControllerSelectBuiding instance { get; private set; }

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
        }else{
            instance = this;
        }

        // El objeto Gamemanager a de persistir en todas las escenas
        DontDestroyOnLoad(gameObject);
    }


    public void UpdateEdificio(int id_edifio){
        edificio_seleccionado = id_edifio;
        Debug.Log("El nuevo valor del edificio es " + edificio_seleccionado);
    }
}