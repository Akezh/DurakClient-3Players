using System;
using System.Collections;

public class Cards : CollectionBase, ICloneable
{
    /// <summary>
    /// Add a card to the collection
    /// </summary>
    /// <param name="newCard"> The card to be added </param>
    public void Add(CardBox newCard)
    {
        List.Add(newCard);
    }

    /// <summary>
    /// Remove a card from the collection
    /// </summary>
    /// <param name="oldCard"> The card to be removed </param>
    public void Remove(CardBox oldCard)
    {
        List.Remove(oldCard);
    }

    /// <summary>
    /// Gets or Sets a card at the given index
    /// </summary>
    /// <param name="cardIndex"> The index of the card </param>
    /// <returns> The card at the index </returns>
    public CardBox this[int cardIndex]
    {
        get
        {
            return (CardBox)List[cardIndex];
        }
        set
        {
            List[cardIndex] = value;
        }
    }

    /// <summary>
    /// Utility method for copying card instances into another Cards
    /// instance—used in Deck.Shuffle(). This implementation assumes that
    /// source and target collections are the same size.
    /// </summary>
    public void CopyTo(Cards targetCards)
    {
        for (int index = 0; index < this.Count; index++)
        {
            targetCards[index] = this[index];
        }
    }

    /// <summary>	
    /// Check to see if the Cards collection contains a particular card.
    /// This calls the Contains() method of the ArrayList for the collection,
    /// which you access through the InnerList property.
    /// </summary>
    public bool Contains(CardBox card)
    {
        return InnerList.Contains(card);
    }

    /// <summary>
    /// Clones the cards collection
    /// </summary>
    /// <returns> The clone as an object </returns>
    public object Clone()
    {
        Cards newCards = new Cards();
        foreach (CardBox sourceCard in List)
        {
            newCards.Add((CardBox)sourceCard.Clone());
        }
        return newCards;
    }
}
