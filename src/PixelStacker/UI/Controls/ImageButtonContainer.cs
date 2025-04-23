using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SkiaSharp;
using PixelStacker.Resources;
using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;

namespace PixelStacker.UI.Controls
{
    [ToolboxItemFilter("PixelStacker.UI.Controls.ImageButtonContainer", ToolboxItemFilterType.Require)]
    public partial class ImageButtonContainer : ScrollableControl
    {
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler<ImageButtonClickEventArgs> TileClicked;


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler<ImageButtonClickEventArgs> TileHover;


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public List<ImageButtonData> ImageButtons { get => _imageButtons; set {
                _imageButtons = value ?? new List<ImageButtonData>();
                UpdateScrollSize();
                this.Refresh();
        } }

        private List<ImageButtonData> _imageButtons = new List<ImageButtonData>();


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Size ImageButtonSize { get; set; } = new Size(80, 80);


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int ImageButtonMargin { get; set; } = 3; 
        
        private int? hoveredIndex = null;



        public ImageButtonContainer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            DoubleBuffered = true;
            AutoScroll = true;
            skHybridControl.MouseMove += SkHybridControl_MouseMove;
            skHybridControl.MouseClick += SkHybridControl_MouseClick;
            UpdateScrollSize();
        }

        private void UpdateScrollSize()
        {
            int tileWidth = ImageButtonSize.Width + 2 * ImageButtonMargin;
            int tileHeight = ImageButtonSize.Height + 2 * ImageButtonMargin;

            int cols = Math.Max(1, Width / tileWidth);
            int rows = (int)Math.Ceiling((float)ImageButtons.Count / cols);

            int totalHeight = rows * tileHeight;

            AutoScrollMinSize = new Size(0, totalHeight);
        }

        public void DoFilterTakeOrderByOperation<TKey>(
            IEnumerable<ImageButtonData> allItems,
            Func<ImageButtonData, bool> filter = null,
            Func<ImageButtonData, TKey> orderBy = null,
            int? take = null
            )
        {
            try
            {
                IEnumerable<ImageButtonData> remaining = allItems;

                if (filter != null)
                {
                    remaining = remaining.Where(x => filter(x));
                }

                if (orderBy != null)
                {
                    remaining = remaining.OrderBy(x => orderBy(x));
                }


                if (take != null)
                {
                    remaining = remaining.Take(take.Value);
                }

                this.ImageButtons = remaining.ToList();
            }
            finally
            {
                this.Refresh();
            }
        }


        private void SkHybridControl_MouseMove(object sender, MouseEventArgs e)
        {
            int tileWidth = ImageButtonSize.Width;
            int tileHeight = ImageButtonSize.Height;
            int margin = ImageButtonMargin;

            int totalWidth = skHybridControl.Width;
            int cols = Math.Max(1, totalWidth / (tileWidth + margin * 2));
            int xOffset = (totalWidth - cols * (tileWidth + margin * 2)) / 2;

            int scrolledY = -AutoScrollPosition.Y;
            Point translated = new Point(e.X, e.Y + scrolledY);

            for (int i = 0; i < ImageButtons.Count; i++)
            {
                int row = i / cols;
                int col = i % cols;

                int x = col * (tileWidth + margin * 2) + xOffset;
                int y = row * (tileHeight + margin * 2) + margin + scrolledY;
                //var (x, y, _, _) = GetTileBounds(i);


                Rectangle bounds = new Rectangle(x, y, tileWidth, tileHeight);
                if (bounds.Contains(translated))
                {
                    if (hoveredIndex != i)
                    {
                        hoveredIndex = i;
                        skHybridControl.Refresh();
                        if (this.TileHover != null)
                        {
                            this.TileHover.Invoke(this, new ImageButtonClickEventArgs()
                            {
                                ImageButtonData = ImageButtons[i]
                            });
                        }
                    }
                    return;
                }
            }

            if (hoveredIndex != null)
            {
                hoveredIndex = null;
                skHybridControl.Refresh();
                if (this.TileHover != null)
                {
                    this.TileHover.Invoke(this, new ImageButtonClickEventArgs()
                    {
                        ImageButtonData = null
                    });
                }
            }
        }


        private void skHybridControl_PaintSurface(object sender, GenericSKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var rect = e.Rect;

            canvas.Clear(SKColors.LightGray);
            //canvas.Translate(0, AutoScrollPosition.Y); // scrolls with the mouse

            int tileWidth = ImageButtonSize.Width;
            int tileHeight = ImageButtonSize.Height;
            int xMargin = ImageButtonMargin;
            int yMargin = ImageButtonMargin;

            int totalWidth = rect.Width;
            int cols = Math.Max(1, totalWidth / (tileWidth + xMargin * 2));
            int xOffset = (totalWidth - cols * (tileWidth + xMargin * 2)) / 2;

            using SKPaint borderPaint = new SKPaint { Color = new SKColor(0, 0, 0), IsStroke = true, StrokeWidth = 2 };
            using SKPaint bgPaint = new SKPaint { Color = new SKColor(207, 207, 207) };
            using var checkedFrame = UIResources.selected_frame_128.BitmapToSKBitmap();

            for (int i = 0; i < ImageButtons.Count; i++)
            {
                int row = i / cols;
                int col = i % cols;

                int x = col * (tileWidth + xMargin * 2) + xOffset;
                int y = row * (tileHeight + yMargin * 2) + yMargin;

                //var (x, y, _, _) = GetTileBounds(i);
                // Skip rendering if not visible
                if (y + tileHeight < -AutoScrollPosition.Y || y > -AutoScrollPosition.Y + rect.Height)
                    continue;

                var imgRect = new SKRect(x, y, x + tileWidth, y + tileHeight);
                canvas.DrawRect(imgRect, bgPaint);

                var data = ImageButtons[i];
                if (data.Image != null)
                {
                    using var img = SKImage.FromBitmap(data.Image);
                    canvas.DrawImage(img, imgRect, Constants.SAMPLE_OPTS_NONE, bgPaint);
                }

                if (hoveredIndex == i)
                {
                    using var highlight = new SKPaint { Color = new SKColor(0, 0, 0, 16) };
                    canvas.DrawRect(x, y, tileWidth, tileHeight, highlight);
                }

                canvas.DrawRect(x, y, tileWidth, tileHeight, borderPaint);
                if (data.IsChecked)
                {
                    canvas.DrawBitmap(checkedFrame, imgRect, bgPaint);
                }
            }
        }

        private void SkHybridControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (hoveredIndex is int i && i >= 0 && i < ImageButtons.Count)
            {
                TileClicked?.Invoke(this, new ImageButtonClickEventArgs
                {
                    ImageButtonData = ImageButtons[i]
                });
                this.Refresh();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateScrollSize();
            skHybridControl.Invalidate();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            skHybridControl.Refresh();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            skHybridControl.Refresh();
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            // Prevent automatic scrolling to any control when clicked
            return this.AutoScrollPosition;
        }
    }

    public class ImageButtonClickEventArgs : EventArgs
    {
        public ImageButtonData ImageButtonData { get; set; }
    }

    public class ImageButtonData
    {
        public SKBitmap Image { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }

        public object Data { get; set; }
        public T GetData<T>() => (T)Data;
    }

}
