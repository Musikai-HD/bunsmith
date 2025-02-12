using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Frame", menuName = "Weapon/Frame", order = 0)]
public class WeaponFrame : ItemWrapper
{
    public Sprite sprite;
    public float damage = 1f;
    public float fireRate = 1f;
    public float accuracy = 0f;
    public float mag = 1;
    public float bulletCount = 1f;
    public float reload = 1f;
    public float bulletSpeed = 10f;
}
