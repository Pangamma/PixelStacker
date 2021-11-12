using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public enum PxMouseButton
    {
        Left, Middle, Right
    }

    public class PxClickEvent 
    {
        public PxPoint Location { get; set; }
        public PxMouseButton Button { get; set; }
        public bool IsShiftHeld { get; set; } = false;
        public bool IsControlHeld { get; set; } = false;
        public bool IsAltHeld { get; set; } = false;
    }

    public abstract class AbstractCanvasEditorTool<TSettings> : AbstractCanvasEditorTool where TSettings : class
    {
        public TSettings Settings { get; set; }
        protected AbstractCanvasEditorTool(CanvasEditor editor) : base(editor)
        {
        }

    }

    public abstract class AbstractCanvasEditorTool
    {
        protected CanvasEditor CanvasEditor { get; }

        public AbstractCanvasEditorTool(CanvasEditor editor) {
            this.CanvasEditor = editor;
        }

        public abstract void OnClick(MouseEventArgs e);
        public abstract void OnMouseDown(MouseEventArgs e);
        public abstract void OnMouseUp(MouseEventArgs e);
        public abstract void OnMouseMove(MouseEventArgs e);


        private PxPoint GetPointOnImage(PxPoint pointOnPanel, PanZoomSettings pz, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new PxPoint((int)Math.Ceiling((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new PxPoint((int)Math.Floor((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Floor((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            return new PxPoint((int)Math.Round((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Round((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
        }

        public static PxPoint GetPointOnPanel(PxPoint pointOnImage, PanZoomSettings pz)
        {
            if (pz == null)
            {
#if DEBUG
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                        return new PxPoint(0, 0);
#endif
            }

            return new PxPoint((int)Math.Round(pointOnImage.X * pz.zoomLevel + pz.imageX), (int)Math.Round(pointOnImage.Y * pz.zoomLevel + pz.imageY));
        }
    }

}
