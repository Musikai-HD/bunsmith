using UnityEngine;

public class WalkerObject
{
    public Vector2 position;
    public Vector2 direction;
    public float changeChance;

    public WalkerObject(Vector2 pos, Vector2 dir, float change)
    {
        position = pos;
        direction = dir;
        changeChance = change;
    }
}
