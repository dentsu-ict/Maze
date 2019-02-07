using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    FollowCamera camerascript;
    public GameObject camera,Button;
    CharacterController controller;
    float y=0;
    float x, z;
    int score = 0;
    Text Score,Clear;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camerascript = camera.GetComponent<FollowCamera>();
        Score = GameObject.Find("Score").GetComponent<Text>();
        Clear = GameObject.Find("Clear").GetComponent<Text>();
        Button = GameObject.Find("Restart");
        Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector2 d = new Vector2(x, z);
        if (controller.isGrounded)
        {
            y = 0;
        }
        y -= 9.8f * Time.deltaTime;
        Vector3 dir = Vector3.zero;
        if (d.magnitude > 0)
        {
            dir = Quaternion.Euler(0, camerascript.deg, 0) * new Vector3(x, 0.0f, z) * 7.0f;
            float deg = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            Vector3 angles = transform.localEulerAngles;
            angles.y = -(deg - 90);
            transform.localEulerAngles = angles;
        }
        dir .y= y;
        controller.Move(dir * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gold")
        {
            score += 100;
            Score.text = "Score " + score;
            Destroy(other.gameObject);
            if (score >= 500)
            {
                Clear.text = "Game Clear!";
                Button.SetActive(true);
            }
        }
    }
    public void Onclik()
    {
        SceneManager.LoadScene("maze");
    }
}
