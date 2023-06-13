using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : Unit
{
    public GameObject shootPoint;

    private float shootDelay = 0.7f;
    private float temp;

    // Start is called before the first frame update
    void Start()
    {
        temp = shootDelay;
    }

    // Update is called once per frame
    void Update()
    {
        Unit targetUnit = null; //찾으려는 타겟의 빈 값을 생성.
                                
        Collider[] colliderList = Physics.OverlapSphere(transform.position, 50.0f, LayerMask.GetMask("Unit"));
        
        for (int i = 0; i < colliderList.Length; i++)
        {
            //Unit라는 스크립트를 가진 오브젝트가 있는 지 확인
            Unit searchTarget = colliderList[i].GetComponent<Unit>();
            //타워에 타입과 공격범위안에 들어온 유닛이 아군이라면 패스
            if (isEnemy == searchTarget.isEnemy)
                continue;
            // 아니라면
            if (searchTarget && searchTarget.isDie == false)
            {
                //StartCoroutine(BulletBustShoot2());
                //적의 타겟이 됨.
                targetUnit = searchTarget;
                
                //찾았으니 이제 더이상 순환문을 돌 필요 없으니 나감.
                break;
            }

        }
        if (targetUnit != null)
        {
            Vector3 viewPos = targetUnit.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(viewPos, Vector3.up);
            //해당 회전값 만큼 내 몸을 회전 시킴.
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 20.0f);
            if (shootDelay <= 0f)
            {
                //if (Input.GetMouseButtonDown(0)) //여기 에러!!!!!!!!!!!!!!!!!!!!!!!!!
                BulletShoot();
                shootDelay = temp;
            }
            else
            {
                shootDelay -= Time.deltaTime;
            }
        }

    }
    public void BulletShoot()
    {
        //총알을 생성한다
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Objects/bullet"));
        if (bullet)
        {
            //총알의 위치를 유닛에 미리 등록한 총구 위치로 옮김.
            bullet.transform.position = shootPoint.transform.position;
            //해당 총알에서 BulletObj이 있는지 확인하고
            Bullet obj = bullet.GetComponent<Bullet>();
            if (obj )//&& Input.GetMouseButtonDown(0))
            {
                //해당 총알 발사 시작 함수를 실행한다.
                obj.MoveStart(this);
            }
        }
    }

    /*public IEnumerator BulletBustShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            //총알 발사
            BulletShoot();
            //0.2초 간 잠시 딜레이
            yield return new WaitForSeconds(0.2f);

        }
    }*/
    public IEnumerator BulletBustShoot2()
    {
            yield return new WaitForSeconds(5.0f);
        
    }
}
