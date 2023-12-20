using UnityEngine;

public class MudGroundInfo : IGroundInfo
{
    public string Name => "Mud";
    public Color32 Color => new(82, 58, 16, 255);
    public float SpeedModifier => 0.25f;
}
