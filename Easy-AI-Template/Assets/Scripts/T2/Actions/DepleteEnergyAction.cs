namespace T2.Actions
{
    /// <summary>
    /// Action to pass to actuator to deplete energy.
    /// </summary>
    public class DepleteEnergyAction
    {
        /// <summary>
        /// The energy component.
        /// </summary>
        public readonly EnergyComponent EnergyComponent;

        /// <summary>
        /// Create the action data wrapper.
        /// </summary>
        /// <param name="energyComponent">The energy component to pass.</param>
        public DepleteEnergyAction(EnergyComponent energyComponent)
        {
            EnergyComponent = energyComponent;
        }
    }
}