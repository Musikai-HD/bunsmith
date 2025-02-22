using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Attachment", menuName = "Weapon/Attachment", order = 3)]
public class WeaponAttachment : WeaponPart
{
    public float statusTime, statusDamage;
    public bool fixedSpread;
}