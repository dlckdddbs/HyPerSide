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
        // CardDrag�� CardDownEvent�� SpawnHelper �Լ� �߰�
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
        // System ���̺귯�� �ȿ� �ִ� ���� Ŭ����
        // ����Ƽ���� �����ϴ� Random Ŭ���� �ᵵ ��
        System.Random rand = new();

        // ���� ���� ������ �ݺ�
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
    ///                         ������ �ʵ� ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="wayIndex"> ������ ������ ��ȣ, ī�޶�� ����� �� ���� 0, 1, 2���̴�.
    /// </param>
    /// <param name="unit">     ������ ���� ������Ʈ
    /// </param>
    /// <param name="isEnemy">  ������ ������ ������ �Ʊ����� ���� true�� ��, false�� �Ʊ��̴�.
    /// </param>
    void SpawnHelper(int wayIndex, GameObject unit, bool isEnemy)
    {
        GameObject temp = Instantiate(
            // ������ ������Ʈ
            unit,

            // ���̳� �Ʊ��̳Ŀ� ���� �ٸ� ���� ��ġ
            (isEnemy ? enemyPointsVector3 : allyPointsVector3)[wayIndex],

            // ȸ������ ģ���� ���Ϸ����� ���ʹϿ����� ����
            // �Ʊ��̸� ������, �����̸� ������ �ٶ󺸰�
            Quaternion.Euler(0f, isEnemy ? -90f : 90f, 0f));
    }
}
