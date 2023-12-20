using System;
using System.Collections.Generic;
using UnityEngine;

public struct EventArgs
{
    public GameObject Object;
    public List<GameObject> Objects;
    public bool Boolean;
    public Vector3 Vector3Coords;
    public Color32 Color32;
}

public interface ISelectorSelectable
{
    //public event Action<EventArgs> OnSelectEvent;
    //public void Select();
    
    //public void ClearSubscribers();
}
