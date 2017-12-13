using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne
{
    class Croupier
    {
        int cardsCount; //счетчик оставшихся в колоде карт
        Dictionary<int, string> deck = new Dictionary<int, string> //список всех возможных типов и стоимости карт в колоде 
        {
            {3, "J"}, {4, "Q" }, {5, "K" }, {6,"6" }, {7,"7" }, { 8,"8"}, {9,"9" }, {10,"10" }, {11,"A" }
        };
        Card[] deckArr; //колода

        public Croupier()
        {
            deckArr = Help.Shuffle(deck); //тасование карт
            cardsCount = 36; //начальное кол-во карт в колоде
        }

        public void GiveCard(Player player) //метод выдачи карты игроку
        {
            cardsCount--; 
            player.TakeCard(deckArr[cardsCount]);
        }
    }
}
