using System.Drawing;
using System.Collections.Generic;

using Pamella;
using Pamella.Views;

public class Game : View
{
    protected override void OnFrame(IGraphics g)
    {
        
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
            Background.Open(
                Color.LightBlue,
                "sprites/back1.png",
                "sprites/back2.png"
            ),
            new Carbo(),
            new FPS()
        };
    
        AlwaysInvalidateMode();
    }
}