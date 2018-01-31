namespace Forms
{
    partial class DatabaseCreationForm
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
            this.DatabaseNameTextBox = new System.Windows.Forms.TextBox();
            this.dbNameLabel = new System.Windows.Forms.Label();
            this.CreateDatabaseButton2 = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DatabaseNameTextBox
            // 
            this.DatabaseNameTextBox.Location = new System.Drawing.Point(33, 30);
            this.DatabaseNameTextBox.Name = "DatabaseNameTextBox";
            this.DatabaseNameTextBox.Size = new System.Drawing.Size(265, 20);
            this.DatabaseNameTextBox.TabIndex = 0;
            // 
            // dbNameLabel
            // 
            this.dbNameLabel.AutoSize = true;
            this.dbNameLabel.Location = new System.Drawing.Point(30, 14);
            this.dbNameLabel.Name = "dbNameLabel";
            this.dbNameLabel.Size = new System.Drawing.Size(122, 13);
            this.dbNameLabel.TabIndex = 1;
            this.dbNameLabel.Text = "Введите название БД:";
            // 
            // CreateDatabaseButton2
            // 
            this.CreateDatabaseButton2.Location = new System.Drawing.Point(223, 56);
            this.CreateDatabaseButton2.Name = "CreateDatabaseButton2";
            this.CreateDatabaseButton2.Size = new System.Drawing.Size(75, 23);
            this.CreateDatabaseButton2.TabIndex = 2;
            this.CreateDatabaseButton2.Text = "OK";
            this.CreateDatabaseButton2.UseVisualStyleBackColor = true;
            this.CreateDatabaseButton2.Click += new System.EventHandler(this.CreateDatabase2_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(33, 56);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(223, 56);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 4;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Visible = false;
            this.OkButton.Click += new System.EventHandler(this.CreateDatabase1_Click);
            // 
            // DatabaseCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 91);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.CreateDatabaseButton2);
            this.Controls.Add(this.dbNameLabel);
            this.Controls.Add(this.DatabaseNameTextBox);
            this.Name = "DatabaseCreationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DatabaseCreationForm";
            this.Load += new System.EventHandler(this.DatabaseCreationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DatabaseNameTextBox;
        private System.Windows.Forms.Button CancelBtn;
        public System.Windows.Forms.Label dbNameLabel;
        private System.Windows.Forms.Button CreateDatabaseButton2;
        public System.Windows.Forms.Button OkButton;
    }
}