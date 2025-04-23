namespace PixelStacker.UI.Forms
{
    partial class TestForm
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
            imageButtonContainer1 = new PixelStacker.UI.Controls.ImageButtonContainer();
            SuspendLayout();
            // 
            // imageButtonContainer1
            // 
            imageButtonContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            imageButtonContainer1.Location = new System.Drawing.Point(0, 0);
            imageButtonContainer1.Name = "imageButtonContainer1";
            imageButtonContainer1.Size = new System.Drawing.Size(651, 388);
            imageButtonContainer1.TabIndex = 0;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(651, 388);
            Controls.Add(imageButtonContainer1);
            Name = "TestForm";
            Text = "TestForm";
            ResumeLayout(false);

        }

        #endregion

        private Controls.ImageButtonContainer imageButtonContainer1;
    }
}