namespace Final_Project_1
{
    public class Combat
    {
        public static Point _combatLogPositon = new Point(62, 21);
        private Point _combatMenuPositon = new Point(62, 27);

        private bool _playerTurn = true;

        // keep track of what combat menu the player is in 
        private int _menuIndex = 0;

        //keep track of what option is now highlightd in the combat menu
        private int _menuHighLight = 0;

        //is somthing in the middale of Animation
        private bool _inAnimation = false;

        private Player Player { get; set; }
        private Enemy Enemy { get; set; }

        private string[] _mainCombatMenuOptions = { "attack", "spells", "items" };
        private List<string> _combatMenuOptions = new List<string>() { };

        public Combat()
        {
            Random random = new Random();
        }

        public Combat(Player player, Enemy enemy)
        {
            Player = player;
            Enemy = enemy;
        }
        public static void StartFight(Player player, Enemy _enemy)
        {
            //set the Game To Combat mode 
            Levels.InCombat = true;

            // create a class for enemy info and animation print 
            EnemyInfo enemy = new EnemyInfo(_enemy);
            EnemyInfo.firstTimeEnemyApear = true;
            enemy.PrintEnemyInfo();
            EnemyInfo.firstTimeEnemyApear = false;


            // create a class for Combat
            Combat fight = new Combat(player, _enemy);
            fight.combat();



        }
        private void combat()
        {
            //adds the combat menu and log
            ControlCombatMenu();

            //set the combat log for the first time you meet an enemy
            CombatLog($"you encountered {Enemy.Name}");

            while (true)
            {

                if (Player._playerHP > 0 && _playerTurn)
                {
                    PlayerInfo.UpdatePlayerInfo();
                    PlayerInputCombatMenu();
                    PlayerInfo.UpdatePlayerInfo();
                }
                else if (Player._playerHP <= 0)
                {
                    Levels.PlayerIsDead();
                }
                /// prints enemy info
                EnemyInfo enemy = new EnemyInfo(Enemy);
                enemy.PrintEnemyInfo();




                if (Enemy.HP > 0 && !_playerTurn)
                {

                    if (Enemy.IsStunned == 0)
                    {

                        EnemyTurn();


                    }
                    else
                    {
                        _playerTurn = true;
                        Enemy.IsStunned--;
                        EnemyInfo enemy1 = new EnemyInfo(Enemy);
                        enemy1.PrintEnemyDebuff();
                    }
                    BuffGoDown();

                }
                else if (Enemy.HP <= 0)
                {
                    EnemyIsDead();
                    Console.ReadKey();
                }
            }
        }
        //return game state to not in combat
        private void EnemyIsDead()
        {
            Enemy.CleanAfterEnemy(Enemy);
            EnemyDropGold();
            CleanCombatScreen();
            CombatLog($"{Enemy.Name} is dead");
            Enemy.EnemiesList.Remove(Enemy);
            Levels.InCombat = false;
            Player.PlayerSystem();

        }
        private void EnemyDropGold()
        {
            int _dropGold = Random.Shared.Next(1, 2);
            if (_dropGold == 1)
            {
                Console.SetCursorPosition(Enemy.Positon.X, Enemy.Positon.Y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                MapGenerator.MapGrid[Enemy.Positon.Y, Enemy.Positon.X] = 'G';
                Console.Write('G');
                Console.ForegroundColor = ConsoleColor.White;
            }

        }
        // clean combat screen
        private void CleanCombatScreen()
        {
            CleanEnemy();
            CleanCombatLog_Menu();
        }
        private void CleanEnemy()
        {
            for (int i = 61; i < 120; i++)
            {
                for (int y = 0; y < 20; y++)
                {
                    Console.SetCursorPosition(i, y);
                    Console.Write(' ');
                }
            }
        }
        public static void CleanCombatLog_Menu()
        {
            for (int i = 61; i < 120; i++)
            {
                for (int y = 21; y < 30; y++)
                {
                    Console.SetCursorPosition(i, y);
                    Console.Write(' ');
                }
            }
        }
        //reads the player input while in combat menu
        private void PlayerInputCombatMenu()
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
                    _menuIndex = 0;
                    ControlCombatMenu();
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {

                    OptionPicked();
                    break;
                }
            }
        }
        // changes the highlighted option
        private void MenuHighLight(int move)
        {
            if ((_menuHighLight + move) >= 0 && (_menuHighLight + move) < (_combatMenuOptions.Count))
            {
                _menuHighLight += move;
                PrintCombatMenu(_combatMenuOptions);

                switch (_combatMenuOptions[_menuHighLight])
                {
                    case "Boogie Man":
                        {
                            CombatLog("+20 Dodge Chance for 3 turn (Mana Cost: 5)");
                        }
                        break;
                    case "Ballbreaker":
                        {
                            CombatLog("stun enemy for 2 turn (Mana Cost: 5) ");
                        }
                        break;
                    case "Thunderstruck":
                        {
                            CombatLog("+2 DMG for 3 turn (Mana Cost: 5)");
                        }
                        break;
                    case "HP potion":
                        CombatLog("Add 5 HP to player");
                        break;
                    case "Mana potion":
                        CombatLog("Add 5 Mana to player");
                        break;
                }
            }
        }
        //recive player menu choice and switch case of menu options
        private void OptionPicked()
        {
            switch (_combatMenuOptions[_menuHighLight])
            {
                ///main menu options
                case "attack":
                    {

                        PlayerAttack();
                        //adds the "press Enter to continue"
                        PressEnterTo();

                    }
                    break;
                case "spells":
                    {
                        _menuIndex = 1;
                        ControlCombatMenu();
                        _menuHighLight = 0;
                        MenuHighLight(0);
                    }
                    break;
                case "items":
                    {
                        _menuIndex = 2;
                        ControlCombatMenu();
                        _menuHighLight = 0;
                        MenuHighLight(0);
                    }
                    break;


                /// Spell options
                case "Boogie Man":
                    BoogieMan();
                    break;
                case "Ballbreaker":
                    Ballbreaker();
                    PressEnterTo();
                    break;
                case "Thunderstruck":
                    Thunderstruck();
                    break;

                /// item options
                case "HP Potion":
                    Player.TakeHPPotion();
                    break;
                case "Mana Potion":
                    Player.TakeManaPotion();
                    break;

            }
        }
        public static void CombatLog(string Log)
        {
            Console.SetCursorPosition(_combatLogPositon.X, _combatLogPositon.Y);
            Console.WriteLine("                                                   ");
            Console.SetCursorPosition(_combatLogPositon.X, _combatLogPositon.Y);
            Console.Write(Log);
        }
        //chose what menu to print to the console 
        private void ControlCombatMenu()
        {

            switch (_menuIndex)
            {
                case 0:
                    {
                        CleanCombatLog_Menu();
                        _combatMenuOptions.Clear();
                        _combatMenuOptions.AddRange(_mainCombatMenuOptions);
                        PrintCombatMenu(_combatMenuOptions);
                    }
                    break;
                case 1:
                    {

                        if (Player.Spells.Count > 0)
                        {
                            _menuHighLight = 0;
                            CleanCombatLog_Menu();
                            _combatMenuOptions.Clear();
                            _combatMenuOptions.AddRange(Player.Spells);
                            PrintCombatMenu(_combatMenuOptions);
                        }
                        else
                        {
                            CombatLog("You have no spells");
                        }
                    }
                    break;
                case 2:
                    {
                        if (Player.Inventory.Count > 0)
                        {
                            _menuHighLight = 0;
                            CleanCombatLog_Menu();
                            _combatMenuOptions.Clear();
                            _combatMenuOptions.AddRange(Player.Inventory.Keys);
                            PrintCombatMenu(_combatMenuOptions);
                        }
                        else
                        {
                            CombatLog("You have no Items");
                        }
                    }
                    break;
            }
        }
        // prints the combat menu 
        private void PrintCombatMenu(List<string> _menuOptions)
        {

            int _menuX = _combatMenuPositon.X;
            int _menuY = _combatMenuPositon.Y;


            foreach (string options in _menuOptions)
            {
                if (options == _menuOptions[_menuHighLight] && !_inAnimation)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
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


                _menuX += 15;
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        // Adds the "press enter to continue"
        private void PressEnterTo()
        {
            while (true)
            {
                Console.SetCursorPosition(_combatLogPositon.X, _combatLogPositon.Y + 2);
                Console.Write("press Enter to continue");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Console.SetCursorPosition(_combatLogPositon.X, _combatLogPositon.Y + 2);
                Console.Write("                       ");
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }

        }

        private bool ChanceToHitEnemy(int DodgeChance)
        {
            int Chance = Random.Shared.Next(0, 100);
            if (Chance < DodgeChance)
                return false;

            return true;
        }
        private bool ChanceToHitPlayer(int DodgeChance)
        {
            if (DungenGame.PlayerUnit.Buffs["Boogie Man"] > 0)
                DodgeChance += 20;
            int Chance = Random.Shared.Next(0, 100);
            if (Chance < DodgeChance)
                return false;

            return true;
        }
        private void BuffGoDown()
        {
            foreach (string item in DungenGame.PlayerUnit.Buffs.Keys)
            {
                DungenGame.PlayerUnit.Buffs[item] -= 1;
            }
        }

        ////////player actions

        // player basic attack 
        private void PlayerAttack()
        {
            if (ChanceToHitEnemy(Enemy.DodgeChance))
            {
                CombatLog($"{Enemy.Name} Received {Player._dmg} DMG");
                Enemy.HP -= Player._dmg;
                if (DungenGame.PlayerUnit.Buffs["Thunderstruck"] > 0)
                {
                    CombatLog($"{Enemy.Name} Received {Player._dmg + 2} DMG");
                    Enemy.HP -= 2;
                }

                EnemyInfo enemy = new EnemyInfo(Enemy);
                enemy.PrintEnemyInfo();
                enemy.EnemyHitAnimaiton();
            }
            else
            {
                CombatLog("you missed");
            }

            //lets the enemy know its his turn
            _playerTurn = false;
            //Console.ReadKey();
        }
        private void Ballbreaker()
        {
            if (Player._PlayerMana >= 5)
            {
                Player._PlayerMana -= 5;
                PlayerInfo.PrintManaBar();
                if (ChanceToHitEnemy(Enemy.DodgeChance))
                {
                    CombatLog($"you hit {Enemy.Name} with \"Ballbreaker\" ");
                    Enemy.IsStunned += 2;
                }
                else
                {
                    CombatLog("you missed");

                }
                _playerTurn = false;
            }
            else
            {
                CombatLog("you dont have enough Mana");
            }



        }
        private void BoogieMan()
        {
            if (Player._PlayerMana >= 5)
            {
                Player._PlayerMana -= 5;
                DungenGame.PlayerUnit.Buffs["Boogie Man"] = 3;
                PlayerInfo.PrintPlayerDodgeChance();
                PlayerInfo.PrintManaBar();
                _playerTurn = false;
            }
            else
            {
                CombatLog("you dont have enough Mana");
            }


        }

        private void Thunderstruck()
        {
            if (Player._PlayerMana >= 5)
            {
                Player._PlayerMana -= 5;
                DungenGame.PlayerUnit.Buffs["Thunderstruck"] = 3;
                PlayerInfo.PrintPlayerDMG();
                PlayerInfo.PrintManaBar();
                _playerTurn = false;
            }
            else
            {
                CombatLog("you dont have enough Mana");
            }
        }



        ///////// Enemy actions

        //decides enemy action
        private void EnemyTurn()
        {
            EnemyAttack();
            PlayerInfo.PrintHPBar();
            PressEnterTo();
        }
        private void EnemyAttack()
        {
            if (ChanceToHitPlayer(Player.DodgeChance))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                CombatLog($"You Received {Enemy.DMG} DMG");
                Console.ForegroundColor = ConsoleColor.White;
                Player._playerHP -= Enemy.DMG;
                PlayerInfo.PlayerIsHit();
                PlayerInfo.PrintHPBar();
            }
            else
            {
                CombatLog("The enemy missed you");

            }


            _playerTurn = true;

        }


    }
}
