using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float raycastRange = 0.15f;
    public float raycastOffset = 0.6f;

    public bool isFixed;
    public bool isConnected;
    
    public BubbleColor bubbleColor;
}

public enum BubbleColor
{
    Blue, Yellow, Red, Purple, Bomb
}