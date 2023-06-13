using System.Collections;
using UnityEngine;

public enum State
{
    WALK,
    ATTACK,
    DIE
}

public enum LINETYPE
{
    LINE1,
    LINE2,
    LINE3
}


public class Unit : MonoBehaviour
{
    public bool isEnemy;
    public bool isDie;
    private float Delay = 0.1f;
    private float temp1;
    //public bool[] enemy = new bool[3];

    [Header("status")]
    public int hp = 10;
    public int damage = 2;
    public float speed = 3f;

    [Range(0f, 30f)]
    public float distance = 3.0f;

    public float firstDelay;
    public float lastDelay;
    private float delay = 0f;
    private float delTemp;

    public Transform pivot;

    public State state;
    public LINETYPE linetype;

    private Animator anime;
    private readonly int hashWalk = Animator.StringToHash("isWalking");
    private readonly int hashAttack = Animator.StringToHash("isAttack");

    void Start()
    {
        temp1 = delay;
        anime = GetComponent<Animator>();
        state = State.WALK;
        StartCoroutine(CheckingStatement());
    }

    void Update()
    {
        if (state == State.DIE)
            return;

        Unit EnemyCheck = null; //찾으려는 타겟의 빈 값을 생성.

        Collider[] colliderList = Physics.OverlapSphere(transform.position, 20.0f, LayerMask.GetMask("Unit"));

        for (int i = 0; i < colliderList.Length; i++)
        {
            //Unit라는 스크립트를 가진 오브젝트가 있는 지 확인
            Unit searchTarget = colliderList[i].GetComponent<Unit>();
            //타워에 타입과 공격범위안에 들어온 유닛이 아군이라면 패스
            // 아니라면
            if (isEnemy != searchTarget.isEnemy && searchTarget && searchTarget.isDie == false)
            {
                if (linetype == searchTarget.linetype)
                {
                    EnemyCheck = searchTarget;

                    //찾았으니 이제 더이상 순환문을 돌 필요 없으니 나감.
                    break;
                }

            }
            //else 다시 각도 초기화 rotaion = quen.Euler(0,90,0);

        }

        if (EnemyCheck != null)
        {
            Vector3 viewPos = EnemyCheck.transform.position - transform.position;

            Quaternion rot = Quaternion.LookRotation(viewPos, Vector3.up);
            //해당 회전값 만큼 내 몸을 회전 시킴.
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 20.0f);

            //if (Delay <= 0f)
            //{
            //    //if (Input.GetMouseButtonDown(0)) //여기 에러!!!!!!!!!!!!!!!!!!!!!!!!!
            //    state = State.ATTACK;
            //    EnemyCheck.Damage(damage);
            //    Delay = temp1;
            //}
            //else
            //{
            //    Delay -= Time.deltaTime;
            //}

        }
        else if(isEnemy==false)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (isEnemy == true)
            transform.rotation = Quaternion.Euler(0, -90, 0);

        state = State.WALK;
        if (Physics.Raycast(transform.position, new Vector3(1f, 0f, 0f), out RaycastHit hit, distance))
        {
            if (hit.transform.CompareTag("UNIT"))
            {
                Unit unit = hit.transform.GetComponent<Unit>();
                if (unit.isEnemy != isEnemy)
                {
                    state = State.ATTACK;
                    if (firstDelay + lastDelay > delay)
                    {
                        delay += Time.deltaTime;
                    }
                    else
                    {
                        delay = 0f;
                        StartCoroutine(Attack(unit));
                    }
                    //코루틴
                    goto here;
                }
            }
        }

    here:;
        Debug.DrawRay(pivot.position, new Vector3(distance * (isEnemy ? -1 : 1), 0f, 0f), Color.red);

        if (state == State.WALK)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            isDie = true;
            state = State.DIE;
            Destroy(gameObject, 0.5f);
        }
    }

    IEnumerator Attack(Unit unit)
    {
        yield return new WaitForSeconds(firstDelay);
        unit.Damage(damage);
    }

    IEnumerator CheckingStatement()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.WALK:
                    anime.SetBool(hashWalk, true);
                    anime.SetBool(hashAttack, false);
                    break;

                case State.ATTACK:
                    anime.SetBool(hashWalk, false);
                    anime.SetBool(hashAttack, true);
                    break;

                case State.DIE:
                    isDie = true;
                    anime.SetBool(hashWalk, false);
                    anime.SetBool(hashAttack, false);
                    break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}