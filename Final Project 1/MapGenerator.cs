using System.Reflection;
using System.Runtime.CompilerServices;

namespace Final_Project_1
{
    public class MapGenerator
    {
        public static string Path;
        //string that gets the map data from the text file
        public static string[] MapData;
        //metrix that has the map data
        public static Char[,] MapGrid;
        public static int BorderLeftToRightY { get; set; } = 20;
        private static int _borderUpToDownX { get; set; } = 60;
        private static char _borderChar { get; set; } = '*';
        // contain the method for createing the test map
        public static void FirstTimeMapTest()
        {
            //read the map text file and enter the data into "MapGrid"
            ReadMapFromTextFile();

            //Print Map
            PrintMap();

            //gets Start point Locaiton and puts the player ther
            PlayerFirstSpawn();

        }
        //print the border between tabs
        public static void PrintTabBorder()
        {
            for (int i = 0; i < 120; i++)
            {
                Console.SetCursorPosition(i, BorderLeftToRightY);
                Console.Write(_borderChar);
            }
            for (int i = 0; i < 30; i++)
            {
                Console.SetCursorPosition(_borderUpToDownX, i);
                Console.Write(_borderChar);
            }
        }
        // read map from Text file
        
        public static void ReadMapFromTextFile()
        {
            //read map from text file to string
            MapData = File.ReadAllLines($@"{DungenGame.Path}\Maps\Map{Levels.Level}.txt");
            
            
            

            
            //gives the map grid its limts
            MapGrid = new Char[MapData.Length, MapData[0].Length];

            for (int y = 0; y < MapData.Length; y++)
            {
                for (int x = 0; x < MapData[y].Length; x++)
                {
                    MapGrid[y, x] = ' ';
                }
            }

            //takes the map data from string format to metrix format
            for (int y = 0; y < MapData.Length; y++)
            {
                for (int x = 0; x < MapData[y].Length; x++)
                {
                    MapGrid[y, x] = MapData[y][x];
                }
            }


        }
        // prints map 
        public static void PrintMap()
        {
            // Print the map to the console

            for (int y = 0; y < MapGrid.GetLength(0); y++)
            {
                for (int x = 0; x < MapGrid.GetLength(1); x++)
                {
                    if (MapGrid[y, x] == '<')
                    {

                        continue;
                    }
                    else if (MapGrid[y, x] == '$')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(x, y);
                        Console.Write(MapGrid[y, x]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (MapGrid[y, x] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(x, y);
                        Console.Write(MapGrid[y, x]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (MapGrid[y, x] == 'K')
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.SetCursorPosition(x, y);
                        Console.Write(MapGrid[y, x]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    else if (MapGrid[y, x] != '#' && MapGrid[y, x] != '!')
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(MapGrid[y, x]);
                    }
                    else if (MapGrid[y, x] == '!')
                    {
                        //print the enemy in red
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(x, y);
                        Console.Write(MapGrid[y, x]);
                        Console.ForegroundColor = ConsoleColor.White;

                        Enemy.GenerateEnemy(x, y, '!');//////// check when enemy complate
                    }

                }

            }
            
        }
        //return the positon of the chosen char
        public static (int, int) FindXInMap(Char _whatToFind)
        {
            int[] _spawnLoction = { 0, 0 };
            for (int i = 0; i < MapGrid.GetLength(0); i++)
            {
                for (int x = 0; x < MapGrid.GetLength(1); x++)
                {
                    if (MapGrid[i, x] == _whatToFind)
                    {
                        return (x, i);
                    }
                }
            }
            // if the method cant find the X it exits the code 
            Console.SetCursorPosition(0, MapGrid.GetLength(0) + 5);
            Console.Write($"cant find \"{_whatToFind}\"");
            Environment.Exit(0);
            return (0, 0);
        }
        //this method is for the first time the player spawns in a new map
        public static void PlayerFirstSpawn()
        {
            //Get the spawn Positon
            var _spawnLocaiton = FindXInMap('E');
            DungenGame.PlayerUnit.LoctionX = _spawnLocaiton.Item1;
            DungenGame.PlayerUnit.LoctionY = _spawnLocaiton.Item2;

            //moves the player to the positon of his x and y lociton
            Player.MovePlayerToItsPositon();
        }
        public static void DeleteTileKey()
        {
            for (int y = 0; y < MapGrid.GetLength(0); y++)
            {
                for (int x = 0; x < MapGrid.GetLength(1); x++)
                {
                    if (MapGrid[y, x] == '@')
                    {
                        MapGrid[y, x] = ' ';
                        Console.SetCursorPosition(x, y);
                        Console.Write(' ');
                    }
                        

                }
            }
        }
    }
}
