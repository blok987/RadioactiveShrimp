using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform doorPos;
    public Button button; //drag the button you want the door to be opened by over this
    public bool opened = false;
    public Transform doorOpenedP;
    public Transform doorClosedP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (button.pressed && !opened)
        {
            openDoor();
        }
        else if (!button.pressed && opened)
        {
            closeDoor();
        }
    }

    public void openDoor()
    {
        doorPos.position = new Vector2(transform.position.x, doorOpenedP.position.y);
        opened = true;
    }

    public void closeDoor()
    {
        doorPos.position = new Vector2(transform.position.x, doorClosedP.position.y);
        opened = false;
    }
}
