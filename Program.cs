// See https://aka.ms/new-console-template for more information
using System.Numerics;
using Raylib_cs;

namespace Bildung
{
    class Program
    {
        public static int GameWidth = 1200;
        public static int GameHeight = 750;
        static List<Element> elements = new List<Element>();
        public static int elementSize = 50;

        static void Main(string[] args)
        {

            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE | ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_ALWAYS_RUN | ConfigFlags.FLAG_WINDOW_HIGHDPI);
            Raylib.InitWindow(GameWidth, GameHeight, "Bildung");

            Camera2D camera = new Camera2D();
            camera.zoom = 1.0f;
            // Set the camera offset to the center of the screen without counting the top bar
            camera.offset = new Vector2(GameWidth / 2, GameHeight / 2 + GameHeight / 5);

            // Load the sumTexture from assets
            Image sumImage = Raylib.LoadImage("assets/sum2.png");
            Raylib.ImageResizeNN(ref sumImage, elementSize, elementSize);
            Texture2D sumTexture = Raylib.LoadTextureFromImage(sumImage);      // Image converted to sumTexture, uploaded to GPU memory (VRAM)
            Raylib.UnloadImage(sumImage);

            // Set the sumTexture position
            Vector2 sumPosition = new Vector2(GameWidth / 2 - elementSize / 2, GameHeight / 10 - elementSize / 2);

            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {

                // Update

                // Get the mouse position
                Vector2 mousePosition = Raylib.GetMousePosition();
                // Get the mouse delta
                Vector2 delta = Raylib.GetMouseDelta();
                // Get the world point that is under the mouse
                Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(mousePosition, camera);

                // Translate based on mouse right click
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
                {
                    // Convert the delta into world space
                    delta = Raymath.Vector2Scale(delta, -1.0f / camera.zoom);
                    camera.target = Raymath.Vector2Add(camera.target, delta);
                }

                // Zoom based on mouse wheel
                float wheel = Raylib.GetMouseWheelMove();
                if (wheel != 0)
                {
                    // Set the offset to where the mouse is
                    camera.offset = mousePosition;

                    // Set the target to match, so that the camera maps the world space point 
                    // under the cursor to the screen space point under the cursor at any zoom
                    camera.target = mouseWorldPos;

                    // Zoom increment
                    const float zoomIncrement = 0.125f;

                    camera.zoom += wheel * zoomIncrement;
                    if (camera.zoom < zoomIncrement) camera.zoom = zoomIncrement;
                }

                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(sumPosition.X, sumPosition.Y, 50, 50)))
                    {
                        // Center of the screen
                        Vector2 center = new Vector2(GameWidth / 2 - elementSize / 2, GameHeight / 2 - elementSize / 2);

                        // Add the square to the list and pass empty lists for inputs and weights
                        elements.Add(new Sum(Raylib.GetScreenToWorld2D(center, camera), sumTexture, new List<Element>()));
                    }
                }

                // Move the sqaures with drag and drop
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    foreach (Element elmnt in elements)
                    {

                        if (Raylib.CheckCollisionPointRec(mouseWorldPos, new Rectangle(elmnt.Position.X, elmnt.Position.Y, 50, 50)))
                        {
                            // Convert the delta into world space
                            delta = Raymath.Vector2Scale(delta, -1.0f / camera.zoom);
                            // Move the square by the delta
                            elmnt.Position = Raymath.Vector2Add(elmnt.Position, -1 * delta);
                        }
                    }
                }


                // Draw
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);

                Raylib.BeginMode2D(camera);

                // Draw the grid
                Rlgl.rlPushMatrix();
                Rlgl.rlTranslatef(0, 25 * 50, 0);
                Rlgl.rlRotatef(90, 1, 0, 0);
                Raylib.DrawGrid(1000, elementSize / 2);
                Rlgl.rlPopMatrix();

                // Draw the elements
                foreach (Element elmnt in elements)
                {
                    Raylib.DrawTexture(elmnt.Texture, (int)elmnt.Position.X, (int)elmnt.Position.Y, Color.WHITE);
                }

                Raylib.EndMode2D();

                // Draw the top bar
                Raylib.DrawRectangle(0, 0, GameWidth, GameHeight / 5, Color.GRAY);

                // Draw the sumTexture
                Raylib.DrawTexture(sumTexture, (int)sumPosition.X, (int)sumPosition.Y, Color.WHITE);


                Raylib.EndDrawing();

            }

            Raylib.CloseWindow();

            return;

        }
    }
}
