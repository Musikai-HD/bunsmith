using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Damageable
{
    public float maxSpeed;
    public float curSpeed;
    public Vector2 curInput;
    public bool fireHeld;

    public Rigidbody2D rb;

    public GameWeapon weapon;


    void Awake()
    {
        
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        rb.linearVelocity = curInput * maxSpeed;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        weapon.aimAngle = rotZ;

        if (fireHeld)
        {
            weapon.TryFire();
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

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (weapon.reloading == false)
            {
                weapon.Reload();
            }
        }
    }
}
