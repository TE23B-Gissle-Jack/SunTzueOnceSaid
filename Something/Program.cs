using System.Numerics;
using Raylib_cs;
using Something;

//makes the window called "Slap" with a target fps of 60
Raylib.InitWindow(1500, 1000, "Slap");
Raylib.SetTargetFPS(60);
Raylib.SetExitKey(KeyboardKey.Null); //Disable ESC as exit key

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
Balls hugo = new Balls(0, 1, trailList, screen);

Menu test = new Menu(hugo,sim1,screen,trailList);

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

    test.Update();
    test.Draw();

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
