using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class ArmSegment
{
    KeyboardKey[] controlset;
    string role;

    float g = -0.15f; //gravity
    float dragforce = 0.99f;

    public Vector2 position;
    public Vector2 orgin;
    int leangth;
    public double velocity;
    public double acelaration;
    double newAcelaration;

    public Trail trail;

    public float mass = 10;

    public ArmSegment conected;

    public double rotation = Random.Shared.Next(10);

    Color color = Color.Green;

    private Vector2 parentPosition;
    public bool showArm;

    public ArmSegment(Vector2 orgin, int leangth, ArmSegment conected, int controls, string role,List<Trail> trailList)
    {
        this.leangth = leangth;
        this.conected = conected;
        this.role = role;

        if (conected != null)
        {
            orgin = conected.position;//sets the parents moving point as orgin of this child
            color = Color.Gold;

            trail = new Trail(5);
            trailList.Add(trail);

            //makes the parent have this as conected
            conected.conected = this;
        }
        else this.orgin = orgin; //if this is the orgin pendjulum then the orgin point is fixed

        //sets the controls
        if (controls == 1)
        {
            controlset = [KeyboardKey.A, KeyboardKey.D];
        }
        else controlset = [KeyboardKey.Left, KeyboardKey.Right];
    }
    public void Calc()
    {
        if (conected == null)//if this is not double pengulum
        {
            acelaration = (float)(g * Math.Sin(rotation) / leangth);
        }
        else
        {
            if (role == "parent")
            {
                double m2 = conected.mass, o2 = conected.rotation, v2 = conected.velocity, r2 = conected.leangth;
                double m1 = mass, o1 = rotation, v1 = velocity, r1 = leangth;

                double part1 = -g * (2 * m1 + m2) * Math.Sin(o1);
                double part2 = -m2 * g * Math.Sin(o1 - 2 * o2);
                double part3 = -2 * Math.Sin(o1 - o2) * m2;
                double part4 = v2 * v2 * r2 + v1 * v1 * r1 * Math.Cos(o1 - o2);
                double fullPart = part1 + part2 + part3 * part4;

                double divider = r1 * (2 * m1 + m2 - m2 * Math.Cos(2 * o1 - 2 * o2));

                newAcelaration = (-1) * (float)(fullPart / divider);
            }
            else if (role == "child")
            {
                double m1 = conected.mass, o1 = conected.rotation, v1 = conected.velocity, r1 = conected.leangth;
                double m2 = mass, o2 = rotation, v2 = velocity, r2 = leangth;

                double part1 = 2 * Math.Sin(o1 - o2);
                double part2 = v1 * v1 * r1 * (m1 + m2);
                double part3 = g * (m1 + m2) * Math.Cos(o1);
                double part4 = v2 * v2 * r2 * m2 * Math.Cos(o1 - o2);
                double fullPart = part1 * (part2 + part3 + part4);

                double divider = r2 * ((2 * m1) + m2 - (m2 * Math.Cos(2 * o1 - 2 * o2)));

                newAcelaration = (-1) * (float)(fullPart / divider);
            }
        }
    }

    public void Update()
    {
        //does this if double pengulum
        if (conected != null)
        {
            parentPosition = conected.position;
            acelaration = newAcelaration;
        }

        if (role == "child")
        {
            orgin = parentPosition;
            trail.Add(position);

            trail.update(); // Keep the trail short and sweet
        }

        if (Raylib.IsKeyDown(controlset[1]))
        {
            acelaration += 0.001f;
        }
        else if (Raylib.IsKeyDown(controlset[0]))
        {
            acelaration -= 0.001f;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.PageDown))
        {
            showArm = !showArm;
        }

        velocity += acelaration;
        rotation += velocity;

        //velocity *= dragforce; //slows down with time


        position.X = (float)(leangth * Math.Sin(rotation) + orgin.X);
        position.Y = (float)(leangth * Math.Cos(rotation) + orgin.Y);

    }
    public void Draw()
    {
        if (role == "child"||conected==null);

        if (showArm)
        {
            Raylib.DrawLineEx(orgin, position, 10, color);
            Raylib.DrawCircleV(orgin, 10, Color.Pink);
            Raylib.DrawCircleV(position, 10, Color.Red);
        }
    }
}


