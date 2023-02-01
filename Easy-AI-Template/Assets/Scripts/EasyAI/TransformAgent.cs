using UnityEngine;

namespace EasyAI
{
    /// <summary>
    /// Agent which moves directly through its transform.
    /// </summary>
    [DisallowMultipleComponent]
    public class TransformAgent : Agent
    {
        /// <summary>
        /// Transform movement.
        /// </summary>
        public override void MovementCalculations()
        {
            CalculateMoveVelocity(Time.deltaTime);
            transform.position += MoveVelocity3 * DeltaTime;
        }
    }
}