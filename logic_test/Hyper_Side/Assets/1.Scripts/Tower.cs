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
        Unit targetUnit = null; //ã������ Ÿ���� �� ���� ����.
                                
        Collider[] colliderList = Physics.OverlapSphere(transform.position, 50.0f, LayerMask.GetMask("Unit"));
        
        for (int i = 0; i < colliderList.Length; i++)
        {
            //Unit��� ��ũ��Ʈ�� ���� ������Ʈ�� �ִ� �� Ȯ��
            Unit searchTarget = colliderList[i].GetComponent<Unit>();
            //Ÿ���� Ÿ�԰� ���ݹ����ȿ� ���� ������ �Ʊ��̶�� �н�
            if (isEnemy == searchTarget.isEnemy)
                continue;
            // �ƴ϶��
            if (searchTarget && searchTarget.isDie == false)
            {
                //StartCoroutine(BulletBustShoot2());
                //���� Ÿ���� ��.
                targetUnit = searchTarget;
                
                //ã������ ���� ���̻� ��ȯ���� �� �ʿ� ������ ����.
                break;
            }

        }
        if (targetUnit != null)
        {
            Vector3 viewPos = targetUnit.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(viewPos, Vector3.up);
            //�ش� ȸ���� ��ŭ �� ���� ȸ�� ��Ŵ.
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 20.0f);
            if (shootDelay <= 0f)
            {
                //if (Input.GetMouseButtonDown(0)) //���� ����!!!!!!!!!!!!!!!!!!!!!!!!!
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
        //�Ѿ��� �����Ѵ�
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Objects/bullet"));
        if (bullet)
        {
            //�Ѿ��� ��ġ�� ���ֿ� �̸� ����� �ѱ� ��ġ�� �ű�.
            bullet.transform.position = shootPoint.transform.position;
            //�ش� �Ѿ˿��� BulletObj�� �ִ��� Ȯ���ϰ�
            Bullet obj = bullet.GetComponent<Bullet>();
            if (obj )//&& Input.GetMouseButtonDown(0))
            {
                //�ش� �Ѿ� �߻� ���� �Լ��� �����Ѵ�.
                obj.MoveStart(this);
            }
        }
    }

    /*public IEnumerator BulletBustShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            //�Ѿ� �߻�
            BulletShoot();
            //0.2�� �� ��� ������
            yield return new WaitForSeconds(0.2f);

        }
    }*/
    public IEnumerator BulletBustShoot2()
    {
            yield return new WaitForSeconds(5.0f);
        
    }
}
