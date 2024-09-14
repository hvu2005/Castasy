using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool isInteract;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isInteract && Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("SSSSS");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteract = false;
        }
    }
}