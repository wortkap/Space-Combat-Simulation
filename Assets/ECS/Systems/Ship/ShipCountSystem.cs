using Unity.Burst;
using Unity.Entities;

partial struct ShipCountSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        int count = SystemAPI.QueryBuilder().WithAll<ShipTag>().Build().CalculateEntityCount();
        if (count > 0 )
            UnityEngine.Debug.Log("Ship count: " + count); 
    }
}