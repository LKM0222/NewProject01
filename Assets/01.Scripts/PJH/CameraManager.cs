// 최종 수정 날짜: 2023.03.26
// 스크립트 작성자: 박준희
// 핵심 기능: 플레이어 이동에 따른 카메라 이동

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target.gameObject != null)
        {
            this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1, this.transform.position.z);
        }
    }
}
