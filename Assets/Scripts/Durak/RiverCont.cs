using System.Collections;
using System.Collections.Generic;
using DurakServer;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class RiverCont : MonoBehaviour
{
    public List<CardBox> attackerList;
    public List<CardBox> defenceList;
    public List<CardBox> adderList;

    // Start is called before the first frame update
    public void Add(CardBox card, Role role)
    {
        card.transform.SetParent(this.transform);

        if (role == Role.Attacker) attackerList.Add(card);
        else if (role == Role.Adder) adderList.Add(card);
        else if (role == Role.Defender) defenceList.Add(card);

        card.FaceUp = true;
        card.GetCardImage();

        if (attackerList.Count == 0 && defenceList.Count == 0)
        {
            return;
        }
        else
        {
            RectTransform rt = card.GetComponent<RectTransform>();

            switch (role)
            {
                case Role.Attacker:
                    {
                        switch (attackerList.Count)
                        {
                            case 1:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(405, -220), 0.3f, false));
                                break;
                            case 2:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(585, -220), 0.3f, false));
                                break;
                            case 3:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(755, -220), 0.3f, false));
                                break;
                            case 4:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(405, -370), 0.3f, false));
                                break;
                            case 5:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(585, -370), 0.3f, false));
                                break;
                            case 6:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(755, -370), 0.3f, false));
                                break;
                        }
                    }
                    break;
                case Role.Defender:
                    {
                        switch (defenceList.Count)
                        {
                            case 1:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(455, -220), 0.3f, false));
                                break;
                            case 2:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(625, -220), 0.3f, false));
                                break;
                            case 3:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(805, -220), 0.3f, false));
                                break;
                            case 4:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(455, -370), 0.3f, false));
                                break;
                            case 5:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(625, -370), 0.3f, false));
                                break;
                            case 6:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(805, -370), 0.3f, false));
                                break;
                        }
                    }
                    break;
                case Role.Adder:
                    {
                        switch (attackerList.Count + adderList.Count)
                        {
                            case 2:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(585, -220), 0.3f, false));
                                break;
                            case 3:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(755, -220), 0.3f, false));
                                break;
                            case 4:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(405, -370), 0.3f, false));
                                break;
                            case 5:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(585, -370), 0.3f, false));
                                break;
                            case 6:
                                DOTween.Sequence()
                                    .Append(rt.DOAnchorMax(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOAnchorMin(new Vector2(0f, 1f), 0f, false))
                                    .Append(rt.DOPivot(new Vector2(0f, 1f), 0f))
                                    .Append(rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f))
                                    .Join(rt.DOAnchorPos(new Vector3(755, -370), 0.3f, false));
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
