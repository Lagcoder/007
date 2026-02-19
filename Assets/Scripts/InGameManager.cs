using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Users;

public class InGameManager : MonoBehaviour, IBulletHittable
{
    public GameObject bulletPrefab;
    public Dictionary<string, int> itemCounts = new Dictionary<string, int>();
    public string action = "Nothing";
    public string processedAction = "Nothing";
    string Stolen = "None";
    private int RetrievedValue;
    public GameObject currentTargetObject;
    public InGameManager targetManager;
    bool takingEvents = true;
    bool bombShot = false;
    private InGameManager[] allPlayers;
    public SpriteChanger SpriteChanger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetPlayerInput(PlayerInput input)
    {
        //_playerInput = input;
        Debug.Log("Player Input assigned to InGameManager for player: " + this.gameObject.name);
    }
    void Start()
    {
        allPlayers = FindObjectsByType<InGameManager>(FindObjectsSortMode.None);
        itemCounts.Add("Lazer", 0);
        itemCounts.Add("Shield", 0);
        itemCounts.Add("Reflect", 0);
        itemCounts.Add("WeakSH", 1);
        itemCounts.Add("Steal", 1);
        itemCounts.Add("Bomb", 1);
    }
    public void StealProcess()
    {
        targetManager = currentTargetObject.GetComponentInChildren<InGameManager>();
        var StolenItem = targetManager.StolenProcess(gameObject);
        try{
            itemCounts[StolenItem] = 1;
            Stolen = StolenItem;
        }
        catch
        {
            Debug.LogError("Player stolen invalid item or other error");
        }


    }
    public string StolenProcess(GameObject sender)
    {
        //working on
        if (processedAction == "Steal" && sender == currentTargetObject)
        {
            //itemCounts = currentTargetObject.SwapItems()
            return "Nothing";
        }
        else if (processedAction == "Steal" || processedAction == "WeakSH")
        {
            return "Nothing";
        }
        else
        {

            var currentItem = processedAction;
            processedAction = "Nothing";
            return currentItem;

        }
            

    }
    public void PostAction()
    {
        if (processedAction == "Bomb")
        {
            if (bombShot)
            {
                foreach (var player in allPlayers)
                {
                    if (player != null)
                    {
                        StartCoroutine(Fire("Bomb", player.gameObject));
                    }
                }
            }
        }
        if (processedAction == "Reload")
        {
        itemCounts["Lazer"] = Mathf.Max(itemCounts["Lazer"], 1);
        itemCounts["Shield"] = Mathf.Max(itemCounts["Shield"], 1);
        itemCounts["Reflect"] = Mathf.Max(itemCounts["Reflect"], 1);
            
            if (Stolen != "None")
            {
                try
                {
                    itemCounts[Stolen] = 0;
                    Stolen = "None";
                }
                catch
                {
                    Debug.LogError("Player stolen invalid item or other error");
                    Stolen = "None";
                }
            }
        }
    }
    public void PointAward(GameObject Target, float reward)
    {
        //empty 4 now
        return;
    }
    public void ProcessAction()
    {
        if (action == "Reload")
        {
            Debug.Log("Reload");
            processedAction = "Reload";
        }
        else if (action == "Lazer" && itemCounts.TryGetValue("Lazer", out RetrievedValue) && RetrievedValue != 0)
        {
            itemCounts["Lazer"] -= 1;
            Debug.Log("Lazer");
            processedAction = "Lazer";
            StartCoroutine(Fire("Standard", this.gameObject));

        }
        else if (action == "Shield" && itemCounts.TryGetValue("Shield", out RetrievedValue) && RetrievedValue != 0)
        {
            itemCounts["Shield"] -= 1;
            Debug.Log("Shield");
            processedAction = "Shield";
        }
        else if (action == "Reflect" && itemCounts.TryGetValue("Reflect", out RetrievedValue) && RetrievedValue != 0)
        {
            itemCounts["Reflect"] -= 1;
            Debug.Log("Reflect");
            processedAction = "Reflect";
        }
        else if (action == "Steal" && itemCounts.TryGetValue("Steal", out RetrievedValue) && RetrievedValue != 0)
        {
            itemCounts["Steal"] -= 1;
            Debug.Log("Steal");
            processedAction = "Steal";
        }
        else if (action == "Bomb" && itemCounts.TryGetValue("Bomb", out RetrievedValue) && RetrievedValue != 0)
        {
            itemCounts["Bomb"] -= 1;
            Debug.Log("Bomb");
            processedAction = "Bomb";
        }
        else if (itemCounts.TryGetValue("WeakSH", out RetrievedValue) && RetrievedValue != 0)
        {
            itemCounts["WeakSH"] -= 1;
            Debug.Log("WeakSH");
            processedAction = "WeakSH";
        }
        else
        {
            Debug.Log("NoThing");
            processedAction = "Nothing";
        }

        if (Stolen != "None")
        {
            try
            {
                itemCounts[Stolen] = 0;
                Stolen = "None";
            }
            catch
            {
                Debug.LogError("Player stolen invalid item or other error");
                Stolen = "None";
            }
        }
        SpriteChanger.ChangeSprite(processedAction);
    }
    IEnumerator Fire(string type, GameObject originalSender)
    {
        yield return new WaitForSeconds(0.5f);
        if (processedAction == "Lazer" || type != "Standard")
        {
            GameObject newBulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bullet = newBulletGO.GetComponent<Bullet>();
            bullet.Initialize(currentTargetObject.transform, this.gameObject, type, originalSender);
        }
        yield break;
    }
    public void OnBulletHit(string bulletType, GameObject sender, GameObject originalSender)
    {
        if (bulletType == "Explosive" && processedAction != "Shield")
        {
            if (processedAction == "Nothing" || processedAction == "Reload" || processedAction == "Steal")
            {
                Debug.Log($"{this.gameObject.name} was hit by a {bulletType} lazer from {sender.name}!");
                PointAward(sender, 1f);
            }
            else
            {
                Debug.Log($"{this.gameObject.name} was hit by a {bulletType} lazer from {sender.name}, breaking their {processedAction}.");
                PointAward(sender, 1f);
            }
            return;
        }
        if (processedAction == "Lazer" || processedAction == "Reload" || processedAction == "Nothing" || processedAction == "Steal")
        {
            if (originalSender == sender)
            {
                Debug.Log($"{this.gameObject.name} was hit by a {bulletType} lazer from {sender.name}!");
                PointAward(sender, 1f);

            }
            else if (originalSender == this.gameObject)
            {
                Debug.Log($"{this.gameObject.name} was hit by a {bulletType} lazer from their own gun, reflected by {sender.name}!");
                PointAward(sender, 1f);
            }
            else
            {
                Debug.Log($"{this.gameObject.name} was hit by a {bulletType} lazer reflected by {sender.name}, shot by {originalSender.name}!");
                PointAward(sender, 0.5f);
                PointAward(originalSender, 0.5f);
            }
        }
        if (processedAction == "Shield")
            {
                Debug.Log($"{this.gameObject.name} blocked a {bulletType} lazer from {sender.name}!");
            }
        if (processedAction == "WeakSH")
        {
            if (itemCounts.TryGetValue("Shield", out RetrievedValue) && RetrievedValue != 0)
            {
                Debug.Log($"{this.gameObject.name} blocked a {bulletType} lazer from {sender.name} as they didn't do anything, but their shield broke.");
                itemCounts["Shield"] = 0;
            }
            else if (itemCounts.TryGetValue("Reflect", out RetrievedValue) && RetrievedValue != 0 || itemCounts.TryGetValue("Lazer", out RetrievedValue) && RetrievedValue != 0)
            {
                Debug.Log($"{this.gameObject.name} got hit by a {bulletType} lazer from {sender.name} but as they didn't choose an option in time, we will give them another chance.");
            }
            else
            {
                Debug.Log($"{this.gameObject.name} got hit by a {bulletType} lazer from {sender.name}!");
            }
        }
        if (processedAction == "Reflect")
        {
            if (bulletType == "Reflected")
            {
                Debug.Log($"{this.gameObject.name} reflected a {bulletType} lazer reflected by {sender.name}, originally fired by {originalSender.name}, at {currentTargetObject.name}!");
                StartCoroutine(Fire("Multi-Reflected", originalSender));
            }
            else if (bulletType == "Multi-Reflected")
            {
                Debug.Log($"{this.gameObject.name} reflected a {bulletType} lazer reflected too many times to count, at {currentTargetObject.name}");
                StartCoroutine(Fire("Multi-Reflected", originalSender));
            }
                Debug.Log($"{this.gameObject.name} reflected a {bulletType} lazer from {sender.name} at {currentTargetObject.name}!");
                StartCoroutine(Fire("Reflected", originalSender));
        }
        if (processedAction == "Steal")
        {
            Debug.Log($"{this.gameObject.name} was hit by a {bulletType} lazer from {sender.name}!");
        }
        if (processedAction == "Bomb")
        {
            Debug.Log($"{this.gameObject.name} was hit by a {bulletType} lazer from {sender.name}!");
            Debug.Log($"Oh No... {this.gameObject.name} has a bomb! {this.gameObject.name}'s bomb fires an explosive lazer at everyone. It was {sender.name}'s fault!");
            PointAward(sender, -1f);
            bombShot = true;
        }
    }

    public void Lazer(InputAction.CallbackContext context)
    {
        if (takingEvents)
        {
            action = "Lazer";
            
        }
    }
    public void Shield(InputAction.CallbackContext context)
    {
        if (takingEvents)
        {
            action = "Shield";
            
        }
    }
    public void Reflect(InputAction.CallbackContext context)
    {
        if (takingEvents)
        {
            action = "Reflect";
            
        }
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (takingEvents)
        {
            action = "Reload";
            
        }
    }
    public void Steal(InputAction.CallbackContext context)
    {
        if (takingEvents)
        {
            action = "Steal";
            
        }
    }
    public void Bomb(InputAction.CallbackContext context)
    {
        if (takingEvents)
        {
            action = "Bomb";
            
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
