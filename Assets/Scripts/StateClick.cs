using UnityEngine;
using UnityEngine.UI;

public class StateClick : MonoBehaviour
{
    public Color highlightColor = Color.yellow;   // Color to highlight the state
    private Color originalColor;                  // Original color of the state
    private Renderer stateRenderer;               // Renderer for the state object
    public GameObject infoPanel;                  // Reference to the info panel UI
    public Text infoText;                         // Reference to the Text component in the info panel
    public Button closeButton;                    // Reference to the close button
    public string stateInfo;                      // Info to show for this state

    private bool isHighlighted = false;           // Tracks if the state is currently highlighted
    private static StateClick currentlySelectedState;  // Static reference to track the highlighted state

    void Start()
    {
        // Get the renderer component of the state object
        stateRenderer = GetComponent<Renderer>();

        // Store the original color of the state
        if (stateRenderer != null)
        {
            originalColor = stateRenderer.material.color;
        }

        // Set up the info panel to be hidden initially
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);  // Start with the info panel hidden
        }

        // Assign the Close button to hide the panel when clicked
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseInfoPanel);
        }
    }

    void Update()
    {
        // Check for touch input on mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // If the touch has just begun
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position); // Cast a ray from the touch point
                RaycastHit hit;

                // Perform the raycast and check if it hits this state object
                if (Physics.Raycast(ray, out hit))
                {
                    // If this state was hit by the ray
                    if (hit.transform == transform)
                    {
                        ToggleState(); // Toggle state highlight and info
                    }
                }
            }
        }
    }

    // Method to toggle the state highlight and show/hide info
    private void ToggleState()
    {
        // If another state is currently selected, reset it
        if (currentlySelectedState != null && currentlySelectedState != this)
        {
            currentlySelectedState.ResetHighlight();
            currentlySelectedState.CloseInfoPanel();
        }

        // Toggle highlight and info for this state
        if (isHighlighted)
        {
            ResetHighlight();
            CloseInfoPanel();
            currentlySelectedState = null;
        }
        else
        {
            HighlightState();
            ShowInfoPanel();
            currentlySelectedState = this;
        }
    }

    // Method to highlight the state
    private void HighlightState()
    {
        if (stateRenderer != null)
        {
            stateRenderer.material.color = highlightColor;
        }
        isHighlighted = true;
    }

    // Method to reset the highlight (restore original color)
    public void ResetHighlight()
    {
        if (stateRenderer != null)
        {
            stateRenderer.material.color = originalColor;
        }
        isHighlighted = false;
    }

    // Method to show the info panel
    public void ShowInfoPanel()
    {
        if (infoPanel != null && infoText != null)
        {
            infoText.text = stateInfo;     // Set the state info in the text component
            infoPanel.SetActive(true);     // Show the info panel
        }
    }

    // Method to hide the info panel
    public void CloseInfoPanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);    // Hide the info panel
        }
    }
}
