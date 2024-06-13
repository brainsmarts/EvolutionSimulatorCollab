using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateDebugger : MonoBehaviour
{
    [SerializeField]
    public Grid grid;
    [SerializeField]
    public Camera mainCamera;
    [SerializeField]
    public TMPro.TMP_Text coordinatesText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not assigned.");
            return;
        }

        if (grid == null)
        {
            Debug.LogError("Grid is not assigned.");
            return;
        }

        if (coordinatesText == null)
        {
            Debug.LogError("Coordinates Text is not assigned.");
            return;
        }

        // Get the world position of the mouse cursor
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0; // Ensure the z-coordinate is zero to align with 2D grid

        // Convert the world position to a cell position
        Vector3Int cellPosition = grid.WorldToCell(worldPosition);

        // Update the UI Text with the cell coordinates
        coordinatesText.text = $"Cell: {cellPosition}";
    }
}
