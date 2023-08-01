using System;
using System.Collections.Generic;
using Random = System.Random;

public interface IComputerPlayer
{
    /// <summary>
    /// Selects a card to be played
    /// </summary>
    /// <param name="playedCards"> The cards that have been played </param>
    /// <returns> The card that was played </returns>
    CardBox selectCard(List<CardBox> playedCards);
}

public class EasyPlayer : DurakPlayer, IComputerPlayer
{
    //public EasyPlayer(DeckBox cardDraws) /*: base(cardDraws)*/
    //{
        
    //}

    public CardBox selectCard(List<CardBox> playedCards)
    {
        List<CardBox> playableCards = this.GetPlayableCards(playedCards);

        // Number of playable cards
        int numberOfCards = playableCards.Count;

        // Random number allows the AI to select a card at a random index
        Random randomNumber = new Random();

        // If there are no playable cards
        if (numberOfCards == 0)
        {
            throw new OperationCanceledException("No cards are playable...");
        }
        // Select a random playable card from the hand
        CardBox returnCard = playableCards[randomNumber.Next(0, numberOfCards)];

        // Remove any matching card at this index in the hand before adding the same card in the river
        playerHand.MyCards.Remove(returnCard);

        // Return card object
        return returnCard;
    }
}
