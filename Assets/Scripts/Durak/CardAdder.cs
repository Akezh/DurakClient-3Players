using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAdder : MonoBehaviour
{
    public List<CardBox> CardAdderList;

    public void Add(CardBox card, RiverCont river)
    {
        CardAdderList.Add(card);

        RectTransform rt = card.GetComponent<RectTransform>();

        //switch (river.RiverList.Count)
        //{
        //    case 0:
        //        switch (CardAdderList.Count)
        //        {
        //            case 1:
        //                rt.localPosition = new Vector3(380, -140);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 2:
        //                rt.localPosition = new Vector3(560, -140);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 3:
        //                rt.localPosition = new Vector3(730, -140);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 4:
        //                rt.localPosition = new Vector3(380, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 5:
        //                rt.localPosition = new Vector3(560, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 6:
        //                rt.localPosition = new Vector3(730, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            default:
        //                break;
        //        }
        //        break;
        //    case 2:
        //        switch (CardAdderList.Count)
        //        {
        //            case 1:
        //                rt.localPosition = new Vector3(560, -140);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 2:
        //                rt.localPosition = new Vector3(730, -140);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 3:
        //                rt.localPosition = new Vector3(380, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 4:
        //                rt.localPosition = new Vector3(560, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 5:
        //                rt.localPosition = new Vector3(730, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            default:
        //                break;
        //        }
        //        break;
        //    case 4:
        //        switch (CardAdderList.Count)
        //        {
        //            case 1:
        //                rt.localPosition = new Vector3(730, -140);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 2:
        //                rt.localPosition = new Vector3(380, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 3:
        //                rt.localPosition = new Vector3(560, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 4:
        //                rt.localPosition = new Vector3(730, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            default:
        //                break;
        //        }
        //        break;
        //    case 6:
        //        switch (CardAdderList.Count)
        //        {
        //            case 1:
        //                rt.localPosition = new Vector3(380, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 2:
        //                rt.localPosition = new Vector3(560, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 3:
        //                rt.localPosition = new Vector3(730, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            default:
        //                break;
        //        }
        //        break;
        //    case 8:
        //        switch (CardAdderList.Count)
        //        {
        //            case 1:
        //                rt.localPosition = new Vector3(560, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            case 2:
        //                rt.localPosition = new Vector3(730, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            default:
        //                break;
        //        }
        //        break;
        //    case 10:
        //        switch (CardAdderList.Count)
        //        {
        //            case 1:
        //                rt.localPosition = new Vector3(730, -285);
        //                rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        //                break;
        //            default:
        //                break;
        //        }
        //        break;
        //    default:
        //        break;
        //}


    }
    public void Remove(CardBox card)
    {
        CardAdderList.Remove(card);
    }
    public void Clear()
    {
        CardAdderList.Clear();
    }
    public int Count()
    {
        return CardAdderList.Count;
    }
}