using WestWorld.States;

namespace WestWorld.Agents
{
    /// <summary>
    /// The House Keeper, Elsa, in the West World game.
    /// </summary>
    public class HouseKeeper : WestWorldAgent
    {
        /// <summary>
        /// Receive a message from the miner.
        /// Easy-AI doesn't out-of-the-box way to communicate with other agents, so this is an example system.
        /// You may want to explore adding a generic communication system into the base agent class.
        /// </summary>
        /// <param name="message">The message type received.</param>
        public override void ReceiveMessage(WestWorldMessage message)
        {
            // If the miner got home, start cooking stew.
            if (message == WestWorldMessage.HiHoneyImHome)
            {
                Log("Hi honey. Let me make you some of mah fine country stew.");
                SetState<CookStew>();
            }
            // Otherwise, the only other message type is that the stew is ready, so pass the message to the miner.
            else
            {
                Other.ReceiveMessage(WestWorldMessage.StewReady);
            }
        }

        protected override void Start()
        {
            base.Start();

            // Find the miner to communicate with.
            Other = FindObjectOfType<Miner>();
        }
    }
}