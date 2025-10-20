using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    private Text label;
    void Awake()
    {
        if (label == null)
        {
            label = GetComponentInChildren<Text>();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        label.color = getColor(255, 154, 148);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        label.color = getColor(255, 255, 255);
    }

    public Color getColor(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

}
