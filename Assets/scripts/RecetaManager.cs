using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public struct Receta
{
    public string itemA;
    public string itemB;
    public string resultId;     
    public Sprite resultSprite; 

    [Header("Opciones de Consumo")]
    public bool destroyItemA; // ¿El objeto arrastrado desaparece? 
    public bool destroyItemB; // ¿El objeto receptor desaparece? 

    [Header("Recompensa / Mensaje Opcional")]
    [TextArea(2, 4)]
    public string descriptionMessage;      // Texto descripción diálogo encontrar objeto
    public GameObject extraRewardPrefab;    // Prefab del objeto que irá al inventario


}
public class RecetaManager : MonoBehaviour
{
    public static RecetaManager Instance;


    [Header("UI de Texto / Diálogo")]
    public TextMeshProUGUI dialogText;
    public GameObject dialogPanel;

    [Header("Configuración de Diálogo")]
    public bool autoHide = false;        // Marcar para que se cierre solo
    public float displayDuration = 3.5f; // Segundos que dura en pantalla si autoHide es true
    private Coroutine hideCoroutine;

    [Header("Lista de Recetas")]
    public List<Receta> recetas;


    [Header("Referencias de Canvas / UI")]
    public GameObject canvasDialogo;
    private void Awake()
    {
        
        // Patrón Singleton con Persistencia entre Escenas
        if (Instance == null)
        {
            Instance = this;

            // No destruye este gestor
            DontDestroyOnLoad(gameObject);

            // Tampoco destruye el Canvas
            if (canvasDialogo != null)
            {
                DontDestroyOnLoad(canvasDialogo);
            }

            if (dialogPanel != null)
            {
                dialogPanel.SetActive(false);
            }
        }
        else
        {
            // Si regresa a una escena anterior y ya existen, destruye los duplicados
            if (canvasDialogo != null) Destroy(canvasDialogo);
            Destroy(gameObject);
        }
    }

    // Muestra mensaje en pantalla
    public void ShowDialog(string message)
    {
        if (dialogPanel == null || dialogText == null) return;

        // Cancelar el conteo previo si ya había un texto mostrándose
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        dialogText.text = message;
        dialogPanel.SetActive(true);

        // Para ocultarse solo que inicie el conteo
        if (autoHide)
        {
            hideCoroutine = StartCoroutine(HideDialogAfterDelay(displayDuration));
        }
    }

    // Función para Ocultar el Diálogo
    public void CloseDialog()
    {
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
    }

    // espera X segundos para cerrar el panel
    private IEnumerator HideDialogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseDialog();
    }

    public void TryCombine(objetoInteractuable item1, objetoInteractuable item2)
    {
        foreach (Receta recipe in recetas)
        {
            bool isDirectMatch = (item1.itemId == recipe.itemA && item2.itemId == recipe.itemB);
            bool isReverseMatch = (item1.itemId == recipe.itemB && item2.itemId == recipe.itemA);

            if (isDirectMatch || isReverseMatch)
            {
                // para saber cual es el Ítem A y cual el Ítem B de la receta
                // sin importar cuál arrastró el jugador sobre cuál.
                objetoInteractuable objectA = isDirectMatch ? item1 : item2;
                objetoInteractuable objectB = isDirectMatch ? item2 : item1;

                // 1. persistence
                // guarda que ambos objetos originales ya fueron procesados
                PersistentObject pA = objectA.GetComponent<PersistentObject>();
                if (pA != null) pA.MarcarComoRecogido();

                PersistentObject pB = objectB.GetComponent<PersistentObject>();
                if (pB != null) pB.MarcarComoRecogido();

                // 2. transforma
                // actualización de ID y Sprite al objeto que no se vaya a destruir
                if (!string.IsNullOrEmpty(recipe.resultId))
                {
                    objetoInteractuable objetoResultado = null;

                    if (!recipe.destroyItemA)
                    {
                        objectA.UpdateItem(recipe.resultId, recipe.resultSprite);
                        DontDestroyOnLoad(objectA.gameObject);
                        objetoResultado = objectA;
                    }
                    else if (!recipe.destroyItemB)
                    {
                        objectB.UpdateItem(recipe.resultId, recipe.resultSprite);
                        DontDestroyOnLoad(objectB.gameObject);
                        objetoResultado = objectB;
                    }

                    // si el resultado es el objeto para la nueva sala
                    if (objetoResultado != null && barraInventario.Instance != null)
                    {
                        barraInventario.Instance.VerificarYDesbloquearBoton(objetoResultado);
                    }
                }

                // 3. dialogos y recompensas
                if (!string.IsNullOrEmpty(recipe.descriptionMessage))
                {
                    ShowDialog(recipe.descriptionMessage);
                }

                if (recipe.extraRewardPrefab != null)
                {
                    GiveRewardToInventory(recipe.extraRewardPrefab);
                }

                // 4. destruccion segun la receta
                if (recipe.destroyItemA && objectA != null)
                {
                    Destroy(objectA.gameObject);
                }

                if (recipe.destroyItemB && objectB != null)
                {
                    Destroy(objectB.gameObject);
                }

                return;
            }
        }

        
    }

    // Instancia el objeto y lo coloca en el inventario automáticamente
    private void GiveRewardToInventory(GameObject rewardPrefab)
    {
        GameObject newItemObj = Instantiate(rewardPrefab);
        objetoInteractuable newItem = newItemObj.GetComponent<objetoInteractuable>();

        if (newItem != null && barraInventario.Instance != null)
        {
            Vector3 slotPosition = barraInventario.Instance.GetNextFreeSlot(newItem);
            newItem.transform.position = slotPosition;
            DontDestroyOnLoad(newItemObj);
        }
    }
}

