using Unity.Entities;
using UnityEngine;

public partial class ArmorSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var em = EntityManager;

        // Collect all armor type entities with ArmorType component (baked asset)
        // We assume there's exactly one armor type selected per ship (the player picks an armor module id for all slots)
        // For this example, we compute mass/health simply by looking up a module id on the ship root (not implemented),
        // so this system might be extended to map armor module id → ArmorType entity.
    }
}
