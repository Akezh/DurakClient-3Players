using System;
using System.Collections;
using System.Collections.Generic;
using DurakServer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardBox : MonoBehaviour, ICloneable, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    Vector3 offset;
    Vector3 initialPosition;

    void Awake()
    {
        MainCamera = Camera.allCameras[0];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Button cardButton = this.GetComponent<Button>();
        if (cardButton.interactable)
        {
            initialPosition = gameObject.transform.position;
            offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        } else
        {
            return;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        Button cardButton = this.GetComponent<Button>();

        if (cardButton.interactable)
        {
            transform.SetAsLastSibling();
            Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
            newPos.z = 0;
            transform.position = newPos + offset;
        }
        else
        {
            return;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Button cardButton = this.GetComponent<Button>();
        if (cardButton.interactable)
        {
            if (initialPosition.y + 1.2 <= transform.position.y)
            {
                print("Initial Position Y" + initialPosition.y);
                print("The final position Y" + transform.position.y);
                Card_Click(this);
            }

            DurakController.instance.UpdateHumanCardsPositions();
            DurakController.instance.SetPlayableCards();
        }
        else
        {
            return;
        }
    }

    public async void Card_Click(CardBox card)
    {
        if (DurakController.instance.riverCont.attackerList.Count + DurakController.instance.riverCont.adderList.Count == 6 && DurakController.instance.humanPlayer.role != Role.Defender) return;
        Card toSend = new Card();
        toSend.Rank = card.rank;
        toSend.Suit = card.suit;

        await Client.instance.SendCardRequest(toSend);
    }

    // The rank of the card
    public Rank rank;

    // The suit of the card
    public Suit suit;

    // Whether or not the card is face up
    public bool faceUp= true; // Was private before

    /// <summary>
    /// Gets or Sets the faceUp property
    /// </summary>
    public bool FaceUp
    {
        set
        {
            faceUp = value;
        }
        get
        {
            return faceUp;
        }
    }

    /// <summary>
    /// The trump suit.
    /// </summary>
    public static Suit trump = Suit.Club;

    /// <summary>
    /// Default constructor
    /// </summary>
    public CardBox()
    {
    }

    /// <summary>
    /// Constructor that creates a card with the given rank and suit
    /// </summary>
    /// <param name="newSuit"> The suit of the new card </param>
    /// <param name="newRank"> The rank of the new card </param>
    public CardBox(Suit newSuit, Rank newRank)
    {
        suit = newSuit;
        rank = newRank;
    }

    /// <summary>
    /// Clones the card
    /// </summary>
    /// <returns> The clone of the card as an object </returns>
    public object Clone()
    {
        return MemberwiseClone();
    }

    /// <summary>
    /// Returns the card as a string
    /// </summary>
    /// <returns> A string that represents the card </returns>
    public override string ToString()
    {
        return "The " + rank + " of " + suit + "s";
    }

    /// <summary>
    /// Tests if two cards have an equal value
    /// </summary>
    /// <param name="card1"> The first card </param>
    /// <param name="card2"> The second card </param>
    /// <returns> Whether or not the cards are equal </returns>
    public static bool operator ==(CardBox card1, CardBox card2)
    {
        return (card1.suit == card2.suit) && (card1.rank == card2.rank);
    }

    /// <summary>
    /// Tests if two cards have a different value
    /// </summary>
    /// <param name="card1"> The first card </param>
    /// <param name="card2"> The second card </param>
    /// <returns> Whether or not the cards are different </returns>
    public static bool operator !=(CardBox card1, CardBox card2)
    {
        return !(card1 == card2);
    }

    /// <summary>
    /// Tests if two cards have an equal value
    /// </summary>
    /// <param name="card1"> The first card </param>
    /// <param name="card2"> The second card </param>
    /// <returns> Whether or not the cards are equal </returns>
    public override bool Equals(object card)
    {
        return this == (CardBox)card;
    }

    /// <summary>
    /// Gets the hashcode of a card
    /// </summary>
    /// <returns> The hashcode of a card </returns>
    public override int GetHashCode()
    {
        return 13 * (int)suit + (int)rank;
    }

    /// <summary>
    /// Tests if one card is greater than another
    /// </summary>
    /// <param name="card1"> The first card </param>
    /// <param name="card2"> The second card </param>
    /// <returns> Whether or one card is greater than the other </returns>
    public static bool operator >(CardBox card1, CardBox card2)
    {
        if (card1.suit != card2.suit)
        {
            if (card1.suit == CardBox.trump)
            {
                return true;
            }
            else if (card2.suit == CardBox.trump)
            {
                return false;
            }
            else
            {
                return (card1.rank > card2.rank);
            }
        }
        else
        {
            return (card1.rank > card2.rank);
        }
    }

    /// <summary>
    /// Tests if one card is less than another
    /// </summary>
    /// <param name="card1"> The first card </param>
    /// <param name="card2"> The second card </param>
    /// <returns> Whether or one card is less than the other </returns>
    public static bool operator <(CardBox card1, CardBox card2)
    {
        return !(card1 >= card2);
    }

    /// <summary>
    /// Tests if one card is greater than or equal to another
    /// </summary>
    /// <param name="card1"> The first card </param>
    /// <param name="card2"> The second card </param>
    /// <returns> Whether or one card is greater than or equal to the other </returns>
    public static bool operator >=(CardBox card1, CardBox card2)
    {
        if (card1.suit != card2.suit)
        {
            if (card1.suit == CardBox.trump)
            {
                return true;
            }
            else if (card2.suit == CardBox.trump)
            {
                return false;
            }
            else
            {
                return (card1.rank >= card2.rank);
            }
        }
        else
        {
            return (card1.rank >= card2.rank);
        }
    }

    /// <summary>
    /// Tests if one card is less than or equal to another
    /// </summary>
    /// <param name="card1"> The first card </param>
    /// <param name="card2"> The second card </param>
    /// <returns> Whether or one card is less than or equal to the other </returns>
    public static bool operator <=(CardBox card1, CardBox card2)
    {
        return !(card1 > card2);
    }

    /// <summary>
    /// Determines if the card is playable
    /// </summary>
    /// <param name="playedCards"> The cards to check against </param>
    /// <returns> True is the card is playable; false otherwise </returns>
    public bool isPlayable(List<CardBox> playedCards)
    {
        bool returnValue = false;

        if (playedCards.Count == 0)
        {
            returnValue = true;
        }
        else
        {
            if (this.suit == playedCards[playedCards.Count - 1].suit)
            {
                returnValue = (this > playedCards[playedCards.Count - 1]);
            }
            else if (this.suit == CardBox.trump)
            {
                returnValue = true;
            }
        }

        return returnValue;
    }

    public bool isAddable(List<CardBox> addedCards)
    {
        bool returnValue = false;

        if (addedCards.Count == 0)
        {
            returnValue = true;
        }
        else
        {
            foreach (CardBox card in addedCards)
            {
                if (this.rank == card.rank)
                {
                    returnValue = true;
                }
            }
        }

        return returnValue;
    }

    //public bool isPlayable(List<CardBox> playedCards)
    //{
    //    bool returnValue = false;

    //    if (playedCards.Count == 0)
    //    {
    //        returnValue = true;
    //    }
    //    else
    //    {
    //        if (playedCards.Count % 2 == 0)
    //        {
    //            foreach (CardBox card in playedCards)
    //            {
    //                if (this.rank == card.rank)
    //                {
    //                    returnValue = true;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (this.suit == playedCards[playedCards.Count - 1].suit)
    //            {
    //                returnValue = (this > playedCards[playedCards.Count - 1]);
    //            }
    //            else if (this.suit == CardBox.trump)
    //            {
    //                returnValue = true;
    //            }
    //        }
    //    }

    //    return returnValue;
    //}

    /// <summary>
    /// Gets the image of the card
    /// </summary>
    /// <returns> The image of the card </returns>
    public Sprite GetCardImage()
    {
        string imageName = "";    //the name of the image in the resources file
        Sprite cardImage;    //holds the image

        //if the card is not face up
        if (!faceUp)
        {
            //set the image name to "Back"
            imageName = "back"; //sets it to the image name foe the back of a card
        }
        else // else the card is fce up and ot joker
        {
            //set the image name to{Suit} _{Rank}
            switch (suit)
            {
                case Suit.Club:
                    imageName = "C";
                    break;
                case Suit.Diamond:
                    imageName = "D";
                    break;
                case Suit.Heart:
                    imageName = "H";
                    break;
                case Suit.Spade:
                    imageName = "S";
                    break;
                default:
                    break;
            }

            switch (rank)
            {
                case Rank.Six:
                    imageName += "6";
                    break;
                case Rank.Seven:
                    imageName += "7";
                    break;
                case Rank.Eight:
                    imageName += "8";
                    break;
                case Rank.Nine:
                    imageName += "9";
                    break;
                case Rank.Ten:
                    imageName += "10";
                    break;
                case Rank.Jack:
                    imageName += "J";
                    break;
                case Rank.Queen:
                    imageName += "Q";
                    break;
                case Rank.King:
                    imageName += "K";
                    break;
                case Rank.Ace:
                    imageName += "A";
                    break;
                default:
                    break;
            }

            //imageName = suit.ToString() + "_" + rank.ToString(); // enums are handy!
        }

        var rt = GetComponent<Image>();
        //set the image to the appropriate objcet we get from the resources file
        cardImage = Resources.Load<Sprite>($"Images/Durak/{imageName}");
        rt.sprite = cardImage;
        // return the image
        return cardImage;
    }
}