using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameEndEventHandler(bool isVictory);
    public static GameEndEventHandler GameOver;

    [HideInInspector]
    public bool isPlaying = true;

    public Nexus allyNexus;
    public Nexus enemyNexus;

    public TextMeshProUGUI ally_hp_txt;
    public TextMeshProUGUI enemy_hp_txt;
    public Image ally_hp;
    public Image enemy_hp;

    private float allyFullHP;
    private float enemyFullHP;

    static AudioSource background;
    static GameObject endPanel;

    static GameObject victoryImg;
    static GameObject loseImg;

    void Start()
    {
        allyFullHP = allyNexus.hp;
        enemyFullHP = enemyNexus.hp;

        background = GetComponent<AudioSource>();

        endPanel = GameObject.Find("EndPanel");

        victoryImg = GameObject.Find("v");
        loseImg = GameObject.Find("l");

        endPanel.SetActive(false);
        victoryImg.SetActive(false);
        loseImg.SetActive(false);

        Nexus.GameEnd += GameEnd;

        background.Play();
    }

    void Update()
    {
        ally_hp_txt.text = $"{(allyNexus.hp > 0 ? allyNexus.hp : 0)}";
        enemy_hp_txt.text = $"{(enemyNexus.hp > 0 ? enemyNexus.hp : 0)}";

        ally_hp.fillAmount = (allyNexus.hp > 0 ? allyNexus.hp : 0) / allyFullHP;
        enemy_hp.fillAmount = (enemyNexus.hp > 0 ? enemyNexus.hp : 0) / enemyFullHP;
    }

    void GameEnd(bool isVictory)
    {
        isPlaying = false;

        if (isVictory)
        {

            victoryImg.SetActive(true);
        }
        else
        {
            loseImg.SetActive(true);
        }
        endPanel.SetActive(true);
        GameOver(isVictory);

        background.Stop();
    }
}
