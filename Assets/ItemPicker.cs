using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework.Internal;
using UnityEditor.Rendering;
using UnityEngine;

public static class ItemPicker
{
    public static ItemWrapper PickItem(ItemPool pool, float luck = 0f)
    {
        return PickItem(pool, pool.commonChance, pool.uncommonChance, pool.rareChance, pool.epicChance, pool.legendaryChance);
    }


    public static ItemWrapper PickItem(ItemPool pool, float commonChance, float uncommonChance, float rareChance = 0f, float epicChance = 0f, float legendaryChance = 0f)
    {
        List<ItemWrapper> commons = new List<ItemWrapper>();
        List<ItemWrapper> uncommons = new List<ItemWrapper>();
        List<ItemWrapper> rares = new List<ItemWrapper>();
        List<ItemWrapper> epics = new List<ItemWrapper>();
        List<ItemWrapper> legendaries = new List<ItemWrapper>();
        for (int i = 0; i < pool.items.Count; i++)
        {
            switch (pool.items[i].rarity)
            {
                case ItemWrapper.Rarity.Common:
                    commons.Add(pool.items[i]);
                    break;
            }
        }
        float chance = Random.Range(0f, commonChance + uncommonChance + rareChance + epicChance + legendaryChance);
        float chanceThreshold = 0f;
        if (chance <= legendaryChance)
        {
            if (legendaries[0]) return legendaries[Random.Range(0, legendaries.Count-1)];
            chanceThreshold += legendaryChance;
        }
        if (chance <= chanceThreshold + epicChance)
        {
            if (epics[0]) return epics[Random.Range(0, epics.Count-1)];
            chanceThreshold += epicChance;
        }
        if (chance <= chanceThreshold + rareChance)
        {
            if (rares[0]) return rares[Random.Range(0, rares.Count-1)];
            chanceThreshold += rareChance;
        }
        if (chance <= chanceThreshold + uncommonChance)
        {
            if (uncommons[0]) return uncommons[Random.Range(0, uncommons.Count-1)];
            chanceThreshold += uncommonChance;
        }
        return commons[Random.Range(0, commons.Count-1)];
    }
}

[CreateAssetMenu(fileName = "New Item Drop Table", menuName = "Drop Table", order = 0)]
public class ItemPool
{
    public List<ItemWrapper> items;
    public float commonChance = 60f, uncommonChance = 25f, rareChance = 10f, epicChance = 4f, legendaryChance = 1f;
}