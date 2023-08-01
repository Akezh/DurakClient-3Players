using System;
using System.Collections.Generic;
using System.Linq;
using DurakServer;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Hand : MonoBehaviour
{
    public DeckBox DeckBox;
    public bool isEnemy;

    public List<CardBox> MyCards;

    const int HAND_SIZE = 6;

    private void Start()
    {
        MyCards = new List<CardBox>();

        if (!isEnemy)
        {
            foreach (var card in Client.instance.DurakLobby.iPlayer.Hand)
            {
                var cardBox = DeckBox.DeckList.FirstOrDefault(x => x.rank == card.Rank && x.suit == card.Suit);
                MyCards.Add(cardBox);
                cardBox.transform.SetParent(transform);
            }

            ShowHand();
        }
    }

    private void Remove(CardBox cardBox)
    {
        MyCards.Remove(cardBox);
    } 
    private void ShowHand()
    {
        DurakController.instance.SortHandCards();

        int x = 80;
        float cardSetTime = 0.2f;
        int coefficientAlterationX = 750/(MyCards.Count - 1);

        FindObjectOfType<AudioManager>().Play("DealCards");
        foreach (var card in MyCards)
        {
            card.FaceUp = true;
            card.GetCardImage();

            RectTransform rt = card.GetComponent<RectTransform>();
            //rt.localRotation = Quaternion.Euler(0, 0, 0);
            //rt.localPosition = new Vector3(x, -475f);

            rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false);
            rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false);
            rt.DOPivot(new Vector2(0f, 1f), 0f);
            rt.DOScale(new Vector3(1, 1, 1), cardSetTime);
            rt.DORotate(new Vector3(0, 0, 0), cardSetTime);
            rt.DOAnchorPos(new Vector3(x, -525f), cardSetTime, false);

            x += coefficientAlterationX;
            cardSetTime += 0.3f;
        }
    }
}
