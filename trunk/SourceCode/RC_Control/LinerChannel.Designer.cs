namespace RC_Control
{
    partial class LinerChannel
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
            this.trkChannel = new System.Windows.Forms.TrackBar();
            this.lblValue = new System.Windows.Forms.Label();
            this.numChannel = new System.Windows.Forms.NumericUpDown();
            this.grBox = new System.Windows.Forms.GroupBox();
            this.btnOnOff = new System.Windows.Forms.Button();
            this.chkInv = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChannel)).BeginInit();
            this.grBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // trkChannel
            // 
            this.trkChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.trkChannel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkChannel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trkChannel.Location = new System.Drawing.Point(10, 19);
            this.trkChannel.Maximum = 100;
            this.trkChannel.Name = "trkChannel";
            this.trkChannel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trkChannel.Size = new System.Drawing.Size(45, 253);
            this.trkChannel.TabIndex = 0;
            this.trkChannel.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trkChannel.Scroll += new System.EventHandler(this.trkChannel_Scroll);
            // 
            // lblValue
            // 
            this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblValue.AutoSize = true;
            this.lblValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblValue.Location = new System.Drawing.Point(61, 87);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(33, 15);
            this.lblValue.TabIndex = 2;
            this.lblValue.Text = "0000";
            // 
            // numChannel
            // 
            this.numChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numChannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numChannel.Location = new System.Drawing.Point(61, 193);
            this.numChannel.Name = "numChannel";
            this.numChannel.Size = new System.Drawing.Size(45, 20);
            this.numChannel.TabIndex = 3;
            this.numChannel.ValueChanged += new System.EventHandler(this.numChannel_ValueChanged);
            // 
            // grBox
            // 
            this.grBox.Controls.Add(this.label1);
            this.grBox.Controls.Add(this.chkInv);
            this.grBox.Controls.Add(this.numChannel);
            this.grBox.Controls.Add(this.trkChannel);
            this.grBox.Controls.Add(this.btnOnOff);
            this.grBox.Controls.Add(this.lblValue);
            this.grBox.Location = new System.Drawing.Point(7, 3);
            this.grBox.Name = "grBox";
            this.grBox.Size = new System.Drawing.Size(112, 278);
            this.grBox.TabIndex = 4;
            this.grBox.TabStop = false;
            this.grBox.Text = "Elevator";
            // 
            // btnOnOff
            // 
            this.btnOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOnOff.Image = global::RC_Control.Properties.Resources.sort_descending;
            this.btnOnOff.Location = new System.Drawing.Point(61, 31);
            this.btnOnOff.Name = "btnOnOff";
            this.btnOnOff.Size = new System.Drawing.Size(45, 36);
            this.btnOnOff.TabIndex = 1;
            this.btnOnOff.UseVisualStyleBackColor = true;
            this.btnOnOff.Click += new System.EventHandler(this.btnOnOff_Click);
            // 
            // chkInv
            // 
            this.chkInv.AutoSize = true;
            this.chkInv.Location = new System.Drawing.Point(61, 247);
            this.chkInv.Name = "chkInv";
            this.chkInv.Size = new System.Drawing.Size(41, 17);
            this.chkInv.TabIndex = 4;
            this.chkInv.Text = "Inv";
            this.chkInv.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Channel";
            // 
            // LinerChannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grBox);
            this.Name = "LinerChannel";
            this.Size = new System.Drawing.Size(125, 287);
            ((System.ComponentModel.ISupportInitialize)(this.trkChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChannel)).EndInit();
            this.grBox.ResumeLayout(false);
            this.grBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trkChannel;
        private System.Windows.Forms.Button btnOnOff;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.NumericUpDown numChannel;
        private System.Windows.Forms.GroupBox grBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkInv;
    }
}
