using A2.Pickups;
using A2.States;
using EasyAI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace A2
{
    /// <summary>
    /// Microbe extends agent rather than being a separate component.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class Microbe : TransformAgent
    {
        /// <summary>
        /// The hunger of this microbe.
        /// </summary>
        public int Hunger { get; private set; }
        
        /// <summary>
        /// How long this microbe will live for in seconds.
        /// </summary>
        public float LifeSpan { get; private set; }

        /// <summary>
        /// How far away this microbe can detect other microbes and pickups.
        /// </summary>
        public float DetectionRange { get; private set; }
        
        /// <summary>
        /// How much of this microbe's life in seconds has passed.
        /// </summary>
        public float ElapsedLifespan { get; private set; }
        
        /// <summary>
        /// True if this microbe has already mated, false otherwise.
        /// </summary>
        public bool DidMate { get; private set; }

        /// <summary>
        /// The microbe that is hunting this microbe.
        /// </summary>
        public Microbe Hunter { get; private set; }

        /// <summary>
        /// The pickup this microbe is moving towards.
        /// </summary>
        public MicrobeBasePickup Pickup { get; private set; }
        
        /// <summary>
        /// The number of offspring this microbe has had.
        /// </summary>
        public int Offspring { get; private set; }

        /// <summary>
        /// The microbe is hungry when its hunger level is above zero.
        /// </summary>
        public bool IsHungry => Hunger > 0;

        /// <summary>
        /// If this microbe is being hunted or not.
        /// </summary>
        public bool BeingHunted => Hunter != null;

        /// <summary>
        /// If the microbe currently has a target or not.
        /// </summary>
        public bool HasTarget => _targetMicrobe != null;

        /// <summary>
        /// If the microbe currently has a pickup it wants to pickup.
        /// </summary>
        public bool HasPickup => Pickup != null;

        /// <summary>
        /// The transform of the target microbe.
        /// </summary>
        public Transform TargetMicrobeTransform => _targetMicrobe == null ? null : _targetMicrobe.transform;

        /// <summary>
        /// A microbe is considered an adult if it has reached the halfway point of its life.
        /// </summary>
        public bool IsAdult => ElapsedLifespan >= LifeSpan / 2;

        [Tooltip("The mesh renderer for the mesh that changes color depending on what state the agent is in.")]
        [SerializeField]
        private MeshRenderer stateVisualization;

        [Tooltip("Audio to play when spawning.")]
        [SerializeField]
        private AudioClip spawnAudio;

        [Tooltip("Audio to play when eating another microbe.")]
        [SerializeField]
        private AudioClip eatAudio;

        [Tooltip("Audio to play when mating.")]
        [SerializeField]
        private AudioClip mateAudio;

        [Tooltip("Audio to play when picking up a pickup.")]
        [SerializeField]
        private AudioClip pickupAudio;

        /// <summary>
        /// The microbe that this microbe is moving towards to either eat or mate with.
        /// </summary>
        private Microbe _targetMicrobe;

        /// <summary>
        /// The type (color) of this microbe.
        /// </summary>
        private MicrobeManager.MicrobeType _microbeType;

        /// <summary>
        /// The audio source to play audio from.
        /// </summary>
        private AudioSource _audioSource;

        /// <summary>
        /// The type (color) of this microbe.
        /// </summary>
        public MicrobeManager.MicrobeType MicrobeType
        {
            get => _microbeType;
            set
            {
                _microbeType = value;

                MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
                if (meshRenderer == null)
                {
                    return;
                }

                meshRenderer.material = _microbeType switch
                {
                    MicrobeManager.MicrobeType.Red => MicrobeManager.RedMicrobeMaterial,
                    MicrobeManager.MicrobeType.Orange => MicrobeManager.OrangeMicrobeMaterial,
                    MicrobeManager.MicrobeType.Yellow => MicrobeManager.YellowMicrobeMaterial,
                    MicrobeManager.MicrobeType.Green => MicrobeManager.GreenMicrobeMaterial,
                    MicrobeManager.MicrobeType.Blue => MicrobeManager.BlueMicrobeMaterial,
                    MicrobeManager.MicrobeType.Purple => MicrobeManager.PurpleMicrobeMaterial,
                    _ => MicrobeManager.PinkMicrobeMaterial
                };
            }
        }

        /// <summary>
        /// Mate two microbes.
        /// </summary>
        /// <param name="a">First microbe to mate.</param>
        /// <param name="b">Second microbe to mate.</param>
        /// <returns>True if mating was complete, false otherwise.</returns>
        private static bool Mate(Microbe a, Microbe b)
        {
            if (a._targetMicrobe != b || b._targetMicrobe != a)
            {
                return false;
            }

            a.DidMate = true;
            b.DidMate = true;
            
            int offspring = MicrobeManager.Mate(a, b);
            
            a.Log(offspring == 0
                ? $"Failed to have any offspring with {b.name}."
                : $"Have {offspring} offspring with {b.name}."
            );
            
            b.Log(offspring == 0
                ? $"Failed to have any offspring with {a.name}."
                : $"Have {offspring} offspring with {a.name}."
            );

            return true;
        }

        /// <summary>
        /// Attempt to mate this microbe.
        /// </summary>
        /// <returns>True if the mating was successful, false otherwise.</returns>
        public bool Mate()
        {
            return Vector3.Distance(transform.position, _targetMicrobe.transform.position) <= MicrobeManager.MicrobeInteractRadius && Mate(this, _targetMicrobe);
        }

        /// <summary>
        /// Hunt the microbe currently tracking.
        /// </summary>
        public void StartHunting(Microbe microbe)
        {
            if (microbe == null || microbe == this)
            {
                return;
            }

            _targetMicrobe = microbe;
            Log($"Hunting {_targetMicrobe.name}.");
            _targetMicrobe.Hunter = this;
        }

        /// <summary>
        /// Attempt to attract a mate.
        /// </summary>
        /// <param name="potentialMate"></param>
        public void AttractMate(Microbe potentialMate)
        {
            if (potentialMate == null || potentialMate == this)
            {
                return;
            }

            Log($"Attempting to impress {potentialMate.name} to mate.");

            bool accepted = !potentialMate.DidMate && potentialMate._targetMicrobe == null;

            if (!accepted)
            {
                Log($"Could not mate with {potentialMate.name}.");
                potentialMate.Log($"Cannot mate with {name}.");
                return;
            }

            // If the other microbe agreed to mate, set them as the target microbe.
            Log($"{potentialMate.name} accepted advances to mate.");
            potentialMate.Log($"Accepted advances of {name}.");
            SetTargetMicrobe(potentialMate);
            potentialMate.SetTargetMicrobe(this);
        }

        /// <summary>
        /// Eat another microbe.
        /// </summary>
        /// <returns>True if eating was successful, false otherwise.</returns>
        public bool Eat()
        {
            if (_targetMicrobe == null || Vector3.Distance(transform.position, _targetMicrobe.transform.position) > MicrobeManager.MicrobeInteractRadius)
            {
                return false;
            }
            
            Hunger = Mathf.Max(MicrobeManager.StartingHunger, Hunger - MicrobeManager.HungerRestoredFromEating);
            PlayAudio(eatAudio);
            Log($"Ate {_targetMicrobe.name}.");
            _targetMicrobe.Die();
            return true;
        }

        /// <summary>
        /// Die.
        /// </summary>
        public void Die()
        {
            Log("Died.");
            Instantiate(MicrobeManager.DeathParticlesPrefab, transform.position, Quaternion.Euler(270, 0, 0));
            Destroy(gameObject);
        }

        /// <summary>
        /// Set the hunger of the microbe.
        /// </summary>
        /// <param name="hunger">The hunger to give the microbe.</param>
        public void SetHunger(int hunger)
        {
            Hunger = hunger;
        }

        /// <summary>
        /// Set the life span of the microbe.
        /// </summary>
        /// <param name="lifeSpan">The life span to give the microbe.</param>
        public void SetLifeSpan(float lifeSpan)
        {
            LifeSpan = lifeSpan;
        }

        /// <summary>
        /// Set the detection range of the microbe.
        /// </summary>
        /// <param name="detectionRange">The detection range to give the microbe.</param>
        public void SetDetectionRange(float detectionRange)
        {
            DetectionRange = detectionRange;
        }

        /// <summary>
        /// Set the elapsed lifespan of the microbe.
        /// </summary>
        /// <param name="elapsedTime">The elapsed lifespan to give the microbe.</param>
        public void SetElapsedLifespan(float elapsedTime)
        {
            ElapsedLifespan = elapsedTime;
        }

        /// <summary>
        /// Increase the age of the microbe.
        /// </summary>
        public void Age()
        {
            ElapsedLifespan += Time.deltaTime;
        }

        /// <summary>
        /// Set the target microbe for hunting or mating.
        /// </summary>
        /// <param name="microbe"></param>
        public void SetTargetMicrobe(Microbe microbe)
        {
            _targetMicrobe = microbe;
        }

        /// <summary>
        /// Remove the target microbe for hunting or mating.
        /// </summary>
        public void RemoveTargetMicrobe()
        {
            _targetMicrobe = null;
        }

        /// <summary>
        /// Set the pickup that the microbe is moving for.
        /// </summary>
        /// <param name="pickup">The pickup to assign.</param>
        public void SetPickup(MicrobeBasePickup pickup)
        {
            Pickup = pickup;
        }
        
        /// <summary>
        /// Remove the pickup that the microbe is moving for.
        /// </summary>
        public void RemovePickup()
        {
            Pickup = null;
        }

        /// <summary>
        /// Reset that the microbe has mated so it can mate again.
        /// </summary>
        public void CanMate()
        {
            DidMate = false;
        }

        /// <summary>
        /// Increment the offspring the microbe has had.
        /// </summary>
        /// <param name="number">The number of offspring the microbe had.</param>
        public void HadOffspring(int number)
        {
            Offspring += number;
        }
        
        /// <summary>
        /// Called by the AgentManager to have the agent sense, think, and act.
        /// </summary>
        public override void Perform()
        {
            // Determine if the microbe's hunger should increase.
            if (Random.value <= MicrobeManager.HungerChance * DeltaTime)
            {
                Hunger++;
            }
            
            base.Perform();
        }

        /// <summary>
        /// Set the state visuals for the microbe.
        /// </summary>
        private void SetStateVisual()
        {
            if (stateVisualization == null)
            {
                return;
            }
            
            if (State as MicrobeRoamingState)
            {
                stateVisualization.material = MicrobeManager.SleepingIndicatorMaterial;
                return;
            }
            
            if (State as MicrobeHungryState)
            {
                stateVisualization.material = MicrobeManager.FoodIndicatorMaterial;
                return;
            }
            
            if (State as MicrobeMatingState)
            {
                stateVisualization.material = MicrobeManager.MateIndicatorMaterial;
                return;
            }
            
            if (State as MicrobeSeekingPickupState)
            {
                stateVisualization.material = MicrobeManager.PickupIndicatorMaterial;
            }
        }

        /// <summary>
        /// Play audio for mating.
        /// </summary>
        public void PlayMateAudio()
        {
            PlayAudio(mateAudio);
        }

        /// <summary>
        /// Play audio for picking up a pickup.
        /// </summary>
        public void PlayPickupAudio()
        {
            PlayAudio(pickupAudio);
        }
        
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
            Manager.GuiBox(x, y, w, h, p, 3);

            Manager.GuiLabel(x, y, w, h, p, $"Hunger: {Hunger} | " + (IsHungry ? "Hungry" : "Not Hungry"));
            y = Manager.NextItem(y, h, p);

            Manager.GuiLabel(x, y, w, h, p, $"Lifespan: {ElapsedLifespan} / {LifeSpan} | " + (IsAdult ? "Adult" : "Infant"));
            y = Manager.NextItem(y, h, p);
            
            Manager.GuiLabel(x, y, w, h, p, $"Mating: " + (DidMate ? "Already Mated" : IsAdult && !IsHungry ? _targetMicrobe == null ? "Searching for mate" : $"With {_targetMicrobe.name}" : "No"));
            
            return y;
        }

        protected override void Start()
        {
            base.Start();
            
            SetStateVisual();

            _audioSource = GetComponent<AudioSource>();
            
            PlayAudio(spawnAudio);

            Visuals.rotation = Quaternion.Euler(new(0, Random.Range(0f, 360f), 0));
        }

        /// <summary>
        /// Play an audio clip.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        private void PlayAudio(AudioClip clip)
        {
            try
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
            catch
            {
                // Ignored.
            }
        }

        private void Update()
        {
            SetStateVisual();
        }
    }
}