// 최종 수정 날짜: 2023.04.03
// 스크립트 작성자: 박준희
// 핵심 기능: 플레이어 이동, 충돌 오브젝트에 따른 상호작용, 대화시 플레이어 이동 제한
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어에 붙어 있는 RigidBody2D 컴포넌트
    private Rigidbody2D playerRigidbody2D;
    // 플레이어 이동 속력
    public float speed = 8f;
    // 플레이어 이동 제어 변수
    public bool moveFlag = true;
    // 상호작용 중인이 파악하기 위한 변수
    private RaycastHit2D hit;
    private Vector2 testVector = new Vector2(1f, 1f);
    // Start is called before the first frame update
    void Start()
    {
        // RigidBody2D 컴포넌트 추출
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 방향키 입력 인식
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        float xSpeed = xInput * speed;
        float ySpeed = yInput * speed;

        Vector2 newVelocity = new Vector2(xSpeed, ySpeed);
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, newVelocity);
        Debug.DrawRay(this.transform.position, newVelocity, Color.red);

        if (hit.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameObject.Find("Canvas").GetComponent<DialogManager>().ShowDialogue();
            }
        }
        if (moveFlag)
        {
            Debug.Log("GGGGGG");
            playerRigidbody2D.velocity = newVelocity;
        }
    }
}