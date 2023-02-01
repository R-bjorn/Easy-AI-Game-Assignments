using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Weapons
{
    /// <summary>
    /// Raycast/hit scan weapon.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class RaycastWeapon : Weapon
    {
        [Tooltip("The material for the bullet trail.")]
        [SerializeField]
        private Material material;

        [Tooltip("How many rounds to fire each shot.")]
        [Min(1)]
        [SerializeField]
        private int rounds = 1;

        [Tooltip("How much spread should the shots have.")]
        [Range(0, 1)]
        [SerializeField]
        private float spread;

        /// <summary>
        /// The layer mask for hits.
        /// </summary>
        private LayerMask _layerMask;

        /// <summary>
        /// Shoot hit scan rounds.
        /// </summary>
        /// <param name="positions">Where the shots hit.</param>
        protected override void Shoot(out Vector3[] positions)
        {
            positions = new Vector3[rounds];

            // Get the direction to shoot in.
            Vector3 forward = Soldier.shootPosition.TransformDirection(Vector3.forward);

            // Hold what soldiers get attacked.
            List<AttackedInfo> attackedInfos = new();

            // Shoot all shots.
            for (int i = 0; i < rounds; i++)
            {
                // Randomly spread the shot.
                Vector3 direction = forward + new Vector3(
                    Random.Range(-spread, spread),
                    Random.Range(-spread, spread),
                    Random.Range(-spread, spread)
                );
                direction.Normalize();
                
                // Take the shot.
                if (!Physics.Raycast(Soldier.shootPosition.position, direction, out RaycastHit hit, Mathf.Infinity, _layerMask))
                {
                    // If the shot doesn't hit, shoot off into the distance.
                    positions[i] = Soldier.shootPosition.position + direction * 1000;
                    continue;
                }

                // Add the hit point.
                positions[i] = hit.point;

                // Look to see if a soldier was hit.
                Soldier attacked;
                Transform tr = hit.collider.transform;
                do
                {
                    attacked = tr.GetComponent<Soldier>();
                    tr = tr.parent;
                } while (attacked == null && tr != null);

                // If no soldier was hit or they are on the same team, continue to the next shot.
                if (attacked == null || attacked.RedTeam == Soldier.RedTeam)
                {
                    continue;
                }

                // See if this enemy has already been hit and if so increment the number of hits it took.
                bool found = false;
                for (int j = 0; j < attackedInfos.Count; j++)
                {
                    if (attackedInfos[j].Attacked != attacked)
                    {
                        continue;
                    }

                    AttackedInfo attackedInfo = attackedInfos[j];
                    attackedInfo.Hits++;
                    attackedInfos[j] = attackedInfo;
                    found = true;
                    break;
                }

                // Otherwise, add a new item to store its hits.
                if (!found)
                {
                    attackedInfos.Add(new() {Attacked = attacked, Hits = 1});
                }
            }

            // Damage all enemies that were hit.
            foreach (AttackedInfo attackedInfo in attackedInfos)
            {
                attackedInfo.Attacked.Damage(damage * attackedInfo.Hits, Soldier);
            }
        }

        /// <summary>
        /// Add in more visuals for the hit scan impacts.
        /// </summary>
        /// <param name="positions">The impact positions.</param>
        protected override void ShootVisuals(Vector3[] positions)
        {
            // Go through every impact position.
            foreach (Vector3 v in positions)
            {
                // Add a bullet trail.
                GameObject bullet = new($"{name} Trail");
                LineRenderer lr = bullet.AddComponent<LineRenderer>();
                lr.material = material;
                lr.startColor = lr.endColor = material.color;
                lr.startWidth = lr.endWidth = 0.025f;
                lr.numCornerVertices = lr.numCapVertices = 90;
                lr.positionCount = 2;
                Vector3 barrelPosition = barrel.position;
                lr.SetPositions(new [] { barrelPosition, v });
                StartCoroutine(FadeLine(lr));

                // Play audio and create impact effect.
                ImpactAudio(v, positions.Length);
                ImpactVisual(v, barrelPosition);
            }
            
            base.ShootVisuals(positions);
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            // Ensure the bullet trail time does not exceed the shot delay time.
            if (time > delay)
            {
                time = delay;
            }
            
            // Define the layer mask that shots can hit.
            _layerMask = LayerMask.GetMask("Default", "Obstacle", "Ground", "Projectile", "HitBox");
        }
        
        /// <summary>
        /// Fade the bullet trail over time.
        /// </summary>
        /// <param name="lr">The line renderer itself.</param>
        /// <returns>Nothing.</returns>
        private IEnumerator FadeLine(LineRenderer lr)
        {
            // Get variables.
            Material mat = lr.material;
            Color color = mat.color;
            float startAlpha = lr.startColor.a;
            float duration = 0;
            
            // Loop until fully faded.
            while (duration < 1)
            {
                float alpha = startAlpha * (1 - duration);
                mat.color = new(color.r, color.g, color.b, alpha);
                duration += Time.deltaTime / time;
                yield return null;
            }
            
            // Destroy the bullet trail.
            Destroy(lr.gameObject);
        }
    }
}