using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Trash : MonoBehaviour
{
    public List<CardBox> TrashList;

    public void Add(CardBox card)
    {
        card.FaceUp = false;
        card.GetCardImage();
        TrashList.Add(card);

        RectTransform rt = card.GetComponent<RectTransform>();
        Button bt = card.GetComponent<Button>();
        bt.interactable = false;

        switch (TrashList.Count)
        {
            case 1:
                DOTween.Sequence()
                    .Append(rt.DOAnchorMax(new UnityEngine.Vector2(0f, 1f), 0f, false))
                    .Append(rt.DOAnchorMin(new UnityEngine.Vector2(0f, 1f), 0f, false))
                    .Append(rt.DOPivot(new UnityEngine.Vector2(0f, 1f), 0f))
                    .Append(rt.DOScale(new UnityEngine.Vector3(0.45f, 0.45f, 0.45f), 0.3f))
                    .Join(rt.DOAnchorPos(new UnityEngine.Vector3(1200, -50), 0.3f, false))
                    .Join(rt.DORotate(new UnityEngine.Vector3(0, 0, 30), 0.3f));

                //rt.localPosition = new UnityEngine.Vector3(1200, -10);
                //rt.localScale = new UnityEngine.Vector3(0.45f, 0.45f, 0.45f);
                //rt.localRotation = UnityEngine.Quaternion.Euler(0, 0, 30);
                // Надо будет потом задать углы
                break;
            case 2:
                DOTween.Sequence()
                    .Append(rt.DOAnchorMax(new UnityEngine.Vector2(0f, 1f), 0f, false))
                    .Append(rt.DOAnchorMin(new UnityEngine.Vector2(0f, 1f), 0f, false))
                    .Append(rt.DOPivot(new UnityEngine.Vector2(0f, 1f), 0f))
                    .Append(rt.DOScale(new UnityEngine.Vector3(0.45f, 0.45f, 0.45f), 0.3f))
                    .Join(rt.DOAnchorPos(new UnityEngine.Vector3(1200, -15), 0.3f, false))
                    .Join(rt.DORotate(new UnityEngine.Vector3(0, 0, -20), 0.3f));

                //rt.localPosition = new UnityEngine.Vector3(1200, -15);
                //rt.localScale = new UnityEngine.Vector3(0.45f, 0.45f, 0.45f);
                //rt.localRotation = UnityEngine.Quaternion.Euler(0, 0, -20);
                break;
            default:
                DOTween.Sequence()
                    .Append(rt.DOAnchorMax(new UnityEngine.Vector2(0f, 1f), 0f, false))
                    .Append(rt.DOAnchorMin(new UnityEngine.Vector2(0f, 1f), 0f, false))
                    .Append(rt.DOPivot(new UnityEngine.Vector2(0f, 1f), 0f))
                    .Append(rt.DOScale(new UnityEngine.Vector3(0.45f, 0.45f, 0.45f), 0.3f))
                    .Join(rt.DOAnchorPos(new UnityEngine.Vector3(1220, 0), 0.3f, false))
                    .Join(rt.DORotate(new UnityEngine.Vector3(0, 0, 15), 0.3f));

                //rt.localPosition = new UnityEngine.Vector3(1220, 0);
                //rt.localScale = new UnityEngine.Vector3(0.45f, 0.45f, 0.45f);
                //rt.localRotation = UnityEngine.Quaternion.Euler(0, 0, 15);
                break;
        }
    }
}
