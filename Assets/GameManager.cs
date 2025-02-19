using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController pc;
    public Debugger debugger;
    public GameWeapon gw;
    public TextMeshProUGUI healthText, ammoText, gunText;

    public static GameManager instance;

    public Weapon savedWeapon;
    public HealthComponent savedHealth;
    public float pityReturnFactor, pityCrutchFactor;

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
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        gw = pc.weapon;
        debugger = GameObject.Find("Debugger").GetComponent<Debugger>();
        gunText = GameObject.Find("GunText").GetComponent<TextMeshProUGUI>();
        ammoText = GameObject.Find("Mag Text").GetComponent<TextMeshProUGUI>();
        healthText = GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        healthText.text = $"HP: {pc?.hc.health}/{pc?.hc.maxHealth}";
        ammoText.text = $"{gw.mag}<size=55%><voffset=21>{gw.weapon.Mag}";
        if (debugger.debugToolsOn && gw.weapon != null)
        {
            gunText.text = "<b>" + (gw.weapon.frame == null ? "" : gw.weapon.frame) + "\n</b>"
            + (gw.weapon.stock == null ? "" : gw.weapon.stock) + "\n"
            + (gw.weapon.barrel == null ? "" : gw.weapon.barrel) + "\n"
            + (gw.weapon.attachment == null ? "" : gw.weapon.attachment) + "\n"
            + (gw.weapon.bullets == null ? "" : gw.weapon.bullets);
        }
        CapPity();
    }

    public void AdjustPity(ItemWrapper.ItemType ignore)
    {
        framePity = ItemWrapper.ItemType.WeaponFrame != ignore ? Mathf.Lerp(framePity, 1f, pityReturnFactor) : framePity * pityCrutchFactor;
        stockPity = ItemWrapper.ItemType.WeaponStock != ignore ? Mathf.Lerp(stockPity, 1f, pityReturnFactor) : stockPity * pityCrutchFactor;
        barrelPity = ItemWrapper.ItemType.WeaponBarrel != ignore ? Mathf.Lerp(barrelPity, 1f, pityReturnFactor) : barrelPity * pityCrutchFactor;
        attachmentPity = ItemWrapper.ItemType.WeaponAttachment != ignore ? Mathf.Lerp(attachmentPity, 1f, pityReturnFactor) : attachmentPity * pityCrutchFactor;
        bulletsPity = ItemWrapper.ItemType.WeaponBullets != ignore ? Mathf.Lerp(bulletsPity, 1f, pityReturnFactor) : bulletsPity * pityCrutchFactor;
        CapPity();
    }

    void CapPity()
    {
        framePity = Mathf.Min(1f, framePity);
        stockPity = Mathf.Min(1f, stockPity);
        barrelPity = Mathf.Min(1f, barrelPity);
        attachmentPity = Mathf.Min(1f, attachmentPity);
        bulletsPity = Mathf.Min(1f, bulletsPity);
    }
}
