using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class SAVE_savedColor
{
    //THIS CLASS STORES A RGBA COLOR FOR THE PURPOSE OF SAVING IT IN A SAVE FILE.

    public float[] ColorRGBA = new float[4];

    public SAVE_savedColor (float R, float G, float B, float A)
    {
        ColorRGBA[0] = R;
        ColorRGBA[1] = G;
        ColorRGBA[2] = B;
        ColorRGBA[3] = A;
    }


}
