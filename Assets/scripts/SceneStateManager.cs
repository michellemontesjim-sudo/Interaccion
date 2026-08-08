using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance;

    // lista de ids de objetos recogidos/destruidos
    private HashSet<string> objetosRecogidos = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // poner que un objeto fue tomado o destruido
    public void RegistrarObjetoRecogido(string objectID)
    {
        if (!string.IsNullOrEmpty(objectID) && !objetosRecogidos.Contains(objectID))
        {
            objetosRecogidos.Add(objectID);
        }
    }

    // mirar si un objeto ya fue tomado antes
    public bool EstaRecogido(string objectID)
    {
        return objetosRecogidos.Contains(objectID);
    }
}
