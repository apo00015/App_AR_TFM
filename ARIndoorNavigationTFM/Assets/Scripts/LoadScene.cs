using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenuItems : MonoBehaviour
{
    public static bool is_open_panelQR = false;
    public void AbrirEscenaBluetooth()
    {
        // Carga la escena con el nombre "BLETestScene"
        SceneManager.LoadScene("BluetoothScene");
    }

    public void AbrirEscenaQR()
    {
        // Carga la escena con el nombre "BLETestScene"
        SceneManager.LoadScene("QRScanScene");
    }

    public void AbrirEscenaMain()
    {
        // Carga la escena con el nombre "BLETestScene"
        SceneManager.LoadScene("Main");
    }

}
