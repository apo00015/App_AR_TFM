using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class ItemButtonManager : MonoBehaviour
{
    public static string nameItemSeleccionado;
    public static bool isDireccionConcreta = false;
    private string itemName;
    private Sprite itemImage;
    public List<Vector3> positions;
    private List<string> opciones;
    public Camera cameraOrigin;
    public GameObject itemNavTargetObject;
    private NavMeshPath path;
    private Dropdown dropdownPrefab;
    public string ItemName { set { itemName = value; } }
    public Sprite ItemImage { set => itemImage = value; }
    public List<Vector3> SetPositions { set => positions = value; }
    public Camera SetCameraOrigin { set => cameraOrigin = value; }
    public GameObject ItemNavTargetObject { set => itemNavTargetObject = value; }
    public Dropdown SetDropdwon { set => dropdownPrefab = value; }
    public List<string> SetOptions { set => opciones = value; }

    void Start(){
        path = new NavMeshPath();

        transform.GetChild(0).GetComponent<Text>().text = itemName;
        transform.GetChild(1).GetComponent<RawImage>().texture = itemImage.texture;

        var button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            Debug.Log("Entramos en el listener de la imagen " + itemName);
            isDireccionConcreta = false;
            ChangePositionTarget();
        }
        );
    }

    void Update(){
        if (itemName == nameItemSeleccionado){
            if(!isDireccionConcreta){
                //Debug.Log("Calcukamos el mas cercano");
                getItemCercano();
            }
        }
    }

    public void ChangePositionTarget(int pos = 0, bool redireccion = true, bool is_dropdown = true){
        nameItemSeleccionado = itemName;
        //Debug.Log("La posicion seleccionada es: " + pos + ", el nº de posiciones es: " + positions.Count + " ---- " + "El dropdown es: " + is_dropdown + " el is_concreta es: " + isDireccionConcreta);
        itemNavTargetObject.transform.position = positions[pos];
        // Activamos la línea
        itemNavTargetObject.SetActive(true);

        // Generamos las opciones del dropdwon
        //Debug.Log("El dropdown es: " + is_dropdown + " el is_concreta es: " + isDireccionConcreta);
        if(is_dropdown){
            List<string> listOptions = new List<string>();
            for (int i = 0; i < opciones.Count; i++){
                listOptions.Add(opciones[i]);
                Debug.Log(opciones[i] + " --- " + positions[i]);
            }
            // Limpiar las opciones existentes
            //Debug.Log("Limpiamos el dropdown");
            dropdownPrefab.onValueChanged.RemoveAllListeners();
            dropdownPrefab.ClearOptions();

            // Agregar nuevas opciones
            dropdownPrefab.AddOptions(listOptions);
            
            // Asignar funciones a cada opción
            dropdownPrefab.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdownPrefab);
            });
        }

        // Cerramos el menu de Items
        if (redireccion){
            GameManager.instance.MainMenu();
        }
    }

    public void getItemCercano(){
        if (itemNavTargetObject.activeSelf){
            float distanciaMinima = 100000;
            int posMinimo = 0;
            for (int i = 0; i < positions.Count; i++){
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(cameraOrigin.transform.position, positions[i], NavMesh.AllAreas, path)){
                    float distAux = GetPathLength(path);
                    if (distAux < distanciaMinima){
                        distanciaMinima = distAux;
                        posMinimo = i;
                    }
                    //Debug.Log(positions[i] + " Longitud es de " + distAux + " está en la posicion " + i );
                }
            }
            dropdownPrefab.value = posMinimo;

            // Notifica al Dropdown que ha habido un cambio en su valor
            dropdownPrefab.RefreshShownValue();
            //Debug.Log("Longitud de la ruta más corta al objetivo es de " + distanciaMinima + " está en la posicion " + posMinimo );
            // Cambiamos al objetivo con menos distancia
            ChangePositionTarget(posMinimo, false, false);
        }
    }
    float GetPathLength(NavMeshPath path){
        float pathLength = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return pathLength;
    }

    void DropdownValueChanged(Dropdown dropdown){
        // Habilitamos la variable direccion concreta
        isDireccionConcreta = true;
        // Obtener el índice de la opción seleccionada
        int posSeleccionada = dropdown.value;
        // Debug.Log("La pos seleccionada es: " + posSeleccionada + " *** El nº de posiciones es de: " + opciones.Count + " --- El nº de positions es de: " + positions.Count);
        // Debug.Log("Cambiamos la ruta a la direccion: " + opciones[posSeleccionada]);
        // Debug.Log(positions[0]);
        // Debug.Log(positions[1]);
        // Modificamos la posicion del item he indicamos que se ha seleccionado una direccion concreta
        ChangePositionTarget(posSeleccionada, false, false); 
    }

}
