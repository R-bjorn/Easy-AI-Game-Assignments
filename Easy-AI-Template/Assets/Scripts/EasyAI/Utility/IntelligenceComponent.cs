namespace EasyAI.Utility
{
    /// <summary>
    /// Base component for sensors, actuators, minds, and performance measures.
    /// </summary>
    public abstract class IntelligenceComponent : MessageComponent
    {
        /// <summary>
        /// The agent this component is connected to.
        /// </summary>
        public Agent Agent { get; set; }

        protected virtual void Start()
        {
            Setup();
        }
    
        protected virtual void OnEnable()
        {
            try
            {
                Setup();
            }
            catch
            {
                // Ignored.
            }
        }

        protected virtual void OnDisable()
        {
            try
            {
                Setup();
            }
            catch
            {
                // Ignored.
            }
        }

        protected virtual void OnDestroy()
        {
            try
            {
                Setup();
            }
            catch
            {
                // Ignored.
            }
        }

        /// <summary>
        /// If this was added to the agent later, it won't yet be connected to it, so call the configuration again.
        /// </summary>
        private void Setup()
        {
            if (Agent == null)
            {
                Manager.RefreshAgents();
            }
        }
    }
}