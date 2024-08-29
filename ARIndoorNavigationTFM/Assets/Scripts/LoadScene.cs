using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenuItems : MonoBehaviour
{
    public void AbrirEscena(string escena){
        SceneManager.LoadScene(escena);
    }

}
