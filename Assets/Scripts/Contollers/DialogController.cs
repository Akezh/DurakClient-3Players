using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DurakServer;
using UnityEngine.UI;
using TMPro;

public class DialogController : MonoBehaviour
{
    public static DialogController instance;

    [Header("UI Panels:")]
    public GameObject DialogPanel;

    [Header("Dialog Panel:")]
    public GameObject oi;
    public GameObject hi;
    public GameObject niceGame;
    public GameObject goodLuck;
    public GameObject thanks;
    public GameObject hurry;

    public TextMeshProUGUI dialogWindow;
    public GameObject chatBtn;

    private bool IsDialogPanelActive;

    private void Awake()
    {
        instance = this;

        AddDialogMessageListeners();
    }

    private void AddDialogMessageListeners()
    {
        Button dialogMessage1 = oi.GetComponent<Button>();
        dialogMessage1.onClick.AddListener(() => { OnDialogMessageClick(Dialog.Oi); });

        Button dialogMessage2 = hi.GetComponent<Button>();
        dialogMessage2.onClick.AddListener(() => { OnDialogMessageClick(Dialog.Hi); });

        Button dialogMessage3 = niceGame.GetComponent<Button>();
        dialogMessage3.onClick.AddListener(() => { OnDialogMessageClick(Dialog.NiceGame); });

        Button dialogMessage4 = goodLuck.GetComponent<Button>();
        dialogMessage4.onClick.AddListener(() => { OnDialogMessageClick(Dialog.GoodLuck); });

        Button dialogMessage5 = hurry.GetComponent<Button>();
        dialogMessage5.onClick.AddListener(() => { OnDialogMessageClick(Dialog.Hurry); });

        Button dialogMessage6 = thanks.GetComponent<Button>();
        dialogMessage6.onClick.AddListener(() => { OnDialogMessageClick(Dialog.Thanks); });
    }

	public void ShowDialogPanel()
    {
        IsDialogPanelActive = !IsDialogPanelActive;
        DialogPanel.SetActive(IsDialogPanelActive);
        chatBtn.SetActive(false);
    }

    public async void OnDialogMessageClick(Dialog message)
    {
        IsDialogPanelActive = !IsDialogPanelActive;
        DialogPanel.SetActive(IsDialogPanelActive);
        chatBtn.SetActive(true);
        await Client.instance.SendDialogMessageAsync(message);
    }
    public void HandleSentDialogMessage(Dialog message, string username)
    {
        if (DurakController.instance.humanPlayer.username.text.Equals(username))
        {
            TextMeshProUGUI dialogMessage = DurakController.instance.humanPlayer.dialog.GetComponentInChildren<TextMeshProUGUI>();
            switch (message)
            {
                case Dialog.Oi:
                    dialogMessage.text = "Ой!";
                    break;
                case Dialog.Hi:
                    dialogMessage.text = "Привет!";
                    break;
                case Dialog.NiceGame:
                    dialogMessage.text = "Хорошая игра";
                    break;
                case Dialog.GoodLuck:
                    dialogMessage.text = "Удачи!";
                    break;
                case Dialog.Thanks:
                    dialogMessage.text = "Спасибо";
                    break;
                case Dialog.Hurry:
                    dialogMessage.text = "Побыстрее";
                    break;
            }
            StartCoroutine(DialogCoroutine(DurakController.instance.humanPlayer.dialog));
        }

        foreach (var enemyPlayerGame in DurakController.instance.enemyPlayers)
        {
            if (enemyPlayerGame.username.text.Equals(username))
            {
                TextMeshProUGUI dialogMessage = enemyPlayerGame.dialog.GetComponentInChildren<TextMeshProUGUI>();
                switch (message)
                {
                    case Dialog.Oi:
                        dialogMessage.text = "Ой!";
                        break;
                    case Dialog.Hi:
                        dialogMessage.text = "Привет!";
                        break;
                    case Dialog.NiceGame:
                        dialogMessage.text = "Хорошая игра";
                        break;
                    case Dialog.GoodLuck:
                        dialogMessage.text = "Удачи!";
                        break;
                    case Dialog.Thanks:
                        dialogMessage.text = "Спасибо";
                        break;
                    case Dialog.Hurry:
                        dialogMessage.text = "Побыстрее";
                        break;
                }

                StartCoroutine(DialogCoroutine(enemyPlayerGame.dialog));
            }
        }
    }

    private IEnumerator DialogCoroutine(GameObject dialog)
    {
        dialog.SetActive(true);

        yield return new WaitForSeconds(4);

        dialog.SetActive(false);
    }

}
