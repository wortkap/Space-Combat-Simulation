using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipDataBase", menuName = "Scriptable Objects/ShipDataBase")]
public class ShipDataBase : ScriptableObject
{
    public List<ShipInstanceData> ships = new();

    public ShipInstanceData Get(string id) => ships.Find(ships => ships.Id == id);

    public void AddOrReplace(ShipInstanceData data)
    {
        var existing = ships.FindIndex(s => s.Id == data.Id);
        if (existing >= 0)
            ships[existing] = data;
        else
            ships.Add(data);
    }

    public void Remove(string id) => ships.RemoveAll(s => s.Id == id);
}