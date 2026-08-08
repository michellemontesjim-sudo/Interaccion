using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObjetoInspeccionable : MonoBehaviour
{
    [Header("Identificador Único")]
    [Tooltip("ID único para este contenedor/grieta)")]
    public string uniqueID;

    [Header("Mensaje de Diálogo")]
    [TextArea(2, 4)]
    public string mensajeInspeccion = "No parece haber nada interesante aquí...";

    [Header("Recompensa (Opcional)")]
    public GameObject prefabRecompensa; // objeto que irá al inventario
    public bool darRecompensaUnaSolaVez = true;

    [Header("Mensaje tras recoger la recompensa")]
    [TextArea(2, 4)]
    public string mensajeYaInspeccionado = "La grieta ya está vacía.";

    private bool yaFueRecompensado = false;

    private void Start()
    {
        // al cargar o regresar a la escena consulta si este objeto ya dio recompensa
        if (!string.IsNullOrEmpty(uniqueID) && SceneStateManager.Instance != null)
        {
            if (SceneStateManager.Instance.EstaRecogido(uniqueID))
            {
                yaFueRecompensado = true;
            }
        }
    }

    private void OnMouseDown()
    {
        if (RecetaManager.Instance == null) return;

        // ya entrego la recompensa anteriormente
        if (yaFueRecompensado)
        {
            if (!string.IsNullOrEmpty(mensajeYaInspeccionado))
            {
                RecetaManager.Instance.ShowDialog(mensajeYaInspeccionado);
            }
            return;
        }

        // mostrar texto principal de inspeccion
        if (!string.IsNullOrEmpty(mensajeInspeccion))
        {
            RecetaManager.Instance.ShowDialog(mensajeInspeccion);
        }

        // si tiene una recompensa para entregar
        if (prefabRecompensa != null)
        {
            EntregarRecompensa();

            if (darRecompensaUnaSolaVez)
            {
                yaFueRecompensado = true;

                // poner en el SceneStateManager que esta grieta ya dio recompensa
                if (!string.IsNullOrEmpty(uniqueID) && SceneStateManager.Instance != null)
                {
                    SceneStateManager.Instance.RegistrarObjetoRecogido(uniqueID);
                }
            }
        }
    }

    private void EntregarRecompensa()
    {
        GameObject nuevoObjetoObj = Instantiate(prefabRecompensa);
        objetoInteractuable nuevoObjeto = nuevoObjetoObj.GetComponent<objetoInteractuable>();

        if (nuevoObjeto != null && barraInventario.Instance != null)
        {
            Vector3 slotPosition = barraInventario.Instance.GetNextFreeSlot(nuevoObjeto);
            nuevoObjeto.transform.position = slotPosition;
            DontDestroyOnLoad(nuevoObjetoObj);
        }
    }
}
