#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ShieldNodeBase : Node
{
    protected void SetNodeStyle(Color accent)
    {
        titleContainer.style.backgroundColor = new StyleColor(accent);
        style.minWidth = 260;
    }

    protected Port AddInputPort(string label = "In")
    {
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        port.portName = label;
        inputContainer.Add(port);
        return port;
    }

    protected Port AddOutputPort(string label = "Out")
    {
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        port.portName = label;
        outputContainer.Add(port);
        return port;
    }

    protected VisualElement MakeRow(string label, string value, Color valueColor = default)
    {
        if (valueColor == default) valueColor = new Color(0.85f, 0.95f, 1f);

        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.paddingLeft = 8;
        row.style.paddingRight = 8;
        row.style.paddingTop = 2;
        row.style.paddingBottom = 2;

        var lbl = new Label(label);
        lbl.style.unityFontStyleAndWeight = FontStyle.Bold;
        lbl.style.minWidth = 100;
        lbl.style.color = new StyleColor(new Color(0.7f, 0.7f, 0.7f));

        var val = new Label(value);
        val.style.color = new StyleColor(valueColor);
        val.style.unityTextAlign = TextAnchor.MiddleLeft;

        row.Add(lbl);
        row.Add(val);
        return row;
    }

    protected VisualElement MakeSeparator()
    {
        var sep = new VisualElement();
        sep.style.height = 1;
        sep.style.marginTop = 5;
        sep.style.marginBottom = 5;
        sep.style.marginLeft = 8;
        sep.style.marginRight = 8;
        sep.style.backgroundColor = new StyleColor(new Color(0.35f, 0.35f, 0.35f));
        return sep;
    }

    protected VisualElement MakeWeightBar(float weight, float maxWeight = 1f)
    {
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;
        container.style.paddingLeft = 8;
        container.style.paddingRight = 8;
        container.style.paddingBottom = 6;
        container.style.alignItems = Align.Center;

        var lbl = new Label("Rarity Weight");
        lbl.style.minWidth = 100;
        lbl.style.color = new StyleColor(new Color(0.7f, 0.7f, 0.7f));
        lbl.style.unityFontStyleAndWeight = FontStyle.Bold;

        var track = new VisualElement();
        track.style.flexGrow = 1;
        track.style.height = 8;
        track.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
        track.style.borderTopLeftRadius = 4;
        track.style.borderTopRightRadius = 4;
        track.style.borderBottomLeftRadius = 4;
        track.style.borderBottomRightRadius = 4;

        float ratio = maxWeight > 0 ? Mathf.Clamp01(weight / maxWeight) : 0f;
        var fill = new VisualElement();
        fill.style.width = Length.Percent(ratio * 100f);
        fill.style.height = 8;
        // colour: green = common, orange = uncommon, red = rare
        Color barColor = ratio > 0.6f ? new Color(0.2f, 0.8f, 0.3f)
                       : ratio > 0.3f ? new Color(0.9f, 0.6f, 0.1f)
                       : new Color(0.9f, 0.2f, 0.2f);
        fill.style.backgroundColor = new StyleColor(barColor);
        fill.style.borderTopLeftRadius = 4;
        fill.style.borderBottomLeftRadius = 4;

        var weightLbl = new Label(weight.ToString("0.##"));
        weightLbl.style.marginLeft = 6;
        weightLbl.style.color = new StyleColor(new Color(0.85f, 0.85f, 0.85f));

        track.Add(fill);
        container.Add(lbl);
        container.Add(track);
        container.Add(weightLbl);
        return container;
    }
}
#endif
