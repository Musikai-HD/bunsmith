using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemPicker
{
    public static ItemWrapper PickItem(ItemPool pool)
    {
        //currently pulling from a pool of every existing item in the game
        pool.items = Resources.LoadAll<ItemWrapper>("").ToList();

        //define chances based on floor; defaults 
        float common = BaseChances.FLOOR1_CHANCE_COMMON;
        float uncommon = BaseChances.FLOOR1_CHANCE_UNCOMMON;
        float rare = BaseChances.FLOOR1_CHANCE_RARE;
        float epic = BaseChances.FLOOR1_CHANCE_EPIC;
        float legendary = BaseChances.FLOOR1_CHANCE_LEGENDARY;

        //implement custom chances if pool asks
        if (pool.customChances)
        {
            common = pool.commonChance;
            uncommon = pool.uncommonChance;
            rare = pool.rareChance;
            epic = pool.epicChance;
            legendary = pool.legendaryChance;
        }

        //separate item pool into types, then dump the copied pool's item list
        List<WeaponFrame> framePool = pool.items.OfType<WeaponFrame>().ToList();
        List<WeaponStock> stockPool = pool.items.OfType<WeaponStock>().ToList();
        List<WeaponBarrel> barrelPool = pool.items.OfType<WeaponBarrel>().ToList();
        List<WeaponAttachment> attachmentsPool = pool.items.OfType<WeaponAttachment>().ToList();
        List<WeaponBullets> bulletsPool = pool.items.OfType<WeaponBullets>().ToList();

        //select desired pool based on chance
        ItemWrapper.ItemType _type = PickItemType(pool.items);
        pool.items.Clear();
        switch (_type)
        {
            case ItemWrapper.ItemType.WeaponFrame:
                for (int i = 0; i < framePool.Count; i++) pool.items.Add(framePool[i]);
                break;
            case ItemWrapper.ItemType.WeaponStock:
                for (int i = 0; i < stockPool.Count; i++) pool.items.Add(stockPool[i]);
                break;
            case ItemWrapper.ItemType.WeaponBarrel:
                for (int i = 0; i < barrelPool.Count; i++) pool.items.Add(barrelPool[i]);
                break;
            case ItemWrapper.ItemType.WeaponAttachment:
                for (int i = 0; i < attachmentsPool.Count; i++) pool.items.Add(attachmentsPool[i]);
                break;
            case ItemWrapper.ItemType.WeaponBullets:
                for (int i = 0; i < bulletsPool.Count; i++) pool.items.Add(bulletsPool[i]);
                break;
        }

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
        if (chance < commonChance)
        {
            if (commons.Count > 0) return commons[Random.Range(0, commons.Count-1)];
        }
        chanceThreshold += commonChance;
        if (chance < chanceThreshold + uncommonChance)
        {
            if (uncommons.Count > 0) return uncommons[Random.Range(0, uncommons.Count-1)];
        }
        chanceThreshold += uncommonChance;
        if (chance < chanceThreshold + rareChance)
        {
            if (rares.Count > 0) return rares[Random.Range(0, rares.Count-1)];
        }
        chanceThreshold += rareChance;
        if (chance < chanceThreshold + epicChance)
        {
            if (epics.Count > 0) return epics[Random.Range(0, epics.Count-1)];
        }
        if (legendaries.Count > 0) return legendaries[Random.Range(0, legendaries.Count-1)];
        if (epics.Count > 0) return epics[Random.Range(0, epics.Count-1)];
        if (rares.Count > 0) return rares[Random.Range(0, rares.Count-1)];
        if (uncommons.Count > 0) return uncommons[Random.Range(0, uncommons.Count-1)];
        if (commons.Count > 0) return commons[Random.Range(0, commons.Count-1)];
        return null;
    }

    public static List<T> ItemsOfType<T>()
    {
        T[] resources = Resources.LoadAll("", typeof(T)) as T[];
        List<T> resourceList = new List<T>();
        foreach (T re in resources)
        {
            resourceList.Add(re);
        }
        return resourceList;
    }

    public static ItemWrapper.ItemType PickItemType
    (
        List<ItemWrapper> _pool,
        float _frame = BaseChances.TYPE_CHANCE_FRAME, 
        float _stock = BaseChances.TYPE_CHANCE_STOCK, 
        float _barrel = BaseChances.TYPE_CHANCE_BARREL, 
        float _attachment = BaseChances.TYPE_CHANCE_ATTACHMENT, 
        float _bullets = BaseChances.TYPE_CHANCE_BULLETS
    )
    {
        _frame *= !_pool.OfType<WeaponFrame>().Any() ? 0f : GameManager.instance.framePity;
        _stock *= !_pool.OfType<WeaponStock>().Any() ? 0f : GameManager.instance.stockPity;
        _barrel *= !_pool.OfType<WeaponBarrel>().Any() ? 0f : GameManager.instance.barrelPity;
        _attachment *= !_pool.OfType<WeaponAttachment>().Any() ? 0f : GameManager.instance.attachmentPity;
        _bullets *= !_pool.OfType<WeaponBullets>().Any() ? 0f : GameManager.instance.bulletsPity;
        float randomType = Random.Range(0, _frame
        + _stock
        + _barrel
        + _attachment
        + _bullets
        );


        //what de hell bro attachments are getting picked 99% of the time
        float chanceThreshold = 0f;
        ItemWrapper.ItemType returnedType = ItemWrapper.ItemType.NONE;

        if (randomType < _frame && returnedType == ItemWrapper.ItemType.NONE) returnedType = ItemWrapper.ItemType.WeaponFrame;
        chanceThreshold += _frame;

        if (randomType < chanceThreshold + _stock && returnedType == ItemWrapper.ItemType.NONE) returnedType = ItemWrapper.ItemType.WeaponStock;
        chanceThreshold += _stock;

        if (randomType < chanceThreshold + _barrel && returnedType == ItemWrapper.ItemType.NONE) returnedType = ItemWrapper.ItemType.WeaponBarrel;
        chanceThreshold += _barrel;

        if (randomType < chanceThreshold + _attachment && returnedType == ItemWrapper.ItemType.NONE) returnedType = ItemWrapper.ItemType.WeaponAttachment;
        chanceThreshold += _attachment;

        if (returnedType == ItemWrapper.ItemType.NONE) returnedType = ItemWrapper.ItemType.WeaponBullets;

        GameManager.instance.AdjustPity(returnedType);
        return returnedType;
    }
}