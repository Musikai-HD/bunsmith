using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework.Internal;
using UnityEditor.Rendering;
using UnityEngine;

public static class ItemPicker
{
    public static ItemWrapper PickItem(ItemPool pool, float luck = 0f)
    {
        if (pool.customChances) luck = 0f;
        float common = pool.commonChance - (50f * (luck / 50f));
        float uncommon = pool.uncommonChance - (10f * (luck / 50f));
        float rare = pool.rareChance + (1f * (luck / 50f));
        float epic = pool.epicChance + (3f * (luck / 50f));
        float legendary = pool.legendaryChance + (5f * (luck / 50f));
        return PickItem(pool, common, uncommon, rare, epic, legendary);
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
                case ItemWrapper.Rarity.Uncommon:
                    uncommons.Add(pool.items[i]);
                    break;
                case ItemWrapper.Rarity.Rare:
                    rares.Add(pool.items[i]);
                    break;
                case ItemWrapper.Rarity.Epic:
                    epics.Add(pool.items[i]);
                    break;
                case ItemWrapper.Rarity.Legendary:
                    legendaries.Add(pool.items[i]);
                    break;
            }
        }
        float chance = Random.Range(0f, commonChance + uncommonChance + rareChance + epicChance + legendaryChance);
        float chanceThreshold = 0f;
        if (chance <= legendaryChance)
        {
            if (legendaries.Count > 0) return legendaries[Random.Range(0, legendaries.Count-1)];
            chanceThreshold += legendaryChance;
        }
        if (chance <= chanceThreshold + epicChance)
        {
            if (epics.Count > 0) return epics[Random.Range(0, epics.Count-1)];
            chanceThreshold += epicChance;
        }
        if (chance <= chanceThreshold + rareChance)
        {
            if (rares.Count > 0) return rares[Random.Range(0, rares.Count-1)];
            chanceThreshold += rareChance;
        }
        if (chance <= chanceThreshold + uncommonChance)
        {
            if (uncommons.Count > 0) return uncommons[Random.Range(0, uncommons.Count-1)];
        }
        return commons[Random.Range(0, commons.Count-1)];
    }
}