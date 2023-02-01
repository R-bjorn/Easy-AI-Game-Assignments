using UnityEngine;

namespace Project.Weapons
{
    /// <summary>
    /// Projectile weapon.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class ProjectileWeapon : Weapon
    {
        [Tooltip("How fast the projectile should travel.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float velocity = 10;

        [Tooltip("Splash damage distance.")]
        [Min(0)]
        [SerializeField]
        private float distance;
        
        [Tooltip("The bullet prefab.")]
        [SerializeField]
        private GameObject bulletPrefab;
        
        /// <summary>
        /// Fire projectile.
        /// </summary>
        /// <param name="positions">Only returns the weapon barrel.</param>
        protected override void Shoot(out Vector3[] positions)
        {
            // No hit scan impacts so just return the barrel.
            positions = new[] { barrel.position };

            // Create the projectile.
            GameObject projectile = Instantiate(bulletPrefab, Soldier.shootPosition.position, barrel.rotation);
            projectile.name = $"{name} Projectile";
            ProjectileBullet projectileBullet = projectile.GetComponent<ProjectileBullet>();
            projectileBullet.WeaponIndex = Index;
            projectileBullet.Shooter = Soldier;
            projectileBullet.Damage = damage;
            projectileBullet.Distance = distance;
            projectileBullet.Velocity = velocity;
            
            // Ensure the projectile destroys after its max time.
            Destroy(projectile, time);
        }
    }
}