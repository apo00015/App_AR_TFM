using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField] private Camera CameraAR;

    [SerializeField] private GameObject navTargetObject;
    [SerializeField] private GameObject XROrigin;

    [SerializeField] private GameObject spehereOrigin;
    [SerializeField] private GameObject edificio;

    private NavMeshPath path;
    private LineRenderer line;

    private bool startTracking = false;
    private bool xr_origin_init = false;
    private Quaternion rotacionInicial;
    private float rotacionActualY;
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();

        // Deshabilitamos el primera instancia el objetivo
        navTargetObject.SetActive(false);

        Input.compass.enabled = true;
        Input.location.Start();
        StartCoroutine(InitializeCompass());

        // Guardar la rotación inicial
        rotacionInicial = spehereOrigin.transform.rotation;
        rotacionActualY = 0f; // Inicializar la rotación actual en 0 grados
    }

    // Update is called once per frame
    void Update()
    {
        // Comprobamos si el usuario está en la planta 1
        if (getHightPlayer()){
            GameObject firstChild = edificio.transform.GetChild(0).gameObject;
            firstChild.SetActive(true);
        }else{
            GameObject firstChild = edificio.transform.GetChild(0).gameObject;
            firstChild.SetActive(false);
        }

        // Actualizamos la posicion del player con la misma posicion que la camara
        Vector3 postionMapa = CameraAR.transform.position;
        postionMapa.y = spehereOrigin.transform.position.y;

        spehereOrigin.transform.position = postionMapa;

        // Comprobamos si la brujula está habilitada
        if (startTracking){
            // Modificamos la direccion de la cámara
            if (CameraAR != null && CameraAR.enabled){
                // Comprueba si ha pasado el intervalo deseado (5 segundos en este caso)
                int gradosCompass = (int)Input.compass.trueHeading;

                // Calcular la diferencia incremental
                float diferenciaRotacion = gradosCompass - rotacionActualY;

                if (!xr_origin_init){
                        XROrigin.transform.rotation = rotacionInicial * Quaternion.Euler(0f, 180 + rotacionActualY, 0f);
                        xr_origin_init = true;
                }

                if (Mathf.Abs(diferenciaRotacion) > 10f){
                    // Aplicar la diferencia incremental a la rotación actual
                    rotacionActualY += diferenciaRotacion;

                    // Asegurarse de que la rotación esté dentro del rango válido (0-360 grados)
                    rotacionActualY = Mathf.Repeat(rotacionActualY, 360f);

                    // Aplicar la rotación al objeto spehereOrigin
                    spehereOrigin.transform.rotation = rotacionInicial * Quaternion.Euler(0f, 45 + rotacionActualY, 0f);

                }
            }
        }

        // Comprobamos si debemos calcular la ruta hacia un objetivo válido
        if (navTargetObject.activeSelf){
            NavMesh.CalculatePath(spehereOrigin.transform.position, navTargetObject.transform.position, NavMesh.AllAreas, path);
            // Verifica si hay al menos 15 puntos en el array path.corners
            int count = Mathf.Min(4, path.corners.Length);

            // Crea un array temporal para almacenar los primeros 15 puntos (o menos si no hay 15)
            Vector3[] firstCorners = new Vector3[count];

            // Copia los primeros 4 puntos (o los que haya) en el array temporal
            for (int i = 0; i < count; i++){
                firstCorners[i] = path.corners[i];
            }
            // Asigna los puntos al LineRenderer
            line.positionCount = count;
            line.SetPositions(firstCorners);

            // Activa el LineRenderer
            line.enabled = true;
        }else{
            line.enabled = false;
        }

    }

    IEnumerator InitializeCompass(){
        yield return new WaitForSeconds(1f);
        startTracking |= Input.compass.enabled;
    }

    private static string DegreesToCardinalDetailed(double degrees){
        string[] caridnals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
        return caridnals[(int)Mathf.Round(((float)degrees * 10 % 3600) / 225)];
    }

    private bool getHightPlayer(){
        if (spehereOrigin.transform.position.y > 0){
            // Debug.Log("La posición Y es mayor que 0: " + spehereOrigin);
            return true;
        }

        return false;
    }

}
