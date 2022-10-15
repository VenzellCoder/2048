namespace ConsoleApp1;


public class Cell
{
    public int value;
}


public enum Direction
{
    Up,
    Right,
    Down,
    Left
};


public class Game
{
    private const int NumbersOnGameStart = 2;
    private const int FourNumberChancePercent = 10;
    private const int Width = 6;
    private const int Height = 6;
    
    private Random random = new Random();
    private Cell[,] cells;
    private List<Cell> emptyCells = new List<Cell>();



    #region INIT

    public void Init()
    {
        CreateCells();
        UpdateEmptyCellsList();
        AddNumbersOnGameStart();
    }
    
    
    private void CreateCells()
    {
        cells = new Cell[Height, Width];
        
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                cells[y, x] = new Cell();
            }
        }
    }
    
    
    private void AddNumbersOnGameStart()
    {
        for (int i = 0; i < NumbersOnGameStart; i++)
        {
            AddRandomNumber();
        }
    }

    #endregion
    
    
    
    #region INPUT PROCESSING
    
    public void ProcessInput(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                ProcessLines(true);
                break;
            case Direction.Right:
                ProcessLines(false);
                break;
            case Direction.Up:
                ProcessColumns(true);
                break;
            case Direction.Down:
                ProcessColumns(false);
                break;
        }
    }


    private void ProcessLines(bool isInverseSorting)
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            var ar = GetLine(y);
            CollapseArray(ar, isInverseSorting);
            SetLine(y, ar);
        }
    }
    
    
    private void ProcessColumns(bool isInverseSorting)
    {
        for (int x = 0; x < cells.GetLength(1); x++)
        {
            var ar = GetColumn(x);
            CollapseArray(ar, isInverseSorting);
            SetColumn(x, ar);
        }
    }
    
    
    private Cell[] GetLine(int y)
    {
        Cell[] line = new Cell[cells.GetLength(1)];

        for (int x = 0; x < cells.GetLength(1); x++)
        {
            line[x] = cells[y, x];
        }
        
        return line;
    }
    
    
    private Cell[] GetColumn(int x)
    {
        Cell[] line = new Cell[cells.GetLength(0)];

        for (int y = 0; y < cells.GetLength(0); y++)
        {
            line[y] = cells[y, x];
        }
        
        return line;
    }
    
    
    private void SetLine(int y, Cell[] newCells)
    {
        for (int x = 0; x < cells.GetLength(1); x++)
        {
            cells[y, x] = newCells[x];
        }
    }
    

    private void SetColumn(int x, Cell[] newCells)
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            cells[y, x] = newCells[y];
        }
    }
    
    
    private void CollapseArray(Cell[] input, bool isInverseSorting)
    {
        bool wereItemsMoved = false;
        Cell buffer;
        
        do
        {
            wereItemsMoved = false;
            for (int i = 0; i < input.Count() - 1; i++)
            {
                if (ShouldCollapseCells(input[i], input[i + 1]))
                {
                    input[i].value *= 2;
                    input[i + 1].value = 0;
                    wereItemsMoved = true;
                }
                
                if (ShouldSwipeCells(input[i], input[i + 1]))
                {
                    buffer = input[i + 1];
                    input[i + 1] = input[i];
                    input[i] = buffer;
                    wereItemsMoved = true;
                }
            }
        } 
        while (wereItemsMoved);

        bool ShouldCollapseCells(Cell cellA, Cell cellB)
        {
            return cellA.value != 0 && cellA.value == cellB.value;
        }
        
        bool ShouldSwipeCells(Cell cellA, Cell cellB)
        {
            return (cellA.value != 0 && cellB.value == 0 && !isInverseSorting) ||
                    (cellA.value == 0 && cellB.value != 0 && isInverseSorting);
        }
    }
    
    #endregion


    
    #region ADDING NUMBERS
    
    public void AddRandomNumber()
    {
        int number = (random.Next(0, 100) < FourNumberChancePercent) ? 4 : 2;
        Cell cell = emptyCells[random.Next(0, emptyCells.Count)];

        if (cell == null)
        {
            // GAME OVER
            return;
        }
        
        cell.value = number;
        emptyCells.Remove(cell);
    }
    
    #endregion
    
    
    
    #region UTILITIES
    
    private void UpdateEmptyCellsList()
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                if (ShouldAdd(cells[y, x]))
                {
                    emptyCells.Add(cells[y, x]);
                }
                else if (ShouldRemove(cells[y, x]))
                {
                    emptyCells.Remove(cells[y, x]);
                }
            }
        }

        bool ShouldAdd(Cell cell)
        {
            return cell.value == 0 && !emptyCells.Contains(cell);
        }
        
        bool ShouldRemove(Cell cell)
        {
            return cell.value != 0 && emptyCells.Contains(cell);
        }
    }
    
    #endregion
    
    
    
    #region DRAWING
    
    public void Draw()
    {
        for(int y = 0; y < cells.GetLength(0); y++)
        {
            for(int x = 0; x < cells.GetLength(1); x++)
            {
                Console.SetCursorPosition(x*6, y);
                if (cells[y, x].value == 0)
                {
                    Console.Write('.');
                }
                else
                {
                    Console.Write(cells[y ,x].value);
                }
            }
            
            Console.WriteLine();
        }
    }
    
    #endregion
    
}