namespace olcEngineSpriteEditor
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openSpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsSpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingPanel = new olcEngineSpriteEditor.Cell();
            this.supportedColors = new olcEngineSpriteEditor.Cell();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(841, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSpriteToolStripMenuItem,
            this.toolStripSeparator1,
            this.openSpriteToolStripMenuItem,
            this.saveSpriteToolStripMenuItem,
            this.saveAsSpriteToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSpriteToolStripMenuItem
            // 
            this.newSpriteToolStripMenuItem.Name = "newSpriteToolStripMenuItem";
            this.newSpriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newSpriteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newSpriteToolStripMenuItem.Text = "New Sprite";
            this.newSpriteToolStripMenuItem.Click += new System.EventHandler(this.newSpriteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // openSpriteToolStripMenuItem
            // 
            this.openSpriteToolStripMenuItem.Name = "openSpriteToolStripMenuItem";
            this.openSpriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openSpriteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openSpriteToolStripMenuItem.Text = "Open";
            this.openSpriteToolStripMenuItem.Click += new System.EventHandler(this.openSpriteToolStripMenuItem_Click);
            // 
            // saveSpriteToolStripMenuItem
            // 
            this.saveSpriteToolStripMenuItem.Name = "saveSpriteToolStripMenuItem";
            this.saveSpriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSpriteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveSpriteToolStripMenuItem.Text = "Save";
            this.saveSpriteToolStripMenuItem.Click += new System.EventHandler(this.saveSpriteToolStripMenuItem_Click);
            // 
            // saveAsSpriteToolStripMenuItem
            // 
            this.saveAsSpriteToolStripMenuItem.Name = "saveAsSpriteToolStripMenuItem";
            this.saveAsSpriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsSpriteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsSpriteToolStripMenuItem.Text = "Save As";
            this.saveAsSpriteToolStripMenuItem.Click += new System.EventHandler(this.saveAsSpriteToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleGridToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // toggleGridToolStripMenuItem
            // 
            this.toggleGridToolStripMenuItem.Name = "toggleGridToolStripMenuItem";
            this.toggleGridToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.toggleGridToolStripMenuItem.Text = "Toggle Grid";
            this.toggleGridToolStripMenuItem.Click += new System.EventHandler(this.toggleGridToolStripMenuItem_Click);
            // 
            // drawingPanel
            // 
            this.drawingPanel.BorderColor = System.Drawing.Color.DimGray;
            this.drawingPanel.BorderSize = 2;
            this.drawingPanel.Clicked = false;
            this.drawingPanel.Location = new System.Drawing.Point(1, 34);
            this.drawingPanel.Name = "drawingPanel";
            this.drawingPanel.Size = new System.Drawing.Size(576, 576);
            this.drawingPanel.TabIndex = 3;
            // 
            // supportedColors
            // 
            this.supportedColors.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.supportedColors.BorderColor = System.Drawing.Color.Transparent;
            this.supportedColors.BorderSize = 2;
            this.supportedColors.Clicked = false;
            this.supportedColors.Location = new System.Drawing.Point(581, 34);
            this.supportedColors.Name = "supportedColors";
            this.supportedColors.Size = new System.Drawing.Size(256, 64);
            this.supportedColors.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 613);
            this.Controls.Add(this.supportedColors);
            this.Controls.Add(this.drawingPanel);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "olc Sprite Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSpriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSpriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsSpriteToolStripMenuItem;
        private Cell drawingPanel;
        private Cell supportedColors;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSpriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

