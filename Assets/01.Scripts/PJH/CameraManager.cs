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
            this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z - 10);
        }
    }
}
