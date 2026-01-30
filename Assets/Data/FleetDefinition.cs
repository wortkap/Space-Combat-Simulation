using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FleetEntry
{
    public string ShipId;
    public int Count;
    public int faction;
    public Vector3 SpawnLocation;
}

[CreateAssetMenu(fileName = "FleetDefinition", menuName = "Scriptable Objects/FleetDefinition")]
public class FleetDefinition : ScriptableObject
{
    public string FleetId;
    public string DisplayName;
    public List<FleetEntry> Entries = new();
}