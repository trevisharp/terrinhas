Test test = new Test();
App.Open(test);

public class Test : View
{
    private Button bt = null;

    protected override void OnRender(IGraphics g)
    {
        g.Clear(Color.Aqua);
        bt.Draw(g);
    }

    protected override void OnFrame(IGraphics g)
    {
        bt.Draw(g);
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

        bt = new Button();

        bt.BackColor = Color.Blue;
        bt.SelectedColor = Color.LightBlue;
        bt.PressedColor = Color.Yellow;

        bt.Text = "...";
        bt.Rect = new Rectangle(50, 50, 200, 200);

        bt.BorderColor = Color.Black;
        bt.SelectedBorderColor = Color.Orange;
        bt.PressedBorderColor = Color.Yellow;
        bt.BorderWidth = 2f;

        bt.CornerRadius = 50;

        bt.OnMouseDown += delegate
        {
            bt.Text = choose(
                "Hello, Universe!",
                "Olá, Universo!",
                "Olá, Mundo!",
                "Hello, World!"
            );
        };

        bt.OnMouseUp += delegate
        {
            bt.Text = "...";
        };
        bt.AlwaysInvalidateMode();
    }
}