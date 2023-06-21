using System.Collections;
using UnityEngine;

public class SpawnCtrl : MonoBehaviour
{
    public GameManager manager;

    public Transform[] allyPoints;
    public Transform[] enemyPoints;

    public Vector3[] allyPointsVector3 = new Vector3[3];
    public Vector3[] enemyPointsVector3 = new Vector3[3];

    public GameObject[] enemies;

    public Nexus enemyNexus;

    void Start()
    {
        // CardDrag의 CardDownEvent에 SpawnHelper 함수 추가
        CardDrag.CardDownEvent += SpawnHelper;

        for(int i = 0; i < 3; i++)
        {
            enemyPointsVector3[i] = enemyPoints[i].position;
            allyPointsVector3[i] = allyPoints[i].position;
        }

        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        // System 라이브러리 안에 있는 랜덤 클래스
        // 유니티에서 제공하는 Random 클래스 써도 됨
        System.Random rand = new();

        // 게임 끝날 때까지 반복
        while (manager.isPlaying)
        {
            if (rand.Next(0, enemyNexus.hp / 100 + 10) == 0)
            {
                SpawnHelper(
                    rand.Next(0, 3),
                    enemies[rand.Next(0, enemies.Length)],
                    true);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    ///                         유닛을 필드 위에 생성하는 함수
    /// </summary>
    /// <param name="wayIndex"> 생성할 라인의 번호, 카메라와 가까운 쪽 부터 0, 1, 2번이다.
    /// </param>
    /// <param name="unit">     생성할 유닛 오브젝트
    /// </param>
    /// <param name="isEnemy">  생성할 유닛이 적인지 아군인지 구분 true가 적, false가 아군이다.
    /// </param>
    void SpawnHelper(int wayIndex, GameObject unit, bool isEnemy)
    {
        GameObject temp = Instantiate(
            // 생성할 오브젝트
            unit,

            // 적이냐 아군이냐에 따라 다른 생성 위치
            (isEnemy ? enemyPointsVector3 : allyPointsVector3)[wayIndex],

            // 회전각을 친숙한 오일러에서 쿼터니온으로 변경
            // 아군이면 오른쪽, 적군이면 왼쪽을 바라보게
            Quaternion.Euler(0f, isEnemy ? -90f : 90f, 0f));
    }
}
