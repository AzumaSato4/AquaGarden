using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectPlayer : MonoBehaviour
{
    public GameObject NumberOfPlayer;
    List<Button> buttons = new List<Button>();

    public Color selectedColor = new Color(0, 50, 0, 1);
    public Color deselectedColor = new Color(0, 50, 0, 0.3f);
    public static int selectPlayer;

    public void Start()
    {

        foreach (Transform child in NumberOfPlayer.transform)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null)
            {
                buttons.Add(btn);
            }
        }
    }


    public void Select(Button selectedButton)
    {
        foreach (Button btn in buttons)
        {
            Image img = btn.GetComponent<Image>();
            if (btn == selectedButton)
            {
                img.color = selectedColor;
            }
            else
            {
                img.color = deselectedColor;
            }
        }
    }

    public void SelectNumber(int player)
    {
        selectPlayer = player;
        Debug.Log(selectPlayer);
    }
}
