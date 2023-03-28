using System;
using EasyAI;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace COMP499_Project.Game_Scripts
{
    public class ZombieCharacter : TransformAgent
    {
        public float Hunger { get; private set; }
        public float Smell { get; private set; }
        public float Hear { get; private set; }

        public float LifeSpan { get; private set; }
        public float DetectionRange { get; private set; }
        public float HungerRange { get; private set; }
        public float SmellingRange { get; private set; }
        public float HearingRange { get; private set; }
        public float ElapsedLifespan { get; private set; }
        
        public bool DidEat { get; private set; }
        public bool SmellingSomething { get; private set; }
        public bool HearingSomething { get; private set; }
        public bool LookingAtPlayer { get; private set; }
        
        public Object Hunter { get; private set; }
        private GameObject _targetObject;
        private Player _targetPlayer;
        private ZombieCharacter _targetZombie;
        
        public bool IsHungry => Hunger > HungerRange;
        public bool IsSmelling => Smell > SmellingRange;
        public bool IsHearing => Hear > HearingRange;
        
        public bool IsBeingHunted => Hunter != null;
        public bool HasTarget => _targetObject != null;

        public Transform TargetObjectTransform => _targetObject == null ? null : _targetObject.transform;
        
        public bool IsFirstGenZombie => ElapsedLifespan >= 0;
        public bool IsSecGenZombie => ElapsedLifespan >= LifeSpan / 5;
        public bool IsThirdGenZombie => ElapsedLifespan >= 2 * (LifeSpan / 5);
        public bool IsForthGenZombie => ElapsedLifespan >= 3 * (LifeSpan / 5);
        
        // Zombie Manager Type
        
        // Functions for Zombie Character
        public void StartHunting(GameObject obj)
        {
            if (obj == null || obj.GetComponent<ZombieCharacter>() == this)
                return;
            if (obj.GetComponent<ZombieCharacter>())
            {
                _targetZombie = obj.GetComponent<ZombieCharacter>();
                Log($"Hunting Zombie : {_targetZombie.name}");
                _targetZombie.Hunter = this;
            }

            if (obj.GetComponent<Player>())
            {
                _targetPlayer = obj.GetComponent<Player>();
                Log($"Hunting Player : {_targetPlayer.name}");
                // Design an Player npc which also runs from zombies. 
            }
        }

        public bool KillPlayer()
        {
            if (_targetPlayer == null || Vector3.Distance(transform.position, _targetPlayer.transform.position) >
                ZombieManager.PlayerInteractionRadius)
                return false;
            
            Hunger = Mathf.Max(ZombieManager.StartingHunger, Hunger - ZombieManager.HungerRestoredFromEatingPlayer);
            // Play audio of zombie killing player
            Log($"Kill player : {_targetPlayer.name}");
            _targetPlayer.Die();
            return true;
        }

        public bool EatZombie()
        {
            if (_targetZombie == null || Vector3.Distance(transform.position, _targetZombie.transform.position) > ZombieManager.ZombieInteractRadius)
                return false;

            Hunger = Mathf.Max(ZombieManager.StartingHunger, Hunger - ZombieManager.HungerRestoredFromEatingZombie);
            // PlayAudio(eatAudio);
            Log($"Ate Zombie : {_targetZombie.name}.");
            _targetZombie.Die();
            return true;
        }

        public void Die()
        {
            Log("Died!");
            // Instantiate() Add Death animation
            Destroy(gameObject);
        }

        public void SetHunger(float hunger)
        {
            Hunger = hunger;
        }

        public void SetSmell(float smell)
        {
            Smell = smell;
        }

        public void SetHear(float hear)
        {
            Hear = hear;
        }

        public void SetLifeSpan(float lifeSpan)
        {
            LifeSpan = lifeSpan;
        }

        public void SetDetectionRange(float detectionRange)
        {
            DetectionRange = detectionRange;
        }
        
        public void SetElapsedLifespan(float elapsedTime)
        {
            ElapsedLifespan = elapsedTime;
        }
        
        public void Age()
        {
            ElapsedLifespan += Time.deltaTime;
        }
        
        public void SetTarget(GameObject target)
        {
            if (target.GetComponent<Player>())
                _targetPlayer = target.GetComponent<Player>();
            if (target.GetComponent<ZombieCharacter>())
                _targetZombie = target.GetComponent<ZombieCharacter>();
        }
        
        public void RemoveTarget()
        {
            _targetPlayer = null;
            _targetZombie = null;
        }

        public override void Perform()
        {
            base.Perform();
            if (Random.value <= ZombieManager.HungerChance * DeltaTime)
                Hunger++;
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
