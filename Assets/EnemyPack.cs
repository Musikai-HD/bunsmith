using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Pack", menuName = "Enemy/Enemy Pack", order = 1)]
public class EnemyPack : ScriptableObject
{
    public List<PickableEnemy> enemyPack;
}

[System.Serializable]
public class PickableEnemy
{
    public EnemyContainer enemy;
    public int chance;
}