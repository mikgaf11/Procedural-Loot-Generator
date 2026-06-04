using UnityEngine;

[CreateAssetMenu(menuName = "PCG/Shield Blueprint")]
/// <summary>
/// A ScriptableObject that defines a blueprint for generating shields, including parts and base attributes.
/// </summary>
public class ShieldBlueprintAsset : ScriptableObject
{
    [Header("Identity")]
    /// <summary>
    /// Unique identifier for the shield blueprint.
    /// </summary>
    public string shieldId;

    /// <summary>
    /// The type or category of the shield (e.g., "Round", "Kite").
    /// </summary>
    public string shieldType;

    [Header("Parts")]
    /// <summary>
    /// The base part of the shield, defining its core structure.
    /// </summary>
    public string basePart;

    /// <summary>
    /// The rim part of the shield, providing edge details.
    /// </summary>
    public string rimPart;

    /// <summary>
    /// Optional accessory part for additional customization.
    /// </summary>
    public string accessoryPart;

    /// <summary>
    /// Visual effects part for the shield.
    /// </summary>
    public string vfxPart;

    [Header("Attributes")]
    /// <summary>
    /// Base defense value of the shield.
    /// </summary>
    public float defense;

    /// <summary>
    /// Base chance to block incoming attacks.
    /// </summary>
    public float blockChance;

    /// <summary>
    /// Base durability of the shield.
    /// </summary>
    public float durability;
}
