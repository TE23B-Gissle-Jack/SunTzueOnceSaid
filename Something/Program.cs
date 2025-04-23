using System.Numerics;
using Raylib_cs;
using Something;

Raylib.InitWindow(1000, 1000, "Slap");
Raylib.SetTargetFPS(60);
int screenHeight = Raylib.GetScreenHeight();
int screenWidth = Raylib.GetScreenWidth();

PendjulumSim sim1 = new PendjulumSim(0, 100, new(screenWidth, screenHeight), true,null);
Balls hugo = new Balls(1,2);

Ball ball = new Ball(10, new(400, 400));
ArmSegment aaa = new ArmSegment(ball.position, 50, null, 1, "parent");
ArmSegment bbb = new ArmSegment(aaa.position, 50, aaa, 1, "child");


while (!Raylib.WindowShouldClose())
{
    Raylib.ClearBackground(Color.Black);
    Raylib.BeginDrawing();

    sim1.Update();
    hugo.update();

    // ball.Update();
    // ball.Draw();

    // aaa.orgin = ball.position;
    // aaa.Calc();
    // bbb.Calc();
    // aaa.Update();
    // aaa.Draw();
    // bbb.Update();
    // bbb.Draw();

    Raylib.EndDrawing();
}

public class PendjulumSim
{
    List<ArmSegment[]> arms = new List<ArmSegment[]>();
    bool paused = false;

    public List<Vector2> orgins;

    public PendjulumSim(int amt, int leangth, Vector2 screen, bool random,List<Vector2> og)
    {
        orgins=og;
        for (int i = 0; i < amt; i++)
        {
            Vector2 position;

            int armLeangth = leangth;

            if (random)
            {
                int x = Random.Shared.Next(armLeangth * 2, (int)screen.X - armLeangth * 2);
                int y = Random.Shared.Next(armLeangth * 2, (int)screen.Y - armLeangth * 2);
                position = new Vector2(x, y);
            }
            else position = og[i];



            ArmSegment arm = new ArmSegment(position, 100, null, 1, "parent");
            ArmSegment arm2 = new ArmSegment(position, 100, arm, 2, "child");

            arms.Add([arm, arm2]);
        }
    }

    public void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            paused = !paused;
        }

        for (int i = 0; i < arms.Count; i++)
        {
            if (orgins!=null)
            {
                arms[i][0].orgin = orgins[i];
            }

            if (!paused)//runs calculations and update while game is not paused
            {
                arms[i][0].Calc();
                arms[i][1].Calc();

                arms[i][0].Update();
                arms[i][1].Update();
            }

            Draw();
        }
    }
    public void Draw()
    {
        for (int i = 0; i < arms.Count; i++)
        {
            arms[i][0].Draw();
            arms[i][1].Draw();
        }
    }

}

public class Balls
{
    List<Ball> balls = new List<Ball>();
    List<Vector2> ballArms = new List<Vector2>();
    PendjulumSim orbits;

    public Balls(int amt, int armCount)
    {
        for (int i = 0; i < amt; i++)
        {
            balls.Add(new Ball(10, new(Random.Shared.Next(1000), Random.Shared.Next(400))));
            for (int j = 0; j < armCount; j++)
            {
                ballArms.Add(balls[i].position);
            }
        }
        orbits = new PendjulumSim(armCount*amt, 50, new(1000, 1000), false, ballArms);
    }
    public void update()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].Update();
            for (int j = 0; j < ballArms.Count/balls.Count; j++)
            {
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
