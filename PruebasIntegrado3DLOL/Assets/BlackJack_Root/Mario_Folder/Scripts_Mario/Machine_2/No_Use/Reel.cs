using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    // Controls whether the reel spins
    public bool spin;

    // Speed at which the reel spins
    private float speed = 1500f;
    private float targetSpeed = 0f;
    private float decelerationRate = 500f; // How fast the reel slows down

    // Start is called before the first frame update
    void Start()
    {
        spin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            foreach (Transform image in transform)
            {
                // Move the image downward
                image.transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);

                // Reset position when the image moves out of view
                if (image.transform.localPosition.y <= -300f)
                {
                    float resetPosition = 300f; // Adjust this value based on reel dimensions
                    image.transform.localPosition = new Vector3(
                        image.transform.localPosition.x,
                        image.transform.localPosition.y + resetPosition * 2,
                        image.transform.localPosition.z
                    );
                }
            }

            // Slow down the reel speed gradually if targetSpeed is set
            if (speed > targetSpeed)
            {
                speed -= decelerationRate * Time.deltaTime;
                if (speed <= targetSpeed)
                {
                    speed = targetSpeed;
                    spin = false; // Stop spinning when speed reaches the target
                }
            }
        }
    }

    // Method to start spinning the reel with a specific speed
    public void StartSpinning()
    {
        spin = true;
        speed = 1500f; // Reset to full speed
    }

    // Method to initiate slowing down the reel
    public void SlowDownAndStop()
    {
        targetSpeed = 0f; // Target speed to stop completely
    }

    // Aligns the images randomly after the reel stops
    public void AlignMiddle(string targetColor)
    {
        List<int> middlePosition = new List<int> { 0 }; // Center position
        List<int> positions = new List<int> { 200, 100, -100, -200, -300 };

        foreach (Transform image in transform)
        {
            if (image.name.Equals(targetColor) && middlePosition.Count > 0)
            {
                image.transform.localPosition = new Vector3(
                    image.transform.localPosition.x,
                    middlePosition[0],
                    image.transform.localPosition.z
                );
                middlePosition.RemoveAt(0);
            }
            else
            {
                int randomIndex = Random.Range(0, positions.Count);
                image.transform.localPosition = new Vector3(
                    image.transform.localPosition.x,
                    positions[randomIndex],
                    image.transform.localPosition.z
                );
                positions.RemoveAt(randomIndex);
            }
        }
    }
}
