using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject itemsMenuCanvas;
    [SerializeField] private GameObject navTargetObject;
    [SerializeField] private GameObject indicador;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Inicializamos el UIManager");
        GameManager.instance.OnMainMenu += ActivateMainMenu;
        GameManager.instance.OnItemsMenu += ActivateItemsMenu;
    }

    private void ActivateMainMenu(){
        // Mostramos el boton de items de ubicacion cerrado
        mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1,1,1), 0.3f);

        // Los demás botones permanecen ocultos
        itemsMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0,0,0), 0.5f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0,0,0), 0.3f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOMoveY(180,0.3f);

        // Si hay un objeto seleccionado, habilitamos el boton de limpiar pantalla
        if (navTargetObject.activeSelf){
            Debug.Log("El boton de limpiar se ha habilitado");
            itemsMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(1,1,1), 0.3f);

            Debug.Log("Habilitamos el dropdown para cambiar las opciones");
            itemsMenuCanvas.transform.GetChild(3).transform.DOScale(new Vector3(1,1,1), 0.3f);
            
        }else{
            Debug.Log("El boton de limpiar se ha deshabilitado");
            itemsMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(0,0,0), 0.3f);

            Debug.Log("Desabilitamos el dropdown");
            itemsMenuCanvas.transform.GetChild(3).transform.DOScale(new Vector3(0,0,0), 0.3f);
        }
    }

    private void ActivateItemsMenu(){
        Debug.Log("Entramos en el metodo de ActivateItemsMenu");
        // Abrimos los itemes del canvas de itemsMenu
        
        itemsMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1,1,1), 0.5f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1,1,1), 0.3f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOMoveY(600,0.3f);

        // Cerramos los items restantes
        mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0,0,0), 0.3f);
    }

    public void limpiarRuta(){
        // Activamos la línea
        navTargetObject.SetActive(false);

        itemsMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(0,0,0), 0.3f);
        itemsMenuCanvas.transform.GetChild(3).transform.DOScale(new Vector3(0,0,0), 0.3f);

        ItemButtonManager.isDireccionConcreta = false;
    }

}
