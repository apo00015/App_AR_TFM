using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManagerQR : MonoBehaviour
{

    public GameObject qrDecodeGameObject;

    void Start()
    {
        if (qrDecodeGameObject != null)
        {
            QRDecodeTest qrDecodeTest = qrDecodeGameObject.GetComponent<QRDecodeTest>();
            if (qrDecodeTest != null)
            {
                qrDecodeTest.Play(); // Llamar al método Play del script QRDecodeTest
            }
            else
            {
                Debug.LogError("No se encontró ningún componente QRDecodeTest en el GameObject especificado.");
            }
        }
        else
        {
            Debug.LogError("qrDecodeGameObject no está asignado en el inspector.");
        }
    }
}