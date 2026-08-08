using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barraInventario : MonoBehaviour
{
    public static barraInventario Instance;

    [Header("Desbloqueo por Ítem Clave")]
    [Tooltip("ID del objeto que activa botón de cambio de escena")]
    public string itemIdClaveAcertijo = "llave_acertijo";

    [Header("Puntos donde se colocan los objetos")]
    public Transform[] inventarioSlots; // posiciones en la barra
    private objetoInteractuable[] itemsEnSlots;

    private void Awake()
    {
        // Patrón Singleton con persistencia entre escenas
        if (Instance == null)
        {
            Instance = this;
            // evita que el inventario se borre al cambiar de escena
            DontDestroyOnLoad(gameObject);

            // garantiza que los objetos que estén dentro de la barra no se destruyan
            itemsEnSlots = new objetoInteractuable[inventarioSlots.Length];
        }
        else
        {
            Destroy(gameObject); // Evita duplicar el inventario si regresa a la escena inicial
        }
    }

    // Encuentra la primera casilla libre y regresa su posición
    public Vector3 GetNextFreeSlot(objetoInteractuable item)
    {
        for (int i = 0; i < inventarioSlots.Length; i++)
        {
            if (itemsEnSlots[i] == null || itemsEnSlots[i] == item)
            {
                itemsEnSlots[i] = item;

                // comprueba item clave para el secreto
                VerificarYDesbloquearBoton(item);
                return inventarioSlots[i].position;
            }
        }

        // si el inventario está lleno, regresa la posición actual del objeto
        return item.transform.position;
    }

    // libera casilla cuando el objeto sale de la barra
    public void RemoveFromInventory(objetoInteractuable item)
    {
        for (int i = 0; i < itemsEnSlots.Length; i++)
        {
            if (itemsEnSlots[i] == item)
            {
                itemsEnSlots[i] = null;
                break;
            }
        }
    }

    public void VerificarYDesbloquearBoton(objetoInteractuable item)
    {
        // directamente le pide al botón verificar su ítem clave
        
        BotonCambiarEscena[] botones = FindObjectsOfType<BotonCambiarEscena>(true);

        foreach (BotonCambiarEscena boton in botones)
        {
            if (boton != null && item != null && item.itemId == boton.itemIdClave)
            {
                boton.MostrarBoton();
            }
        }
    }

    //para el boton misterioso
    public bool TieneItem(string idBuscado)
    {
        if (itemsEnSlots == null) return false;

        for (int i = 0; i < itemsEnSlots.Length; i++)
        {
            if (itemsEnSlots[i] != null && itemsEnSlots[i].itemId == idBuscado)
            {
                return true; // encontro item en la barra
            }
        }

        return false; // item no esta en el inventario
    }
}
