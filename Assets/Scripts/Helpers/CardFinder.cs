using DurakServer; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    public static class CardFinder
    {
        public static CardBox FindCard(this List<CardBox> cardBoxes, Card card)
        {
            return cardBoxes.FirstOrDefault(x => x.rank == card.Rank && x.suit == card.Suit);
        }

        public static bool IsTrueNull(this UnityEngine.Object obj)
        {
            return obj == null;
        }
    }
}
