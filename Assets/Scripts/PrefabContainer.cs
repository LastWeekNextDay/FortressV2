using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabContainer", menuName = "ScriptableObjects/PrefabContainer", order = 1)]
public class PrefabContainer : ScriptableObject
{
    public List<GameObject> prefabs;
    
    public GameObject GetPrefab(string prefabName)
    {
        return prefabs.FirstOrDefault(prefab => prefab.name == prefabName);
    }
}
