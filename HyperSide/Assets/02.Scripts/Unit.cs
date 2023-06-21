using System.Collections;
using UnityEngine;

public enum State
{
    WALK,
    ATTACK,
    DIE
}

public class Unit : MonoBehaviour
{
    public bool isEnemy;

    public int hp;
    public int attack;
    [Range(0f, 10f)]
    public float speed = 1f;

    public float distance;

    public float spawnDelay;

    public Transform pivot;

    bool isDie;
    State state;

    Animator animator;
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashDie = Animator.StringToHash("Die");

    Unit enemyUnit;
    Nexus enemyNexus;

    void Start()
    {
        // GameManager.GameOver�� ���� �Լ� �߰�
        // �� �״�� �̸� ���� �Լ�
        // �ܿ� �̰� �ϳ� ���۽�Ű�ڴٰ� �Լ� �ϳ� ����� �׷���
        /* ����
         * "Apple(int a) { �ڵ� }"�̶�� �Լ��� ���ٽ��� ����� ǥ���ϸ�
         * "(int a) => { �ڵ� }"�� ���� ǥ���̴�
         * ���� �̸��� �ְ� ������ ������ �� ������ �Ȱ��� �Ѵ�
         */
        GameManager.GameOver += (bool isVictory) =>
        {
            // �Ʊ��� �̰�µ� ���̸� ���
            // �ݴ�� ���µ� �Ʊ��̸� ���
            if (isVictory == isEnemy)
                state = State.DIE;
        };

        animator = GetComponent<Animator>();
        state = State.WALK;
        StartCoroutine(CheckingStatement());
    }

    void Update()
    {
        if (state == State.DIE)
        {
            //Debug.Log("dead");
            return;
        }

        if (Physics.Raycast(pivot.position, new Vector3(isEnemy ? -1f : 1f, 0f), out RaycastHit hit, distance))
        {
            if (hit.transform.CompareTag("UNIT"))
            {
                enemyUnit = hit.transform.GetComponent<Unit>();
                if (enemyUnit.isEnemy != isEnemy)
                {
                    state = State.ATTACK;
                    return;
                }
            }

            if (hit.transform.CompareTag("NEXUS"))
            {
                enemyNexus = hit.transform.GetComponent<Nexus>();
                if (enemyNexus.isEnemy != isEnemy)
                {
                    state = State.ATTACK;
                    return;
                }
            }
        }

        //Debug.DrawRay(pivot.position, new Vector3(isEnemy ? -1f : 1f, 0f) * distance, Color.red);

        enemyUnit = null;
        enemyNexus = null;

        state = State.WALK;
        transform.position += new Vector3((isEnemy ? -1f : 1f) * speed * Time.deltaTime, 0f);
    }

    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            state = State.DIE;
        }
    }

    // �ִϸ��̼ǿ��� �Լ� ȣ��
    // ������Ʈ�信�� �ش� ������Ʈ�� ���ݸ�ǿ��� Edit ������
    // �ؿ� Event �г� �������� Functionĭ�� Attack ����
    // ���ٸ� �ٸ� �ִϸ��̼��̰ų� ���� �� ������ ���� ����
    public void Attack()
    {
        if (enemyUnit)
            enemyUnit.Damage(attack);

        if (enemyNexus)
            enemyNexus.Damage(attack);
    }

    // �굵 ���������� �ִϸ��̼ǿ��� ȣ��
    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator CheckingStatement()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.WALK:
                    animator.SetBool(hashAttack, false);
                    break;

                case State.ATTACK:
                    animator.SetBool(hashAttack, true);
                    break;

                case State.DIE:
                    isDie = true;
                    speed = 0f;
                    GetComponent<BoxCollider>().enabled = false;
                    animator.SetTrigger(hashDie);
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
