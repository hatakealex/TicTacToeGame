namespace TicTacToeGame
{
    public class Game
    {
        private int cellsCount;
        private readonly CellStatus[] board;
        private GameMode gameMode;

        private Player whoMoveFirst;
        public Player CurrentPlayer { get; private set; }
        private CellStatus currentCellSatus = CellStatus.X; 

        public Game(GameMode gameMode, int cellsCount = 9, Player whoMoveFirst = Player.Human) 
        {
            this.gameMode = gameMode;
            this.whoMoveFirst = whoMoveFirst;
            CurrentPlayer = this.whoMoveFirst;
            this.cellsCount = cellsCount;

            board = new CellStatus[cellsCount];

            for (int i = 0; i < board.Length; i++)
            {
                board[i] = CellStatus.V;
            }
        }

        /// <summary>
        /// Следующий ход
        /// </summary>
        /// <param name="stepToField">номер ячейки для хода</param>
        /// <exception cref="ArgumentException">При некорректном параметре</exception>
        /// <exception cref="FieldAccessException">Если ячейка занята</exception>
        public GameStatus NextMove(int stepToField) 
        {
            if (stepToField < 1 || stepToField > cellsCount)
                throw new ArgumentException($"stepToField должно быть от 1 до {cellsCount}");

            if (board[stepToField-1] != CellStatus.V)
                throw new FieldAccessException("Ячейка уже занята");

            board[stepToField - 1] = currentCellSatus;

            ChangeNextPlayer();

            return CheckStepResult();
        }

        private void ChangeNextPlayer() 
        {
            CurrentPlayer = gameMode == GameMode.PlayerVsComputer ? CurrentPlayer == Player.Computer ? Player.Human : Player.Computer : Player.Human;
            currentCellSatus = currentCellSatus == CellStatus.O ? CellStatus.X : CellStatus.O;
        }

        public GameStatus CheckStepResult()
        {
            int[] winCombinations = { 0, 1, 2, 0, 4, 8, 0, 3, 6, 3, 4, 5, 1, 4, 7, 6, 4, 2, 6, 7, 8, 2, 5, 8 };

            for (int i = 0; i < winCombinations.Length; i += 3)
            {
                if (CheckLine(winCombinations[i], winCombinations[i+1], winCombinations[i+2]))
                    return board[winCombinations[i]] == CellStatus.X ? GameStatus.WinX : GameStatus.WinO;
            }

            if (AllFieldsIsWriten()) 
                return GameStatus.Draw;

            return GameStatus.IsRun;
        }

        private bool CheckLine(int a, int b, int c) 
        {
            return board[a] != CellStatus.V && board[a] == board[b] && board[a] == board[c];
        }

        private bool AllFieldsIsWriten()
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == CellStatus.V)
                    return false;
            }

            return true;
        }

        public CellStatus GetCellStatus(int cellNumber)
        {
            return board[cellNumber];
        }

        public int ComputerMove() 
        {
            int cell;
            while (true)
            {
                cell = new Random(DateTime.Now.Millisecond).Next(0, 9);

                if (GetCellStatus(cell) == CellStatus.V)
                {
                    NextMove(cell+1);
                    break;
                }
            }

            return cell;
        }
    }
}