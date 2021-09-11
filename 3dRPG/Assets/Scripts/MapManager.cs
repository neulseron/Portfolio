using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    #region Singletone
    static MapManager instance;
    public static MapManager Instance => instance;
    #endregion Singletone

    [SerializeField]
    NavMeshData warpMeshData;
    [SerializeField]
    NavMeshData gameMeshData;
    [SerializeField]
    NavMeshSurface warpMeshSurface;
    [SerializeField]
    NavMeshSurface gameMeshSurface;

    [SerializeField]
    GameObject warpRoute;


    void Awake() {
        instance = this;
    }

    public void OffMesh()
    {
        //warpMeshSurface.RemoveData();
        //warpMeshSurface.gameObject.SetActive(false);
        warpMeshSurface.enabled = false;
        gameMeshSurface.enabled = true;
        warpRoute.SetActive(false);
    }

    public void OnMesh()
    {
        //warpMeshSurface.gameObject.SetActive(true);
        warpMeshSurface.enabled = true;
        gameMeshSurface.enabled = false;
        warpRoute.SetActive(false);
    }
}
