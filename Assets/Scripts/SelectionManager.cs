using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Species;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectionManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<GameObject> selectedGameObjects;
    
    private bool _selecting;
    private Vector3 _selectionStart;
    private Vector3 _selectionEnd;
    private LineRenderer _selectionRenderer;

    private void Awake()
    {
        selectedGameObjects = new List<GameObject>();
        _selecting = false;
        // Create a LineRenderer component
        _selectionRenderer = gameObject.AddComponent<LineRenderer>();
        _selectionRenderer.positionCount = 4;
        _selectionRenderer.startWidth = 0.1f;
        _selectionRenderer.endWidth = 0.1f;
        _selectionRenderer.loop = true;
        _selectionRenderer.useWorldSpace = true;
        _selectionRenderer.material.color = Color.red;
        _selectionRenderer.sortingOrder = 2;
        _selectionRenderer.enabled = false;
    }
    private void Update()
    {
        ProcessSelectionArea();
        UpdateSelectionVisualization();
        ProcessSelectionFollowInput();
    }

    private void ProcessSelectionFollowInput()
    {
        if (!Input.GetKeyUp(KeyCode.Space)) return;
        if (selectedGameObjects.Count == 1 && selectedGameObjects[0].GetComponent<SpeciesGameObject>() != null)
        {
            var args = new EventArgs
            {
                Object = selectedGameObjects[0],
                Boolean = false
            };
            gameManager.eventManager.InvokeFollowSpeciesEvent(args);
        }
        else
        {
            var args = new EventArgs();
            gameManager.eventManager.InvokeUnfollowSpeciesEvent(args);
        }
    }

    public void MakeSelected(GameObject selection, bool additive = false)
    {
        if (selection == null) return;
        if (!IsSelectable(selection)) return;
        Debug.Log("Selecting " + selection.name);
        if (!additive)
        {
            var toBeRemoved = new List<GameObject>();
            foreach (var objEach in selectedGameObjects)
            {
                toBeRemoved.Add(objEach);
            }
            foreach (var obj in toBeRemoved)
            {
                var argsEach = new EventArgs
                {
                    Object = obj,
                    Boolean = true
                };
                gameManager.eventManager.InvokeUnselectSelectableObjectEvent(argsEach);
            }
            selectedGameObjects.Clear();
        }
        else
        {
            if (IsSelected(selection)) return;    
        }
        selectedGameObjects.Add(selection);
        var args = new EventArgs
        {
            Object = selection,
            Boolean = additive
        };
        // To prevent infinite loop
        gameManager.eventManager.OnSelectSelectableObjectEvent -= gameManager.eventManager.HandleMakeSelectedArgs;
        gameManager.eventManager.InvokeSelectableObjectSelectionEvent(args);
        gameManager.eventManager.OnSelectSelectableObjectEvent += gameManager.eventManager.HandleMakeSelectedArgs;
    }
    
    public void MakeUnselected(GameObject selection = null, bool additive = false)
    {
        Debug.Log("Deselecting");
        if (additive)
        {
            if (selection == null) return;
            if (!IsSelectable(selection)) return;
            if (!IsSelected(selection)) return;
            Debug.Log(" " + selection.name);
            var args = new EventArgs
            {
                Object = selection,
                Boolean = true,
            };
            selectedGameObjects.Remove(selection);
            // To prevent infinite loop
            gameManager.eventManager.OnUnselectSelectableObjectEvent -= gameManager.eventManager.HandleMakeUnselectedArgs;
            gameManager.eventManager.InvokeUnselectSelectableObjectEvent(args);
            gameManager.eventManager.OnUnselectSelectableObjectEvent += gameManager.eventManager.HandleMakeUnselectedArgs;
        }
        else
        {
            var toBeRemoved = selectedGameObjects.ToList();
            foreach (var obj in toBeRemoved)
            {
                var argsEach = new EventArgs
                {
                    Object = obj,
                    Boolean = true
                };
                // To prevent infinite loop
                gameManager.eventManager.OnUnselectSelectableObjectEvent -= gameManager.eventManager.HandleMakeUnselectedArgs;
                gameManager.eventManager.InvokeUnselectSelectableObjectEvent(argsEach);
                gameManager.eventManager.OnUnselectSelectableObjectEvent += gameManager.eventManager.HandleMakeUnselectedArgs;        
            }
            selectedGameObjects.Clear();
        }
    }

    private void ProcessSelectionArea()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            _selecting = true;
            _selectionStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _selectionStart.z = -5f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _selecting = false;
            _selectionEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _selectionEnd.z = -5f;

            // Perform actions on selected objects (implement your logic here)
            SelectObjectsInArea();
        }
    }

    private void SelectObjectsInArea()
    {
        // Perform actions on objects within the selection rectangle
        var colliders = Physics2D.OverlapAreaAll(_selectionStart, _selectionEnd);
        var additive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var selectables = 0;
        var toBeRemoved = selectedGameObjects.ToList();
        if (!additive)
        {
            foreach (var obj in toBeRemoved)
            {
                var args = new EventArgs
                {
                    Object = obj,
                    Boolean = true
                };
                gameManager.eventManager.InvokeUnselectSelectableObjectEvent(args);
            }
        }
        foreach (var touch in colliders)
        {
            if (!IsSelectable(touch.gameObject)) continue;
            selectables++;
            var args = new EventArgs
            {
                Object = touch.gameObject,
                Boolean = true
            };
            gameManager.eventManager.InvokeSelectableObjectSelectionEvent(args);
        }
        if (selectables == 0)
        {
            var args = new EventArgs
            {
                Boolean = additive
            };
            gameManager.eventManager.InvokeUnselectSelectableObjectEvent(args);
        }
    }

    private void UpdateSelectionVisualization()
    {
        if (_selecting)
        {
            // Use current mouse position as the second point of the rectangle
            var currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = -5f;
            
            if (Vector3.Distance(_selectionStart, currentMousePosition) > 1f)
            {
                _selectionRenderer.enabled = true;
            }

            // Update LineRenderer positions to visualize the selection rectangle
            var positions = new Vector3[4];
            positions[0] = _selectionStart;
            positions[1] = new Vector3(currentMousePosition.x, _selectionStart.y, _selectionStart.z);
            positions[2] = currentMousePosition;
            positions[3] = new Vector3(_selectionStart.x, currentMousePosition.y, _selectionStart.z);

            _selectionRenderer.SetPositions(positions);
        }
        else
        {
            // Disable the LineRenderer when not selecting
            _selectionRenderer.enabled = false;
        }
    }

    public List<GameObject> GetSelectedPlayerControlledSpecies()
    {
        var selectedSpecies = new List<GameObject>();
        foreach (var selectedGameObject in selectedGameObjects)
        {
            var speciesGameObject = selectedGameObject.GetComponent<SpeciesGameObject>();
            if (speciesGameObject == null) continue;
            if (speciesGameObject.PlayerControlled)
                selectedSpecies.Add(speciesGameObject.gameObject);
        }
        return selectedSpecies;
    }
    
    public bool IsSelected(GameObject selection)
    {
        return selectedGameObjects.Contains(selection);
    }
    
    public static bool IsSelectable(GameObject selection)
    {
        return selection.GetComponent<ISelectorSelectable>() != null;
    }
}
