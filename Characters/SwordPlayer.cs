using System.Drawing;
using Pamella;

public class SwordPlayer : Player
{
    private Bitmap swordBmp;
    private int frame = 0;

    public SwordPlayer(string path, string sword) 
        : base(path)
    {
        this.swordBmp = Bitmap.FromFile(sword) as Bitmap;
    }

    protected override void OnFrame(IGraphics g)
    {
        base.OnFrame(g);
    }

    protected override void OnRender(IGraphics g)
    {
        var size = new Size(swordBmp.Width, swordBmp.Height);
        var rect = new Rectangle(Location, size);
        
        g.RotateAt(frame++, 
            Location.X + size.Width / 2,
            Location.Y + size.Height / 2
        );
        g.DrawImage(rect, swordBmp,
            new Rectangle(Point.Empty, size)
        );
        g.Reset();
        
        base.OnRender(g);
    }

    protected override void OnStart(IGraphics g)
    {
        base.OnStart(g);
    }
}