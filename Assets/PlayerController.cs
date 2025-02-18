using UnityEngine;
using UnityEngine.InputSystem;
using static Extensions;

public class PlayerController : Damageable
{
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
        rb.linearVelocity = curInput * maxSpeed;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        weapon.aimAngle = rotZ;
        spr.flipX = EulerSign(rotZ) == -1;

        if (fireHeld)
        {
            weapon.TryFire();
        }
    }

    void OnDisable()
    {
        //fix this! passing ref instead of copy
        GameManager.instance.savedHealth = hc;
        GameManager.instance.savedWeapon = weapon.weapon;
    }

    void OnEnable()
    {
        hc = GameManager.instance.savedHealth ?? hc;
        weapon.weapon = GameManager.instance.savedWeapon ?? weapon.weapon;
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
            interactor.Interact();
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            Debug.Log("reload");
            if (weapon.reloading == false)
            {
                weapon.Reload();
            }
        }
    }
}
