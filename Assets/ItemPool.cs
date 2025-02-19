using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Drop Table", menuName = "Drop Table", order = 0)]
public class ItemPool : ScriptableObject
{
    public List<ItemWrapper> items;
    public float 
    commonChance = BaseChances.FLOOR1_CHANCE_COMMON, 
    uncommonChance = BaseChances.FLOOR1_CHANCE_UNCOMMON, 
    rareChance = BaseChances.FLOOR1_CHANCE_RARE, 
    epicChance = BaseChances.FLOOR1_CHANCE_EPIC, 
    legendaryChance = BaseChances.FLOOR1_CHANCE_LEGENDARY;
    public bool customChances;
}