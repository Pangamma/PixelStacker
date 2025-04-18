
namespace PixelStacker.UI.Controls
{
    partial class CanvasEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CanvasEditor));
            timerPaint = new System.Windows.Forms.Timer(components);
            skiaControl = new SkHybridControl();
            timerBufferedChangeQueue = new System.Windows.Forms.Timer(components);
            bgWorkerBufferedChangeQueue = new System.ComponentModel.BackgroundWorker();
            toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            tsCanvasTools = new System.Windows.Forms.ToolStrip();
            btnPointer = new System.Windows.Forms.ToolStripButton();
            btnWorldEditOrigin = new System.Windows.Forms.ToolStripButton();
            btnPencil = new System.Windows.Forms.ToolStripButton();
            btnBrush = new System.Windows.Forms.ToolStripButton();
            btnFill = new System.Windows.Forms.ToolStripButton();
            btnPicker = new System.Windows.Forms.ToolStripButton();
            btnEraser = new System.Windows.Forms.ToolStripButton();
            btnSuggester = new System.Windows.Forms.ToolStripButton();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            lblBrushWidth = new System.Windows.Forms.ToolStripLabel();
            btnBrushWidthMinus = new System.Windows.Forms.ToolStripButton();
            tbxBrushWidth = new System.Windows.Forms.ToolStripTextBox();
            btnBrushWidthAdd = new System.Windows.Forms.ToolStripButton();
            btnMaterialCombination = new System.Windows.Forms.ToolStripButton();
            lblHoverInfo = new System.Windows.Forms.ToolStripLabel();
            btnPaintLayerFilter = new System.Windows.Forms.ToolStripButton();
            PointerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            filterMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setWorldEditOriginHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripContainer.ContentPanel.SuspendLayout();
            toolStripContainer.LeftToolStripPanel.SuspendLayout();
            toolStripContainer.TopToolStripPanel.SuspendLayout();
            toolStripContainer.SuspendLayout();
            tsCanvasTools.SuspendLayout();
            toolStrip1.SuspendLayout();
            PointerContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // timerPaint
            // 
            timerPaint.Enabled = true;
            timerPaint.Interval = Logic.IO.Config.Constants.DisplayRefreshIntervalMs;
            timerPaint.Tick += timerPaint_Tick;
            // 
            // skiaControl
            // 
            skiaControl.BackColor = System.Drawing.Color.IndianRed;
            skiaControl.BackgroundImage = (System.Drawing.Image)resources.GetObject("skiaControl.BackgroundImage");
            skiaControl.BoxShadowOnEdges = Logic.IO.Config.Constants.SHOW_BOX_SHADOW_IN_UI;
            skiaControl.Dock = System.Windows.Forms.DockStyle.Fill;
            skiaControl.Location = new System.Drawing.Point(0, 0);
            skiaControl.Name = "skiaControl";
            skiaControl.Size = new System.Drawing.Size(561, 398);
            skiaControl.TabIndex = 0;
            skiaControl.PaintSurface += skiaControl_PaintSurface;
            skiaControl.MouseClick += ImagePanel_Click;
            skiaControl.MouseDoubleClick += ImagePanel_DoubleClick;
            skiaControl.MouseDown += ImagePanel_MouseDown;
            skiaControl.MouseMove += ImagePanel_MouseMove;
            skiaControl.MouseUp += ImagePanel_MouseUp;
            // 
            // timerBufferedChangeQueue
            // 
            timerBufferedChangeQueue.Enabled = true;
            timerBufferedChangeQueue.Interval = Logic.IO.Config.Constants.DisplayRefreshIntervalMs;
            timerBufferedChangeQueue.Tick += timerBufferedChangeQueue_Tick;
            // 
            // bgWorkerBufferedChangeQueue
            // 
            bgWorkerBufferedChangeQueue.DoWork += bgWorkerBufferedChangeQueue_DoWork;
            bgWorkerBufferedChangeQueue.RunWorkerCompleted += BgWorkerBufferedChangeQueue_RunWorkerCompleted;
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            toolStripContainer.BottomToolStripPanel.Enabled = false;
            toolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer.ContentPanel
            // 
            toolStripContainer.ContentPanel.Controls.Add(skiaControl);
            toolStripContainer.ContentPanel.Size = new System.Drawing.Size(561, 398);
            toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer.LeftToolStripPanel
            // 
            toolStripContainer.LeftToolStripPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            toolStripContainer.LeftToolStripPanel.Controls.Add(tsCanvasTools);
            toolStripContainer.LeftToolStripPanel.MinimumSize = new System.Drawing.Size(48, 0);
            toolStripContainer.Location = new System.Drawing.Point(0, 0);
            toolStripContainer.Name = "toolStripContainer";
            toolStripContainer.Size = new System.Drawing.Size(611, 441);
            toolStripContainer.TabIndex = 2;
            toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip1);
            // 
            // tsCanvasTools
            // 
            tsCanvasTools.Dock = System.Windows.Forms.DockStyle.None;
            tsCanvasTools.ImageScalingSize = new System.Drawing.Size(32, 32);
            tsCanvasTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnPointer, btnWorldEditOrigin, btnPencil, btnBrush, btnFill, btnPicker, btnEraser, btnSuggester });
            tsCanvasTools.Location = new System.Drawing.Point(0, 4);
            tsCanvasTools.MinimumSize = new System.Drawing.Size(48, 0);
            tsCanvasTools.Name = "tsCanvasTools";
            tsCanvasTools.Padding = new System.Windows.Forms.Padding(0);
            tsCanvasTools.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            tsCanvasTools.Size = new System.Drawing.Size(50, 292);
            tsCanvasTools.TabIndex = 0;
            tsCanvasTools.LayoutStyleChanged += toolstrip_LayoutStyleChanged;
            // 
            // btnPointer
            // 
            btnPointer.AutoSize = false;
            btnPointer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnPointer.Image = Resources.UIResources.pointer;
            btnPointer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPointer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            btnPointer.Name = "btnPointer";
            btnPointer.Size = new System.Drawing.Size(40, 40);
            btnPointer.ToolTipText = "Pencil";
            btnPointer.Click += Toolbox_OnClickPointer;
            // 
            // btnWorldEditOrigin
            // 
            btnWorldEditOrigin.AutoSize = false;
            btnWorldEditOrigin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnWorldEditOrigin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnWorldEditOrigin.Image = Resources.UIResources.compass;
            btnWorldEditOrigin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnWorldEditOrigin.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnWorldEditOrigin.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            btnWorldEditOrigin.Name = "btnWorldEditOrigin";
            btnWorldEditOrigin.Size = new System.Drawing.Size(40, 40);
            btnWorldEditOrigin.ToolTipText = "World Edit Origin";
            btnWorldEditOrigin.Click += Toolbox_OnClickWorldEditOrigin;
            // 
            // btnPencil
            // 
            btnPencil.AutoSize = false;
            btnPencil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnPencil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnPencil.Image = Resources.UIResources.pencil_1;
            btnPencil.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnPencil.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPencil.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            btnPencil.Name = "btnPencil";
            btnPencil.Size = new System.Drawing.Size(40, 40);
            btnPencil.ToolTipText = "Pencil";
            btnPencil.Click += Toolbox_OnClickPencil;
            // 
            // btnBrush
            // 
            btnBrush.AutoSize = false;
            btnBrush.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnBrush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnBrush.Image = Resources.UIResources.paintbrush;
            btnBrush.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnBrush.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnBrush.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            btnBrush.Name = "btnBrush";
            btnBrush.Size = new System.Drawing.Size(40, 40);
            btnBrush.ToolTipText = "Paint brush";
            btnBrush.Click += Toolbox_OnClickBrush;
            // 
            // btnFill
            // 
            btnFill.AutoSize = false;
            btnFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnFill.Image = Resources.UIResources.paint_bucket;
            btnFill.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFill.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            btnFill.Name = "btnFill";
            btnFill.Size = new System.Drawing.Size(40, 40);
            btnFill.ToolTipText = "Fill";
            btnFill.Click += Toolbox_OnClickFill;
            // 
            // btnPicker
            // 
            btnPicker.AutoSize = false;
            btnPicker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnPicker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnPicker.Image = Resources.UIResources.dropper;
            btnPicker.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnPicker.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPicker.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            btnPicker.Name = "btnPicker";
            btnPicker.Size = new System.Drawing.Size(40, 40);
            btnPicker.ToolTipText = "Color Picker";
            btnPicker.Click += Toolbox_OnClickPicker;
            // 
            // btnEraser
            // 
            btnEraser.AutoSize = false;
            btnEraser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnEraser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnEraser.Image = Resources.UIResources.eraser;
            btnEraser.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnEraser.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnEraser.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            btnEraser.Name = "btnEraser";
            btnEraser.Size = new System.Drawing.Size(40, 40);
            btnEraser.ToolTipText = "Eraser";
            btnEraser.Click += Toolbox_OnClickEraser;
            // 
            // btnSuggester
            // 
            btnSuggester.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnSuggester.Image = Resources.UIResources.color;
            btnSuggester.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnSuggester.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnSuggester.Name = "btnSuggester";
            btnSuggester.Size = new System.Drawing.Size(49, 36);
            btnSuggester.Text = "Color Suggestions";
            btnSuggester.Visible = false;
            btnSuggester.Click += btnSuggester_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(38, 38);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblBrushWidth, btnBrushWidthMinus, tbxBrushWidth, btnBrushWidthAdd, btnMaterialCombination, lblHoverInfo, btnPaintLayerFilter });
            toolStrip1.Location = new System.Drawing.Point(4, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(433, 43);
            toolStrip1.TabIndex = 0;
            // 
            // lblBrushWidth
            // 
            lblBrushWidth.Name = "lblBrushWidth";
            lblBrushWidth.Size = new System.Drawing.Size(86, 40);
            lblBrushWidth.Text = "Brush width";
            // 
            // btnBrushWidthMinus
            // 
            btnBrushWidthMinus.AutoSize = false;
            btnBrushWidthMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnBrushWidthMinus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnBrushWidthMinus.Image = (System.Drawing.Image)resources.GetObject("btnBrushWidthMinus.Image");
            btnBrushWidthMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnBrushWidthMinus.Name = "btnBrushWidthMinus";
            btnBrushWidthMinus.Size = new System.Drawing.Size(24, 24);
            btnBrushWidthMinus.Text = "-";
            btnBrushWidthMinus.Click += btnBrushWidthMinus_Click;
            // 
            // tbxBrushWidth
            // 
            tbxBrushWidth.AutoSize = false;
            tbxBrushWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbxBrushWidth.Name = "tbxBrushWidth";
            tbxBrushWidth.Size = new System.Drawing.Size(50, 27);
            tbxBrushWidth.KeyPress += tbxBrushWidth_KeyPress;
            tbxBrushWidth.TextChanged += tbxBrushWidth_TextChanged;
            // 
            // btnBrushWidthAdd
            // 
            btnBrushWidthAdd.AutoSize = false;
            btnBrushWidthAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnBrushWidthAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnBrushWidthAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnBrushWidthAdd.Name = "btnBrushWidthAdd";
            btnBrushWidthAdd.Size = new System.Drawing.Size(24, 24);
            btnBrushWidthAdd.Text = "+";
            btnBrushWidthAdd.Click += btnBrushWidthAdd_Click;
            // 
            // btnMaterialCombination
            // 
            btnMaterialCombination.AutoSize = false;
            btnMaterialCombination.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnMaterialCombination.Image = (System.Drawing.Image)resources.GetObject("btnMaterialCombination.Image");
            btnMaterialCombination.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnMaterialCombination.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            btnMaterialCombination.Name = "btnMaterialCombination";
            btnMaterialCombination.Size = new System.Drawing.Size(40, 40);
            btnMaterialCombination.Text = "toolStripButton1";
            btnMaterialCombination.Click += btnMaterialCombination_Click;
            // 
            // lblHoverInfo
            // 
            lblHoverInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            lblHoverInfo.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            lblHoverInfo.Name = "lblHoverInfo";
            lblHoverInfo.Size = new System.Drawing.Size(124, 40);
            lblHoverInfo.Text = "Mouse hover info";
            // 
            // btnPaintLayerFilter
            // 
            btnPaintLayerFilter.AutoSize = false;
            btnPaintLayerFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnPaintLayerFilter.Image = Resources.UIResources.iso2_both_layers;
            btnPaintLayerFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPaintLayerFilter.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            btnPaintLayerFilter.Name = "btnPaintLayerFilter";
            btnPaintLayerFilter.Size = new System.Drawing.Size(40, 40);
            btnPaintLayerFilter.Text = "toolStripButton1";
            btnPaintLayerFilter.Click += btnPaintLayerFilter_Click;
            // 
            // PointerContextMenuStrip
            // 
            PointerContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            PointerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { filterMaterialsToolStripMenuItem, setWorldEditOriginHereToolStripMenuItem });
            PointerContextMenuStrip.Name = "PointerContextMenuStrip";
            PointerContextMenuStrip.Size = new System.Drawing.Size(213, 80);
            // 
            // filterMaterialsToolStripMenuItem
            // 
            filterMaterialsToolStripMenuItem.Name = "filterMaterialsToolStripMenuItem";
            filterMaterialsToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            filterMaterialsToolStripMenuItem.Text = "Filter Materials";
            // 
            // setWorldEditOriginHereToolStripMenuItem
            // 
            setWorldEditOriginHereToolStripMenuItem.Name = "setWorldEditOriginHereToolStripMenuItem";
            setWorldEditOriginHereToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            setWorldEditOriginHereToolStripMenuItem.Text = "Set WorldEdit origin";
            setWorldEditOriginHereToolStripMenuItem.Click += setWorldEditOriginHereToolStripMenuItem_Click;
            // 
            // CanvasEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImage = Resources.UIResources.bg_imagepanel;
            Controls.Add(toolStripContainer);
            Name = "CanvasEditor";
            Size = new System.Drawing.Size(611, 441);
            Load += CanvasEditor_Load;
            toolStripContainer.ContentPanel.ResumeLayout(false);
            toolStripContainer.LeftToolStripPanel.ResumeLayout(false);
            toolStripContainer.LeftToolStripPanel.PerformLayout();
            toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.PerformLayout();
            toolStripContainer.ResumeLayout(false);
            toolStripContainer.PerformLayout();
            tsCanvasTools.ResumeLayout(false);
            tsCanvasTools.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            PointerContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerPaint;
        private Controls.SkHybridControl skiaControl;
        private System.Windows.Forms.Timer timerBufferedChangeQueue;
        private System.ComponentModel.BackgroundWorker bgWorkerBufferedChangeQueue;
        private System.Windows.Forms.ToolStrip tsCanvasTools;
        private System.Windows.Forms.ToolStripButton btnWorldEditOrigin;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblBrushWidth;
        private System.Windows.Forms.ToolStripButton btnBrushWidthAdd;
        private System.Windows.Forms.ToolStripTextBox tbxBrushWidth;
        private System.Windows.Forms.ToolStripButton btnBrushWidthMinus;
        private System.Windows.Forms.ToolStripButton btnEraser;
        private System.Windows.Forms.ToolStripButton btnPencil;
        private System.Windows.Forms.ToolStripButton btnBrush;
        private System.Windows.Forms.ToolStripButton btnFill;
        private System.Windows.Forms.ToolStripButton btnPicker;
        private System.Windows.Forms.ToolStripLabel lblHoverInfo;
        private System.Windows.Forms.ToolStripButton btnMaterialCombination;
        public System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStripButton btnPaintLayerFilter;
        private System.Windows.Forms.ToolStripButton btnSuggester;
        private System.Windows.Forms.ToolStripButton btnPointer;
        public System.Windows.Forms.ContextMenuStrip PointerContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem filterMaterialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWorldEditOriginHereToolStripMenuItem;
    }
}
