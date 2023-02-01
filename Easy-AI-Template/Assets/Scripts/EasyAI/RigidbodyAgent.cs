using UnityEngine;

namespace EasyAI
{
    /// <summary>
    /// Agent which moves through a rigidbody.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyAgent : Agent
    {
        /// <summary>
        /// This agent's rigidbody.
        /// </summary>
        private Rigidbody _rigidbody;

        protected override void Start()
        {
            base.Start();
        
            // Get the rigidbody.
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            // Since rotation is all done with the root visuals transform, freeze rigidbody rotation.
            _rigidbody.freezeRotation = true;
            _rigidbody.drag = 0;
            _rigidbody.angularDrag = 0;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.isKinematic = false;
        }
        
        public override void MovementCalculations()
        {
            if (_rigidbody == null)
            {
                return;
            }
        
            CalculateMoveVelocity(Time.fixedDeltaTime);
            _rigidbody.velocity = new(MoveVelocity.x, _rigidbody.velocity.y, MoveVelocity.y);
        }
    }
}