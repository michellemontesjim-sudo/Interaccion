using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorEscenaFinal : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("El componente TextMeshProUGUI donde se escribirá la reflexión.")]
    public TextMeshProUGUI textoReflexionUI;

    [Header("Configuración del Mensaje")]
    [TextArea(4, 8)]
    public string mensajeReflexion = "mensaje de reflexión final sobre tu juego...";

    [Header("Efecto de Escritura")]
    [Tooltip("Si está marcado, el texto aparecerá letra por letra.")]
    public bool usarEfectoEscritura = true;
    public float velocidadEscritura = 0.04f;

    [Header("Opciones Finales (Opcional)")]
    public GameObject botonReiniciar;

    private void Start()
    {
        if (botonReiniciar != null)
            botonReiniciar.SetActive(false);

        if (textoReflexionUI != null)
        {
            if (usarEfectoEscritura)
            {
                textoReflexionUI.text = "";
                StartCoroutine(EscribirTextoLetraPorLetra());
            }
            else
            {
                textoReflexionUI.text = mensajeReflexion;
                if (botonReiniciar != null) botonReiniciar.SetActive(true);
            }
        }
    }

    private IEnumerator EscribirTextoLetraPorLetra()
    {
        foreach (char letra in mensajeReflexion.ToCharArray())
        {
            textoReflexionUI.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        // por si se pone algo para volver
        if (botonReiniciar != null)
        {
            botonReiniciar.SetActive(true);
        }
    }

   
    public void ReiniciarJuego()
    {
        
        SceneManager.LoadScene(0);
    }
}