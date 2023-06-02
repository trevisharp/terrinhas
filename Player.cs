using System;
using System.Drawing;
using System.Numerics;

using Pamella;
using Pamella.Views;

public class Player : View
{
    public Player(string spritePath)
    {
        this.spritePath = spritePath;
    }

    private const float gravity = 500;
    private const float moveSpeed = 200;
    private const float jumpForce = 400;

    // main data
    private string spritePath;
    private Sprite<int> sprite;

    // dynamic data
    private DateTime last = DateTime.Now;
    private bool inGround = false;
    private bool tryJump = false;
    private bool tryLeft = false;
    private bool tryRight = false;
    private bool seeingRight = true;

    public Point Location
    {
        get => sprite.Rect.Location;
        set => sprite.Rect = new Rectangle(
            value, sprite.Rect.Size
        );
    }

    public Size Size
    {
        get => sprite.Rect.Size;
        set => sprite.Rect = new Rectangle(
            sprite.Rect.Location, value
        );
    }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    protected override void OnStart(IGraphics g)
    {
        var spriteSheet = Bitmap.FromFile(spritePath) as Bitmap;
        var size = new Size(40, 56);
        var desloc = new Size(0, size.Height);
        
        var idle = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 0 * desloc,
            desloc, 1);
        var attacking = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 1 * desloc,
            desloc, 4);
        var jumping = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 5 * desloc,
            desloc, 1);
        var walking = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 6 * desloc,
            desloc, 14);
        
        this.sprite = new Sprite<int>();
        this.sprite.Animation.AddSprite(0, idle);
        this.sprite.Animation.AddSprite(1, walking);
        this.sprite.Animation.AddSprite(2, jumping);
        this.sprite.Animation.AddSprite(3, attacking);
        this.sprite.Animation.State = 1;
        AddSubView(this.sprite);
        
        this.Position = new Vector2(300, 300);
        this.Location = new Point((int)Position.X, (int)Position.Y);
        this.Size = 2 * size;

        AlwaysInvalidateMode();

        g.SubscribeKeyDownEvent(input =>
        {
            switch (input)
            {
                case Input.A:
                    tryLeft = true;
                    break;
                    
                case Input.D:
                    tryRight = true;
                    break;
                    
                case Input.W:
                    tryJump = true;
                    break;
                    
                case Input.S:
                
                    break;
            }
        });

        g.SubscribeKeyUpEvent(input =>
        {
            switch (input)
            {
                case Input.A:
                    tryLeft = false;
                    break;
                    
                case Input.D:
                    tryRight = false;
                    break;
                    
                case Input.W:
                    tryJump = false;
                    break;
                    
                case Input.S:
                
                    break;
            }
        });
    }

    protected override void OnFrame(IGraphics g)
    {
        var time = getTime();

        inGround = this.Location.Y + this.Size.Height >= g.Height;

        if (inGround)
        {
            this.Location = new Point(Location.X, g.Height - this.Size.Height);
            Velocity = Velocity * Vector2.UnitX;

            if (tryJump)
                Velocity -= jumpForce * Vector2.UnitY;
        }
        else
        {
            Velocity += gravity * Vector2.UnitY * time;
            this.sprite.Animation.State = 2;
        }

        if (tryLeft && !tryRight)
        {
            seeingRight = false;
            Velocity = Velocity * Vector2.UnitY - moveSpeed * Vector2.UnitX;
            if (inGround)
                this.sprite.Animation.State = 1;
        }
        else if (tryRight && !tryLeft)
        {
            seeingRight = true;
            Velocity = Velocity * Vector2.UnitY + moveSpeed * Vector2.UnitX;
            if (inGround)
                this.sprite.Animation.State = 1;
        }
        else
        {
            Velocity = Velocity * Vector2.UnitY;
        }

        if (Velocity == Vector2.Zero)
            this.sprite.Animation.State = 0;
        Position += Velocity * time;

        if (seeingRight)
        {
            if (this.Size.Width < 0)
                this.Size = new Size(-this.Size.Width, this.Size.Height);
            this.Location = new Point((int)Position.X, (int)Position.Y);
        }
        else
        {
            if (this.Size.Width > 0)
                this.Size = new Size(-this.Size.Width, this.Size.Height);
            this.Location = new Point((int)Position.X - this.Size.Width, (int)Position.Y);
        }
    }

    protected override void OnRender(IGraphics g)
    {

    }

    private float getTime()
    {
        var now = DateTime.Now;
        var time = now - last;
        last = now;

        var seconds = (float)time.TotalSeconds;
        return seconds;
    }
}
