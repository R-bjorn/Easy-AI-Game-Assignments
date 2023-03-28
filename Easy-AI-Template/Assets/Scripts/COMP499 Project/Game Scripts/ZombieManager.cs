using EasyAI;

namespace COMP499_Project.Game_Scripts
{
    public class ZombieManager : Manager
    {
        public static double PlayerInteractionRadius { get; set; }
        public static float StartingHunger { get; set; }
        public static int HungerRestoredFromEatingPlayer { get; set; }
        public static double ZombieInteractRadius { get; set; }
        public static float HungerRestoredFromEatingZombie { get; set; }
        public static double HungerChance { get; set; }
    }
}
