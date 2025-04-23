using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class Ball
{
    float gravity = 0.1f;

    public Vector2 position;
    Vector2 velcotiy;

    Color color;
    int size;

    List<Vector2> trail = new();
    int maxTrailLength = 300;
    int[] trailColors = new int[6];

    public Ball(int radius, Vector2 location)
    {
        size = radius;
        position = location;
        velcotiy = new(Random.Shared.Next(1, 5), 0);

        while (!trailColors.Contains(1))
        {
            for (int i = 0; i < trailColors.Length; i++)
            {
                trailColors[i] = Random.Shared.Next(2);// 1 or 0
            }
        }
        color = new Color(trailColors[0], trailColors[2], trailColors[4]);
    }

    public void Update()
    {
        velcotiy.Y += gravity;
        position += velcotiy;

        if (position.Y > 1000 - size)
        {
            velcotiy.Y = -velcotiy.Y;
            position.Y = 1000-size;
        }
        if (position.X < 0 + size || position.X > 1000 - size)
        {
            velcotiy.X = -velcotiy.X;
        }

        trail.Add(position);
        if (trail.Count > maxTrailLength) trail.RemoveAt(0);
    }
    public void Draw()
    {
        Raylib.DrawCircleV(position, size, color);
        Trail();
    }

    void Trail()
    {
        for (int i = 1; i < trail.Count; i++)
        {
            float t = (i - 1) / (float)(trail.Count - 1); // 0..1 fade
            byte r = (byte)(t * 255 * trailColors[0] + (255 - 255 * trailColors[1]));
            byte g = (byte)(t * 255 * trailColors[2] + (255 - 255 * trailColors[3]));
            byte b = (byte)(t * 255 * trailColors[4] + (255 - 255 * trailColors[5]));

            Color fade = new Color(r, g, b, (byte)(255 * t));

            Raylib.DrawLineEx(trail[i - 1], trail[i], size, fade);
        }
    }
}
