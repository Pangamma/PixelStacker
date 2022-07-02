
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CanvasEditor));
            this.timerPaint = new System.Windows.Forms.Timer(this.components);
            this.skiaControl = new PixelStacker.UI.Controls.SkHybridControl();
            this.timerBufferedChangeQueue = new System.Windows.Forms.Timer(this.components);
            this.bgWorkerBufferedChangeQueue = new System.ComponentModel.BackgroundWorker();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.tsCanvasTools = new System.Windows.Forms.ToolStrip();
            this.btnPanZoom = new System.Windows.Forms.ToolStripButton();
            this.btnWorldEditOrigin = new System.Windows.Forms.ToolStripButton();
            this.btnPencil = new System.Windows.Forms.ToolStripButton();
            this.btnBrush = new System.Windows.Forms.ToolStripButton();
            this.btnFill = new System.Windows.Forms.ToolStripButton();
            this.btnPicker = new System.Windows.Forms.ToolStripButton();
            this.btnEraser = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblBrushWidth = new System.Windows.Forms.ToolStripLabel();
            this.btnBrushWidthMinus = new System.Windows.Forms.ToolStripButton();
            this.tbxBrushWidth = new System.Windows.Forms.ToolStripTextBox();
            this.btnBrushWidthAdd = new System.Windows.Forms.ToolStripButton();
            this.btnMaterialCombination = new System.Windows.Forms.ToolStripButton();
            this.lblHoverInfo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.tsCanvasTools.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerPaint
            // 
            this.timerPaint.Enabled = true;
            this.timerPaint.Interval = 2;
            this.timerPaint.Tick += new System.EventHandler(this.timerPaint_Tick);
            // 
            // skiaControl
            // 
            this.skiaControl.BackColor = System.Drawing.Color.IndianRed;
            this.skiaControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("skiaControl.BackgroundImage")));
            this.skiaControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skiaControl.Location = new System.Drawing.Point(0, 0);
            this.skiaControl.Name = "skiaControl";
            this.skiaControl.Size = new System.Drawing.Size(561, 355);
            this.skiaControl.TabIndex = 0;
            this.skiaControl.PaintSurface += new System.EventHandler<PixelStacker.UI.Controls.GenericSKPaintSurfaceEventArgs>(this.skiaControl_PaintSurface);
            this.skiaControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_Click);
            this.skiaControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_DoubleClick);
            this.skiaControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseDown);
            this.skiaControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseMove);
            this.skiaControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseUp);
            // 
            // timerBufferedChangeQueue
            // 
            this.timerBufferedChangeQueue.Enabled = true;
            this.timerBufferedChangeQueue.Interval = 3;
            this.timerBufferedChangeQueue.Tick += new System.EventHandler(this.timerBufferedChangeQueue_Tick);
            // 
            // bgWorkerBufferedChangeQueue
            // 
            this.bgWorkerBufferedChangeQueue.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerBufferedChangeQueue_DoWork);
            this.bgWorkerBufferedChangeQueue.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgWorkerBufferedChangeQueue_RunWorkerCompleted);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Enabled = false;
            this.toolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.skiaControl);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(561, 355);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer.LeftToolStripPanel
            // 
            this.toolStripContainer.LeftToolStripPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolStripContainer.LeftToolStripPanel.Controls.Add(this.tsCanvasTools);
            this.toolStripContainer.LeftToolStripPanel.MinimumSize = new System.Drawing.Size(48, 0);
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(611, 382);
            this.toolStripContainer.TabIndex = 2;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // tsCanvasTools
            // 
            this.tsCanvasTools.Dock = System.Windows.Forms.DockStyle.None;
            this.tsCanvasTools.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsCanvasTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPanZoom,
            this.btnWorldEditOrigin,
            this.btnPencil,
            this.btnBrush,
            this.btnFill,
            this.btnPicker,
            this.btnEraser});
            this.tsCanvasTools.Location = new System.Drawing.Point(0, 4);
            this.tsCanvasTools.MinimumSize = new System.Drawing.Size(48, 0);
            this.tsCanvasTools.Name = "tsCanvasTools";
            this.tsCanvasTools.Padding = new System.Windows.Forms.Padding(0);
            this.tsCanvasTools.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tsCanvasTools.Size = new System.Drawing.Size(50, 292);
            this.tsCanvasTools.TabIndex = 0;
            this.tsCanvasTools.LayoutStyleChanged += new System.EventHandler(this.toolstrip_LayoutStyleChanged);
            // 
            // btnPanZoom
            // 
            this.btnPanZoom.AutoSize = false;
            this.btnPanZoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(221)))), ((int)(((byte)(245)))));
            this.btnPanZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPanZoom.Checked = true;
            this.btnPanZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnPanZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPanZoom.Image = global::PixelStacker.Resources.UIResources.all_directions;
            this.btnPanZoom.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPanZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPanZoom.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnPanZoom.Name = "btnPanZoom";
            this.btnPanZoom.Size = new System.Drawing.Size(40, 40);
            this.btnPanZoom.ToolTipText = "Pan and Zoom";
            this.btnPanZoom.Click += new System.EventHandler(this.Toolbox_OnClickPanZoom);
            // 
            // btnWorldEditOrigin
            // 
            this.btnWorldEditOrigin.AutoSize = false;
            this.btnWorldEditOrigin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnWorldEditOrigin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnWorldEditOrigin.Image = global::PixelStacker.Resources.UIResources.compass;
            this.btnWorldEditOrigin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnWorldEditOrigin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnWorldEditOrigin.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnWorldEditOrigin.Name = "btnWorldEditOrigin";
            this.btnWorldEditOrigin.Size = new System.Drawing.Size(40, 40);
            this.btnWorldEditOrigin.ToolTipText = "World Edit Origin";
            this.btnWorldEditOrigin.Click += new System.EventHandler(this.Toolbox_OnClickWorldEditOrigin);
            // 
            // btnPencil
            // 
            this.btnPencil.AutoSize = false;
            this.btnPencil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPencil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPencil.Image = global::PixelStacker.Resources.UIResources.pencil_1;
            this.btnPencil.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPencil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPencil.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnPencil.Name = "btnPencil";
            this.btnPencil.Size = new System.Drawing.Size(40, 40);
            this.btnPencil.ToolTipText = "Pencil";
            this.btnPencil.Click += new System.EventHandler(this.Toolbox_OnClickPencil);
            // 
            // btnBrush
            // 
            this.btnBrush.AutoSize = false;
            this.btnBrush.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBrush.Image = global::PixelStacker.Resources.UIResources.paintbrush;
            this.btnBrush.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnBrush.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBrush.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnBrush.Name = "btnBrush";
            this.btnBrush.Size = new System.Drawing.Size(40, 40);
            this.btnBrush.ToolTipText = "Paint brush";
            this.btnBrush.Click += new System.EventHandler(this.Toolbox_OnClickBrush);
            // 
            // btnFill
            // 
            this.btnFill.AutoSize = false;
            this.btnFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFill.Image = global::PixelStacker.Resources.UIResources.paint_bucket;
            this.btnFill.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFill.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(40, 40);
            this.btnFill.ToolTipText = "Fill";
            this.btnFill.Click += new System.EventHandler(this.Toolbox_OnClickFill);
            // 
            // btnPicker
            // 
            this.btnPicker.AutoSize = false;
            this.btnPicker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPicker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPicker.Image = global::PixelStacker.Resources.UIResources.dropper;
            this.btnPicker.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPicker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPicker.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnPicker.Name = "btnPicker";
            this.btnPicker.Size = new System.Drawing.Size(40, 40);
            this.btnPicker.ToolTipText = "Color Picker";
            this.btnPicker.Click += new System.EventHandler(this.Toolbox_OnClickPicker);
            // 
            // btnEraser
            // 
            this.btnEraser.AutoSize = false;
            this.btnEraser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEraser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEraser.Image = global::PixelStacker.Resources.UIResources.eraser;
            this.btnEraser.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnEraser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEraser.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnEraser.Name = "btnEraser";
            this.btnEraser.Size = new System.Drawing.Size(40, 40);
            this.btnEraser.ToolTipText = "Eraser";
            this.btnEraser.Click += new System.EventHandler(this.Toolbox_OnClickEraser);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblBrushWidth,
            this.btnBrushWidthMinus,
            this.tbxBrushWidth,
            this.btnBrushWidthAdd,
            this.btnMaterialCombination,
            this.lblHoverInfo});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(391, 27);
            this.toolStrip1.TabIndex = 0;
            // 
            // lblBrushWidth
            // 
            this.lblBrushWidth.Name = "lblBrushWidth";
            this.lblBrushWidth.Size = new System.Drawing.Size(86, 24);
            this.lblBrushWidth.Text = "Brush width";
            // 
            // btnBrushWidthMinus
            // 
            this.btnBrushWidthMinus.AutoSize = false;
            this.btnBrushWidthMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBrushWidthMinus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBrushWidthMinus.Image = ((System.Drawing.Image)(resources.GetObject("btnBrushWidthMinus.Image")));
            this.btnBrushWidthMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBrushWidthMinus.Name = "btnBrushWidthMinus";
            this.btnBrushWidthMinus.Size = new System.Drawing.Size(24, 24);
            this.btnBrushWidthMinus.Text = "-";
            this.btnBrushWidthMinus.Click += new System.EventHandler(this.btnBrushWidthMinus_Click);
            // 
            // tbxBrushWidth
            // 
            this.tbxBrushWidth.AutoSize = false;
            this.tbxBrushWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxBrushWidth.Name = "tbxBrushWidth";
            this.tbxBrushWidth.Size = new System.Drawing.Size(50, 27);
            this.tbxBrushWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxBrushWidth_KeyPress);
            this.tbxBrushWidth.TextChanged += new System.EventHandler(this.tbxBrushWidth_TextChanged);
            // 
            // btnBrushWidthAdd
            // 
            this.btnBrushWidthAdd.AutoSize = false;
            this.btnBrushWidthAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBrushWidthAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBrushWidthAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBrushWidthAdd.Name = "btnBrushWidthAdd";
            this.btnBrushWidthAdd.Size = new System.Drawing.Size(24, 24);
            this.btnBrushWidthAdd.Text = "+";
            this.btnBrushWidthAdd.Click += new System.EventHandler(this.btnBrushWidthAdd_Click);
            // 
            // btnMaterialCombination
            // 
            this.btnMaterialCombination.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMaterialCombination.Image = ((System.Drawing.Image)(resources.GetObject("btnMaterialCombination.Image")));
            this.btnMaterialCombination.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialCombination.Name = "btnMaterialCombination";
            this.btnMaterialCombination.Size = new System.Drawing.Size(29, 24);
            this.btnMaterialCombination.Text = "toolStripButton1";
            this.btnMaterialCombination.Click += new System.EventHandler(this.btnMaterialCombination_Click);
            // 
            // lblHoverInfo
            // 
            this.lblHoverInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblHoverInfo.Name = "lblHoverInfo";
            this.lblHoverInfo.Size = new System.Drawing.Size(124, 24);
            this.lblHoverInfo.Text = "Mouse hover info";
            // 
            // CanvasEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PixelStacker.Resources.UIResources.bg_imagepanel;
            this.Controls.Add(this.toolStripContainer);
            this.Name = "CanvasEditor";
            this.Size = new System.Drawing.Size(611, 382);
            this.Load += new System.EventHandler(this.CanvasEditor_Load);
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.tsCanvasTools.ResumeLayout(false);
            this.tsCanvasTools.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.ToolStripButton btnPanZoom;
        private System.Windows.Forms.ToolStripButton btnEraser;
        private System.Windows.Forms.ToolStripButton btnPencil;
        private System.Windows.Forms.ToolStripButton btnBrush;
        private System.Windows.Forms.ToolStripButton btnFill;
        private System.Windows.Forms.ToolStripButton btnPicker;
        private System.Windows.Forms.ToolStripLabel lblHoverInfo;
        private System.Windows.Forms.ToolStripButton btnMaterialCombination;
        public System.Windows.Forms.ToolStripContainer toolStripContainer;
    }
}
