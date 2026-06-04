#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class TemplateNode : ShieldNodeBase
{
    public Port Output;

    public TemplateNode(ShieldBlueprintAsset bp)
    {
        title = "Template";
        SetNodeStyle(new Color(0.13f, 0.22f, 0.40f));

        Output = AddOutputPort("Pipeline");

        mainContainer.Add(MakeSeparator());
        mainContainer.Add(MakeRow("Step", "1 — Load Template"));
        mainContainer.Add(MakeRow("Shield Type", bp.shieldType, new Color(1f, 0.85f, 0.4f)));
        mainContainer.Add(MakeRow("Shield ID", bp.shieldId.Substring(0, 8) + "..."));
        mainContainer.Add(MakeSeparator());

        RefreshExpandedState();
        RefreshPorts();
    }
}
#endif
