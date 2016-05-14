namespace DiabloInterface
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.button1 = new System.Windows.Forms.Button();
            this.lvlLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.strengthLabel = new System.Windows.Forms.Label();
            this.dexterityLabel = new System.Windows.Forms.Label();
            this.vitalityLabel = new System.Windows.Forms.Label();
            this.energyLabel = new System.Windows.Forms.Label();
            this.fireResLabel = new System.Windows.Forms.Label();
            this.coldResLabel = new System.Windows.Forms.Label();
            this.lightningResLabel = new System.Windows.Forms.Label();
            this.poisonResLabel = new System.Windows.Forms.Label();
            this.goldLabel = new System.Windows.Forms.Label();
            this.deathsLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lvlLabel
            // 
            this.lvlLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lvlLabel.ForeColor = System.Drawing.Color.White;
            this.lvlLabel.Location = new System.Drawing.Point(244, 63);
            this.lvlLabel.Name = "lvlLabel";
            this.lvlLabel.Size = new System.Drawing.Size(80, 16);
            this.lvlLabel.TabIndex = 2;
            this.lvlLabel.Text = "LVL: -";
            // 
            // nameLabel
            // 
            this.nameLabel.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.nameLabel.Location = new System.Drawing.Point(97, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(222, 33);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Char Name";
            // 
            // strengthLabel
            // 
            this.strengthLabel.AutoSize = true;
            this.strengthLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.strengthLabel.ForeColor = System.Drawing.Color.Coral;
            this.strengthLabel.Location = new System.Drawing.Point(100, 84);
            this.strengthLabel.Name = "strengthLabel";
            this.strengthLabel.Size = new System.Drawing.Size(56, 16);
            this.strengthLabel.TabIndex = 7;
            this.strengthLabel.Text = "STR: -";
            // 
            // dexterityLabel
            // 
            this.dexterityLabel.AutoSize = true;
            this.dexterityLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dexterityLabel.ForeColor = System.Drawing.Color.Coral;
            this.dexterityLabel.Location = new System.Drawing.Point(100, 100);
            this.dexterityLabel.Name = "dexterityLabel";
            this.dexterityLabel.Size = new System.Drawing.Size(56, 16);
            this.dexterityLabel.TabIndex = 8;
            this.dexterityLabel.Text = "DEX: -";
            // 
            // vitalityLabel
            // 
            this.vitalityLabel.AutoSize = true;
            this.vitalityLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.vitalityLabel.ForeColor = System.Drawing.Color.Coral;
            this.vitalityLabel.Location = new System.Drawing.Point(100, 116);
            this.vitalityLabel.Name = "vitalityLabel";
            this.vitalityLabel.Size = new System.Drawing.Size(56, 16);
            this.vitalityLabel.TabIndex = 10;
            this.vitalityLabel.Text = "VIT: -";
            // 
            // energyLabel
            // 
            this.energyLabel.AutoSize = true;
            this.energyLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.energyLabel.ForeColor = System.Drawing.Color.Coral;
            this.energyLabel.Location = new System.Drawing.Point(100, 132);
            this.energyLabel.Name = "energyLabel";
            this.energyLabel.Size = new System.Drawing.Size(56, 16);
            this.energyLabel.TabIndex = 11;
            this.energyLabel.Text = "ENE: -";
            // 
            // fireResLabel
            // 
            this.fireResLabel.AutoSize = true;
            this.fireResLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fireResLabel.ForeColor = System.Drawing.Color.Red;
            this.fireResLabel.Location = new System.Drawing.Point(244, 84);
            this.fireResLabel.Name = "fireResLabel";
            this.fireResLabel.Size = new System.Drawing.Size(64, 16);
            this.fireResLabel.TabIndex = 12;
            this.fireResLabel.Text = "FIRE: -";
            // 
            // coldResLabel
            // 
            this.coldResLabel.AutoSize = true;
            this.coldResLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.coldResLabel.ForeColor = System.Drawing.Color.DodgerBlue;
            this.coldResLabel.Location = new System.Drawing.Point(244, 100);
            this.coldResLabel.Name = "coldResLabel";
            this.coldResLabel.Size = new System.Drawing.Size(64, 16);
            this.coldResLabel.TabIndex = 13;
            this.coldResLabel.Text = "COLD: -";
            // 
            // lightningResLabel
            // 
            this.lightningResLabel.AutoSize = true;
            this.lightningResLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lightningResLabel.ForeColor = System.Drawing.Color.Yellow;
            this.lightningResLabel.Location = new System.Drawing.Point(244, 116);
            this.lightningResLabel.Name = "lightningResLabel";
            this.lightningResLabel.Size = new System.Drawing.Size(64, 16);
            this.lightningResLabel.TabIndex = 14;
            this.lightningResLabel.Text = "LIGH: -";
            // 
            // poisonResLabel
            // 
            this.poisonResLabel.AutoSize = true;
            this.poisonResLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.poisonResLabel.ForeColor = System.Drawing.Color.YellowGreen;
            this.poisonResLabel.Location = new System.Drawing.Point(244, 132);
            this.poisonResLabel.Name = "poisonResLabel";
            this.poisonResLabel.Size = new System.Drawing.Size(64, 16);
            this.poisonResLabel.TabIndex = 15;
            this.poisonResLabel.Text = "POIS: -";
            // 
            // goldLabel
            // 
            this.goldLabel.AutoSize = true;
            this.goldLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.goldLabel.ForeColor = System.Drawing.Color.Gold;
            this.goldLabel.Location = new System.Drawing.Point(100, 63);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(64, 16);
            this.goldLabel.TabIndex = 16;
            this.goldLabel.Text = "GOLD: -";
            // 
            // deathsLabel
            // 
            this.deathsLabel.AutoSize = true;
            this.deathsLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deathsLabel.ForeColor = System.Drawing.Color.Snow;
            this.deathsLabel.Location = new System.Drawing.Point(100, 42);
            this.deathsLabel.Name = "deathsLabel";
            this.deathsLabel.Size = new System.Drawing.Size(80, 16);
            this.deathsLabel.TabIndex = 17;
            this.deathsLabel.Text = "DEATHS: -";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 63);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(61, 24);
            this.button3.TabIndex = 20;
            this.button3.Text = "Settings";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(12, 181);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 219);
            this.panel1.TabIndex = 21;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(334, 412);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.deathsLabel);
            this.Controls.Add(this.goldLabel);
            this.Controls.Add(this.poisonResLabel);
            this.Controls.Add(this.lightningResLabel);
            this.Controls.Add(this.coldResLabel);
            this.Controls.Add(this.fireResLabel);
            this.Controls.Add(this.energyLabel);
            this.Controls.Add(this.vitalityLabel);
            this.Controls.Add(this.dexterityLabel);
            this.Controls.Add(this.strengthLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.lvlLabel);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "DiabloInterface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lvlLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label strengthLabel;
        private System.Windows.Forms.Label dexterityLabel;
        private System.Windows.Forms.Label vitalityLabel;
        private System.Windows.Forms.Label energyLabel;
        private System.Windows.Forms.Label fireResLabel;
        private System.Windows.Forms.Label coldResLabel;
        private System.Windows.Forms.Label lightningResLabel;
        private System.Windows.Forms.Label poisonResLabel;
        private System.Windows.Forms.Label goldLabel;
        private System.Windows.Forms.Label deathsLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel1;
    }
}

