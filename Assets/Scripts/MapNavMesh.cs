using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEngine;

public class MapNavMesh
{
    private readonly NavMeshSurface _navMeshSurface;
    private CollectSources2d _collectSources2D;
    
    public MapNavMesh(NavMeshSurface navMeshSurface, CollectSources2d collectSources2D)
    {
        _navMeshSurface = navMeshSurface;
        _collectSources2D = collectSources2D;
    }
    
    public void BuildNavMesh()
    {
        _navMeshSurface.gameObject.transform.rotation = Quaternion.Euler(-89.98f, 0, 0);
        _navMeshSurface.BuildNavMesh();
    }
    
    public void UpdateNavMesh()
    {
        _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
    }
}
