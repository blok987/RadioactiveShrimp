using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public Transform CheckpointPos;
    public bool CheckpointHit;

    private void Start()
    {
        CheckpointPos = transform;
    }
}
