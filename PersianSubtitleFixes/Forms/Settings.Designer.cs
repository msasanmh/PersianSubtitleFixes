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
            this.LabelEncoding = new System.Windows.Forms.Label();
            this.LabelTheme = new System.Windows.Forms.Label();
            this.CustomComboBoxEncoding = new CustomControls.CustomComboBox();
            this.CustomComboBoxTheme = new CustomControls.CustomComboBox();
            this.CustomButtonSave = new CustomControls.CustomButton();
            this.SuspendLayout();
            // 
            // LabelEncoding
            // 
            this.LabelEncoding.AutoSize = true;
            this.LabelEncoding.Location = new System.Drawing.Point(12, 23);
            this.LabelEncoding.Name = "LabelEncoding";
            this.LabelEncoding.Size = new System.Drawing.Size(117, 15);
            this.LabelEncoding.TabIndex = 0;
            this.LabelEncoding.Text = "Default file encoding";
            // 
            // LabelTheme
            // 
            this.LabelTheme.AutoSize = true;
            this.LabelTheme.Location = new System.Drawing.Point(12, 53);
            this.LabelTheme.Name = "LabelTheme";
            this.LabelTheme.Size = new System.Drawing.Size(75, 15);
            this.LabelTheme.TabIndex = 1;
            this.LabelTheme.Text = "Select theme";
            // 
            // CustomComboBoxEncoding
            // 
            this.CustomComboBoxEncoding.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.CustomComboBoxEncoding.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CustomComboBoxEncoding.FormattingEnabled = true;
            this.CustomComboBoxEncoding.Location = new System.Drawing.Point(143, 20);
            this.CustomComboBoxEncoding.Name = "CustomComboBoxEncoding";
            this.CustomComboBoxEncoding.SelectionColor = System.Drawing.SystemColors.ActiveBorder;
            this.CustomComboBoxEncoding.Size = new System.Drawing.Size(121, 24);
            this.CustomComboBoxEncoding.TabIndex = 2;
            this.CustomComboBoxEncoding.SelectedIndexChanged += new System.EventHandler(this.CustomComboBoxEncoding_SelectedIndexChanged);
            // 
            // CustomComboBoxTheme
            // 
            this.CustomComboBoxTheme.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.CustomComboBoxTheme.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CustomComboBoxTheme.FormattingEnabled = true;
            this.CustomComboBoxTheme.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.CustomComboBoxTheme.Location = new System.Drawing.Point(143, 50);
            this.CustomComboBoxTheme.Name = "CustomComboBoxTheme";
            this.CustomComboBoxTheme.SelectionColor = System.Drawing.SystemColors.ActiveBorder;
            this.CustomComboBoxTheme.Size = new System.Drawing.Size(121, 24);
            this.CustomComboBoxTheme.TabIndex = 3;
            this.CustomComboBoxTheme.SelectedIndexChanged += new System.EventHandler(this.CustomComboBoxTheme_SelectedIndexChanged);
            // 
            // CustomButtonSave
            // 
            this.CustomButtonSave.BorderColor = System.Drawing.Color.Red;
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
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 117);
            this.Controls.Add(this.CustomButtonSave);
            this.Controls.Add(this.LabelEncoding);
            this.Controls.Add(this.LabelTheme);
            this.Controls.Add(this.CustomComboBoxEncoding);
            this.Controls.Add(this.CustomComboBoxTheme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label LabelEncoding;
        private Label LabelTheme;
        private CustomControls.CustomComboBox CustomComboBoxEncoding;
        private CustomControls.CustomComboBox CustomComboBoxTheme;
        private CustomControls.CustomButton CustomButtonSave;
    }
}