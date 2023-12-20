using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DrawManager : MonoBehaviour
{
    public GameManager gameManager;
    public PrefabContainer prefabContainer;

    private Dictionary<GameObject, GameObject> _moveBoxes; // First is species, then the box
    private Dictionary<GameObject, GameObject> _selectionBoxes; // First is selection, then the box
    
    private void Awake()
    {
        _selectionBoxes = new Dictionary<GameObject, GameObject>();
        _moveBoxes = new Dictionary<GameObject, GameObject>();
    }
    
    public GameObject GetMoveBox(GameObject origin)
    {
        if (!_moveBoxes.ContainsKey(origin)) return null;
        return _moveBoxes[origin];
    }

    public void AssignSelectionBoxToGameObject(GameObject selectable)
    {
        if (selectable == null) return;
        if (!SelectionManager.IsSelectable(selectable)) return;
        if (_selectionBoxes.ContainsKey(selectable))
        {
            UnassignSelectionBoxFromGameObject(selectable);
        }
        var selectionBox = Instantiate(prefabContainer.GetPrefab("Selection"), selectable.transform);
        selectionBox.transform.position = selectable.transform.position;
        selectionBox.SetActive(false);
        _selectionBoxes.Add(selectable, selectionBox);
    }
    
    public void UnassignSelectionBoxFromGameObject(GameObject selectable)
    {
        if (selectable == null) return;
        if (!SelectionManager.IsSelectable(selectable)) return;
        if (!_selectionBoxes.ContainsKey(selectable)) return;
        Destroy(_selectionBoxes[selectable]);
        _selectionBoxes.Remove(selectable);
    }
    
    public void ShowSelectionBox(GameObject selectable)
    {
        if (selectable == null) return;
        if (!SelectionManager.IsSelectable(selectable)) return;
        if (!_selectionBoxes.ContainsKey(selectable)) return;
        _selectionBoxes[selectable].SetActive(true);
    }
    
    public void HideSelectionBox(GameObject selectable)
    {
        if (selectable == null) return;
        if (!SelectionManager.IsSelectable(selectable)) return;
        if (!_selectionBoxes.ContainsKey(selectable)) return;
        _selectionBoxes[selectable].SetActive(false);
    }

    public void AssignMoveBoxToPosition(Vector3 movePosition, GameObject origin)
    {
        if (origin == null) return;
        if (_moveBoxes.TryGetValue(origin, out var box))
        {
            Destroy(box);
        }
        _moveBoxes[origin] = Instantiate(prefabContainer.GetPrefab("Move"), movePosition, Quaternion.identity);
    }

    public void HideMoveBox(GameObject origin = null)
    {
        if (origin == null)
        {
            foreach (var box in _moveBoxes.Values)
            {
                Destroy(box);
            }
        }
        else
        {
            if (!_moveBoxes.ContainsKey(origin)) return;
            Destroy(_moveBoxes[origin]);
        }
    }

    public void UpdateMoveBoxPosition(Vector3 movePosition, GameObject argsObject)
    {
        if (argsObject == null) return;
        if (!_moveBoxes.ContainsKey(argsObject)) return;
        if (_moveBoxes[argsObject] == null) return;
        _moveBoxes[argsObject].transform.position = movePosition;
    }
}
