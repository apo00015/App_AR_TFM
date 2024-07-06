using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenuItems : MonoBehaviour
{
    public static bool is_open_panelQR = false;
    public void AbrirEscenaBluetooth(){
        SceneManager.LoadScene("BluetoothScene");
    }

    public void AbrirEscenaQR(){
        SceneManager.LoadScene("QRScanScene");
    }

    public void AbrirEscenaMain(){
        SceneManager.LoadScene("Main");
    }

}
