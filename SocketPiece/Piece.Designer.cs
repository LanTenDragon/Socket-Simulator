namespace SocketPiece
{
    partial class Piece
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
            this.button1 = new System.Windows.Forms.Button();
            this.PhyBulb = new LedBulb.Bulb();
            this.PhyLabel = new System.Windows.Forms.Label();
            this.LogicLabel = new System.Windows.Forms.Label();
            this.SocketName = new System.Windows.Forms.Label();
            this.PowerLabel = new System.Windows.Forms.Label();
            this.LogicBulb = new LedBulb.Bulb();
            this.PowerDisplay = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(43, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "PHY Switch";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PhyBulb
            // 
            this.PhyBulb.Location = new System.Drawing.Point(223, 12);
            this.PhyBulb.Name = "PhyBulb";
            this.PhyBulb.On = false;
            this.PhyBulb.Size = new System.Drawing.Size(27, 24);
            this.PhyBulb.TabIndex = 1;
            // 
            // PhyLabel
            // 
            this.PhyLabel.AutoSize = true;
            this.PhyLabel.Location = new System.Drawing.Point(166, 23);
            this.PhyLabel.Name = "PhyLabel";
            this.PhyLabel.Size = new System.Drawing.Size(46, 13);
            this.PhyLabel.TabIndex = 2;
            this.PhyLabel.Text = "Physical";
            // 
            // LogicLabel
            // 
            this.LogicLabel.AutoSize = true;
            this.LogicLabel.Location = new System.Drawing.Point(166, 55);
            this.LogicLabel.Name = "LogicLabel";
            this.LogicLabel.Size = new System.Drawing.Size(41, 13);
            this.LogicLabel.TabIndex = 3;
            this.LogicLabel.Text = "Logical";
            // 
            // SocketName
            // 
            this.SocketName.AutoSize = true;
            this.SocketName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SocketName.Location = new System.Drawing.Point(39, 23);
            this.SocketName.MaximumSize = new System.Drawing.Size(100, 0);
            this.SocketName.Name = "SocketName";
            this.SocketName.Size = new System.Drawing.Size(100, 16);
            this.SocketName.TabIndex = 4;
            this.SocketName.Text = "Smart Socket";
            // 
            // PowerLabel
            // 
            this.PowerLabel.AutoSize = true;
            this.PowerLabel.Location = new System.Drawing.Point(166, 86);
            this.PowerLabel.Name = "PowerLabel";
            this.PowerLabel.Size = new System.Drawing.Size(37, 13);
            this.PowerLabel.TabIndex = 5;
            this.PowerLabel.Text = "Power";
            // 
            // LogicBulb
            // 
            this.LogicBulb.Location = new System.Drawing.Point(223, 44);
            this.LogicBulb.Name = "LogicBulb";
            this.LogicBulb.On = false;
            this.LogicBulb.Size = new System.Drawing.Size(27, 24);
            this.LogicBulb.TabIndex = 6;
            // 
            // PowerDisplay
            // 
            this.PowerDisplay.AutoSize = true;
            this.PowerDisplay.Location = new System.Drawing.Point(223, 85);
            this.PowerDisplay.Name = "PowerDisplay";
            this.PowerDisplay.Size = new System.Drawing.Size(22, 13);
            this.PowerDisplay.TabIndex = 7;
            this.PowerDisplay.Text = "0.0";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Piece
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.PowerDisplay);
            this.Controls.Add(this.LogicBulb);
            this.Controls.Add(this.PowerLabel);
            this.Controls.Add(this.SocketName);
            this.Controls.Add(this.LogicLabel);
            this.Controls.Add(this.PhyLabel);
            this.Controls.Add(this.PhyBulb);
            this.Controls.Add(this.button1);
            this.Name = "Piece";
            this.Size = new System.Drawing.Size(272, 120);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private LedBulb.Bulb PhyBulb;
        private System.Windows.Forms.Label PhyLabel;
        private System.Windows.Forms.Label LogicLabel;
        private System.Windows.Forms.Label SocketName;
        private System.Windows.Forms.Label PowerLabel;
        private LedBulb.Bulb LogicBulb;
        private System.Windows.Forms.Label PowerDisplay;
        private System.Windows.Forms.Timer timer1;
    }
}
