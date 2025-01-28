using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{
    public Reel[] reel; // Array of reels
    private bool startSpin; // Ensures spins are not interrupted
    public string color; // Target color for aligning symbols

    // Start is called before the first frame update
    void Start()
    {
        startSpin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startSpin) // Prevents new spins while reels are spinning
        {
            if (Input.GetKeyDown(KeyCode.K)) // Spin trigger
            {
                startSpin = true;
                StartCoroutine(Spinning());
            }
        }
    }

    private IEnumerator Spinning()
    {
        // Start spinning all reels
        foreach (Reel spinner in reel)
        {
            spinner.StartSpinning();
        }

        // Stop each reel after a random delay with gradual slowdown
        for (int i = 0; i < reel.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(1,3));
            reel[i].SlowDownAndStop();

            // Wait until the reel has fully stopped
            while (reel[i].spin)
            {
                yield return null;
            }

            reel[i].AlignMiddle(color); // Align the reel to the target color
        }

        // Allow spins to start again
        startSpin = false;
    }
}