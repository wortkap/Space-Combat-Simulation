using Unity.Entities;
using UnityEngine;

public class ShipECSBridge : MonoBehaviour
{
    private Entity _shipEntity = Entity.Null;

    public Entity ShipEntity
    {
        get => _shipEntity;
        set
        {
            if (_shipEntity == value) return;

            // Remove old one
            ShipBridgeRegistry.Unregister(_shipEntity);

            _shipEntity = value;

            // Register new one
            ShipBridgeRegistry.Register(_shipEntity, this);
        }
    }

    private void OnDestroy()
    {
        ShipBridgeRegistry.Unregister(_shipEntity);
    }
}