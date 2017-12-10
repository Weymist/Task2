﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TwentyOne
{
    class Player
    {
        int _score;
        List<Card> _hand;
        int _name;
        int takeOrNot = new Random().Next(1, new Random().Next(1, 666));
        Thread gameProcess;
        Croupier _croupier;

        public Player(Croupier croupier, int name)
        {
            _croupier = croupier;
            Score = 0;
            _name = name;
            Hand = new List<Card>();
            gameProcess = new Thread(Game);
            gameProcess.Name = name.ToString();
            gameProcess.Start();
        }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

        public int Name
        {
            get
            { return _name; }
        }

        List<Card> Hand
        {
            get { return _hand; }
            set { _hand = value; }
        }

        public delegate void Finish(Player player);
        public event Finish FinishEvent;
        public void FinishEventSend(object sender, EventArgs e)
        {
            FinishEvent(this);
        }

        public delegate void Stop(Player player);
        public event Stop StopEvent;
        public void StopEventSend(object sender, EventArgs e)
        {
            StopEvent(this);
        }

        public delegate void Over(Player player);
        public event Over Lose;
        public void LoseEventSend(object sender, EventArgs e)
        {
            Lose(this);
        }

        public void Game()
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

        public void TakeCard(Card card)
        {
            Hand.Add(card);
            Score += card._cost;
            Game();
        }

        void AskCard(int threadSleepTime)
        {
            Thread.Sleep(threadSleepTime);
            Console.WriteLine("{0} игрок берёт карту", gameProcess.Name);
            lock (_croupier)
            {
                _croupier.GiveCard(this);
            }
        }

        public void StopThread()
        {
            gameProcess.Abort();
        }

        public void PrintHand()
        {
            foreach (Card card in Hand)
            {
                Console.Write(card._content + " ");
            }
            Console.WriteLine();
        }

        public string ShowHand()
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
