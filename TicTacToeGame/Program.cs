using TicTacToeGame;

Game game;
int cellsCount = 9;

Console.WriteLine("Игра крестики-нолики");
while (true)
{
    StartGame();

    Console.WriteLine("Хотите сыграть еще? (д / н)");
    string repeatGame = Console.ReadLine();
    if (string.Compare(repeatGame, "д", true) == 0)
        continue;
    else
        break;
}

Console.ReadLine();

void StartGame() 
{
    while (true)
    {
        Console.WriteLine("Выбери режим игры:");
        Console.WriteLine("1 - игрок против компьтера");
        Console.WriteLine("2 - 2 игрока");

        if (int.TryParse(Console.ReadLine(), out int gameMode) && gameMode > 0 && gameMode < 3)
        {
            int whoMoveFirst = 0;

            if (gameMode == 1)
            {
                while (true)
                {
                    Console.WriteLine("Кто ходит первым?");
                    Console.WriteLine("1 - первым ходит игрок");
                    Console.WriteLine("2 - первым ходит компьютер");

                    if (int.TryParse(Console.ReadLine(), out whoMoveFirst) && whoMoveFirst > 0 && whoMoveFirst < 3)
                    {
                        break;
                    }
                }
            }

            game = new Game(gameMode == 1 ? GameMode.PlayerVsComputer : GameMode.PlayerVsPlayer,
                            cellsCount,
                            whoMoveFirst == 2 ? Player.Computer : Player.Human);

            break;
        }
    }

    GameStatus gameStatus;

    PrintGameFields();

    while ((gameStatus = game.CheckStepResult()) == GameStatus.IsRun)
    {
        try
        {
            int field;

            if (game.CurrentPlayer == Player.Computer)
            {
                field = game.ComputerMove();
                Console.WriteLine($"Ход компьютера: {field}");
            }
            else
            {
                Console.WriteLine($"Ваш ход. Введите номер ячейки от 1 до {cellsCount}");
                string buffer = Console.ReadLine();

                if (string.IsNullOrEmpty(buffer) ||
                    !int.TryParse(buffer, out field) ||
                    field < 1 ||
                    field > cellsCount)
                {
                    continue;
                }

                game.NextMove(field);
            }

        }
        catch (FieldAccessException ex)
        {
            Console.WriteLine(ex.Message);
            continue;
        }

        PrintGameFields();
    }

    switch (gameStatus)
    {
        case GameStatus.WinX:
            Console.WriteLine("Крестики победили");
            break;

        case GameStatus.WinO:
            Console.WriteLine("Нолики победили");
            break;

        case GameStatus.Draw:
            Console.WriteLine("Ничья!");
            break;
    }
}

void PrintGameFields()
{
    PrintRow(0);
    PrintRow(1);
    PrintRow(2);

    void PrintRow(int row)
    {
        Console.Write('|');
        int start = row * 3;
        for (int i = start; i < start + 3; i++)
        {
            CellStatus cellStatus = game.GetCellStatus(i);
            Console.Write(cellStatus == CellStatus.V ? i+1 : cellStatus);
            Console.Write('|');
        }
        Console.WriteLine();
    }
}

