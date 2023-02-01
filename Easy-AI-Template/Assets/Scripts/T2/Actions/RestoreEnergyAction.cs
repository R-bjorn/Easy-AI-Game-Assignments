namespace T2.Actions
{
    /// <summary>
    /// Action to pass to actuator to restore energy.
    /// </summary>
    public class RestoreEnergyAction
    {
        /// <summary>
        /// The energy component.
        /// </summary>
        public readonly EnergyComponent EnergyComponent;

        /// <summary>
        /// Create the action data wrapper.
        /// </summary>
        /// <param name="energyComponent">The energy component to pass.</param>
        public RestoreEnergyAction(EnergyComponent energyComponent)
        {
            EnergyComponent = energyComponent;
        }
    }
}