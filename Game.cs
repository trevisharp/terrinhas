using System.Drawing;
using System.Collections.Generic;

using Pamella;
using Pamella.Views;

public class Game : View
{
    protected override void OnRender(IGraphics g)
    { 
        g.Clear(Color.White);
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
            new Player("sprites/carbo.png")
        };

        AlwaysInvalidateMode();
    }
}