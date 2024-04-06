// Доска

using Board = System.Collections.Generic.List<System.Collections.Generic.List<Piece?>>;
using System.Text;

// Цвет (будет означать как цвет шашки, так и цвет игрока)
internal enum Color
{
    White,
    Black
}

// Тип шашки (дамка или пешка)
internal enum TypeOfPiece
{
    Queen,
    Pawn
}

// Шашка
record Piece
{
    public TypeOfPiece TypeOfPiece;
    public Color Color;
}

// Ход
record Step
{
    public int Row1; //ряд той шашки которую мы хотим подвинуть
    public int Column1; //столбец той шашки которую мы хотим подвинуть
    public int Row2; // ряд клетки, куда мы хотим передвинуть
    public int Column2; // столбец клетки, куда мы хотим передвинуть
}

namespace Checkers
{
    // Реализация русских шашек, по правилам отсюда: https://www.gambler.ru/Draughts_russian
    public abstract class Checkers
    {
        private const int BoardSize = 8;
        private const char WhitePawn = '\u26c0'; // символ белой шашки
        private const char BlackPawn = '\u26c2'; // символ черной шашки
        private const char WhiteQueen = '\u26c1'; // символ белой дамки
        private const char BlackQueen = '\u26c3'; // символ черной дамки
        //Для проверки корректности ввода ходов 
        private static readonly List<char> Letters = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        private static readonly List<char> Digits = new() { '1', '2', '3', '4', '5', '6', '7', '8' };
        
        // Возвращает доску с первоначальной расстановкой пешек 
        private static Board FullBoard()
        {
            List<Piece?> row8 = new List<Piece?>
            {
                null, new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }
            };
            List<Piece?> row7 = new List<Piece?>
            {
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null
            };
            List<Piece?> row6 = new List<Piece?>
            {
                null, new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.Black, TypeOfPiece = TypeOfPiece.Pawn }
            };
            List<Piece?> row5 = new List<Piece?> { null, null, null, null, null, null, null, null };
            List<Piece?> row4 = new List<Piece?> { null, null, null, null, null, null, null, null };
            List<Piece?> row3 = new List<Piece?>
            {
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null
            };
            List<Piece?> row2 = new List<Piece?>
            {
                null, new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }
            };
            List<Piece?> row1 = new List<Piece?>
            {
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null,
                new Piece { Color = Color.White, TypeOfPiece = TypeOfPiece.Pawn }, null
            };
            return new Board { row1, row2, row3, row4, row5, row6, row7, row8 };
        }

        // Печатает доску с заданным расположением шашек
        private static void PrintBoard(Board board, Color moving)
        {
         
            if (moving == Color.White) // если ходят белые печатаем доску как обычно
            {
                for (int row = BoardSize-1; row >= 0; row--)
                {
                    for (int column = 0; column <= BoardSize; column++)
                    {
                        if (column < BoardSize)
                        {
                            // если сумма индексов столбца и ряда четна то клетка черная (темно-красная, чтоб
                            // сливалась с черным пешками, иначе белая
                            Console.BackgroundColor =
                                (column + row) % 2 == 0 ? ConsoleColor.DarkRed : ConsoleColor.White;

                            if (board[row][column] == null) Console.Write("  "); //печатаем пробел если шашки нет
                            else if (board[row][column]!.Color == Color.White)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                if (board[row][column]!.TypeOfPiece == TypeOfPiece.Pawn)
                                {
                                    Console.Write(WhitePawn + " "); 
                                }
                                else
                                {   
                                    Console.Write(WhiteQueen + " "); 
                                }
                            }

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                if (board[row][column]!.TypeOfPiece == TypeOfPiece.Pawn)
                                    Console.Write(BlackPawn + " ");
                                else Console.Write(BlackQueen + " "); 
                            }

                            Console.ResetColor();
                        }
                        else Console.Write(" " + (row + 1)); // печатаем числовую координату клетки
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("a b c d e f g h"); // ряд буквенных координат доски
            }

            if (moving == Color.Black) // если цвет черный то будем разворачивать доску для удобства черных
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    for (int column = BoardSize-1; column >= -1; column--)
                    {
                        if (column >= 0)
                        {
                            Console.BackgroundColor = (column + row) % 2 == 0 ? ConsoleColor.DarkRed : ConsoleColor.White;
                            if (board[row][column] == null) Console.Write("  "); 
                            else if (board[row][column]!.Color == Color.White)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                if (board[row][column]!.TypeOfPiece == TypeOfPiece.Pawn)
                                {
                                    Console.Write(WhitePawn + " ");
                                }
                                else
                                {
                                    Console.Write(WhiteQueen + " ");
                                }
                            }

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                if (board[row][column]!.TypeOfPiece == TypeOfPiece.Pawn)
                                    Console.Write(BlackPawn + " ");
                                else Console.Write(BlackQueen + " ");
                            }

                            Console.ResetColor();
                        }
                        else Console.Write(" " + (row + 1));
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("h g f e d c b a"); //ряд буквенных координат доски
            }
        }


        private static Tuple<List<Step>, bool>? // будем возращать все возможные ходы + был ли сделан ударный ход
            AvailableSteps(Color moving, Board board)
        {
            var allAvailableSteps = new List<Step>(); // абсолютно все ходы обоих игроков (если пуст то ничья)
            var availableEatSteps = new List<Step>(); // доступные ударные ходы (ходящего)
            var availableSilentSteps = new List<Step>(); // доступные тихие ходы (ходящего)
            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++) // перебираем клетки
                {
                    if (board[row][column] != null) //если шашка есть
                    {
                        Color color = board[row][column]!.Color; // цвет текущей шашки
                        TypeOfPiece typeOfPiece = board[row][column]!.TypeOfPiece; // тип текущей шашки
                        if (typeOfPiece == TypeOfPiece.Pawn)
                            // поиск ударных ходов пешек    
                        {
                            if (row + 2 < BoardSize && column + 2 < BoardSize &&
                                board[row + 1][column + 1] !=
                                null) // проверяем не закончилась ли доска в том месте куда мы хотим срубить
                                // и что в следующей клетке не пусто
                            {
                                if (board[row + 1][column + 1]!.Color != color &&
                                    board[row + 2][column + 2] == null) // проверяем можем ли мы рубить
                                {
                                    allAvailableSteps.Add(new Step
                                    {
                                        Row1 = row, Column1 = column, Row2 = row + 2, Column2 = column + 2
                                    });
                                    if (color == moving) // проверяем совпадает ли цвет фигуры с цветом ходящего
                                        availableEatSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row + 2, Column2 = column + 2 });
                                }
                            }

                            if (row + 2 < BoardSize && column - 2 >= 0 && board[row + 1][column - 1] != null)
                            {
                                if (board[row + 1][column - 1]!.Color != color && board[row + 2][column - 2] == null)
                                {
                                    allAvailableSteps.Add(new Step
                                        { Row1 = row, Column1 = column, Row2 = row + 2, Column2 = column - 2 });
                                    if (color == moving)
                                        availableEatSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row + 2, Column2 = column - 2 });
                                }
                            }

                            if (row - 2 >= 0 && column + 2 < BoardSize && board[row - 1][column + 1] != null)
                            {
                                if (board[row - 1][column + 1]!.Color != color && board[row - 2][column + 2] == null)
                                {
                                    allAvailableSteps.Add(new Step
                                        { Row1 = row, Column1 = column, Row2 = row - 2, Column2 = column + 2 });
                                    if (color == moving)
                                        availableEatSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row - 2, Column2 = column + 2 });
                                }
                            }

                            if (row - 2 >= 0 && column - 2 >= 0 && board[row - 1][column - 1] != null)
                            {
                                if (board[row - 1][column - 1]!.Color != color && board[row - 2][column - 2] == null)
                                {
                                    allAvailableSteps.Add(new Step
                                        { Row1 = row, Column1 = column, Row2 = row - 2, Column2 = column - 2 });
                                    if (color == moving)
                                        availableEatSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row - 2, Column2 = column - 2 });
                                }
                            }

                            // поиск тихих ходов пешек
                            if (color == Color.White) // тут проверяем ходы вперед (белые ходят только вперед)
                            {
                                if (row + 1 < BoardSize && column + 1 < BoardSize) // проверяем не закончилась ли доска
                                {
                                    if (board[row + 1][column + 1] == null) // проверяем что там пусто
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row + 1, Column2 = column + 1 });
                                        if (color == moving) // проверяем совпадает ли цвет фигуры с цветом ходящего
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row + 1, Column2 = column + 1 });
                                    }
                                }

                                if (row + 1 < BoardSize && column - 1 >= 0)
                                {
                                    if (board[row + 1][column - 1] == null)
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row + 1, Column2 = column - 1 });
                                        if (color == moving)
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row + 1, Column2 = column - 1 });
                                    }
                                }
                            }

                            if (color == Color.Black) // тут проверяем ходы назад (черные ходят назад (относительно
                                // белых))
                            {
                                if (row - 1 >= 0 && column + 1 < BoardSize) // проверяем что доска не закончилась
                                {
                                    if (board[row - 1][column + 1] == null) // проверяем что в клетке пусто
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row - 1, Column2 = column + 1 });
                                        if (color == moving) // проверяем совпадает ли цвет фигуры с цветом ходящего
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row - 1, Column2 = column + 1 });
                                    }
                                }

                                if (row - 1 >= 0 && column - 1 >= 0)
                                {
                                    if (board[row - 1][column - 1] == null)
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row - 1, Column2 = column - 1 });
                                        if (color == moving)
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row - 1, Column2 = column - 1 });
                                    }
                                }
                            }
                        }

                        if (typeOfPiece == TypeOfPiece.Queen)
                        {
                            // поиск ударных ходов дамок
                            for (int i = 2; i < BoardSize; i++)
                            {
                                if (row + i < BoardSize &&
                                    column + i < BoardSize) // проверяем не закончилась ли доска в месте куда мы хотим срубить
                                {
                                    if (board[row + i][column + i] == null) // проверяем что в этом месте пусто
                                    {
                                        // если в предыдущем так же пусто то нужно продолжить идти по этой диагонали
                                        // в поисках ударного хода
                                        if (board[row + i - 1][column + i - 1] == null) continue;
                                        // если там фигура противоположного цвета то ищем все ходы за эту фигуру
                                        if (board[row + i - 1][column + i - 1]!.Color != color)
                                        {
                                            for (int j = i; j < BoardSize; j++)
                                            {
                                                if (row + j < BoardSize && column + j < BoardSize)
                                                {
                                                    if (board[row + j][column + j] == null)
                                                    {
                                                        allAvailableSteps.Add(new Step
                                                        {
                                                            Row1 = row, Column1 = column, Row2 = row + j,
                                                            Column2 = column + j
                                                        });
                                                        if (color == moving) // проверяем совпадают ли цвета
                                                            availableEatSteps.Add(new Step
                                                            {
                                                                Row1 = row, Column1 = column, Row2 = row + j,
                                                                Column2 = column + j
                                                            });
                                                    }

                                                    else break;
                                                }
                                            }
                                        }

                                        break;
                                    }

                                    // если в клетке союзная фигура то прекращаем поиск
                                    if (board[row + i][column + i]!.Color == color) break; // если там союзная фигура
                                    // если и в предыдыдущей пусто, а в текущей клетке вражеская фигура, то нужно
                                    // продолжить поиск (следующая итерация найдет ударные ходы)
                                    if (board[row + i - 1][column + i - 1] == null) continue;
                                }

                                break;
                            }

                            for (int i = 2; i < BoardSize; i++)
                            {
                                if (row + i < BoardSize &&
                                    column - i >= 0)
                                {
                                    if (board[row + i][column - i] == null)
                                    {
                                        if (board[row + i - 1][column - i + 1] == null) continue;
                                        if (board[row + i - 1][column - i + 1]!.Color != color)
                                        {
                                            for (int j = i; j < BoardSize; j++)
                                            {
                                                if (row + j < BoardSize && column - j >= 0)
                                                {
                                                    if (board[row + j][column - j] == null)
                                                    {
                                                        allAvailableSteps.Add(new Step
                                                        {
                                                            Row1 = row, Column1 = column, Row2 = row + j,
                                                            Column2 = column - j
                                                        });
                                                        if (color == moving)
                                                            availableEatSteps.Add(new Step
                                                            {
                                                                Row1 = row, Column1 = column, Row2 = row + j,
                                                                Column2 = column - j
                                                            });
                                                    }

                                                    else break;
                                                }
                                            }
                                        }

                                        break;
                                    }

                                    if (board[row + i][column - i]!.Color == color) break;
                                    if (board[row + i - 1][column - i + 1] == null) continue;
                                }

                                break;
                            }

                            for (int i = 2; i < BoardSize; i++)
                            {
                                if (row - i >= 0 &&
                                    column + i < BoardSize)
                                {
                                    if (board[row - i][column + i] == null)
                                    {
                                        if (board[row - i + 1][column + i - 1] == null) continue;
                                        if (board[row - i + 1][column + i - 1]!.Color != color)
                                        {
                                            for (int j = i; j < BoardSize; j++)
                                            {
                                                if (row - j >= 0 && column + j < BoardSize)
                                                {
                                                    if (board[row - j][column + j] == null)
                                                    {
                                                        allAvailableSteps.Add(new Step
                                                        {
                                                            Row1 = row, Column1 = column, Row2 = row - j,
                                                            Column2 = column + j
                                                        });
                                                        if (color == moving)
                                                            availableEatSteps.Add(new Step
                                                            {
                                                                Row1 = row, Column1 = column, Row2 = row - j,
                                                                Column2 = column + j
                                                            });
                                                    }

                                                    else break;
                                                }
                                            }
                                        }

                                        break;
                                    }

                                    if (board[row - i][column + i]!.Color == color) break;
                                    if (board[row - i + 1][column + i - 1] == null) continue;
                                }

                                break;
                            }

                            for (int i = 2; i < BoardSize; i++)
                            {
                                if (row - i >= 0 &&
                                    column - i >= 0)
                                {
                                    if (board[row - i][column - i] == null)
                                    {
                                        if (board[row - i + 1][column - i + 1] == null) continue;
                                        if (board[row - i + 1][column - i + 1]!.Color != color)
                                        {
                                            for (int j = i; j < BoardSize; j++)
                                            {
                                                if (row - j >= 0 && column - j >= 0)
                                                {
                                                    if (board[row - j][column - j] == null)
                                                    {
                                                        allAvailableSteps.Add(new Step
                                                        {
                                                            Row1 = row, Column1 = column, Row2 = row - j,
                                                            Column2 = column - j
                                                        });
                                                        if (color == moving)
                                                            availableEatSteps.Add(new Step
                                                            {
                                                                Row1 = row, Column1 = column, Row2 = row - j,
                                                                Column2 = column - j
                                                            });
                                                    }

                                                    else break;
                                                }
                                            }
                                        }

                                        break;
                                    }

                                    if (board[row - i][column - i]!.Color == color) break;
                                    if (board[row - i + 1][column - i + 1] == null) continue;
                                }

                                break;
                            }

                            // поиск тихих ходов дамок
                            for (int i = 1; i < BoardSize; i++)
                            {
                                if (row + i < BoardSize && // смотрим не закончилась ли доска
                                    column + i < BoardSize)
                                {
                                    if (board[row + i][column + i] == null) // если пусто добавляем ход
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row + i, Column2 = column + i });
                                        if (color == moving) // сравниваем цвет
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row + i, Column2 = column + i });
                                    }
                                    else break; // если не пусто то дальше ходов уже не будет - выходим из цикла
                                }
                            }

                            for (int i = 1; i <= BoardSize; i++)
                            {
                                if (row + i < BoardSize &&
                                    column - i >= 0)
                                {
                                    if (board[row + i][column - i] == null)
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row + i, Column2 = column - i });
                                        if (color == moving)
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row + i, Column2 = column - i });
                                    }
                                    else break;
                                }
                            }

                            for (int i = 1; i <= BoardSize; i++)
                            {
                                if (row - i >= 0 &&
                                    column + i < BoardSize)
                                {
                                    if (board[row - i][column + i] == null)
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row - i, Column2 = column + i });
                                        if (color == moving)
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row - i, Column2 = column + i });
                                    }
                                    else break;
                                }
                            }

                            for (int i = 1; i <= BoardSize; i++)
                            {
                                if (row - i >= 0 &&
                                    column - i >= 0)
                                {
                                    if (board[row - i][column - i] == null)
                                    {
                                        allAvailableSteps.Add(new Step
                                            { Row1 = row, Column1 = column, Row2 = row - i, Column2 = column - i });
                                        if (color == moving)
                                            availableSilentSteps.Add(new Step
                                                { Row1 = row, Column1 = column, Row2 = row - i, Column2 = column - i });
                                    }
                                    else break;
                                }
                            }
                        }
                    }
                }
            }

            if (allAvailableSteps.Count == 0) return null; // если ходов совсем нет возвращаем ничью
            // если есть ударные ходы возвращаем их
            if (availableEatSteps.Count > 0) return new Tuple<List<Step>, bool>(availableEatSteps, true);
            // если нет, то вовзращаем тихие ходы
            return new Tuple<List<Step>, bool>(availableSilentSteps, false);
        }

        private static Color? Game(Board board, Color moving)
        {
            PrintBoard(board, moving); // печатаем текущее положение доски
            var availableSteps = AvailableSteps(moving, board); // считаем возможные ходы
            if (availableSteps == null) return null; // если нет никаких ходов то ничья
            if (availableSteps.Item1.Count == 0) // если нет ходов у ходящего, то побеждает другой игрок
            {
                if (moving == Color.Black) return Color.White;
                return Color.Black;
            }

            while (true)
            {
                Console.Write(moving == Color.White ? "Ход белых: " : "Ход чёрных: ");
                var input = Console.ReadLine()!; // ввод хода из консоли
                if (input == "Сдаюсь") // сдача
                {
                    if (moving == Color.Black) return Color.White;
                    return Color.Black;
                }

                if (input == "Ничья") // ничья
                {
                    Console.WriteLine("Второй игрок согласен на ничью? (Да/Нет)");
                    var agree = Console.ReadLine()!;
                    if (agree == "Да") return null;
                    Console.WriteLine("Соперник отклонил предложение о ничье. Продолжайте игру.");
                }

                var cells = input.Split();
                if (cells.Length == 2) // проверки на корректность ввода
                {
                    if (cells[0].Length == 2 && cells[1].Length == 2)
                    {
                        if (Letters.Contains(cells[0][0]) && Digits.Contains(cells[0][1]) &&
                            Letters.Contains(cells[1][0]) &&
                            Digits.Contains(cells[1][1]))
                        {
                            // координаты хода (97 - код символа 'a', 48 - код символа '0')
                            var column1 = cells[0][0] - 97;
                            var row1 = cells[0][1] - 48 - 1;
                            var column2 = cells[1][0] - 97;
                            var row2 = cells[1][1] - 48 - 1;
                            var step = new Step { Row1 = row1, Column1 = column1, Row2 = row2, Column2 = column2 };
                            if (availableSteps.Item1.Contains(step)) // проверяем возможен ли такой ход
                            {
                                var color = board[row1][column1]!.Color;
                                board[row2][column2] = board[row1][column1]; // ставим фигуру в новую клетку
                                board[row1][column1] = null; // убираем с прошлой
                                if (availableSteps.Item2) // проверяем был ли сделан ударный ход
                                {
                                    if (row2 > row1 && column2 > column1) // смотрим в какую сторону срубили
                                    {
                                        for (int i = 1; i < row2 - row1; i++)
                                        {
                                            board[row1 + i][column1 + i] = null; // удаляем все между
                                        }
                                    }

                                    if (row2 > row1 && column2 < column1)
                                    {
                                        for (int i = 1; i < row2 - row1; i++)
                                        {
                                            board[row1 + i][column1 - i] = null;
                                        }
                                    }

                                    if (row2 < row1 && column2 > column1)
                                    {
                                        for (int i = 1; i < row1 - row2; i++)
                                        {
                                            board[row1 - i][column1 + i] = null;
                                        }
                                    }

                                    if (row2 < row1 && column2 < column1)
                                    {
                                        for (int i = 1; i < row1 - row2; i++)
                                        {
                                            board[row1 - i][column1 - i] = null;
                                        }
                                    }
                                }

                                if ((row2 == BoardSize-1 && color == Color.White) || (row2 == 0 && color == Color.Black))
                                    board[row2][column2]!.TypeOfPiece = TypeOfPiece.Queen; // превращение в дамку
                                // генерируем новые ходы для проверки на то можно ли продолжать рубить
                                var newAvailableSteps = AvailableSteps(moving, board);
                                if (newAvailableSteps != null)
                                {
                                    if (availableSteps.Item2 && newAvailableSteps.Item2) // проверяем, доступен ли
                                    // ударный ход
                                    {
                                        // проверяем есть ли среди них ход нашей фигурой
                                        foreach (var t in newAvailableSteps.Item1)
                                        {
                                            if (t.Row1 == row2 && // если есть то игрок должен продолжить ход
                                                t.Column1 == column2)
                                            {
                                                Console.WriteLine("Продолжайте ход.");
                                                return Game(board, moving);
                                            }
                                        }
                                    }
                                }

                                if (color == Color.White) return Game(board, Color.Black);
                                return Game(board, Color.White);
                            }

                            Console.WriteLine("Ход невозможен.");
                        }
                        else Console.WriteLine("Ввод неверный. Попробуйте еще раз.");
                    }
                    else Console.WriteLine("Ввод неверный. Попробуйте еще раз.");
                }
                else Console.WriteLine("Ввод неверный. Попробуйте еще раз.");
            }
        }

        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8; // используем кодировку UTF-8, чтобы корректно выводить шашки
            var board = FullBoard(); // генерируем доску
            Console.WriteLine("Вводите ходы в формате \"x y\", где x - координата фигуры, которую вы хотите подвинуть" +
                              ", а y - координата клетки, в которую вы хотите поставить фигуру (например, \"c3 d4\").");
            Console.WriteLine("Введите \"Сдаюсь\", если хотите сдаться.");
            Console.WriteLine("Введите \"Ничья\", чтобы отправить предлоежение о ничье сопернику.");
            var winner = Game(board, Color.White); //первыми ходят белые
            if (winner == Color.Black) Console.WriteLine("Победили чёрные.");
            else if (winner == Color.White) Console.WriteLine("Победили белые.");
            else Console.WriteLine("Ничья.");
        }
    }
}