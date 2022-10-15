using ConsoleApp1;


class Program
{
    public static void Main(string[] args)
    {
        Dictionary<ConsoleKey, Direction> keyMap = new Dictionary<ConsoleKey, Direction>()
        {
            {ConsoleKey.UpArrow, Direction.Up},
            {ConsoleKey.RightArrow, Direction.Right},
            {ConsoleKey.DownArrow, Direction.Down},
            {ConsoleKey.LeftArrow, Direction.Left},
        };

        // Инициализация 
        Game game = new Game();
        game.Init();

        // Игровой цикл
        while (true)
        {
            Console.Clear();
            game.Draw();
            
            var key = Console.ReadKey();
            if (keyMap.ContainsKey(key.Key))
            {
                game.ProcessInput(keyMap[key.Key]);
            }
            
            game.AddRandomNumber();
        }
    }
}