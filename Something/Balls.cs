using System;
using System.Numerics;
using Raylib_cs;

namespace Something;

public class Balls
{
    //creats a list of balls in this sim
    public List<Ball> balls = new List<Ball>();

    //the list for the arms that are the orbits around the balls
    List<List<Vector2>> ballArms = new List<List<Vector2>>();
    //the simulation for the orbits
    List<PendjulumSim> orbits = new List<PendjulumSim>();

    Vector2 screen;
    public int orbitCount;//arm count per ball
    List<Trail> trailList;
    List<Trail> deadTrails;

    //constructor
    public Balls(int amt, int armCount, List<Trail>[] trailLists, Vector2 screen)
    {
        this.screen = screen;
        this.orbitCount = armCount;
        this.trailList = trailLists[0];
        this.deadTrails = trailLists[1];

        //adds balls to balls for the amount specified
        for (int i = 0; i < amt; i++)
        {
            //adds a ball att a random posistion in the upper half of the screen
            balls.Add(new Ball(10, new(Random.Shared.Next((int)screen.X), Random.Shared.Next((int)screen.Y / 2)), trailList, screen));
            //adds the orbits orgins for later creation of the arms
            ballArms.Add(new List<Vector2>());
            for (int j = 0; j < armCount; j++)
            {
                ballArms[i].Add(balls[i].position);
            }
             //creats a new penjulum sim with the orgins of the balls positions
            orbits.Add(new PendjulumSim(armCount, 20, screen, false, ballArms[i], trailLists));
        }  
    }

    //updates the balls and orbits
    public void update()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            //updates the curent ball
            balls[i].Update();
            
            //updates the orbits of the ball in question
            for (int j = 0; j < orbitCount; j++)
            {
                //if the distance betwen the last x position and the current exedes 100 then logic happens
                // if ((ballArms[i][0].X - balls[i].position.X) > 100)
                // {
                //     //adds a position in the trail all orbits of the balls
                //     //this is to make it so the trail doesnt become a line over the screen when the ball teleports
                //     for (int k = 0; k < ballArms.Count / balls.Count; k++)
                //     {
                //         orbits[i].arms[k][1].trail.Add(new Vector2(9999, -9999));
                //     }
                // }
                //updates the local orgins of the arms
                ballArms[i].RemoveAt(0);
                ballArms[i].Add(balls[i].position);
            }
            //acculy updates the orgins of the orbits
            orbits[i].orgins = ballArms[i];

            //incresses the orbit count if there are said to be more in this sim than in the acual orbit sim
            if (orbits[i].arms.Count<orbitCount)
            {
                orbits[i].NewArm(false,balls[i].position);
            }
            else if(orbits[i].arms.Count>orbitCount)
            {
                orbits[i].Amputate();
            }
            //does a update of the penjulum sim of this ball
            orbits[i].Update();
        }

        //draws all the balls
        Draw();
    }
    public void Draw()
    {
        //loops though all the balls and draws them
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].Draw();
        }
    }
    public void NewBall()
    {
        List<Vector2> alalathbar = new List<Vector2>();
        Vector2 position = new Vector2(Random.Shared.Next((int)screen.X), Random.Shared.Next((int)screen.Y / 2));
            //adds a ball at a random posistion in the upper half of the screen
            balls.Add(new Ball(10,position , trailList, screen));
            orbits.Add(new PendjulumSim(0, 20, screen, false, new List<Vector2>(), [trailList,deadTrails]));
            ballArms.Add(new List<Vector2>());
            //adds the orbits orgins for later creation of the arms
            for (int j = 0; j < orbitCount; j++)
            {
                ballArms[ballArms.Count-1].Add(balls[balls.Count-1].position);
                orbits[orbits.Count-1].NewArm(true,balls[balls.Count-1].position);
            }
        
        //creats a new penjulum sim with the orgins of the balls positions
    }
    public void ViciouslyMurderBall(int victim)
    {
        for (int i = 0; i < orbitCount; i++)
        {
            orbits[victim].Amputate();
        }
        deadTrails.Add(balls[victim].trail);
        orbits.RemoveAt(victim);
        trailList.Remove(balls[victim].trail);
        balls.RemoveAt(victim);
        ballArms.RemoveAt(victim);
    }
}

