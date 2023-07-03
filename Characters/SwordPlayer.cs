using System.Drawing;
using Pamella;

public class SwordPlayer : Player
{
    private Bitmap swordBmp;
    private int frame = 0;
    private int angle = 0;
    private bool onAttack = false;

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
        if (g.IsDown && !onAttack)
            onAttack = true;

        if (!onAttack)
        {
            base.OnRender(g);
            return;
        }

        // SetState(PlayerState.Attacking);

        var size = new Size(
            swordBmp.Width,
            swordBmp.Height
        );
        var swordLocation = new Point(
            Location.X + 75,
            Location.Y
        );
        var rect = new Rectangle(
            swordLocation, 
            size
        );
        
        angle += 15;
        
        g.RotateAt(angle, 
            Location.X + 50,
            Location.Y + size.Height - 25
        );
        g.DrawImage(rect, swordBmp,
            new Rectangle(Point.Empty, size)
        );
        g.Reset();

        base.OnRender(g);

        if (angle > 90)
        {
            angle = -90;
            onAttack = false;
        }
    }

    protected override void OnStart(IGraphics g)
    {
        base.OnStart(g);
    }
}