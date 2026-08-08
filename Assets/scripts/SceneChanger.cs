using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public static SceneChanger Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // evita que scenemanager se destruya
        }
        else
        {
            Destroy(gameObject); // destruye duplicados
        }
    }
    // cambia a la escena
    public void CambiarAEscena(string nombreEscena)
    {
        Debug.Log("Intentando cargar la escena: " + nombreEscena);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscena);
        
    }
}
