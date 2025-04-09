using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class ArmSegment
{
    KeyboardKey[] controlset;
    string role;

    float gravity = -0.15f;
    float dragforce = 0.99f;

    public Vector2 position;
    Vector2 orgin;
    int leangth;
    float velocity;
    float acelaration;

    public float mass = 10;

    public ArmSegment conected;

    public double rotation = Math.PI / Random.Shared.Next(1, 6);

    Color color = Color.Green;

    public ArmSegment(Vector2 orgin, int leangth, ArmSegment conected, int controls, string role)
    {
        this.leangth = leangth;
        this.conected = conected;
        if (conected != null)
        {
            orgin = conected.position;

            //makes the parent have this a conected
            conected.conected = this;
        }
        else this.orgin = orgin;

        if (controls == 1)
        {
            controlset = [KeyboardKey.A, KeyboardKey.D];
        }
        else controlset = [KeyboardKey.Left, KeyboardKey.Right];
    }

    public void Update()
    {
        if (role == "child")
        {
            orgin = conected.position;
        }

        if (conected == null)
        {
            acelaration = (float)(gravity * Math.Sin(rotation) / leangth);
            if (Raylib.IsKeyDown(controlset[1]))
            {
                acelaration += 0.001f;
            }
            else if (Raylib.IsKeyDown(controlset[0]))
            {
                acelaration -= 0.001f;
            }
        }
        else
        {
            if (role == "parent")
            {
                double part1 = -gravity * (2 * mass + conected.mass) * Math.Sin(rotation);
                double part2 = -conected.mass * gravity * Math.Sin(rotation - 2 * conected.rotation);
                double part3 = -2 * Math.Sin(rotation - conected.rotation) * conected.mass;
                double part4 = conected.velocity * conected.velocity * conected.leangth + velocity * velocity * leangth * Math.Cos(rotation - conected.rotation);
                double fullPart = part1 + part2 + part3 * part4;

                double divider = leangth * (2 * mass + conected.mass - conected.mass * Math.Cos(2 * rotation - 2 * conected.rotation));

                acelaration = (float)(fullPart / divider);
            }
            else
            {
                double part1 = 2 * Math.Sin(conected.rotation-rotation);
                double part2 = conected.velocity * conected.velocity * conected.leangth * (conected.mass + mass);
                double part3 = gravity * (conected.mass + mass) * Math.Cos(conected.rotation);
                double part4 = velocity * velocity * leangth * mass * Math.Cos(conected.rotation - rotation);
                double fullPart = part1 * (part2 + part3 + part4);

                double divider = leangth * (2 * conected.mass * mass - mass * Math.Cos(2 * conected.rotation - 2 * rotation));

                acelaration = (float)(fullPart/divider);
            }
        }

        velocity += acelaration;
        rotation += velocity;

        //velocity *= dragforce;


        position.X = (float)(leangth * Math.Sin(rotation) + orgin.X);
        position.Y = (float)(leangth * Math.Cos(rotation) + orgin.Y);
    }
    public void Draw()
    {
        Raylib.DrawLineEx(orgin, position, 10, color);
    }
}
