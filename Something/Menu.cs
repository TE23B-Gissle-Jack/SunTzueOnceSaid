using System;
//using System.Drawing;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class Menu
{
    Balls ballSims;
    PendjulumSim pendjulumSims;

    Vector2 screen;

    //if the menu is open
    bool active = false;
    List<Trail> trailList;

    //cosnstructor ;)
    public Menu(Balls ballSims, PendjulumSim pendjulumSims,Vector2 screen,List<Trail> trailList)
    {
        this.trailList = trailList;

        this.ballSims = ballSims;
        this.pendjulumSims = pendjulumSims;

        this.screen = screen;
    }

    public void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            active = !active;
        }
    }

    public void Draw()
    {
        if (active)
        {
            //draws a semi transparent box from top to botom on left side of screen
            Raylib.DrawRectangle(0,0,(int)screen.X/3,(int)screen.Y,new(50,50,50,200));
            TextBox("Menu",50,new(screen.X/6,50));

            TextBox("Inputs",30,new(screen.X/6,150));
            TextBox("Pause: Space.",15,new(screen.X/6,200));
            TextBox("Show arms: page down.",15,new(screen.X/6,225));
            TextBox("First arm: a;d.",15,new(screen.X/6,250));
            TextBox("Second arm: arrow left;right.",15,new(screen.X/6,275));

            TextBox("Pendgulum Simulation",30,new(screen.X/6,300));
            TextBox("Count",20,new(screen.X/6,325));

            TextBox(""+pendjulumSims.arms.Count,50,new(screen.X/6,350));
            if (Button(new(50,50),new(screen.X/6-70,350),"+"))
            {
                pendjulumSims.NewArm(false,new(400,400));
            }
            if (Button(new(50,50),new(screen.X/6+70,350),"-"))
            {
                pendjulumSims.Amputate();
            }

            TextBox("Ball Simulation",30,new(screen.X/6,500));
            TextBox("Ball Count",20,new(screen.X/6,525));

            TextBox(""+ballSims.balls.Count,50,new(screen.X/6,550));
            if (Button(new(50,50),new(screen.X/6-70,550),"+"))
            {
                ballSims.NewBall();
            }
            if (Button(new(50,50),new(screen.X/6+70,550),"-"))
            {
                ballSims.ViciouslyMurderBall();
            }

            TextBox("Ball Orbits Count",20,new(screen.X/6,625));

            TextBox(""+ballSims.orbitCount,50,new(screen.X/6,650));
            if (Button(new(50,50),new(screen.X/6-70,650),"+"))
            {
                ballSims.orbitCount++;
            }
            if (Button(new(50,50),new(screen.X/6+70,650),"-"))
            {
                if (ballSims.orbitCount>0)
                {
                    ballSims.orbitCount--;
                }
            }
        }
    }
    void TextBox(string input, int size, Vector2 positionCenter)
    {
        int textWidth = Raylib.MeasureText(input, size);
        Vector2 textPos = positionCenter - new Vector2(textWidth / 2f, 0);

        Raylib.DrawText(input, (int)textPos.X, (int)textPos.Y, size, Color.White);
    }

    bool Button(Vector2 size, Vector2 position, string text)
    {
        Rectangle button = new Rectangle(position-new Vector2(size.X/2,0),size);
        Color color = Color.DarkBlue;
        //checks if mouse is over button
        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(),button))
        {
            color = Color.Blue;
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return true;
            }
        }
        
        Raylib.DrawRectangleRec(button,color);
        TextBox(text,(int)size.X,position);
        return false;
    } 
}
