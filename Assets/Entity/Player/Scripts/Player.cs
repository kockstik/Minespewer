using UnityEngine;


public class Player : Minespewer
{
    public static Player shared { get; private set; }

    void Awake()
    {
        shared = this;
    }
}
