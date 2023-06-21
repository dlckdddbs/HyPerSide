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
        // GameManager.GameOver에 무명 함수 추가
        // 말 그대로 이름 없는 함수
        // 겨우 이거 하나 동작시키겠다고 함수 하나 만들긴 그래서
        /* 설명
         * "Apple(int a) { 코드 }"이라는 함수를 람다식을 사용해 표현하면
         * "(int a) => { 코드 }"랑 같은 표현이다
         * 단지 이름이 있고 없고의 차이일 뿐 동작은 똑같이 한다
         */
        GameManager.GameOver += (bool isVictory) =>
        {
            // 아군이 이겼는데 적이면 사망
            // 반대로 졌는데 아군이면 사망
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

    // 애니메이션에서 함수 호출
    // 프로젝트뷰에서 해당 오브젝트의 공격모션에서 Edit 누르고
    // 밑에 Event 패널 내려보면 Function칸에 Attack 있음
    // 없다면 다른 애니메이션이거나 내가 안 넣은걸 수도 있음
    public void Attack()
    {
        if (enemyUnit)
            enemyUnit.Damage(attack);

        if (enemyNexus)
            enemyNexus.Damage(attack);
    }

    // 얘도 마찬가지로 애니메이션에서 호출
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
