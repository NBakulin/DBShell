using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Forms
{
    public partial class AttributeCreationForm : Form
    {
        private readonly Regex _attributeName = new Regex(@"^[a-zA-Z0-9_]*[a-zA-Z0-9]+[a-zA-Z0-9_]*$");
        private readonly Dictionary<string, string> _attribute = new Dictionary<string, string>();
        private List<string> _nodes = new List<string>();

        public AttributeCreationForm()
        {
            InitializeComponent();
        }

        private void CreateAttribute_Click(object sender, EventArgs e)
        {
            if (_attributeName.IsMatch(AttributeNameTextbox.Text))
            {
                _attribute.Add("Name", AttributeNameTextbox.Text);
            }

            _attribute.Add("SQLType", TypeComboBox.Text);
            _attribute.Add("Description", AttributeDescriptionTExtbox.Text);
            _attribute.Add("IsNullable", IsNullableFlag.AutoCheck.ToString());
            _attribute.Add("IsIndexed", IsIndexedFlag.AutoCheck.ToString());
            _attribute.Add("KeyType", FKcombo.Text);
            Close();
        }

        public void SetNodes(List<string> nodes)
        {
            _nodes = nodes;
        }

        public void IsPrimaryKeyAttribute(TreeNode node)
        {
            if (node.Text == @"PrimaryKey") { }
        }

        public Dictionary<string, string> GetAttributeDictionary()
        {
            return _attribute;
        }

        private void AttributeCreationForm_Load(object sender, EventArgs e)
        {

        }
    }
}