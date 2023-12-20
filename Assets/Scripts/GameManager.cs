using System;
using System.Collections.Generic;
using Species;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public PrefabContainer prefabContainer;
    
    public EventManager eventManager;
    public MapManager mapManager;
    public SelectionManager selectionManager;
    public DrawManager drawManager;
    public SpeciesHandler speciesHandler;
    
    private void Start()
    {
        mapManager.BuildMap();
        while (mapManager.IsMapBuilt == false)
        {
            // Wait for map to be built    
        }
        speciesHandler.SpawnSpecies(new HumanSpecies(Color.red), new Vector3(0, 0, 0), true);
        speciesHandler.SpawnSpecies(new HumanSpecies(Color.blue), new Vector3(6, 0, 0), true);
        speciesHandler.SpawnSpecies(new HumanSpecies(Color.green), new Vector3(0, 6, 0), true);
    }
}
