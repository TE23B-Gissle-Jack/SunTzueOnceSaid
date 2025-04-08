using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class ArmSegment
{
    KeyboardKey[] controlset;

    float gravity = -0.15f;
    float dragforce = 0.99f;

    public Vector2 position;
    Vector2 orgin;
    int leangth;
    float velocity;
    float acelaration;

    ArmSegment conected;

    double rotation = Math.PI/Random.Shared.Next(1,6);

    Color color = Color.Green;

    public ArmSegment(Vector2 orgin, int leangth, ArmSegment conected, int controls)
    {
        this.leangth = leangth;
        this.conected = conected;
        if (conected!=null)
        {
            orgin = conected.position;
        }
        else this.orgin = orgin;

        if (controls == 1)
        {
            controlset = [KeyboardKey.A,KeyboardKey.D];
        }
        else controlset = [KeyboardKey.Left,KeyboardKey.Right];
    }

    public void Update()
    {   
        if (conected != null)
        {
            orgin = conected.position;
        }

        acelaration = (float)(gravity*Math.Sin(rotation)/leangth);
        if (Raylib.IsKeyDown(controlset[1]))
        {
            acelaration+=0.001f;
        }
        else if(Raylib.IsKeyDown(controlset[0]))
        {
            acelaration-=0.001f;
        }

        velocity+=acelaration;
        rotation+=velocity;
        
        velocity*=dragforce;
        

        position.X = (float)(leangth * Math.Sin(rotation) + orgin.X);
        position.Y = (float)(leangth * Math.Cos(rotation) + orgin.Y);
    }
    public void Draw()
    {
        Raylib.DrawLineEx(orgin,position,10,color);
    }
}
