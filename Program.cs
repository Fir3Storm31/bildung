// See https://aka.ms/new-console-template for more information
using System;
using System.Data.SqlTypes;
using System.Numerics;
using Raylib_cs;

namespace Bildung
{
    public class DraggableSquare
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }

        public DraggableSquare(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
        }
    }

    class Program
    {
        public static int GameWidth = 1200;
        public static int GameHeight = 750;

        static List<DraggableSquare> draggableSquares = new List<DraggableSquare>();
        static DraggableSquare? selectedSquare = null;

        static void Main(string[] args)
        {

            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE | ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_ALWAYS_RUN | ConfigFlags.FLAG_WINDOW_HIGHDPI);
            Raylib.InitWindow(GameWidth, GameHeight, "Bildung");

            Camera2D camera = new Camera2D();
            camera.zoom = 1.0f;

            // Red square position
            Vector2 square_position = new Vector2(GameWidth / 2 - 25, GameHeight / 10 - 25);

            Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {

            // Update

            // Translate based on mouse right click
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                Vector2 delta = Raylib.GetMouseDelta();
                delta = Raymath.Vector2Scale(delta, -1.0f/camera.zoom);

                camera.target = Raymath.Vector2Add(camera.target, delta);
            }

            Vector2 mouse_position = Raylib.GetMousePosition();

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                // Create a new draggable square at a specific position
                Vector2 newSquarePosition = new Vector2((int)square_position.X, (int)square_position.Y);
                DraggableSquare newSquare = new DraggableSquare(newSquarePosition, Color.BLUE);
                draggableSquares.Add(newSquare);
            }

            // Check for mouse interactions with draggable squares
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                // Check interactions with the blue squares
                foreach (DraggableSquare square in draggableSquares)
                {
                    if (Raylib.CheckCollisionPointRec(mouse_position, new Rectangle(square.Position.X, square.Position.Y, 50, 50)))
                    {
                        selectedSquare = square;
                        break;
                    }
                }

                if (selectedSquare != null)
                {
                    // Move the selected blue square with the mouse
                    selectedSquare.Position = mouse_position;
                }
            }


            // Draw
            Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);

                Raylib.BeginMode2D(camera);

                // Draw the grid
                Rlgl.rlPushMatrix();
                    Rlgl.rlTranslatef(0, 25*50, 0);
                    Rlgl.rlRotatef(90, 1, 0, 0);
                    Raylib.DrawGrid(100, 40);
                Rlgl.rlPopMatrix();

                // Draw the draggable squares
                foreach (DraggableSquare square in draggableSquares)
                {
                    Raylib.DrawRectangle((int)square.Position.X, (int)square.Position.Y, 50, 50, square.Color);
                }

                Raylib.EndMode2D();

                // Draw the top bar
                Raylib.DrawRectangle(0, 0, GameWidth, GameHeight / 5, Color.GRAY);

                // Draw a square in the top bar
                Raylib.DrawRectangle((int)square_position.X, (int)square_position.Y, 50, 50, Color.RED);

            Raylib.EndDrawing();

        }

        Raylib.CloseWindow();

        return;

        }
    }
}