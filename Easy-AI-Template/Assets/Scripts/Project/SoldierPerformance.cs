using EasyAI;

namespace Project
{
    /// <summary>
    /// Calculate how well a soldier is doing based off their captures, returns, kills, and deaths.
    /// </summary>
    public class SoldierPerformance : PerformanceMeasure
    {
        /// <summary>
        /// Calculate how well a soldier is doing based off their captures, returns, kills, and deaths.
        /// </summary>
        /// <returns>The soldier's score.</returns>
        public override float CalculatePerformance() =>
            Agent is not Soldier {Alive: true} soldier
                ? int.MinValue
                : soldier.Captures * SoldierManager.ScoreCapture + soldier.Returns * SoldierManager.ScoreReturn + (soldier.Kills - soldier.Deaths) * SoldierManager.ScoreKillsDeaths;
    }
}