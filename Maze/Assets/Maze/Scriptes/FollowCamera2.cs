using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera2 : MonoBehaviour
{
    public Transform Target;
    public GameObject MainCamera;
    FollowCamera f;
    Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        f = MainCamera.GetComponent<FollowCamera>();
        vec = transform.position- Target.position  ;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 angles = transform.localEulerAngles;
        angles.y = f.deg;
        transform.localEulerAngles = angles;

        transform.position=Target.position+vec;
    }
}
