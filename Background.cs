using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using Pamella;
using Pamella.Views;

public class Background : View
{
    public Color BackColor { get; set; } = Color.White;

    private string[] paths;
    private List<Bitmap> bgs = new List<Bitmap>();
    
    private Background(string[] paths)
        => this.paths = paths;
    
    private Background(Color color, string[] paths) : this(paths)
        => this.BackColor = color;

    protected override void OnStart(IGraphics g)
    {
        foreach (var path in this.paths)
        {
            var img = Bitmap.FromFile(path);
            var bmp = img as Bitmap;
            if (bmp is null)
                continue;

            bmp = bmp.GetThumbnailImage(g.Width, g.Height, null, nint.Zero) as Bitmap;
            
            bgs.Add(bmp);
        }
    }

    protected override void OnRender(IGraphics g)
    {
        g.Clear(BackColor);
        var screenRect = new RectangleF(
            0, 0, g.Width, g.Height
        );
        foreach (var bg in this.bgs)
            g.DrawImage(screenRect, bg);
    }

    public static Background Open(params string[] paths)
        => new Background(paths);

    public static Background Open(Color color, params string[] paths)
        => new Background(color, paths);
}