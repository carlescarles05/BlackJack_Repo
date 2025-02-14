/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
    private int randomValue;            // Random value for spins
    private float timeInterval;         // Time interval between rotations
    public bool rowStopped;             // To check if the row has stopped
    public string stoppedSlot;          // Name of the stopped slot
    private bool isSpinning = false;
    /// temporary
    private float spinStartTime; // Store when the spin starts
    private float spinDuration;  // Store how long it took

    void Start()
    {
        rowStopped = true;
        GameControl.HandlePulled += StartRotating; // Subscribe to the HandlePulled event
    }

    private void StartRotating()
    {
        Debug.Log($"Row {gameObject.name} started rotating.");

        stoppedSlot = "";
        rowStopped = false;

        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        rowStopped = false;
        timeInterval = 0.025f;
        while (isSpinning)
            {
            for (int i = 0; i < 30; i++)
            {
                if (transform.position.y <= -3.5f)
                {
                    transform.position = new Vector2(transform.position.x, 1.75f);
                }
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
                yield return new WaitForSeconds(timeInterval);
            }

            randomValue = Random.Range(60, 100);

            switch (randomValue % 3)
            {
                case 1:
                    randomValue += 2;
                    break;
                case 2:
                    randomValue += 1;
                    break;
            }

            for (int i = 0; i < randomValue; i++)
            {
                if (transform.position.y <= -3.5f)
                {
                    transform.position = new Vector2(transform.position.x, 1.75f);
                }
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

                if (i > Mathf.RoundToInt(randomValue * 0.95f)) { timeInterval = 0.2f; }
                else if (i > Mathf.RoundToInt(randomValue * 0.75f)) { timeInterval = 0.15f; }
                else if (i > Mathf.RoundToInt(randomValue * 0.5f)) { timeInterval = 0.05f; }
                else if (i > Mathf.RoundToInt(randomValue * 0.25f)) { timeInterval = 0.01f; }

                yield return new WaitForSeconds(timeInterval);
            }
        }//end while
        isSpinning = false;
        // Determine the stopped slot based on the final position
        float yPosition = transform.position.y;
        if (Mathf.Abs(yPosition - (-3.5f)) < 0.01f) { stoppedSlot = "Diamond"; }
        else if (Mathf.Abs(yPosition - (-2.75f)) < 0.01f) { stoppedSlot = "Crown"; }
        else if (Mathf.Abs(yPosition - (-2f)) < 0.01f) { stoppedSlot = "Melon"; }
        else if (Mathf.Abs(yPosition - (-1.25f)) < 0.01f) { stoppedSlot = "Bar"; }
        else if (Mathf.Abs(yPosition - (-0.5f)) < 0.01f) { stoppedSlot = "Seven"; }
        else if (Mathf.Abs(yPosition - (0.25f)) < 0.01f) { stoppedSlot = "Cherry"; }
        else if (Mathf.Abs(yPosition - (1f)) < 0.01f) { stoppedSlot = "Lemon"; }
        else if (Mathf.Abs(yPosition - (1.75f)) < 0.01f) { stoppedSlot = "Diamond"; }

        rowStopped = true;
        Debug.Log($"Row {gameObject.name} stopped with slot: {stoppedSlot}");
    }

    private void OnDestroy()
    {
        GameControl.HandlePulled -= StartRotating; // Unsubscribe from the HandlePulled event
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;
public class Row : MonoBehaviour
{
    private int randomValue;            // Random value for spins
    private float timeInterval;         // Time interval between rotations
    public bool rowStopped;             // To check if the row has stopped
    public string stoppedSlot;          // Name of the stopped slot

    void Start()
    {
        rowStopped = true;
        GameControl.HandlePulled += StartRotating; // Subscribe to the HandlePulled event
    }
    private void StartRotating()
    {
        Debug.Log($"Row {gameObject.name} started rotating.");

        stoppedSlot = "";
        // Play spin sound when the row starts spinning
        if (SFXManagerSMtwo.Instance != null)
        {
            SFXManagerSMtwo.Instance.Spin();
        }

        StartCoroutine(Rotate());
    }
    /*
 private IEnumerator Rotate()
    {
        rowStopped = false;
        timeInterval = 0.025f;

        for (int i = 0; i < 30; i++)
        {
            if (transform.position.y <= -3.5f)
            {
                transform.position = new Vector2(transform.position.x, 1.75f);
            }
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
            yield return new WaitForSeconds(timeInterval);
        }

        // FORCE a specific winning combination for testing
        stoppedSlot = "Diamond";  // Change to "Diamond" if needed
        rowStopped = true;

        Debug.Log($"Row {gameObject.name} stopped with forced slot: {stoppedSlot}");

    }
   */
   private IEnumerator Rotate()
     {
         rowStopped = false;
         timeInterval = 0.025f;
        // Initial rotation (fixed number of spins)
        for (int i = 0; i < 30; i++)
        {
            if (transform.position.y <= -3.5f)
            {
                transform.position = new Vector2(transform.position.x, 1.75f);
            }
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
            yield return new WaitForSeconds(timeInterval);
        }
        // Generate a random value for spins
        randomValue = Random.Range(60, 100);

        // Adjust randomValue to ensure alignment
        switch (randomValue % 3)
        {
            case 1:
                randomValue += 2;
                break;
            case 2:
                randomValue += 1;
                break;
        }

        // Main spinning logic with slowing down effect
        for (int i = 0; i < randomValue; i++)
        {
            if (transform.position.y <= -3.5f)
            {
                transform.position = new Vector2(transform.position.x, 1.75f);
            }
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

            // Adjust time interval to simulate slowing down
            if (i > Mathf.RoundToInt(randomValue * 0.95f))
            {
                timeInterval = 0.2f;
            }
            else if (i > Mathf.RoundToInt(randomValue * 0.75f))
            {
                timeInterval = 0.15f;
            }
            else if (i > Mathf.RoundToInt(randomValue * 0.5f))
            {
                timeInterval = 0.05f;
            }
            else if (i > Mathf.RoundToInt(randomValue * 0.25f))
            {
                timeInterval = 0.01f;
            }

            yield return new WaitForSeconds(timeInterval);
        }

        // Determine the stopped slot based on the final position
        float yPosition = transform.position.y;
        if (Mathf.Abs(yPosition - (-3.5f)) < 0.01f) { stoppedSlot = "Diamond"; }
        else if (Mathf.Abs(yPosition - (-2.75f)) < 0.01f) { stoppedSlot = "Crown"; }
        else if (Mathf.Abs(yPosition - (-2f)) < 0.01f) { stoppedSlot = "Melon"; }
        else if (Mathf.Abs(yPosition - (-1.25f)) < 0.01f) { stoppedSlot = "Bar"; }
        else if (Mathf.Abs(yPosition - (-0.5f)) < 0.01f) { stoppedSlot = "Seven"; }
        else if (Mathf.Abs(yPosition - (0.25f)) < 0.01f) { stoppedSlot = "Cherry"; }
        else if (Mathf.Abs(yPosition - (1f)) < 0.01f) { stoppedSlot = "Lemon"; }
        else if (Mathf.Abs(yPosition - (1.75f)) < 0.01f) { stoppedSlot = "Diamond"; }

        rowStopped = true; // Mark row as stopped
        Debug.Log($"Row {gameObject.name} stopped with slot: {stoppedSlot}");
    }
  
    private void OnDestroy()
    {
        
        GameControl.HandlePulled -= StartRotating; // Unsubscribe from the HandlePulled event
     
    }
}
