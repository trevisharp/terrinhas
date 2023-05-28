using System;
using System.Linq;
using System.Collections.Generic;

App.Open<Test>();

// public class Sprite : View
// {
//     Queue<int> queue = new Queue<int>();
//     DateTime last;
//     int k = 0;

//     int i;
//     RectangleF rect;
//     RectangleF frame;

//     Bitmap img = null;

//     protected override void OnStart(IGraphics g)
//     {   
//         img = Bitmap.FromFile("sprites/carbo.png") as Bitmap;
//         i = 6;
//         rect = new RectangleF(300, 300, 2 * 40, 2 * 56);
//         frame = new RectangleF(0, 0, 40, 56);
//         last = DateTime.Now;
//     }
 

//     protected override void OnRender(IGraphics g)
//     {
//         k++;
//         if ((DateTime.Now - last).TotalSeconds >= 0.1)
//         {
//             queue.Enqueue(k);
//             k = 0;
//             last = DateTime.Now;
//         }
//         if (queue.Count > 10)
//             queue.Dequeue();
//         var fps = queue.Count == 0 ? 0 : 10 * queue.Average();
//         g.DrawText(new RectangleF(500, 500, 100, 100), fps.ToString());
//         g.DrawImage(rect, img, frame);
        
//         frame = new RectangleF(0, 56 * i, 40, 56);

//         i++;
//         if (i > 19)
//             i = 6;
//     }
// }

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

        var controller = SpriteController.Load("sprites/carbo.png", 
            new Size(40, 56),
            new PointF(0, 56 * 6),
            new Size(0, 56), 14);

        sprite = new Sprite<int>();
        sprite.Animation.AddSprite(0, controller);
        sprite.Rect = new Rectangle(300, 300, 80, 112);

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
            sprite
        };
    }

    private Button bt;
    private Sprite<int> sprite;
}