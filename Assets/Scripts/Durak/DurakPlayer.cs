using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DurakServer;
using UnityEngine.UI;
using TMPro;

public class DurakPlayer : MonoBehaviour
{
    // Hand object
    public TextMeshProUGUI username;
    public Image status;
    public GameObject dialog;
    public Hand playerHand;
    public Role role;

    const int DEFAULT_HAND_SIZE = 6;

    /// <summary>
    /// Default Player constructor
    /// </summary>
    /// <param name="cardDraws"> The deck to draw the hand from </param>
    //public DurakPlayer()
    //{
    //    playerHand = new Hand(cardDraws);
    //}

    private void Start()
    {
        playerHand.isEnemy = false;
    }

    /// <summary>
    /// Getter for the human player's hand
    /// </summary>
    /// <returns>the player's hand</returns>
    public Hand GetHand()
    {
        return playerHand;
    }

    /// <summary>
    /// If there is less than six cards in hand, the hand is replenished with more cards from the deck
    /// </summary>
    /// <param name="cardDraws"> The deck to draw cards from </param>
    public void FillHand(DeckBox talon)
    {
        for (int cardCount = playerHand.MyCards.Count; cardCount < DEFAULT_HAND_SIZE && talon.ShuffledDeckList.Count != 0; cardCount++)
        {
            playerHand.MyCards.Add(talon.DrawCardFromShuf(0));
        }
    }

    /// <summary>
    /// Get all cards that are playable based on cards that are already in the river
    /// </summary>
    /// <param name="playedCards"> The cards that have been played </param>
    /// <returns> A collection of playable cards </returns>
    public List<CardBox> GetPlayableCards(List<CardBox> playedCards)
    {
        List<CardBox> playableCards = new List<CardBox>();

        foreach (CardBox card in playerHand.MyCards)
        {
            if (card.isPlayable(playedCards))
            {
                playableCards.Add(card);
            }
        }

        return playableCards;
    }

    /// <summary>
    /// Takes an array of cards from the river and adds them to the hand of the defender if the attacker wins
    /// </summary>
    /// <param name="addCards"> The collection of cards to be added </param>
    public void AddCardsToHand(List<CardBox> addCards)
    {
        foreach (CardBox card in addCards)
        {
            playerHand.MyCards.Add(card);
        }
    }

    /// <summary>
    /// Draws a card from the deck
    /// </summary>
    /// <param name="drawDeck"> The deck to draw the card from </param>
    public void DrawCard(DeckBox drawDeck)
    {
        playerHand.MyCards.Add(drawDeck.DrawCard());
    }
}
