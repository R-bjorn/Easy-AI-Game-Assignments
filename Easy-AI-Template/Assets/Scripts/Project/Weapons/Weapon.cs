using System.Collections;
using UnityEngine;

namespace Project.Weapons
{
    /// <summary>
    /// Base class for weapons.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Store info on what enemies were attacked and how many hits they received.
        /// </summary>
        protected struct AttackedInfo
        {
            /// <summary>
            /// The soldier that was attacked.
            /// </summary>
            public Soldier Attacked;

            /// <summary>
            /// How many hits they took.
            /// </summary>
            public int Hits;
        }

        [Tooltip("The maximum ammo of the weapon, setting to less than 0 will give unlimited ammo.")]
        [SerializeField]
        private int maxAmmo = -1;

        /// <summary>
        /// The weapon index on the soldier.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The soldier using this weapon.
        /// </summary>
        public Soldier Soldier { get; set; }

        /// <summary>
        /// If the weapon is ready to shoot.
        /// </summary>
        public bool CanShoot { get; private set; } = true;

        /// <summary>
        /// How much ammo the weapon has.
        /// </summary>
        public int Ammo { get; private set; }

        public int MaxAmmo => maxAmmo;

        public float MoveSpeed => moveSpeed;

        public float RotationSpeed => rotationSpeed;

        [Tooltip("The sound to make upon bullet impact.")]
        [SerializeField]
        private AudioClip impactSound;

        [Tooltip("The effect prefab to show upon bullet impact.")]
        [SerializeField]
        private GameObject impactEffectPrefab;
        
        [Tooltip("The barrel of the weapon.")]
        [SerializeField]
        protected Transform barrel;
        
        [Tooltip("How much damage the weapon should do.")]
        [Min(1)]
        [SerializeField]
        protected int damage;
        
        [Tooltip("How long between shots should there be.")]
        [Min(0)]
        [SerializeField]
        protected float delay;
        
        [Tooltip("How long bullet trails or projectiles last for.")]
        [Min(0)]
        [SerializeField]
        protected float time;

        [Tooltip("How fast an agent can move when using this weapon.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float moveSpeed = 10;

        [Tooltip("How fast an agent can rotate when using this weapon.")]
        [Min(0)]
        [SerializeField]
        private float rotationSpeed = 30;

        [Tooltip("How far away shots can be heard by agents.")]
        [Min(0)]
        [SerializeField]
        private float soundRange;
        
        /// <summary>
        /// The visuals of the weapon.
        /// </summary>
        private MeshRenderer[] _renderers;

        /// <summary>
        /// The sound to play when shooting.
        /// </summary>
        private AudioSource _shootSound;

        /// <summary>
        /// Play audio on impact.
        /// </summary>
        /// <param name="p">The position to play the audio.</param>
        /// <param name="numImpacts">The number of impacts.</param>
        public void ImpactAudio(Vector3 p, int numImpacts)
        {
            // Create the object to hold the sound at the set position.
            GameObject impactObj = new($"{name} Audio")
            {
                transform =
                {
                    position = p
                }
            };
            
            // Add and play the audio.
            AudioSource impact = impactObj.AddComponent<AudioSource>();
            impact.clip = impactSound;
            impact.volume = SoldierManager.Volume / numImpacts;
            impact.spatialBlend = 1;
            impact.dopplerLevel = _shootSound.dopplerLevel;
            impact.spread = _shootSound.spread;
            impact.rolloffMode = _shootSound.rolloffMode;
            impact.minDistance = _shootSound.minDistance;
            impact.maxDistance = _shootSound.maxDistance;
            impact.Play();
            
            // Destroy once the audio is done.
            Destroy(impactObj, impactSound.length);
        }

        /// <summary>
        /// Display impact visuals.
        /// </summary>
        /// <param name="p">The position to show the effects.</param>
        /// <param name="lookAt">The position to look at.</param>
        public void ImpactVisual(Vector3 p, Vector3 lookAt)
        {
            // Create the effect.
            GameObject effect = Instantiate(impactEffectPrefab, p, Quaternion.identity);
            effect.name = $"{name} Effect";
            
            // Randomly rotate it if no position given, otherwise look at it.
            if (lookAt == Vector3.zero)
            {
                effect.transform.rotation = Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360));
            }
            else
            {
                effect.transform.LookAt(lookAt);
            }
            
            // Destroy once the effect is done.
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        }

        /// <summary>
        /// Replenish ammo.
        /// </summary>
        public void Replenish()
        {
            Ammo = maxAmmo;
        }
        
        /// <summary>
        /// Toggle visibility.
        /// </summary>
        /// <param name="visible">If the weapon is visible or not.</param>
        public void Visible(bool visible)
        {
            _renderers ??= GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer meshRenderer in _renderers)
            {
                meshRenderer.enabled = visible;
            }
        }
        
        /// <summary>
        /// Shoot the weapon.
        /// </summary>
        public void Shoot()
        {
            // If this is not the selected weapon of the soldier, do not shoot.
            if (Index != Soldier.WeaponIndex || !CanShoot)
            {
                return;
            }
            
            Soldier.Log((Soldier.WeaponIndexes) Soldier.WeaponIndex switch
            {
                Soldier.WeaponIndexes.MachineGun => "Shooting machine gun.",
                Soldier.WeaponIndexes.Shotgun => "Shooting shotgun.",
                Soldier.WeaponIndexes.Sniper => "Shooting sniper.",
                Soldier.WeaponIndexes.RocketLauncher => "Shooting rocket launcher.",
                _=> "Shooting pistol."
            });

            // Shoot and display visuals.
            Shoot(out Vector3[] positions);
            ShootVisuals(positions);
            
            // Delay being able to shoot again.
            StartDelay();
            
            // See if any enemies can hear this weapon.
            foreach (Soldier enemy in Soldier.GetEnemies())
            {
                enemy.Hear(Soldier, soundRange);
            }
        }

        /// <summary>
        /// Implement shooting behaviour.
        /// </summary>
        /// <param name="positions">Impact positions.</param>
        protected abstract void Shoot(out Vector3[] positions);

        /// <summary>
        /// Implement firing visuals which by default only plays audio.
        /// </summary>
        /// <param name="positions">The positions to show effects.</param>
        protected virtual void ShootVisuals(Vector3[] positions)
        {
            _shootSound.Play();
        }

        protected virtual void Awake()
        {
            // Ensure weapon is loaded to start.
            Replenish();
        }

        private void Start()
        {
            // Ensure volume is good.
            _shootSound = GetComponent<AudioSource>();
            _shootSound.volume = SoldierManager.Volume;
        }

        /// <summary>
        /// Ensure a time delay between shots.
        /// </summary>
        private void StartDelay()
        {
            // Reduce ammo count, unless if the weapon has unlimited ammo.
            if (Ammo > 0)
            {
                Ammo--;
            }
            
            // Start the delay.
            StartCoroutine(ShotDelay());
        }
        
        /// <summary>
        /// Wait before the next shot.
        /// </summary>
        /// <returns>Nothing.</returns>
        private IEnumerator ShotDelay()
        {
            CanShoot = false;
            yield return new WaitForSeconds(delay);
            CanShoot = true;
        }
    }
}