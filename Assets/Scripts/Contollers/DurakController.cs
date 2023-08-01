using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Helpers;
using DurakServer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEditor;

public class DurakController : MonoBehaviour
{
    public static DurakController instance;

    //Игроки
    public List<EnemyPlayer> enemyPlayers;
    public DurakPlayer humanPlayer;

    public DeckBox talon;
    public GameObject deckSprite;

    public RiverCont riverCont;
    public Trash trash;
    
    public btnEndAttack endAttack;
    public btnEndDefence endDefence;
    public btnEndAdding endAdding;

    public GameObject gameEndPanel;
    public Text gameEndMessage;
    
    public bool TwoPlayersLeft = false;

    private async void Awake()
    {
        instance = this;

        CardBox.trump = talon.GetTrumpSuit();
        SetTrumpCard();

        await Client.instance.InitiliazeTimerStream();
    }

    private void OnApplicationQuit()
    {


        Client.instance?.ShutdownConnect();
    }


    private void SetTrumpCard()
    {
        CardBox trump = talon.GetTrumpCard();
        trump.FaceUp = true;
        trump.GetCardImage();

        RectTransform rt = trump.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(1030, -260);
        rt.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        //rt.transform.Rotate(0, 0, 90);
        rt.localRotation = Quaternion.Euler(0, 0, 90);
        trump.transform.SetAsFirstSibling();

        SuitIcon trumpIcon = talon.SuitIcons.First(icon => icon.suitName == trump.suit);
        trumpIcon.gameObject.SetActive(true);
        trumpIcon.transform.SetAsFirstSibling();
        deckSprite.transform.SetAsFirstSibling();
    }
    // -----------------Awake functions END------------------------
    private void Start()
    {
        SetPositionsAccordingToInitialRoles();
        SetInformationBase();
        SetInitialRoles();
        SetDeckCardsPositions();
        GiveCardsToEnemies();

        UpdateGameVisuals();
    }
    private void SetPositionsAccordingToInitialRoles()
    {
        if ((Client.instance.DurakLobby.enemyPlayers[0].Role == Role.Defender && Client.instance.DurakLobby.enemyPlayers[1].Role == Role.Attacker) || 
            (Client.instance.DurakLobby.enemyPlayers[0].Role == Role.Attacker && Client.instance.DurakLobby.enemyPlayers[1].Role == Role.Waiter) ||
            (Client.instance.DurakLobby.enemyPlayers[0].Role == Role.Waiter && Client.instance.DurakLobby.enemyPlayers[1].Role == Role.Defender)
            )
        {
            enemyPlayers[0].HasLeftPosition = false;
            enemyPlayers[1].HasLeftPosition = true;
        }
        else
        {
            enemyPlayers[0].HasLeftPosition = true;
            enemyPlayers[1].HasLeftPosition = false;
        }
    }
    public void SetInformationBase()
    {
        humanPlayer.username.text = Client.instance.DurakLobby.iPlayer.Username;

        if (enemyPlayers[0].HasLeftPosition)
        {
            enemyPlayers[0].username.text = Client.instance.DurakLobby.enemyPlayers[0].Username;
            enemyPlayers[1].username.text = Client.instance.DurakLobby.enemyPlayers[1].Username;
        }
        else
        {
            enemyPlayers[1].username.text = Client.instance.DurakLobby.enemyPlayers[0].Username;
            enemyPlayers[0].username.text = Client.instance.DurakLobby.enemyPlayers[1].Username;
        }
    }
    public void SetInitialRoles()
    {
        switch (Client.instance.DurakLobby.iPlayer.Role)
        {
            case Role.Attacker:
                humanPlayer.role = Role.Attacker;
                break;
            case Role.Defender:
                humanPlayer.role = Role.Defender;
                break;
            case Role.Waiter:
                humanPlayer.role = Role.Waiter;
                break;
        }

        foreach (var enemyServer in Client.instance.DurakLobby.enemyPlayers)
        {
            foreach (var enemyGame in enemyPlayers)
            {
                if (enemyServer.Username.Equals(enemyGame.username.text))
                {
                    switch (enemyServer.Role)
                    {
                        case Role.Attacker:
                            enemyGame.role = Role.Attacker;
                            break;
                        case Role.Defender:
                            enemyGame.role = Role.Defender;
                            break;
                        case Role.Waiter:
                            enemyGame.role = Role.Waiter;
                            break;
                    }
                }
            }
        }

    }
    private void SetDeckCardsPositions()
    {
        foreach (var cardBox in talon.ShuffledDeckList)
        {
            if (cardBox == talon.GetTrumpCard())
            {
                continue;
            }
            cardBox.FaceUp = false;
            cardBox.GetCardImage();
            RectTransform rt = cardBox.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(1120, -140);
            rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        }
    }
    public void GiveCardsToEnemies()
    {
        foreach (var enemyServer in Client.instance.DurakLobby.enemyPlayers)
        {
            foreach (var enemyGame in enemyPlayers)
            {
                if (enemyServer.Username.Equals(enemyGame.username.text))
                {
                    foreach (var card in enemyServer.Hand)
                    {
                        var cardBox = talon.DeckList.FirstOrDefault(x => x.rank == card.Rank && x.suit == card.Suit);
                        cardBox.transform.SetParent(enemyGame.playerHand.transform);
                        enemyGame.playerHand.MyCards.Add(cardBox);
                    }
                }
            }
        }
    }

    public void PutCardToRiver(CardBox card)
    {
        if (humanPlayer.playerHand.MyCards.Contains(card))
        {
            humanPlayer.playerHand.MyCards.Remove(card);
            riverCont.Add(card, humanPlayer.role);
        }
        else if (enemyPlayers[0].playerHand.MyCards.Contains(card))
        {
            enemyPlayers[0].playerHand.MyCards.Remove(card);
            riverCont.Add(card, enemyPlayers[0].role);
        }
        else if (enemyPlayers[1].playerHand.MyCards.Contains(card))
        {
            enemyPlayers[1].playerHand.MyCards.Remove(card);
            riverCont.Add(card, enemyPlayers[1].role);
        }
        
        card.transform.SetParent(riverCont.transform);
        // The sound of the throwed card
        FindObjectOfType<AudioManager>().Play("ThrowCard1");

        UpdateGameVisuals();
    }
    public void HandleEndAttackState(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        UpdatePlayersRolesFromServer(iPlayer, ePlayers);

        int waiterAndInactiveCount = 0;
        if (humanPlayer.role == Role.Waiter || humanPlayer.role == Role.Inactive) waiterAndInactiveCount++;
        if (enemyPlayers[0].role == Role.Waiter || enemyPlayers[0].role == Role.Inactive) waiterAndInactiveCount++;
        if (enemyPlayers[1].role == Role.Waiter || enemyPlayers[1].role == Role.Inactive) waiterAndInactiveCount++;

        if (waiterAndInactiveCount == 1) DefenderBeatsCards(iPlayer, ePlayers);

        UpdateGameVisuals();
    }
    public void HandleEndDefenceState(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        UpdatePlayersRolesFromServer(iPlayer, ePlayers);

        if (riverCont.attackerList.Count == 6) DefenderTakesCards(iPlayer, ePlayers);

        UpdateGameVisuals();
    }
    public void HandleEndAddingState(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        UpdatePlayersRolesFromServer(iPlayer, ePlayers);

        if (TwoPlayersLeft == true)
        {
            DefenderTakesCards(iPlayer, ePlayers);
        } else
        {
            if (humanPlayer.role != Role.Adder && enemyPlayers[0].role != Role.Adder && enemyPlayers[1].role != Role.Adder)
                DefenderTakesCards(iPlayer, ePlayers);
        }

        UpdateGameVisuals();
    }
    public void HandleFinishGameRoundState(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        UpdatePlayersRolesFromServer(iPlayer, ePlayers);

        DefenderBeatsCards(iPlayer, ePlayers);

        UpdateGameVisuals();
    }
    public void HandleEnableTwoPlayersMode(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        TwoPlayersLeft = true;

        UpdatePlayersRolesFromServer(iPlayer, ePlayers);

        UpdateGameVisuals();
    }
    public void HandleGameEnd(List<WinnerPlayer> winners, string winMessage)
    {
        TwoPlayersLeft = true;
        

        foreach (WinnerPlayer winner in winners)
        {
            if (humanPlayer.username.text.Equals(winner.Username))
            {
                UpdateButtonStates(false, false, false);
                foreach (var card in humanPlayer.playerHand.MyCards) card.GetComponent<Button>().interactable = false;

                if (humanPlayer.status.sprite.name != $"{winner.BeetCount}bits")
                {
                    humanPlayer.status.sprite = Resources.Load<Sprite>($"{winner.BeetCount}bits");
                    RectTransform statusRect = humanPlayer.status.GetComponent<RectTransform>();
                    Vector3 initialPosition = statusRect.localPosition;

                    statusRect.localPosition = new Vector3(Screen.width / 2, -(Screen.height / 2));
                    statusRect.sizeDelta = new Vector2(157, 84);

                    DOTween.Sequence()
                        .Append(statusRect.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                        .Append(statusRect.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                        .Append(statusRect.DOPivot(new Vector2(0f, 1f), 0f))
                        .Append(statusRect.DOSizeDelta(new Vector2(105, 56), 1f, false))
                        .PrependInterval(0.5f)
                        .Append(statusRect.DOJumpAnchorPos(new Vector3(initialPosition.x + 10, initialPosition.y - 20), 3f, 1, 1f, false));
                }

                FindObjectOfType<AudioManager>().Play("GameWin1");

                if (winner.BeetCount == 3 || winner.BeetCount == 5) 
                    winMessage += $"{winner.BeetCount} битов";

                ActivateGameEndPanel(winMessage);
            }

            foreach (var enemyPlayer in enemyPlayers)
            {
                if (enemyPlayer.username.text.Equals(winner.Username)) {
                    if (enemyPlayer.status.sprite.name != $"{winner.BeetCount}bits")
                    {
                        enemyPlayer.role = Role.Inactive;
                        enemyPlayer.status.sprite = Resources.Load<Sprite>($"{winner.BeetCount}bits");
                        RectTransform statusRect = enemyPlayer.status.GetComponent<RectTransform>();
                        Vector3 initialPosition = statusRect.localPosition;

                        statusRect.localPosition = new Vector3(Screen.width/2, -(Screen.height / 2));
                        statusRect.sizeDelta = new Vector2(157, 84);

                        DOTween.Sequence()
                            .Append(statusRect.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                            .Append(statusRect.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                            .Append(statusRect.DOPivot(new Vector2(0f, 1f), 0f))
                            .Append(statusRect.DOSizeDelta(new Vector2(105, 56), 1f, false))
                            .PrependInterval(0.5f)
                            .Append(statusRect.DOJumpAnchorPos(new Vector3(initialPosition.x + 10, initialPosition.y - 20), 3f, 1, 1f, false));
                    }
                }
            }
        }
    }

    private void DefenderBeatsCards(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        foreach (var card in iPlayer.Hand.ToList())
        {
            if (humanPlayer.playerHand.MyCards.Contains(humanPlayer.playerHand.MyCards.FindCard(card))) continue;
            var cardBox = talon.ShuffledDeckList.FindCard(card);
            cardBox.transform.SetParent(humanPlayer.playerHand.transform);
            humanPlayer.GetHand().MyCards.Add(cardBox);
            talon.ShuffledDeckList.Remove(cardBox);
        }

        foreach (var enemyPlayer in enemyPlayers)
        {
            foreach (var ePlayer in ePlayers.ToList())
            {
                if (enemyPlayer.username.text.Equals(ePlayer.Username))
                {
                    foreach (var card in ePlayer.Hand.ToList())
                    {
                        if (enemyPlayer.playerHand.MyCards.Contains(enemyPlayer.playerHand.MyCards.FindCard(card))) continue;
                        var cardBox = talon.ShuffledDeckList.FindCard(card);
                        cardBox.transform.SetParent(enemyPlayer.playerHand.transform);
                        enemyPlayer.GetHand().MyCards.Add(cardBox);
                        talon.ShuffledDeckList.Remove(cardBox);
                    }
                }
            }
        }

        // Переносим карты в биту
        FindObjectOfType<AudioManager>().Play("ThrowToTrash");
        foreach (var cardBox in riverCont.attackerList.ToList())
        {
            riverCont.attackerList.Remove(cardBox);
            trash.Add(cardBox);
            cardBox.transform.SetParent(trash.transform);
        }
        foreach (var cardBox in riverCont.defenceList.ToList())
        {
            riverCont.defenceList.Remove(cardBox);
            trash.Add(cardBox);
            cardBox.transform.SetParent(trash.transform);
        }
    }
    private void DefenderTakesCards(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        riverCont.adderList.AddRange(riverCont.attackerList);
        riverCont.adderList.AddRange(riverCont.defenceList);

        riverCont.attackerList.Clear();
        riverCont.defenceList.Clear();

        foreach (var card in iPlayer.Hand.ToList())
        {
            if (humanPlayer.playerHand.MyCards.Contains(humanPlayer.playerHand.MyCards.FindCard(card))) continue;

            if (riverCont.adderList.Contains(riverCont.adderList.FindCard(card)))
            {
                var cardBox = riverCont.adderList.FindCard(card);
                cardBox.transform.SetParent(humanPlayer.playerHand.transform);
                humanPlayer.GetHand().MyCards.Add(cardBox);
                riverCont.adderList.Remove(cardBox);
            }
            else
            {
                var cardBox = talon.ShuffledDeckList.FindCard(card);
                cardBox.transform.SetParent(humanPlayer.playerHand.transform);
                humanPlayer.GetHand().MyCards.Add(cardBox);
                talon.ShuffledDeckList.Remove(cardBox);
            }
        }

        FindObjectOfType<AudioManager>().Play("ThrowToTrash");
        foreach (var enemyPlayer in enemyPlayers)
        {
            foreach (var ePlayer in ePlayers.ToList())
            {
                if (enemyPlayer.username.text.Equals(ePlayer.Username))
                {
                    foreach (var card in ePlayer.Hand.ToList())
                    {
                        if (enemyPlayer.playerHand.MyCards.Contains(enemyPlayer.playerHand.MyCards.FindCard(card))) continue;

                        if (riverCont.adderList.Contains(riverCont.adderList.FindCard(card)))
                        {
                            var cardBox = riverCont.adderList.FindCard(card);
                            riverCont.adderList.Remove(cardBox);
                            enemyPlayer.GetHand().MyCards.Add(cardBox);
                            cardBox.transform.SetParent(enemyPlayer.playerHand.transform);
                        }
                        else
                        {
                            var cardBox = talon.ShuffledDeckList.FindCard(card);
                            talon.ShuffledDeckList.Remove(cardBox);
                            enemyPlayer.GetHand().MyCards.Add(cardBox);
                            cardBox.transform.SetParent(enemyPlayer.playerHand.transform);
                        }
                    }
                }
            }
        }
    }

    // Function that updates the roles of the players from the server
    private void UpdatePlayersRolesFromServer(DurakNetPlayer iPlayer, List<DurakNetPlayer> ePlayers)
    {
        humanPlayer.role = iPlayer.Role;
        foreach (var enemyPlayer in enemyPlayers)
            foreach (var enemyServer in ePlayers)
                if (enemyPlayer.username.text.Equals(enemyServer.Username))
                    enemyPlayer.role = enemyServer.Role;
    }

    // Function that updates POSITIONS, BUTTON STATES and CARD INTERACTABILITY
    private void UpdateGameVisuals()
    {
        UpdateButtonStatesDueRoles();
        UpdateHumanCardsPositions();
        UpdateComputerCardsPositions();
        UpdateCardInteractability();
        SetPlayableCards();
        UpdatePlayerStatuses();
    }

    // POSITIONS 
    public void UpdateHumanCardsPositions()
    {
        SortHandCards();

        foreach (var card in humanPlayer.GetHand().MyCards)
        {
            card.FaceUp = true;
            card.GetCardImage();
            card.transform.SetParent(humanPlayer.playerHand.transform);

            //card.transform.SetParent(humanPlayer.playerHand.transform);
            RectTransform rt = card.GetComponent<RectTransform>();
            rt.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (humanPlayer.playerHand.MyCards.Count == 0)
        {
            return;
        }
        if (humanPlayer.playerHand.MyCards.Count == 1)
        {
            CardBox card = humanPlayer.playerHand.MyCards.First();
            RectTransform rt = card.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(450, -475f);
            rt.localScale = new Vector3(1, 1, 1);
        }
        else if (humanPlayer.playerHand.MyCards.Count == 2)
        {
            int x = 325;
            int coefficientAlterationX = 150;

            foreach (var card in humanPlayer.playerHand.MyCards)
            {
                RectTransform rt = card.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(x, -475f);
                rt.localScale = new Vector3(1, 1, 1);
                x += coefficientAlterationX;
            }
        }
        else if (humanPlayer.playerHand.MyCards.Count == 3)
        {
            int x = 250;
            int coefficientAlterationX = 150;

            foreach (var card in humanPlayer.playerHand.MyCards)
            {
                RectTransform rt = card.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(x, -475f);
                rt.localScale = new Vector3(1, 1, 1);
                x += coefficientAlterationX;
            }
        }
        else if (humanPlayer.playerHand.MyCards.Count == 4)
        {
            int x = 150;
            int coefficientAlterationX = 150;

            foreach (var card in humanPlayer.playerHand.MyCards)
            {
                RectTransform rt = card.GetComponent<RectTransform>();
                //rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false);
                //rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false);
                //rt.DOPivot(new Vector2(0f, 1f), 0f);
                //rt.DOScale(new Vector3(1, 1, 1), 0f);
                //rt.DOAnchorPos(new Vector3(x, -525), 0.05f, false);

                rt.localPosition = new Vector3(x, -475f);
                rt.localScale = new Vector3(1, 1, 1);
                x += coefficientAlterationX;
            }
        }
        else if (humanPlayer.playerHand.MyCards.Count == 5)
        {
            int x = 100;
            int coefficientAlterationX = 150;

            foreach (var card in humanPlayer.playerHand.MyCards)
            {
                RectTransform rt = card.GetComponent<RectTransform>();
                //rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false);
                //rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false);
                //rt.DOPivot(new Vector2(0f, 1f), 0f);
                //rt.DOScale(new Vector3(1, 1, 1), 0f);
                //rt.DOAnchorPos(new Vector3(x, -525), 0.05f, false);

                rt.localPosition = new Vector3(x, -475f);
                rt.localScale = new Vector3(1, 1, 1);
                x += coefficientAlterationX;
            }
        }
        else
        {
            int x = 30;
            int coefficientAlterationX = 750 / (humanPlayer.playerHand.MyCards.Count - 1);

            foreach (var card in humanPlayer.playerHand.MyCards)
            {
                RectTransform rt = card.GetComponent<RectTransform>();
                //rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false);
                //rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false);
                //rt.DOPivot(new Vector2(0f, 1f), 0f);
                //rt.DOScale(new Vector3(1, 1, 1), 0f);
                //rt.DOScale(new Vector3(1, 1, 1), 0f);
                //rt.DOAnchorPos(new Vector3(x, -525), 0.05f, false);
                rt.localPosition = new Vector3(x, -475f);
                rt.localScale = new Vector3(1, 1, 1);
                x += coefficientAlterationX;
            }
        }
    }
    private void UpdateComputerCardsPositions()
    {
        int count = 0;
        foreach (var enemy in enemyPlayers)
        {
            count++;
            foreach (var card in enemy.GetHand().MyCards.ToList())
            {
                card.FaceUp = false;
                card.GetCardImage();

                RectTransform rt = card.GetComponent<RectTransform>();
                rt.localRotation = Quaternion.Euler(0, 0, 0);
            }

            switch (count)
            {
                case 1:
                    {
                        if (enemy.GetHand().MyCards.Count == 0)
                        {
                            continue;
                        }
                        if (enemy.GetHand().MyCards.Count == 1)
                        {
                            CardBox card = enemy.playerHand.MyCards.First();
                            RectTransform rt = card.GetComponent<RectTransform>();

                            DOTween.Sequence()
                                .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.2f))
                                .Join(rt.DOAnchorPos(new Vector3(75, -220f), 0.2f, false));

                            //rt.localPosition = new Vector3(15, -205);
                            //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        }
                        else if (enemy.GetHand().MyCards.Count == 2)
                        {
                            int x = 75;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 30;

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -220f), cardSetTime, false));

                                //rt.localPosition = new Vector3(x, -205);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                            }
                        }
                        else if (enemy.GetHand().MyCards.Count == 3)
                        {
                            int x = 75;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 30;

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -220f), cardSetTime, false));
                                
                                //rt.localPosition = new Vector3(x, -205);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                                cardSetTime += 0.1f;
                            }
                        }
                        else if (enemy.GetHand().MyCards.Count == 4)
                        {
                            int x = 75;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 30;

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -220f), cardSetTime, false));

                                //rt.localPosition = new Vector3(x, -205);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                                cardSetTime += 0.1f;
                            }
                        }
                        else
                        {
                            int x = 75;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 125 / (enemy.playerHand.MyCards.Count - 1);

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -220f), cardSetTime, false));

                                //rt.localPosition = new Vector3(x, -205);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                                cardSetTime += 0.1f;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        if (enemy.GetHand().MyCards.Count == 0)
                        {
                            return;
                        }
                        if (enemy.GetHand().MyCards.Count == 1)
                        {
                            CardBox card = enemy.playerHand.MyCards.First();
                            RectTransform rt = card.GetComponent<RectTransform>();

                            DOTween.Sequence()
                                .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.2f))
                                .Join(rt.DOAnchorPos(new Vector3(450, -65), 0.2f, false));

                            //rt.localPosition = new Vector3(610, 0);
                            //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        }
                        else if (enemy.GetHand().MyCards.Count == 2)
                        {
                            int x = 450;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 30;

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -65), cardSetTime, false));

                                //rt.localPosition = new Vector3(x, 0);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                                cardSetTime += 0.1f;
                            }
                        }
                        else if (enemy.GetHand().MyCards.Count == 3)
                        {
                            int x = 450;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 30;

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -65), cardSetTime, false));

                                //rt.localPosition = new Vector3(x, 0);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                                cardSetTime += 0.1f;
                            }
                        }
                        else if (enemy.GetHand().MyCards.Count == 4)
                        {
                            int x = 450;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 30;

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -65), cardSetTime, false));

                                //rt.localPosition = new Vector3(x, 0);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                                cardSetTime += 0.1f;
                            }
                        }
                        else
                        {
                            int x = 450;
                            float cardSetTime = 0.1f;
                            int coefficientAlterationX = 125 / (enemy.playerHand.MyCards.Count - 1);

                            foreach (var card in enemy.GetHand().MyCards)
                            {
                                RectTransform rt = card.GetComponent<RectTransform>();

                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.3f, 0.3f, 0.3f), cardSetTime))
                                    .Join(rt.DOAnchorPos(new Vector3(x, -65), cardSetTime, false));

                                //rt.localPosition = new Vector3(x, 0);
                                //rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                x += coefficientAlterationX;
                                cardSetTime += 0.1f;
                            }
                        }
                    }
                    break;
            }
        }
    }

    // BUTTON STATES
    public void UpdateButtonStatesDueRoles()
    {
        switch (humanPlayer.role)
        {
            case Role.Attacker:
                {
                    if (riverCont.attackerList.Count == riverCont.defenceList.Count && riverCont.attackerList.Count > 0 && riverCont.attackerList.Count + riverCont.defenceList.Count != 12) UpdateButtonStates(true, false, false);
                    else UpdateButtonStates(false, false, false);
                }
                break;
            case Role.Defender:
                {
                    if (riverCont.attackerList.Count > riverCont.defenceList.Count) UpdateButtonStates(false, true, false);
                    else if (riverCont.adderList.Count > 0 || riverCont.attackerList.Count == riverCont.defenceList.Count) UpdateButtonStates(false, false, false);

                    foreach (var enemyPlayer in enemyPlayers)
                        if (enemyPlayer.role == Role.Adder)
                            UpdateButtonStates(false, false, false);
                }
                break;
            case Role.Adder:
                UpdateButtonStates(false, false, true);
                break;
            case Role.Waiter:
                UpdateButtonStates(false, false, false);
                break;
            case Role.FormerAttacker:
                UpdateButtonStates(false, false, false);
                break;
            case Role.Inactive:
                UpdateButtonStates(false, false, false);
                break;
        }
    }
    public void UpdateButtonStates(bool attacking, bool defencing, bool adding)
    {
        Button attack = endAttack.GetComponent<Button>();
        Button defence = endDefence.GetComponent<Button>();
        Button pass = endAdding.GetComponent<Button>();
        if (attacking == true)
        {
            attack.interactable = true;
            attack.gameObject.SetActive(true);
            defence.interactable = false;
            defence.gameObject.SetActive(false);
            pass.interactable = false;
            pass.gameObject.SetActive(false);
        }
        else if (defencing == true)
        {
            attack.interactable = false;
            attack.gameObject.SetActive(false);
            defence.interactable = true;
            defence.gameObject.SetActive(true);
            pass.interactable = false;
            pass.gameObject.SetActive(false);
        }
        else if (adding == true)
        {
            attack.interactable = false;
            attack.gameObject.SetActive(false);
            defence.interactable = false;
            defence.gameObject.SetActive(false);
            pass.interactable = true;
            pass.gameObject.SetActive(true);
        }
        else
        {
            attack.interactable = false;
            attack.gameObject.SetActive(false);
            defence.interactable = false;
            defence.gameObject.SetActive(false);
            pass.interactable = false;
            pass.gameObject.SetActive(false);
        }
    }

    // CARD INTERACTABILITY
    public void UpdateCardInteractability()
    {
        DisableOuterCards();

        int defCount = 0;
        if (humanPlayer.role == Role.Defender) defCount = humanPlayer.playerHand.MyCards.Count;
        else if (enemyPlayers[0].role == Role.Defender) defCount = enemyPlayers[0].playerHand.MyCards.Count;
        else if (enemyPlayers[1].role == Role.Defender) defCount = enemyPlayers[1].playerHand.MyCards.Count;

        switch (humanPlayer.role)
        {
            case Role.Attacker:
                {
                    if (riverCont.attackerList.Count + riverCont.adderList.Count < 6 && (riverCont.attackerList.Count + riverCont.adderList.Count - riverCont.defenceList.Count < defCount))
                    {
                        foreach (var card in humanPlayer.playerHand.MyCards)
                        {
                            Button cardClicker = card.GetComponent<Button>();
                            cardClicker.interactable = true;
                        }
                    }
                    else
                    {
                        foreach (var card in humanPlayer.playerHand.MyCards)
                        {
                            Button cardClicker = card.GetComponent<Button>();
                            cardClicker.interactable = false;
                        }
                    }
                }
                break;
            case Role.Defender:
                {
                    if (riverCont.attackerList.Count + riverCont.defenceList.Count == 0 || riverCont.attackerList.Count == riverCont.defenceList.Count || enemyPlayers[0].role == Role.Adder || enemyPlayers[1].role == Role.Adder)
                    {
                        foreach (var cardBox in humanPlayer.GetHand().MyCards)
                        {
                            Button cardClicker = cardBox.GetComponent<Button>();
                            cardClicker.interactable = false;
                        }
                    }
                    else
                    {
                        foreach (var cardBox in humanPlayer.GetHand().MyCards)
                        {
                            Button cardClicker = cardBox.GetComponent<Button>();
                            cardClicker.interactable = true;
                        }
                    }
                }
                break;
            case Role.Adder:
                {
                    if (riverCont.attackerList.Count + riverCont.adderList.Count < 6 && (riverCont.attackerList.Count + riverCont.adderList.Count - riverCont.defenceList.Count < defCount))
                    {
                        foreach (var card in humanPlayer.playerHand.MyCards)
                        {
                            Button cardClicker = card.GetComponent<Button>();
                            cardClicker.interactable = true;
                        }
                    }
                    else if (riverCont.attackerList.Count + riverCont.adderList.Count == 6)
                    {
                        foreach (var card in humanPlayer.playerHand.MyCards)
                        {
                            Button cardClicker = card.GetComponent<Button>();
                            cardClicker.interactable = false;
                        }
                    }
                }
                break;
            case Role.Waiter:
            case Role.FormerAttacker:
            case Role.Inactive:
                {
                    foreach (var card in humanPlayer.playerHand.MyCards)
                    {
                        Button cardClicker = card.GetComponent<Button>();
                        cardClicker.interactable = false;
                    }
                }
                break;
        }
    }
    public void DisableOuterCards()
    {
        foreach (var card in riverCont.attackerList)
        {
            Button cardClicker = card.GetComponent<Button>();
            cardClicker.interactable = false;
        }
        foreach (var card in riverCont.defenceList)
        {
            Button cardClicker = card.GetComponent<Button>();
            cardClicker.interactable = false;
        }
        foreach (var card in riverCont.adderList)
        {
            Button cardClicker = card.GetComponent<Button>();
            cardClicker.interactable = false;
        }
        foreach (var card in talon.ShuffledDeckList)
        {
            Button cardClicker = card.GetComponent<Button>();
            cardClicker.interactable = false;
        }
        foreach (var enemyPlayer in enemyPlayers)
        {
            foreach (var card in enemyPlayer.playerHand.MyCards)
            {
                Button cardClicker = card.GetComponent<Button>();
                cardClicker.interactable = false;
            }
        }

    }

    // PLAYERS' STATUSES
    public void UpdatePlayerStatuses()
    {
        switch (humanPlayer.role)
        {
            case Role.Attacker:
                humanPlayer.status.sprite = Resources.Load<Sprite>("AttackerStatus");
                break;
            case Role.Defender:
                humanPlayer.status.sprite = Resources.Load<Sprite>("DefenderStatus");
                break;
            case Role.Adder:
                humanPlayer.status.sprite = Resources.Load<Sprite>("AdderStatus");
                break;
            case Role.Waiter:
                humanPlayer.status.sprite = Resources.Load<Sprite>("WaiterStatus");
                break;
            case Role.FormerAttacker:
                humanPlayer.status.sprite = Resources.Load<Sprite>("WaiterStatus");
                break;
        }

        foreach (var enemy in enemyPlayers)
        {
            switch (enemy.role)
            {
                case Role.Attacker:
                    enemy.status.sprite = Resources.Load<Sprite>("AttackerStatus");
                    break;
                case Role.Defender:
                    enemy.status.sprite = Resources.Load<Sprite>("DefenderStatus");
                    break;
                case Role.Adder:
                    enemy.status.sprite = Resources.Load<Sprite>("AdderStatus");
                    break;
                case Role.Waiter:
                    enemy.status.sprite = Resources.Load<Sprite>("WaiterStatus");
                    break;
                case Role.FormerAttacker:
                    enemy.status.sprite = Resources.Load<Sprite>("WaiterStatus");
                    break;
                case Role.Inactive:
                    break;
            }
        }
    }

    public void SortHandCards()
    {
        List<CardBox> sortedHand = humanPlayer.playerHand.MyCards.OrderBy(card => card.suit == talon.GetTrumpSuit()).ThenBy(card => card.rank).ToList();

        humanPlayer.playerHand.MyCards.Clear();

        foreach (var card in sortedHand)
            card.transform.SetParent(humanPlayer.transform);

        humanPlayer.playerHand.MyCards.AddRange(sortedHand);
        sortedHand.Clear();
    }
    public void SetPlayableCards()
    {
        List<CardBox> notBeatedCardBox = new List<CardBox>();
        if (riverCont.attackerList.Count > riverCont.defenceList.Count)
        {
            int indexOfNotBeatedCard = riverCont.defenceList.Count;
            notBeatedCardBox.Add(riverCont.attackerList[indexOfNotBeatedCard]);
        }

        List<CardBox> addedCardBoxes = new List<CardBox>();
        addedCardBoxes.AddRange(riverCont.attackerList);
        addedCardBoxes.AddRange(riverCont.defenceList);

        int defCount = 0;
        if (humanPlayer.role == Role.Defender) defCount = humanPlayer.playerHand.MyCards.Count;
        else if (enemyPlayers[0].role == Role.Defender) defCount = enemyPlayers[0].playerHand.MyCards.Count;
        else if (enemyPlayers[1].role == Role.Defender) defCount = enemyPlayers[1].playerHand.MyCards.Count;

        //Loop through all the Cardbox controls
        foreach (CardBox card in humanPlayer.GetHand().MyCards)
        {
            Button bt = card.GetComponent<Button>();
            RectTransform rt = card.GetComponent<RectTransform>();

            if (humanPlayer.role == Role.Attacker || humanPlayer.role == Role.Adder)
            {
                if (card.isAddable(addedCardBoxes) && (riverCont.attackerList.Count + riverCont.adderList.Count - riverCont.defenceList.Count < defCount))
                {
                    bt.interactable = true;
                }
                else
                {
                    bt.interactable = false;

                    //rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false);
                    //rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false);
                    //rt.DOPivot(new Vector2(0f, 1f), 0f);
                    //rt.DOAnchorPos(new Vector3(rt.localPosition.x, rt.localPosition.y - 70), 0.1f, false);

                    rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y - 20);
                }
            }


            if (humanPlayer.role == Role.Defender && riverCont.attackerList.Count > riverCont.defenceList.Count && enemyPlayers[0].role != Role.Adder && enemyPlayers[1].role != Role.Adder)
            {
                if (card.isPlayable(notBeatedCardBox))
                {
                    bt.interactable = true;
                }
                else
                {
                    bt.interactable = false;
                    //rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false);
                    //rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false);
                    //rt.DOPivot(new Vector2(0f, 1f), 0f);
                    //rt.DOAnchorPos(new Vector3(rt.localPosition.x, rt.localPosition.y - 70), 0.1f, false);

                    rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y - 20);
                }
            }
        }
    }
    private void ActivateGameEndPanel(string winMessage)
    {
        gameEndMessage.text = winMessage;
        gameEndPanel.SetActive(true);
    }
    public async void btnEndAttack_Click()
    {
        FindObjectOfType<AudioManager>().Play("Done");
        await Client.instance.EndAttackRequest();
    }
    public async void btnEndDefence_Click()
    {
        FindObjectOfType<AudioManager>().Play("Take1");
        await Client.instance.EndDefenceRequest();
    }
    public async void btnEndAdding_Click()
    {
        FindObjectOfType<AudioManager>().Play("Pass");
        await Client.instance.EndAddingRequest();
    }
}