using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba1
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Встановлюємо кодування консолі на UTF-8 для підтримки спеціальних символів.
            Console.OutputEncoding = Encoding.UTF8;

            // Створюємо об'єкти для двох гравців та гру.
            GameAccount player1 = new GameAccount();
            GameAccount player2 = new GameAccount();
            Game game = new Game(player1, player2);

            // Розпочинаємо гру.
            game.StartGame();

            // Виводимо статистику гравців після завершення гри.
            player1.GetStats();
            player2.GetStats();
        }
    }

    // Клас, що представляє гравця гри.
    public class GameAccount
    {
        public string UserName { get; set; } // Ім'я гравця
        public int CurrentRating { get; set; } // Поточний рейтинг гравця
        private List<GameResult> gameHistory; // Історія ігор гравця
        public int GamesCount { get; set; } // Кількість ігор гравця

        // Конструктор класу GameAccount, де можливо вказати початкову кількість ігор (за замовчуванням - 0).
        public GameAccount(int gamesCount = 0)
        {
            GamesCount = gamesCount;
            gameHistory = new List<GameResult>();
        }

        // Методи для фіксації результатів гри та оновлення статистики гравця.

        public void WinGame(string opponentName, int rating)
        {
            GamesCount++;
            CurrentRating += rating;
            gameHistory.Add(new GameResult(opponentName, true, rating));
        }

        public void LoseGame(string opponentName, int rating)
        {
            GamesCount++;
            CurrentRating -= rating;
            gameHistory.Add(new GameResult(opponentName, false, rating));
        }

        // Виведення статистики гравця на консоль.
        public void GetStats()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("\n.................................\n");
            Console.WriteLine($"ІСТОРІЯ ІГОР для {UserName}:");
            for (int i = 0; i < gameHistory.Count; i++)
            {
                var result = gameHistory[i];
                String matchResult;
                if (result.Won) { matchResult = "Перемога"; } else { matchResult = "Поразка"; }
                Console.WriteLine($"Гра {i + 1}: \n" +
                                  $"Опонент: {result.OpponentName}\n" +
                                  $"Результат: {(matchResult)}\n" +
                                  $"Зміна рейтингу: {result.RatingChange}\n");
            }
            Console.WriteLine($"Поточний рейтинг для {UserName}: {CurrentRating}\n" +
                              $"Кількість ігор: {GamesCount}\n");
        }
    }

    // Клас, що представляє результат однієї гри.
    public class GameResult
    {
        public string OpponentName { get; }
        public bool Won { get; }
        public int RatingChange { get; }

        public GameResult(string opponentName, bool won, int ratingChange)
        {
            OpponentName = opponentName;
            Won = won;
            RatingChange = ratingChange;
        }
    }

    // Клас, що представляє гру між двома гравцями.
    public class Game
    {
        public GameAccount player1 { get; set; }
        public GameAccount player2 { get; set; }

        public Game(GameAccount player1, GameAccount player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }

        // Розпочати гру між гравцями.
        public void StartGame()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Ласкаво просимо до гри!\n");

            // Введення імен гравців та початкового рейтингу.
            Console.Write("Введіть ім'я першого гравця: ");
            player1.UserName = Console.ReadLine().Trim();

            Console.Write("Введіть ім'я другого гравця: ");
            player2.UserName = Console.ReadLine().Trim();

            Console.Write("\nВведіть початковий рейтинг: ");
            int startRating = Convert.ToInt32(Console.ReadLine());
            while (startRating <= 0)
            {
                Console.WriteLine("Початковий рейтинг повинен бути більше 0");
                Console.Write("Введіть початковий рейтинг: ");
                startRating = Convert.ToInt32(Console.ReadLine());
            }
            player1.CurrentRating = startRating;
            player2.CurrentRating = startRating;

            // Початок гри.
            Play();
        }

        // Метод, що виконує гру між гравцями.
        public void Play()
        {
            Console.WriteLine("\n--------------------------------------------------------\n");
            Console.Write("Введіть рейтинг на який граєте: ");
            int rating = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            if (rating < 0)
            {
                Console.WriteLine("Некоректне значення. Введіть додатнє число.");
                Play();
                return;
            }
            if (rating > player1.CurrentRating - 1 || rating > player2.CurrentRating - 1)
            {
                Console.WriteLine("У одного з гравців недостатньо рейтингу.");
                Play();
                return;
            }

            // Симуляція кидання кубиків і визначення переможця.
            Random random = new Random();
            int player1Roll = random.Next(1, 7);
            int player2Roll = random.Next(1, 7);
            Console.WriteLine($"{player1.UserName} кинув кубик і випало {player1Roll}");
            Console.WriteLine($"{player2.UserName} кинув кубик і випало {player2Roll}");
            if (player1Roll > player2Roll)
            {
                player1.WinGame(player2.UserName, rating);
                player2.LoseGame(player1.UserName, rating);
                Console.WriteLine($"Переміг {player1.UserName}!");
                player1.GetStats();
                player2.GetStats();
            }
            if (player1Roll < player2Roll)
            {
                player2.WinGame(player1.UserName, rating);
                player1.LoseGame(player2.UserName, rating);
                Console.WriteLine($"Переміг {player2.UserName}!");
                player1.GetStats();
                player2.GetStats();
            }
            if (player1Roll == player2Roll)
            {
                Console.WriteLine("Нічия");
            }

            // Питання про гру ще раз.
            Console.WriteLine("\n--------------------------------------------------------\n");
            Console.Write("Хочете зіграти ще одну гру? (Так/Ні): ");
            string playAgainResponse = Console.ReadLine().Trim();

            bool playAgain = true;
            if (!playAgainResponse.Equals("Так", StringComparison.OrdinalIgnoreCase))
            {
                playAgain = false;
            }
            if (playAgain) Play();
        }
    }
}
