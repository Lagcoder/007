using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
public class KeyboardSplitter : MonoBehaviour
{
    // Assign these in the Inspector.
    // They should reference the PlayerInput components of your player prefabs.
    public PlayerInput player1Input;
    public PlayerInput player2Input;

    void Start()
    {
        // Check if both player inputs are assigned and a keyboard is available.
        if (player1Input != null && player2Input != null && Keyboard.current != null)
        {
            // First, make sure the PlayerInputManager doesn't try to auto-pair.
            // This is crucial to prevent conflicts.
            // In a real-world scenario, you would have a more robust way to handle this.
            // For this example, we assume we're handling all pairing manually. 
                // Unpair any existing device from Player 1's user.
                player1Input.user.UnpairDevices();

                // Manually pair the keyboard to Player 1's user.
                InputUser.PerformPairingWithDevice(Keyboard.current, player1Input.user);

                // Set the control scheme for Player 1.
                player1Input.SwitchCurrentControlScheme("Player", Keyboard.current);
                // Unpair any existing device from Player 2's user.
                player2Input.user.UnpairDevices();

                // Manually pair the keyboard to Player 2's user.
                // This is the key step that allows a single device to be shared.
                InputUser.PerformPairingWithDevice(Keyboard.current, player2Input.user);

                // Set the control scheme for Player 2.
                player2Input.SwitchCurrentControlScheme("Player 2", Keyboard.current);

            Debug.Log("Keyboard manually split and assigned to both Player 1 and Player 2.");
        }
        else
        {
            Debug.LogError("KeyboardSplitter is missing PlayerInput references or no keyboard is available.");
        }
    }
}
