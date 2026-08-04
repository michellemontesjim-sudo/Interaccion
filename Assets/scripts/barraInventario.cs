using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barraInventario : MonoBehaviour
{
    public static barraInventario Instance;

    [Header("Puntos donde se colocarán los objetos")]
    public Transform[] inventarioSlots; // Posiciones fijas en la barra
    private objetoInteractuable[] itemsEnSlots;

    private void Awake()
    {
        // Patrón Singleton con Persistencia entre Escenas
        if (Instance == null)
        {
            Instance = this;
            // ¡ESTA LÍNEA ES LA CLAVE! Evita que el inventario se borre al cambiar de escena
            DontDestroyOnLoad(gameObject);

            // Garantiza que los objetos que estén dentro de la barra tampoco se destruyan
            itemsEnSlots = new objetoInteractuable[inventarioSlots.Length];
        }
        else
        {
            Destroy(gameObject); // Evita duplicar el inventario si regresas a la escena inicial
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
                return inventarioSlots[i].position;
            }
        }

        // Si el inventario está lleno, regresa la posición actual del objeto
        return item.transform.position;
    }

    // Libera la casilla cuando el objeto sale de la barra
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
}
