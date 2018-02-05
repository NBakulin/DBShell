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
                switch (Text)
                {
                    case @"База данных":
                        MessageBox.Show(@"Введите название базы данных!");
                    break;
                    case @"Таблица":
                        MessageBox.Show(@"Введите название таблицы!");
                        break;
                    case @"Переименновать базу данных":
                        MessageBox.Show(@"Введите новое название базы данных!");
                        break;
                    case @"Переименновать таблицу":
                        MessageBox.Show(@"Введите новое название таблицы!");
                        break;
                    case @"Переименновать атрибут":
                        MessageBox.Show(@"Введите новое название атрибута!");
                        break;
                    default:
                        MessageBox.Show(@"Введите имя сервера!");
                        break;
                }
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

        public void SetTextboxValue(string newValue)
        {
            DatabaseNameTextBox.Text = newValue;
        }

    }
}