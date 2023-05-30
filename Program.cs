using System;
using System.Linq;
using System.Collections.Generic;

App.Open<Test>();

public class Player : View
{
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

    private Sprite<int> sprite;

    public Player()
    {
        var path = "sprites/carbo.png";
        var spriteSheet = Bitmap.FromFile(path) as Bitmap;
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
        
        this.Location = new Point(300, 300);
        this.Size = 2 * size;

        AlwaysInvalidateMode();
    }
}

public class Test : View
{
    protected override void OnRender(IGraphics g)
    { 
        g.Clear(Color.White);
        Invalidate();
    }

    protected override void OnStart(IGraphics g)
    {
        g.SubscribeKeyDownEvent(input =>
        {
            switch (input)
            {
                case Input.Escape:
                    App.Close();
                    break;
            }
        });

        Content = new Container
        {
            button(ref bt,
                text: "...",
                rect: rect(50, 50, 200, 200),
                cornerRadius: 200,
                onMouseDown: delegate
                {
                    bt.Text = choose(
                        "Hello, Universe!",
                        "Olá, Universo!",
                        "Olá, Mundo!",
                        "Hello, World!"
                    );
                },
                onMouseUp: delegate
                {
                    bt.Text = "...";
                }
            ),
            button(
                rect: rect(270, 50, 200, 200),
                text: "Atualizar!",
                onMouseDown: delegate
                {
                    bt.Color = 
                    bt.SelectedColor = 
                    bt.PressedColor = 
                    choose(
                        Color.Blue,
                        Color.Red,
                        Color.Green
                    );
                }
            ),
            new Player()
        };
    }

    private Button bt;
}