using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_1
{
    public class Enemy
    {
        public string Name;
        public int  HP, MaxHP, DMG, DodgeChance, IsStunned;
        public bool FirstMovement;
        public bool Alert;
        public bool InCombat;
        public Char Character;
        public static List<Enemy> EnemiesList = new List<Enemy>();
        public Point Positon;
        
        public Enemy()
        {
            Random random = new Random();
        }
        public Enemy(String Name,Point Positon, int HP,int MaxHP, int DMG,bool 
            FirstMovement,bool InCombat,bool Alert,int IsStunned,int DodgeChance, char Character) 
        {
            this.Name = Name;
            this.Positon = Positon;
            this.HP = HP;
            this.MaxHP = MaxHP;
            this.DMG = DMG;
            this.Character = Character;
            this.Alert = Alert;
            this.FirstMovement = FirstMovement;
            this.InCombat = InCombat;
            this.DodgeChance = DodgeChance;
            this.IsStunned = IsStunned;
        }
        public static void GenerateEnemy(int X,int Y,char EnemyType)
        {
            Point _positon = new Point(X,Y);
            Enemy enemy = new Enemy();
            switch (EnemyType)
            {
                case '!':
                    enemy = Enemy.GenerateEnemyType( _positon, EnemyType);
                    break;
            }
            EnemiesList.Add(enemy);
        }
        public static Enemy GenerateEnemyType(Point _positon, char EnemyType)
        {
            Enemy enemy = new Enemy();
            

            enemy.Name = "skeltony";
            enemy.Positon = _positon;
            enemy.Character = EnemyType;
            enemy.Alert = false;
            enemy.FirstMovement = true;
            enemy.MaxHP = Random.Shared.Next(3,9);
            enemy.HP = enemy.MaxHP;
            enemy.DMG = Random.Shared.Next(1, 3);
            enemy.DodgeChance = Random.Shared.Next(0,10);
            enemy.IsStunned = 0;

            return enemy;
        }
        // first methd to start the enemy
        public static void EnemyAlart()
        {
            foreach (var _enemy in EnemiesList)
            {
                if(IsPlayerNearby(_enemy))
                {
                    Combat.StartFight(DungenGame.PlayerUnit,_enemy); break;
                }
                else if (IsPlayerInAlartRange(_enemy) || _enemy.Alert)
                {
                    _enemy.Alert = true;
                    EnemyMovment(_enemy);
                }
                else
                {
                    //_enemy.Alert = false;
                }
                if (IsPlayerNearby(_enemy))
                {
                    Combat.StartFight(DungenGame.PlayerUnit, _enemy); break;
                }
            }
        }
        public static bool IsPlayerNearby(Enemy _enemy)
        {
            
            List<Point> _neighbors = new List<Point>();
            _neighbors = ShortestPathFinder.GenerateNeighbors(_enemy.Positon);
            foreach(Point item in  _neighbors)
            {
                if (item.X == DungenGame.PlayerUnit.LoctionX && item.Y == DungenGame.PlayerUnit.LoctionY)
                {
                    return true;
                }
                           
            }
            return false;
        }
        //Check if the player is 5 tiles away from the enemy
        public static bool IsPlayerInAlartRange(Enemy _enemy)
        {
            
            for(int x = -5;x < 5;x++)
            {
                for (int y = -3; y < 3;y++)
                {
                    if(DungenGame.PlayerUnit.LoctionX == (_enemy.Positon.X + x) 
                        && DungenGame.PlayerUnit.LoctionY == (_enemy.Positon.Y + y))
                    {

                        return true;
                    }
                }
            }
            return false;  
        }
        public static void EnemyMovment(Enemy _enemy)
        {
            Point PlayerPositon = new Point(0, 0);
            PlayerPositon.X = DungenGame.PlayerUnit.LoctionX;
            PlayerPositon.Y = DungenGame.PlayerUnit.LoctionY;
            
            if (_enemy.Alert == true)
            {
                    List<Point> _path = new List<Point>(ShortestPathFinder.FindShortestPath(_enemy.Positon, PlayerPositon));
                /*
                    if(_path.Count < 1) 
                    {
                    Console.ReadKey();
                    }
                */
                    Point _TileToGo = new Point(_path[0].X, _path[0].Y);
                    MoveEnemyTo(_enemy, _TileToGo);
            }
        }
        public static void MoveEnemyTo(Enemy _enemy, Point _tileGoTo)
        {
            //clean the tile that the player was on
            CleanAfterEnemy(_enemy);

            _enemy.Positon.X = _tileGoTo.X;
            _enemy.Positon.Y = _tileGoTo.Y;

            //moves the player to the positon of his x and y lociton
            MoveEnemyToItsPositon(_enemy);
        }
        public static void CleanAfterEnemy(Enemy _enemy)
        {
            Console.SetCursorPosition(_enemy.Positon.X,_enemy.Positon.Y);
            if(_enemy.FirstMovement == true)
            {
                MapGenerator.MapGrid[_enemy.Positon.Y, _enemy.Positon.X] = ' ';
                _enemy.FirstMovement = false;
            }
            //if the Enemy is on a trap the trap then change the forcolor to magenta
            if (MapGenerator.MapGrid[_enemy.Positon.Y, _enemy.Positon.X] == '#')
                Console.ForegroundColor = ConsoleColor.DarkMagenta;

            Console.Write(MapGenerator.MapGrid[_enemy.Positon.Y, _enemy.Positon.X]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, MapGenerator.MapGrid.GetLength(0) + 5);
        }
        public static void MoveEnemyToItsPositon(Enemy _enemy)
        {
            // Store the current cursor position
            int previousCursorLeft = Console.CursorLeft;
            int previousCursorTop = Console.CursorTop;

            // Set the cursor position to the player's location
            Console.SetCursorPosition(_enemy.Positon.X, _enemy.Positon.Y);

            //if the Enemy is alert he will be red
            if (_enemy.Alert == true) 
                Console.ForegroundColor = ConsoleColor.Red; 

            // Print the player character
            Console.Write('!');

            //reset the color
            Console.ForegroundColor = ConsoleColor.White;

            // Restore the previous cursor position
            Console.SetCursorPosition(previousCursorLeft, previousCursorTop);

        }
    }

    /// <summary>
    /// ///////////////////////////class for Enemy Path finding
    /// </summary>
    public class ShortestPathFinder
    {
        //return a list with the path to the target
        public static List<Point> FindShortestPath(Point start, Point end)
        {
            List<Point> _empty = new List<Point>() {};
            _empty.Add(start);
            Queue<Point> _queue = new Queue<Point>();

            //keep track of what we checked already
            HashSet<Point> _visited = new HashSet<Point>(new PointComparer());

            Dictionary<Point, Point> _parentChild = new Dictionary<Point, Point>();

            Point PlayerPositon = new Point(DungenGame.PlayerUnit.LoctionX, DungenGame.PlayerUnit.LoctionY);

            _queue.Enqueue(start);
            _visited.Add(start);
            while (_queue.Count > 0)
            {
                Point _current = _queue.Dequeue();
                Console.SetCursorPosition(_current.X, _current.Y);

                //test pathfinding
                /*
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(_current.X, _current.Y);
                Console.Write(' ');
                Console.BackgroundColor = ConsoleColor.Black;
                */

                if (_current.X == end.X && _current.Y == end.Y)
                {
                    return ReconstructPath(_parentChild, end);
                }
                List<Point> Neighbors = GenerateNeighbors(_current);

                foreach (Point _neighbors in Neighbors)
                {
                    if (!_visited.Contains(_neighbors) && IsClear(_neighbors))
                    {
                        if ((_neighbors.X < start.X + 30) && (_neighbors.X > start.X - 30) && (_neighbors.Y < start.Y + 30) && (_neighbors.Y > start.Y - 30))
                        {
                            CheckForDuplicate(_visited,_neighbors);
                            _queue.Enqueue(_neighbors);
                            //_parentChild[_neighbors] = _current;
                            AddToDictionary(_parentChild,_neighbors,_current);
                        }

                    }

                }
            }
            // no path found
            return _empty;
        }
        //check if the enemy can go to the tile
        private static bool IsClear(Point _tile)
        {
            try
            {
                foreach(Enemy enemy in Enemy.EnemiesList) 
                {
                    if(enemy.Positon.Y == _tile.Y && enemy.Positon.X == _tile.X)
                    { return false; }   
                }
                if (MapGenerator.MapGrid[_tile.Y, _tile.X] == ' ' || MapGenerator.MapGrid[_tile.Y, _tile.X] == '#' )
                {
                    if(!(CheckIfEnemyIsInTile(_tile.Y, _tile.X)))
                    {
                        return true;
                    }
                    
                }
                return false;
            }
            catch { return false; }

        }

        private static bool CheckIfEnemyIsInTile(int x, int y)
        {
            foreach(Enemy _enemy in Enemy.EnemiesList)
            {
                if(_enemy.Positon.X == x && _enemy.Positon.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        //Generate the tile Neughors
        public static List<Point> GenerateNeighbors(Point _current)
        {
            List<Point> _neighbors = new List<Point>();
            _neighbors.Add(new Point(_current.X + 1, _current.Y));
            _neighbors.Add(new Point(_current.X - 1, _current.Y));
            _neighbors.Add(new Point(_current.X, _current.Y + 1));
            _neighbors.Add(new Point(_current.X, _current.Y - 1));
            return _neighbors;
        }

        public static void AddToDictionary(Dictionary<Point,Point> Parent, Point key, Point value)
        {
            foreach(KeyValuePair<Point,Point> P in Parent)
            {
                if (P.Key.X == key.X && P.Key.Y == key.Y)
                {
                    return;
                }
                    
            }
            Parent[key] = value;


        }

        //return a list with the path to the goal
        private static List<Point> ReconstructPath(Dictionary<Point, Point> parents, Point goal)
        {
            Point _current = goal;
            List<Point> _PathToGoal = new List<Point>();

            while (parents.ContainsKey(_current))
            {
                _PathToGoal.Insert(0, _current);
                _current = parents[_current];
            }
            //_PathToGoal.Insert(0, _current);
            return _PathToGoal;

        }

        public static void CheckForDuplicate(HashSet<Point> list, Point point)
        {
            bool _isin = false;
            foreach(Point i in list)
            {
                if (i.X == point.X && i.Y == point.Y)
                {
                    _isin = true; break;
                }
            }

            if (!_isin)
            {
                list.Add(point);
            }

        }
    }

    public struct Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                return Equals((Point)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }
    public class PointComparer : IEqualityComparer<Point>
    {
        public bool Equals(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public int GetHashCode(Point p)
        {
            return p.X.GetHashCode() ^ p.Y.GetHashCode();
        }
    }

    


}
