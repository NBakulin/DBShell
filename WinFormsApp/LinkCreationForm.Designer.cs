namespace Forms
{
    partial class LinkCreationForm
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
            this.CancelBtn = new System.Windows.Forms.Button();
            this.MTable = new System.Windows.Forms.Label();
            this.LinksBox = new System.Windows.Forms.ComboBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CascadeDelete = new System.Windows.Forms.CheckBox();
            this.CascadeUpdate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(15, 85);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MTable
            // 
            this.MTable.AutoSize = true;
            this.MTable.Location = new System.Drawing.Point(52, 15);
            this.MTable.Name = "MTable";
            this.MTable.Size = new System.Drawing.Size(35, 13);
            this.MTable.TabIndex = 8;
            this.MTable.Text = "label1";
            // 
            // LinksBox
            // 
            this.LinksBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LinksBox.FormattingEnabled = true;
            this.LinksBox.Location = new System.Drawing.Point(173, 12);
            this.LinksBox.Name = "LinksBox";
            this.LinksBox.Size = new System.Drawing.Size(121, 21);
            this.LinksBox.TabIndex = 9;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(222, 85);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 7;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CascadeDelete
            // 
            this.CascadeDelete.AutoSize = true;
            this.CascadeDelete.Location = new System.Drawing.Point(173, 39);
            this.CascadeDelete.Name = "CascadeDelete";
            this.CascadeDelete.Size = new System.Drawing.Size(127, 17);
            this.CascadeDelete.TabIndex = 10;
            this.CascadeDelete.Text = "Is Cascade Deletable";
            this.CascadeDelete.UseVisualStyleBackColor = true;
            // 
            // CascadeUpdate
            // 
            this.CascadeUpdate.AutoSize = true;
            this.CascadeUpdate.Location = new System.Drawing.Point(173, 62);
            this.CascadeUpdate.Name = "CascadeUpdate";
            this.CascadeUpdate.Size = new System.Drawing.Size(131, 17);
            this.CascadeUpdate.TabIndex = 11;
            this.CascadeUpdate.Text = "Is Cascade Updatable";
            this.CascadeUpdate.UseVisualStyleBackColor = true;
            // 
            // LinkCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 124);
            this.Controls.Add(this.CascadeUpdate);
            this.Controls.Add(this.CascadeDelete);
            this.Controls.Add(this.LinksBox);
            this.Controls.Add(this.MTable);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancelBtn);
            this.Name = "LinkCreationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LinkCreationForm";
            this.Load += new System.EventHandler(this.LinkCreationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label MTable;
        private System.Windows.Forms.ComboBox LinksBox;
        public System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.CheckBox CascadeDelete;
        private System.Windows.Forms.CheckBox CascadeUpdate;
    }
}