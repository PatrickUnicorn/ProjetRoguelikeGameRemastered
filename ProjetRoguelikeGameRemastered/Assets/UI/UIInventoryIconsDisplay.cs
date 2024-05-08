using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(LayoutGroup))]
public class UIInventoryIconsDisplay : MonoBehaviour
{

    public GameObject slotTemplate;
    public uint maxSlots = 6;
    public bool showEmptySlots = true;
    public bool showLevels = true;
    public PlayerInventory inventory;

    public GameObject[] slots;

    [Header("Paths")]
    public string iconPath;
    public string levelTextPath;
    [HideInInspector] public string targetedItemList;

    void Reset()
    {
        slotTemplate = transform.GetChild(0).gameObject;
        inventory = FindObjectOfType<PlayerInventory>();
    }

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (!inventory) Debug.LogWarning("No inventory attached to the UI icon display.");

        Type t = typeof(PlayerInventory);
        FieldInfo field = t.GetField(targetedItemList, BindingFlags.Public | BindingFlags.Instance);

        if (field == null)
        {
            Debug.LogWarning("The list in the inventory is not found.");
            return;
        }

        List<PlayerInventory.Slot> items = (List<PlayerInventory.Slot>)field.GetValue(inventory);

        for(int i = 0; i < slots.Length; i++)
        {
            Item item = items[i].item;

            Transform iconObj = slots[i].transform.Find(iconPath);
            if(iconObj)
            {
                Image icon = iconObj.GetComponentInChildren<Image>();

                if (!item) icon.color = new Color(1, 1, 1, 0);
                else
                {
                    icon.color = new Color(1, 1, 1, 1);
                    if (icon) icon.sprite = item.data.icon;
                }
            }

            Transform levelObj = slots[i].transform.Find(levelTextPath);
            if(levelObj)
            {
                TextMeshProUGUI levelTxt = levelObj.GetComponentInChildren<TextMeshProUGUI>();
                if (levelTxt)
                {
                    if (!item) levelTxt.text = "";
                    else levelTxt.text = item.currentLevel.ToString();
                }
            }
        }
    }
}
