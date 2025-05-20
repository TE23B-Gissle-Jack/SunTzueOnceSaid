using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class Trail
{
    //Stores the positions that make up the trail
    public List<Vector2> trail = new();

    //maximum number of points allowed in the trail
    int maxTrailLength = 300;

    //Randomly generated color channel toggles (0 or 1) used for coloring the trail (R, G, B, R, G, B)
    int[] trailColors = new int[6];

    //thickness of the trail lines
    int size;

    //Constructor
    public Trail(int size)
    {
        this.size = size;

        //Randomly generate trail color toggles until at least one color component is active
        while (!trailColors.Contains(1))
        {
            for (int i = 0; i < trailColors.Length; i++)
            {
                trailColors[i] = Random.Shared.Next(2); // 0 or 1
            }
        }
    }

    //adds a new position to the trail
    public void Add(Vector2 position)
    {
        trail.Add(position);
        if (trail.Count > maxTrailLength) trail.RemoveAt(0);
    }

    //keeps the trail length by removing the oldest points if it exceeds the max limit
    public void update()
    {
        
    }

    //Draws the trail using fading color
    public void Draw()
    {
        for (int i = 1; i < trail.Count; i++)
        {
            //skip drawing trail segments that are visually "broken"
            if (TrailBreak(trail[i], trail[i - 1])) continue;

            //Compute a value between 0 and 1 for fading
            float t = (i - 1) / (float)(trail.Count - 1);

            //Calculate color values based on fade and the trailColors toggles
            byte r = (byte)(t * 255 * trailColors[0] + (255 - 255 * trailColors[1]));
            byte g = (byte)(t * 255 * trailColors[2] + (255 - 255 * trailColors[3]));
            byte b = (byte)(t * 255 * trailColors[4] + (255 - 255 * trailColors[5]));

            //Combine color components and alpha into a fade color
            Color fade = new Color(r, g, b, (byte)(255 * t));

            //Draw a line segment between two trail points with the color and size
            Raylib.DrawLineEx(trail[i - 1], trail[i], size, fade);
        }
    }
    
    //decides whether the trail segment is to far apart (broken)
    public bool TrailBreak(Vector2 point1, Vector2 point2)
    {
        Vector2 breakMarker = new(9999, -9999);

        //Break if either point is a marker indicating a break
        if (point1 == breakMarker || point2 == breakMarker)
        {
            return true;
        }

        //break if the distance between the points is abnormally large
        if (Vector2.Distance(point1, point2) > 200)
        {
            return true;
        }
        //otherwise this bool is false
        return false;
    }
}
