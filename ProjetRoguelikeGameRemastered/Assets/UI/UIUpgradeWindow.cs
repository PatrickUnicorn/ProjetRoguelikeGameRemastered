using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class UIUpgradeWindow : MonoBehaviour
{

    VerticalLayoutGroup verticalLayout;
    public RectTransform upgradeOptionTemplate;
    public TextMeshProUGUI tooltipTemplate;

    [Header("Settings")]
    public int maxOptions = 4;
    public string iconPath = "Icon/Item Icon", namePath = "Name", descriptionPath = "Description", buttonPath = "Button";

    RectTransform rectTransform; 
    float optionHeight;
    int activeOptions; 
    List<RectTransform> upgradeOptions = new List<RectTransform>();
    Vector2 lastScreen;

    public void SetUpgrades(PlayerInventory inventory, List<ItemData> possibleUpgrades, int pick = 3, string tooltip = "") 
    {
        pick = Mathf.Min(maxOptions, pick);

        if (maxOptions > upgradeOptions.Count)
        {
            for (int i = upgradeOptions.Count; i < pick; i++)
            {
                GameObject go = Instantiate(upgradeOptionTemplate.gameObject, transform);
                upgradeOptions.Add((RectTransform)go.transform);
            }
        }

        
        if (tooltip != "")
        {
            tooltipTemplate.text = tooltip;
            tooltipTemplate.gameObject.SetActive(true);
        }
        else
        {
            tooltipTemplate.gameObject.SetActive(false);
        }

        
        activeOptions = 0;
        foreach(RectTransform r in upgradeOptions)
        {
            if (activeOptions < possibleUpgrades.Count)
            {
                r.gameObject.SetActive(true);

                
                ItemData selected = possibleUpgrades[Random.Range(0, possibleUpgrades.Count)];
                possibleUpgrades.Remove(selected);
                Item item = inventory.Get(selected);

                
                TextMeshProUGUI name = r.Find(namePath).GetComponent<TextMeshProUGUI>();
                if(name)
                {
                    name.text = selected.name;
                    if(item)
                    {
                        name.text += " - Level " + (item.currentLevel + 1);
                    }
                }

                
                TextMeshProUGUI desc = r.Find(descriptionPath).GetComponent<TextMeshProUGUI>();
                if (desc)
                {
                    if (item)
                    {
                        desc.text = selected.GetLevelData(item.currentLevel + 1).description;
                    }
                    else
                    {
                        desc.text = selected.GetLevelData(1).description;
                    }
                }

                Image icon = r.Find(iconPath).GetComponent<Image>();
                if(icon)
                {
                    icon.sprite = selected.icon;
                }

                
                Button b = r.Find(buttonPath).GetComponent<Button>();
                if (b)
                {
                    b.onClick.RemoveAllListeners();
                    if (item)
                        b.onClick.AddListener(() => inventory.LevelUp(item));
                    else
                        b.onClick.AddListener(() => inventory.Add(selected));
                }
            }
            else r.gameObject.SetActive(false);
        }

        RecalculateLayout();
    }

    void RecalculateLayout()
    {

        optionHeight = (rectTransform.rect.height - verticalLayout.padding.top - verticalLayout.padding.bottom - (maxOptions - 1) * verticalLayout.spacing);
        if (activeOptions == maxOptions && tooltipTemplate.gameObject.activeSelf)
            optionHeight /= maxOptions + 1;
        else
            optionHeight /= maxOptions;

        if (tooltipTemplate.gameObject.activeSelf)
        {
            RectTransform tooltipRect = (RectTransform)tooltipTemplate.transform;
            tooltipTemplate.gameObject.SetActive(true);
            tooltipRect.sizeDelta = new Vector2(tooltipRect.sizeDelta.x, optionHeight);
            tooltipTemplate.transform.SetAsLastSibling();
        }

        foreach (RectTransform r in upgradeOptions)
        {
            if (!r.gameObject.activeSelf) continue;
            r.sizeDelta = new Vector2(r.sizeDelta.x, optionHeight);
        }
    }

    void Update()
    {
        RecalculateLayout();
    }

    void Awake()
    {
        verticalLayout = GetComponentInChildren<VerticalLayoutGroup>();
        if (tooltipTemplate) tooltipTemplate.gameObject.SetActive(false);
        if (upgradeOptionTemplate) upgradeOptions.Add(upgradeOptionTemplate);

        rectTransform = (RectTransform)transform;
    }

    void Reset()
    {
        upgradeOptionTemplate = (RectTransform)transform.Find("Upgrade Option");
        tooltipTemplate = transform.Find("Tooltip").GetComponentInChildren<TextMeshProUGUI>();
    }
}
