using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using Pamella;
using Pamella.Views;

public class FPS : View
{
    private DateTime first;
    private Queue<DateTime> frames;
    private TimeSpan duration;
    private int fps;
    private RectangleF fpsRect;

    protected override void OnStart(IGraphics g)
    {
        this.frames = new Queue<DateTime>();
        first = DateTime.Now;
        frames.Enqueue(DateTime.Now);
        fps = 0;
        duration = TimeSpan.Zero;
        fpsRect = new RectangleF(
            g.Width - 60, 5, 50, 20
        );
        AlwaysInvalidateMode();
    }

    protected override void OnFrame(IGraphics g)
    {
        var last = DateTime.Now;
        frames.Enqueue(last);

        if (frames.Count > 40)
            first = frames.Dequeue();
    
        duration = last - first;
        fps = (int)(frames.Count / duration.TotalSeconds);
    }

    protected override void OnRender(IGraphics g)
    {
        g.FillRectangle(fpsRect, Brushes.Black);
        g.DrawText(fpsRect,
            StringAlignment.Center, 
            StringAlignment.Center, 
            Brushes.Yellow, fps.ToString()
        );
    }
}