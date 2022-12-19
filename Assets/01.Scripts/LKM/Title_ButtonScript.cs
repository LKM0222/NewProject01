using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_ButtonScript : MonoBehaviour
{
    public void OnStartButtonClick(){
        SceneManager.LoadScene("GameScene");
    }
}
