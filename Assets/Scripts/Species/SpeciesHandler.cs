using System.Collections.Generic;
using UnityEngine;

namespace Species
{
    public class SpeciesHandler : MonoBehaviour
    {
        public GameManager gameManager;
        public PrefabContainer prefabContainer;
    
        private List<GameObject> _speciesGameObjects;
    
        private void Awake()
        {
            _speciesGameObjects = new List<GameObject>();
        }

        public void SpawnSpecies(Species species, Vector3 position, bool playerControlled)
        {
            var speciesGameObject = Instantiate(prefabContainer.GetPrefab(species.SpeciesName), position, Quaternion.identity);
            speciesGameObject.GetComponent<SpeciesGameObject>().Species = species;
            speciesGameObject.GetComponent<SpeciesGameObject>().PlayerControlled = playerControlled;
            _speciesGameObjects.Add(speciesGameObject);
            var args = new EventArgs
            {
                Object = speciesGameObject,
            };
            gameManager.eventManager.InvokeSpawnSpeciesEvent(args);
        }

        public void SpawnSpecies(string speciesName, Vector3 position, bool playerController)
        {
            var speciesGameObject = Instantiate(prefabContainer.GetPrefab(speciesName), position, Quaternion.identity);
            speciesGameObject.GetComponent<SpeciesGameObject>().PlayerControlled = playerController;
            _speciesGameObjects.Add(speciesGameObject);
            var args = new EventArgs
            {
                Object = speciesGameObject,
            };
            gameManager.eventManager.InvokeSpawnSpeciesEvent(args);
        }
    
        public void RemoveSpecies(SpeciesGameObject speciesGameObject)
        {
            if (!_speciesGameObjects.Contains(speciesGameObject.gameObject)) return;
            _speciesGameObjects.Remove(speciesGameObject.gameObject);
            var args = new EventArgs
            {
                Object = speciesGameObject.gameObject,
            };
            gameManager.eventManager.InvokeRemoveSpeciesEvent(args);
            Destroy(speciesGameObject.gameObject);
        }
    }
}
