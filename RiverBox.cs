using System.Collections.Generic;
using UnityEngine;

// Наша доска, куда кидаются карты
public class RiverBox : MonoBehaviour
{
    public List<CardBox> RiverList;

    // Start is called before the first frame update
    public void Add(CardBox card)
    {
        card.transform.SetParent(this.transform);
        RiverList.Add(card);
        card.FaceUp = true;
        card.GetCardImage();

        if (RiverList.Count == 0)
        {
            return;
        }
        else
        {
            RectTransform rt = card.GetComponent<RectTransform>();

            switch (RiverList.Count)
            {
                case 1:
                    rt.localPosition = new Vector3(525, -170);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 2:
                    rt.localPosition = new Vector3(430, -140);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 3:
                    rt.localPosition = new Vector3(560, -140);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 4:
                    rt.localPosition = new Vector3(610, -140);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 5:
                    rt.localPosition = new Vector3(730, -140);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 6:
                    rt.localPosition = new Vector3(780, -140);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 7:
                    rt.localPosition = new Vector3(380, -285);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 8:
                    rt.localPosition = new Vector3(430, -285);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 9:
                    rt.localPosition = new Vector3(560, -285);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 10:
                    rt.localPosition = new Vector3(610, -285);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 11:
                    rt.localPosition = new Vector3(730, -285);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                case 12:
                    rt.localPosition = new Vector3(780, -285);
                    rt.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    break;
                default:
                    break;
            }
        }
    }
    public void Remove(CardBox card)
    {
        RiverList.Remove(card);
    }
    public void Clear()
    {
        RiverList.Clear();
    }
    public int Count()
    {
        return RiverList.Count;
    }
}
