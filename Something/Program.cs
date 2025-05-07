using System.Numerics;
using Raylib_cs;
using Something;

//makes the window called "Slap" with a target fps of 60
Raylib.InitWindow(1000, 1000, "Slap");
Raylib.SetTargetFPS(60);

//stores the screen size in a vector2 for later use
Vector2 screen = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

//creats a list of Trails to keep them in one place
List<Trail> trailList = new List<Trail>();


//Initializes a pendulum simulation with the following parameters:
//number of pendulums, length of each arm, screen dimensions, whether positions are random,
//optional origin list, and a reference to the trail list
PendjulumSim sim1 = new PendjulumSim(0, 100, screen, true, null, trailList);

// Initializes a ball simulation with the following parameters:
// number of balls, number of pendulums orbiting each ball, the trail list, and screen size
Balls hugo = new Balls(1, 3, trailList, screen);

// Main game loop - runs while the window is open
while (!Raylib.WindowShouldClose())
{
    //clears the screen and begins the drawing proces
    Raylib.ClearBackground(Color.Black);
    Raylib.BeginDrawing();

    //updates both sims
    sim1.Update();
    hugo.update();

    //draws all the trails
    DrawTrails();

    Raylib.EndDrawing();
}

void DrawTrails()
{
    //loops though every trail in trailList and draws them
    for (int i = 0; i < trailList.Count; i++)
    {
        trailList[i].Draw();
    }
}

public class PendjulumSim
{
    //a list that holds pairs of arm segments (double pendulums)
    public List<ArmSegment[]> arms = new List<ArmSegment[]>();

    //toggle for whether the game is pasued
    bool paused = false;

    //holds the origin points for the first arm of each pendulum pair
    public List<Vector2> orgins;

    //constructor
    public PendjulumSim(int amt, int leangth, Vector2 screen, bool random, List<Vector2> orgins, List<Trail> trailList)
    {
        //stores the origins provided when creating the simulation
        this.orgins = orgins;

        //loop to make the specified amount of double penjulums
        for (int i = 0; i < amt; i++)
        {
            Vector2 position;

            //not nececary
            int armLeangth = leangth;

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
            else position = orgins[i];


            //makes the first arm using the position/orgin and gives it a role of parent
            ArmSegment arm = new ArmSegment(position, 100, null, 1, "parent", trailList);
            //makes another amr as child wich orgin is overtien later to be parents moving point
            ArmSegment arm2 = new ArmSegment(position, 100, arm, 2, "child", trailList);

            //adds the two arm segments as an array(parent first) in the list arms
            arms.Add([arm, arm2]);
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

}

public class Balls
{
    //creats a list of balls in this sim
    List<Ball> balls = new List<Ball>();

    //the list for the arms that are the orbits around the balls
    List<Vector2> ballArms = new List<Vector2>();
    //the simulation for the orbits
    PendjulumSim orbits;

    //constructor
    public Balls(int amt, int armCount, List<Trail> trailList, Vector2 screen)
    {
        //adds balls to balls for the amount specified
        for (int i = 0; i < amt; i++)
        {
            //adds a ball att a random posistion in the upper half of the screen
            balls.Add(new Ball(10, new(Random.Shared.Next((int)screen.X), Random.Shared.Next((int)screen.X / 2)), trailList, screen));
            //adds the orbits orgins for later creation of the arms
            for (int j = 0; j < armCount; j++)
            {
                ballArms.Add(balls[i].position);
            }
        }
        //creats a new penjulum sim with the orgins of the balls positions
        orbits = new PendjulumSim(armCount * amt, 20, screen, false, ballArms, trailList);
    }

    //updates the balls and orbits
    public void update()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            //updates the curent ball
            balls[i].Update();
            //updates the orbits of the ball in question
            for (int j = 0; j < ballArms.Count / balls.Count; j++)
            {
                //if the distance betwen the last x position and the current exedes 100 then logic happens
                if ((ballArms[i].X - balls[i].position.X) > 100)
                {
                    //adds a position in the trail all orbits of the balls
                    //this is to make it so the trail doesnt become a line over the screen when the ball teleports
                    for (int k = 0; k < orbits.arms.Count; k++)
                    {
                        orbits.arms[k][1].trail.Add(new Vector2(9999, -9999));
                    }
                }
                //updates the local orgins of the arms
                ballArms.RemoveAt(0);
                ballArms.Add(balls[i].position);
            }
        }

        //acculy updates the orgins of the orbits
        orbits.orgins = ballArms;
        //does a update of the penjulum sim
        orbits.Update();

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
}
