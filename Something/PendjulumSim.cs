using System;
using System.Numerics;
using Raylib_cs; //  använder raylib_cs (från lukas)

namespace Something;

public class PendjulumSim
{
    //a list that holds pairs of arm segments (double pendulums)
    public List<ArmSegment[]> arms = new List<ArmSegment[]>();

    //toggle for whether the game is pasued
    bool paused = false;

    //holds the origin points for the first arm of each pendulum pair
    public List<Vector2> orgins;

    Vector2 screen;
    List<Trail> trailList;
    bool random;

    //constructor
    public PendjulumSim(int amt, int leangth, Vector2 screen, bool random, List<Vector2> orgins, List<Trail> trailList)
    {
        //stores the origins provided when creating the simulation
        this.orgins = orgins;
        this.screen = screen;
        this.trailList = trailList;
        this.random = random;

        //loop to make the specified amount of double penjulums
        for (int i = 0; i < amt; i++)
        {
            NewArm(false,orgins[i]);
        }
    }

    public void Update()
    {
        //pasues the game if space is preased
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            //changes the state of paused
            paused = !paused;
        }

        //updates each arm pair
        for (int i = 0; i < arms.Count; i++)
        {
            //if there are orgins, then they are enforced again / in case they have moved
            if (orgins != null)
            {
                arms[i][0].orgin = orgins[i];
            }

            //runs calculations and update while game is not paused
            if (!paused)
            {
                //runs calculations for parent and child first without updating values
                //this is for the calculations are dependet of echothers original values
                arms[i][0].Calc();
                arms[i][1].Calc();

                //updates the arms angles and velocitys based on calculations
                arms[i][0].Update();
                arms[i][1].Update();
            }

            //draws all arms
            Draw();
        }
    }
    public void Draw()
    {
        //for every arm draws all arm segments
        for (int i = 0; i < arms.Count; i++)
        {
            arms[i][0].Draw();
            arms[i][1].Draw();
        }
    }

    public void NewArm(bool independant,Vector2 orgin)
    {
        if(independant)orgins.Add(orgin);
        
        Vector2 position;

        //not nececary
        int armLeangth = 100;

        //if parameter random is true then the arms orgins are random
        if (random)
        {
            //makes the position random and uses screen to limit max and takes the arm legth into acount
            //*2 becuse there are two arm segments in each arm
            int x = Random.Shared.Next(armLeangth * 2, (int)screen.X - armLeangth * 2);
            int y = Random.Shared.Next(armLeangth * 2, (int)screen.Y - armLeangth * 2);
            position = new Vector2(x, y);
        }
        //use the provided origin from the list
        else position = orgin;


        //makes the first arm using the position/orgin and gives it a role of parent
        ArmSegment arm = new ArmSegment(position, 100, null, 1, "parent", trailList);
        //makes another amr as child wich orgin is overtien later to be parents moving point
        ArmSegment arm2 = new ArmSegment(position, 100, arm, 2, "child", trailList);

        //adds the two arm segments as an array(parent first) in the list arms
        arms.Add([arm, arm2]);
    }
}

