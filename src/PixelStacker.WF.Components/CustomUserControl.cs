//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace PixelStacker.WF.Components
//{
//    [Designer(typeof(PixelStacker.WF.Components.CustomWidgetDesigner))]
//    [ToolboxItemFilter("PixelStacker.WF.Components.CustomUserControl", ToolboxItemFilterType.Require)]
//    public partial class CustomUserControl : UserControl, ICustomWidget
//    {
//        public CustomUserControl()
//        {
//            InitializeComponent();
//            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
//        }

//        public virtual void CustomInitialize()
//        {
//        }

//        #region Implementation
//        protected override void OnLayout(LayoutEventArgs levent)
//        {
//            base.OnLayout(levent);

//            // Repaint when the layout has changed. 
//            this.Refresh();
//        }

//        protected override void OnPaint(PaintEventArgs e)
//        {
//            Graphics g = e.Graphics;
//            g.Clear(Color.Red);
//            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
//            base.OnPaint(e);
//        }
//        #endregion Implementation
//    }
//}
