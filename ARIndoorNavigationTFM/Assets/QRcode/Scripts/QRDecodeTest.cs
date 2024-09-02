using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TBEasyWebCam;
using EasyUI.Toast ;

public class QRDecodeTest : MonoBehaviour
{
	public QRCodeDecodeController e_qrController;

	public GameObject scanLineObj;

	public void qrScanFinished(string dataText)
	{
        Debug.Log(dataText);
		if (this.scanLineObj != null)
		{
			// Decodificamos el QR
			try{
				string[] valoresData = dataText.Split(',');
				// Parse the values and create the vectors
				Vector3 positionPlayer = new Vector3(
					float.Parse(valoresData[0], System.Globalization.CultureInfo.InvariantCulture),
					float.Parse(valoresData[1], System.Globalization.CultureInfo.InvariantCulture),
					float.Parse(valoresData[2], System.Globalization.CultureInfo.InvariantCulture)
				);
		
				this.scanLineObj.SetActive(false);

				// Asignamos las variables a la clase Singleton
				ControllerPlayer.instance.positionPlayer = positionPlayer;
				string escenaRA = "";
				switch (ControllerSelectBuiding.instance.edificio_seleccionado){
					case 1:
						escenaRA = "Main";
						break;
					case 2:
						escenaRA = "EscenaMuestra";
						break;
				}
				Stop();
				// Redirigimos al usuario en función del edificio seleccionado

				// Volvemos a la escena main
				SceneManager.LoadScene(escenaRA);
			}
			catch (Exception ex){
				showToast("No se reconoce el formato del QR");
				Reset();
			}
		}
	}

	public void Reset()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.Reset();
		}

		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(true);
		}
	}

	public void Play()
	{
		Reset ();
		if (this.e_qrController != null)
		{
			this.e_qrController.StartWork();
		}
	}

	public void Stop()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.StopWork();
		}

		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(false);
		}
	}

	public void GotoNextScene(string scenename)
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.StopWork();
		}
		//Application.LoadLevel(scenename);
		SceneManager.LoadScene(scenename);
	}

	public void showToast(string message){
		Toast.Show(message, 3f, ToastColor.Red) ;
	}
    

}
