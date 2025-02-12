using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController pc;
    public Debugger debugger;
    public GameWeapon gw;
    public TextMeshProUGUI healthText, ammoText, gunText;

    void Awake()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        gw = pc.weapon;
        debugger = GameObject.Find("Debugger").GetComponent<Debugger>();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        healthText.text = $"HP: {pc.hc.health}/{pc.hc.maxHealth}";
        ammoText.text = $"{gw.mag}<size=55%><voffset=21>{gw.weapon.Mag}";
        if (debugger.debugToolsOn)
        {
            gunText.text = "<b>" + gw.weapon.frame == null ? "" : gw.weapon.frame.name + "\n</b>"
            + gw.weapon.stock == null ? "" : gw.weapon.stock.name + "\n"
            + gw.weapon.barrel == null ? "" : gw.weapon.barrel.name + "\n"
            + gw.weapon.attachment == null ? "" : gw.weapon.attachment.name + "\n"
            + gw.weapon.bullets == null ? "" : gw.weapon.bullets.name;
        }
    }
}
