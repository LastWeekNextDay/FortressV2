using UnityEngine;

public class GrassGroundInfo : IGroundInfo
{
    public string Name { get; } = "Grass";
    public Color32 Color { get; } = new(0, 150, 0, 255);
    public float SpeedModifier { get; } = 0.95f;
}
