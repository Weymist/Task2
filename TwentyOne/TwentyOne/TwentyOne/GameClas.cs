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
                players[i].Lose += PlayerLose;
                players[i].FinishEvent += PlayerFinish;
            }

            Console.ReadKey();
        }

        void PlayerStop(Player player)
        {
            playerFinish++;
            if (winPlayer == null)
            {
                winPlayer = player;
            }
            else
            {
                winPlayer = (winPlayer.Score > player.Score) ? winPlayer : player;
            }
            Console.WriteLine("Игрок {0} составил руку", player.Name);
            hands += player.ShowHand() + "\n";
            FinishGame();
            player.StopThread();
        }

        void PlayerLose(Player player)
        {
            playerFinish++;
            Console.WriteLine("Игрок {0}: <<Перебор!>>", player.Name);
            player.PrintHand();
            FinishGame();
            player.StopThread();
        }

        void PlayerFinish(Player player)
        {
            player.PrintHand();
            Console.WriteLine("Игрок {0} набрал 21", player.Name);
            foreach (Player man in players)
            {
                man.StopThread();
            }
        }

        void FinishGame()
        {
            if (playerFinish == playersCount)
            {
                Console.WriteLine("Имя выигравшего игрока: " +
                    (winPlayer != null ? winPlayer.Name.ToString() : "все слились"));
                Console.WriteLine(hands);
            }
        }
    }
}
