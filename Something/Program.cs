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


//creats a penjulum sim with the creations perameters:
// how many penjulums, how log each arm in the pendjulum, if the positions are random,
// a list for position used by ball sim and the trail list for the trails in the sim to put into
PendjulumSim sim1 = new PendjulumSim(0, 100, screen, true, null, trailList);

//Creats a ball sim with parameters of: ball count, if and how many pendjulm each arm has orbeting it,
//Trail list again to put the balls and orbits trails into and the screen for the ball to refrence when hitning walls/floor
Balls hugo = new Balls(1, 3, trailList, screen);

//active aslong as the window is not closed
while (!Raylib.WindowShouldClose())
{
    //clears the background and begins the drawing proces
    Raylib.ClearBackground(Color.Black);
    Raylib.BeginDrawing();

    //updates both sims
    sim1.Update();
    hugo.update();

    //draws the trails in traillist
    DrawTrails();

    Raylib.EndDrawing();
}

void DrawTrails()
{
    //draw every trail in trailList
    for (int i = 0; i < trailList.Count; i++)
    {
        trailList[i].Draw();
    }
}

public class PendjulumSim
{
    //a list contaning arrays of arm pairs
    public List<ArmSegment[]> arms = new List<ArmSegment[]>();

    // it is what it is
    bool paused = false;

    // used for the orgin points of the first arm of the arm array
    public List<Vector2> orgins;

    //constructor
    public PendjulumSim(int amt, int leangth, Vector2 screen, bool random, List<Vector2> orgins, List<Trail> trailList)
    {
        //sets the orgins to the once put in while making the inctance of PenjulumSim
        this.orgins = orgins;

        //for the amount of double penjulums
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
            //makes the orgin for the arm be that of the coresponding orgin in orgins
            else position = orgins[i];


            //makes the first arm using the position/orgin and gives it a role of parent
            ArmSegment arm = new ArmSegment(position, 100, null, 1, "parent", trailList);
            //makes another amr as child wich orgin is overtien later to be parents moving point
            ArmSegment arm2 = new ArmSegment(position, 100, arm, 2, "child", trailList);

            //adds the two arms as an array,parent first, in the list arms
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

        //for all arm pairs
        for (int i = 0; i < arms.Count; i++)
        {
            //if there are orgins, then they are enforced again/ in case they have moved
            if (orgins != null)
            {
                arms[i][0].orgin = orgins[i];
            }

            if (!paused)//runs calculations and update while game is not paused
            {
                //runs calculations for parent and child first without updating values
                //this is for the calculations are dependet of echothers original values
                arms[i][0].Calc();
                arms[i][1].Calc();
                
                //updates the arms angles and velocitys based on calculations
                arms[i][0].Update();
                arms[i][1].Update();
            }

            //draws the whole arm
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
    //the simelation for the orbits
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
        orbits = new PendjulumSim(armCount * amt, 20, screen, false, ballArms, trailList);
    }
    
    public void update()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].Update();
            for (int j = 0; j < ballArms.Count / balls.Count; j++)
            {
                if ((ballArms[i].X - balls[i].position.X) > 100)
                {
                    for (int k = 0; k < orbits.arms.Count; k++)
                    {
                        orbits.arms[k][1].trail.Add(new Vector2(9999, -9999));
                    }
                }
                ballArms.RemoveAt(0);
                ballArms.Add(balls[i].position);
            }
        }

        orbits.orgins = ballArms;
        orbits.Update();
        Draw();
    }
    public void Draw()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].Draw();
        }
    }
}
