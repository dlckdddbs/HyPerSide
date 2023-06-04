using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public Tower mastertower = null;
    public bool isMove = true;
    public int lifeTime = 100;
    // Start is called before the first frame update
    
    public void MoveStart(Tower tower)
    {
        //�� �Ѿ��� ������ ���
        mastertower = tower;

        //�Ѿ��� �������� ����(ȸ����)�� ������ �ٶ󺸴� �������� ����.
        transform.rotation = mastertower.transform.rotation;

        //���� ������ ����. �̰��� true�� �Ǹ� update�Լ����� �̵��� ����.
        isMove = true;
    }
    
    void Update()
    {
        if (isMove)
        {
            //�ش� �Ѿ��� ���Ⱚ���� �̵��Ѵ�. �ӵ���ŭ
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            //������ ������Ÿ���� 0�� �Ǹ�
            if (lifeTime <= 0)      //���⵵ �ָ��� ����
            {
                //�ش� �Ѿ��� �������.
                Destroy(gameObject);    
                return;
            }

            //������ ������Ÿ���� �� ������ 1�� �����Ѵ�.
            lifeTime--;

        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�浹");

        //�Ѿ˰� �浹�� ������Ʈ�� ���̾ ���ؼ� Unit���� üũ
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            //Destroy(gameObject);
            //�浹�� ��ü�� unit ��ũ��Ʈ�� ������? 
            Unit unit = collision.gameObject.GetComponent<Unit>();
            if (unit && unit.isAIType!=mastertower.isAIType)
            {
                //Unit�� �浹������ ������� �ڷ�ƾ�� ����.
                StartCoroutine(Disapear());
                //�ش� ���ֿ� ������� ����.
                unit.Damage(Random.Range(3, 6));
            }
        }
    }
    IEnumerator Disapear()
    {
        //�켱 �̵��� ���߰�
        isMove = false;
        yield return new WaitForSeconds(1.0f);

        //�� �� źȯ�� ����
        Destroy(gameObject);
    }
}
