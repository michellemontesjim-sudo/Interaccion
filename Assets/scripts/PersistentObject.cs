using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    [Header("ID Único del Objeto en el Escenario")]
    [Tooltip("identificador único para este objeto específico en la escena")]
    public string uniqueID;

    private bool fueRecogido = false;

    private void Start()
    {
        // si ya fue marcado recogido /inventario no se destruye
        if (fueRecogido) return;

        // al cargar escena, verifica si objeto ya fue recogido antes en el mundo
        if (SceneStateManager.Instance != null && SceneStateManager.Instance.EstaRecogido(uniqueID))
        {
            // destruye la copia del escenario que vuelve a aparecer al cargar la escena
            Destroy(gameObject);
        }
    }

    // momento exacto en que el jugador recoge o guarda el objeto
    public void MarcarComoRecogido()
    {
        fueRecogido = true; // marca que este clon especifico es el del inventario

        if (SceneStateManager.Instance != null)
        {
            SceneStateManager.Instance.RegistrarObjetoRecogido(uniqueID);
        }
    }
}