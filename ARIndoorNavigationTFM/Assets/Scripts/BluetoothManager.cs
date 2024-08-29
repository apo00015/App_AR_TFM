using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothManager : MonoBehaviour
{
    public Text deviceAdd;
    public Text dataToSend;
    public Text receivedData;
    public GameObject devicesListContainer;
    public GameObject deviceMACText;
    private bool isConnected;

    private static AndroidJavaClass unity3dbluetoothplugin;
    private static AndroidJavaObject BluetoothConnector;

    /// <summary>
    /// Método para inicialziar el controlador de la escenade de Bluetooth
    /// </summary>
    void Start()
    {
        InitBluetooth();
        isConnected = false;
    }

    /// <summary>
    /// Método para crear una instancia de bluetooth y acceder al bluetooth del dispositivo
    /// </summary>
    public void InitBluetooth()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // Validamos que tenemos los permisos necesarios para conectarnos al bluetooth
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)
            || !Permission.HasUserAuthorizedPermission(Permission.FineLocation)
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADMIN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {

            Permission.RequestUserPermissions(new string[] {
                        Permission.CoarseLocation,
                            Permission.FineLocation,
                            "android.permission.BLUETOOTH_ADMIN",
                            "android.permission.BLUETOOTH",
                            "android.permission.BLUETOOTH_SCAN",
                            "android.permission.BLUETOOTH_ADVERTISE",
                             "android.permission.BLUETOOTH_CONNECT"
                    });

        }

        unity3dbluetoothplugin = new AndroidJavaClass("com.example.unity3dbluetoothplugin.BluetoothConnector");
        BluetoothConnector = unity3dbluetoothplugin.CallStatic<AndroidJavaObject>("getInstance");
    }

    /// <summary>
    /// Método para escanear los dispotivos bluetooth que existen en nuestra area
    /// </summary>
    public void StartScanDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        foreach (Transform child in devicesListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        BluetoothConnector.CallStatic("StartScanDevices");
    }
    
    /// <summary>
    /// Método para parar el escaneo de dispositivos bluetooth
    /// </summary>
    public void StopScanDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("StopScanDevices");
    }

    /// <summary>
    /// Método que llamará la clase Java par ainformar que se ha actualizado el estado del escaneo de dispositivos
    /// </summary>
    public void ScanStatus(string status)
    {
        Toast("Scan Status: " + status);
    }

    /// <summary>
    /// Método que llamará la clase Java cada vez que un nuevo dispositivo ha sido encontrado
    /// </summary>
    public void NewDeviceFound(string data)
    {
        GameObject newDevice = deviceMACText;
        newDevice.GetComponent<Text>().text = data;
        Instantiate(newDevice, devicesListContainer.transform);  
    }

    /// <summary>
    /// Método para obtener la información de los dispositivos bluetooth que existen en un area
    /// </summary>
    public void GetPairedDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // This function when called returns an array of PairedDevices as "MAC+Name" for each device found
        string[] data = BluetoothConnector.CallStatic<string[]>("GetPairedDevices"); ;

        // Destroy devicesListContainer child objects for new Paired Devices display
        foreach (Transform child in devicesListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Display the paired devices
        foreach (var d in data)
        {
            GameObject newDevice = deviceMACText;
            newDevice.GetComponent<Text>().text = d;
            Instantiate(newDevice, devicesListContainer.transform);
        }
    }

    /// <summary>
    /// Método para realizar la conexión con un dispositivo en función de la MAC introducida
    /// </summary>
    public void StartConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("StartConnection", deviceAdd.text.ToString().ToUpper());
    }

    /// <summary>
    /// Método para detener la conexión con el dispositvo BLE
    /// </summary>
    public void StopConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
            BluetoothConnector.CallStatic("StopConnection");
    }

    /// <summary>
    /// Método que llamará la clase de Java para validar el estado de conexión con el dispositivo conectado
    /// </summary>
    public void ConnectionStatus(string status)
    {
        Toast("Connection Status: " + status);
        isConnected = status == "connected";
    }

    /// <summary>
    /// Método que llamaará la clase Java en caso de recibir datos del dispositivo con el que se está conectado
    /// </summary>
    public void ReadData(string data)
    {
        Debug.Log("BT Stream: " + data);
        receivedData.text = data;
    }

    /// <summary>
    /// Método para enviar datos al dispositivo Bluetooth conectado
    /// </summary>
    public void WriteData()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
            BluetoothConnector.CallStatic("WriteData", dataToSend.text.ToString());
    }

    /// <summary>
    /// Método que será llamado por la clase Java para enviar los mensajes de logs
    /// </summary>
    public void ReadLog(string data)
    {
        Debug.Log(data);
    }


    /// <summary>
    /// Método para mostrar un toast en la escena de bluetooth
    /// </summary>
    public void Toast(string data)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("Toast", data);
    }
}

