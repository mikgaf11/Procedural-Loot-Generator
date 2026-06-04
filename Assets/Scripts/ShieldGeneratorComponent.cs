using System;
using UnityEngine;
using System.Linq;

public class ShieldGeneratorComponent : MonoBehaviour
{
    [Serializable]
    public class TemplateChoice
    {
        public string label;
        public ShieldTemplateConfig config;
        public float weight = 1f;
    }

    [Header("Templates (Shapes)")]
    [SerializeField] private TemplateChoice[] templates;
    public ShieldTemplateConfig LastUsedTemplate { get; private set; }

    [Header("Spawn")]
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 1, 3);

    [Header("Anchors")]
    [SerializeField] private string rimAnchorName = "RimAnchor";
    [SerializeField] private string accessoryAnchorName = "AccessoryAnchor";
    [SerializeField] private string vfxAnchorName = "VFXAnchor";

    public ShieldTemplateConfig PickTemplate()
    {
        if (templates == null || templates.Length == 0) return null;
        float total = templates.Sum(t => t.weight);
        float r = UnityEngine.Random.value * total;
        float c = 0f;
        foreach (var t in templates)
        {
            c += t.weight;
            if (r <= c) return t.config;
        }
        return templates[0].config;
    }

    public ShieldTemplateConfig GetTemplateForType(string type)
    {
        var choice = templates.FirstOrDefault(t => t.config.shieldType == type);
        return choice != null ? choice.config : (templates.Length > 0 ? templates[0].config : null);
    }

    public GeneratedShieldData GenerateDataOnly(int? seed = null)
    {
        var template = PickTemplate();
        if (template == null) return null;
        
        LastUsedTemplate = template;
        if (seed.HasValue) UnityEngine.Random.InitState(seed.Value);

        var baseDef = PickWeighted(template.baseParts);
        var rimDef  = PickWeighted(template.rimParts);
        var accDef  = PickWeighted(template.accessoryParts);
        var vfxDef  = PickWeighted(template.vfxParts);

        var data = new GeneratedShieldData
        {
            shieldId = Guid.NewGuid().ToString(),
            shieldType = template.shieldType,
            basePart = SafeName(baseDef),
            rimPart = SafeName(rimDef),
            accessoryPart = SafeName(accDef),
            vfxPart = SafeName(vfxDef),
            defense = Mathf.Round(RollRarerHigh(template.defenseRange.minValue, template.defenseRange.maxValue, template.defenseRange.rarityWeight)),
            blockChance = (float)Math.Round(RollRarerHigh(template.blockRange.minValue, template.blockRange.maxValue, template.blockRange.rarityWeight), 2),
            durability = Mathf.Round(RollRarerHigh(template.durabilityRange.minValue, template.durabilityRange.maxValue, template.durabilityRange.rarityWeight))
        };
        return data;
    }

    public GameObject SpawnShieldFromData(GeneratedShieldData data)
    {
        var template = templates.FirstOrDefault(t => t.config.shieldType == data.shieldType)?.config;
        if (template == null) return null;

        var root = new GameObject($"Shield_{data.shieldId.Substring(0, 6)}");
        if (spawnParent != null) root.transform.SetParent(spawnParent, false);
        root.transform.position = spawnPosition;

        var baseDef = FindByName(template.baseParts, data.basePart);
        var rimDef  = FindByName(template.rimParts, data.rimPart);
        var accDef  = FindByName(template.accessoryParts, data.accessoryPart);
        var vfxDef  = FindByName(template.vfxParts, data.vfxPart);

        var baseObj = InstantiatePart(baseDef.prefab, root.transform, $"Base_{data.basePart}", baseDef.positionOffset, baseDef.rotationOffset);
        
        var rimAnchor = FindOrSelf(baseObj.transform, rimAnchorName);
        var accAnchor = FindOrSelf(baseObj.transform, accessoryAnchorName);
        var vfxAnchor = FindOrSelf(baseObj.transform, vfxAnchorName);

        if (rimDef != null) InstantiatePart(rimDef.prefab, rimAnchor, $"Rim_{data.rimPart}", rimDef.positionOffset, rimDef.rotationOffset);
        if (accDef != null) InstantiatePart(accDef.prefab, accAnchor, $"Acc_{data.accessoryPart}", accDef.positionOffset, accDef.rotationOffset);
        if (vfxDef != null) InstantiatePart(vfxDef.prefab, vfxAnchor, $"VFX_{data.vfxPart}", vfxDef.positionOffset, vfxDef.rotationOffset);

        return root;
    }

    private GameObject InstantiatePart(GameObject prefab, Transform parent, string name, Vector3 pos, Vector3 rot)
    {
        if (prefab == null) return null;
        #if UNITY_EDITOR
        var go = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (go == null) go = Instantiate(prefab);
        #else
        var go = Instantiate(prefab);
        #endif
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localRotation = Quaternion.Euler(rot);
        go.transform.localScale = prefab.transform.localScale;
        return go;
    }

    private float RollRarerHigh(float min, float max, float rarityBias)
    {
        float b = Mathf.Clamp(rarityBias, 0.05f, 1f);
        return Mathf.Lerp(min, max, Mathf.Pow(UnityEngine.Random.value, 1f / b));
    }

    private ShieldTemplateConfig.ShieldPartDefinition PickWeighted(ShieldTemplateConfig.ShieldPartDefinition[] defs)
    {
        if (defs == null || defs.Length == 0) return null;
        float total = defs.Sum(d => d.rarityWeight);
        float r = UnityEngine.Random.value * total;
        float c = 0f;
        foreach (var d in defs)
        {
            c += d.rarityWeight;
            if (r <= c) return d;
        }
        return defs[0];
    }

    private string SafeName(ShieldTemplateConfig.ShieldPartDefinition def) => def != null ? def.partName : "None";

    private ShieldTemplateConfig.ShieldPartDefinition FindByName(ShieldTemplateConfig.ShieldPartDefinition[] defs, string name) 
        => defs?.FirstOrDefault(d => d.partName == name);

    private Transform FindOrSelf(Transform root, string childName) 
        => root.Find(childName) ?? root;
}