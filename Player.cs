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

    public float Gravity { get; set; } = 500;
    public float Speed { get; set; } = 200;
    public float JumpForce { get; set; } = 400;

    // main data
    private string spritePath;
    private Sprite<PlayerState> sprite;

    // opeartion data
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

        this.sprite = new Sprite<PlayerState>();
        this.sprite.Animation.State = PlayerState.Idle;
        AddSubView(this.sprite);
        
        var idle = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 0 * desloc,
            desloc, 1);
        this.sprite.Animation.AddSprite(
            PlayerState.Idle, idle
        );

        var attacking = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 1 * desloc,
            desloc, 4);
        this.sprite.Animation.AddSprite(
            PlayerState.Attacking, attacking
        );
        
        var jumping = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 5 * desloc,
            desloc, 1);
        this.sprite.Animation.AddSprite(
            PlayerState.Jumping, jumping
        );

        var walking = SpriteController.Load(
            spriteSheet, size,
            PointF.Empty + 6 * desloc,
            desloc, 14);
        this.sprite.Animation.AddSprite(
            PlayerState.Walking, walking
        );

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

    protected override void OnRender(IGraphics g)
    {

    }

    protected override void OnFrame(IGraphics g)
    {
        var time = getTime();
        var limitY = discoverIfInGround(g);

        getYVelocity(time, limitY ?? -1);
        getXVelocity(time);
        
        updatePosition(time);
    }

    private float getTime()
    {
        var now = DateTime.Now;
        var time = now - last;
        last = now;

        var seconds = (float)time.TotalSeconds;
        return seconds;
    }

    private int? discoverIfInGround(IGraphics g)
    {
        inGround = this.Location.Y + this.Size.Height >= g.Height;

        if (!inGround)
            return null;
        
        int limit = (int)g.Height;
        return limit;
    }

    private void getYVelocity(float secs, int groundLimit)
    {

        if (inGround)
        {
            this.Location = new Point(Location.X, groundLimit - this.Size.Height);
            Velocity = Velocity * Vector2.UnitX;

            if (tryJump)
                Velocity -= JumpForce * Vector2.UnitY;
        }
        else
        {
            Velocity += Gravity * Vector2.UnitY * secs;
            this.sprite.Animation.State = PlayerState.Jumping;
        }
    }

    private void getXVelocity(float secs)
    {
        if (tryLeft && !tryRight)
        {
            seeingRight = false;
            Velocity = Velocity * Vector2.UnitY - Speed * Vector2.UnitX;
            if (inGround)
                this.sprite.Animation.State = PlayerState.Walking;
        }
        else if (tryRight && !tryLeft)
        {
            seeingRight = true;
            Velocity = Velocity * Vector2.UnitY + Speed * Vector2.UnitX;
            if (inGround)
                this.sprite.Animation.State = PlayerState.Walking;
        }
        else Velocity = Velocity * Vector2.UnitY;
    }

    private void updatePosition(float secs)
    {
        if (Velocity == Vector2.Zero)
            this.sprite.Animation.State = PlayerState.Idle;
        Position += Velocity * secs;

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

    private enum PlayerState
    {
        Idle,
        Walking,
        Jumping,
        Attacking
    }
}