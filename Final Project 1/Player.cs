using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Final_Project_1
{
    public class Player
    {
        
        public string Name;
        public int LoctionX, LoctionY, _playerMaxHp, _playerHP,_dmg,_PlayerMana,_playerMaxMana,Level,Exp,ExpToLevelUp, DodgeChance,Gold;
        public List<string>  Spells;
        public Dictionary<string, int> Inventory;
        public char Playerchar;
        public static bool playerMoved;
        public Dictionary<string, int> Buffs;

        public Player(char Playerchar,int Gold ,int playerLocationX, int playerLocationY, int playerMaxHP, int PlayerHP,
            int PlayerDMG,int DodgeChance, int PlayerMana, int PlayerMaxMana, string PlayerName,int PlayerLevel,int PlayerExp,
            int PlayerExpToLevelUp, Dictionary<string,int> Inventory, List<string> Spells,Dictionary<string,int> Buffs) 
        {
            this.Playerchar = Playerchar;
            this.Gold = Gold;
            LoctionX = playerLocationX;
            LoctionY = playerLocationY;
            _playerMaxHp = playerMaxHP;
            _playerHP = PlayerHP;
            _dmg = PlayerDMG;
            _PlayerMana = PlayerMana;
            _playerMaxMana = PlayerMaxMana;
            Name = PlayerName;
            Exp = PlayerExp;
            ExpToLevelUp = PlayerExpToLevelUp;
            this.Inventory = Inventory;
            this.Spells = Spells;
            this.DodgeChance = DodgeChance;
            this.Buffs = Buffs;
        }     
        public static void PlayerSystem()
        {
            
            
                PlayerInputCheck();
            
            
        }
        // method for Unit Spawning in map
        public static void SpawnUnit(char UnitSpawnLetter ,char UnitName)
        {
            //return Unit Loction for Spawn
            var _result = MapGenerator.FindXInMap(UnitSpawnLetter);

            // write to player its new loction
            DungenGame.PlayerUnit.LoctionX = _result.Item1;
            DungenGame.PlayerUnit.LoctionY = _result.Item2;

            // prints the Unit to its Lociton
           // MapGenerator.MapMetrix[_result.Item1, _result.Item2 + 1] = UnitName;
        }
        public static void PlayerInputCheck()
        {
            bool isKeyPressed = false;
            
            
            do
            {
                if (DungenGame.PlayerUnit._playerHP <= 0)
                    Levels.PlayerIsDead();

                // Check if any key is available
                if (Console.KeyAvailable)
                {
                    // Read the key without blocking
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    
                    //clean the Log
                    Combat.CombatLog("");

                    // Check if the key is an arrow key and it's not currently pressed
                    if (keyInfo.Key == ConsoleKey.LeftArrow && !isKeyPressed)
                    {
                        MovePlayerIfClear(-1, 0);
                        isKeyPressed = true; // Mark the key as pressed
                        

                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow && !isKeyPressed)
                    {
                        MovePlayerIfClear(1, 0);
                        isKeyPressed = true; // Mark the key as pressed
                        
                    }
                    else if (keyInfo.Key == ConsoleKey.UpArrow && !isKeyPressed)
                    {
                        MovePlayerIfClear(0, -1);
                        isKeyPressed = true; // Mark the key as pressed
                        
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow && !isKeyPressed)
                    {
                        MovePlayerIfClear(0, 1);
                        isKeyPressed = true; // Mark the key as pressed
                        
                    }
                    else if (keyInfo.Key == ConsoleKey.Q && !isKeyPressed)
                    {
                        TakeHPPotion();                       
                    }
                    else if (keyInfo.Key == ConsoleKey.W && !isKeyPressed)
                    {
                        TakeManaPotion();                       
                    }
                    if (isKeyPressed && playerMoved)
                    {
                        Enemy.EnemyAlart();
                        isKeyPressed=false;
                    }
                        
                }
                else
                {
                    isKeyPressed = false; // Reset the key pressed state if no key is available
                }

                

            } while (!Levels.InCombat);
        }
        //check what is in the tile that the player wants to move to 
        public static void MovePlayerIfClear(int _directionX, int _directionY)
        {
            //keep track if the player moved 
            playerMoved = false;
            if (CheckIfPlayerCanGoToTile(_directionX, _directionY, 'G'))
            {
                ChangePlayerLocitonData(_directionX, _directionY);
                DungenGame.PlayerUnit.Gold += 1;
                PlayerInfo.PrintGold();
                MapGenerator.MapGrid[DungenGame.PlayerUnit.LoctionY, DungenGame.PlayerUnit.LoctionX] = ' ';
                Console.ForegroundColor = ConsoleColor.Yellow;
                Combat.CombatLog("you picked up a Gold Coin ");
                Console.ForegroundColor = ConsoleColor.White;
                playerMoved = true;
            }
            else if (CheckIfPlayerCanGoToTile(_directionX, _directionY, 'S'))
            {
                Shop shop = new Shop(DungenGame.PlayerUnit,_directionX,_directionY);
                shop.EnterShop();
            }
            else if (CheckIfPlayerCanGoToTile(_directionX, _directionY, 'K'))
            {
                MapGenerator.DeleteTileKey();
                ChangePlayerLocitonData(_directionX, _directionY);
                MapGenerator.MapGrid[DungenGame.PlayerUnit.LoctionY, DungenGame.PlayerUnit.LoctionX] = ' ';
                playerMoved = true;
            }
            else if (CheckIfPlayerCanGoToTile(_directionX, _directionY, 'X'))
            {
                Levels.LevelTransition();
            }
            else if (CheckIfPlayerCanGoToTile(_directionX, _directionY, '$'))
            {
                OpenTreasureChest();
                ChangePlayerLocitonData(_directionX, _directionY);
                MapGenerator.MapGrid[DungenGame.PlayerUnit.LoctionY, DungenGame.PlayerUnit.LoctionX] = ' ';
                playerMoved = true;
            }
            else if (CheckIfPlayerCanGoToTile(_directionX, _directionY, '#'))
            {

                PlayerTakeTrapDMG();
                ChangePlayerLocitonData(_directionX, _directionY);
                playerMoved = true;

            }
             else if (CheckIfPlayerCanGoToTile(_directionX, _directionY,' '))
            {
                ChangePlayerLocitonData(_directionX, _directionY);
                playerMoved = true;
            }
        }
        //check if the tile that the player wants to go to is clear
        public static bool CheckIfPlayerCanGoToTile(int _directionX, int _directionY, char _tile)
        {
            if (MapGenerator.MapGrid[DungenGame.PlayerUnit.LoctionY + _directionY, DungenGame.PlayerUnit.LoctionX + _directionX] == _tile )
            {
                return true;
            }
            return false;
        }
        //sets the x and y lociton of player in his data
        public static void ChangePlayerLocitonData(int _directionX, int _directionY)
        {
            //clean the tile that the player was on
            CleanAfterPlayer(); 

            DungenGame.PlayerUnit.LoctionX += _directionX;
            DungenGame.PlayerUnit.LoctionY += _directionY;

            //moves the player to the positon of his x and y lociton
            MovePlayerToItsPositon();
        }
        //moves the player to the positon of his x and y lociton
        public static void MovePlayerToItsPositon()
        {
            // Store the current cursor position
            int previousCursorLeft = Console.CursorLeft;
            int previousCursorTop = Console.CursorTop;

            //Prints the map again so the player previous is cleard
            //MapGenerator.PrintMap();

            // Set the cursor position to the player's location
            Console.SetCursorPosition(DungenGame.PlayerUnit.LoctionX, DungenGame.PlayerUnit.LoctionY);

            // Print the player character
            Console.Write(DungenGame.PlayerUnit.Playerchar);

            // Restore the previous cursor position
            Console.SetCursorPosition(previousCursorLeft, previousCursorTop);
            
        }
        public static void CleanAfterPlayer()
        {      
            Console.SetCursorPosition(DungenGame.PlayerUnit.LoctionX, DungenGame.PlayerUnit.LoctionY);
            if(MapGenerator.MapGrid[DungenGame.PlayerUnit.LoctionY, DungenGame.PlayerUnit.LoctionX] == '#')
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else 
                Console.ForegroundColor = ConsoleColor.White;           
            Console.Write(MapGenerator.MapGrid[DungenGame.PlayerUnit.LoctionY, DungenGame.PlayerUnit.LoctionX]);
            Console.SetCursorPosition(0, MapGenerator.MapGrid.GetLength(0) + 5);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PlayerTakeTrapDMG()
        {
            int _trapDmg = Random.Shared.Next(1, 3);
            DungenGame.PlayerUnit._playerHP -= _trapDmg;
            PlayerInfo.PrintHPBar();

            //Prints to log that the player is on a trap 
            Console.ForegroundColor= ConsoleColor.Red;
            Combat.CombatLog($"you steped on a trap and recived {_trapDmg} DMG");
            Console.ForegroundColor= ConsoleColor.White;
        }
        public static void OpenTreasureChest()
        {
            int _random = Random.Shared.Next(0, 3);
            switch ( _random ) 
            {
                case 0:
                    if(DungenGame.PlayerUnit.Inventory.ContainsKey("HP Potion"))
                    {
                        DungenGame.PlayerUnit.Inventory["HP Potion"] += 1;
                    }
                    else
                    {
                        DungenGame.PlayerUnit.Inventory.Add("HP Potion",1); 
                    }
                    Combat.CombatLog("you got a HP Potion");
                    break;

                case 1:
                    if (DungenGame.PlayerUnit.Inventory.ContainsKey("HP Potion"))
                    {
                        DungenGame.PlayerUnit.Inventory["Mana Potion"] += 1;
                    }
                    else
                    {
                        DungenGame.PlayerUnit.Inventory.Add("Mana Potion", 1);
                    }
                    Combat.CombatLog("you got a Mana Potion");
                    break;

                    case 2:
                    {
                        int _gold = Random.Shared.Next(2,10);
                        DungenGame.PlayerUnit.Gold += _gold;
                        Combat.CombatLog($"you got a {_gold} gold");
                    }
                    break;
            }
            PlayerInfo.UpdatePlayerInfo();

        }        
        public static void TakeHPPotion()
        {
            if (DungenGame.PlayerUnit.Inventory["HP Potion"] > 0)
            {
                DungenGame.PlayerUnit._playerHP += 5;
                DungenGame.PlayerUnit.Inventory["HP Potion"] -= 1;
                Combat.CombatLog("You used a HP Potion and got 5 HP");
            }
            else
                Combat.CombatLog("you dont have a HP Potion");
            
            PlayerInfo.PrintHPBar();
            PlayerInfo.PrintPlayerItems();

        }
        public static void TakeManaPotion()
        {
            if (DungenGame.PlayerUnit.Inventory["Mana Potion"] > 0)
            {
                DungenGame.PlayerUnit._PlayerMana += 5;
                DungenGame.PlayerUnit.Inventory["Mana Potion"] -= 1;
                Combat.CombatLog("You used a Mana Potion and got 5 Mana");
            }
            else
                Combat.CombatLog("you dont have a Mana Potion");
            
            PlayerInfo.PrintManaBar();
            PlayerInfo.PrintPlayerItems();

        }
    }
    
}
