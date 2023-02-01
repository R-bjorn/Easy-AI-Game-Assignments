using UnityEngine;

namespace Project.Positions
{
    /// <summary>
    /// A position that a soldier can move to.
    /// </summary>
    [DisallowMultipleComponent]
    public class StrategicPoint : Position
    {
        /// <summary>
        /// If this is an defensive or offensive position.
        /// </summary>
        public bool defensive = true;

        /// <summary>
        /// If this position is free to move to.
        /// </summary>
        public bool Open => Count == 0;
    }
}