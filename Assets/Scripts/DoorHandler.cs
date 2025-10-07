using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    Animator anim;
    private bool isOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player seen");
            anim.SetTrigger("playerEnter");
            isOpen = true;
            anim.SetBool("isOpen", isOpen);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player exited");
            anim.SetTrigger("playerExit");
            isOpen = false;
            anim.SetBool("isOpen", isOpen);
        }
    }




}
