using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_1
{
    
    public  class Levels
    {
        // tracks what level the player is in 
        public static int Level = 1;
        public static bool InCombat = false;
        private static int _endGameX = 25;
        private static int _endGameY = 5;
        private static string _endGame = "congratulations you finished The Game";
        private static string[] _deadText;
        private static char[,] _deadTextGrid;
        public static void LevelTransition()
        {
            
            Enemy.EnemiesList.Clear();
            Console.Clear();
            Level +=  1;
            if(Level == 11)
                GameEnd();
            DungenGame.StartGame();
        }
        private static void GameEnd()
        {
            Console.Clear();
            Console.SetCursorPosition(_endGameX, _endGameY);
            Console.Write(_endGame);
            Console.SetCursorPosition(Console.WindowWidth-26, Console.WindowHeight-3);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

        }
        public static void PlayerIsDead()
        {
            Console.Clear();
            //Console.SetCursorPosition(_endGameX, _endGameY);
            //Console.Write(_playerDead);
            ReadTextFromTextFile();
            PrintText();
            Console.SetCursorPosition(Console.WindowWidth - 26, Console.WindowHeight - 3);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            MainMenu menu = new MainMenu();
            menu.Mainmenu();


        }
        public static void ReadTextFromTextFile()
        {
            //read map from text file to string
            _deadText = File.ReadAllLines($@"{DungenGame.Path}\Players\PlayerDead1.txt");
            //gives the map grid its limts
            _deadTextGrid = new Char[_deadText.Length, _deadText[0].Length];

            for (int y = 0; y < _deadText.Length; y++)
            {
                for (int x = 0; x < _deadText[y].Length; x++)
                {
                    _deadTextGrid[y, x] = ' ';
                }
            }

            //takes the map data from string format to metrix format
            for (int y = 0; y < _deadText.Length; y++)
            {
                for (int x = 0; x < _deadText[y].Length; x++)
                {
                    _deadTextGrid[y, x] = _deadText[y][x];
                }
            }


        }
        // prints map 
        public static void PrintText()
        {
            // Print the map to the console
            Console.ForegroundColor = ConsoleColor.Red;
            for (int y = 0; y < _deadTextGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _deadTextGrid.GetLength(1); x++)
                {            
                    Console.SetCursorPosition(_endGameX + x,_endGameY +  y);
                    Console.Write(_deadTextGrid[y, x]);
                    
                    
                }
                Thread.Sleep(100);

            }

        }
    }
}
