using System.Collections.Generic;
using UnityEngine;

public class EquipmentCombiner
{
    Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();
    private readonly Transform transform;


#region Methods
    public EquipmentCombiner(GameObject rootGO)
    {
        transform = rootGO.transform;
        TranverseHierarchy(transform);
    }

    void TranverseHierarchy(Transform root)
    {
        foreach (Transform child in root) {
            rootBoneDictionary.Add(child.name.GetHashCode(), child);
            TranverseHierarchy(child);
        }
    }

    public Transform AddMesh(GameObject itemGO, ItemType type)
    {
        Transform itemTransforms = ProcessMeshObject(itemGO.GetComponentInChildren<MeshRenderer>(), type);
        return itemTransforms;
    }

    Transform ProcessMeshObject(MeshRenderer meshRenderers, ItemType type)
    {

        Transform parent = rootBoneDictionary[(int)type == 3 ? "hand_l".GetHashCode() : "hand_r".GetHashCode()];

        GameObject itemGO = GameObject.Instantiate(meshRenderers.gameObject, parent);

        return itemGO.transform;
    }
#endregion Methods
}