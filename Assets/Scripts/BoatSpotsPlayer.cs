using UnityEngine;

public class BoatSpotsPlayer : MonoBehaviour
{
    public PlayerSpotted playerSpottedEventChannel; // Reference to the PlayerSpotted event channel
    public GameObject agent; // Reference to the agent GameObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player GameObject has the tag "Player"
        {
            GameObject player = other.gameObject;
            playerSpottedEventChannel.SendEventMessage(agent, player);
        }
    }
}