using UnityEngine;

public class PlaceAtScreenEdge : MonoBehaviour
{
    public Camera cameraToUse; // Assign your main camera here if not using Camera.main
    public float objectWidth = 0f; // Adjust this based on the size of your object to avoid half of it being off-screen
    private int lastScreenWidth;
    private int lastScreenHeight;
    
    void Start()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }

    void Update()
    {
        // Check if the screen size has changed
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            // Update last screen size
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;

            // Call your method to handle the screen size change
            PositionObjectAtLeftEdge();
        }
    }

    void PositionObjectAtLeftEdge()
    {
        // Use Camera.main if no specific camera is assigned
        if (!cameraToUse)
        {
            cameraToUse = Camera.main;
        }
        
        Vector3 newPosition = cameraToUse.ViewportToWorldPoint(new Vector3(0, 0.5f, cameraToUse.nearClipPlane));
        newPosition += cameraToUse.transform.right * objectWidth; // Adjust position based on the object's width to keep it fully visible

        // Set the object's position, keeping its current y and z (depth) position
        transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);
    }
}