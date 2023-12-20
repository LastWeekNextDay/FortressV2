using System;
using System.Collections.Generic;
using System.Linq;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MapManager : MonoBehaviour
{
    public GameManager gameManager;
    
    private MapNavMesh _mapNavMesh;
    private List<GameObject> _cellGameObjects;
    
    [DoNotSerialize] 
    public bool IsMapBuilt = false;
    private void Awake()
    {
        IsMapBuilt = false;
        _cellGameObjects = new List<GameObject>();
        var navMesh = GameObject.Find("NavMesh");
        _mapNavMesh = new MapNavMesh(navMesh.GetComponent<NavMeshSurface>(), 
            navMesh.GetComponent<CollectSources2d>());
    }

    private void Update()
    {
        if (IsMapBuilt)
        {
            _mapNavMesh?.UpdateNavMesh();    
        }
    }

    public void BuildMap()
    {
        var mapParent = GameObject.Find("Map");
        var cellsParent = mapParent.transform.Find("Cells");
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                var cellGameObject = new GameObject("Cell")
                {
                    transform =
                    {
                        position = new Vector3(j, i, 0f),
                        localScale = new Vector3(100, 100, 1)
                    }
                };
                cellGameObject.AddComponent<SpriteRenderer>();
                cellGameObject.GetComponent<SpriteRenderer>().sprite = 
                    Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
                var cellData = new CellData();
                IGroundInfo groundInfo;
                if (i % 3 == 0 || j % 3 == 0)
                    groundInfo = new MudGroundInfo();
                else
                    groundInfo = new GrassGroundInfo();
                cellData.GroundInfo = groundInfo;
                cellGameObject.AddComponent<CellGameObject>();
                cellGameObject.GetComponent<CellGameObject>().CellData = cellData;
                cellGameObject.AddComponent<BoxCollider2D>();
                cellGameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                cellGameObject.AddComponent<Rigidbody2D>();
                cellGameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                cellGameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                cellGameObject.AddComponent<NavMeshModifier>();
                cellGameObject.transform.parent = cellsParent.transform;
                cellGameObject.layer = LayerMask.NameToLayer($"Floor");
                cellGameObject.tag = "Floor";
                _cellGameObjects.Add(cellGameObject);
                gameManager.eventManager.InvokeCellCreationEvent(new EventArgs {Object = cellGameObject});
            }
        }
        _mapNavMesh.BuildNavMesh();
        IsMapBuilt = true;
    }

    public GameObject GetCell(Vector3 coords)
    {
        var x = Mathf.RoundToInt(coords.x);
        var y = Mathf.RoundToInt(coords.y);
        return _cellGameObjects.FirstOrDefault(cell => cell.GetComponent<CellGameObject>().IsOverCell(coords));
    }
}
