using System.Drawing;

using Pamella;
using Pamella.Games;

public class Block : View, ICollider
{
    public SpriteController sprite;

    public Block(string path)
    {
        var bmp = Bitmap.FromFile(path) as Bitmap;
        sprite = SpriteController.Load(bmp);
    }

    public RectangleF Rect { get; set; }
}