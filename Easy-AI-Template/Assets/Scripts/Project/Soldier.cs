using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyAI;
using Project.Pickups;
using Project.Positions;
using Project.Weapons;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project
{
    /// <summary>
    /// Agent used to control the soldiers in the project.
    /// </summary>
    public class Soldier : CharacterAgent
    {
        /// <summary>
        /// The behaviour of soldiers is dependent upon their role on the team.
        /// Dead - Nothing as they are dead.
        /// Collector - One on each team who's main goal is to collect the enemy flag and return it.
        /// Attacker - Move between locations on the enemy's side of the map.
        /// Defender - Move between locations on their side of the map and move to return their flag if it has been taken.
        /// </summary>
        public enum SoliderRole : byte
        {
            Dead = 0,
            Collector = 1,
            Attacker = 2,
            Defender = 3
        }
        
        /// <summary>
        /// The indexes of weapons the soldier can use, which will correspond to the weapon priority variable indexes.
        /// </summary>
        public enum WeaponIndexes
        {
            MachineGun = 0,
            Shotgun = 1,
            Sniper = 2,
            RocketLauncher = 3,
            Pistol = 4
        }

        /// <summary>
        /// The data a soldier holds.
        /// </summary>
        public class EnemyMemory
        {
            public Soldier Enemy;

            public bool HasFlag;

            public bool Visible;

            public Vector3 Position;

            public float DeltaTime;
        }

        /// <summary>
        /// The data on the current target of a soldier.
        /// </summary>
        public struct TargetData
        {
            public Soldier Enemy;
            
            public Vector3 Position;

            public bool Visible;
        }

        [Tooltip("The position of the solder's head.")]
        public Transform headPosition;

        [Tooltip("The position of where to cast rays and spawn projectiles from.")]
        public Transform shootPosition;

        [Tooltip("The position of where to hold the flag when carrying it.")]
        public Transform flagPosition;

        [Tooltip("The position of where weapons are held at by the soldier.")]
        public Transform weaponPosition;

        [SerializeField]
        [Tooltip("All visuals which change color based on the soldier's team.")]
        private MeshRenderer[] colorVisuals;

        [SerializeField]
        [Tooltip("All remaining visuals that do not change color based on the soldier's team.")]
        private MeshRenderer[] otherVisuals;

        /// <summary>
        /// The health of the soldier.
        /// </summary>
        public int Health { get; set; }
        
        /// <summary>
        /// The currently selected weapon of the soldier.
        /// </summary>
        public int WeaponIndex { get; set; }
        
        /// <summary>
        /// The target of the soldier.
        /// </summary>
        public TargetData? Target { get; private set; }
        
        /// <summary>
        /// How many kills this soldier has.
        /// </summary>
        public int Kills { get; set; }
        
        /// <summary>
        /// How many deaths this soldier has.
        /// </summary>
        public int Deaths { get; set; }
        
        /// <summary>
        /// How many flag captures this soldier has.
        /// </summary>
        public int Captures { get; set; }
        
        /// <summary>
        /// How many flag returns this soldier has.
        /// </summary>
        public int Returns { get; set; }

        /// <summary>
        /// If this soldier is on the red team or not.
        /// </summary>
        public bool RedTeam { get; private set; }
        
        /// <summary>
        /// The weapons of this soldier.
        /// </summary>
        public Weapon[] Weapons { get; private set; }

        /// <summary>
        /// If the soldier is alive or not.
        /// </summary>
        public bool Alive => Role != SoliderRole.Dead;
        
        /// <summary>
        /// The soldier's current role on the team.
        /// </summary>
        public SoliderRole Role { get; private set; }

        /// <summary>
        /// The colliders that are attached to this soldier.
        /// </summary>
        public Collider[] Colliders { get; private set; }
        
        public float DistanceTarget => Target == null ? float.MaxValue : Vector3.Distance(shootPosition.position, Target.Value.Position);

        /// <summary>
        /// The enemies which this soldier currently has detected.
        /// </summary>
        public readonly List<EnemyMemory> DetectedEnemies = new();

        /// <summary>
        /// Which weapons the soldier has a preference to currently use.
        /// </summary>
        public readonly int[] WeaponPriority = new int[(int) WeaponIndexes.Pistol + 1];

        /// <summary>
        /// If this soldier is carrying the flag.
        /// </summary>
        public bool CarryingFlag => RedTeam ? FlagPickup.BlueFlag != null && FlagPickup.BlueFlag.carryingPlayer == this : FlagPickup.RedFlag != null && FlagPickup.RedFlag.carryingPlayer == this;

        /// <summary>
        /// If this soldier's flag is at its base.
        /// </summary>
        public bool FlagAtBase => RedTeam ? FlagPickup.RedFlag != null && FlagPickup.RedFlag.transform.position == FlagPickup.RedFlag.SpawnPosition : FlagPickup.BlueFlag != null && FlagPickup.BlueFlag.transform.position == FlagPickup.BlueFlag.SpawnPosition;

        /// <summary>
        /// The location of the enemy flag.
        /// </summary>
        public Vector3 EnemyFlagPosition => RedTeam ? FlagPickup.BlueFlag != null ? FlagPickup.BlueFlag.transform.position : Vector3.zero : FlagPickup.RedFlag != null ? FlagPickup.RedFlag.transform.position : Vector3.zero;

        /// <summary>
        /// The location of the team's flag.
        /// </summary>
        public Vector3 TeamFlagPosition => RedTeam ? FlagPickup.RedFlag != null ? FlagPickup.RedFlag.transform.position : Vector3.zero : FlagPickup.BlueFlag != null ? FlagPickup.BlueFlag.transform.position : Vector3.zero;
        
        /// <summary>
        /// The location of this soldier's base.
        /// </summary>
        public Vector3 BasePosition => RedTeam ? FlagPickup.RedFlag != null ? FlagPickup.RedFlag.SpawnPosition : Vector3.zero : FlagPickup.BlueFlag != null ? FlagPickup.BlueFlag.SpawnPosition : Vector3.zero;
        
        /// <summary>
        /// Override for custom detail rendering on the automatic GUI.
        /// </summary>
        /// <param name="x">X rendering position. In most cases this should remain unchanged.</param>
        /// <param name="y">Y rendering position. Update this with every component added and return it.</param>
        /// <param name="w">Width of components. In most cases this should remain unchanged.</param>
        /// <param name="h">Height of components. In most cases this should remain unchanged.</param>
        /// <param name="p">Padding of components. In most cases this should remain unchanged.</param>
        /// <returns>The updated Y position after all custom rendering has been done.</returns>
        public override float DisplayDetails(float x, float y, float w, float h, float p)
        {
            y = Manager.NextItem(y, h, p);
            Manager.GuiBox(x, y, w, h, p, 8);
            
            // Display overall flags captured for each team.
            Manager.GuiLabel(x, y, w, h, p, $"Team Captures - Red: {SoldierManager.CapturedRed} | Blue: {SoldierManager.CapturedBlue}");
            y = Manager.NextItem(y, h, p);
            
            // Display overall kills for each team.
            Manager.GuiLabel(x, y, w, h, p, $"Team Kills - Red: {SoldierManager.KillsRed} | Blue: {SoldierManager.KillsBlue}");
            y = Manager.NextItem(y, h, p);

            // Display the role of this soldier.
            Manager.GuiLabel(x, y, w, h, p, Role == SoliderRole.Dead ? "Respawning" : $"Role: {Role}");
            y = Manager.NextItem(y, h, p);

            // Display the health of this soldier.
            Manager.GuiLabel(x, y, w, h, p, $"Health: {Health} / {SoldierManager.Health}");
            y = Manager.NextItem(y, h, p);

            // Display the weapon this soldier is using.
            Manager.GuiLabel(x, y, w, h, p, Role == SoliderRole.Dead ? "Weapon: None" : WeaponIndex switch
            {
                (int) WeaponIndexes.MachineGun => $"Weapon: Machine Gun | Ammo: {Weapons[WeaponIndex].Ammo} / {Weapons[WeaponIndex].MaxAmmo}",
                (int) WeaponIndexes.Shotgun => $"Weapon: Shotgun | Ammo: {Weapons[WeaponIndex].Ammo} / {Weapons[WeaponIndex].MaxAmmo}",
                (int) WeaponIndexes.Sniper => $"Weapon: Sniper | Ammo: {Weapons[WeaponIndex].Ammo} / {Weapons[WeaponIndex].MaxAmmo}",
                (int) WeaponIndexes.RocketLauncher => $"Weapon: Rocket Launcher | Ammo: {Weapons[WeaponIndex].Ammo} / {Weapons[WeaponIndex].MaxAmmo}",
                _ => "Weapon: Pistol"
            });
            y = Manager.NextItem(y, h, p);
            
            // Display the enemy this soldier is fighting.
            Manager.GuiLabel(x, y, w, h, p, Target == null || Target.Value.Enemy == null ? "Fighting: Nobody" : $"Fighting: {Target.Value.Enemy.name}");
            y = Manager.NextItem(y, h, p);

            // Display all enemies this soldier has detected.
            int visible = DetectedEnemies.Count(e => e.Visible);
            Manager.GuiLabel(x, y, w, h, p, $"Detected Enemies - See: {visible} | Hear: {DetectedEnemies.Count - visible}");
            y = Manager.NextItem(y, h, p);

            // Display how many flag captures this soldier has.
            Manager.GuiLabel(x, y, w, h, p, $"Captures: {Captures} | Returns: {Returns} | K/D : {Kills}/{Deaths}");
            
            return y;
        }
        
        /// <summary>
        /// Override to have the soldier perform its actions.
        /// </summary>
        public override void Perform()
        {
            // Do nothing when dead.
            if (Role == SoliderRole.Dead)
            {
                return;
            }

            // Remove detected enemies that have exceeded their maximum memory time.
            for (int i = 0; i < DetectedEnemies.Count; i++)
            {
                // Increment how long the enemy has been in memory.
                DetectedEnemies[i].DeltaTime += DeltaTime;
                
                // If the detected enemy is too old or they have died, remove it.
                if (DetectedEnemies[i].DeltaTime > SoldierManager.MemoryTime || DetectedEnemies[i].Enemy.Role == SoliderRole.Dead)
                {
                    DetectedEnemies.RemoveAt(i--);
                }
            }

            // Standard mind and states.
            base.Perform();

            int priority = int.MaxValue;
            int selected = (int) WeaponIndexes.Pistol;
            
            // Go through the weapon priority and select the most preferred option which has ammo.
            for (int i = 0; i < WeaponPriority.Length; i++)
            {
                if (Weapons[i].Ammo <= 0 && Weapons[i].MaxAmmo >= 0)
                {
                    continue;
                }

                if (WeaponPriority[i] >= priority)
                {
                    continue;
                }

                selected = i;
                priority = WeaponPriority[i];
            }

            if (WeaponIndex != selected)
            {
                SelectWeapon(selected);
            }
        }
        
        /// <summary>
        /// Character controller movement.
        /// </summary>
        public override void MovementCalculations()
        {
            // Only move when the controller is enabled to avoid throwing an error as it needs to be disabled when dead.
            if (CharacterController != null && CharacterController.enabled)
            {
                base.MovementCalculations();
            }
        }

        /// <summary>
        /// Set the weapon priority for the soldier to choose the most ideal weapon, with lower values meaning higher priority.
        /// </summary>
        /// <param name="machineGun">The priority for the soldier to use the machine gun.</param>
        /// <param name="shotgun">The priority for the soldier to use the shotgun.</param>
        /// <param name="sniper">The priority for the soldier to use the sniper.</param>
        /// <param name="rocketLauncher">The priority for the soldier to use the rocket launcher.</param>
        /// <param name="pistol">The priority for the soldier to use the pistol.</param>
        public void SetWeaponPriority(int machineGun = 5, int shotgun = 5, int sniper = 5, int rocketLauncher = 5, int pistol = 5)
        {
            WeaponPriority[0] = machineGun;
            WeaponPriority[1] = shotgun;
            WeaponPriority[2] = sniper;
            WeaponPriority[3] = rocketLauncher;
            WeaponPriority[4] = pistol;
        }
        
        public void SetTarget(TargetData targetData)
        {
            Target = targetData;
        }

        public void NoTarget()
        {
            Target = null;
        }
        
        /// <summary>
        /// Receive damage from another soldier.
        /// </summary>
        /// <param name="amount">How much damage was taken.</param>
        /// <param name="shooter">What soldier shot.</param>
        public void Damage(int amount, Soldier shooter)
        {
            // If already dead, do nothing.
            if (Role == SoliderRole.Dead)
            {
                return;
            }
            
            // Reduce health.
            Health -= amount;
            
            // Nothing more to do if still alive.
            if (Health <= 0)
            {
                SoldierManager.AddKill(shooter, this);
            }
        }

        /// <summary>
        /// Add an enemy to memory that was heard.
        /// </summary>
        /// <param name="enemy">The enemy which was heard.</param>
        /// <param name="distance">How far away before the sound is considered out of range.</param>
        public void Hear(Soldier enemy, float distance)
        {
            // Do not "hear" an enemy if the shot was out of range.
            if (Vector3.Distance(headPosition.position, enemy.headPosition.position) > distance)
            {
                return;
            }
            
            // See if this item already exists in memory and if it does, simply update values.
            EnemyMemory memory = DetectedEnemies.FirstOrDefault(e => e.Enemy == enemy && !e.Visible);
            if (memory != null)
            {
                memory.DeltaTime = 0;
                memory.Position = enemy.headPosition.position;
                memory.Visible = false;
                memory.HasFlag = false;
                return;
            }
            
            // Otherwise add the instance into memory.
            DetectedEnemies.Add(new()
            {
                DeltaTime = 0,
                Enemy = enemy,
                Position = enemy.headPosition.position,
                Visible = false,
                HasFlag = false
            });
        }

        /// <summary>
        /// Heal this soldier.
        /// </summary>
        public void Heal()
        {
            // Cannot heal if dead.
            if (Role == SoliderRole.Dead)
            {
                return;
            }

            Health = SoldierManager.Health;
        }

        /// <summary>
        /// Get all enemies.
        /// </summary>
        /// <returns>An enumerable of all enemies.</returns>
        public IEnumerable<Soldier> GetEnemies()
        {
            return (RedTeam ? SoldierManager.TeamBlue : SoldierManager.TeamRed).Where(s => s.Alive);
        }

        /// <summary>
        /// Assign roles to the team.
        /// </summary>
        public void AssignRoles()
        {
            // Get the soldiers on this team, ordered by how close they are to the enemy flag.
            Soldier[] team = GetTeam();
            
            // Loop through every team member.
            for (int i = 0; i < team.Length; i++)
            {
                // Clear any current movement data.
                team[i].StopMoving();
                team[i].StopNavigating();
                
                // The closest soldier to the enemy flag becomes the collector.
                if (i == 0)
                {
                    team[i].Role = SoliderRole.Collector;
                }
                // The nearest half become attackers.
                else if (i <= team.Length / 2)
                {
                    team[i].Role = SoliderRole.Attacker;
                }
                // The furthest become defenders.
                else
                {
                    team[i].Role = SoliderRole.Defender;
                }
            }
        }

        /// <summary>
        /// Spawn the soldier in.
        /// </summary>
        public void Spawn()
        {
            // Get spawn points on their side of the map.
            SpawnPoint[] points = SoldierManager.SpawnPoints.Where(p => p.RedTeam == RedTeam).ToArray();
            
            // Get all open spawn points.
            SpawnPoint[] open = points.Where(p => p.Open).ToArray();
            
            // If there are open spawn points, spawn at one of them, otherwise, default to any spawn point.
            SpawnPoint spawn = open.Length > 0 ? open[Random.Range(0, open.Length)] : points[Random.Range(0, points.Length)];

            // Since there is a character controller attached, it needs to be disabled to move the soldier to the spawn.
            CharacterController.enabled = false;

            // Move to the spawn point.
            Transform spawnTr = spawn.transform;
            transform.position = spawnTr.position;
            Visuals.rotation = spawnTr.rotation;
            
            // Set that the spawn point has been used so other soldiers avoid using it.
            spawn.Use();
            
            // Reenable the character controller.
            // ReSharper disable once Unity.InefficientPropertyAccess
            CharacterController.enabled = true;
            
            // Set a dummy role to indicate the soldier is no longer dead.
            Role = SoliderRole.Collector;
            
            // Get new roles, heal, start with the machine gun, and reset to find a new point.
            AssignRoles();
            Heal();
            SelectWeapon(0);
            ToggleAlive();

            foreach (Weapon weapon in Weapons)
            {
                weapon.Replenish();
            }
        }

        /// <summary>
        /// Detect which enemies are visible.
        /// </summary>
        /// <returns>All enemies in line of sight.</returns>
        public IEnumerable<Soldier> SeeEnemies()
        {
            return GetEnemies().Where(enemy => !Physics.Linecast(headPosition.position, enemy.headPosition.position, Manager.ObstacleLayers)).ToArray();
        }

        /// <summary>
        /// Respawn the soldier after being killed.
        /// </summary>
        /// <returns>Nothing.</returns>
        public IEnumerator Respawn()
        {
            // Set that the soldier has died.
            Role = SoliderRole.Dead;
            ToggleAlive();
            
            // Reassign team roles.
            AssignRoles();
            
            // Clear data the soldier had.
            DetectedEnemies.Clear();
            Target = null;
            StopNavigating();
            StopMoving();
            StopLooking();
            MoveVelocity = Vector2.zero;
            
            // Wait to spawn.
            yield return new WaitForSeconds(SoldierManager.Respawn);
            
            // Spawn the soldier.
            Spawn();
        }

        protected override void Start()
        {
            // Perform default setup.
            base.Start();

            // Setup all weapons.
            Weapons = GetComponentsInChildren<Weapon>();
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].Soldier = this;
                Weapons[i].Index = i;
            }

            // Assign team.
            RedTeam = SoldierManager.TeamRed.Count <= SoldierManager.TeamBlue.Count;
            if (RedTeam)
            {
                SoldierManager.TeamRed.Add(this);
            }
            else
            {
                SoldierManager.TeamBlue.Add(this);
            }

            // Assign name.
            name = (RedTeam ? "Red " : "Blue ") + (RedTeam ? SoldierManager.TeamRed.Count : SoldierManager.TeamBlue.Count);

            // Get all attached colliders.
            List<Collider> colliders = GetComponents<Collider>().ToList();
            colliders.AddRange(GetComponentsInChildren<Collider>());
            Colliders = colliders.Distinct().ToArray();

            // Assign team colors.
            foreach (MeshRenderer meshRenderer in colorVisuals)
            {
                meshRenderer.material = RedTeam ? SoldierManager.Red : SoldierManager.Blue;
            }
            
            // Spawn in.
            Spawn();
        }

        protected override void OnDestroy()
        {
            try
            {
                SoldierManager.TeamRed.Remove(this);
            }
            catch
            {
                // Ignored.
            }
            
            try
            {
                SoldierManager.TeamBlue.Remove(this);
            }
            catch
            {
                // Ignored.
            }
            
            base.OnDestroy();
        }

        /// <summary>
        /// Get all members of this soldier's team.
        /// </summary>
        /// <returns>All soldiers on this solder's team by closest to the enemy flag.</returns>
        private Soldier[] GetTeam()
        {
            IEnumerable<Soldier> team = (RedTeam ? SoldierManager.TeamRed : SoldierManager.TeamBlue).Where(s => s.Alive);
            if (RedTeam)
            {
                if (FlagPickup.BlueFlag != null)
                {
                    team = team.OrderBy(s => Vector3.Distance(s.transform.position, FlagPickup.BlueFlag.transform.position));
                }
            }
            else
            {
                if (FlagPickup.RedFlag != null)
                {
                    team = team.OrderBy(s => Vector3.Distance(s.transform.position, FlagPickup.RedFlag.transform.position));
                }
            }

            return team.ToArray();
        }

        /// <summary>
        /// Toggle all meshes, colliders, and weapons based on if the soldier is alive.
        /// </summary>
        private void ToggleAlive()
        {
            foreach (MeshRenderer meshRenderer in colorVisuals)
            {
                meshRenderer.enabled = Alive;
            }
            
            foreach (MeshRenderer meshRenderer in otherVisuals)
            {
                meshRenderer.enabled = Alive;
            }

            foreach (Collider col in Colliders)
            {
                col.enabled = Alive;
            }

            WeaponVisible();
        }

        /// <summary>
        /// Select a given weapon.
        /// </summary>
        /// <param name="i">The weapon index selected.</param>
        private void SelectWeapon(int i)
        {
            int lastWeapon = WeaponIndex;
            
            // Set the new selected weapon.
            WeaponIndex = math.clamp(i, 0, Weapons.Length - 1);

            if (lastWeapon != WeaponIndex)
            {
                Log((WeaponIndexes) WeaponIndex switch
                {
                    WeaponIndexes.MachineGun => "Selecting machine gun.",
                    WeaponIndexes.Shotgun => "Selecting shotgun.",
                    WeaponIndexes.Sniper => "Selecting sniper.",
                    WeaponIndexes.RocketLauncher => "Selecting rocket launcher.",
                    _=> "Selecting pistol."
                });
            }
            
            // Limit agent rotation speed based on their weapon.
            SetMoveSpeed(Weapons[WeaponIndex].MoveSpeed);
            SetLookSpeed(Weapons[WeaponIndex].RotationSpeed);
            
            // Ensure weapons are properly visible.
            WeaponVisible();
        }

        /// <summary>
        /// Ensure only the selected weapon is visible.
        /// </summary>
        private void WeaponVisible()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                // Only the selected weapon is visible, and none are visible if the soldier is dead.
                Weapons[i].Visible(Alive && i == WeaponIndex);
            }
        }
    }
}