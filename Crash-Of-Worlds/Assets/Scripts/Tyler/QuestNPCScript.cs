using UnityEngine;

public class QuestNPCScript : MonoBehaviour
{
    public GameManagerScript Game;

    public GameObject questRequirement;
    public GameObject reward;
    public bool questStarted;
    public bool questFinished;

    public bool rewardDisapears;
    public bool rewardApears;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            questStarted = true;
        }
    }
}
