using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CellGameObject : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CellData _cellData;
    private GameObject _moveBox;
    public CellData CellData 
    { 
        get => _cellData;
        set 
        {
            _cellData = value;
            if (_cellData.GroundInfo != null)
            {
                _spriteRenderer.color = _cellData.GroundInfo.Color;
            }
        } 
    }
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var navigator = other.GetComponent<Navigator>();
        if (navigator == null) return;
        if (navigator.OnCellData != _cellData)
        {
            navigator.OnCellData = _cellData; 
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var selectionManager = GameObject.Find("Game Manager").GetComponent<SelectionManager>();
            var selectedSpecies = selectionManager.GetSelectedPlayerControlledSpecies();
            if (selectedSpecies.Count <= 0) return;
            foreach (var species in selectedSpecies)
            {
                species.GetComponent<Navigator>().GoTo(transform.position);
            }
        }
    }
    
    public bool IsOverCell(Vector3 position)
    {
        var cellPosition = transform.position;
        var cellSize = GetComponent<SpriteRenderer>().bounds.size;
        var cellXMin = cellPosition.x - cellSize.x / 2;
        var cellXMax = cellPosition.x + cellSize.x / 2;
        var cellYMin = cellPosition.y - cellSize.y / 2;
        var cellYMax = cellPosition.y + cellSize.y / 2;
        var isOverCell = position.x >= cellXMin && position.x <= cellXMax && position.y >= cellYMin && position.y <= cellYMax;
        return isOverCell;
    }
}
