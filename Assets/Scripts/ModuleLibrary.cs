using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class ModuleLibrary : MonoBehaviour
{
    public Dictionary<string, Entity> _moduleById = new();

    public IReadOnlyDictionary<string, Entity> Modules => _moduleById;

    IEnumerator Start()
    {
        yield return null;
        BuildLookup();
    }

    public void BuildLookup()
    {
        _moduleById.Clear();
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;

        var q = em.CreateEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(ModuleId) },
            Options = EntityQueryOptions.IncludePrefab
        });
        using var ents = q.ToEntityArray(Allocator.Temp);
        foreach (var e in ents)
        {
            var id = em.GetComponentData<ModuleId>(e).Id.ToString();
            if (!_moduleById.ContainsKey(id))
            {
                Debug.LogWarning($"Loaded module {id}");
                _moduleById.Add(id, e);
            }
        }
    }

    public Entity GetModule(string moduleId)
    {
        if (moduleId == null)
            return Entity.Null;
        if (_moduleById.TryGetValue(moduleId, out var e))
            return e;

        Debug.LogWarning($"ModuleLibrary: module id '{moduleId}' not found.");
        return Entity.Null;
    }
}