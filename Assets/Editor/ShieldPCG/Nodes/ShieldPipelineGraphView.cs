#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldPipelineGraphView : GraphView
{
    private ShieldBlueprintAsset _bp;
    private ShieldTemplateConfig _config;
    private Action _onPartChanged;

    public ShieldPipelineGraphView()
    {
        style.flexGrow = 1;

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        var minimap = new MiniMap { anchored = true };
        minimap.SetPosition(new Rect(10, 30, 180, 120));
        Add(minimap);
    }

    public void SetOnPartChanged(Action callback) => _onPartChanged = callback;

    public void BuildPipeline(ShieldBlueprintAsset bp, ShieldTemplateConfig config)
    {
        _bp = bp;
        _config = config;

        DeleteElements(graphElements);

        string[] baseNames = GetNames(config.baseParts);
        string[] rimNames  = GetNames(config.rimParts);
        string[] accNames  = GetNames(config.accessoryParts);
        string[] vfxNames  = GetNames(config.vfxParts);

        float baseWeight = GetWeight(config.baseParts, bp.basePart);
        float rimWeight  = GetWeight(config.rimParts,  bp.rimPart);
        float accWeight  = GetWeight(config.accessoryParts, bp.accessoryPart);
        float vfxWeight  = GetWeight(config.vfxParts,  bp.vfxPart);

        var templateNode = new TemplateNode(bp);

        var baseNode = new PartNode("⬡  Base Part",  "2 — Pick Base",      bp.basePart,      baseWeight, new Color(0.35f, 0.20f, 0.08f), baseNames, bp, v => OnPartSwapped("base", v));
        var rimNode  = new PartNode("◎  Rim Part",   "3 — Pick Rim",       bp.rimPart,       rimWeight,  new Color(0.22f, 0.22f, 0.28f), rimNames,  bp, v => OnPartSwapped("rim",  v));
        var accNode  = new PartNode("✦  Accessory",  "4 — Pick Accessory", bp.accessoryPart, accWeight,  new Color(0.28f, 0.12f, 0.28f), accNames,  bp, v => OnPartSwapped("acc",  v));
        var vfxNode  = new PartNode("✨  VFX",        "5 — Pick VFX",       bp.vfxPart,       vfxWeight,  new Color(0.08f, 0.22f, 0.32f), vfxNames,  bp, v => OnPartSwapped("vfx",  v));
        var statsNode = new StatsNode(bp);

        templateNode.SetPosition(new Rect(40,  220, 270, 170));
        baseNode.SetPosition    (new Rect(380,  30, 280, 180));
        rimNode.SetPosition     (new Rect(380, 230, 280, 180));
        accNode.SetPosition     (new Rect(380, 430, 280, 180));
        vfxNode.SetPosition     (new Rect(380, 630, 280, 180));
        statsNode.SetPosition   (new Rect(730, 220, 290, 210));

        AddElement(templateNode);
        AddElement(baseNode);
        AddElement(rimNode);
        AddElement(accNode);
        AddElement(vfxNode);
        AddElement(statsNode);

        AddElement(Connect(templateNode.Output, baseNode.Input));
        AddElement(Connect(templateNode.Output, rimNode.Input));
        AddElement(Connect(templateNode.Output, accNode.Input));
        AddElement(Connect(templateNode.Output, vfxNode.Input));
        AddElement(Connect(baseNode.Output,  statsNode.Input));
        AddElement(Connect(rimNode.Output,   statsNode.Input));
        AddElement(Connect(accNode.Output,   statsNode.Input));
        AddElement(Connect(vfxNode.Output,   statsNode.Input));
    }

    private void OnPartSwapped(string slot, string newPartName)
    {
        if (_bp == null || _config == null) return;

        // Update blueprint asset
        switch (slot)
        {
            case "base": _bp.basePart       = newPartName; break;
            case "rim":  _bp.rimPart        = newPartName; break;
            case "acc":  _bp.accessoryPart  = newPartName; break;
            case "vfx":  _bp.vfxPart        = newPartName; break;
        }

        EditorUtility.SetDirty(_bp);
        AssetDatabase.SaveAssets();

        // Notify window to rebuild scene shield
        _onPartChanged?.Invoke();
    }

    private Edge Connect(Port from, Port to)
    {
        var edge = new Edge { output = from, input = to };
        edge.input.Connect(edge);
        edge.output.Connect(edge);
        return edge;
    }

    private string[] GetNames(ShieldTemplateConfig.ShieldPartDefinition[] defs)
    {
        if (defs == null) return new string[0];
        return defs.Where(d => d != null && !string.IsNullOrEmpty(d.partName))
                   .Select(d => d.partName).ToArray();
    }

    private float GetWeight(ShieldTemplateConfig.ShieldPartDefinition[] defs, string name)
    {
        if (defs == null || string.IsNullOrEmpty(name) || name == "None") return 0f;
        foreach (var d in defs)
            if (d != null && d.partName == name) return d.rarityWeight;
        return 0f;
    }
}
#endif
