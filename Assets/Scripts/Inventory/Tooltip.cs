using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText;
    public GameObject tooltipPanel;

    public void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;
        tooltipPanel.SetActive(true);
        SetTooltipPosition();
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            SetTooltipPosition();
            ResizeTooltip();
        }
    }
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
    private void ResizeTooltip()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipPanel.GetComponent<RectTransform>()); // Adjust size dynamically
    }
    private void SetTooltipPosition()
    {
        tooltipPanel.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(60, -150);
        //Vector2 position =;
        //Vector2 screenPosition = Input.mousePosition; //Camera.main.WorldToScreenPoint(position);
        // Offset the tooltip slightly above the cursor
        //screenPosition.y += rt.sizeDelta.y * 0.5f;
        // Check if the tooltip goes off the screen and adjust accordingly
        /*if (screenPosition.y + rt.sizeDelta.y > Screen.height)
        {
            screenPosition.y -= rt.sizeDelta.y * 2;
        }

        if (screenPosition.x + rt.sizeDelta.x > Screen.width)
        {
            screenPosition.x += rt.sizeDelta.x * 4;
        }*/
        //screenPosition.x += rt.sizeDelta.x; // * 1.5f - 200;
        //screenPosition.y -= rt.sizeDelta.y * 1;



        // Apply the position to the tooltip RectTransform
        //rt.position = screenPosition;
    }
}
