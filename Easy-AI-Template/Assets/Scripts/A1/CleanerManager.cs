using System.Collections.Generic;
using System.Linq;
using EasyAI;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace A1
{
    /// <summary>
    /// Extension of AgentManager to handle floor tile generation.
    /// </summary>
    [DisallowMultipleComponent]
    public class CleanerManager : Manager
    {
        /// <summary>
        /// All floors.
        /// </summary>
        public static List<Floor> Floors => CleanerSingleton._floors;
        
        /// <summary>
        /// Getter to cast the AgentManager singleton into a FloorManager.
        /// </summary>
        private static CleanerManager CleanerSingleton => Singleton as CleanerManager;
        
        /// <summary>
        /// All floors.
        /// </summary>
        private readonly List<Floor> _floors = new();

        [Header("Cleaner Parameters")]
        [Tooltip("How many floor sections will be generated.")]
        [SerializeField]
        private Vector2 floorSize = new(3, 1);

        [Tooltip("How many units wide will each floor section be generated as.")]
        [SerializeField]
        [Min(1)]
        private int floorScale = 1;

        [Tooltip("The percentage chance that any floor section during generation will be likely to get dirty meaning the odds in increases in dirt level every time are double that of other floor sections.")]
        [Range(0, 1)]
        [SerializeField]
        private float likelyToGetDirtyChance;

        [Tooltip("How many seconds between every time dirt is randomly added to the floor.")]
        [Min(0)]
        [SerializeField]
        private float timeBetweenDirtGeneration = 5;

        [Tooltip("The percentage chance that a floor section will increase in dirt level during dirt generation.")]
        [Range(0, 1)]
        [SerializeField]
        private float chanceDirty;
        
        [Header("Prefabs")]
        [Tooltip("The prefab for the cleaning agent that will be spawned in.")]
        [SerializeField]
        private GameObject cleanerAgentPrefab;

        [Header("Floor Materials")]
        [Tooltip("The material applied to normal floor sections when they are clean.")]
        [SerializeField]
        private Material materialCleanNormal;

        [Tooltip("The material applied to like to get dirty floor sections when they are clean.")]
        [SerializeField]
        private Material materialCleanLikelyToGetDirty;

        [Tooltip("The material applied to a floor section when it is dirty.")]
        [SerializeField]
        private Material materialDirty;

        [Tooltip("The material applied to a floor section when it is very dirty.")]
        [SerializeField]
        private Material materialVeryDirty;

        [Tooltip("The material applied to a floor section when it is extremely dirty.")]
        [SerializeField]
        private Material materialExtremelyDirty;

        /// <summary>
        /// The root game object of the cleaner agent.
        /// </summary>
        private GameObject _cleanerAgent;

        /// <summary>
        /// Keep track of how much time has passed since the last time floor tiles were made dirty.
        /// </summary>
        private float _elapsedTime;

        /// <summary>
        /// Generate the floor.
        /// </summary>
        private static void GenerateFloor()
        {
            // Destroy the previous agent.
            if (CleanerSingleton._cleanerAgent != null)
            {
                Destroy(CleanerSingleton._cleanerAgent.gameObject);
            }
            
            // Destroy all previous floors.
            foreach (Floor floor in CleanerSingleton._floors)
            {
                Destroy(floor.gameObject);
            }
            CleanerSingleton._floors.Clear();
            
            // Generate the floor tiles.
            Vector2 offsets = new Vector2((CleanerSingleton.floorSize.x - 1) / 2f, (CleanerSingleton.floorSize.y - 1) / 2f) * CleanerSingleton.floorScale;
            for (int x = 0; x < CleanerSingleton.floorSize.x; x++)
            {
                for (int y = 0; y < CleanerSingleton.floorSize.y; y++)
                {
                    GenerateFloorTile(new(x, y), offsets);
                }
            }

            // Add the cleaner agent.
            CleanerSingleton._cleanerAgent = Instantiate(CleanerSingleton.cleanerAgentPrefab, Vector3.zero, quaternion.identity);
            CleanerSingleton._cleanerAgent.name = "Cleaner Agent";

            // Reset elapsed time.
            CleanerSingleton._elapsedTime = 0;
        }

        /// <summary>
        /// Generate a floor tile.
        /// </summary>
        /// <param name="position">Its position relative to the rest of the floor tiles.</param>
        /// <param name="offsets">How much to offset the floor tile so all floors are centered around the origin.</param>
        private static void GenerateFloorTile(Vector2 position, Vector2 offsets)
        {
            // Create a quad, then position, rotate, size, and name it.
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.position = new(position.x * CleanerSingleton.floorScale - offsets.x, 0, position.y * CleanerSingleton.floorScale - offsets.y);
            go.transform.rotation = Quaternion.Euler(90, 0, 0);
            go.transform.localScale = new(CleanerSingleton.floorScale, CleanerSingleton.floorScale, 1);
            go.name = $"Floor {position.x} {position.y}";
            
            // Its collider is not needed.
            Destroy(go.GetComponent<Collider>());
            
            // Add and setup its floor component.
            Floor floor = go.AddComponent<Floor>();
            bool likelyToGetDirty = Random.value < CleanerSingleton.likelyToGetDirtyChance;
            floor.Setup(likelyToGetDirty, likelyToGetDirty ? CleanerSingleton.materialCleanLikelyToGetDirty : CleanerSingleton.materialCleanNormal, CleanerSingleton.materialDirty, CleanerSingleton.materialVeryDirty, CleanerSingleton.materialExtremelyDirty);
            CleanerSingleton._floors.Add(floor);
        }

        /// <summary>
        /// Update the states of floor tiles.
        /// </summary>
        private static void UpdateFloor()
        {
            // Increment how much time has passed and return if it has not been long enough since the last dirt generation.
            CleanerSingleton._elapsedTime += Time.deltaTime;
            if (CleanerSingleton._elapsedTime < CleanerSingleton.timeBetweenDirtGeneration)
            {
                return;
            }

            // Reset elapsed time.
            CleanerSingleton._elapsedTime = 0;
            
            SetFloorTiles();
        }

        /// <summary>
        /// Set floor tiles to be dirty.
        /// </summary>
        private static void SetFloorTiles()
        {
            // If all floor tiles are already at max dirt level return as there is nothing more which can be updated.
            if (CleanerSingleton._floors.Count(f => f.State != Floor.DirtLevel.ExtremelyDirty) == 0)
            {
                return;
            }

            // Get the chance that any tile will become dirty.
            float currentDirtyChance = Mathf.Max(CleanerSingleton.chanceDirty, float.Epsilon);
            
            // We will loop until at least a single tile has been made dirty.
            bool addedDirty = false;
            do
            {
                // Loop through all floor tiles.
                foreach (Floor floor in CleanerSingleton._floors.Where(f => f.State != Floor.DirtLevel.ExtremelyDirty))
                {
                    // Double the chance to get dirty of the current floor tile is likely to get dirty.
                    float dirtChance = floor.LikelyToGetDirty ? currentDirtyChance * 2 : currentDirtyChance;

                    // Attempt to make each tile dirty three times meaning there is a chance a tile can gain multiple dirt levels at once.
                    for (int i = 0; i < 3; i++)
                    {
                        if (Random.value <= dirtChance)
                        {
                            floor.Dirty();
                            addedDirty = true;
                        }
                    }
                }

                // Double the chances of tiles getting dirty for the next loop so we are not infinitely looping.
                currentDirtyChance *= 2;
            }
            while (!addedDirty);
        }

        protected override void Start()
        {
            base.Start();
            GenerateFloor();
            SetFloorTiles();
        }

        protected override void Update()
        {
            base.Update();
            UpdateFloor();
        }

        /// <summary>
        /// Render buttons to regenerate the floor or change its size.
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
                ClearMessages();
                GenerateFloor();
                SetFloorTiles();
            }
            
            // Increase the floor width.
            if (floorSize.x < 5)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Increase Size X"))
                {
                    floorSize.x++;
                    GenerateFloor();
                }
            }

            // Decrease the floor width.
            if (floorSize.x > 1)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Decrease Size X"))
                {
                    floorSize.x--;
                    GenerateFloor();
                }
            }
            
            // Increase the floor height.
            if (floorSize.y < 5)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Increase Size Y"))
                {
                    floorSize.y++;
                    GenerateFloor();
                }
            }

            // Decrease the floor height.
            if (floorSize.y > 1)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Decrease Size Y"))
                {
                    floorSize.y--;
                    GenerateFloor();
                }
            }
            
            return NextItem(y, h, p);
        }
    }
}