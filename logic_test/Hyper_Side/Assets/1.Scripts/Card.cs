using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
    public delegate void UnitSpawningHandler(GameObject unit, int wayIndex, bool isEenemy);
    public static event UnitSpawningHandler OnUnitSpawning;

    public delegate void DeckDrawingHandler(int idx);
    public static event DeckDrawingHandler OnDeckDrawing;

    public delegate void AreaHandler(bool isCardHolding);
    public static event AreaHandler OnCardHolding;

    public GameObject unit;
    private Deck deck;

    private int cardIndex;

    Vector3 m;

    public int CardIndex
    {
        set => cardIndex = value;
    }

    Material mat;
    Vector3 defaultPos;

    void Start()
    {
        deck = GetComponentInParent<Deck>();
        mat = GetComponent<MeshRenderer>().materials[0];
        defaultPos = transform.localPosition;
    }

    void OnMouseDrag()
    {
        m = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, - Camera.main.transform.position.z));
        transform.localPosition = new Vector3((m.x - 0.5f) * 12f, (m.y - 0.2f) * 7f, 0.7f);

        mat.color = new Color(1, 1, 1, 0.34f);
        OnCardHolding(true);

        //Debug.Log(m.ToString());
    }

    void OnMouseUp()
    {
        OnCardHolding(false);
        float y = m.y;
        if (y > .77f)
        {
            //Debug.Log("3");

            ThrowCard(2);
            return;
        }
        else if (y > .54f)
        {
            //Debug.Log("2");

            ThrowCard(1);
            return;
        }
        else if (y > .31f)
        {
            //Debug.Log("1");

            ThrowCard(0);
            return;
        }

        transform.DOLocalMove(defaultPos, 0.3f).SetEase(Ease.OutExpo);
        //transform.localPosition = defaultPos;
        mat.color = new Color(1, 1, 1, 1);
    }

    void ThrowCard(int idx)
    {
        GameObject card = Resources.Load<GameObject>("card");

        deck.unitQueue.Enqueue(card);
        OnUnitSpawning(unit, idx, false);
        OnDeckDrawing(cardIndex);
        Destroy(gameObject);
    }
}
