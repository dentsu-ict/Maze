using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float deg=0;
    Vector3 max_dir,now_dir,rotdir;
    float scale = 1.0f, nowscale = 1.0f;
    float x;
    float height = 1.0f;
    Vector3 t_pos;
    // Start is called before the first frame update
    void Start()
    {
        t_pos = target.position;
        t_pos.y += height;
        max_dir = transform.position - t_pos ;
        now_dir = max_dir;
    }

    private void Update()
    {
        x = 0;
        if (Input.GetKey(KeyCode.Z))
        {
            x = -1.0f;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            x = 1.0f;
        }
        deg += 80 *x* Time.deltaTime;
        if (deg < 0) deg += 360;
        if (deg > 360) deg -= 360;
        Vector3 dir = Quaternion.Euler(0, deg, 0) * max_dir;
        var ray = new Ray(target.position, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                t_pos = target.position;
                t_pos.y += height;
                Vector3 target_dir = (hit.point -t_pos)*0.85f;
                Vector3 rot_max_dir = Quaternion.Euler(0, deg, 0) * max_dir;
                scale = target_dir.magnitude / rot_max_dir.magnitude;
                if (scale >= 1.0f) scale = 1.0f;
                if (nowscale > scale)
                {
                    now_dir = Vector3.Lerp(now_dir, max_dir * scale, 6f * Time.deltaTime);
                }
                else
                {
                    now_dir = Vector3.Lerp(now_dir, max_dir * scale, 3f * Time.deltaTime);
                }
                nowscale = now_dir.magnitude / rot_max_dir.magnitude;
            }
        }
        else
        {
            now_dir = Vector3.Lerp(now_dir, max_dir,3f * Time.deltaTime);
        }
        rotdir = Quaternion.Euler(0, deg, 0) * now_dir;
        Vector3 angles = transform.localEulerAngles;
        angles.y = deg;
        transform.localEulerAngles = angles;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        t_pos = target.position;
        t_pos.y += height;
        transform.position = t_pos + rotdir;
        Debug.DrawLine(t_pos, transform.position, Color.red, 0f, false);
    }
}
