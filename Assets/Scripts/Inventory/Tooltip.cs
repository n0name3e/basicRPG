using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText;
    public GameObject tooltipPanel;
    private RectTransform rt;

    private void OnEnable()
    {
        rt = GetComponent<RectTransform>();
    }
    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipPanel.SetActive(true);
        SetTooltipPosition();
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy) SetTooltipPosition();
    }
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
    private void SetTooltipPosition()
    {
        //Vector2 position =;
        Vector2 screenPosition = Input.mousePosition; //Camera.main.WorldToScreenPoint(position);
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
        screenPosition.x += rt.sizeDelta.x * 1.5f - 200;
        screenPosition.y -= rt.sizeDelta.y * 1;



        // Apply the position to the tooltip RectTransform
        rt.position = screenPosition;
    }
}
