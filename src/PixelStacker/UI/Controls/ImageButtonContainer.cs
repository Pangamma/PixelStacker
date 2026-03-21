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
        private SKBitmap _checkedFrameCache;
        private SKBitmap CheckedFrame => _checkedFrameCache ??= UIResources.selected_frame_128.BitmapToSKBitmap();



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
            IEnumerable<ImageButtonData> remaining = allItems;

            if (filter != null)
                remaining = remaining.Where(x => filter(x));

            if (orderBy != null)
                remaining = remaining.OrderBy(x => orderBy(x));

            if (take != null)
                remaining = remaining.Take(take.Value);

            this.ImageButtons = remaining.ToList();
        }


        private void SkHybridControl_MouseMove(object sender, MouseEventArgs e)
        {
            int tileWidth = ImageButtonSize.Width;
            int tileHeight = ImageButtonSize.Height;
            int margin = ImageButtonMargin;
            int cellWidth = tileWidth + margin * 2;
            int cellHeight = tileHeight + margin * 2;

            int totalWidth = skHybridControl.Width;
            int cols = Math.Max(1, totalWidth / cellWidth);
            int xOffset = (totalWidth - cols * cellWidth) / 2;

            // e.Y is already in skHybridControl virtual coords (the control is scroll-sized).
            // Adding scrolledY again would double the offset and break hit testing.
            int ax = e.X - xOffset;
            int ay = e.Y;

            int? newIndex = null;
            if (ax >= 0 && ay >= 0 && ImageButtons.Count > 0)
            {
                int col = ax / cellWidth;
                int xInCell = ax % cellWidth;
                int row = ay / cellHeight;
                int yInCell = ay % cellHeight;

                // tile occupies [0, tileWidth) in x, [margin, margin+tileHeight) in y within each cell
                if (col < cols && xInCell < tileWidth && yInCell >= margin && yInCell < margin + tileHeight)
                {
                    int idx = row * cols + col;
                    if (idx < ImageButtons.Count)
                        newIndex = idx;
                }
            }

            if (newIndex != hoveredIndex)
            {
                hoveredIndex = newIndex;
                skHybridControl.Refresh();
                TileHover?.Invoke(this, new ImageButtonClickEventArgs
                {
                    ImageButtonData = newIndex.HasValue ? ImageButtons[newIndex.Value] : null
                });
            }
        }


        private void skHybridControl_PaintSurface(object sender, GenericSKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var rect = e.Rect;

            canvas.Clear(SKColors.LightGray);

            int tileWidth = ImageButtonSize.Width;
            int tileHeight = ImageButtonSize.Height;
            int xMargin = ImageButtonMargin;
            int yMargin = ImageButtonMargin;
            int cellWidth = tileWidth + xMargin * 2;
            int cellHeight = tileHeight + yMargin * 2;

            int totalWidth = rect.Width;
            int cols = Math.Max(1, totalWidth / cellWidth);
            int xOffset = (totalWidth - cols * cellWidth) / 2;

            // rect.Height is the full virtual surface height (skHybridControl fills DisplayRectangle).
            // Use the actual visible viewport height for culling instead.
            int scrollY = -AutoScrollPosition.Y;
            int viewportHeight = this.ClientSize.Height;
            int firstRow = Math.Max(0, scrollY / cellHeight - 1);
            int lastRow = (scrollY + viewportHeight) / cellHeight + 1;
            int firstIdx = firstRow * cols;
            int lastIdx = Math.Min(ImageButtons.Count - 1, (lastRow + 1) * cols - 1);

            using SKPaint borderPaint = new SKPaint { Color = new SKColor(0, 0, 0), IsStroke = true, StrokeWidth = 2 };
            using SKPaint bgPaint = new SKPaint { Color = new SKColor(207, 207, 207) };
            using var highlight = new SKPaint { Color = new SKColor(0, 0, 0, 16) };

            for (int i = firstIdx; i <= lastIdx; i++)
            {
                int row = i / cols;
                int col = i % cols;

                int x = col * cellWidth + xOffset;
                int y = row * cellHeight + yMargin;

                var imgRect = new SKRect(x, y, x + tileWidth, y + tileHeight);
                canvas.DrawRect(imgRect, bgPaint);

                var data = ImageButtons[i];
                if (data.CachedImage != null)
                {
                    canvas.DrawImage(data.CachedImage, imgRect, Constants.SAMPLE_OPTS_NONE, bgPaint);
                }

                if (hoveredIndex == i)
                {
                    canvas.DrawRect(x, y, tileWidth, tileHeight, highlight);
                }

                canvas.DrawRect(x, y, tileWidth, tileHeight, borderPaint);
                if (data.IsChecked)
                {
                    canvas.DrawBitmap(CheckedFrame, imgRect, bgPaint);
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
        private SKBitmap _image;
        private SKImage _cachedImage;

        public SKBitmap Image
        {
            get => _image;
            set
            {
                if (_image == value) return;
                _image = value;
                var old = _cachedImage;
                _cachedImage = value != null ? SKImage.FromBitmap(value) : null;
                old?.Dispose();
            }
        }

        public SKImage CachedImage => _cachedImage;
        public string Text { get; set; }
        public bool IsChecked { get; set; }
        public object Data { get; set; }
        public T GetData<T>() => (T)Data;
    }

}
