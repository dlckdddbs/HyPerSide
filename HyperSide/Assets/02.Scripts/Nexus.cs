using UnityEngine;

public class Nexus : MonoBehaviour
{
    public delegate void GameEndEventHandler(bool isWin);
    public static GameEndEventHandler GameEnd;

    public bool isEnemy;
    public int hp = 100;

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GameEnd(isEnemy);
        }
    }
}
