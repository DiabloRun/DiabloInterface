using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiabloInterface.Gui.Controls
{
    public partial class SimpleSaveDialog : Form
    {
        public string FileName { get; set; }
        public string NewFileName { get { return txtNewFilename.Text; } }
        public SimpleSaveDialog()
        {
            InitializeComponent();
        }



        public SimpleSaveDialog(string fileName)
        {
            InitializeComponent();

            FileName = fileName;

            if (FileName == String.Empty)
            {
                Text = "Create new file";
            }
            else
            {
                Text = "Clone file";
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (CheckValidFilename())
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Sorry , please enter a valid name");
            }
        }

        private bool CheckValidFilename()
        {
            string fileName = txtNewFilename.Text;

            return !string.IsNullOrEmpty(fileName) &&
                   fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                   !File.Exists(Path.Combine(Application.StartupPath + @"\Settings", fileName));
        }

    }
}
