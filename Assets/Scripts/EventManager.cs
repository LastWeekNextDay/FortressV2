using System;
using System.Collections;
using System.Collections.Generic;
using Species;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameManager gameManager;
    
    public event Action<EventArgs> OnSelectSelectableObjectEvent;
    public event Action<EventArgs> OnUnselectSelectableObjectEvent;
    
    public event Action<EventArgs> OnFollowSpeciesEvent; 
    public event Action<EventArgs> OnUnfollowSpeciesEvent; 
    
    public event Action<EventArgs> OnSpawnSpeciesEvent; 
    public event Action<EventArgs> OnRemoveSpeciesEvent;
    
    public event Action<EventArgs> OnCellCreationEvent;
    public event Action<EventArgs> OnCellDeletionEvent;
    
    public event Action<EventArgs> OnCellMoveEvent;
    
    public event Action<EventArgs> OnReachDestinationEvent;
    public event Action<EventArgs> OnMoveEvent;

    private void Awake()
    {
        OnSelectSelectableObjectEvent += HandleMakeSelectedArgs;
        OnSelectSelectableObjectEvent += HandleShowPlayerControlledSpeciesMove;
        OnSelectSelectableObjectEvent += HandleShowSelectionBoxArgs;
        
        OnUnselectSelectableObjectEvent += HandleMakeUnselectedArgs;
        OnUnselectSelectableObjectEvent += HandleHidePlayerControlledSpeciesMove;
        OnUnselectSelectableObjectEvent += HandleHideSelectionBoxArgs;
        
        
        OnSpawnSpeciesEvent += HandleAssignSelectionBoxToGameObjectArgs;
            
        OnRemoveSpeciesEvent += InvokeUnselectSelectableObjectEvent;
        OnRemoveSpeciesEvent += HandleUnassignSelectionBoxFromGameObjectArgs;
        
        OnMoveEvent += HandlePlayerControlledSpeciesMoveUpdate;
    }
    
    public void HandleMakeSelectedArgs(EventArgs args)
    {
        gameManager.selectionManager.MakeSelected(args.Object, args.Boolean);
    }
    
    public void HandleMakeUnselectedArgs(EventArgs args)
    {
        gameManager.selectionManager.MakeUnselected(args.Object, args.Boolean);
    }
    
    public void HandleShowPlayerControlledSpeciesMove(EventArgs args)
    {
        if (args.Object.GetComponent<SpeciesGameObject>() == null) return;
        if (args.Object.GetComponent<SpeciesGameObject>().PlayerControlled == false) return;
        if (args.Object.GetComponent<Navigator>() == null) return;
        var movePositions = args.Object.GetComponent<Navigator>().PathNodes;
        if (movePositions == null) return;
        if (movePositions.Count <= 0) return;
        gameManager.drawManager.AssignMoveBoxToPosition(movePositions[^1], args.Object);
    }
    
    public void HandleHidePlayerControlledSpeciesMove(EventArgs args)
    {
        gameManager.drawManager.HideMoveBox();
    }

    public void HandlePlayerControlledSpeciesMoveUpdate(EventArgs args)
    {
        if (args.Object.GetComponent<SpeciesGameObject>() == null) return;
        if (args.Object.GetComponent<SpeciesGameObject>().PlayerControlled == false) return;
        if (args.Object.GetComponent<Navigator>() == null) return;
        if (gameManager.selectionManager.IsSelected(args.Object) == false) return;
        var movePositions = args.Object.GetComponent<Navigator>().PathNodes;
        if (movePositions == null) return;
        if (movePositions.Count <= 0) return;
        gameManager.drawManager.UpdateMoveBoxPosition(movePositions[^1], args.Object);
    }
    
    public void HandleAssignSelectionBoxToGameObjectArgs(EventArgs args)
    {
        gameManager.drawManager.AssignSelectionBoxToGameObject(args.Object);
    }

    public void HandleUnassignSelectionBoxFromGameObjectArgs(EventArgs args)
    {
        gameManager.drawManager.UnassignSelectionBoxFromGameObject(args.Object);
    }
    
    public void HandleShowSelectionBoxArgs(EventArgs args)
    {
        gameManager.drawManager.ShowSelectionBox(args.Object);
    }
    
    public void HandleHideSelectionBoxArgs(EventArgs args)
    {
        gameManager.drawManager.HideSelectionBox(args.Object);
    }
    
    public void InvokeSelectableObjectSelectionEvent(EventArgs args)
    {
        OnSelectSelectableObjectEvent?.Invoke(args);
    }
    
    public void InvokeUnselectSelectableObjectEvent(EventArgs args)
    {
        OnUnselectSelectableObjectEvent?.Invoke(args);
    }
    
    public void InvokeFollowSpeciesEvent(EventArgs args)
    {
        OnFollowSpeciesEvent?.Invoke(args);
    }
    
    public void InvokeUnfollowSpeciesEvent(EventArgs args)
    {
        OnUnfollowSpeciesEvent?.Invoke(args);
    }
    
    public void InvokeSpawnSpeciesEvent(EventArgs args)
    {
        OnSpawnSpeciesEvent?.Invoke(args);
    }
    
    public void InvokeRemoveSpeciesEvent(EventArgs args)
    {
        OnRemoveSpeciesEvent?.Invoke(args);
    }
    
    public void InvokeCellCreationEvent(EventArgs args)
    {
        OnCellCreationEvent?.Invoke(args);
    }
    
    public void InvokeCellDeletionEvent(EventArgs args)
    {
        OnCellDeletionEvent?.Invoke(args);
    }
    
    public void InvokeCellMoveEvent(EventArgs args)
    {
        OnCellMoveEvent?.Invoke(args);
    }
    
    public void InvokeReachDestinationEvent(EventArgs args)
    {
        OnReachDestinationEvent?.Invoke(args);
    }
    
    public void InvokeMoveEvent(EventArgs args)
    {
        OnMoveEvent?.Invoke(args);
    }
}
