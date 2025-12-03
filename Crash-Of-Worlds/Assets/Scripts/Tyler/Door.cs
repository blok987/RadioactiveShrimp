using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform doorPos; // The doors current position
    public Button button; // Drag the button you want the door to be opened by over this
    public bool opened = false; // If the door has been opened
    public Transform doorOpenedP; // The doors opened position
    public Transform doorClosedP; // The doors closed position
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region DOOR CHECKS
        // If the button was pressed and the door isn't opened, open the door
        if (button.pressed && !opened)
        {
            openDoor();
        }
        else if (!button.pressed && opened) // If the button isn't pressed and the door is open, close the door
        {
            closeDoor();
        }
        #endregion
    }
    #region DOOR METHODS
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
    #endregion
}
