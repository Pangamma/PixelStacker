using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    /// <summary>
    /// Single SkiaSharp-rendered panel that draws a grid of material tiles.
    /// Uses SkHybridControl (GPU when available) to match ImageButtonContainer performance.
    /// No SKBitmap→Bitmap conversion; materials are cached as SKImage directly.
    /// </summary>
    public class MaterialCategoryGridPanel : Panel
    {
        private const int TileSize = 64;
        private const int TileMargin = 3;
        private const int CellSize = TileSize + TileMargin * 2;

        private List<Material> _visibleMaterials = new();
        private readonly Dictionary<(string id, bool isv), SKImage> _imageCache = new();
        private SKBitmap _checkedFrameCache;
        private SKBitmap CheckedFrame => _checkedFrameCache ??= UIResources.selected_frame_128.BitmapToSKBitmap();

        private readonly Options _opts;
        private int _hoveredIndex = -1;
        private int _lastClickedIndex = -1;

        private readonly SkHybridControl _skControl;

        public event EventHandler<Material> MaterialClicked;
        public event EventHandler<Material> MaterialHovered;

        public IReadOnlyList<Material> VisibleMaterials => _visibleMaterials;

        public MaterialCategoryGridPanel(Options opts)
        {
            _opts = opts;

            _skControl = new SkHybridControl();
            _skControl.Dock = DockStyle.Fill;
            _skControl.PaintSurface += OnSkPaintSurface;
            _skControl.MouseMove += OnSkMouseMove;
            _skControl.MouseLeave += OnSkMouseLeave;
            _skControl.MouseClick += OnSkMouseClick;
            Controls.Add(_skControl);
        }

        public void SetVisibleMaterials(IEnumerable<Material> materials)
        {
            _visibleMaterials = new List<Material>(materials);
            _hoveredIndex = -1;
            UpdateHeight();
            _skControl.Refresh();
        }

        public void InvalidateBitmapCache()
        {
            foreach (var img in _imageCache.Values) img?.Dispose();
            _imageCache.Clear();
            _skControl.Refresh();
        }

        private int GetCols() => Math.Max(1, Width / CellSize);

        private void UpdateHeight()
        {
            int cols = GetCols();
            int rows = _visibleMaterials.Count == 0 ? 0 : (int)Math.Ceiling((double)_visibleMaterials.Count / cols);
            int newHeight = rows * CellSize;
            if (Height != newHeight)
                Height = newHeight;
        }

        private SKImage GetImage(Material m)
        {
            bool isv = _opts?.IsSideView ?? false;
            var key = (m.PixelStackerID, isv);
            if (!_imageCache.TryGetValue(key, out var img))
            {
                img = SKImage.FromBitmap(m.GetImage(isv));
                _imageCache[key] = img;
            }
            return img;
        }

        private void OnSkPaintSurface(object sender, GenericSKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.LightGray);

            int cols = GetCols();

            using var bgPaint = new SKPaint { Color = new SKColor(207, 207, 207) };
            using var borderPaint = new SKPaint { Color = SKColors.Black, IsStroke = true, StrokeWidth = 1 };
            using var hoverPaint = new SKPaint { Color = new SKColor(255, 255, 255, 100) };

            for (int i = 0; i < _visibleMaterials.Count; i++)
            {
                int row = i / cols;
                int col = i % cols;
                int x = col * CellSize + TileMargin;
                int y = row * CellSize + TileMargin;
                var tileRect = new SKRect(x, y, x + TileSize, y + TileSize);

                canvas.DrawRect(tileRect, bgPaint);

                var img = GetImage(_visibleMaterials[i]);
                if (img != null)
                    canvas.DrawImage(img, tileRect);

                if (_visibleMaterials[i].IsEnabledF(_opts))
                    canvas.DrawBitmap(CheckedFrame, tileRect);

                if (_hoveredIndex == i)
                    canvas.DrawRect(tileRect, hoverPaint);

                canvas.DrawRect(x, y, TileSize, TileSize, borderPaint);
            }
        }

        private int HitTest(int mx, int my)
        {
            int cols = GetCols();
            int col = mx / CellSize;
            int row = my / CellSize;
            int xInCell = mx % CellSize;
            int yInCell = my % CellSize;
            if (xInCell >= TileMargin && xInCell < TileMargin + TileSize &&
                yInCell >= TileMargin && yInCell < TileMargin + TileSize)
            {
                int idx = row * cols + col;
                if (idx >= 0 && idx < _visibleMaterials.Count)
                    return idx;
            }
            return -1;
        }

        private void OnSkMouseMove(object sender, MouseEventArgs e)
        {
            int newHovered = HitTest(e.X, e.Y);
            if (newHovered != _hoveredIndex)
            {
                _hoveredIndex = newHovered;
                _skControl.Refresh();
                MaterialHovered?.Invoke(this, newHovered >= 0 ? _visibleMaterials[newHovered] : null);
            }
        }

        private void OnSkMouseLeave(object sender, EventArgs e)
        {
            if (_hoveredIndex != -1)
            {
                _hoveredIndex = -1;
                _skControl.Refresh();
                MaterialHovered?.Invoke(this, null);
            }
        }

        private void OnSkMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            int idx = HitTest(e.X, e.Y);
            if (idx < 0) return;

            var mat = _visibleMaterials[idx];
            if (ModifierKeys.HasFlag(Keys.Shift) && _lastClickedIndex >= 0)
            {
                int min = Math.Min(_lastClickedIndex, idx);
                int max = Math.Max(_lastClickedIndex, idx);
                bool targetState = !ModifierKeys.HasFlag(Keys.Control);
                for (int i = min; i <= max; i++)
                    _visibleMaterials[i].IsEnabledF(_opts, targetState);
            }
            else
            {
                _lastClickedIndex = idx;
                mat.IsEnabledF(_opts, !mat.IsEnabledF(_opts));
            }

            MaterialClicked?.Invoke(this, mat);
            _skControl.Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateHeight();
            _skControl.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var img in _imageCache.Values) img?.Dispose();
                _imageCache.Clear();
                _checkedFrameCache?.Dispose();
                _checkedFrameCache = null;
            }
            base.Dispose(disposing);
        }
    }
}
