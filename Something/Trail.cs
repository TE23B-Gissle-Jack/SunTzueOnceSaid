using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class Trail
{
    List<Vector2> trail = new();
    int maxTrailLength = 300;
    int[] trailColors = new int[6];

    int size;

    public Trail(int size)
    {
        this.size = size;

        while (!trailColors.Contains(1))
        {
            for (int i = 0; i < trailColors.Length; i++)
            {
                trailColors[i] = Random.Shared.Next(2);// 1 or 0
            }
        }
    }

    public void Add(Vector2 position)
    {
        trail.Add(position);
    }

    public void update()
    {
        if (trail.Count > maxTrailLength) trail.RemoveAt(0);
    }

    public void Draw()
    {
        for (int i = 1; i < trail.Count; i++)
        {
            if (TrailBreak(trail[i],trail[i-1])) continue;

            float t = (i - 1) / (float)(trail.Count - 1); // 0..1 fade
            byte r = (byte)(t * 255 * trailColors[0] + (255 - 255 * trailColors[1]));
            byte g = (byte)(t * 255 * trailColors[2] + (255 - 255 * trailColors[3]));
            byte b = (byte)(t * 255 * trailColors[4] + (255 - 255 * trailColors[5]));

            Color fade = new Color(r, g, b, (byte)(255 * t));

            Raylib.DrawLineEx(trail[i - 1], trail[i], size, fade);
        }
    }

    public bool TrailBreak(Vector2 point1, Vector2 point2)
    {
        Vector2 case1 = new (9999,-9999);

        if (point1 == case1 || point2 == case1)
        {
            return true;
        }
        if (Vector2.Distance(point1,point2)>200)
        {
            return true;
        }

        return false;
    }
}
