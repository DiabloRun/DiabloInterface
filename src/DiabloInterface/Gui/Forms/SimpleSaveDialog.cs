using System;
using System.IO;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Gui.Forms
{
    public class SimpleSaveDialog : Form
    {
        const string ENTER_VALID_NAME = "Please enter a valid file name";

        private TextBox txtNewFilename;

        public string NewFileName { get { return txtNewFilename.Text; } }
        
        public SimpleSaveDialog(string title, string fileName)
        {
            var btnSave = new Button();
            txtNewFilename = new TextBox();
            var btnCancel = new Button();
            SuspendLayout();

            btnSave.Location = new System.Drawing.Point(282, 12);
            btnSave.Size = new System.Drawing.Size(75, 20);
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new EventHandler(btnSave_Click);

            txtNewFilename.Location = new System.Drawing.Point(12, 12);
            txtNewFilename.Size = new System.Drawing.Size(264, 20);
            txtNewFilename.Text = fileName;
            txtNewFilename.KeyPress += new KeyPressEventHandler(txtNewFilename_KeyPress);

            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(363, 12);
            btnCancel.Size = new System.Drawing.Size(75, 20);
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(445, 42);
            Controls.Add(btnCancel);
            Controls.Add(txtNewFilename);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = Properties.Resources.di;
            MaximumSize = new System.Drawing.Size(461, 81);
            MinimumSize = new System.Drawing.Size(461, 81);
            Text = title;
            ResumeLayout(false);
            PerformLayout();
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
                Close();
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
            } else if (e.KeyChar == 27)
            {
                // esc is pressed
                Close();
            }
        }
    }
}
