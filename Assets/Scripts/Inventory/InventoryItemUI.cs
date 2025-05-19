using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // Assign this appropriately
    public Tooltip tooltip; // Reference to the Tooltip instance


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        if (tooltip == null)
            tooltip = FindObjectOfType<Tooltip>();
        // Immediately show the tooltip when the cursor hovers over the item
        System.Text.StringBuilder description = new System.Text.StringBuilder();
        //description.Append($"<size=25>{item.Name}</size> <size=20>" + $"\n{item.Description} </size> \n");
        description.Append(item.name);
        description.Append((item.Description != "" ? $"\n{item.Description}" : ""));
        //tooltip.ShowTooltip($"<size=25>{item.Name}</size> <size=20>" + $"\n{item.Description} </size>");
        if (item is Equipment equipment)
        {
            Dictionary<StatType, float> statsDictionary
                = equipment.gainedStats;
            description.Append($"\n+{equipment.Defense} Defense \n");
            foreach (KeyValuePair<StatType, float> stat in statsDictionary)
            {
                if (stat.Value != 0)
                {
                    if (stat.Value > 0)
                        description.Append($"\n+{stat.Value} {stat.Key}");
                    else
                        description.Append($"\n{stat.Value} {stat.Key}"); // for negative

                    //description.Append($"<size=20> {stat.Key} +{stat.Value} </size> \n");
                }
            }
        }
        if (item is Sword sword)
        {
            description.Append($"\n\n{sword.physicalDamage} Damage");
            description.Append($"\n{Mathf.Round((1/sword.attackDuration) * 100) / 100} Attack Speed");
        }
        if (item is Staff staff)
        {
            description.Append($"\nMana cost: {staff.manaCost}");
            description.Append($"\n\n{staff.magicDamage} Damage");
            description.Append($"\n{Mathf.Round((1 / staff.attackCooldown) * 100) / 100} Attack Speed");
        }
        tooltip.ShowTooltip(description.ToString());
        //tooltip.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooltip.transform.GetChild(1).GetComponent<Text>().preferredWidth);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip when the cursor leaves the item // omg so useful comment it must be left here
        tooltip.HideTooltip();
    }

}
