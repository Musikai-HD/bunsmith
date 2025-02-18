using UnityEngine;
using Pathfinding;

public class EnemyBehavior : Damageable
{
    protected AIPath path;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector2 target;
    public float attentionSpan, sightDistance;
    public Vector2 aimDirection;
    public float sightAngle;
    public LayerMask wallMask;
    protected GameObject player;
    public bool alerted;
    protected float timeSinceLastSeen;

    protected override void Awake()
    {
        base.Awake();
        path = GetComponent<AIPath>();
        player = GameObject.Find("Player");
    }

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void Update()
    {
        base.Update();
        path.maxSpeed = moveSpeed;
        if (!SeesPlayer()) timeSinceLastSeen += Time.deltaTime;
        if (alerted && SeesPlayer() || timeSinceLastSeen < attentionSpan)
        {
            timeSinceLastSeen = 0f;
            aimDirection = (player.transform.position - transform.position).normalized;
        }

        spr.flipX = Mathf.Sign(aimDirection.x) == -1f;

        
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        alerted = true;
    }

    public bool SeesPlayer()
    {
        return DistFromPlayer() < sightDistance && !Physics2D.Raycast(transform.position, player.transform.position - transform.position, DistFromPlayer(), wallMask);
    }

    public float DistFromPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }
}
