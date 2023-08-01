using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DurakServer;
using System.Linq;
using Random = System.Random;


public class DeckBox : MonoBehaviour
{
    public List<CardBox> DeckList;
    public List<CardBox> ShuffledDeckList;
    public List<SuitIcon> SuitIcons;

    //public void Shuffle()
    //{
    //    var numberOfCards = DeckList.Count;
    //    var randomNumber = new Random();

    //    for (var draws = 0; draws < numberOfCards; draws++)
    //    {
    //        ShuffledDeckList.Add(DrawCard(randomNumber.Next(0, DeckList.Count)));
    //    }
    //}

    private void Start()
    {
        ShuffleFromServer(Client.instance.DurakLobby.DeckList);
    }

    public void ShuffleFromServer(List<Card> cards)
    {
        foreach (var card in cards)
        {
            var cardBox = DeckList.FirstOrDefault(x => x.rank == card.Rank && x.suit == card.Suit);
            ShuffledDeckList.Add(cardBox);
        }
    }

    public CardBox DrawCard(int position = 0)
    {
        CardBox returnCard = DeckList[position];

        DeckList.Remove(returnCard);

        return returnCard;
    }

    public CardBox DrawCardFromShuf(int position = 0)
    {
        CardBox returnCard = ShuffledDeckList[position];

        ShuffledDeckList.Remove(returnCard);

        return returnCard;
    }

    public Suit GetTrumpSuit()
    {
        //int index = ShuffledDeckList.Count - 1;

        //return ShuffledDeckList[index].suit;

        return Client.instance.DurakLobby.TrumpBox.Suit;
    }

    public CardBox GetTrumpCard()
    {
        //return ShuffledDeckList[ShuffledDeckList.Count - 1];

        var trump = Client.instance.DurakLobby.TrumpBox;
        return DeckList.FirstOrDefault(x => x.rank == trump.Rank && x.suit == trump.Suit);
    }

}
