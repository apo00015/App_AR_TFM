using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private ItemButtonManager itemButtonManager;
    [SerializeField] private GameObject itemNavTargetObject;
    [SerializeField] private Camera CameraAr;
    [SerializeField] private Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnItemsMenu += CreateButtons;
    }

    private void CreateButtons(){
        foreach (var item in items){
            Debug.Log("Item: " + item.ItemName);
            ItemButtonManager itemButton;
            itemButton = Instantiate(itemButtonManager, buttonContainer.transform);
            itemButton.ItemName = item.ItemName;
            itemButton.ItemImage = item.ItemImage;
            itemButton.ItemNavTargetObject = itemNavTargetObject;
            itemButton.SetPositions = item.positions;
            itemButton.SetCameraOrigin = CameraAr;
            itemButton.name = item.ItemName;
            itemButton.SetDropdwon = dropdown;
            itemButton.SetOptions = item.descripciones;
            //Desubscribimos el evento para que no se genere cada vez que se llama
            GameManager.instance.OnItemsMenu -= CreateButtons;
        }
    }
}

