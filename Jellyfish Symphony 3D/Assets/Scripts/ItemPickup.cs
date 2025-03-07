using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int quantity;
    public TextMeshPro textPrompt;

    private void Start()
    {
        if (textPrompt != null)
        {
            textPrompt.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (textPrompt != null)
        {
            textPrompt.transform.rotation = Quaternion.LookRotation(textPrompt.transform.position - Camera.main.transform.position);
        }
    }
    public void ShowPickupPrompt(bool show)
    {
        if (textPrompt != null)
        {
            textPrompt.gameObject.SetActive(show);
            textPrompt.text = show ? "Press E" : "";
        }
    }
}
