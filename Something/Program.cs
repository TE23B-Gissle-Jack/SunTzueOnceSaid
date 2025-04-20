using Raylib_cs;
using Something;

Raylib.InitWindow(1000,1000, "Slap");
Raylib.SetTargetFPS(60);
int screenHeight = Raylib.GetScreenHeight();
int screenWidth = Raylib.GetScreenWidth();

List<ArmSegment[]> arms = new List<ArmSegment[]>();

for (int i = 0; i < 1; i++)
{
    ArmSegment arm = new ArmSegment(new(400,400),100, null,1,"parent");
    ArmSegment arm2 = new ArmSegment(new(400,400),100, arm,2,"child");

    arms.Add([arm,arm2]);
}

while (!Raylib.WindowShouldClose())
{
    Raylib.ClearBackground(Color.Black);
    Raylib.BeginDrawing();

    for (int i = 0; i < arms.Count; i++)
    {
        arms[i][0].Calc();
        arms[i][1].Calc();

        arms[i][0].Update();
        arms[i][1].Update();

        arms[i][0].Draw();
        arms[i][1].Draw();
    }
    

    // Raylib.DrawText($"Arm 1: Velocity: {arm.velocity} A: {arm.acelaration}",50,50,20,Color.White);
    // Raylib.DrawText($"Arm 2: Velocity: {arm2.velocity} A: {arm2.acelaration}",50,150,20,Color.White);

    Raylib.EndDrawing();
}