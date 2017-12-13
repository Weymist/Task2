using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TwentyOne
{
    class Player
    {
        int _score; //счет
        List<Card> _hand; //рука
        int _name; //имя игрока
        int takeOrNot = new Random().Next(1, new Random().Next(1, 666)); //случ. число (исп. для определения брать или не брать карту)     
        Thread gameProcess; //поток 
        Croupier _croupier; //крупье

        public Player(Croupier croupier, int name) //конструктор игрока
        {
            _croupier = croupier;
            Score = 0;
            _name = name;
            Hand = new List<Card>(); //создание списка карт в руке
            gameProcess = new Thread(Game); //создание потока
            gameProcess.Name = name.ToString(); //задание названия потока
            gameProcess.Start(); //запуск потока
        }

        public int Score //свойство счет
        {
            get //возвращение значения поля
            {
                return _score;
            }
            set //присваивание значения полю
            {
                _score = value;
            }
        }

        public int Name  //свойство имя
        {
            get //возвращение значения
            { return _name; }
        }

        List<Card> Hand //список карт в руке
        {
            get { return _hand; } //возвращение значения
            set { _hand = value; } //присваивание значения
        }

        public delegate void Finish(Player player);  //создание делегата 
        public event Finish FinishEvent; //создание события связанного с делегатом
        public void FinishEventSend(object sender, EventArgs e) //метод вызова события
        {
            FinishEvent(this);
        }

        public delegate void Stop(Player player); //создание делегата
        public event Stop StopEvent; //создание события связанного с делегатом
        public void StopEventSend(object sender, EventArgs e) //метод вызова события
        {
            StopEvent(this);
        }

        public delegate void Over(Player player); //создание делегата
        public event Over Lose;//создание события связанного с делегатом
        public void LoseEventSend(object sender, EventArgs e) //метод вызова события
        {
            Lose(this);
        }

        public void Game() //метод, определяющий, будет ли игрок брать карту (в зависимости от кол-ва очков)
        {
            if (Score == 21)
            {
                FinishEventSend(null, null);
            }
            else if (Score <= 11)
            {
                AskCard(400/Name);
            }
            else if (Score < 16)
            {
                if (takeOrNot % 2 == 0)
                {
                    AskCard(200);
                }
                else
                {
                    StopEventSend(null, null);
                }
            }
            else if (Score < 19)
            {
                if (takeOrNot % 5 == 0)
                {
                    AskCard(300);
                }
                else
                {
                    StopEventSend(null, null);
                }
            }
            else if (Score < 21)
            {
                StopEventSend(null, null);
            }
            else
            {
                LoseEventSend(null, null);
            }
        }

        public void TakeCard(Card card) //взятие карты игроком
        {
            Hand.Add(card); //добавление карты в руку
            Score += card._cost; //изменение счета игрока
            Game(); //проверка на продолжение(конец) игры
        }

        void AskCard(int threadSleepTime) //метод, выполняющийся при запросе карты игроком
        {
            Thread.Sleep(threadSleepTime); //приостановка потока
            Console.WriteLine("{0} игрок берёт карту", gameProcess.Name); 
            lock (_croupier) //закрытие крупье для других потоков на время выдачи карты
            {
                _croupier.GiveCard(this); //выдача карты игроку
            }
        }

        public void StopThread()
        {
            gameProcess.Abort(); //остановка потока
        }

        public void PrintHand() //вывод руки в консоль перебравшего игрока или набравшего 21 очко
        {
            foreach (Card card in Hand)
            {
                Console.Write(card._content + " ");
            }
            Console.WriteLine();
        }

        public string ShowHand() //вывод руки в консоль когда закончил игру
        {
            string hand = "";
            foreach (Card card in Hand)
            {
                hand += card._content + " ";
            }
            return hand;
        }
    }
}
