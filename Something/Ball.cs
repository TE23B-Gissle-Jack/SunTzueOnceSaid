using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class Ball
{
    //constant acceleration
    float gravity = 0.1f;

    public Vector2 position;
    Vector2 velcotiy;

    Color color;
    //radius for the ball
    int size;

    //screen size
    Vector2 screen;

    //this balls trail
    public Trail trail;

    //constructor
    public Ball(int radius, Vector2 location,List<Trail> trailList, Vector2 window)
    {
        size = radius;
        position = location;
        velcotiy = new(Random.Shared.Next(1, 5), 0);

        trail = new Trail(size);
        trailList.Add(trail);
        color = new Color(0,0,0,0);//lazy

        screen = window;
    }

    public void Update()
    {
        //increses the velocity downward by gravity amount
        velcotiy.Y += gravity;
        //changes position based on velocity
        position += velcotiy;

        //checks if any part of the ball tuches bottom of screen
        if (position.Y > screen.Y - size)
        {
            //inverts the velocity
            velcotiy.Y = -velcotiy.Y;
            //puts the ball at the botton of the screen, in case it was going to fast and past it
            position.Y = screen.Y-size;
        }
        //checks if the ball is a bit past the border of the screen
        if (position.X > screen.X*1.1 + size)
        {
            //takes it to the other side, outside the screen
            position.X = -200;
            //gets its trail out of the way
            trail.Add(position+new Vector2(9999,-9999));
            //trail.Add(position+new Vector2(-200,-9999));
        }
        trail.update();
        // adds the curent position to the trail
        trail.Add(position);
    }
    //draws the ball
    public void Draw()
    {
        Raylib.DrawCircleV(position, size, color);
    }
}
