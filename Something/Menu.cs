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
    bool showPromt = true;
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
            showPromt = false;
        }
    }

    public void Draw()
    {
        if (showPromt)
        {
            TextBox("Press [ESC] to open menu!!!!",40,new(screen.X/2,50),new(150,150,150,200));
        }
        if (active)
        {
            //draws a semi transparent box from top to botom on left side of screen
            Raylib.DrawRectangle(0,0,(int)screen.X/3,(int)screen.Y,new(50,50,50,200));
            TextBox("Menu",50,new(screen.X/6,50),Color.White);

            TextBox("Inputs",30,new(screen.X/6,150),Color.White);
            TextBox("Pause: Space.",15,new(screen.X/6,200),Color.White);
            TextBox("Show arms: page down.",15,new(screen.X/6,225),Color.White);
            TextBox("First arm: a;d.",15,new(screen.X/6,250),Color.White);
            TextBox("Second arm: arrow left;right.",15,new(screen.X/6,275),Color.White);

            TextBox("Pendgulum Simulation",30,new(screen.X/6,325),Color.White);
            TextBox("Count",20,new(screen.X/6,350),Color.White);

            TextBox(""+pendjulumSims.arms.Count,50,new(screen.X/6,375),Color.White);
            if (Button(new(50,50),new(screen.X/6-70,375),"+",Color.Blue))
            {
                pendjulumSims.NewArm(true,new(400,400));
            }
            if (Button(new(50,50),new(screen.X/6+70,375),"-",Color.Blue))
            {
                pendjulumSims.Amputate();
            }

            TextBox("Ball Simulation",30,new(screen.X/6,500),Color.White);
            TextBox("Ball Count",20,new(screen.X/6,525),Color.White);

            TextBox(""+ballSims.balls.Count,50,new(screen.X/6,550),Color.White);
            if (Button(new(50,50),new(screen.X/6-70,550),"+",Color.Blue))
            {
                ballSims.NewBall();
            }
            if (Button(new(50,50),new(screen.X/6+70,550),"-",Color.Blue))
            {
                ballSims.ViciouslyMurderBall(0);
            }

            TextBox("Ball Orbits Count",20,new(screen.X/6,625),Color.White);
            TextBox(""+ballSims.orbitCount,50,new(screen.X/6,650),Color.White);
            if (Button(new(50,50),new(screen.X/6-70,650),"+",Color.Blue))
            {
                ballSims.orbitCount++;
            }
            if (Button(new(50,50),new(screen.X/6+70,650),"-",Color.Blue))
            {
                if (ballSims.orbitCount>0)
                {
                    ballSims.orbitCount--;
                }
            }

            if (Button(new(250,70),new(screen.X/6,850),"Quit",Color.Red))
            {
                Environment.Exit(1);
            }
        }
    }
    void TextBox(string input, int size, Vector2 positionCenter,Color color)
    {
        int textWidth = Raylib.MeasureText(input, size);
        Vector2 textPos = positionCenter - new Vector2(textWidth / 2f, 0);

        Raylib.DrawText(input, (int)textPos.X, (int)textPos.Y, size, color);
    }

    bool Button(Vector2 size, Vector2 position, string text, Color color)
    {
        Rectangle button = new Rectangle(position-new Vector2(size.X/2,0),size);
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
        TextBox(text,(int)size.Y/2,new(position.X,position.Y+size.Y/4),Color.White);
        return false;
    } 
}
