using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Entities;

namespace Forms
{
    public partial class LinkCreationForm : Form
    {
        private bool isCascadeDelete = false,
        isCascadeUpdate = false;
        private string slaveTable;
        private string masterTable;
        private List<TreeNode> listOfConnectiableNodes = new List<TreeNode>();
        public LinkCreationForm()
        {
            InitializeComponent();
        }

        private void LinkCreationForm_Load(object sender, EventArgs e)
        {
            LinksBox.Items.Clear();
            foreach (TreeNode node in listOfConnectiableNodes)
                LinksBox.Items.Add(node.Text);
            MTable.Text = masterTable;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            isCascadeDelete = CascadeDelete.AutoCheck;
            isCascadeUpdate = CascadeUpdate.AutoCheck;
            slaveTable = LinksBox.Text;
            Close();
        }

        public void setListOfNodes(List<TreeNode> list)
        {
            listOfConnectiableNodes = list;
        }

        public void setMasterTable(string mTable)
        {
            masterTable = mTable;
        }

        public string getSlaveTable()
        {
            return slaveTable;
        }

        public bool getCascadeUpdate()
        {
            return isCascadeUpdate;
        }

        public bool getCascadeDelete()
        {
            return isCascadeDelete;
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
