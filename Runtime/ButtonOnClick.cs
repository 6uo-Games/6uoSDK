using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClick : MonoBehaviour
{
    private Color originalColor; // Original color of the text
    
    [SerializeField]
    float colorChangeDuration = 0.3f; // Duration for color change (in seconds)

    private Button button;
    private Text buttonText;

    void Start()
    {
        // Get references to Button and Text components
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<Text>();

        // Store the original color of the text
        originalColor = buttonText.color;

        // Add onClick listener to the button
        button.onClick.AddListener(ChangeTextColorOnClick);
    }

    void ChangeTextColorOnClick()
    {
        // Change the color of the text to the reaction color
        buttonText.color = originalColor * 0.6f;

        // Invoke the method to revert the color change after a delay
        Invoke("RevertTextColor", colorChangeDuration);
    }

    void RevertTextColor()
    {
        // Revert the color of the text to the original color
        buttonText.color = originalColor;
    }
}
