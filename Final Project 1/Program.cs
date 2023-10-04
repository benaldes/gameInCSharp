// C# Final Project(Dor BenDor)
//         ben aldes
// ----------------------------
using System.Runtime.CompilerServices;

namespace Final_Project_1
{
    class DungenGame
    {
        // declare the static player instnce
        public static Player PlayerUnit;
        public static string Path;
        static void Main(string[] args)
        {
            Path = GetThisFilePath();
            Path = Path.Replace("Program.cs", "");
            
            // declare the player
            PlayerUnit = new Player('p',0, default, default, 30, 30, 3, 10, 10, 10, "P warrior", 10, 5, 10,
                new Dictionary<string, int>() { { "HP Potion", 0 }, { "Mana Potion", 0 } },
                new List<string>() { "Boogie Man", "Ballbreaker", "Thunderstruck" },
                new Dictionary<string, int>() { { "Boogie Man", 0 },{ "Thunderstruck", 0 } }); 

            // let the game use UTF*
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Change the console title
            Console.Title = "Dungen Game";

            // makes the curser invisible
            Console.CursorVisible = false;

            //start the Main Menu
            MainMenu menu = new MainMenu();
            menu.Mainmenu();
        }
        public static void StartGame()
        {
            Console.Clear();

            //rest Console Colors
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            //read map from text file and prints it to console
            MapGenerator.FirstTimeMapTest();
            MapGenerator.PrintTabBorder();
            PlayerInfo.PlayerInformation();

            //moves player 
            Player.PlayerSystem();
        }

        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return path;
        }
    }
}