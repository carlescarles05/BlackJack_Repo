using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.left * DinoManager.Instance.GetScrollSpeed() * Time.deltaTime);
    }

}
