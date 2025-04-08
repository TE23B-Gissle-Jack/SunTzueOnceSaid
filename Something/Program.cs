using Raylib_cs;
using Something;

Raylib.InitWindow(800,800, "Slap");
Raylib.SetTargetFPS(60);
int screenHeight = Raylib.GetScreenHeight();
int screenWidth = Raylib.GetScreenWidth();

ArmSegment arm = new ArmSegment(new(400,400),100, null,1);
ArmSegment arm2 = new ArmSegment(new(400,400),100, arm,2);

while (!Raylib.WindowShouldClose())
{
    Raylib.ClearBackground(Color.Black);
    Raylib.BeginDrawing();

    arm.Update();
    arm.Draw();
    arm2.Update();
    arm2.Draw();

    Raylib.EndDrawing();
}