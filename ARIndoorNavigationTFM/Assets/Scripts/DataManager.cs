using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private ItemButtonManager itemButtonManager;
    [SerializeField] private GameObject itemNavTargetObject;
    [SerializeField] private Camera CameraAr;
    [SerializeField] private Dropdown dropdown;

    private const string URL = "http://18.201.72.47:5000/service/getDocuments/";
    
    void Start()
    {
        GameManager.instance.OnItemsMenu += CreateButtons;
    }

    private void CreateButtons(){
        // Realizamos la llamada a la API para obtener los elementos
        var url = URL + ControllerSelectBuiding.instance.edificio_seleccionado;
        
        StartCoroutine(GetRequest(url));
        
    }

    IEnumerator GetRequest(string uri)
    {
        Debug.Log("Realizamos la llamada GET");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Establecer el timeout a 10 segundos
            webRequest.timeout = 10;

            // Enviar solicitud y esperar la respuesta
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                Debug.Log("Error al intentar conectar con la API, se cargarna los datos por defecto" );
        
        
                // En caso de que falle la API se introducen los elementos predeterminados
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
            else
            {
                // Obtener la respuesta como texto
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);
                    
                // Deserializar la respuesta JSON en un diccionario
                var elementosList = JsonConvert.DeserializeObject<List<Dictionary<string, Elemento>>>(jsonResponse);

                // Procesar el diccionario
                foreach (var dict in elementosList)
                {
                    foreach (var keyValuePair in dict)
                    {
                        Debug.Log($"Key: {keyValuePair.Key}");
                        var elemento = keyValuePair.Value;
                        Debug.Log($"id_edificio: {elemento.id_edificio}");
                        Debug.Log($"id_elemento: {elemento.id_elemento}");
                        Debug.Log($"nombre_edificio: {elemento.nombre_edificio}");
                        Debug.Log($"nombre_elemento: {elemento.nombre_elemento}");
                        Debug.Log("Posiciones y descripciones:");
                        for (int i = 0; i < elemento.posiciones.Count; i++)
                        {
                            Debug.Log(elemento.descripciones[i] + " --- " + elemento.posiciones[i]);
                        }

                        // Creamos un nuevo item
                        Item itemNuevo = ScriptableObject.CreateInstance<Item>();
                        itemNuevo.ItemName = elemento.nombre_elemento;
                        // Asignamos la imagen en función del id de elemento
                        switch (elemento.id_elemento){
                            case "1":
                                itemNuevo.ItemImage = Resources.Load<Sprite>("Textures/Salida");
                                break;
                            case "2":
                                itemNuevo.ItemImage = Resources.Load<Sprite>("Textures/icon_alarma");
                                break;
                            case "3":
                                itemNuevo.ItemImage = Resources.Load<Sprite>("Textures/extintor");
                                break;
                        }
                         
                        itemNuevo.descripciones = elemento.descripciones;
                        List<Vector3> positions = new List<Vector3>();
                        for (int i = 0; i < elemento.posiciones.Count; i++)
                        {
                            Vector3 vector_new = StringToVector3(elemento.posiciones[i]);
                            positions.Add(vector_new);
                        }
                        itemNuevo.positions = positions;

                        // Creación del botón
                        Debug.Log("Item: " + itemNuevo.ItemName);
                        ItemButtonManager itemButton;
                        itemButton = Instantiate(itemButtonManager, buttonContainer.transform);
                        
                        itemButton.ItemName = itemNuevo.ItemName;
                        itemButton.ItemImage = itemNuevo.ItemImage;
                        itemButton.ItemNavTargetObject = itemNavTargetObject;
                        itemButton.SetPositions = itemNuevo.positions;
                        itemButton.SetCameraOrigin = CameraAr;
                        itemButton.name = itemNuevo.ItemName;
                        itemButton.SetDropdwon = dropdown;
                        itemButton.SetOptions = itemNuevo.descripciones;
                        //Desubscribimos el evento para que no se genere cada vez que se llama
                        GameManager.instance.OnItemsMenu -= CreateButtons;
                    }
                }
            }
        }
    }

    Vector3 StringToVector3(string texto)
    {
        // Separar el string usando ',' como delimitador
        string[] values = texto.Split(',');

        Debug.Log("El string de values en Datamanager es: " + values[0] + "," + values[1] + "," + values[2]);
        // Validamos que el vector tiene 3 elementos
        if (values.Length == 3)
        {
            float x = float.Parse(values[0], System.Globalization.CultureInfo.InvariantCulture);
            float y = float.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
            float z = float.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture);

            Debug.Log("El Vector final es: " + x + "," + y + "," + z);
            return new Vector3(x, y, z);
        }
        else
        {
            Debug.LogError("Error en la creación del vector de posiciones para el string " + texto);
            return Vector3.zero; 
        }
    }
}

