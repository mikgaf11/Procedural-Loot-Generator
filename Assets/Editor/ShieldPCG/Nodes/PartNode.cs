#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PartNode : ShieldNodeBase
{
    public Port Input;
    public Port Output;

    public PartNode(
        string nodeTitle,
        string stepLabel,
        string currentPart,
        float weight,
        Color accent,
        string[] allPartNames,
        ShieldBlueprintAsset blueprint,
        Action<string> onPartChanged)
    {
        title = nodeTitle;
        SetNodeStyle(accent);

        Input  = AddInputPort("In");
        Output = AddOutputPort("Out");

        mainContainer.Add(MakeSeparator());
        mainContainer.Add(MakeRow("Step", stepLabel));
        mainContainer.Add(MakeSeparator());

        // --- Dropdown ---
        var choices = new List<string>(allPartNames);
        if (!choices.Contains("None")) choices.Insert(0, "None");

        string initial = string.IsNullOrEmpty(currentPart) ? "None" : currentPart;
        if (!choices.Contains(initial)) choices.Add(initial);

        var dropdown = new PopupField<string>("Part", choices, initial);
        dropdown.style.paddingLeft = 6;
        dropdown.style.paddingRight = 6;
        dropdown.style.paddingBottom = 4;

        dropdown.RegisterValueChangedCallback(evt =>
        {
            onPartChanged?.Invoke(evt.newValue);
        });

        mainContainer.Add(dropdown);
        mainContainer.Add(MakeSeparator());
        mainContainer.Add(MakeWeightBar(weight));

        RefreshExpandedState();
        RefreshPorts();
    }
}
#endif
