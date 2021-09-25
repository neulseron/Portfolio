using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
#region Singletone
    static MapManager instance;
    public static MapManager Instance => instance;
#endregion Singletone


#region Variables
    [SerializeField]
    NavMeshSurface warpMeshSurface;
    [SerializeField]
    NavMeshSurface gameMeshSurface;

    [SerializeField]
    GameObject warpRoute;
#endregion Variables


    void Awake() {
        instance = this;
    }

    public void OffMesh()
    {
        warpMeshSurface.enabled = false;
        gameMeshSurface.enabled = true;
        warpRoute.SetActive(false);
    }

    public void OnMesh()
    {
        warpMeshSurface.enabled = true;
        gameMeshSurface.enabled = false;
        warpRoute.SetActive(false);
    }
}
