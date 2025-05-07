using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class ArmSegment
{
    //controls for interacting with the segment via keyboard
    KeyboardKey[] controlset;
    //indicates if this segment is a "parent" or "child"
    string role;

    float g = -0.15f; //gravity
    float dragforce = 0.99f; //not used, but is used to simulate air resistance or such

    // Core physical properties
    public Vector2 position;  //Current endpoint position of the segment
    public Vector2 orgin;     //origin point (anchor or parent endpoint)
    int length;               //Length of the segment
    public double velocity;   //current velocity for angle change
    public double acelaration;//current angular acceleration
    double newAcelaration;    //temporary value for precomputed acceleration

    public Trail trail;

    public float mass = 10;//mass of the segment

    public ArmSegment conected;//refrace to parent or child if this is a double pendulum

    public double rotation = Random.Shared.Next(10); // starting rotation

    //color of the arm
    Color color = Color.Green;

    //refrance to the parents end point
    private Vector2 parentPosition;
    //a toggle for if the arms are drawn
    public bool showArm;

    //consturcor
    public ArmSegment(Vector2 orgin, int leangth, ArmSegment conected, int controls, string role,List<Trail> trailList)
    {
        this.length = leangth;
        this.conected = conected;
        this.role = role;

        if (conected != null)
        {
            orgin = conected.position;//sets the parents end point as orgin of this child
            color = Color.Gold;

            //makes a trail with thicknes of 5
            trail = new Trail(5);
            //adds the trail to the traillist
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
        if (conected == null)//if this is not double a pengulum
        {
            acelaration = (float)(g * Math.Sin(rotation) / length);
        }
        else
        {
            //calculation for the parents angular acceleration
            if (role == "parent")
            {
                double m2 = conected.mass, o2 = conected.rotation, v2 = conected.velocity, r2 = conected.length;
                double m1 = mass, o1 = rotation, v1 = velocity, r1 = length;

                double part1 = -g * (2 * m1 + m2) * Math.Sin(o1);
                double part2 = -m2 * g * Math.Sin(o1 - 2 * o2);
                double part3 = -2 * Math.Sin(o1 - o2) * m2;
                double part4 = v2 * v2 * r2 + v1 * v1 * r1 * Math.Cos(o1 - o2);
                double fullPart = part1 + part2 + part3 * part4;

                double divider = r1 * (2 * m1 + m2 - m2 * Math.Cos(2 * o1 - 2 * o2));

                newAcelaration = (-1) * (float)(fullPart / divider);
            }
            //calculation for the childs angular acceleration
            else if (role == "child")
            {
                double m1 = conected.mass, o1 = conected.rotation, v1 = conected.velocity, r1 = conected.length;
                double m2 = mass, o2 = rotation, v2 = velocity, r2 = length;

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
            //updates the position to the parents new end point
            parentPosition = conected.position;
            //updates the acceleration
            acelaration = newAcelaration;
        }

        if (role == "child")
        {
            //updates the childs orgin point
            orgin = parentPosition;
            //adds this position to the trail
            trail.Add(position);

            trail.update(); // Keep the trail short and sweet
        }

        //"player" input
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

        //updates the aceleration and velocity
        velocity += acelaration;
        rotation += velocity;

        //velocity *= dragforce; //slows down with time

        //makes the actual position change to the position it should be based on the angle and length
        position.X = (float)(length * Math.Sin(rotation) + orgin.X);
        position.Y = (float)(length * Math.Cos(rotation) + orgin.Y);

    }
    public void Draw()
    {
        if (role == "child"||conected==null);//???

        //draws the arm if the toggle is true
        if (showArm)
        {
            Raylib.DrawLineEx(orgin, position, 10, color);
            Raylib.DrawCircleV(orgin, 10, Color.Pink);
            Raylib.DrawCircleV(position, 10, Color.Red);
        }
    }
}


