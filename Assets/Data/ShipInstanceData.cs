using System;
using UnityEngine;

[Serializable]
public class ShipInstanceData
{
    public string Id;
    public string Name;
    public string HullId;

    public string ThrusterModuleId;

    public string[] WeaponModuleIds;

    public string[] UtilityModuleIds;

    public int ArmorFrontCount;
    public int ArmorBackCount;
    public int ArmorBodyCount;

    public Vector3[] WeaponLocalPositions;
    public Vector3[] WeaponLocalEuler;
    public Vector3 ThrusterLocalPosition;
    public Vector3 ThrusterLocalEuler;
}
