using Unity.Entities;
using System.Collections.Generic;

public partial class HullPrefabRegistrySystem : SystemBase
{
    public static Dictionary<string, Entity> HullPrefabs = new();

    protected override void OnStartRunning() { }

    protected override void OnUpdate() { }
}