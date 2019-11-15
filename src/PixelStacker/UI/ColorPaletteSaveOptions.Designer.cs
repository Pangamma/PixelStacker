namespace PixelStacker.UI
{
    partial class ColorPaletteSaveOptions
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPaletteSaveOptions));
            this.btnCompactGraph = new System.Windows.Forms.Button();
            this.btnCompactBrick = new System.Windows.Forms.Button();
            this.btnGrid = new System.Windows.Forms.Button();
            this.btnFullGrid = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCompactGraph
            // 
            this.btnCompactGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCompactGraph.Image = global::PixelStacker.Properties.Resources.feature_palette_compact_graph;
            this.btnCompactGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCompactGraph.Location = new System.Drawing.Point(12, 24);
            this.btnCompactGraph.Name = "btnCompactGraph";
            this.btnCompactGraph.Size = new System.Drawing.Size(230, 31);
            this.btnCompactGraph.TabIndex = 0;
            this.btnCompactGraph.Text = "Graph";
            this.btnCompactGraph.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCompactGraph.UseVisualStyleBackColor = true;
            this.btnCompactGraph.Click += new System.EventHandler(this.btnCompactGraph_Click);
            // 
            // btnCompactBrick
            // 
            this.btnCompactBrick.Image = global::PixelStacker.Properties.Resources.feature_palette_compact_brick;
            this.btnCompactBrick.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCompactBrick.Location = new System.Drawing.Point(12, 61);
            this.btnCompactBrick.Name = "btnCompactBrick";
            this.btnCompactBrick.Size = new System.Drawing.Size(230, 35);
            this.btnCompactBrick.TabIndex = 1;
            this.btnCompactBrick.Text = "Brick";
            this.btnCompactBrick.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCompactBrick.UseVisualStyleBackColor = true;
            this.btnCompactBrick.Click += new System.EventHandler(this.btnCompactBrick_Click);
            // 
            // btnGrid
            // 
            this.btnGrid.Image = global::PixelStacker.Properties.Resources.feature_palette_compact_grid;
            this.btnGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGrid.Location = new System.Drawing.Point(12, 102);
            this.btnGrid.Name = "btnGrid";
            this.btnGrid.Size = new System.Drawing.Size(230, 31);
            this.btnGrid.TabIndex = 2;
            this.btnGrid.Text = "Compact Grid";
            this.btnGrid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGrid.UseVisualStyleBackColor = true;
            this.btnGrid.Click += new System.EventHandler(this.btnGrid_Click);
            // 
            // btnFullGrid
            // 
            this.btnFullGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFullGrid.Image = global::PixelStacker.Properties.Resources.feature_palette_detailed_grid;
            this.btnFullGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFullGrid.Location = new System.Drawing.Point(12, 139);
            this.btnFullGrid.Name = "btnFullGrid";
            this.btnFullGrid.Size = new System.Drawing.Size(230, 42);
            this.btnFullGrid.TabIndex = 3;
            this.btnFullGrid.Text = "Full grid";
            this.btnFullGrid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFullGrid.UseVisualStyleBackColor = true;
            this.btnFullGrid.Click += new System.EventHandler(this.btnFullGrid_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Which style do you want?";
            // 
            // ColorPaletteSaveOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 205);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFullGrid);
            this.Controls.Add(this.btnGrid);
            this.Controls.Add(this.btnCompactBrick);
            this.Controls.Add(this.btnCompactGraph);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ColorPaletteSaveOptions";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCompactGraph;
        private System.Windows.Forms.Button btnCompactBrick;
        private System.Windows.Forms.Button btnGrid;
        private System.Windows.Forms.Button btnFullGrid;
        private System.Windows.Forms.Label label1;
    }
}