using UnityEngine;

public class TriggerCooldown : MonoBehaviour
{
    public float cooldownTime = 10f;
    private float nextTriggerTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= nextTriggerTime)
            {
                // Trigger the desired action here
                Debug.Log("Trigger activated!");
                // Set the next trigger time
                nextTriggerTime = Time.time + cooldownTime;
            }
            else
            {
                Debug.Log("Trigger on cooldown. Please wait.");
            }
        }
    }
}
