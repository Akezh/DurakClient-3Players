using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Models;
using Assets.Scripts.Helpers;
using Cysharp.Threading.Tasks;
using DurakServer;
using Grpc.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Channel = Grpc.Core.Channel;
using Status = DurakServer.Status;
using System.Threading;

public class Client : MonoBehaviour
{
    public static Client instance;
    public CancellationTokenSource Token;
    private Channel _grpcChannel;
    private DurakGame.DurakGameClient durakClient;

    AsyncDuplexStreamingCall<DurakRequest, DurakReply> stream;
    public Lobby DurakLobby;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async void Connect()
    {
        _grpcChannel = new Channel("192.168.0.102:6322", ChannelCredentials.Insecure);

        await InitiliazeDurakStream();
    }

    public async UniTask InitiliazeDurakStream()
    {
        durakClient = new DurakGame.DurakGameClient(_grpcChannel);
        stream = durakClient.DurakStreaming();

        //Выстрел о начатии игры
        await stream.RequestStream.WriteAsync(new DurakRequest { PlayRequest = new PlayRequest() });

        //Слушание
        try
        {
            while (await stream.ResponseStream.MoveNext(_grpcChannel.ShutdownToken))
            {
                var currentMessage = stream.ResponseStream.Current;


                switch (currentMessage.ReplyCase)
                {
                    case DurakReply.ReplyOneofCase.LobbyReply:
                        {
                            DurakLobby = new Lobby
                            {
                                Id = currentMessage.LobbyReply.Id,
                                iPlayer = currentMessage.LobbyReply.IPlayer,
                                enemyPlayers = currentMessage.LobbyReply.EnemyPlayers.ToList(),
                                DeckList = currentMessage.LobbyReply.DeckBox.ToList(),
                                TrumpBox = currentMessage.LobbyReply.Trump,
                            };

                            StartCoroutine(LoadDurakScene());
                        }
                        break;
                    case DurakReply.ReplyOneofCase.DialogReply:
                        {
                            Dialog dialogMessage = currentMessage.DialogReply.Dialog;
                            string senderPlayerUsername = currentMessage.DialogReply.Username;
                            DialogController.instance.HandleSentDialogMessage(dialogMessage, senderPlayerUsername);
                        }
                        break;
                    case DurakReply.ReplyOneofCase.TurnReply:
                        {
                            Card card = currentMessage.TurnReply.Card;
                            TransferFromServerRiver(currentMessage.TurnReply);
                        }
                        break;
                    case DurakReply.ReplyOneofCase.EndAttackReply:
                        {
                            DurakNetPlayer iPlayer = currentMessage.EndAttackReply.IPlayer;
                            List<DurakNetPlayer> ePlayers = currentMessage.EndAttackReply.EnemyPlayers.ToList();
                            DurakController.instance.HandleEndAttackState(iPlayer, ePlayers);
                        }
                        break;
                    case DurakReply.ReplyOneofCase.EndDefenceReply:
                        {
                            DurakNetPlayer iPlayer = currentMessage.EndDefenceReply.IPlayer;
                            List<DurakNetPlayer> ePlayers = currentMessage.EndDefenceReply.EnemyPlayers.ToList();
                            DurakController.instance.HandleEndDefenceState(iPlayer, ePlayers);
                        }
                        break;
                    case DurakReply.ReplyOneofCase.EndAddingReply:
                        {
                            DurakNetPlayer iPlayer = currentMessage.EndAddingReply.IPlayer;
                            List<DurakNetPlayer> ePlayers = currentMessage.EndAddingReply.EnemyPlayers.ToList();
                            DurakController.instance.HandleEndAddingState(iPlayer, ePlayers);
                        }
                        break;
                    case DurakReply.ReplyOneofCase.FinishGameRoundReply:
                        {
                            DurakNetPlayer iPlayer = currentMessage.FinishGameRoundReply.IPlayer;
                            List<DurakNetPlayer> ePlayers = currentMessage.FinishGameRoundReply.EnemyPlayers.ToList();
                            DurakController.instance.HandleFinishGameRoundState(iPlayer, ePlayers);
                        }
                        break;
                    case DurakReply.ReplyOneofCase.EnableTwoPlayersModeReply:
                        {
                            DurakNetPlayer iPlayer = currentMessage.EnableTwoPlayersModeReply.IPlayer;
                            List<DurakNetPlayer> ePlayers = currentMessage.EnableTwoPlayersModeReply.EnemyPlayers.ToList();
                            DurakController.instance.HandleEnableTwoPlayersMode(iPlayer, ePlayers);
                        }
                        break;
                    case DurakReply.ReplyOneofCase.GameEndReply:
                        {
                            List<WinnerPlayer> winners = currentMessage.GameEndReply.WinnerPlayers.ToList();
                            string winMessage = currentMessage.GameEndReply.WinMessage;
                            DurakController.instance.HandleGameEnd(winners, winMessage);
                        }
                        break;
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await stream.RequestStream.CompleteAsync();
        }


        
    }
    public async UniTask InitiliazeTimerStream()
    {
        Token = new CancellationTokenSource();

        using (var stream = durakClient.StartTimerStreaming(new TimerRequest
            {
                LobbyId = DurakLobby.Id, 
                Username = DurakLobby.iPlayer.Username
            }, 
            null, null, Token.Token))
        {
            try
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    var timerReply = stream.ResponseStream.Current;
                    TimerController.instance.SetTimer(timerReply.Time, timerReply.Username);
                }
            }
            catch
            {
                Debug.Log("Stream cancelled.");
                ShutdownConnect();
            }
        }
    }

    public void ShutdownConnect()
    {
        Token.Cancel();
        stream.RequestStream.CompleteAsync();
    }

    public async UniTask SendDialogMessageAsync(Dialog message)
    {
        var sendMessageRequest = new DurakRequest { DialogRequest = new DialogRequest { Dialog = message } };

        await stream.RequestStream.WriteAsync(sendMessageRequest);
    }
    public async UniTask SendCardRequest(Card card)
    {
        var sendCardRequest = new DurakRequest { TurnRequest = new TurnRequest { Card = card } };

        await stream.RequestStream.WriteAsync(sendCardRequest);
    }
    public async UniTask EndAttackRequest()
    {
        var endAttackRequest = new DurakRequest { EndAttackRequest = new EndAttackRequest { } };

        await stream.RequestStream.WriteAsync(endAttackRequest);
    }
    public async UniTask EndDefenceRequest()
    {
        var endDefenceRequest = new DurakRequest { EndDefenceRequest = new EndDefenceRequest { } };

        await stream.RequestStream.WriteAsync(endDefenceRequest);
    }
    public async UniTask EndAddingRequest()
    {
        var endAddingRequest = new DurakRequest { EndAddingRequest = new EndAddingRequest { } };

        await stream.RequestStream.WriteAsync(endAddingRequest);
    }
    public void TransferFromServerRiver(TurnReply reply)
    {
        CardBox cardBox = DurakController.instance.humanPlayer.playerHand.MyCards.FindCard(reply.Card);

        EnemyPlayer enemyPlayer1 = DurakController.instance.enemyPlayers[0];
        EnemyPlayer enemyPlayer2 = DurakController.instance.enemyPlayers[1];

        if (cardBox.IsTrueNull())
            cardBox = enemyPlayer1.playerHand.MyCards.FindCard(reply.Card);

        if (cardBox.IsTrueNull())
            cardBox = enemyPlayer2.playerHand.MyCards.FindCard(reply.Card);

        DurakController.instance.PutCardToRiver(cardBox);
    }
    public IEnumerator LoadDurakScene()
    {
        SceneManager.LoadScene(1);
        yield break;
    }
}
