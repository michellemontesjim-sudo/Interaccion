using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimpiadorEscenaFinal : MonoBehaviour
{
    private void Start()
    {
        // ocultar barra de inventario si hay singleton
        if (barraInventario.Instance != null)
        {
            barraInventario.Instance.gameObject.SetActive(false);
        }

        // destruir todos los objetos interactuables persistentes que se trajeron al inventario
        objetoInteractuable[] objetosEnEscena = FindObjectsOfType<objetoInteractuable>();
        foreach (objetoInteractuable obj in objetosEnEscena)
        {
            Destroy(obj.gameObject);
        }

        // busca los Canvas de la escena
        Canvas[] todosLosCanvas = FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in todosLosCanvas)
        {
            // oculta canva que no pertenezca a la escenafinalactual
            if (canvas.gameObject.scene.name == "DontDestroyOnLoad")
            {
                canvas.gameObject.SetActive(false);
            }
        }


    }
}
