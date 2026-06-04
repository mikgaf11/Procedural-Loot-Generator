#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldPipelineGraphWindow : EditorWindow
{
    private static ShieldBlueprintAsset _lastBlueprint;
    private static ShieldTemplateConfig _lastConfig;
    private static GameObject _lastShieldGO;

    private ShieldPipelineGraphView _graphView;
    private ShieldBlueprintAsset _blueprint;
    private ShieldTemplateConfig _config;

    [MenuItem("PCG/Shield Pipeline Graph")]
    public static void OpenMenu() => GetWindow<ShieldPipelineGraphWindow>(false, "Shield Pipeline", true).Show();

    public static void OpenWith(ShieldBlueprintAsset bp, ShieldTemplateConfig config, GameObject shieldGO)
    {
        _lastBlueprint = bp;
        _lastConfig    = config;
        _lastShieldGO  = shieldGO;

        var w = GetWindow<ShieldPipelineGraphWindow>(false, "Shield Pipeline", true);
        w._blueprint = bp;
        w._config    = config;
        w.Show();
        w.Focus();
        w.RebuildGraph();
    }

    public void CreateGUI()
    {
        var toolbar = new Toolbar();
        var bpField = new ObjectField("Blueprint") { objectType = typeof(ShieldBlueprintAsset), value = _blueprint };
        bpField.RegisterValueChangedCallback(e => { _blueprint = e.newValue as ShieldBlueprintAsset; _config = null; RebuildGraph(); });
        var cfgField = new ObjectField("Config") { objectType = typeof(ShieldTemplateConfig), value = _config };
        cfgField.RegisterValueChangedCallback(e => { _config = e.newValue as ShieldTemplateConfig; RebuildGraph(); });
        var reloadBtn = new ToolbarButton(RebuildGraph) { text = "⟳  Reload" };
        toolbar.Add(bpField); toolbar.Add(cfgField); toolbar.Add(reloadBtn);
        rootVisualElement.Add(toolbar);

        _graphView = new ShieldPipelineGraphView();
        _graphView.StretchToParentSize();
        _graphView.SetOnPartChanged(OnPartChangedInGraph);
        rootVisualElement.Add(_graphView);

        if (_blueprint == null) _blueprint = _lastBlueprint;
        if (_config    == null) _config    = _lastConfig;
        RebuildGraph();
    }

    private void RebuildGraph()
    {
        if (_blueprint == null) return;
        if (_config == null)
        {
            var gen = FindObjectOfType<ShieldGeneratorComponent>();
            if (gen != null) _config = gen.GetTemplateForType(_blueprint.shieldType);
        }
        if (_config != null && _graphView != null) _graphView.BuildPipeline(_blueprint, _config);
    }

    private void OnPartChangedInGraph()
    {
        if (_lastShieldGO == null) return;
        var gen = FindObjectOfType<ShieldGeneratorComponent>();
        if (gen == null) return;

        var data = new GeneratedShieldData {
            shieldId = _blueprint.shieldId, shieldType = _blueprint.shieldType,
            basePart = _blueprint.basePart, rimPart = _blueprint.rimPart,
            accessoryPart = _blueprint.accessoryPart, vfxPart = _blueprint.vfxPart,
            defense = _blueprint.defense, blockChance = _blueprint.blockChance, durability = _blueprint.durability
        };

        Vector3 oldPos = _lastShieldGO.transform.position;
        Undo.DestroyObjectImmediate(_lastShieldGO);
        var newGO = gen.SpawnShieldFromData(data);
        if (newGO != null) { newGO.transform.position = oldPos; _lastShieldGO = newGO; Selection.activeGameObject = newGO; }
        RebuildGraph();
    }
}
#endif