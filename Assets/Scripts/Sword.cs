using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animation anim;
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.Play("swing");
        }
    }
}
