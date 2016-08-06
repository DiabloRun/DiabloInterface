using System;
using System.IO;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Gui.Forms
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
            txtNewFilename.Text = fileName;

            if (FileName == String.Empty)
            {
                Text = "Create new file";
            }
            else
            {
                Text = "Clone file";
            }

        }

        private bool CheckValidFilename()
        {
            string fileName = txtNewFilename.Text;

            return !string.IsNullOrEmpty(fileName) &&
                   fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                   !File.Exists(Path.Combine(Application.StartupPath + @"\Settings", fileName));
        }

        private void CheckAndCloseForm()
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            CheckAndCloseForm();
        }

        private void txtNewFilename_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                // enter is pressed:
                CheckAndCloseForm();
            } else if ( e.KeyChar == 27)
            {
                // esc is pressed
                this.Close();
            }
        }
    }
}
