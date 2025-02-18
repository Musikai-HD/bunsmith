using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Drop Table", menuName = "Drop Table", order = 0)]
public class ItemPool : ScriptableObject
{
    public List<ItemWrapper> items;
    public float commonChance = 60f, uncommonChance = 25f, rareChance = 10f, epicChance = 4f, legendaryChance = 1f;
    public bool customChances;
}