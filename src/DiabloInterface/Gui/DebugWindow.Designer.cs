using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Plugin;

namespace Zutatensuppe.DiabloInterface.Gui
{
    partial class DebugWindow
    {
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugWindow));
            this.groupBox1 = new GroupBox();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.tabPage2 = new TabPage();
            this.tabPage3 = new TabPage();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.label10 = new Label();
            this.textItemDesc = new RichTextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(15, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(555, 555);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Quest-Bits";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(549, 536);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(541, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Normal";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(541, 510);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nightmare";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(541, 510);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Hell";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.ForeColor = System.Drawing.Color.Aquamarine;
            this.label1.Location = new System.Drawing.Point(700, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 40);
            this.label1.TabIndex = 5;
            this.label1.Text = "Head";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.LabelClick);
            this.label1.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.ForeColor = System.Drawing.Color.Aquamarine;
            this.label2.Location = new System.Drawing.Point(628, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 80);
            this.label2.TabIndex = 6;
            this.label2.Text = "Left Arm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.LabelClick);
            this.label2.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label2.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.ForeColor = System.Drawing.Color.Aquamarine;
            this.label3.Location = new System.Drawing.Point(772, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 80);
            this.label3.TabIndex = 7;
            this.label3.Text = "Right Arm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Click += new System.EventHandler(this.LabelClick);
            this.label3.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label3.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.ForeColor = System.Drawing.Color.Aquamarine;
            this.label4.Location = new System.Drawing.Point(700, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 60);
            this.label4.TabIndex = 8;
            this.label4.Text = "Body";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Click += new System.EventHandler(this.LabelClick);
            this.label4.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label4.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.ForeColor = System.Drawing.Color.Aquamarine;
            this.label5.Location = new System.Drawing.Point(628, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 40);
            this.label5.TabIndex = 9;
            this.label5.Text = "Gloves";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.LabelClick);
            this.label5.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label5.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.ForeColor = System.Drawing.Color.Aquamarine;
            this.label6.Location = new System.Drawing.Point(700, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Belt";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label6.Click += new System.EventHandler(this.LabelClick);
            this.label6.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label6.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.ForeColor = System.Drawing.Color.Aquamarine;
            this.label7.Location = new System.Drawing.Point(674, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "L";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label7.Click += new System.EventHandler(this.LabelClick);
            this.label7.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label7.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.ForeColor = System.Drawing.Color.Aquamarine;
            this.label8.Location = new System.Drawing.Point(746, 251);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "R";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label8.Click += new System.EventHandler(this.LabelClick);
            this.label8.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label8.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.ForeColor = System.Drawing.Color.Aquamarine;
            this.label9.Location = new System.Drawing.Point(772, 251);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 40);
            this.label9.TabIndex = 13;
            this.label9.Text = "Boots";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label9.Click += new System.EventHandler(this.LabelClick);
            this.label9.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label9.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.ForeColor = System.Drawing.Color.Aquamarine;
            this.label10.Location = new System.Drawing.Point(746, 149);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "A";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label10.Click += new System.EventHandler(this.LabelClick);
            this.label10.MouseEnter += new System.EventHandler(this.LabelMouseEnter);
            this.label10.MouseLeave += new System.EventHandler(this.LabelMouseLeave);
            // 
            // textItemDesc
            // 
            this.textItemDesc.Location = new System.Drawing.Point(579, 294);
            this.textItemDesc.Name = "textItemDesc";
            this.textItemDesc.Size = new System.Drawing.Size(284, 171);
            this.textItemDesc.TabIndex = 14;
            this.textItemDesc.Text = "";
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 571);
            this.Controls.Add(this.textItemDesc);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);

            foreach (Control p in di.plugins.DebugControls)
                this.Controls.Add(p);

            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugWindow";
            this.Text = "Debug";
            this.Load += new System.EventHandler(this.DebugWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private GroupBox groupBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private RichTextBox textItemDesc;
    }
}