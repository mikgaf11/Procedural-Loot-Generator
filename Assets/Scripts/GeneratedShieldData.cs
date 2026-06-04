using UnityEngine;

[System.Serializable]
/// <summary>
/// Represents the data for a generated shield, including its parts and stats.
/// </summary>
public class GeneratedShieldData
{
    /// <summary>
    /// Unique identifier for the shield, used for referencing in code and assets.
    /// </summary>
    public string shieldId;

    /// <summary>
    /// The type or category of the shield (e.g., "Round", "Kite").
    /// </summary>
    public string shieldType;

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

    /// <summary>
    /// Defense value of the shield, affecting damage reduction.
    /// </summary>
    public float defense;

    /// <summary>
    /// Chance to block incoming attacks.
    /// </summary>
    public float blockChance;

    /// <summary>
    /// Durability of the shield, determining how much damage it can withstand.
    /// </summary>
    public float durability;
}
