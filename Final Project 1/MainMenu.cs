namespace Final_Project_1
{
    public class MainMenu
    {
        private string _path = DungenGame.Path;

        private string _gameName = "Welcome To My Dungen";
        private  string[] _deadText;
        private  char[,] _deadTextGrid;
        private  int _gameNameX = 30;
        private  int _gameNameY = 0;

        private string _startGame = "Start Game";
        private int _startGameX = 49;
        private int _startGameY = 13;

        private string _changeAvatar = "Change Avatar";
        private int _changeAvatarX = 50;
        private int _changeAvatarY = 17;

        private string _credits = "Credits";
        private int _CreditsX = 51;
        private int _CreditsY = 21;

        private string _exit = "Exit";
        private int _exitX = 52;
        private int _exitY = 25;

        private int _menuindex = 1;

        private List<char> _avatars = new List<char>() { 'P','A','H','T'};
        private int _avatarX = 37;
        private int _avatarY = 10;
        private string[] _playerFrame;
        private char[,] _playerFrameGrid;
        private string[] _playerPic;
        private char[,] _playerPicGrid;
        private int _avatarMenuindex = 1;
        private char _playerAvatarPick = 'P';

        public MainMenu() { }
        public void Mainmenu()
        {

            Console.Clear();
            GameName();
            MenuHighLight(0);
            while (true)
            {
                PlayerInputMainMenu();
            }

        }
        private void PlayerInputMainMenu()
        {
            bool keyIsPressed = false;
            while (!keyIsPressed)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    MenuHighLight(-1);
                    keyIsPressed = true;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    MenuHighLight(1);
                    keyIsPressed = true;
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
            if ((_menuindex + move) >= 1 && (_menuindex + move) <= (4))
            {
                _menuindex += move;
                PrintMenu();

                switch (_menuindex)
                {
                    case 1:
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            GameStart();
                        }
                        break;
                    case 2:
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            ChangeAvatar();
                        }
                        break;
                    case 3:
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Credits();
                        }
                        break;
                    case 4:
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Exit();
                        }
                        break;
                    
                }
            }
        }
        private void OptionPicked()
        {
            switch (_menuindex)
            {
                ///main menu options
                case 1:
                    {
                        DungenGame.StartGame();
                    }
                    break;
                case 2:
                    {
                        Console.Clear();
                        ChangeAvatars();
                    }
                    break;
                case 3:
                    {
                        printCredits();
                    }
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }
        private void printCredits()
        {
            Console.ForegroundColor= ConsoleColor.White;
            Console.Clear();
            Console.SetCursorPosition(_changeAvatarX, _changeAvatarY);
            Console.Write("made by ben aldes");
            Console.ReadKey();
            Console.Clear();
            Mainmenu();

        }
        ///////////////////////////// print main option
        private void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
                        
            GameStart();
            ChangeAvatar();
            Credits();
            Exit();

        }
        private void GameName()
        {
            ReadTextFromTextFile();
            PrintText();
            
        }
        private void GameStart()
        {
            Console.SetCursorPosition(_startGameX, _startGameY);
            Console.Write('┌');
            Console.SetCursorPosition(_startGameX + 18, _startGameY);
            Console.Write('┐');
            Console.SetCursorPosition(_startGameX, _startGameY + 2);
            Console.Write('└');
            Console.SetCursorPosition(_startGameX + 18, _startGameY + 2);
            Console.Write('┘');
            Console.SetCursorPosition((_startGameX + 9) - (_startGame.Length / 2), _startGameY + 1);
            Console.Write(_startGame);
        }
        private void ChangeAvatar()
        {
            Console.SetCursorPosition(_changeAvatarX, _changeAvatarY);
            Console.Write('┌');
            Console.SetCursorPosition(_changeAvatarX + 16, _changeAvatarY);
            Console.Write('┐');
            Console.SetCursorPosition(_changeAvatarX, _changeAvatarY + 2);
            Console.Write('└');
            Console.SetCursorPosition(_changeAvatarX + 16, _changeAvatarY + 2);
            Console.Write('┘');
            Console.SetCursorPosition((_changeAvatarX + 8) - (_changeAvatar.Length / 2), _changeAvatarY + 1);
            Console.Write(_changeAvatar);
        }
        private void Credits()
        {
            Console.SetCursorPosition(_CreditsX, _CreditsY);
            Console.Write('┌');
            Console.SetCursorPosition(_CreditsX + 14, _CreditsY);
            Console.Write('┐');
            Console.SetCursorPosition(_CreditsX, _CreditsY + 2);
            Console.Write('└');
            Console.SetCursorPosition(_CreditsX + 14, _CreditsY + 2);
            Console.Write('┘');
            Console.SetCursorPosition((_CreditsX + 7) - (_credits.Length / 2), _CreditsY + 1);
            Console.Write(_credits);
        }
        private void Exit()
        {
            Console.SetCursorPosition(_exitX, _exitY);
            Console.Write('┌');
            Console.SetCursorPosition(_exitX + 12, _exitY);
            Console.Write('┐');
            Console.SetCursorPosition(_exitX, _exitY + 2);
            Console.Write('└');
            Console.SetCursorPosition(_exitX + 12, _exitY + 2);
            Console.Write('┘');
            Console.SetCursorPosition((_exitX + 6) - (_exit.Length / 2), _exitY + 1);
            Console.Write(_exit);
        }
        ///////////////////////////
        private void ChangeAvatars()
        {
            _menuindex = 1;
            AvatarMenuPicked();
            while(true)
            {
                AvatarMenuInput();
            }
            
        }
        private void PlayerReadFrame()
        {
            Console.ForegroundColor = ConsoleColor.White;
            //read Player Frame from text file to string
            _playerFrame = File.ReadAllLines($@"{_path}\Players\PlayerFrame.txt");
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
        private void PlayerFramePrint()
        {

            // prints the player Frame to console
            for (int y = 0; y < _playerFrameGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _playerFrameGrid.GetLength(1); x++)
                {
                    Console.SetCursorPosition(_avatarX + x, _avatarY + y);
                    Console.Write(_playerFrameGrid[y, x]);

                }
            }
        }
        private void PlayerReadFace(char _avatarChar)
        {
            Console.ForegroundColor = ConsoleColor.White;
            //read map from text file to string
            _playerPic = File.ReadAllLines($@"{_path}\Players\Player-{_avatarChar}.txt");
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
        private void PrintTheFace()
        {
            // prints the player face to console
            for (int y = 0; y < _playerPicGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _playerPicGrid.GetLength(1); x++)
                {
                    Console.SetCursorPosition(_avatarX + x + 1, _avatarY + y + 1);
                    Console.Write(_playerPicGrid[y, x]);

                }
            }
        }
        private void AvatarMenuInput()
        {
            bool keyIsPressed = false;
            while (!keyIsPressed)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    AvatarMenuHighLight(1);
                    keyIsPressed = true;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    AvatarMenuHighLight(-1);
                    keyIsPressed = true;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {

                    AvatarNameChange();
                    DungenGame.PlayerUnit.Playerchar = _playerAvatarPick;
                    Console.Clear();
                    GameName();
                    Mainmenu();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    GameName();
                    Mainmenu();
                    break;
                }

            }
        }
        private void AvatarMenuHighLight(int move)
        {
            if ((_avatarMenuindex + move) >= 1 && (_avatarMenuindex + move) <= (4))
            {
                _avatarMenuindex += move;

                switch (_avatarMenuindex)
                {
                    case 1:
                        {
                            _playerAvatarPick = 'P';
                            
                        }
                        break;
                    case 2:
                        {
                            _playerAvatarPick = 'A';

                        }
                        break;
                    case 3:
                        {
                            _playerAvatarPick = 'H';

                        }
                        break;
                    case 4:
                        {
                            _playerAvatarPick = 'T';
                            
                        }
                        break;

                }
                AvatarMenuPicked();
            }
        }
        private void AvatarMenuPicked()
        {
            
            PlayerReadFrame();
            _avatarX =37;
            foreach (var avatar in _avatars)
            {
                if(avatar == _playerAvatarPick)
                    Console.ForegroundColor = ConsoleColor.Red;
                PlayerFramePrint();
                Console.ForegroundColor = ConsoleColor.White;
                PlayerReadFace(avatar);
                PrintTheFace();
                _avatarX += 13;
            }
        }
        private void AvatarNameChange()
        {
            switch(_playerAvatarPick)
            {
                case 'P':
                    DungenGame.PlayerUnit.Name = "P warrior";
                    break;
                case 'A':
                    DungenGame.PlayerUnit.Name = "A warrior";
                    break;
                case 'H':
                    DungenGame.PlayerUnit.Name = "H warrior";
                    break;
                case 'T':
                    DungenGame.PlayerUnit.Name = "T warrior";
                    break;

            }

        }
        private void ReadTextFromTextFile()
        {
            //read map from text file to string
            _deadText = File.ReadAllLines($@"{_path}\EneterGame.txt");
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
        private void PrintText()
        {
            // Print the map to the console
            
            for (int y = 0; y < _deadTextGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _deadTextGrid.GetLength(1); x++)
                {
                    Console.SetCursorPosition(_gameNameX + x, _gameNameY + y);
                    Console.Write(_deadTextGrid[y, x]);


                }
                

            }

        }
    }
}
