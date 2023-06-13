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
        //이 총알의 주인을 등록
        mastertower = tower;

        //총알이 나가려는 방향(회전값)을 주인이 바라보는 방향으로 설정.
        transform.rotation = mastertower.transform.rotation;

        //이제 무빙을 시작. 이값이 true가 되면 update함수에서 이동을 시작.
        isMove = true;
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (isMove)
        {
            //해당 총알의 방향값으로 이동한다. 속도만큼
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            //정해진 라이프타임이 0이 되면
            //if (lifeTime <= 0)      //여기도 애매한 에러
            //{
            //    //해당 총알은 사라진다.
            //    Destroy(gameObject);    
            //    return;
            //}

            ////정해진 라이프타임이 매 프레임 1씩 감소한다.
            //lifeTime--;

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);

        //총알과 충돌한 오브젝트의 레이어를 구해서 Unit인지 체크
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            //Destroy(gameObject);
            //충돌한 객체에 unit 스크립트가 있으면? 
            Unit unit = collision.gameObject.GetComponent<Unit>();
            if (unit && unit.isEnemy != mastertower.isEnemy)
            {
                //Unit과 충돌했으면 사라지는 코루틴을 시작.
                //StartCoroutine(Disapear());
                //해당 유닛에 대미지를 입힘.
                unit.Damage(Random.Range(3, 6));
            }
        }

        Destroy(gameObject);
    }
    IEnumerator Disapear()
    {
        //우선 이동을 멈추고
        isMove = false;
        yield return new WaitForSeconds(1.0f);

        //그 후 탄환을 삭제
        Destroy(gameObject);
    }
}
