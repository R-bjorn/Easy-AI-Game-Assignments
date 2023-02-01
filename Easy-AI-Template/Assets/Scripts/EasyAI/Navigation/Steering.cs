using UnityEngine;

namespace EasyAI.Navigation
{
    /// <summary>
    /// Steering behaviours implemented.
    /// These are static calls using simple parameters so they are not directly tied to agents but are easily implementable by them.
    /// </summary>
    public static class Steering
    {
        /// <summary>
        /// The various move behaviours available for agents.
        /// </summary>
        public enum Behaviour : byte
        {
            Seek,
            Flee,
            Pursue,
            Evade
        }

        /// <summary>
        /// Perform a move.
        /// </summary>
        /// <param name="behaviour">The type of move.</param>
        /// <param name="position">The position of the agent.</param>
        /// <param name="velocity">The current velocity of the agent.</param>
        /// <param name="targetCurrent">The current position of the target.</param>
        /// <param name="targetLast">The last position of the target if needed.</param>
        /// <param name="speed">The speed at which the agent can move.</param>
        /// <param name="deltaTime">The time elapsed between when the target is in its current position and its previous if needed.</param>
        /// <returns>Calculated movement.</returns>
        public static Vector2 Move(Behaviour behaviour, Vector2 position, Vector2 velocity, Vector2 targetCurrent, Vector2 targetLast, float speed, float deltaTime)
        {
            switch (behaviour)
            {
                case Behaviour.Evade:
                    return Evade(position, velocity, targetCurrent, targetLast, speed, deltaTime);
                case Behaviour.Pursue:
                    return Pursue(position, velocity, targetCurrent, targetLast, speed, deltaTime);
                case Behaviour.Flee:
                    return Flee(position, velocity, targetCurrent, speed);
                case Behaviour.Seek:
                default:
                    return Seek(position, velocity, targetCurrent, speed);
            }
        }

        /// <summary>
        /// Check if this is an approaching or moving away behaviour
        /// </summary>
        /// <param name="behaviour">The behaviour to check</param>
        /// <returns>True if it is an approaching behaviour, false otherwise</returns>
        public static bool IsApproachingBehaviour(Behaviour behaviour)
        {
            return behaviour is Behaviour.Seek or Behaviour.Pursue;
        }

        /// <summary>
        /// Check if a move is complete.
        /// </summary>
        /// <param name="behaviour">The move type</param>
        /// <param name="position">The position of the agent.</param>
        /// <param name="target">The desired destination position.</param>
        /// <returns>True if the move is complete, false otherwise.</returns>
        public static bool IsMoveComplete(Behaviour behaviour, Vector2 position, Vector2 target)
        {
            return !IsApproachingBehaviour(behaviour)
                ? Manager.FleeAcceptableDistance >= 0 &&
                  Vector2.Distance(position, target) >= Manager.FleeAcceptableDistance
                : Manager.SeekAcceptableDistance >= 0 &&
                  Vector2.Distance(position, target) <= Manager.SeekAcceptableDistance;
        }

        /// <summary>
        /// The color to make a certain move type appear with gizmos.
        /// Note that although not listed here, white and green are for pathfinding display and yellow for velocity.
        /// </summary>
        /// <param name="behaviour">The behaviour type.</param>
        /// <returns>The color to display.</returns>
        public static Color GizmosColor(Behaviour behaviour)
        {
            switch (behaviour)
            {
                case Behaviour.Evade:
                    return new(1f, 0.65f, 0f);
                case Behaviour.Pursue:
                    return Color.cyan;
                case Behaviour.Flee:
                    return Color.red;
                case Behaviour.Seek:
                default:
                    return Color.blue;
            }
        }
        
        /// <summary>
        /// Seek - Move directly towards a position.
        /// </summary>
        /// <param name="position">The position of the agent.</param>
        /// <param name="velocity">The current velocity of the agent.</param>
        /// <param name="evader">The position of the evader to seek to.</param>
        /// <param name="speed">The speed at which the agent can move.</param>
        /// <returns>The velocity to apply to the agent to perform the seek.</returns>
        public static Vector2 Seek(Vector2 position, Vector2 velocity, Vector2 evader, float speed)
        {
            return (evader - position).normalized * speed - velocity;
        }

        /// <summary>
        /// Flee - Move directly away from a position.
        /// </summary>
        /// <param name="position">The position of the agent.</param>
        /// <param name="velocity">The current velocity of the agent.</param>
        /// <param name="pursuer">The position of the pursuer to flee from.</param>
        /// <param name="speed">The speed at which the agent can move.</param>
        /// <returns>The velocity to apply to the agent to perform the flee.</returns>
        private static Vector2 Flee(Vector2 position, Vector2 velocity, Vector2 pursuer, float speed)
        {
            // TODO - Assignment 3 - Complete the remaining steering behaviours and use them to improve the microbes level.
            return Vector2.zero;
        }

        /// <summary>
        /// Pursue - Move towards a position factoring in its current speed to predict where it is moving.
        /// </summary>
        /// <param name="position">The position of the agent.</param>
        /// <param name="velocity">The current velocity of the agent.</param>
        /// <param name="evader">The position of the evader to pursuit to.</param>
        /// <param name="evaderLastPosition">The position of the evader during the last time step.</param>
        /// <param name="speed">The speed at which the agent can move.</param>
        /// <param name="deltaTime">The time elapsed between when the target is in its current position and its previous.</param>
        /// <returns>The velocity to apply to the agent to perform the pursuit.</returns>
        private static Vector2 Pursue(Vector2 position, Vector2 velocity, Vector2 evader, Vector2 evaderLastPosition, float speed, float deltaTime)
        {
            // TODO - Assignment 3 - Complete the remaining steering behaviours and use them to improve the microbes level.
            return Vector2.zero;
        }

        /// <summary>
        /// Evade - Move from a position factoring in its current speed to predict where it is moving.
        /// </summary>
        /// <param name="position">The position of the agent.</param>
        /// <param name="velocity">The current velocity of the agent.</param>
        /// <param name="pursuer">The position of the pursuer to evade from.</param>
        /// <param name="pursuerLastPosition">The position of the pursuer during the last time step.</param>
        /// <param name="speed">The speed at which the agent can move.</param>
        /// <param name="deltaTime">The time elapsed between when the target is in its current position and its previous.</param>
        /// <returns>The velocity to apply to the agent to perform the evade.</returns>
        private static Vector2 Evade(Vector2 position, Vector2 velocity, Vector2 pursuer, Vector2 pursuerLastPosition, float speed, float deltaTime)
        {
            // TODO - Assignment 3 - Complete the remaining steering behaviours and use them to improve the microbes level.
            return Vector2.zero;
        }
    }
}