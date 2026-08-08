using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonCambiarEscena : MonoBehaviour
{
    [Header("Configuración de la Escena")]
    [Tooltip("Nombre EXACTO de la escena de acertijo a la que se cambiará.")]
    public string nombreEscenaAcertijo;

    [Header("Persistencia del Botón")]
    [Tooltip("ID único para guardar el estado del botón en SceneStateManager")]
    public string uniqueIDDesbloqueo;

    [Header("Pieza de Acertijo Requerida")]
    [Tooltip("ID del objeto en el inventario que activa este botón")]
    public string itemIdClave = "llave_acertijo";

    private void Start()
    {
        // mirar si ya fue guardado en la persistencia de escena
        if (!string.IsNullOrEmpty(uniqueIDDesbloqueo) && SceneStateManager.Instance != null)
        {
            if (SceneStateManager.Instance.EstaRecogido(uniqueIDDesbloqueo))
            {
                MostrarBoton();
                return;
            }
        }

        // mirar si ya tiene el corazonColgante en el inventario al cargar escena
        if (barraInventario.Instance != null && barraInventario.Instance.TieneItem(itemIdClave))
        {
            MostrarBoton();
            return;
        }

        // sino que este oculto
        gameObject.SetActive(false);
    }

    // mira slots del inventario a ver si el objeto esta
    public bool TieneElObjetoEnInventario()
    {
        if (barraInventario.Instance != null)
        {
            return barraInventario.Instance.TieneItem(itemIdClave);
        }
        return false;
    }

    // metodo para hacer visible el boton
    public void MostrarBoton()
    {
        gameObject.SetActive(true);

        // Guardar persistencia
        if (!string.IsNullOrEmpty(uniqueIDDesbloqueo) && SceneStateManager.Instance != null)
        {
            SceneStateManager.Instance.RegistrarObjetoRecogido(uniqueIDDesbloqueo);
        }
    }

    // para poner el evento click al boton
    public void IrAEscenaAcertijo()
    {
        if (!string.IsNullOrEmpty(nombreEscenaAcertijo))
        {
            SceneManager.LoadScene(nombreEscenaAcertijo);
        }
        else
        {
            Debug.LogWarning("No se asignó el nombre de la escena en el Inspector.");
        }
    }
}