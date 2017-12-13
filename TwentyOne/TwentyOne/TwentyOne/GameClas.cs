using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne
{
    class GameClas
    {
        const int playersCount = 3;  //кол-во игроков
        int playerFinish = 0; //кол-во закончивших игроков
        Player winPlayer; //выигравший игрок
        string hands = ""; //финальные руки игроков, которые не перебрали
        Croupier croupier = new Croupier(); //создание крупье
        Player[] players = new Player[playersCount]; //массив игроков

        public GameClas()
        {
            Play();
        }

        void Play()
        {
            for (int i = 0; i < playersCount; i++)
            {
                players[i] = new Player(croupier, i + 1); //создание игроков
                players[i].StopEvent += PlayerStop; //подписка на события
                players[i].Lose += PlayerLose; //подписка на события
                players[i].FinishEvent += PlayerFinish; //подписка на события
            }

            Console.ReadKey();
        }

        void PlayerStop(Player player) //метод, выполняющийся, если у игрока <21 и он не будет брать карту 
        {
            playerFinish++; //увел. счетчика закончивших игроков
            if (winPlayer == null) //определение победителя на момент этого события
            {
                winPlayer = player; 
            }
            else
            {
                winPlayer = (winPlayer.Score > player.Score) ? winPlayer : player;
            }
            Console.WriteLine("Игрок {0} составил руку", player.Name); 
            hands += player.ShowHand() + "\n"; //вывод руки игрока на экран
            FinishGame(); //проверка, все ли игроки закончили
            player.StopThread(); //остановка потока
        }

        void PlayerLose(Player player) //метод, выполняющийся, если игрок набрал >21 
        {
            playerFinish++; //увел. счетчика закончивших игроков
            Console.WriteLine("Игрок {0}: <<Перебор!>>", player.Name);
            player.PrintHand();  //вывод руки игрока на экран
            FinishGame(); //проверка, все ли игроки закончили
            player.StopThread(); //остановка потока
        }

        void PlayerFinish(Player player) //метод, выполняющийся, если игрок набрал ровно 21
        {
            player.PrintHand(); //вывод руки игрока на экран
            Console.WriteLine("Игрок {0} набрал 21", player.Name);
            foreach (Player man in players)
            {
                man.StopThread(); //остановка всех потоков
            }
        }

        void FinishGame() //метод, выполняющийся, когда все игроки завершили набор карт
        {
            if (playerFinish == playersCount)
            {
                Console.WriteLine("Имя выигравшего игрока: " +
                    (winPlayer != null ? winPlayer.Name.ToString() : "все слились")); //выводится имя победителя, либо "все слились" если у всех >21 
                Console.WriteLine(hands); //вывод на экран рук всех закончивших игроков
            }
        }
    }
}
