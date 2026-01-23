
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

partial struct ShipPositionSyncSystem : ISystem
{
    private const float CellSize = 5f;

    public void OnUpdate(ref SystemState state)
    {
        var em = state.EntityManager;

        foreach (var bridge in GameObject.FindObjectsByType<ShipECSBridge>(FindObjectsSortMode.None))
        {
            if (bridge.ShipEntity == Entity.Null)
                continue;

            float3 pos = bridge.transform.position;

            em.SetComponentData(bridge.ShipEntity, new ShipPosition { Value = pos });
            em.SetComponentData(bridge.ShipEntity, new GridCell { Value = new int2(
                (int)math.floor(pos.x / CellSize),
                (int)math.floor(pos.z / CellSize))
            });
        }
    }
}
