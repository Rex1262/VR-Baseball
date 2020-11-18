using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirroringScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isRight = true;
    public static int mode = 0;

    public void SetLeft()
    {
        isRight = false;
    }

    public void SetRight()
    {
        isRight = true;
    }
}
