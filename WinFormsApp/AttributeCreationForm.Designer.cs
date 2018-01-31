namespace Forms
{
    partial class AttributeCreationForm
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
            this.IsNullableFlag = new System.Windows.Forms.CheckBox();
            this.AttributeNameTextbox = new System.Windows.Forms.TextBox();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.IsIndexedFlag = new System.Windows.Forms.CheckBox();
            this.AttributeDescriptionTExtbox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.SqlTypeLabel = new System.Windows.Forms.Label();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.FKlabel = new System.Windows.Forms.Label();
            this.FKcombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // IsNullableFlag
            // 
            this.IsNullableFlag.AutoSize = true;
            this.IsNullableFlag.Location = new System.Drawing.Point(315, 67);
            this.IsNullableFlag.Name = "IsNullableFlag";
            this.IsNullableFlag.Size = new System.Drawing.Size(72, 17);
            this.IsNullableFlag.TabIndex = 0;
            this.IsNullableFlag.Text = "IsNullable";
            this.IsNullableFlag.UseVisualStyleBackColor = true;
            // 
            // AttributeNameTextbox
            // 
            this.AttributeNameTextbox.Location = new System.Drawing.Point(12, 28);
            this.AttributeNameTextbox.Name = "AttributeNameTextbox";
            this.AttributeNameTextbox.Size = new System.Drawing.Size(269, 20);
            this.AttributeNameTextbox.TabIndex = 1;
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Items.AddRange(new object[] {
            "DECIMAL",
            "INT",
            "NVARCHAR",
            "FLOAT"});
            this.TypeComboBox.Location = new System.Drawing.Point(12, 67);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.TypeComboBox.TabIndex = 2;
            // 
            // IsIndexedFlag
            // 
            this.IsIndexedFlag.AutoSize = true;
            this.IsIndexedFlag.Location = new System.Drawing.Point(396, 67);
            this.IsIndexedFlag.Name = "IsIndexedFlag";
            this.IsIndexedFlag.Size = new System.Drawing.Size(72, 17);
            this.IsIndexedFlag.TabIndex = 4;
            this.IsIndexedFlag.Text = "IsIndexed";
            this.IsIndexedFlag.UseVisualStyleBackColor = true;
            // 
            // AttributeDescriptionTExtbox
            // 
            this.AttributeDescriptionTExtbox.Location = new System.Drawing.Point(315, 27);
            this.AttributeDescriptionTExtbox.Name = "AttributeDescriptionTExtbox";
            this.AttributeDescriptionTExtbox.Size = new System.Drawing.Size(462, 20);
            this.AttributeDescriptionTExtbox.TabIndex = 5;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(12, 12);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 13);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Name";
            // 
            // SqlTypeLabel
            // 
            this.SqlTypeLabel.AutoSize = true;
            this.SqlTypeLabel.Location = new System.Drawing.Point(9, 51);
            this.SqlTypeLabel.Name = "SqlTypeLabel";
            this.SqlTypeLabel.Size = new System.Drawing.Size(51, 13);
            this.SqlTypeLabel.TabIndex = 7;
            this.SqlTypeLabel.Text = "SQL type";
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(312, 11);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.DescriptionLabel.TabIndex = 8;
            this.DescriptionLabel.Text = "Description";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(90, 110);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 10;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(565, 110);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 9;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.CreateAttribute_Click);
            // 
            // FKlabel
            // 
            this.FKlabel.AutoSize = true;
            this.FKlabel.Location = new System.Drawing.Point(159, 51);
            this.FKlabel.Name = "FKlabel";
            this.FKlabel.Size = new System.Drawing.Size(48, 13);
            this.FKlabel.TabIndex = 12;
            this.FKlabel.Text = "Key type";
            // 
            // FKcombo
            // 
            this.FKcombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FKcombo.FormattingEnabled = true;
            this.FKcombo.Items.AddRange(new object[] {
            "PrimaryKey",
            "ForeignKey",
            "None"});
            this.FKcombo.Location = new System.Drawing.Point(160, 67);
            this.FKcombo.Name = "FKcombo";
            this.FKcombo.Size = new System.Drawing.Size(121, 21);
            this.FKcombo.TabIndex = 11;
            // 
            // AttributeCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 145);
            this.Controls.Add(this.FKlabel);
            this.Controls.Add(this.FKcombo);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.SqlTypeLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.AttributeDescriptionTExtbox);
            this.Controls.Add(this.IsIndexedFlag);
            this.Controls.Add(this.TypeComboBox);
            this.Controls.Add(this.AttributeNameTextbox);
            this.Controls.Add(this.IsNullableFlag);
            this.Name = "AttributeCreationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New attribute";
            this.Load += new System.EventHandler(this.AttributeCreationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox IsNullableFlag;
        private System.Windows.Forms.TextBox AttributeNameTextbox;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.CheckBox IsIndexedFlag;
        private System.Windows.Forms.TextBox AttributeDescriptionTExtbox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label SqlTypeLabel;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.Button CancelBtn;
        public System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label FKlabel;
        private System.Windows.Forms.ComboBox FKcombo;
    }
}