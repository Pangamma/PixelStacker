using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Helpers
{
    public class ChildInfo
    {
        public Control Control { get; set; }
        public bool IsSnapped { get; set; }
        public Point ParentOffset { get; set; }
    }

    public class SnapManager
    {
        private Func<System.Drawing.Rectangle> GetBoundaries { get; set; }
        public Control Parent { get; }

        List<ChildInfo> Children = new List<ChildInfo>();

        public SnapManager(Control parent)
        {
            this.Parent = parent;
            this.Parent.Move += Parent_Move;
            this.Parent.Resize += Parent_Resize;
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            foreach (var child in Children)
            {
                if (child.IsSnapped)
                    child.IsSnapped = false;
            }
        }

        private void Parent_Move(object sender, EventArgs e)
        {
            foreach (var child in Children)
            {
                if (child.IsSnapped)
                {
                    child.Control.Location = this.Parent.PointToScreen(child.ParentOffset);
                }
            }
        }

        public void RegisterChild(Control child)
        {
            child.Move += Child_Move;
            child.Disposed += Child_Disposed;
            this.Children.Add(new ChildInfo()
            {
                Control = child,
                IsSnapped = false
            });
        }

        private void Child_Disposed(object sender, EventArgs e)
        {
            var child = (Control)sender;
            var info = Children.RemoveAll(x => x.Control == child);
        }

        private int Dist(int a, int b)
        {
            return Math.Abs(Math.Max(a, b) - Math.Min(a, b));
        }

        private void Child_Move(object sender, EventArgs e)
        {
            var child = (Control)sender;
            var TL = Parent.PointToClient(child.Location);
            var pRect = Parent.ClientRectangle;
            int padding = 0;
            int snapTolerance = 30;
            bool isSnapped = false;

            var snapBounds = new Rectangle(
                x: padding, // left snap point is 5px away from edge
                y: padding,

                // right snap point is the right edge, minus padding, minus width of child width
                width: pRect.Right - padding - child.Size.Width,
                height: pRect.Bottom - padding - child.Size.Height);

            if (Dist(TL.X, snapBounds.Left) < snapTolerance || Dist(TL.X, snapBounds.Right) < snapTolerance)
            {
                if (TL.X > -snapTolerance && TL.X < pRect.Right)
                {
                    TL.X = Math.Min(Math.Max(TL.X, padding), pRect.Right - child.Size.Width - padding);
                    isSnapped = true;
                }
            }

            if (Dist(TL.Y, snapBounds.Top) < snapTolerance || Dist(TL.Y, snapBounds.Bottom) < snapTolerance)
            {
                if (TL.Y > -snapTolerance && TL.Y < pRect.Bottom)
                {
                    TL.Y = Math.Min(Math.Max(TL.Y, padding), pRect.Bottom - child.Size.Height - padding);
                    isSnapped = true;
                }
            }

            var info = Children.FirstOrDefault(x => x.Control == child);
            info.ParentOffset = TL;
            info.IsSnapped = isSnapped;

            if (isSnapped)
            {
                child.Location = Parent.PointToScreen(TL);
            }
        }

        public Point GetRelativePosition(Control child)
        {
            var pp = Parent.PointToClient(child.Location);
            return pp;
        }
    }
}