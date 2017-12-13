using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne
{
    class Help
    {
        static List<int> number = new List<int>
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
            19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 }; //создание списка незанятых позиций в колоде
        static Random rnd = new Random();

        public static Card[] Shuffle(Dictionary<int, string> deck) //замешивание карт в случайные места
        {
            Card[] arr = new Card[36]; //создание колоды карт
            for (int i = 0, n = 3; i < 36; n++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int index = rnd.Next(0, Math.Max(0, 36 - i)); //выбор случайной позиции в колоде
                    arr[number[index]] = new Card(n, deck[n]); //вставка карты в колоду на выбранную позицию
                    number.RemoveAt(index); //удаление этой позиции из списка возможных
                    i++;
                }
            }
            return arr;
        }
    }
}
