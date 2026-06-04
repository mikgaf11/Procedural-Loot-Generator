#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsNode : ShieldNodeBase
{
    public Port Input;

    public StatsNode(ShieldBlueprintAsset bp)
    {
        title = "📊  Stats";
        SetNodeStyle(new Color(0.1f, 0.30f, 0.15f));

        Input = AddInputPort("In");

        mainContainer.Add(MakeSeparator());
        mainContainer.Add(MakeRow("Step", "6 — Rolled Attributes"));
        mainContainer.Add(MakeSeparator());

        mainContainer.Add(MakeRow("Defense",      bp.defense.ToString("0"),               new Color(0.4f, 0.9f, 0.5f)));
        mainContainer.Add(MakeRow("Block Chance",  (bp.blockChance * 100f).ToString("0.#") + "%", new Color(0.4f, 0.8f, 1f)));
        mainContainer.Add(MakeRow("Durability",    bp.durability.ToString("0"),             new Color(1f, 0.8f, 0.3f)));
        mainContainer.Add(MakeSeparator());

        RefreshExpandedState();
        RefreshPorts();
    }
}
#endif
