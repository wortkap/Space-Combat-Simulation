using UnityEngine;

[CreateAssetMenu(fileName = "HullDefinition", menuName = "Ship/Hull Definition")]
public class HullDefinition : ScriptableObject
{
    public string HullId;
    public string DisplayName;

    [Header("Gameplay Stats")]
    public float BaseHealth;
    public float BaseMass;
    public int WeaponSlotCount;
    public int UtilitySlotCount;
    public int MaxArmorPerSlot;

    [Header("Visuals")]
    public GameObject HullVisualPrefab; // Must be a prefab with mesh + MeshRenderer
}