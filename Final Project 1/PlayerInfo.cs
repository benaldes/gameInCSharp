using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_1
{
    public class PlayerInfo
    {

        private static int _infoStatPostionX { get; set; } = 12;
        private static int _playerFaceX = 0;
        private static int _playerFaceY = 21;
        
        public static string[] _playerPic;
        public static char[,] _playerPicGrid;

        public static string[] _playerFrame;
        public static char[,] _playerFrameGrid;

        public static int _goldX = 51;
        public static int _goldY = 21;

        DungenGame game = new DungenGame() {};
        
        public static void PlayerInformation()
        {

            printPlayerName();
            printPlayerLevel();
            PrintGold();
            PrintHPBar();
            PrintManaBar();
            PrintPlayerDMG();
            PrintPlayerDodgeChance();
            PlayerFace();
            PrintPlayerItems();
            PrintPlayerBuffs();
        }
        public static void printPlayerName()
        {
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{DungenGame.PlayerUnit.Name}");
        }
        public static void printPlayerLevel()
        {
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Map {Levels.Level}");
        }
        public static void PrintGold()
        {
            Console.SetCursorPosition(_goldX, _goldY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Gold: {DungenGame.PlayerUnit.Gold} ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintHPBar()
        {
            if (DungenGame.PlayerUnit._playerHP > DungenGame.PlayerUnit._playerMaxHp)
                DungenGame.PlayerUnit._playerHP = DungenGame.PlayerUnit._playerMaxHp;
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 3);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"Health({DungenGame.PlayerUnit._playerHP}/{DungenGame.PlayerUnit._playerMaxHp}): ");
            Console.SetCursorPosition(_infoStatPostionX + 13, MapGenerator.BorderLeftToRightY + 3);
            //change to  color by the amount of HP the player has
            if (DungenGame.PlayerUnit._playerHP > DungenGame.PlayerUnit._playerMaxHp / 2)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (DungenGame.PlayerUnit._playerHP > DungenGame.PlayerUnit._playerMaxHp / 4)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            for (int i = 0;i < DungenGame.PlayerUnit._playerHP;i++)
            {
                
                Console.Write("|");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < (DungenGame.PlayerUnit._playerMaxHp - DungenGame.PlayerUnit._playerHP); i++)
            {

                Console.Write("|");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            
        }
        public static void PrintManaBar()
        {
            if (DungenGame.PlayerUnit._PlayerMana > DungenGame.PlayerUnit._playerMaxMana)
                DungenGame.PlayerUnit._PlayerMana = DungenGame.PlayerUnit._playerMaxMana;
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 4);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"Mana({DungenGame.PlayerUnit._PlayerMana}/{DungenGame.PlayerUnit._playerMaxMana}): ");
            Console.ForegroundColor= ConsoleColor.DarkBlue;
            Console.SetCursorPosition(_infoStatPostionX + 13, MapGenerator.BorderLeftToRightY + 4);
            for (int i = 0; i < DungenGame.PlayerUnit._PlayerMana; i++)
            {
                Console.Write("|");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;

            for (int i = 0;i<(DungenGame.PlayerUnit._playerMaxMana - DungenGame.PlayerUnit._PlayerMana);i++)
            {
               
                Console.Write("|");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintPlayerDMG()
        {
            
            
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 5);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"DMG: {DungenGame.PlayerUnit._dmg}");
            if (DungenGame.PlayerUnit.Buffs["Thunderstruck"] > 0)
                Console.Write($" (+2)");
            Console.ForegroundColor = ConsoleColor.Gray;
            
        }
        public static void PrintPlayerDodgeChance()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 6);
            Console.Write($"Dodge: {DungenGame.PlayerUnit.DodgeChance}%");
            if (DungenGame.PlayerUnit.Buffs["Boogie Man"] > 0)
                Console.Write($" (+20%)");
            else
                Console.Write("        ");

           

        }
        public static void PrintPlayerBuffs()
        {
            bool _nobuff = true;
            bool _first = false;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 9);
            foreach(String key in DungenGame.PlayerUnit.Buffs.Keys)
            {
                if (DungenGame.PlayerUnit.Buffs[key] > 0)
                {
                    _nobuff = false;
                    if (_first)
                        Console.Write(", ");
                    Console.Write($"\"{key}\" {DungenGame.PlayerUnit.Buffs[key]} turn");
                    _first = true;
                }

                
            }
            if (_nobuff)
                Console.Write("                                ");
            Console.ForegroundColor = ConsoleColor.White;


        }
        public static void PlayerFace()
        {
            // print the player frame
            PlayerReadFrame();
            PlayerFramePrint();

            // print the player face
            PlayerReadFace();
            PrintTheFace();
        }
        public static void PlayerReadFrame()
        {
            Console.ForegroundColor = ConsoleColor.White;
            //read Player Frame from text file to string
            _playerFrame = File.ReadAllLines($@"{DungenGame.Path}\Players\PlayerFrame.txt");
            //gives the Frame grid its limts
            _playerFrameGrid = new Char[_playerFrame.Length, _playerFrame[0].Length];

            //takes the map data from string format to metrix format
            for (int y = 0; y < _playerFrame.Length; y++)
            {
                for (int x = 0; x < _playerFrame[y].Length - 1; x++)
                {
                    _playerFrameGrid[y, x] = _playerFrame[y][x];
                }
            }
        }
        public static void PlayerFramePrint()
        {
            // prints the player Frame to console
            for (int y = 0; y < _playerFrameGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _playerFrameGrid.GetLength(1); x++)
                {
                    Console.SetCursorPosition(_playerFaceX + x, _playerFaceY + y);
                    Console.Write(_playerFrameGrid[y, x]);

                }
            }
        }
        public static void PlayerReadFace()
        {
            Console.ForegroundColor = ConsoleColor.White;
            //read map from text file to string
            _playerPic = File.ReadAllLines($@"{DungenGame.Path}\Players\Player-{DungenGame.PlayerUnit.Playerchar}.txt");
            //gives the map grid its limts
            _playerPicGrid = new Char[_playerPic.Length, _playerPic[0].Length];

            //takes the map data from string format to metrix format
            for (int y = 0; y < _playerPic.Length; y++)
            {
                for (int x = 0; x < _playerPic[y].Length - 1; x++)
                {
                    _playerPicGrid[y, x] = _playerPic[y][x];
                }
            }
        }
        public static void PrintTheFace()
        {
            // prints the player face to console
            for (int y = 0; y < _playerPicGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _playerPicGrid.GetLength(1); x++)
                {
                    Console.SetCursorPosition(_playerFaceX + x + 1, _playerFaceY + y + 1);
                    Console.Write(_playerPicGrid[y, x]);

                }
            }
        }        
        public static void PrintPlayerItems()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(_infoStatPostionX , MapGenerator.BorderLeftToRightY + 7);
            Console.Write($"HP   Potion:{DungenGame.PlayerUnit.Inventory["HP Potion"]} (\"Q\" to use)");
            Console.SetCursorPosition(_infoStatPostionX, MapGenerator.BorderLeftToRightY + 8);
            Console.Write($"Mana Potion:{DungenGame.PlayerUnit.Inventory["Mana Potion"]} (\"W\" to use)");
        }
        public static void PlayerIsHit()
        {
            for(int i = 0; i< 2; i++) 
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                PrintTheFace();
                Thread.Sleep(200);
                Console.ForegroundColor = ConsoleColor.White;
                PrintTheFace();
                Thread.Sleep(200);
            }
        }
        public static void UpdatePlayerInfo()
        {
            PrintPlayerItems();
            PrintPlayerBuffs();
            PrintHPBar();
            PrintManaBar();
            PrintPlayerDMG();
            PrintPlayerDodgeChance();
            PrintGold();
            Console.ForegroundColor = ConsoleColor.White;
            
        }
    }
}
