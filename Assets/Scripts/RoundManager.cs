using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public float RoundTime = 5f;
    private const string BulletTag = "Bullet";
    private InGameManager[] allPlayers;
    private bool postActionsProcessed = false;

    void Start()
    {
        allPlayers = FindObjectsByType<InGameManager>(FindObjectsSortMode.None);
        StartNewRound();
    }

    private void StartNewRound()
    {
        postActionsProcessed = false;
        StartCoroutine(RunRound());
    }

    private IEnumerator RunRound()
    {
        Debug.Log("New Round started. Waiting for player input to lock in...");
        
        yield return new WaitForSeconds(RoundTime);

        ProcessPlayerInput();
        Debug.Log("Player actions processed. Executing round logic now.");

        float noBulletTimer = 0f;
        while (true)
        {
            GameObject[] activeBullets = GameObject.FindGameObjectsWithTag(BulletTag);
            
            if (activeBullets.Length == 0)
            {
                noBulletTimer += Time.deltaTime;

                if (noBulletTimer >= 1f && !postActionsProcessed)
                {
                    ProcessPostActions();
                    postActionsProcessed = true; 
                    Debug.Log("Post actions processed.");
                }

                if (noBulletTimer >= 2f)
                {
                    Debug.Log("All actions complete. Starting new round.");
                    StartNewRound();
                    yield break; // Exit the coroutine
                }
            }
            else
            {
                noBulletTimer = 0f;
            }
            yield return null; // Wait for the next frame
        }
    }
    
    private void ProcessPlayerInput()
    {
        foreach (var player in allPlayers)
        {
            if (player != null)
            {
                player.ProcessAction();
            }
        }
    }

    private void ProcessPostActions()
    {
        foreach (var player in allPlayers)
        {
            if (player != null)
            {
                player.PostAction();
            }
        }
    }
}