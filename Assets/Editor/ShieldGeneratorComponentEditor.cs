#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(ShieldGeneratorComponent))]
public class ShieldGeneratorComponentEditor : Editor
{
    private const string BlueprintFolder = "Assets/Data/GeneratedBlueprints";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var gen = (ShieldGeneratorComponent)target;

        if (GUILayout.Button("▶   Generate Shield", GUILayout.Height(36)))
        {
            var data = gen.GenerateDataOnly();
            if (data == null) return;
            var shieldGO = gen.SpawnShieldFromData(data);
            
            if (!AssetDatabase.IsValidFolder(BlueprintFolder)) Directory.CreateDirectory(BlueprintFolder);
            
            var bp = ScriptableObject.CreateInstance<ShieldBlueprintAsset>();
            bp.shieldId = data.shieldId;
            bp.shieldType = data.shieldType;
            bp.basePart = data.basePart;
            bp.rimPart = data.rimPart;
            bp.accessoryPart = data.accessoryPart;
            bp.vfxPart = data.vfxPart;
            bp.defense = data.defense;
            bp.blockChance = data.blockChance;
            bp.durability = data.durability;

            string path = $"{BlueprintFolder}/ShieldBlueprint_{data.shieldId.Substring(0, 6)}.asset";
            AssetDatabase.CreateAsset(bp, path);
            AssetDatabase.SaveAssets();
            
            ShieldPipelineGraphWindow.OpenWith(bp, gen.LastUsedTemplate, shieldGO);
        }
    }
}
#endif