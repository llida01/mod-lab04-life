using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Text.Json;


namespace cli_life
{
    public class Params
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int CellSize { get; set; }
        public double LiveDensity { get; set; }
    }
    public class Cell
    {
        public bool IsAlive;
        public readonly List<Cell> neighbors = new List<Cell>();
        private bool IsAliveNext;
        public bool DetermineNextLiveState()
        {
            bool changes = false;
            int liveNeighbors = neighbors.Where(x => x.IsAlive).Count();
            if (IsAlive)
            {
                IsAliveNext = liveNeighbors == 2 || liveNeighbors == 3;
                if (!IsAliveNext)
                {
                    changes = true;
                }
            }
            else
            {
                IsAliveNext = liveNeighbors == 3;
                if (IsAliveNext)
                {
                    changes = true;
                }
            }
            return changes;
        }
        public void Advance()
        {
            IsAlive = IsAliveNext;
        }
    }
    public class Board
    {
        public readonly Cell[,] Cells;
        public readonly int CellSize;

        public int Columns { get { return Cells.GetLength(0); } }
        public int Rows { get { return Cells.GetLength(1); } }
        public int Width { get { return Columns * CellSize; } }
        public int Height { get { return Rows * CellSize; } }

        public Board(int width, int height, int cellSize, double liveDensity)
        {
            CellSize = cellSize;

            Cells = new Cell[width / cellSize, height / cellSize];
            for (int x = 0; x < Columns; x++)
                for (int y = 0; y < Rows; y++)
                    Cells[x, y] = new Cell();

            ConnectNeighbors();
        }
        readonly Random rand = new Random();
        public void Randomize(double liveDensity)
        {
            foreach (var cell in Cells)
                cell.IsAlive = rand.NextDouble() < liveDensity;
        }
        public void ReadFile(string file)
        {
            string[] lines = File.ReadAllLines(file);
            int i = 0;
            foreach (string s in lines)
            {
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j] == '*')
                    {
                        Cells[j, i].IsAlive = true;
                    }
                }
                i++;
            }
        }
        public int Advance()
        {
            int changes = 0;
            bool res = false;
            foreach (var cell in Cells)
            {
                res = cell.DetermineNextLiveState();
                if (res)
                {
                    changes++;
                }
            }
            foreach (var cell in Cells)
                cell.Advance();
            return changes;
        }
        private void ConnectNeighbors()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    int xL = (x > 0) ? x - 1 : Columns - 1;
                    int xR = (x < Columns - 1) ? x + 1 : 0;

                    int yT = (y > 0) ? y - 1 : Rows - 1;
                    int yB = (y < Rows - 1) ? y + 1 : 0;

                    Cells[x, y].neighbors.Add(Cells[xL, yT]);
                    Cells[x, y].neighbors.Add(Cells[x, yT]);
                    Cells[x, y].neighbors.Add(Cells[xR, yT]);
                    Cells[x, y].neighbors.Add(Cells[xL, y]);
                    Cells[x, y].neighbors.Add(Cells[xR, y]);
                    Cells[x, y].neighbors.Add(Cells[xL, yB]);
                    Cells[x, y].neighbors.Add(Cells[x, yB]);
                    Cells[x, y].neighbors.Add(Cells[xR, yB]);
                }
            }
        }
        public int Hives()
        {
            int hives = 0;
            for (int i = 0; i < Rows - 3; i++)
            {
                for (int j = 0; j < Columns - 2; j++)
                {
                    if (!Cells[j, i].IsAlive && !Cells[j + 2, i].IsAlive &&
                        !Cells[j + 1, i + 1].IsAlive && !Cells[j + 1, i + 2].IsAlive &&
                        !Cells[j, i + 3].IsAlive && !Cells[j + 2, i + 3].IsAlive &&
                        Cells[j + 1, i].IsAlive && Cells[j, i + 1].IsAlive &&
                        Cells[j + 2, i + 1].IsAlive && Cells[j, i + 2].IsAlive &&
                        Cells[j + 2, i + 2].IsAlive && Cells[j + 1, i + 3].IsAlive)
                    {
                        if (!Cells[j, i].neighbors[0].IsAlive && !Cells[j, i].neighbors[1].IsAlive
                           && !Cells[j, i].neighbors[2].IsAlive && !Cells[j, i].neighbors[3].IsAlive
                           && !Cells[j, i].neighbors[5].IsAlive && !Cells[j, i + 3].neighbors[0].IsAlive
                           && !Cells[j, i + 3].neighbors[3].IsAlive && !Cells[j, i + 3].neighbors[5].IsAlive
                           && !Cells[j, i + 3].neighbors[6].IsAlive && !Cells[j, i + 3].neighbors[7].IsAlive
                           && !Cells[j + 2, i].neighbors[1].IsAlive && !Cells[j + 2, i].neighbors[2].IsAlive
                           && !Cells[j + 2, i].neighbors[4].IsAlive && !Cells[j + 2, i].neighbors[7].IsAlive
                           && !Cells[j + 2, i + 3].neighbors[2].IsAlive && !Cells[j + 2, i + 3].neighbors[4].IsAlive
                           && !Cells[j + 2, i + 3].neighbors[6].IsAlive && !Cells[j + 2, i + 3].neighbors[7].IsAlive)
                        {
                            hives++;
                        }
                    }
                }
            }
            return hives;
        }
        public int Ponds()
        {
            int ponds = 0;
            for (int i = 0; i < Rows - 3; i++)
            {
                for (int j = 0; j < Columns - 3; j++)
                {
                    if (!Cells[j, i].IsAlive && Cells[j + 1, i].IsAlive &&
                        Cells[j + 2, i].IsAlive && !Cells[j + 3, i].IsAlive &&
                        Cells[j, i + 1].IsAlive && !Cells[j + 1, i + 1].IsAlive
                        && !Cells[j + 2, i + 1].IsAlive && Cells[j + 3, i + 1].IsAlive
                        && Cells[j, i + 2].IsAlive && !Cells[j + 1, i + 2].IsAlive &&
                        !Cells[j + 2, i + 2].IsAlive && Cells[j + 3, i + 2].IsAlive &&
                        !Cells[j, i + 3].IsAlive && Cells[j + 1, i + 3].IsAlive &&
                        Cells[j + 2, i + 3].IsAlive && !Cells[j + 3, i + 3].IsAlive)
                    {
                        if (!Cells[j, i].neighbors[0].IsAlive && !Cells[j, i].neighbors[1].IsAlive
                            && !Cells[j, i].neighbors[2].IsAlive && !Cells[j, i].neighbors[3].IsAlive
                            && !Cells[j, i].neighbors[5].IsAlive && !Cells[j + 3, i].neighbors[0].IsAlive
                            && !Cells[j + 3, i].neighbors[1].IsAlive && !Cells[j + 3, i].neighbors[2].IsAlive
                            && !Cells[j + 3, i].neighbors[4].IsAlive && !Cells[j + 3, i].neighbors[7].IsAlive
                            && !Cells[j, i + 3].neighbors[0].IsAlive && !Cells[j, i + 3].neighbors[3].IsAlive
                            && !Cells[j, i + 3].neighbors[5].IsAlive && !Cells[j, i + 3].neighbors[6].IsAlive
                            && !Cells[j, i + 3].neighbors[7].IsAlive && !Cells[j + 3, i + 3].neighbors[2].IsAlive
                            && !Cells[j + 3, i + 3].neighbors[4].IsAlive && !Cells[j + 3, i + 3].neighbors[5].IsAlive
                            && !Cells[j + 3, i + 3].neighbors[6].IsAlive && !Cells[j + 3, i + 3].neighbors[7].IsAlive)
                        {
                            ponds++;
                        }

                    }
                }
            }
            return ponds;
        }
        public int Boxes()
        {
            int boxes = 0;
            for (int i = 0; i < Rows - 2; i++)
            {
                for (int j = 0; j < Columns - 2; j++)
                {
                    if (!Cells[j, i].IsAlive && Cells[j + 1, i].IsAlive &&
                        !Cells[j + 2, i].IsAlive && Cells[j, i + 1].IsAlive &&
                        !Cells[j + 1, i + 1].IsAlive && Cells[j + 2, i + 1].IsAlive &&
                        !Cells[j, i + 2].IsAlive && Cells[j + 1, i + 2].IsAlive &&
                        !Cells[j + 2, i + 2].IsAlive)
                    {
                        if (!Cells[j, i].neighbors[0].IsAlive && !Cells[j, i].neighbors[1].IsAlive
                           && !Cells[j, i].neighbors[2].IsAlive && !Cells[j, i].neighbors[3].IsAlive
                           && !Cells[j, i].neighbors[5].IsAlive && !Cells[j, i + 2].neighbors[3].IsAlive
                           && !Cells[j, i + 2].neighbors[5].IsAlive && !Cells[j
                           , i + 2].neighbors[6].IsAlive
                           && !Cells[j + 2, i].neighbors[1].IsAlive && !Cells[j + 2, i].neighbors[2].IsAlive
                           && !Cells[j + 2, i].neighbors[4].IsAlive && !Cells[j + 2, i + 2].neighbors[2].IsAlive
                           && !Cells[j + 2, i + 2].neighbors[4].IsAlive && !Cells[j + 2, i + 2].neighbors[5].IsAlive
                           && !Cells[j + 2, i + 2].neighbors[6].IsAlive && !Cells[j + 2, i + 2].neighbors[7].IsAlive)
                        {
                            boxes++;
                        }

                    }
                }
            }
            return boxes;
        }
        public int Blocks()
        {
            int blocks = 0;
            for (int i = 0; i < Rows - 1; i++)
            {
                for (int j = 0; j < Columns - 1; j++)
                {
                    if (Cells[j, i].IsAlive && Cells[j + 1, i].IsAlive &&
                        Cells[j, i + 1].IsAlive && Cells[j + 1, i + 1].IsAlive)
                    {
                        if (!Cells[j, i].neighbors[0].IsAlive && !Cells[j, i].neighbors[1].IsAlive
                           && !Cells[j, i].neighbors[3].IsAlive && !Cells[j, i + 1].neighbors[3].IsAlive
                           && !Cells[j, i + 1].neighbors[5].IsAlive && !Cells[j, i + 1].neighbors[6].IsAlive
                           && !Cells[j + 1, i].neighbors[1].IsAlive && !Cells[j + 1, i].neighbors[2].IsAlive
                           && !Cells[j + 1, i].neighbors[4].IsAlive && !Cells[j + 1, i + 1].neighbors[4].IsAlive
                           && !Cells[j + 1, i + 1].neighbors[6].IsAlive && !Cells[j + 1, i + 1].neighbors[7].IsAlive)
                        {
                            blocks++;
                        }
                    }
                }
            }
            return blocks;
        }
        public bool Symmetry()
        {
            bool result = true;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < (Columns / 2); j++)
                {
                    if (Cells[j, i].IsAlive != Cells[(Columns - j - 1), i].IsAlive)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
    }
    public class LifeGame
    {
        static Board board;
        static private int Reset(string file_name)
        {
            string text = File.ReadAllText("params.json");
            Params param = JsonSerializer.Deserialize<Params>(text);
            board = new Board(width: param.Width, height: param.Height,
                cellSize: param.CellSize, liveDensity: param.LiveDensity);
            board.ReadFile(file_name);
            return (param.Width * param.Height / param.CellSize);
        }
        static int Render()
        {
            int alive = 0;
            for (int row = 0; row < board.Rows; row++)
            {
                for (int col = 0; col < board.Columns; col++)
                {
                    var cell = board.Cells[col, row];
                    if (cell.IsAlive)
                    {
                        Console.Write('*');
                        alive++;
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write('\n');
            }
            return alive;
        }
        public (int genCount, int total, int alive, int hives, int ponds, int boxes, int blocks, bool symmetry) Run(string file_name)
        {
            int total = Reset(file_name);
            int alive = 0;
            int genCount = 0;
            int hives = 0;
            int ponds = 0;
            int boxes = 0;
            int blocks = 0;
            bool symmetry = false;
            int changes;
            int[] end = { -1, -1, -1, -1, -1 };
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo name = Console.ReadKey();
                    if (name.KeyChar == 'q')
                    {
                        break;
                    }
                    else if (name.KeyChar == 's')
                    {
                        string fname = "gen-" + genCount.ToString();
                        StreamWriter writer = new StreamWriter(fname + ".txt");
                        for (int row = 0; row < board.Rows; row++)
                        {
                            for (int col = 0; col < board.Columns; col++)
                            {
                                var cell = board.Cells[col, row];
                                if (cell.IsAlive)
                                {
                                    writer.Write('*');
                                }
                                else
                                {
                                    writer.Write(' ');
                                }
                            }
                            writer.Write("\n");
                        }
                        writer.Close();
                    }
                }
                try
                {
                    Console.Clear();
                }
                catch
                {

                }
                alive = Render();
                hives = board.Hives();
                ponds = board.Ponds();
                boxes = board.Boxes();
                blocks = board.Blocks();
                symmetry = board.Symmetry();
                changes = board.Advance();
                genCount++;
                end[genCount % 5] = changes;
                if (genCount > 4)
                {
                    if ((end[0] == end[2]) && (end[0] == end[4]))
                    {
                        break;
                    }
                }
                Thread.Sleep(10);
            }
            (int, int, int, int, int, int, int, bool) result = (genCount - 2, total, alive, hives, ponds, boxes, blocks, symmetry);
            return result;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start4.txt");
            int stabilization = result.genCount;
            int total = result.total;
            int alive = result.alive;
            int dead = total - alive;
            double alive_density = (double)alive / total;
            double dead_density = (double)dead / total;
            int hives = result.hives;
            int ponds = result.ponds;
            int boxes = result.boxes;
            int blocks = result.blocks;
            bool symmetry = result.symmetry;
            int symmetry_figures = hives + ponds + boxes + blocks;
            Console.WriteLine("Фаза остановки: " + stabilization);
            Console.WriteLine("Всего клеток: " + total);
            Console.WriteLine("Живых клеток: " + alive);
            Console.WriteLine("Мертвых клеток: " + dead);
            Console.WriteLine("Плотность Живых клеток: " + alive_density);
            Console.WriteLine("Плотность Мертвых клеток: " + dead_density);
            Console.WriteLine("Всего фигуры улей: " + hives);
            Console.WriteLine("Всего фигуры пруд: " + ponds);
            Console.WriteLine("Всего фигуры ящик: " + boxes);
            Console.WriteLine("Всего фигуры блок: " + blocks);
            if (symmetry)
            {
                Console.WriteLine("Доска симметрична");
            }
            else
            {
                Console.WriteLine("Доска несимметрична");
            }
            Console.WriteLine("Симметричных фигур: " + symmetry_figures);
        }
    }
}