using System.Collections.Generic;
using System.Linq;
using EasyAI;
using Project.Pickups;
using Project.Positions;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// Manager for soldiers.
    /// </summary>
    public class SoldierManager : Manager
    {
        /// <summary>
        /// How much health each soldier has.
        /// </summary>
        public static int Health => SoldierSingleton.health;

        /// <summary>
        /// How many seconds soldiers need to wait to respawn.
        /// </summary>
        public static float Respawn => SoldierSingleton.respawn;

        /// <summary>
        /// How many seconds before a pickup can be used again.
        /// </summary>
        public static float PickupTimer => SoldierSingleton.pickupTimer;

        /// <summary>
        /// How many seconds before an old seen or hear enemy is removed from memory.
        /// </summary>
        public static float MemoryTime => SoldierSingleton.memoryTime;

        /// <summary>
        /// How loud the audio is.
        /// </summary>
        public static float Volume => SoldierSingleton.volume;

        /// <summary>
        /// The material to apply to the red soldiers.
        /// </summary>
        public static Material Red => SoldierSingleton.red;

        /// <summary>
        /// The material to apply to the blue soldiers.
        /// </summary>
        public static Material Blue => SoldierSingleton.blue;

        /// <summary>
        /// The flags captured by the red team.
        /// </summary>
        public static int CapturedRed => SoldierSingleton._capturedRed;

        /// <summary>
        /// The flags captured by the blue team.
        /// </summary>
        public static int CapturedBlue => SoldierSingleton._capturedBlue;

        /// <summary>
        /// The total kills by the red team.
        /// </summary>
        public static int KillsRed => SoldierSingleton._killsRed;

        /// <summary>
        /// The total kills by the blue team.
        /// </summary>
        public static int KillsBlue => SoldierSingleton._killsBlue;

        /// <summary>
        /// The spawn points for soldiers.
        /// </summary>
        public static IEnumerable<SpawnPoint> SpawnPoints => SoldierSingleton._spawnPoints;
        
        /// <summary>
        /// How much score each flag capture is worth for a soldier's performance.
        /// </summary>
        public static int ScoreCapture => SoldierSingleton.scoreCapture;

        /// <summary>
        /// How much score each flag return is worth for a soldier's performance.
        /// </summary>
        public static int ScoreReturn => SoldierSingleton.scoreReturn;

        /// <summary>
        /// How much score each kill gains for a soldier and each death loses.
        /// </summary>
        public static int ScoreKillsDeaths => SoldierSingleton.scoreKillsDeaths;
        
        /// <summary>
        /// All soldiers on the red team.
        /// </summary>
        public static List<Soldier> TeamRed => SoldierSingleton._teamRed;
        
        /// <summary>
        /// All soldiers on the blue team.
        /// </summary>
        public static List<Soldier> TeamBlue => SoldierSingleton._teamBlue;
        
        /// <summary>
        /// Cast the Manager singleton into a SoldierManager.
        /// </summary>
        private static SoldierManager SoldierSingleton => Singleton as SoldierManager;

        [Tooltip("How many soldiers to have on each team.")]
        [Range(1, 15)]
        [SerializeField]
        private int soldiersPerTeam = 3;

        [Header("Match Settings")]
        [Tooltip("How much health each soldier has.")]
        [Min(1)]
        [SerializeField]
        private int health = 100;

        [Tooltip("How many seconds soldiers need to wait to respawn.")]
        [Min(0)]
        [SerializeField]
        private float respawn = 10;

        [Tooltip("How many seconds before a pickup can be used again.")]
        [Min(0)]
        [SerializeField]
        private float pickupTimer = 10;

        [Tooltip("How many seconds before an old seen or hear enemy is removed from memory.")]
        [Min(0)]
        [SerializeField]
        private float memoryTime = 5;

        [Tooltip("How loud the audio is.")]
        [Range(0, 1)]
        [SerializeField]
        private float volume;

        [Header("Performance Scores")]
        [Tooltip("How much score each flag capture is worth for a soldier's performance.")]
        [Min(0)]
        [SerializeField]
        private int scoreCapture = 10;

        [Tooltip("How much score each flag return is worth for a soldier's performance.")]
        [Min(0)]
        [SerializeField]
        private int scoreReturn = 5;

        [Tooltip("How much score each kill gains for a soldier and each death loses.")]
        [Min(0)]
        [SerializeField]
        private int scoreKillsDeaths = 1;

        [Header("Prefabs")]
        [Tooltip("The prefab for soldiers.")]
        [SerializeField]
        private GameObject soldierPrefab;

        [Header("Materials")]
        [Tooltip("The material to apply to the red soldiers.")]
        [SerializeField]
        private Material red;

        [Tooltip("The material to apply to the blue soldiers.")]
        [SerializeField]
        private Material blue;

        /// <summary>
        /// The flags captured by the red team.
        /// </summary>
        private int _capturedRed;

        /// <summary>
        /// The flags captured by the blue team.
        /// </summary>
        private int _capturedBlue;

        /// <summary>
        /// The total kills by the red team.
        /// </summary>
        private int _killsRed;

        /// <summary>
        /// The total kills by the blue team.
        /// </summary>
        private int _killsBlue;

        /// <summary>
        /// The spawn points for soldiers.
        /// </summary>
        private SpawnPoint[] _spawnPoints;

        /// <summary>
        /// All strategic positions for soldiers to use.
        /// </summary>
        private StrategicPoint[] _strategicPoints;

        /// <summary>
        /// All health and weapon pickups.
        /// </summary>
        private HealthAmmoPickup[] _healthWeaponPickups;
        
        /// <summary>
        /// All soldiers on the red team.
        /// </summary>
        private readonly List<Soldier> _teamRed = new();
        
        /// <summary>
        /// All soldiers on the blue team.
        /// </summary>
        private readonly List<Soldier> _teamBlue = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public static void CaptureFlag(FlagPickup flag)
        {
            if (flag.IsRedFlag)
            {
                SoldierSingleton._capturedBlue++;
            }
            else
            {
                SoldierSingleton._capturedRed++;
            }
            
            // Add the capture to the player.
            flag.carryingPlayer.Log("Captured the flag.");
            flag.carryingPlayer.Captures++;

            // Finally return the flag and reassign roles.
            Soldier soldier = flag.carryingPlayer;
            flag.ReturnFlag(null);
            soldier.AssignRoles();
        }

        /// <summary>
        /// Add a kill.
        /// </summary>
        /// <param name="shooter">The solider that got the kill.</param>
        /// <param name="killed">The soldier that got killed.</param>
        public static void AddKill(Soldier shooter, Soldier killed)
        {
            // Reset killed player stats.
            killed.Health = 0;
            killed.Deaths++;
            
            // Add a kill for the shooter.
            shooter.Kills++;
            
            // Add messages to each.
            killed.Log($"Killed by {shooter.name}.");
            shooter.Log($"Killed {killed.name}");
            
            // Add team score.
            if (shooter.RedTeam)
            {
                SoldierSingleton._killsRed++;
            }
            else
            {
                SoldierSingleton._killsBlue++;
            }

            // Start the respawn counter and reassign team roles as a team member has died.
            killed.StopAllCoroutines();
            killed.StartCoroutine(killed.Respawn());
        }

        /// <summary>
        /// Get a strategic point to move to.
        /// </summary>
        /// <param name="soldier">The soldier.</param>
        /// <param name="defensive">If this is for a defensive or offensive point.</param>
        /// <returns>A point to move to.</returns>
        public static Vector3 RandomStrategicPosition(Soldier soldier, bool defensive)
        {
            // Get all points for the team and for the given type.
            StrategicPoint[] points = SoldierSingleton._strategicPoints.Where(p => p.RedTeam == soldier.RedTeam && p.defensive == defensive).ToArray();
            
            // Get all open spots.
            StrategicPoint[] open = points.Where(s => s.Open).ToArray();
            
            // Move to an open spot if there is one, otherwise to a random point.
            return open.Length > 0 ? open[Random.Range(0, open.Length)].transform.position : points[Random.Range(0, points.Length)].transform.position;
        }

        /// <summary>
        /// Get a health pack to move to.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns>The health pack to move to or null if none are ready.</returns>
        public static HealthAmmoPickup NearestHealthPickup(Agent agent)
        {
            // A health pickup is just a weapon pickup with an index of -1, so simply return that.
            return NearestAmmoPickup(agent, -1);
        }

        /// <summary>
        /// Get an ammo pickup to move to.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="weaponIndex">The weapon type to look for.</param>
        /// <returns>The ammo pickup to move to or null if none are ready.</returns>
        public static HealthAmmoPickup NearestAmmoPickup(Agent agent, int weaponIndex)
        {
            // Get all pickups for the given type that can be picked up.
            HealthAmmoPickup[] ready = SoldierSingleton._healthWeaponPickups.Where(p => p.weaponIndex == weaponIndex && p.Ready).ToArray();
            
            // Get the nearest one if there are any, otherwise return null.
            return ready.Length > 0 ? ready.OrderBy(p => Vector3.Distance(agent.transform.position, p.transform.position)).First() : null;
        }

        /// <summary>
        /// Reset the level.
        /// </summary>
        private static void NewGame()
        {
            // Return the red flag.
            if (FlagPickup.RedFlag != null)
            {
                FlagPickup.RedFlag.ReturnFlag(null);
            }
            
            // Return the blue flag.
            if (FlagPickup.BlueFlag != null)
            {
                FlagPickup.BlueFlag.ReturnFlag(null);
            }

            // Enable every spawn point.
            foreach (SpawnPoint spawnPoint in SoldierSingleton._spawnPoints)
            {
                spawnPoint.Used = false;
            }
            
            // Reset every soldier.
            foreach (Soldier soldier in Singleton.Agents.Where(a => a is Soldier).Cast<Soldier>())
            {
                soldier.Spawn();
                soldier.Kills = 0;
                soldier.Deaths = 0;
                soldier.Captures = 0;
                soldier.Returns = 0;
            }
            
            ClearMessages();
            
            // Reset every pickup.
            foreach (HealthAmmoPickup pickup in SoldierSingleton._healthWeaponPickups)
            {
                pickup.StopAllCoroutines();
                pickup.Ready = true;
            }

            // Reset all values.
            SoldierSingleton._killsRed = 0;
            SoldierSingleton._killsBlue = 0;
            SoldierSingleton._capturedRed = 0;
            SoldierSingleton._capturedBlue = 0;
        }
        
        protected override void Start()
        {
            // Perform base agent manager setup.
            base.Start();

            // Get all points in the level.
            _spawnPoints = FindObjectsOfType<SpawnPoint>();
            _strategicPoints = FindObjectsOfType<StrategicPoint>();
            _healthWeaponPickups = FindObjectsOfType<HealthAmmoPickup>();

            // Spawn all soldiers.
            for (int i = 0; i < soldiersPerTeam * 2; i++)
            {
                Instantiate(soldierPrefab);
            }
        }

        protected override void Update()
        {
            // Perform base agent manager updates.
            base.Update();

            // Loop through every agent.
            foreach (Agent agent in Agents)
            {
                // Only perform on alive soldiers.
                if (agent is not Soldier { Alive: true } soldier)
                {
                    continue;
                }
                
                // Detect seen enemies and add them to memory.
                foreach (Soldier enemy in soldier.SeeEnemies())
                {
                    // If there is no existing memory, add it to memory.
                    Soldier.EnemyMemory memory = soldier.DetectedEnemies.FirstOrDefault(e => e.Enemy == enemy);
                    if (memory != null)
                    {
                        memory.DeltaTime = 0;
                        memory.Enemy = enemy;
                        memory.Position = enemy.headPosition.position;
                        memory.Visible = true;
                        memory.HasFlag = FlagPickup.RedFlag != null && FlagPickup.RedFlag.carryingPlayer == enemy || FlagPickup.BlueFlag != null && FlagPickup.BlueFlag.carryingPlayer == enemy;
                    }
                    // Otherwise, update the existing memory.
                    else
                    {
                        soldier.DetectedEnemies.Add(new()
                        {
                            DeltaTime = 0,
                            Enemy = enemy,
                            Position = enemy.headPosition.position,
                            Visible = true,
                            HasFlag = FlagPickup.RedFlag != null && FlagPickup.RedFlag.carryingPlayer == enemy || FlagPickup.BlueFlag != null && FlagPickup.BlueFlag.carryingPlayer == enemy
                        });
                    }

                    // If this enemy is not the soldier's current target, continue.
                    if (soldier.Target == null || soldier.Target.Value.Enemy != enemy)
                    {
                        continue;
                    }
                    
                    soldier.SetTarget(new()
                    {
                        Enemy = enemy,
                        Position = enemy.headPosition.position,
                        Visible = true
                    });
                }
            }

            int layerMask = LayerMask.GetMask("Default", "Obstacle", "Ground", "Projectile", "HitBox");

            // Loop through all agents again.
            foreach (Agent agent in Agents)
            {
                // Only perform for alive soldiers.
                if (agent is not Soldier { Alive: true } soldier)
                {
                    agent.StopMoving();
                    agent.StopNavigating();
                    agent.StopLooking();
                    if (agent == SelectedAgent)
                    {
                        Soldier[] aliveSoldiers = Agents.Where(a => a is Soldier {Alive: true, PerformanceMeasure: not null}).Cast<Soldier>().ToArray();
                        float best = float.MinValue;
                        foreach (Soldier s in aliveSoldiers)
                        {
                            float score = s.PerformanceMeasure.CalculatePerformance();
                            if (score <= best)
                            {
                                continue;
                            }

                            best = score;
                            SelectedAgent = s;
                        }
                    }
                    continue;
                }

                // If the soldier has no target, reset its look angle.
                if (soldier.Target == null)
                {
                    soldier.StopLooking();
                    soldier.headPosition.localRotation = Quaternion.identity;
                    soldier.weaponPosition.localRotation = Quaternion.identity;
                    continue;
                }

                // Otherwise, look towards the target.
                Vector3 position = soldier.Target.Value.Position;
                soldier.Look(position);
                soldier.headPosition.LookAt(position);
                soldier.headPosition.localRotation = Quaternion.Euler(soldier.headPosition.localRotation.eulerAngles.x, 0, 0);
                soldier.weaponPosition.LookAt(position);
                soldier.weaponPosition.localRotation = Quaternion.Euler(soldier.weaponPosition.localRotation.eulerAngles.x, 0, 0);

                // Continue if nothing is in line of sight of the soldier.
                if (!soldier.Weapons[soldier.WeaponIndex].CanShoot || !Physics.Raycast(soldier.shootPosition.position, soldier.shootPosition.forward, out RaycastHit hit, float.MaxValue, layerMask))
                {
                    continue;
                }

                // If something was hit, see if it was another soldier.
                Transform tr = hit.collider.transform;
                do
                {
                    Soldier attacked = tr.GetComponent<Soldier>();
                    if (attacked != null)
                    {
                        // If it was a soldier on the other team, shoot at them.
                        if (attacked.RedTeam != soldier.RedTeam)
                        {
                            soldier.Weapons[soldier.WeaponIndex].Shoot();
                        }

                        break;
                    }

                    tr = tr.parent;
                } while (tr != null);
            }
        }
        
        /// <summary>
        /// Render buttons to reset the level.
        /// </summary>
        /// <param name="x">X rendering position. In most cases this should remain unchanged.</param>
        /// <param name="y">Y rendering position. Update this with every component added and return it.</param>
        /// <param name="w">Width of components. In most cases this should remain unchanged.</param>
        /// <param name="h">Height of components. In most cases this should remain unchanged.</param>
        /// <param name="p">Padding of components. In most cases this should remain unchanged.</param>
        /// <returns>The updated Y position after all custom rendering has been done.</returns>
        protected override float CustomRendering(float x, float y, float w, float h, float p)
        {
            // Reset the game.
            if (GuiButton(x, y, w, h, "Reset"))
            {
                NewGame();
            }
            
            return NextItem(y, h, p);
        }
    }
}