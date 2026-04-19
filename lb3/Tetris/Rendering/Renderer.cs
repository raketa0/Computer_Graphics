using OpenTK.Graphics.OpenGL;
using Task3.Rendering;
using Tetris.System;
using Tetris.Types;


namespace Tetris.Rendering
{
    public class Renderer
    {
        private const int Cell = 30;
        private const int FieldWidth = 10;
        private const int FieldHeight = 20;
        private const int FieldX = 50;
        private const int FieldY = 50;
        private const int InfoX = 410;
        private const int InfoY = 50;
        private const int NextX = 410;
        private const int NextY = 280;
        private const int CharWidth = 8;
        private const int CharHeight = 8;

        public void Draw(Tetris_Game game)
        {
            DrawFieldBackground();
            DrawField(game);
            DrawCurrent(game);
            DrawBorders();
            DrawUI(game);
            DrawNextFigure(game);

            if (game.State == GameState.GameOver)
            {
                DrawGameOver();
            }
            else if (game.State == GameState.Paused)
            {
                DrawPaused();
            }
        }

        private void DrawFieldBackground()
        {
            GL.Color3(0.05f, 0.05f, 0.1f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(FieldX, FieldY);
            GL.Vertex2(FieldX + FieldWidth * Cell, FieldY);
            GL.Vertex2(FieldX + FieldWidth * Cell, FieldY + FieldHeight * Cell);
            GL.Vertex2(FieldX, FieldY + FieldHeight * Cell);
            GL.End();
        }

        private void DrawBorders()
        {
            for (int y = 0; y < FieldHeight; y++)
            {
                DrawBorderBlockWithOutline(-1, y);
            }

            for (int y = 0; y < FieldHeight; y++)
            {
                DrawBorderBlockWithOutline(FieldWidth, y);
            }

            for (int x = -1; x <= FieldWidth; x++)
            {
                DrawBorderBlockWithOutline(x, FieldHeight);
            }

            DrawBorderBlockWithOutline(-1, FieldHeight);
            DrawBorderBlockWithOutline(FieldWidth, FieldHeight);
        }

        private void DrawBorderBlockWithOutline(int x, int y)
        {
            float px = FieldX + x * Cell;
            float py = FieldY + y * Cell;

            GL.Color3(0.3f, 0.3f, 0.35f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(px, py);
            GL.Vertex2(px + Cell, py);
            GL.Vertex2(px + Cell, py + Cell);
            GL.Vertex2(px, py + Cell);
            GL.End();

            GL.Color3(0f, 0f, 0f);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(px, py);
            GL.Vertex2(px + Cell, py);
            GL.Vertex2(px + Cell, py + Cell);
            GL.Vertex2(px, py + Cell);
            GL.End();

            GL.Color3(0.5f, 0.5f, 0.55f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(px + 1, py + 1);
            GL.Vertex2(px + Cell - 1, py + 1);
            GL.Vertex2(px + 1, py + 1);
            GL.Vertex2(px + 1, py + Cell - 1);
            GL.End();
        }

        private void DrawField(Tetris_Game game)
        {
            var f = game.Field;

            for (int y = 0; y < f.Height; y++)
            {
                for (int x = 0; x < f.Width; x++)
                {
                    int c = f.GetCell(x, y);
                    if (c != 0)
                    {
                        DrawCell(x, y, (ColorType)(c - 1));
                    }
                }

            }
        }

        private void DrawCurrent(Tetris_Game game)
        {
            foreach (var b in game.Current.Blocks)
            {
                if (b.Y >= 0)
                {
                    DrawCell(b.X, b.Y, game.Current.Color);
                }
            }
        }

        private void DrawNextFigure(Tetris_Game game)
        {
            DrawText(NextX, NextY - 35, "NEXT FIGURE:", 1f, 1f, 0f, 1.5f);

            int frameWidth = 140;
            int frameHeight = 140;

            GL.Color3(0.3f, 0.3f, 0.35f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(NextX - 15, NextY);
            GL.Vertex2(NextX + frameWidth - 15, NextY);
            GL.Vertex2(NextX + frameWidth - 15, NextY + frameHeight);
            GL.Vertex2(NextX - 15, NextY + frameHeight);
            GL.End();

            GL.Color3(0f, 0f, 0f);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(NextX - 15, NextY);
            GL.Vertex2(NextX + frameWidth - 15, NextY);
            GL.Vertex2(NextX + frameWidth - 15, NextY + frameHeight);
            GL.Vertex2(NextX - 15, NextY + frameHeight);
            GL.End();

            var blocks = game.Next.Blocks;

            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (var b in blocks)
            {
                minX = Math.Min(minX, b.X);
                maxX = Math.Max(maxX, b.X);
                minY = Math.Min(minY, b.Y);
                maxY = Math.Max(maxY, b.Y);
            }

            int figureWidth = (maxX - minX + 1) * Cell;
            int figureHeight = (maxY - minY + 1) * Cell;

            int offsetX = NextX + (frameWidth - figureWidth) / 2 - (minX * Cell);
            int offsetY = NextY + (frameHeight - figureHeight) / 2 - (minY * Cell);

            foreach (var b in blocks)
            {
                DrawCellAbsolute(offsetX + b.X * Cell, offsetY + b.Y * Cell, game.Next.Color);
            }
        }

        private void DrawUI(Tetris_Game game)
        {
            DrawText(InfoX, InfoY, "TETRIS", 0f, 1f, 0f, 2.5f);

            GL.Color3(0.3f, 0.3f, 0.35f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(InfoX - 15, InfoY + 65);
            GL.Vertex2(InfoX + 155, InfoY + 65);
            GL.Vertex2(InfoX + 155, InfoY + 68);
            GL.Vertex2(InfoX - 15, InfoY + 68);
            GL.End();

            DrawText(InfoX, InfoY + 90, "SCORE:", 1f, 1f, 0f, 1.3f);
            DrawText(InfoX + 120, InfoY + 90, game.Score.ToString(), 1f, 1f, 1f, 1.3f);

            DrawText(InfoX, InfoY + 130, "LEVEL:", 1f, 1f, 0f, 1.3f);
            DrawText(InfoX + 120, InfoY + 130, game.Level.ToString(), 1f, 1f, 1f, 1.3f);

            DrawText(InfoX, InfoY + 170, "LINES:", 1f, 1f, 0f, 1.3f);
            DrawText(InfoX + 120, InfoY + 170, game.LinesToNext.ToString(), 1f, 1f, 1f, 1.3f);
        }

        private void DrawCell(int x, int y, ColorType color)
        {
            DrawCellAbsolute(FieldX + x * Cell, FieldY + y * Cell, color);
        }

        private void DrawCellAbsolute(float px, float py, ColorType color)
        {
            SetColor(color);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(px, py);
            GL.Vertex2(px + Cell, py);
            GL.Vertex2(px + Cell, py + Cell);
            GL.Vertex2(px, py + Cell);
            GL.End();

            GL.Color3(0f, 0f, 0f);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(px, py);
            GL.Vertex2(px + Cell, py);
            GL.Vertex2(px + Cell, py + Cell);
            GL.Vertex2(px, py + Cell);
            GL.End();
        }

        private void DrawText(float x, float y, string text, float r, float g, float b, float scale = 1f)
        {
            GL.Color3(r, g, b);
            float currentX = x;
            float charWidth = CharWidth * scale;
            float charHeight = CharHeight * scale;

            foreach (char c in text.ToUpper())
            {
                byte[] pattern = BitmapFont.GetCharacter(c);

                for (int row = 0; row < CharHeight; row++)
                {
                    byte rowBits = pattern[row];
                    if (rowBits == 0)
                    {
                        continue;
                    }

                    for (int col = 0; col < CharWidth; col++)
                    {
                        if ((rowBits & (1 << (7 - col))) != 0)
                        {
                            float px = currentX + col * scale;
                            float py = y + row * scale;

                            GL.Begin(PrimitiveType.Quads);
                            GL.Vertex2(px, py);
                            GL.Vertex2(px + scale, py);
                            GL.Vertex2(px + scale, py + scale);
                            GL.Vertex2(px, py + scale);
                            GL.End();
                        }
                    }
                }
                currentX += charWidth + (scale * 0.5f);
            }
        }

        private void DrawGameOver()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Color4(0f, 0f, 0f, 0.8f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(FieldX, FieldY + FieldHeight * Cell / 2 - 80);
            GL.Vertex2(FieldX + FieldWidth * Cell, FieldY + FieldHeight * Cell / 2 - 80);
            GL.Vertex2(FieldX + FieldWidth * Cell, FieldY + FieldHeight * Cell / 2 + 80);
            GL.Vertex2(FieldX, FieldY + FieldHeight * Cell / 2 + 80);
            GL.End();
            GL.Disable(EnableCap.Blend);

            DrawText(FieldX + 40, FieldY + FieldHeight * Cell / 2 - 40, "GAME OVER", 1f, 0f, 0f, 2f);
            DrawText(FieldX + 25, FieldY + FieldHeight * Cell / 2 + 10, "PRESS ENTER", 1f, 1f, 1f, 1.2f);
            DrawText(FieldX + 35, FieldY + FieldHeight * Cell / 2 + 40, "TO RESTART", 1f, 1f, 1f, 1.2f);
        }

        private void DrawPaused()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Color4(0f, 0f, 0f, 0.7f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(FieldX, FieldY + FieldHeight * Cell / 2 - 50);
            GL.Vertex2(FieldX + FieldWidth * Cell, FieldY + FieldHeight * Cell / 2 - 50);
            GL.Vertex2(FieldX + FieldWidth * Cell, FieldY + FieldHeight * Cell / 2 + 50);
            GL.Vertex2(FieldX, FieldY + FieldHeight * Cell / 2 + 50);
            GL.End();
            GL.Disable(EnableCap.Blend);

            DrawText(FieldX + 55, FieldY + FieldHeight * Cell / 2 - 20, "PAUSED", 1f, 1f, 0f, 2f);
            DrawText(FieldX + 20, FieldY + FieldHeight * Cell / 2 + 15, "PRESS P TO RESUME", 1f, 1f, 1f, 1f);
        }

        private void SetColor(ColorType c)
        {
            switch (c)
            {
                case ColorType.YELLOW: GL.Color3(1f, 1f, 0f); break;
                case ColorType.PURPLE: GL.Color3(0.8f, 0.2f, 0.8f); break;
                case ColorType.ORANGE: GL.Color3(1f, 0.5f, 0f); break;
                case ColorType.BLUE: GL.Color3(0f, 0.5f, 1f); break;
                case ColorType.GREEN: GL.Color3(0f, 1f, 0f); break;
                case ColorType.RED: GL.Color3(1f, 0f, 0f); break;
                default: GL.Color3(1f, 1f, 1f); break;
            }
        }
    }
}