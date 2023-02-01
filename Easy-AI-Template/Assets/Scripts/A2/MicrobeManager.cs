using System.Linq;
using A2.Pickups;
using EasyAI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace A2
{
    /// <summary>
    /// Agent manager with additional fields for handling microbes for assignment two.
    /// </summary>
    [DisallowMultipleComponent]
    public class MicrobeManager : Manager
    {
        /// <summary>
        /// Identifier for the type (color) of microbes.
        /// </summary>
        public enum MicrobeType
        {
            Red = 0,
            Orange,
            Yellow,
            Green,
            Blue,
            Purple,
            Pink
        }

        /// <summary>
        /// The maximum number of microbes there can be.
        /// </summary>
        public static int MaxMicrobes => MicrobeSingleton.maxMicrobes;

        /// <summary>
        /// The chance that a microbe will increase in hunger every tick.
        /// </summary>
        public static float HungerChance => MicrobeSingleton.hungerChance;

        /// <summary>
        /// The hunger restored from eating a microbe.
        /// </summary>
        public static int HungerRestoredFromEating => MicrobeSingleton.hungerRestoredFromEating;

        /// <summary>
        /// How close microbes must be to interact.
        /// </summary>
        public static float MicrobeInteractRadius => MicrobeSingleton.microbeInteractRadius;

        /// <summary>
        /// How close microbes must be to interact.
        /// </summary>
        public static Material RedMicrobeMaterial => MicrobeSingleton.redMicrobeMaterial;

        /// <summary>
        /// Material to apply for orange microbes.
        /// </summary>
        public static Material OrangeMicrobeMaterial => MicrobeSingleton.orangeMicrobeMaterial;

        /// <summary>
        /// Material to apply for yellow microbes.
        /// </summary>
        public static Material YellowMicrobeMaterial => MicrobeSingleton.yellowMicrobeMaterial;

        /// <summary>
        /// Material to apply for green microbes.
        /// </summary>
        public static Material GreenMicrobeMaterial => MicrobeSingleton.greenMicrobeMaterial;

        /// <summary>
        /// Material to apply for blue microbes.
        /// </summary>
        public static Material BlueMicrobeMaterial => MicrobeSingleton.blueMicrobeMaterial;

        /// <summary>
        /// Material to apply for purple microbes.
        /// </summary>
        public static Material PurpleMicrobeMaterial => MicrobeSingleton.purpleMicrobeMaterial;

        /// <summary>
        /// Material to apply for pink microbes.
        /// </summary>
        public static Material PinkMicrobeMaterial => MicrobeSingleton.pinkMicrobeMaterial;

        /// <summary>
        /// Material to apply to the microbe state indicator when sleeping.
        /// </summary>
        public static Material SleepingIndicatorMaterial => MicrobeSingleton.sleepingIndicatorMaterial;

        /// <summary>
        /// Material to apply to the microbe state indicator when sleeping.
        /// </summary>
        public static Material FoodIndicatorMaterial => MicrobeSingleton.foodIndicatorMaterial;

        /// <summary>
        /// Material to apply to the microbe state indicator when seeking a mate.
        /// </summary>
        public static Material MateIndicatorMaterial => MicrobeSingleton.mateIndicatorMaterial;

        /// <summary>
        /// Material to apply to the microbe state indicator when seeking a pickup.
        /// </summary>
        public static Material PickupIndicatorMaterial => MicrobeSingleton.pickupIndicatorMaterial;

        /// <summary>
        /// Prefab for the death particles object.
        /// </summary>
        public static GameObject DeathParticlesPrefab => MicrobeSingleton.deathParticlesPrefab;

        /// <summary>
        /// Prefab for the mate particles object.
        /// </summary>
        public static GameObject MateParticlesPrefab => MicrobeSingleton.mateParticlesPrefab;

        /// <summary>
        /// Prefab for the pickup particles object.
        /// </summary>
        public static GameObject PickupParticlesPrefab => MicrobeSingleton.pickupParticlesPrefab;

        /// <summary>
        /// The hunger to start microbes at.
        /// </summary>
        public static int StartingHunger => MicrobeSingleton.startingHunger;

        /// <summary>
        /// The radius of the floor.
        /// </summary>
        public static float FloorRadius => MicrobeSingleton.floorRadius;

        /// <summary>
        /// Random position in the level.
        /// </summary>
        public static Vector2 RandomPosition => Random.insideUnitCircle * MicrobeSingleton.floorRadius;

        /// <summary>
        /// The score for each microbe for every second it has been alive.
        /// </summary>
        public static float ScoreSeconds => MicrobeSingleton.scoreSeconds;

        /// <summary>
        /// The score for each offspring the microbe has had since it has been alive.
        /// </summary>
        public static float ScoreOffspring => MicrobeSingleton.scoreOffspring;

        /// <summary>
        /// All microbes in the scene.
        /// </summary>
        public static Microbe[] Microbes => Singleton.Agents.Where(a => a is Microbe).Cast<Microbe>().ToArray();
        
        /// <summary>
        /// Access to the singleton directly as a microbe manager.
        /// </summary>
        private static MicrobeManager MicrobeSingleton => Singleton as MicrobeManager;

        [Header("Microbe Parameters")]
        [Tooltip("The hunger to start microbes at.")]
        [SerializeField]
        private int startingHunger = -100;

        [Tooltip("The maximum hunger before a microbe dies of starvation.")]
        [SerializeField]
        private int maxHunger = 200;

        [Tooltip("The hunger restored from eating a microbe.")]
        [Min(1)]
        [SerializeField]
        private int hungerRestoredFromEating = 100;

        [Tooltip("The radius of the floor.")]
        [Min(0)]
        [SerializeField]
        private float floorRadius = 10f;

        [Tooltip("The minimum number of microbes there must be.")]
        [Min(2)]
        [SerializeField]
        private int minMicrobes = 10;

        [Tooltip("The maximum number of microbes there can be.")]
        [Min(2)]
        [SerializeField]
        private int maxMicrobes = 30;

        [Tooltip("The number of pickups present in the level at any time.")]
        [Min(0)]
        [SerializeField]
        private int activePickups = 5;

        [Tooltip("The chance that a new microbe could randomly spawn every tick.")]
        [Min(0)]
        [SerializeField]
        private float randomSpawnChance;

        [Tooltip("The slowest speed a microbe can have.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float minMicrobeSpeed = 5f;

        [Tooltip("The fastest speed a microbe can have.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float maxMicrobeSpeed = 10f;

        [Tooltip("The shortest lifespan a microbe can have.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float minMicrobeLifespan = 20f;

        [Tooltip("The longest lifespan a microbe can have.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float maxMicrobeLifespan = 30f;

        [Tooltip("The maximum number of offspring microbes can have when mating.")]
        [Min(1)]
        [SerializeField]
        private int maxOffspring = 4;

        [Tooltip("How close microbes must be to interact.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float microbeInteractRadius = 1;

        [Tooltip("How small to make newborn microbes.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float minMicrobeSize = 0.25f;

        [Tooltip("How large to make fully grown microbes.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float maxMicrobeSize = 1;

        [Tooltip("The minimum distance a microbe can detect up to.")]
        [Min(0)]
        [SerializeField]
        private float minMicrobeDetectionRange = 5;
        
        [Tooltip("The chance that a microbe will increase in hunger every tick.")]
        [Min(0)]
        [SerializeField]
        private float hungerChance = 0.05f;

        [Header("Performance Scores")]
        [Tooltip("The score for each microbe for every second it has been alive.")]
        [Min(0)]
        [SerializeField]
        private float scoreSeconds = 1;
        
        [Tooltip("The score for each offspring the microbe has had since it has been alive.")]
        [Min(0)]
        [SerializeField]
        private float scoreOffspring = 10;
        
        [Header("Prefabs")]
        [Tooltip("Prefab for the microbes.")]
        [SerializeField]
        private GameObject microbePrefab;

        [Tooltip("Prefabs for pickups.")]
        [SerializeField]
        private GameObject[] pickupPrefabs;

        [Tooltip("Prefab for the spawn particles object.")]
        [SerializeField]
        private GameObject spawnParticlesPrefab;

        [Tooltip("Prefab for the death particles object.")]
        [SerializeField]
        private GameObject deathParticlesPrefab;

        [Tooltip("Prefab for the mate particles object.")]
        [SerializeField]
        private GameObject mateParticlesPrefab;

        [Tooltip("Prefab for the pickup particles object.")]
        [SerializeField]
        private GameObject pickupParticlesPrefab;

        [Header("Materials")]
        [Tooltip("Material to apply to the floor.")]
        [SerializeField]
        private Material floorMaterial;

        [Tooltip("Material to apply for red microbes.")]
        [SerializeField]
        private Material redMicrobeMaterial;

        [Tooltip("Material to apply for orange microbes.")]
        [SerializeField]
        private Material orangeMicrobeMaterial;

        [Tooltip("Material to apply for yellow microbes.")]
        [SerializeField]
        private Material yellowMicrobeMaterial;

        [Tooltip("Material to apply for green microbes.")]
        [SerializeField]
        private Material greenMicrobeMaterial;

        [Tooltip("Material to apply for blue microbes.")]
        [SerializeField]
        private Material blueMicrobeMaterial;

        [Tooltip("Material to apply for purple microbes.")]
        [SerializeField]
        private Material purpleMicrobeMaterial;

        [Tooltip("Material to apply for pink microbes.")]
        [SerializeField]
        private Material pinkMicrobeMaterial;

        [Tooltip("Material to apply to the microbe state indicator when sleeping.")]
        [SerializeField]
        private Material sleepingIndicatorMaterial;

        [Tooltip("Material to apply to the microbe state indicator when seeking food.")]
        [SerializeField]
        private Material foodIndicatorMaterial;

        [Tooltip("Material to apply to the microbe state indicator when seeking a mate.")]
        [SerializeField]
        private Material mateIndicatorMaterial;

        [Tooltip("Material to apply to the microbe state indicator when seeking a pickup.")]
        [SerializeField]
        private Material pickupIndicatorMaterial;

        /// <summary>
        /// Mate two microbes.
        /// </summary>
        /// <param name="parentA">First parent.</param>
        /// <param name="parentB">Second parent.</param>
        /// <returns>The number of offspring spawned.</returns>
        public static int Mate(Microbe parentA, Microbe parentB)
        {
            int born;
            
            // Spawn between the two parents.
            Vector3 position = (parentA.transform.position + parentB.transform.position) / 2;
            for (born = 0; born < MicrobeSingleton.maxOffspring && MicrobeSingleton.Agents.Count < MicrobeSingleton.maxMicrobes; born++)
            {
                SpawnMicrobe(
                    // Inherit the color from either parent.
                    Random.value <= 0.5f ? parentA.MicrobeType : parentB.MicrobeType,
                    position,
                    // Inherit the average speed of both parents offset by a slight random value.
                    Mathf.Clamp((parentA.MoveSpeed + parentB.MoveSpeed) / 2 + Random.value - 0.5f, MicrobeSingleton.minMicrobeSpeed, MicrobeSingleton.maxMicrobeSpeed),
                    // Inherit the average lifespan of both parents offset by a slight random value.
                    Mathf.Clamp((parentA.LifeSpan + parentB.LifeSpan) / 2 + Random.value - 0.5f, MicrobeSingleton.minMicrobeLifespan, MicrobeSingleton.maxMicrobeLifespan),
                    // Inherit the average detection range of both parents offset by a slight random value.
                    Mathf.Clamp((parentA.DetectionRange + parentB.DetectionRange) / 2 + Random.value - 0.5f, MicrobeSingleton.minMicrobeDetectionRange, MicrobeSingleton.floorRadius * 2));
            }

            if (born == 0)
            {
                return 0;
            }

            Instantiate(MateParticlesPrefab, position, Quaternion.Euler(270, 0, 0));
            parentA.HadOffspring(born);
            if (parentA != parentB)
            {
                parentB.HadOffspring(born);
            }

            return born;
        }

        protected override void Start()
        {
            // Generate the floor.
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Destroy(floor.GetComponent<Collider>());
            floor.transform.position = new(0, -1, 0);
            floor.transform.localScale = new(floorRadius * 2 + 3, 1, floorRadius * 2 + 3);
            floor.name = "Floor";
            floor.GetComponent<MeshRenderer>().material = floorMaterial;

            // Spawn initial agents.
            ResetAgents();
            
            // Spawn pickups.
            for (int i = FindObjectsOfType<MicrobeBasePickup>().Length; i < activePickups; i++)
            {
                SpawnPickup();
            }

            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            // Loop through all microbes.
            for (int i = 0; i < Agents.Count; i++)
            {
                // There should never be any that are not microbes but check just in case.
                if (Agents[i] is not Microbe microbe)
                {
                    continue;
                }

                // Increment the lifespan.
                microbe.Age();

                // If a microbe has not starved, not died of old age, and has not gone out of bounds, update its size to reflect its age.
                if (microbe.Hunger <= maxHunger && microbe.ElapsedLifespan < microbe.LifeSpan && Vector3.Distance(Agents[i].transform.position, Vector3.zero) <= floorRadius + 2)
                {
                    if (Agents[i].Visuals != null)
                    {
                        float scale = microbe.ElapsedLifespan / microbe.LifeSpan * (maxMicrobeSize - minMicrobeSize) + minMicrobeSize;
                        Agents[i].Visuals.localScale = new(scale, scale, scale);
                    }

                    continue;
                }

                // Otherwise, kill the microbe.
                microbe.Die();
                i--;
            }

            // Ensure there are enough microbes in the level.
            while (Agents.Count < minMicrobes)
            {
                SpawnMicrobe();
            }

            // Randomly spawn microbes.
            if (randomSpawnChance > 0)
            {
                for (int i = Agents.Count; i < maxMicrobes; i++)
                {
                    if (Random.value <= randomSpawnChance)
                    {
                        SpawnMicrobe();
                    }
                }
            }

            // Ensure there are enough pickups in the level.
            for (int i = FindObjectsOfType<MicrobeBasePickup>().Length; i < activePickups; i++)
            {
                SpawnPickup();
            }
        }
        
        /// <summary>
        /// Render buttons to regenerate the floor or change its size..
        /// </summary>
        /// <param name="x">X rendering position. In most cases this should remain unchanged.</param>
        /// <param name="y">Y rendering position. Update this with every component added and return it.</param>
        /// <param name="w">Width of components. In most cases this should remain unchanged.</param>
        /// <param name="h">Height of components. In most cases this should remain unchanged.</param>
        /// <param name="p">Padding of components. In most cases this should remain unchanged.</param>
        /// <returns>The updated Y position after all custom rendering has been done.</returns>
        protected override float CustomRendering(float x, float y, float w, float h, float p)
        {
            // Regenerate the floor button.
            if (GuiButton(x, y, w, h, "Reset"))
            {
                ResetAgents();
                ClearMessages();
            }
            
            return NextItem(y, h, p);
        }

        /// <summary>
        /// Reset all agents.
        /// </summary>
        private void ResetAgents()
        {
            for (int i = Agents.Count - 1; i >= 0; i--)
            {
                Destroy(Agents[i].gameObject);
            }
            
            for (int i = 0; i < minMicrobes; i++)
            {
                SpawnMicrobe();
            }
        }

        /// <summary>
        /// Spawn a microbe completely randomly.
        /// </summary>
        private void SpawnMicrobe()
        {
            SpawnMicrobe((MicrobeType) Random.Range((int) MicrobeType.Red, (int) MicrobeType.Pink + 1));
        }

        /// <summary>
        /// Spawn a microbe with a given type/color but everything else random.
        /// </summary>
        /// <param name="microbeType">The type for the microbe.</param>
        private void SpawnMicrobe(MicrobeType microbeType)
        {
            Vector3 position = Random.insideUnitSphere * floorRadius;
            position = new(position.x, 0, position.z);
            
            SpawnMicrobe(microbeType, position);
        }

        /// <summary>
        /// Spawn a microbe with a given type/color at a set position but everything else random.
        /// </summary>
        /// <param name="microbeType">The type for the microbe.</param>
        /// <param name="position">The position of the microbe.</param>
        private void SpawnMicrobe(MicrobeType microbeType, Vector3 position)
        {
            SpawnMicrobe(microbeType, position, Random.Range(minMicrobeSpeed, maxMicrobeSpeed), Random.Range(minMicrobeLifespan, maxMicrobeLifespan), Random.Range(minMicrobeDetectionRange, floorRadius * 2));
        }

        /// <summary>
        /// Spawn a microbe.
        /// </summary>
        /// <param name="microbeType">The type for the microbe.</param>
        /// <param name="position">The position of the microbe.</param>
        /// <param name="moveSpeed">The speed the microbe will move at.</param>
        /// <param name="lifespan">How long the microbe can live.</param>
        /// <param name="detectionRange">How far away microbes can detect others and pickups from.</param>
        private static void SpawnMicrobe(MicrobeType microbeType, Vector3 position, float moveSpeed, float lifespan, float detectionRange)
        {
            if (MicrobeSingleton.Agents.Count >= MicrobeSingleton.maxMicrobes)
            {
                return;
            }
            
            // Setup the microbe.
            GameObject go = Instantiate(MicrobeSingleton.microbePrefab, position, Quaternion.identity);
            Microbe microbe = go.GetComponent<Microbe>();
            if (microbe == null)
            {
                return;
            }

            microbe.MicrobeType = microbeType;
            microbe.SetHunger(MicrobeSingleton.startingHunger);
            microbe.SetLifeSpan(lifespan);
            microbe.SetDetectionRange(detectionRange);
            microbe.SetMoveSpeed(moveSpeed);

            // Setup the microbe name.
            string n = microbeType switch
            {
                MicrobeType.Red => "Red",
                MicrobeType.Orange => "Orange",
                MicrobeType.Yellow => "Yellow",
                MicrobeType.Green => "Green",
                MicrobeType.Blue => "Blue",
                MicrobeType.Purple => "Purple",
                _ => "Pink"
            };

            Agent[] coloredMicrobes = MicrobeSingleton.Agents.Where(a => a is Microbe m && m.MicrobeType == microbeType && m != microbe).ToArray();
            if (coloredMicrobes.Length == 0)
            {
                microbe.name = $"{n} 1";
            }

            for (int i = 1;; i++)
            {
                if (coloredMicrobes.Any(m => m.name == $"{n} {i}"))
                {
                    continue;
                }

                n = $"{n} {i}";
                microbe.name = n;
                break;
            }
            
            SortAgents();
            GlobalLog($"Spawned microbe {n}.");
            Instantiate(MicrobeSingleton.spawnParticlesPrefab, microbe.transform.position, Quaternion.Euler(270, 0, 0));
        }

        /// <summary>
        /// Spawn a pickup.
        /// </summary>
        private static void SpawnPickup()
        {
            Vector3 position = Random.insideUnitSphere * MicrobeSingleton.floorRadius;
            position = new(position.x, 0, position.z);
            
            GameObject go = Instantiate(MicrobeSingleton.pickupPrefabs[Random.Range(0, MicrobeSingleton.pickupPrefabs.Length)], position, Quaternion.identity);

            go.transform.localScale = new(0.5f, 1, 0.5f);
        }
    }
}