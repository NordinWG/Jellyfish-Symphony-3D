using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [System.Serializable]
    public class Quest
    {
        public string questName;
        public string description;
        public bool isActive;
        public bool isCompleted;
        public Item requiredItem;
        public int requiredQuantity;
        public Item rewardItem;

        public Quest(string name, string desc, Item reqItem, int reqQty, Item reward = null)
        {
            questName = name;
            description = desc;
            requiredItem = reqItem;
            requiredQuantity = reqQty;
            rewardItem = reward;
            isActive = false;
            isCompleted = false;
        }
    }

    public List<Quest> quests = new List<Quest>();
    public DialogueLines crabNPC;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("QuestManager instance initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartQuest(string questName)
    {
        Quest quest = quests.Find(q => q.questName == questName);
        if (quest != null)
        {
            if (!quest.isActive)
            {
                quest.isActive = true;
                Debug.Log($"Quest started: {quest.questName}");
            }
            else
            {
                Debug.Log($"Quest {questName} is already active.");
            }
        }
        else
        {
            Debug.LogError($"Quest {questName} not found in QuestManager!");
        }
    }

    public void CheckQuestProgress()
    {
        foreach (Quest quest in quests)
        {
            if (quest.isActive && !quest.isCompleted)
            {
                int itemCount = GetItemCount(quest.requiredItem);
                if (itemCount >= quest.requiredQuantity)
                {
                    quest.isCompleted = true;
                    Debug.Log($"Quest {quest.questName} ready to turn in!");
                }
            }
        }
    }

    public bool TurnInQuest(string questName)
    {
        Quest quest = quests.Find(q => q.questName == questName);
        if (quest != null && quest.isActive && quest.isCompleted)
        {
            Inventory.instance.RemoveItem(quest.requiredItem, quest.requiredQuantity);
            Debug.Log($"Turned in {quest.requiredQuantity} {quest.requiredItem.itemName} for {quest.questName}");

            if (quest.rewardItem != null)
            {
                Inventory.instance.AddItem(quest.rewardItem, 1);
                Debug.Log($"Received reward: {quest.rewardItem.itemName}");
            }

            quest.isActive = false;
            return true;
        }
        Debug.Log($"TurnInQuest failed for {questName}. Active: {quest?.isActive}, Completed: {quest?.isCompleted}");
        return false;
    }

    private int GetItemCount(Item item)
    {
        int count = 0;
        foreach (InventorySlot slot in Inventory.instance.inventorySlots)
        {
            if (slot.item == item)
            {
                count += slot.quantity;
            }
        }
        return count;
    }
}