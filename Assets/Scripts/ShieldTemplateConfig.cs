using UnityEngine;

[CreateAssetMenu(fileName = "RoundShieldTemplate", menuName = "PCG/Shield Template Config")]
/// <summary>
/// Configuration asset for shield generation templates, defining available parts and attribute ranges.
/// </summary>
public class ShieldTemplateConfig : ScriptableObject
{
    /// <summary>
    /// The type or category of shields this template generates (e.g., "RoundShield").
    /// </summary>
    public string shieldType = "RoundShield";

    [Header("Parts (weights: higher = more likely)")]
    /// <summary>
    /// Array of base part definitions for shields.
    /// </summary>
    public ShieldPartDefinition[] baseParts;

    /// <summary>
    /// Array of rim part definitions for shields.
    /// </summary>
    public ShieldPartDefinition[] rimParts;

    /// <summary>
    /// Array of accessory part definitions for shields.
    /// </summary>
    public ShieldPartDefinition[] accessoryParts;

    /// <summary>
    /// Array of VFX part definitions for shields.
    /// </summary>
    public ShieldPartDefinition[] vfxParts;

    [Header("Attribute ranges (higher values should be rarer)")]
    /// <summary>
    /// Range for defense attribute values.
    /// </summary>
    public ShieldAttribute defenseRange = new ShieldAttribute("Defense", 10, 100, 0.3f);

    /// <summary>
    /// Range for block chance attribute values.
    /// </summary>
    public ShieldAttribute blockRange = new ShieldAttribute("BlockChance", 0.1f, 0.5f, 0.5f);

    /// <summary>
    /// Range for durability attribute values.
    /// </summary>
    public ShieldAttribute durabilityRange = new ShieldAttribute("Durability", 50, 200, 0.7f);

    [System.Serializable]
    /// <summary>
    /// Defines a shield part with name, rarity weight, and prefab.
    /// </summary>
    public class ShieldPartDefinition
    {
        /// <summary>
        /// The name of the part.
        /// </summary>
        public string partName;

        [Header("Tweaks")]
        public Vector3 positionOffset = Vector3.zero; // <--- Add this
        public Vector3 rotationOffset = Vector3.zero; // <--- Add this

        /// <summary>
        /// Rarity weight for selection (higher values are more likely).
        /// </summary>
        public float rarityWeight = 1f;

        /// <summary>
        /// The prefab GameObject for this part.
        /// </summary>
        public GameObject prefab;
    }
}
