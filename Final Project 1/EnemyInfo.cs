using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_1
{
    public class EnemyInfo
    {
        //if it is the first time an enemy appear it will have some animation
        public static bool firstTimeEnemyApear = true;

        private int _enemyInfoBarPositonX = 62;
        private int _enemyInfoBarPositonY = 14;

        private int _enemypicturePositonX = 86;
        private int _enemypicturePositonY = 7;

        

        //string that gets the map data from the text file
        private string[] EnemyPic;
        //metrix that has the map data
        private Char[,] EnemyPicGrid;

        private Enemy enemy { get; set; }
        public EnemyInfo(Enemy _enemy) 
        {
            enemy = _enemy;
        }
       

        public void PrintEnemyInfo()
        {
            PrintEnemyPicture();
            PrintEnemyNameAndLevel();
            PrintEnemyHP();
            PrintEnemyDMG();
            PrintEnemyDodgeChance();
            PrintEnemyDebuff();



            Console.ForegroundColor = ConsoleColor.White;
        }
       

        private void PrintEnemyPicture()
        {
            
            ReadEnemyPicture();
            PrintEnemyPic();
        }
        private void ReadEnemyPicture()
        {
            //read map from text file to string
            EnemyPic = File.ReadAllLines($@"{DungenGame.Path}\enemy-{enemy.Character}.txt");
            //gives the map grid its limts
            EnemyPicGrid = new Char[EnemyPic.Length, EnemyPic[0].Length];

            for (int y = 0; y < EnemyPic.Length; y++)
            {
                for (int x = 0; x < EnemyPic[y].Length; x++)
                {
                    EnemyPicGrid[y, x] = ' ';
                }
            }

            //takes the map data from string format to metrix format
            for (int y = 0; y < EnemyPic.Length; y++)
            {
                for (int x = 0; x < EnemyPic[y].Length; x++)
                {
                    EnemyPicGrid[y, x] = EnemyPic[y][x];
                }
            }
        }

        private void PrintEnemyPic()
        {
            
            for (int y = 0; y < EnemyPicGrid.GetLength(0); y++)
            {
                for (int x = 0; x < EnemyPicGrid.GetLength(1); x++)
                {  
                    Console.SetCursorPosition(x + _enemypicturePositonX, y + _enemypicturePositonY);
                    Console.Write(EnemyPicGrid[y, x]);
                    if(firstTimeEnemyApear)
                    {
                        Thread.Sleep(20);
                    }  
                }
            }
        }

        private void PrintEnemyNameAndLevel() 
        {
            Console.SetCursorPosition(_enemyInfoBarPositonX, _enemyInfoBarPositonY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(enemy.Name);
            
            Console.ForegroundColor = ConsoleColor.White;
            
        }
        
        private void PrintEnemyHP()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(_enemyInfoBarPositonX,_enemyInfoBarPositonY+1);
            if(enemy.HP < 0) {enemy.HP = 0;}
            Console.Write($"Health({enemy.MaxHP}/{enemy.HP}):");
            
            if(enemy.HP > enemy.MaxHP / 2)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (enemy.HP > enemy.MaxHP / 4)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            for (int i = 0; i < enemy.HP; i++)
            {

                Console.Write("|");
                if (firstTimeEnemyApear)
                {
                    Thread.Sleep(35);
                }
                
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < (enemy.MaxHP - enemy.HP); i++)
            {

                Console.Write("|");
                
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void PrintEnemyDMG()
        {
            Console.SetCursorPosition(_enemyInfoBarPositonX, _enemyInfoBarPositonY + 2);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"DMG: {enemy.DMG}");
            Console.ForegroundColor = ConsoleColor.White;

        }

        private void PrintEnemyDodgeChance()
        {
            Console.SetCursorPosition(_enemyInfoBarPositonX, _enemyInfoBarPositonY + 3);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"Dodge: {enemy.DodgeChance}%");
            Console.ForegroundColor = ConsoleColor.White;
        }

         public void PrintEnemyDebuff()
        {
            if(enemy.IsStunned > 0)
            {
                Console.SetCursorPosition(_enemyInfoBarPositonX, _enemyInfoBarPositonY + 4);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"stunned for {enemy.IsStunned} turns");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.SetCursorPosition(_enemyInfoBarPositonX, _enemyInfoBarPositonY + 4);        
                Console.Write("                                        ");
            }
         }
            
        

        // enemy Hit animaiton
        public void EnemyHitAnimaiton()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                PrintEnemyPic();
                Thread.Sleep(200);
                Console.ForegroundColor = ConsoleColor.White;
                PrintEnemyPic();
                Thread.Sleep(200);
            }
        }
    }
}
