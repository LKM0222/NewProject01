// 최종 수정 날짜: 2023.04.05
// 스크립트 작성자: 박준희
// 핵심 기능: 플레이어 이동, 대화시 플레이어 이동 제한, 레이캐스트
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
    // RayCast를 쏘기 위한 플레이어의 바라보는 방향 벡터
    private Vector2 playerDir;
    private SpriteRenderer player;
    private int layerMask;

    void Start()
    {
        // RigidBody2D 컴포넌트 추출
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        player = GetComponent<SpriteRenderer>();
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));  // Everything에서 Player 레이어만 제외하고 충돌 체크함
    }

    // Update is called once per frame
    void Update()
    {
        // 방향키 입력 인식
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        // 좌우 속도 계산
        float xSpeed = xInput * speed;
        float ySpeed = yInput * speed;

        // 속도를 토대로 이동할 Vector2 변수 생성
        Vector2 newVelocity = new Vector2(xSpeed, ySpeed);

        // moveFlag 변수가 true일때만 rigidbody 컴포넌트의 velocity에 접근하여 Player 위치 이동
        if (moveFlag)
        {
            playerRigidbody2D.velocity = newVelocity;
        }
        else // false일때는 velocity를 0으로 만들어줌.
        {
            playerRigidbody2D.velocity = Vector2.zero;
        }

        // Player가 바라보는 방향으로 RayCast를 발사함
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, playerDir, 2f, layerMask);
        Debug.DrawRay(this.transform.position, playerDir * 2f, Color.red);

        // PlayerDir를 계산
        if (xInput != 0 && yInput == 0)
        {
            if (xInput == 1) playerDir = Vector2.right; // 오른쪽
            else if (xInput == -1) playerDir = Vector2.left; // 왼쪽
        }
        else if (xInput == 0 && yInput != 0)
        {
            if (yInput == 1) playerDir = Vector2.up; // 위쪽
            else if (yInput == -1) playerDir = Vector2.down; // 아래쪽
        }
        else if (xInput != 0 && yInput != 0)
        {   // 대각선 방향
            if (xInput == 1 && yInput == 1) playerDir = Vector2.up + Vector2.right;
            else if (xInput == 1 && yInput == -1) playerDir = Vector2.down + Vector2.right;
            else if (xInput == -1 && yInput == -1) playerDir = Vector2.down + Vector2.left;
            else if (xInput == -1 && yInput == 1) playerDir = Vector2.up + Vector2.left;
        }


        // Ray가 collider를 지닌 오브젝트와 충돌했을 때
        if (hit.collider != null)
        {
            // 만약 그 오브젝트가 NPC라면
            if (hit.collider.tag == "NPC")
            {
                // C키를 눌렀을 때 대화가 시작되도록
                if (Input.GetKeyDown(KeyCode.C))
                {
                    GameObject.Find("Canvas").GetComponent<DialogManager>().ShowDialogue();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // EventSpot에 들어가면 자동으로 대사가 출력
        if (other.tag == "EventSpot") GameObject.Find("Canvas").GetComponent<DialogManager>().ShowDialogue();

        // 충돌을 감지한 오브젝트보다 뒤쪽에 그려지게 sortingOrder을 -1로 변환.
        if (other.tag == "Object Box") player.sortingOrder = -1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 충돌을 감지한 collider에서 벗어나면 sortingOrder을 0으로 변환.
        if (other.tag == "Object Box") player.sortingOrder = 0;

    }
}