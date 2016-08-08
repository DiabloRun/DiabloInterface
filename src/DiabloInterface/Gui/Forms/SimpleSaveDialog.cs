using System;
using System.IO;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Gui.Forms
{
    public partial class SimpleSaveDialog : Form
    {
        const string CREATE_NEW_FILE = "Create new file";
        const string CLONE_FILE = "Clone file";
        const string ENTER_VALID_NAME = "Sorry , please enter a valid name";


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
                Text = CREATE_NEW_FILE;
            }
            else
            {
                Text = CLONE_FILE;
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
                MessageBox.Show(ENTER_VALID_NAME);
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
