using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Forms
{
    public partial class DatabaseCreationForm : Form
    {
        private readonly Regex _namingRegex = new Regex(@"^[a-zA-Z0-9_]*[a-zA-Z0-9]+[a-zA-Z0-9_]*$");
        public string InputText = string.Empty;
        private List<string> _nodes = new List<string>();

        public DatabaseCreationForm()
        {
            InitializeComponent();
        }

        public void CreateDatabase1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateDatabase2_Click(object sender, EventArgs e)
        {
            if (_namingRegex.IsMatch(DatabaseNameTextBox.Text))
            {
                bool flag = false;
                if (_nodes.Count != 0)
                {
                    foreach (string node in _nodes)
                    {
                        if (node == DatabaseNameTextBox.Text)
                        {
                            flag = true;
                        }
                    }
                }

                if (!flag)
                {
                    InputText = DatabaseNameTextBox.Text;
                    CreateDatabase1_Click(sender, e);
                }
                else
                {
                    MessageBox.Show(@"Такое имя уже существует!");
                }
            }
            else
            {
                MessageBox.Show(Text == @"База данных" ? "Введите название базы данных!" : (Text == @"Таблица" ? "Введите название таблицы!" : "Введите имя сервера!"));
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void SetNodes(List<string> nodes)
        {
            _nodes = nodes;
        }

        private void DatabaseCreationForm_Load(object sender, EventArgs e)
        {

        }
    }
}