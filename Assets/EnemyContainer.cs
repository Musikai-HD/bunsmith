using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Container", menuName = "Enemy/Enemy Container", order = 0)]
public class EnemyContainer : ScriptableObject, IComparable
{
    public GameObject enemyPrefab;
    public int enemyPower;

    public int CompareTo(object enemy)
    {
        var a = this;
        var b = enemy as EnemyContainer;
     
        if (a.enemyPower < b.enemyPower)
            return -1;
     
        if (a.enemyPower > b.enemyPower)
            return 1;

        return 0;
    }
}
