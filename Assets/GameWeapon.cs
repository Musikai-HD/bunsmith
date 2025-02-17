using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Extensions;

public class GameWeapon : MonoBehaviour
{
    public Weapon weapon;
    public float aimAngle;
    bool canFire = true;
    public bool reloading;
    public int mag;

    public GameObject bulletPrefab;
    Transform spriteParent;
    SpriteRenderer spr;
    public float recoil;
    public float recoilMult, recoilRotMult, recoilSpeed, recoilReturnSpeed, aimSpeed;

    public AudioClip fire, bigFire, reload, finishReload;

    float gunDist;

    void Awake()
    {
        spriteParent = transform.GetChild(0);
        spr = spriteParent.GetComponent<SpriteRenderer>();
        gunDist = spriteParent.localPosition.x;
    }

    void Start()
    {
        canFire = true;
        InitializeWeapon();
    }
    
    void OnValidate()
    {
        InitializeWeapon();
    }

    public void InitializeWeapon()
    {
        weapon.InitializeWeapon();
        mag = weapon.Mag;
    }

    // Update is called once per frame
    void Update()
    {
        spr.flipY = EulerSign(aimAngle) == -1;
        if (reloading)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, EulerSign(aimAngle) == 1 ? -70f : -110f), Time.deltaTime * aimSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, aimAngle + (EulerSign(aimAngle) * recoil * recoilRotMult)), Time.deltaTime * aimSpeed);
        }
        spriteParent.GetComponent<SpriteRenderer>();
        spriteParent.localPosition = Vector3.Lerp(spriteParent.localPosition, new Vector3(gunDist - recoil, 0f, 0f), Time.deltaTime * recoilSpeed);
        recoil = Mathf.Lerp(recoil, 0f, recoilReturnSpeed * Time.deltaTime);
    }

    public void TryFire()
    {
        if (canFire)
        {
            if (mag > 0)
            {
                Fire();
                canFire = false;
                mag--;
                Invoke("SetCanFire", weapon.FireRate);
                if (mag <= 0) Reload();
            }
        }
    }

    void Fire()
    {
        for (int i = 0; i < weapon.BulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, spriteParent.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = (Vector2)(Quaternion.Euler(0, 0, aimAngle + Random.Range(-weapon.Accuracy, weapon.Accuracy)) * Vector2.right) * weapon.BulletSpeed;
            bullet.GetComponent<Bullet>().hb.hitInfo = new HitInfo(weapon.Damage, weapon.BulletStatus, weapon.StatusDamage, weapon.StatusTime);
            bullet.GetComponent<Bullet>().lifetime = weapon.frame.bulletLiftetime;
        }
        recoil = weapon.Damage * recoilMult * weapon.BulletCount;
        AudioManager.instance.Play(bigFire);
    }

    void SetCanFire()
    {
        canFire = true;
    }

    public void Reload()
    {
        CancelInvoke("FinishReload");
        canFire = false;
        mag = 0;
        reloading = true;
        Invoke("FinishReload", weapon.Reload);
        AudioManager.instance.Play(reload);
    }
    
    void FinishReload()
    {
        reloading = false;
        SetCanFire();
        mag = weapon.Mag;
        AudioManager.instance.Play(finishReload);
    }
}
