using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_1
{

    public class Shop
    {
        public List<Shop> Shoplist;
        private int _menuX = 62;
        private int _menuY = 27;
        private int _ShopIndex = 1;
        private string _item;
        private Dictionary<string, int> _itemsToSell = new Dictionary<string, int>();
        private List<string> _whatCanSell = new List<string>() { "HP Potion", "Mana Potion", "+1 DMG", "+5% Dodge", "+3 Max HP", "+3 Max Mana" };

        private Player player { get; set; }
        private int ShopPositonX;
        private int ShopPositonY;
        public Shop(Player player,int ShopPositonX,int ShopPositonY)
        {
         this.player = player;
         this.ShopPositonX = ShopPositonX;
         this.ShopPositonY = ShopPositonY;

        }


        public void EnterShop()
        {
            ShopPositonX = player.LoctionX + ShopPositonX;
            ShopPositonY = player.LoctionY + ShopPositonY;
            WhatToSell();
            MenuHighLight(0);

            PlayerInputShopMenu();
            
        }
        private void MenuHighLight(int move)
        {
            if ((_ShopIndex + move) >= 0 && (_ShopIndex + move) < (_itemsToSell.Count))
            {
                _ShopIndex += move;
                PrintShopMenu();
            }
        }
        private void PrintShopMenu()
        {
            Combat.CleanCombatLog_Menu();
            _menuX = 62;
            int index = 0;
            foreach (string options in _itemsToSell.Keys)
            {
                if (index == _ShopIndex)
                {
                    Combat.CombatLog($"the price is {_itemsToSell[options]} Gold");
                    Console.ForegroundColor = ConsoleColor.Red;
                    _item = options;
                }    
                Console.SetCursorPosition(_menuX, _menuY);
                Console.Write('┌');
                Console.SetCursorPosition(_menuX + 12, _menuY);
                Console.Write('┐');
                Console.SetCursorPosition(_menuX, _menuY + 2);
                Console.Write('└');
                Console.SetCursorPosition(_menuX + 12, _menuY + 2);
                Console.Write('┘');
                Console.SetCursorPosition((_menuX + 6) - (options.Length / 2), _menuY + 1);
                Console.Write(options);
                index++;
                _menuX += 15;
                Console.ForegroundColor = ConsoleColor.White;
                
            }
        }
        private void WhatToSell()
        {
            int _amountOfItems = Random.Shared.Next(2, 5);
            for (int i = 1; i <= _amountOfItems; i++)
            {
                int index = Random.Shared.Next(0, _whatCanSell.Count);
                int _price = Random.Shared.Next(3, 6);
                _itemsToSell.Add(_whatCanSell[index],_price);
                _whatCanSell.RemoveAt(index);
            }
        }

        private void PlayerInputShopMenu()
        {
           
            while(true)
            {
                bool keyIsPressed = false;
                while (!keyIsPressed)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        MenuHighLight(-1);
                        keyIsPressed = true;
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        MenuHighLight(1);
                        keyIsPressed = true;
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        DeleteSeller();
                        Player.PlayerSystem();
                    }
                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {

                        Buy();
                        break;
                    }
                    PlayerInputShopMenu();
                }
            }
            
        }
        private void Buy()
   
        {
            if (_itemsToSell[_item] <= player.Gold)
            {
                switch (_item)
                {
                    case "HP Potion":
                        player.Inventory["HP Potion"] += 1;
                            break;
                    case "Mana Potion":
                        player.Inventory["Mana Potion"] += 1;
                            break;
                    case "+1 DMG":
                        player._dmg += 1;
                            break;
                    case "+5% Dodge":
                        player.DodgeChance += 5;
                            break;
                    case "+3 Max HP":
                        player._playerMaxHp += 3;
                            break;
                    case "+3 Max Mana":
                        player._PlayerMana += 3;
                            break;

                }
                player.Gold -= _itemsToSell[_item];
                Combat.CombatLog($"You bought {_item}");
                _itemsToSell.Remove(_item);
                PlayerInfo.UpdatePlayerInfo();
                _ShopIndex = 0;
                Console.ReadKey();
                MenuHighLight(0);
                if (_itemsToSell.Count == 0)
                {
                    DeleteSeller();
                }

            }
            else
            {
                Combat.CombatLog("you Dont have enough Gold");
            }
        }
        private void DeleteSeller()
        {
            Combat.CleanCombatLog_Menu();
            Console.SetCursorPosition(ShopPositonX, ShopPositonY);
            Console.Write(' ');
            MapGenerator.MapGrid[ShopPositonY, ShopPositonX] = ' ';
            Player.PlayerSystem();
        }


    }
}
