using UnityEngine;

public class GameWeapon : MonoBehaviour
{
    public Weapon weapon;
    public float aimAngle;
    bool canFire = true;
    public bool reloading;
    int mag;

    public GameObject bulletPrefab;

    void Start()
    {
        canFire = true;
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
        transform.localRotation = Quaternion.Euler(0f, 0f, aimAngle);
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
            }
            else
            {
                Reload();
            }
        }
    }

    void Fire()
    {
        for (int i = 0; i < weapon.BulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = (Vector2)(Quaternion.Euler(0, 0, aimAngle + Random.Range(-weapon.Accuracy, weapon.Accuracy)) * Vector2.right) * weapon.BulletSpeed;
            bullet.GetComponent<Hitbox>().damage = weapon.Damage;
        }
    }

    void SetCanFire()
    {
        canFire = true;
    }

    public void Reload()
    {
        canFire = false;
        reloading = true;
        Invoke("FinishReload", weapon.Reload);
    }
    
    void FinishReload()
    {
        SetCanFire();
        mag = weapon.Mag;
    }
}
