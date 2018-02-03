namespace Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DatabasesTree = new System.Windows.Forms.TreeView();
            this.CreateDatabaseDatabase = new System.Windows.Forms.Button();
            this.CreateTableButton = new System.Windows.Forms.Button();
            this.CreateAttributeButton = new System.Windows.Forms.Button();
            this.showDatabaseButton = new System.Windows.Forms.Button();
            this.CreateLinkButton = new System.Windows.Forms.Button();
            this.ShowLinks = new System.Windows.Forms.Button();
            this.LinksView = new System.Windows.Forms.DataGridView();
            this.DeleteLinkButton = new System.Windows.Forms.Button();
            this.DeleteAttributeButton = new System.Windows.Forms.Button();
            this.DeleteTableButton = new System.Windows.Forms.Button();
            this.DeleteDatabaseButton = new System.Windows.Forms.Button();
            this.masterAttribute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlaveAttribute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MasterAttributeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlaveAttributeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.LinksView)).BeginInit();
            this.SuspendLayout();
            // 
            // DatabasesTree
            // 
            this.DatabasesTree.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DatabasesTree.Indent = 25;
            this.DatabasesTree.Location = new System.Drawing.Point(42, 36);
            this.DatabasesTree.Name = "DatabasesTree";
            this.DatabasesTree.Size = new System.Drawing.Size(413, 441);
            this.DatabasesTree.TabIndex = 0;
            // 
            // CreateDatabaseDatabase
            // 
            this.CreateDatabaseDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateDatabaseDatabase.Location = new System.Drawing.Point(769, 132);
            this.CreateDatabaseDatabase.Name = "CreateDatabaseDatabase";
            this.CreateDatabaseDatabase.Size = new System.Drawing.Size(151, 30);
            this.CreateDatabaseDatabase.TabIndex = 1;
            this.CreateDatabaseDatabase.Text = "Create database";
            this.CreateDatabaseDatabase.UseVisualStyleBackColor = true;
            this.CreateDatabaseDatabase.Click += new System.EventHandler(this.CreateDatabase_Click);
            // 
            // CreateTableButton
            // 
            this.CreateTableButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateTableButton.Location = new System.Drawing.Point(769, 168);
            this.CreateTableButton.Name = "CreateTableButton";
            this.CreateTableButton.Size = new System.Drawing.Size(151, 30);
            this.CreateTableButton.TabIndex = 2;
            this.CreateTableButton.Text = "Create table";
            this.CreateTableButton.UseVisualStyleBackColor = true;
            this.CreateTableButton.Click += new System.EventHandler(this.CreateTable_Click);
            // 
            // CreateAttributeButton
            // 
            this.CreateAttributeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateAttributeButton.Location = new System.Drawing.Point(769, 204);
            this.CreateAttributeButton.Name = "CreateAttributeButton";
            this.CreateAttributeButton.Size = new System.Drawing.Size(151, 30);
            this.CreateAttributeButton.TabIndex = 3;
            this.CreateAttributeButton.Text = "Add attribute";
            this.CreateAttributeButton.UseVisualStyleBackColor = true;
            this.CreateAttributeButton.Click += new System.EventHandler(this.CreateAttribute_Click);
            // 
            // showDatabaseButton
            // 
            this.showDatabaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.showDatabaseButton.Location = new System.Drawing.Point(769, 36);
            this.showDatabaseButton.Name = "showDatabaseButton";
            this.showDatabaseButton.Size = new System.Drawing.Size(151, 30);
            this.showDatabaseButton.TabIndex = 4;
            this.showDatabaseButton.Text = "Show databases";
            this.showDatabaseButton.UseMnemonic = false;
            this.showDatabaseButton.UseVisualStyleBackColor = true;
            this.showDatabaseButton.Click += new System.EventHandler(this.showDatabaseButton_Click);
            // 
            // CreateLinkButton
            // 
            this.CreateLinkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateLinkButton.Location = new System.Drawing.Point(769, 240);
            this.CreateLinkButton.Name = "CreateLinkButton";
            this.CreateLinkButton.Size = new System.Drawing.Size(151, 30);
            this.CreateLinkButton.TabIndex = 5;
            this.CreateLinkButton.Text = "Create link";
            this.CreateLinkButton.UseVisualStyleBackColor = true;
            this.CreateLinkButton.Click += new System.EventHandler(this.CreateLinkButton_Click);
            // 
            // ShowLinks
            // 
            this.ShowLinks.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ShowLinks.Location = new System.Drawing.Point(769, 72);
            this.ShowLinks.Name = "ShowLinks";
            this.ShowLinks.Size = new System.Drawing.Size(151, 30);
            this.ShowLinks.TabIndex = 6;
            this.ShowLinks.Text = "Show links";
            this.ShowLinks.UseVisualStyleBackColor = true;
            this.ShowLinks.Click += new System.EventHandler(this.ShowLinks_Click);
            // 
            // LinksView
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.LinksView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.LinksView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.LinksView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LinksView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.masterAttribute,
            this.SlaveAttribute,
            this.MasterAttributeID,
            this.SlaveAttributeID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.LinksView.DefaultCellStyle = dataGridViewCellStyle2;
            this.LinksView.Location = new System.Drawing.Point(471, 36);
            this.LinksView.Name = "LinksView";
            this.LinksView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LinksView.Size = new System.Drawing.Size(273, 441);
            this.LinksView.TabIndex = 7;
            // 
            // DeleteLinkButton
            // 
            this.DeleteLinkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeleteLinkButton.Location = new System.Drawing.Point(769, 412);
            this.DeleteLinkButton.Name = "DeleteLinkButton";
            this.DeleteLinkButton.Size = new System.Drawing.Size(151, 30);
            this.DeleteLinkButton.TabIndex = 11;
            this.DeleteLinkButton.Text = "Delete link";
            this.DeleteLinkButton.UseVisualStyleBackColor = true;
            this.DeleteLinkButton.Click += new System.EventHandler(this.DeleteLinkButton_Click);
            // 
            // DeleteAttributeButton
            // 
            this.DeleteAttributeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeleteAttributeButton.Location = new System.Drawing.Point(769, 376);
            this.DeleteAttributeButton.Name = "DeleteAttributeButton";
            this.DeleteAttributeButton.Size = new System.Drawing.Size(151, 30);
            this.DeleteAttributeButton.TabIndex = 10;
            this.DeleteAttributeButton.Text = "Delete attribute";
            this.DeleteAttributeButton.UseVisualStyleBackColor = true;
            this.DeleteAttributeButton.Click += new System.EventHandler(this.DeleteAttributeButton_Click);
            // 
            // DeleteTableButton
            // 
            this.DeleteTableButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeleteTableButton.Location = new System.Drawing.Point(769, 340);
            this.DeleteTableButton.Name = "DeleteTableButton";
            this.DeleteTableButton.Size = new System.Drawing.Size(151, 30);
            this.DeleteTableButton.TabIndex = 9;
            this.DeleteTableButton.Text = "Delete table";
            this.DeleteTableButton.UseVisualStyleBackColor = true;
            this.DeleteTableButton.Click += new System.EventHandler(this.DeleteTableButton_Click);
            // 
            // DeleteDatabaseButton
            // 
            this.DeleteDatabaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeleteDatabaseButton.Location = new System.Drawing.Point(769, 304);
            this.DeleteDatabaseButton.Name = "DeleteDatabaseButton";
            this.DeleteDatabaseButton.Size = new System.Drawing.Size(151, 30);
            this.DeleteDatabaseButton.TabIndex = 8;
            this.DeleteDatabaseButton.Text = "Delete database";
            this.DeleteDatabaseButton.UseVisualStyleBackColor = true;
            this.DeleteDatabaseButton.Click += new System.EventHandler(this.DeleteDatabaseButton_Click);
            // 
            // masterAttribute
            // 
            this.masterAttribute.Frozen = true;
            this.masterAttribute.HeaderText = "Master attribute";
            this.masterAttribute.Name = "masterAttribute";
            this.masterAttribute.ReadOnly = true;
            this.masterAttribute.Width = 115;
            // 
            // SlaveAttribute
            // 
            this.SlaveAttribute.Frozen = true;
            this.SlaveAttribute.HeaderText = "Slave attribute";
            this.SlaveAttribute.Name = "SlaveAttribute";
            this.SlaveAttribute.ReadOnly = true;
            this.SlaveAttribute.Width = 115;
            // 
            // MasterAttributeID
            // 
            this.MasterAttributeID.HeaderText = "Master ID";
            this.MasterAttributeID.Name = "MasterAttributeID";
            this.MasterAttributeID.ReadOnly = true;
            // 
            // SlaveAttributeID
            // 
            this.SlaveAttributeID.HeaderText = "Slave ID";
            this.SlaveAttributeID.Name = "SlaveAttributeID";
            this.SlaveAttributeID.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 573);
            this.Controls.Add(this.DeleteLinkButton);
            this.Controls.Add(this.DeleteAttributeButton);
            this.Controls.Add(this.DeleteTableButton);
            this.Controls.Add(this.DeleteDatabaseButton);
            this.Controls.Add(this.LinksView);
            this.Controls.Add(this.ShowLinks);
            this.Controls.Add(this.CreateLinkButton);
            this.Controls.Add(this.showDatabaseButton);
            this.Controls.Add(this.CreateAttributeButton);
            this.Controls.Add(this.CreateTableButton);
            this.Controls.Add(this.CreateDatabaseDatabase);
            this.Controls.Add(this.DatabasesTree);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Тип конструктор БД";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LinksView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView DatabasesTree;
        private System.Windows.Forms.Button CreateDatabaseDatabase;
        private System.Windows.Forms.Button CreateTableButton;
        private System.Windows.Forms.Button CreateAttributeButton;
        private System.Windows.Forms.Button showDatabaseButton;
        private System.Windows.Forms.Button CreateLinkButton;
        private System.Windows.Forms.Button ShowLinks;
        private System.Windows.Forms.DataGridView LinksView;
        private System.Windows.Forms.Button DeleteLinkButton;
        private System.Windows.Forms.Button DeleteAttributeButton;
        private System.Windows.Forms.Button DeleteTableButton;
        private System.Windows.Forms.Button DeleteDatabaseButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterAttribute;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlaveAttribute;
        private System.Windows.Forms.DataGridViewTextBoxColumn MasterAttributeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlaveAttributeID;
    }
}

