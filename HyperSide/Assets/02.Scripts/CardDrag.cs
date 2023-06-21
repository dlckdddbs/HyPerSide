using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

//IDragHandler : 유니티에서 제공하는 UI 드래그 핸들러 인터페이스
//IEndDragHandler : 유니티에서 제공하는 UI 놓기 핸들러 인터페이스
public class CardDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public delegate void CardDownEventHandler(int wayIndex, GameObject unit, bool isEnemy);
    public static event CardDownEventHandler CardDownEvent;

    public GameObject unit;
    public GameObject area;

    public Image delayPanel;
    public TextMeshProUGUI delayTxt;

    static GameManager manager;

    float spawnDelayTime;
    float screenHeight;

    Vector3 m_StartPos;

    void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnDelayTime = unit.GetComponent<Unit>().spawnDelay;
    }

    void Start()
    {
        screenHeight = Screen.height;
        m_StartPos = transform.position;

        delayPanel.fillAmount = 0f;

        area.SetActive(false);
    }

    void Update()
    {
        delayPanel.fillAmount -= Time.deltaTime / spawnDelayTime;
        if (delayPanel.fillAmount > 0f)
        {
            delayTxt.text = $"{Mathf.Ceil(delayPanel.fillAmount * spawnDelayTime)}";
        }
        else
        {
            delayTxt.text = "";
        }
    }

    //UI 드래그하는 동안 호출
    public void OnDrag(PointerEventData eventData)
    {
        // fillAmount가 0보다 크거나 게임이 끝나면 드래그 X
        if (delayPanel.fillAmount > 0 || !manager.isPlaying)
            return;

        transform.position = eventData.position;
        area.SetActive(true);
    }

    //UI 드래그 끝났을 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        // fillAmount가 0보다 크거나 게임이 끝나면 드래그 X
        if (!(delayPanel.fillAmount > 0 || !manager.isPlaying))
        {
            float y = eventData.position.y;
            //Debug.Log($"{y} {screenHeight}");
            if (y > screenHeight * 0.75f)
            {
                CardDown(2);
            }
            else if (y > screenHeight * 0.5f)
            {
                CardDown(1);
            }
            else if (y > screenHeight * 0.25f)
            {
                CardDown(0);
            }
        }

        //제자리에 돌아가기
        transform.DOMove(m_StartPos, 0.3f).SetEase(Ease.OutExpo);
        area.SetActive(false);
    }

    void CardDown(int idx)
    {
        delayPanel.fillAmount = 1f;
        CardDownEvent(idx, unit, false);
    }
}
