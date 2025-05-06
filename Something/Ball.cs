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

    Vector2 screen;

    Trail trail;

    public Ball(int radius, Vector2 location,List<Trail> trailList, Vector2 window)
    {
        size = radius;
        position = location;
        velcotiy = new(Random.Shared.Next(1, 5), 0);

        trail = new Trail(size);
        trailList.Add(trail);
        color = new Color(0,0,0,0);

        screen = window;
    }

    public void Update()
    {
        velcotiy.Y += gravity;
        position += velcotiy;

        if (position.Y > screen.Y - size)
        {
            velcotiy.Y = -velcotiy.Y;
            position.Y = screen.Y-size;
        }
        if (position.X > screen.X*1.1 + size)
        {
            position.X = -200;
            trail.Add(position+new Vector2(9999,-9999));
            trail.Add(position+new Vector2(-200,-9999));
        }

        trail.update();
        trail.Add(position);
    }
    public void Draw()
    {
        Raylib.DrawCircleV(position, size, color);
        // Trail();
    }

    // void Trail()
    // {
    //     for (int i = 1; i < trail.Count; i++)
    //     {
    //         float t = (i - 1) / (float)(trail.Count - 1); // 0..1 fade
    //         byte r = (byte)(t * 255 * trailColors[0] + (255 - 255 * trailColors[1]));
    //         byte g = (byte)(t * 255 * trailColors[2] + (255 - 255 * trailColors[3]));
    //         byte b = (byte)(t * 255 * trailColors[4] + (255 - 255 * trailColors[5]));

    //         Color fade = new Color(r, g, b, (byte)(255 * t));

    //         Raylib.DrawLineEx(trail[i - 1], trail[i], size, fade);
    //     }
    // }
}
