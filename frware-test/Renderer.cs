﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace frware_test
{
    internal class Renderer
    {
        // Window size of the renderer
        IntVector2 size;

        public DoubleDrawingBuffer buffer;

        public Renderer(IntVector2 size)
        {
            this.size = size;
            buffer = new DoubleDrawingBuffer(size);
        }

        public void DrawLine(IntVector2 coords, (string color, char letter)[] values)
        {
            DrawingChar[] line = values.Select(x => new DrawingChar(x.letter, x.color)).ToArray();
            buffer.DrawLine(coords, line);
        }

        public void DrawLine(IntVector2 coords, (string color, string word) value)
        {
            DrawingChar[] line = value.word.Select(x => new DrawingChar(x, value.color)).ToArray();
            buffer.DrawLine(coords, line);
        }

        public void DrawWindow(Window window)
        {
            if (window.Data == null)
                return;

            buffer.DrawChar(window.Position, (DrawingChar)window.Border.CornerTopLeft);
            buffer.DrawChar(window.Position + (window.Size.X, 0), (DrawingChar)window.Border.CornerTopRight);
            buffer.DrawChar(window.Position + (window.Size.X, window.Size.Y-1), (DrawingChar)window.Border.CornerBottomRight);
            buffer.DrawChar(window.Position + (0, window.Size.Y-1), (DrawingChar)window.Border.CornerBottomLeft);

            buffer.DrawVLine(window.Position + (0, 1), window.Border.Left);
            buffer.DrawVLine(window.Position + (window.Size.X, 1), window.Border.Right);

            buffer.DrawLine(window.Position + (1, 0), window.Border.Top);
            buffer.DrawLine(window.Position + (1, window.Size.Y-1), window.Border.Bottom);


            //foreach (DrawingChar[] line in window.Data) {
            //    buffer.DrawLine(coords + (1,0), line);
            //}

            // buffer.drawLine(coords, line);
        }

        public void Draw()
        {
            List<Tuple<IntVector2, int>> toUpdate = buffer.CheckDirty();
            //foreach (Tuple<IntVector2, int> tpl in toUpdate)
                //System.Diagnostics.Debug.WriteLine("update dirty: x{0} y{1}, len {2}",tpl.Item1.X, tpl.Item1.Y, tpl.Item2);

            buffer.AcceptDirty();

            foreach(Tuple<IntVector2, int> sector in toUpdate)
            {
                //Console.SetCursorPosition(0, 0);
                //Console.WriteLine(sector.Item2);
                Console.SetCursorPosition(sector.Item1.X, sector.Item1.Y);
                Console.Write(buffer.GetLine(sector.Item1, sector.Item2));
            }
        }
    }
}
