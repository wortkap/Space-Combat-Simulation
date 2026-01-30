using Unity.Entities;

public struct ThrusterModule : IComponentData
{
    public float MaxSpeed;
    public float CurrentSpeed;
    public float Thrust;
    public float TurnRate;
}
