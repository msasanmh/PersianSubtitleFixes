namespace PersianSubtitleFixes
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.CustomComboBoxEncoding = new CustomControls.CustomComboBox();
            this.CustomComboBoxTheme = new CustomControls.CustomComboBox();
            this.CustomButtonSave = new CustomControls.CustomButton();
            this.CustomLabelEncoding = new CustomControls.CustomLabel();
            this.CustomLabelTheme = new CustomControls.CustomLabel();
            this.SuspendLayout();
            // 
            // CustomComboBoxEncoding
            // 
            this.CustomComboBoxEncoding.BackColor = System.Drawing.Color.DimGray;
            this.CustomComboBoxEncoding.BorderColor = System.Drawing.Color.Blue;
            this.CustomComboBoxEncoding.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CustomComboBoxEncoding.ForeColor = System.Drawing.Color.White;
            this.CustomComboBoxEncoding.FormattingEnabled = true;
            this.CustomComboBoxEncoding.ItemHeight = 17;
            this.CustomComboBoxEncoding.Location = new System.Drawing.Point(143, 20);
            this.CustomComboBoxEncoding.Name = "CustomComboBoxEncoding";
            this.CustomComboBoxEncoding.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomComboBoxEncoding.SelectionColor = System.Drawing.SystemColors.ActiveBorder;
            this.CustomComboBoxEncoding.Size = new System.Drawing.Size(121, 23);
            this.CustomComboBoxEncoding.TabIndex = 2;
            this.CustomComboBoxEncoding.SelectedIndexChanged += new System.EventHandler(this.CustomComboBoxEncoding_SelectedIndexChanged);
            // 
            // CustomComboBoxTheme
            // 
            this.CustomComboBoxTheme.BackColor = System.Drawing.Color.DimGray;
            this.CustomComboBoxTheme.BorderColor = System.Drawing.Color.Blue;
            this.CustomComboBoxTheme.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CustomComboBoxTheme.ForeColor = System.Drawing.Color.White;
            this.CustomComboBoxTheme.FormattingEnabled = true;
            this.CustomComboBoxTheme.ItemHeight = 17;
            this.CustomComboBoxTheme.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.CustomComboBoxTheme.Location = new System.Drawing.Point(143, 50);
            this.CustomComboBoxTheme.Name = "CustomComboBoxTheme";
            this.CustomComboBoxTheme.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomComboBoxTheme.SelectionColor = System.Drawing.SystemColors.ActiveBorder;
            this.CustomComboBoxTheme.Size = new System.Drawing.Size(121, 23);
            this.CustomComboBoxTheme.TabIndex = 3;
            this.CustomComboBoxTheme.SelectedIndexChanged += new System.EventHandler(this.CustomComboBoxTheme_SelectedIndexChanged);
            // 
            // CustomButtonSave
            // 
            this.CustomButtonSave.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CustomButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonSave.Location = new System.Drawing.Point(100, 84);
            this.CustomButtonSave.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonSave.Name = "CustomButtonSave";
            this.CustomButtonSave.RoundedCorners = 0;
            this.CustomButtonSave.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.CustomButtonSave.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonSave.TabIndex = 4;
            this.CustomButtonSave.Text = "Save";
            this.CustomButtonSave.UseVisualStyleBackColor = false;
            this.CustomButtonSave.Click += new System.EventHandler(this.CustomButtonSave_Click);
            // 
            // CustomLabelEncoding
            // 
            this.CustomLabelEncoding.AutoSize = true;
            this.CustomLabelEncoding.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelEncoding.Border = false;
            this.CustomLabelEncoding.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelEncoding.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelEncoding.ForeColor = System.Drawing.Color.White;
            this.CustomLabelEncoding.Location = new System.Drawing.Point(12, 23);
            this.CustomLabelEncoding.Name = "CustomLabelEncoding";
            this.CustomLabelEncoding.RoundedCorners = 0;
            this.CustomLabelEncoding.Size = new System.Drawing.Size(119, 17);
            this.CustomLabelEncoding.TabIndex = 5;
            this.CustomLabelEncoding.Text = "Default file encoding";
            // 
            // CustomLabelTheme
            // 
            this.CustomLabelTheme.AutoSize = true;
            this.CustomLabelTheme.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelTheme.Border = false;
            this.CustomLabelTheme.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelTheme.ForeColor = System.Drawing.Color.White;
            this.CustomLabelTheme.Location = new System.Drawing.Point(12, 53);
            this.CustomLabelTheme.Name = "CustomLabelTheme";
            this.CustomLabelTheme.RoundedCorners = 0;
            this.CustomLabelTheme.Size = new System.Drawing.Size(77, 17);
            this.CustomLabelTheme.TabIndex = 6;
            this.CustomLabelTheme.Text = "Select theme";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(276, 117);
            this.Controls.Add(this.CustomLabelTheme);
            this.Controls.Add(this.CustomLabelEncoding);
            this.Controls.Add(this.CustomButtonSave);
            this.Controls.Add(this.CustomComboBoxEncoding);
            this.Controls.Add(this.CustomComboBoxTheme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CustomControls.CustomButton CustomButtonSave;
        protected internal CustomControls.CustomComboBox CustomComboBoxEncoding;
        protected internal CustomControls.CustomComboBox CustomComboBoxTheme;
        private CustomControls.CustomLabel CustomLabelEncoding;
        private CustomControls.CustomLabel CustomLabelTheme;
    }
}