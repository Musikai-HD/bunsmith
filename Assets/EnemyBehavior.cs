using UnityEngine;
using Pathfinding;

public class EnemyBehavior : Damageable
{
    private AIPath path;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform target;

    protected override void Start()
    {
        base.Start();
        path = GetComponent<AIPath>();
    }

    protected override void Update()
    {
        base.Update();
        path.maxSpeed = moveSpeed;
        path.destination = target.position;
    }
}
