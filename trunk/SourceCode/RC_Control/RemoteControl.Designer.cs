namespace RC_Control
{
    partial class RemoteControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteControl));
            this.btnEnableP = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbChannels = new System.Windows.Forms.ComboBox();
            this.linerChannel1 = new RC_Control.LinerChannel();
            this.linerChannel2 = new RC_Control.LinerChannel();
            this.linerChannel4 = new RC_Control.LinerChannel();
            this.linerChannel3 = new RC_Control.LinerChannel();
            this.linerChannel5 = new RC_Control.LinerChannel();
            this.linerChannel6 = new RC_Control.LinerChannel();
            this.linerChannel7 = new RC_Control.LinerChannel();
            this.linerChannel8 = new RC_Control.LinerChannel();
            this.SuspendLayout();
            // 
            // btnEnableP
            // 
            this.btnEnableP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableP.Image = global::RC_Control.Properties.Resources.keyboard2;
            this.btnEnableP.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEnableP.Location = new System.Drawing.Point(542, 23);
            this.btnEnableP.Name = "btnEnableP";
            this.btnEnableP.Size = new System.Drawing.Size(79, 72);
            this.btnEnableP.TabIndex = 2;
            this.btnEnableP.Text = "Transmit";
            this.btnEnableP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEnableP.UseVisualStyleBackColor = true;
            this.btnEnableP.Click += new System.EventHandler(this.btnEnableP_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(528, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rate";
            // 
            // cmbRate
            // 
            this.cmbRate.FormattingEnabled = true;
            this.cmbRate.Items.AddRange(new object[] {
            "44100",
            "96000",
            "192000"});
            this.cmbRate.Location = new System.Drawing.Point(564, 124);
            this.cmbRate.Name = "cmbRate";
            this.cmbRate.Size = new System.Drawing.Size(65, 21);
            this.cmbRate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(528, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Channels";
            // 
            // cmbChannels
            // 
            this.cmbChannels.FormattingEnabled = true;
            this.cmbChannels.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cmbChannels.Location = new System.Drawing.Point(585, 158);
            this.cmbChannels.Name = "cmbChannels";
            this.cmbChannels.Size = new System.Drawing.Size(44, 21);
            this.cmbChannels.TabIndex = 3;
            // 
            // linerChannel1
            // 
            this.linerChannel1.ChannelName = "Throtel";
            this.linerChannel1.ChannelNumber = 1;
            this.linerChannel1.Location = new System.Drawing.Point(12, 12);
            this.linerChannel1.Name = "linerChannel1";
            this.linerChannel1.Size = new System.Drawing.Size(124, 289);
            this.linerChannel1.TabIndex = 0;
            this.linerChannel1.Value = 0;
            this.linerChannel1.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // linerChannel2
            // 
            this.linerChannel2.ChannelName = "Aileron";
            this.linerChannel2.ChannelNumber = 2;
            this.linerChannel2.Location = new System.Drawing.Point(399, 12);
            this.linerChannel2.Name = "linerChannel2";
            this.linerChannel2.Size = new System.Drawing.Size(124, 289);
            this.linerChannel2.TabIndex = 1;
            this.linerChannel2.Value = 0;
            this.linerChannel2.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // linerChannel4
            // 
            this.linerChannel4.ChannelName = "Rudder";
            this.linerChannel4.ChannelNumber = 4;
            this.linerChannel4.Location = new System.Drawing.Point(142, 12);
            this.linerChannel4.Name = "linerChannel4";
            this.linerChannel4.Size = new System.Drawing.Size(124, 289);
            this.linerChannel4.TabIndex = 1;
            this.linerChannel4.Value = 0;
            this.linerChannel4.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // linerChannel3
            // 
            this.linerChannel3.ChannelName = "Elevator";
            this.linerChannel3.ChannelNumber = 3;
            this.linerChannel3.Location = new System.Drawing.Point(272, 12);
            this.linerChannel3.Name = "linerChannel3";
            this.linerChannel3.Size = new System.Drawing.Size(124, 289);
            this.linerChannel3.TabIndex = 1;
            this.linerChannel3.Value = 0;
            this.linerChannel3.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // linerChannel5
            // 
            this.linerChannel5.ChannelName = "Gear";
            this.linerChannel5.ChannelNumber = 5;
            this.linerChannel5.Location = new System.Drawing.Point(12, 307);
            this.linerChannel5.Name = "linerChannel5";
            this.linerChannel5.Size = new System.Drawing.Size(124, 289);
            this.linerChannel5.TabIndex = 4;
            this.linerChannel5.Value = 0;
            this.linerChannel5.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // linerChannel6
            // 
            this.linerChannel6.ChannelName = "Aux1";
            this.linerChannel6.ChannelNumber = 6;
            this.linerChannel6.Location = new System.Drawing.Point(142, 307);
            this.linerChannel6.Name = "linerChannel6";
            this.linerChannel6.Size = new System.Drawing.Size(124, 289);
            this.linerChannel6.TabIndex = 5;
            this.linerChannel6.Value = 0;
            this.linerChannel6.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // linerChannel7
            // 
            this.linerChannel7.ChannelName = "Aux2";
            this.linerChannel7.ChannelNumber = 7;
            this.linerChannel7.Location = new System.Drawing.Point(272, 307);
            this.linerChannel7.Name = "linerChannel7";
            this.linerChannel7.Size = new System.Drawing.Size(124, 289);
            this.linerChannel7.TabIndex = 6;
            this.linerChannel7.Value = 0;
            this.linerChannel7.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // linerChannel8
            // 
            this.linerChannel8.ChannelName = "Aux3";
            this.linerChannel8.ChannelNumber = 8;
            this.linerChannel8.Location = new System.Drawing.Point(399, 307);
            this.linerChannel8.Name = "linerChannel8";
            this.linerChannel8.Size = new System.Drawing.Size(124, 289);
            this.linerChannel8.TabIndex = 7;
            this.linerChannel8.Value = 0;
            this.linerChannel8.OnValueChanged += new RC_Control.LinerChannel.ValueChanged(this.linerChannel1_OnValueChanged);
            // 
            // RemoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 593);
            this.Controls.Add(this.linerChannel8);
            this.Controls.Add(this.linerChannel7);
            this.Controls.Add(this.linerChannel6);
            this.Controls.Add(this.linerChannel5);
            this.Controls.Add(this.linerChannel1);
            this.Controls.Add(this.cmbChannels);
            this.Controls.Add(this.btnEnableP);
            this.Controls.Add(this.linerChannel2);
            this.Controls.Add(this.linerChannel4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linerChannel3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbRate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RemoteControl";
            this.Text = "Remote Control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LinerChannel linerChannel1;
        private LinerChannel linerChannel2;
        private System.Windows.Forms.Button btnEnableP;
        private LinerChannel linerChannel3;
        private LinerChannel linerChannel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRate;
        private System.Windows.Forms.ComboBox cmbChannels;
        private System.Windows.Forms.Label label2;
        private LinerChannel linerChannel5;
        private LinerChannel linerChannel6;
        private LinerChannel linerChannel7;
        private LinerChannel linerChannel8;

    }
}