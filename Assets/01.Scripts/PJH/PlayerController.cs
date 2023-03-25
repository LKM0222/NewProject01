// 최종 수정 날짜: 2023.03.26
// 스크립트 작성자: 박준희
// 핵심 기능: 플레이어 이동, 플레이어 애니메이션, 충돌 오브젝트에 따른 상호작용


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어에 붙어 있는 RigidBody2D 컴포넌트
    private Rigidbody2D playerRigidbody2D;
    // 플레이어 이동 속력
    public float speed = 8f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // RigidBody2D 컴포넌트 추출
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 방향키 입력 인식
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        PlayerMove(xInput, yInput);


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "NPC")
        {
            GameObject.Find("Canvas").GetComponent<DialogManager>().ShowDialogue();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "NPC")
        {
            GameObject.Find("Canvas").GetComponent<DialogManager>().ShowDialogue();
        }
    }

    void PlayerMove(float xInput, float yInput)
    {
        // 방향키와 속도를 곱한 값만큼 움직여줄 변수
        float xSpeed = xInput * speed;
        float ySpeed = yInput * speed;

        // xSpeed, ySpeed로 구성된 Vector2 생성
        Vector2 newVelocity = new Vector2(xSpeed, ySpeed);

        // 플레이어의 RigidBody2D의 velocity(속도) 값에 newVelocity를 대입해 플레이어 이동
        playerRigidbody2D.velocity = newVelocity;

        // 51 ~ 70번 라인은 애니메이션을 위한 코드. 추후 삭제 예정(변수 및 start 메서드 포함)
        // 키보드의 입력이 있다면
        if (xInput != 0 || yInput != 0)
        {
            // 애니메이터의 Move 파라미터를 true로 변경하여 걷는 중임을 표시
            animator.SetBool("Move", true);
            // 각 키 입력에 맞게 ~~Move 파라미터를 변경, 또한 좌우 입력 값이 들어왔을 경우 방향에 맞춰서 플레이어 방향 전환
            if (xInput > 0) { animator.SetFloat("Right Move", xInput); transform.rotation = Quaternion.Euler(0, 0, 0); }
            else if (xInput < 0) { animator.SetFloat("Left Move", xInput); transform.rotation = Quaternion.Euler(180, 0, 180); }
            else if (yInput > 0) animator.SetFloat("Up Move", yInput);
            else if (yInput < 0) animator.SetFloat("Down Move", yInput);
        }
        // 키보드의 입력이 없다면 모든 파라미터를 디폴트 값으로 초기화
        else
        {
            animator.SetBool("Move", false);
            animator.SetFloat("Right Move", 0f);
            animator.SetFloat("Left Move", 0f);
            animator.SetFloat("Up Move", 0f);
            animator.SetFloat("Down Move", 0f);
        }
    }
}
