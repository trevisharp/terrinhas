using System;
using System.Drawing;
using System.Numerics;

using Pamella;
using Pamella.Views;

public interface ICollider
{
    public RectangleF Rect { get; set; }
}