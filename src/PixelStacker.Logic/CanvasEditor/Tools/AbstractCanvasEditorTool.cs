using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor.Tools
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

    public abstract class AbstractCanvasEditorTool<TSettings> where TSettings : class
    {
        public TSettings Settings { get; set; }
        public abstract void OnClick(PxClickEvent e);
    }
}
