using UnityEngine;

[System.Serializable]
/// <summary>
/// Represents an attribute of a shield, such as defense or durability, with configurable min/max values and rarity weighting.
/// </summary>
public class ShieldAttribute
{
    /// <summary>
    /// The name of the attribute (e.g., "Defense").
    /// </summary>
    public string name;

    /// <summary>
    /// Minimum possible value for this attribute.
    /// </summary>
    public float minValue;

    /// <summary>
    /// Maximum possible value for this attribute.
    /// </summary>
    public float maxValue;

    /// <summary>
    /// Current value of the attribute.
    /// </summary>
    public float currentValue;

    /// <summary>
    /// Weight affecting how rare this attribute is (higher values make it rarer).
    /// </summary>
    public float rarityWeight;

    /// <summary>
    /// Initializes a new ShieldAttribute with the given parameters.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <param name="rarityWeight">Rarity weight (default 0.5f).</param>
    public ShieldAttribute(string name, float min, float max, float rarityWeight = 0.5f)
    {
        this.name = name;
        this.minValue = min;
        this.maxValue = max;
        this.rarityWeight = rarityWeight;
        this.currentValue = (min + max) * 0.5f;
    }
}
