using UnityEngine;

public static class GridController
{
    public static readonly Vector3 Grid = new Vector3(0.8f, 1f, 0.8f);
    public static  int LegoLayer = LayerMask.GetMask("Lego");
    
    public static Vector3 SnapToGrid(Vector3 input)
    {
        return new Vector3(Mathf.Floor(input.x / Grid.x) * Grid.x,
                           Mathf.Floor(input.y / Grid.y) * Grid.y,
                           Mathf.Floor(input.z / Grid.z) * Grid.z);
    }
}