using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MiniMapController : MonoBehaviour
{
    [SerializeField] private GameObject UICanvas;
    public RawImage minimap;
    public RectTransform fullScreenRectTransform;
    [SerializeField] private Camera CameraMapa;
    public static bool isFullScreen = false;
    private Vector2 sizeDeltaOriginal;
    private Vector2 anchoredPositionOriginal;
    public static bool isOn = true;
    private Vector3 positionOriginCamera;
    void Start()
    {
        sizeDeltaOriginal = minimap.rectTransform.sizeDelta;
        anchoredPositionOriginal = minimap.rectTransform.anchoredPosition;
        positionOriginCamera = CameraMapa.transform.position;
    }

    void Update()
    {
        if (!isFullScreen)
        {
            // Detectar si se ha tocado el minimapa
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPosition = Input.mousePosition;

                if (RectTransformUtility.RectangleContainsScreenPoint(minimap.rectTransform, touchPosition))
                {
                    // Cambiar el tamaño del minimapa al tocarlo
                    isFullScreen = true;

                    ExpandMinimap();
                    // Mostramos el boton de salir de la pantalla
                    UICanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.5f);

                }
            }
        }
    }

    public void ShowMiniMap(){
        if(!isOn){
            Debug.Log(" ---- Habilitamos el minimapa");
            ShrinkMinimap();
            minimap.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            isOn = true;
        }else{
            Debug.Log(" ---- Ocultamos el minimapa");
            ShrinkMinimap();
            minimap.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
            isOn = false;
        }
        
    }

    void ExpandMinimap()
    {
        minimap.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calcular la posición central
        Vector2 centerPosition = new Vector2(-screenWidth / 2f, -screenHeight / 2f);
        minimap.rectTransform.anchoredPosition = centerPosition;

        // Aumentamos la distancia del mapa
        CameraMapa.orthographicSize = 1.5f;

        // Habilitamos el script de desplazamiento por el mapa
        CameraMapa.GetComponent<DigitalRubyShared.FingersCameraMove3DComponentScript>().enabled = true;

        // Alamacenamos la posicion de origen de la cámara del mapa
        positionOriginCamera = CameraMapa.transform.position;

    }

    public void ShrinkMinimap()
    {
        minimap.rectTransform.sizeDelta = sizeDeltaOriginal;
        minimap.rectTransform.anchoredPosition = anchoredPositionOriginal;

        // Ocultamos el boton de la X
        UICanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.5f);

        isFullScreen = false;

        // Volvemos al tamaño original de la camara
        CameraMapa.orthographicSize = 1.5f;

        // Deshabilitamos el script de desplazamiento por el mapa
        CameraMapa.GetComponent<DigitalRubyShared.FingersCameraMove3DComponentScript>().enabled = false;

        // Volvemos a la posición origen de la cámara
        CameraMapa.transform.position = positionOriginCamera;
    }

}