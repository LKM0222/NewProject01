// 최종 수정 날짜: 2023.04.02
// 스크립트 작성자: 박준희
// 핵심 기능: 대화 시스템, 음영처리, 플레이어 이동 변수 제한
// 참고 사이트: https://make-my-jazz.tistory.com/27

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable] //직접 만든 class에 접근할 수 있도록 해줌. 
public class Dialogue
{
    [TextArea]//한줄 말고 여러 줄 쓸 수 있게 해줌
    public string dialogue; // 대화 텍스트
    public Sprite LeftCharacter; // 왼쪽 캐릭터 이미지
    public Sprite RightCharacter; // 오른쪽 캐릭터 이미지
    public string LeftCharacterName; // 왼쪽 캐릭터 이름
    public string RightCharacterName; // 오른쪽 캐릭터 이름


}
public class DialogManager : MonoBehaviour
{
    //SerializeField : inspector창에서 직접 접근할 수 있도록 하는 변수
    [SerializeField] private SpriteRenderer CharacterImage1; // 캐릭터 이미지가 들어갈 "오브젝트를 설정"하기 위한 변수
    [SerializeField] private SpriteRenderer CharacterImage2; // 캐릭터 이미지가 들어갈 "오브젝트를 설정"하기 위한 변수
    [SerializeField] private TextMeshProUGUI Dialogue; // 텍스트가 들어갈 "오브젝트를 설정"하기 위한 변수
    [SerializeField] private TextMeshProUGUI CharacterName1; // 캐릭터 이름이 들어갈 "오브젝트를 설정"하기 위한 변수
    [SerializeField] private TextMeshProUGUI CharacterName2; // 캐릭터 이름이 들어갈 "오브젝트를 설정"하기 위한 변수


    private bool isDialogue = false; // 대화가 진행중인지 알려줄 변수
    private int count = 0; // 대사가 얼마나 진행됐는지 알려줄 변수

    // Dialogue 클래스의 객체를 SerializeField로 선언함으로써, 인스펙터 창에서 Dialogue의 필드들을 설정 할 수 있게 만듦. 
    [SerializeField] private Dialogue[] dialogue;


    public void ShowDialogue()
    {
        VisibleDialog(true); // Dialog 패널을 활성화 시킴.
        count = 0;
        NextDialogue(); //ShowDialogue 메서드 호출과 동시에, 대화를 시작시킴.
    }

    private void VisibleDialog(bool flag)
    {
        GameObject.Find("Canvas").transform.Find("DialogSystem").gameObject.SetActive(flag); //Canvas 아래 DialogSystem을 찾아서 활성화 시켜줌
        isDialogue = flag;
        // PlayerController의 moveFlag 변수를 현재 대화가 진행중인 flag의 반대값으로 설정(대화중이면 => 움직이지 못함, 대화중이지 않으면 => 움직일 수 있음)
        GameObject.Find("Player").GetComponent<PlayerController>().moveFlag = !flag;
    }

    private void NextDialogue()
    {
        // dialogue 필드에 접근하여, 오브젝트들의 내용을 바꾸어준다(text, sprite etc...)
        // count를 대화 설정이 끝나면 증가시킴으로써 다음 NextDialogue가 호출 돼었을 때는 다음 대사를 진행함.
        Dialogue.text = dialogue[count].dialogue;
        CharacterImage1.sprite = dialogue[count].LeftCharacter;
        CharacterImage2.sprite = dialogue[count].RightCharacter;
        CharacterName1.text = dialogue[count].LeftCharacterName;
        CharacterName2.text = dialogue[count].RightCharacterName;

        // 대사를 읽어들인 다음, 캐릭터 이름 아래 표시되는 BottomBar 오브젝트를 제어하기 위해서 캐릭터의 이름이 없다면(=> 대화 할 차례가 아니라면) 비활성화 시켜줌.
        // 대사를 하고 있지 않은 캐릭터는 color 값을 조정하여 음영처리를 구현함.
        if (dialogue[count].LeftCharacterName == "")
        {
            GameObject.Find("Left_Character_Name").transform.Find("BottomBar").gameObject.SetActive(false);
            CharacterImage1.color = new Color32(150, 150, 150, 255);
        }
        else
        {
            GameObject.Find("Left_Character_Name").transform.Find("BottomBar").gameObject.SetActive(true);
            CharacterImage1.color = new Color32(255, 255, 255, 255);
        }

        if (dialogue[count].RightCharacterName == "")
        {
            GameObject.Find("Right_Character_Name").transform.Find("BottomBar").gameObject.SetActive(false);
            CharacterImage2.color = new Color32(150, 150, 150, 255);
        }
        else
        {
            GameObject.Find("Right_Character_Name").transform.Find("BottomBar").gameObject.SetActive(true);
            CharacterImage2.color = new Color32(255, 255, 255, 255);

        }
        count++;

    }

    // Update is called once per frame
    void Update()
    {
        // Z키를 누를 때마다 대사가 진행. isDialogue는 다른 스크립트에서 ShowDialogue 메서드에 접근하면서 값이 설정됨.
        if (isDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // 현재 진행 중인 대화 index가 대화의 마지막 index를 넘지 않았다면 대화를 계속 진행
                if (count < dialogue.Length) NextDialogue();
                else VisibleDialog(false); // 대화가 끝났다면 Dialog 패널을 모두 숨김

            }
        }

    }
}