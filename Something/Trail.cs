using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class Trail
{
    List<Vector2> trail = new();
    int maxTrailLength = 300;
    int[] trailColors = new int[6];

    public Trail()
    {
        while (!trailColors.Contains(1))
        {
            for (int i = 0; i < trailColors.Length; i++)
            {
                trailColors[i] = Random.Shared.Next(2);// 1 or 0
            }
        }
    }
    public void update()
    {
        if (trail.Count > maxTrailLength) trail.RemoveAt(0);
    }
}
