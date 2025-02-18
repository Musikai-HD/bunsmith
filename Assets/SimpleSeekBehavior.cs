using Pathfinding;
using UnityEngine;

public class SimpleSeekBehavior : EnemyBehavior
{
    protected override void Update()
    {
        base.Update();
        if (timeSinceLastSeen == 0f) 
        {
            target = player.transform.position;
            path.destination = target;
        }
        path.whenCloseToDestination = SeesPlayer() ? CloseToDestinationMode.Stop : CloseToDestinationMode.ContinueToExactDestination; 
    }
}
