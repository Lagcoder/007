using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerLinker : MonoBehaviour
{
    public GameObject[] existingPlayers;
    private int nextPlayerIndex = 0;

    void Awake()
    {
        var playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        if (playerInputManager != null)
        {
            playerInputManager.onPlayerJoined += OnPlayerJoined;
        }
    }
    void Update()
    {
        existingPlayers = existingPlayers.Where(p => p != null && p.activeInHierarchy).ToArray();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (nextPlayerIndex < existingPlayers.Length)
        {
            var playerObject = existingPlayers[nextPlayerIndex];
            var playerMovementScript = playerObject.GetComponentInChildren<InGameManager>();
            if (playerMovementScript != null)
            {
                playerMovementScript.SetPlayerInput(playerInput);
                nextPlayerIndex++;
            }
        }
    }
}
