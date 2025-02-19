using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController pc;
    public Debugger debugger;
    public GameWeapon gw;
    public TextMeshProUGUI healthText, ammoText, gunText;

    public static GameManager instance;

    public Weapon savedWeapon;
    public HealthComponent savedHealth;

    //Pities
    public float 
    framePity = 1f,
    stockPity = 1f,
    barrelPity = 1f,
    attachmentPity = 1f,
    bulletsPity = 1f;

    void Awake()
    {
        //singleton declaration
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
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
        if (debugger.debugToolsOn && gw.weapon != null)
        {
            gunText.text = "<b>" + (gw.weapon.frame == null ? "" : gw.weapon.frame) + "\n</b>"
            + (gw.weapon.stock == null ? "" : gw.weapon.stock) + "\n"
            + (gw.weapon.barrel == null ? "" : gw.weapon.barrel) + "\n"
            + (gw.weapon.attachment == null ? "" : gw.weapon.attachment) + "\n"
            + (gw.weapon.bullets == null ? "" : gw.weapon.bullets);
        }
    }
}
