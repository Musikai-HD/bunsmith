using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using static Extensions;

public class PlayerController : Damageable
{
    public enum PlayerState
    {
        Normal,
        Dashing,
        Locked
    }
    public PlayerState state;
    bool inputsLocked;
    public float maxSpeed;
    public float curSpeed;
    public Vector2 curInput;
    public bool fireHeld;

    public Rigidbody2D rb;

    public GameWeapon weapon;

    public PlayerInput input;
    public InteractChecker interactor;

    protected override void Update()
    {
        base.Update();
        RegisterState();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        if (inputsLocked)
        {
            curInput = Vector2.zero;
        }
        else
        {
            weapon.aimAngle = rotZ;
            spr.flipX = EulerSign(rotZ) == -1;
            rb.linearVelocity = curInput * maxSpeed;
            if (fireHeld)
            {
                weapon.TryFire();
            }
        }
    }

    void OnDisable()
    {
        //fix this! passing ref instead of copy
        GameManager.instance.savedHealth = new HealthComponent();
        GameManager.instance.savedHealth.CopyFrom(hc);
        GameManager.instance.savedWeapon = new Weapon();
        GameManager.instance.savedWeapon.CopyFrom(weapon.weapon);
    }

    void OnEnable()
    {
        hc.CopyFrom(GameManager.instance.savedHealth);
        if (GameManager.instance.savedWeapon.frame != null)
        {
            weapon.weapon = new Weapon();
            weapon.weapon.CopyFrom(GameManager.instance.savedWeapon);
        }
        weapon.InitializeWeapon();
    }

    void RegisterState()
    {
        switch (state)
        {
            case PlayerState.Normal:
                inputsLocked = false;
                break;
            case PlayerState.Dashing:
            case PlayerState.Locked:
                inputsLocked = true;
                break;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        curInput = context.ReadValue<Vector2>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        fireHeld = context.ReadValueAsButton();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (!inputsLocked)
            {
                interactor.Interact();
            }
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            Debug.Log("reload");
            if (weapon.reloading == false && !inputsLocked)
            {
                weapon.Reload();
            }
        }
    }
}
