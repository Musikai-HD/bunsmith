using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hitbox
{
    public LayerMask hittableLayers;
    public HitInfo hitInfo;
    public AudioClip hit;
    public List<GameObject> blacklist = new List<GameObject>();
    public bool canHit = true;

    public virtual int Hit(Collider2D col)
    {
        if ((hittableLayers & 1 << col.gameObject.layer) == 1 << col.gameObject.layer && canHit)
        {
            Damageable dam = col.gameObject.GetComponent<Damageable>();
            if (dam && !blacklist.Contains(col.gameObject)) 
            {
                dam.Damage(hitInfo.damage);
                AudioManager.instance.Play(hit);
                blacklist.Add(col.gameObject);
            }
            if (!dam)
            {
                return 0;
            }
            return 1;
        }
        return 2;
    }
}

public class HitInfo
{
    public float damage, statusDamage, statusTime;
    public Status.StatusType status;
    public HitInfo(float _damage, Status.StatusType _status = Status.StatusType.None, float _statusDamage = 0f, float _statusTime = 0f)
    {
        damage = _damage;
        status = _status;
        statusDamage = _statusDamage;
        statusTime = _statusTime;
    }
}