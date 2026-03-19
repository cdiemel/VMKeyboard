namespace VMKeyboard
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Button_Type = new System.Windows.Forms.Button();
            this.Text_Timeout = new System.Windows.Forms.NumericUpDown();
            this.Text_Main = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Text_Timeout)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Type
            // 
            this.Button_Type.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_Type.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Type.Location = new System.Drawing.Point(59, 3);
            this.Button_Type.Name = "Button_Type";
            this.Button_Type.Size = new System.Drawing.Size(412, 36);
            this.Button_Type.TabIndex = 1;
            this.Button_Type.Text = "Type!";
            this.Button_Type.UseVisualStyleBackColor = true;
            this.Button_Type.Click += new System.EventHandler(this.Button_Type_Click);
            // 
            // Text_Timeout
            // 
            this.Text_Timeout.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Text_Timeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_Timeout.Location = new System.Drawing.Point(11, 3);
            this.Text_Timeout.Name = "Text_Timeout";
            this.Text_Timeout.Size = new System.Drawing.Size(41, 32);
            this.Text_Timeout.TabIndex = 2;
            this.Text_Timeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // Text_Main
            // 
            this.Text_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Main.Location = new System.Drawing.Point(3, 3);
            this.Text_Main.Name = "Text_Main";
            this.Text_Main.Size = new System.Drawing.Size(454, 34);
            this.Text_Main.TabIndex = 3;
            this.Text_Main.Text = "";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.Text_Timeout);
            this.panel1.Controls.Add(this.Button_Type);
            this.panel1.Location = new System.Drawing.Point(1, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 41);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.Text_Main);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(460, 40);
            this.panel2.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 111);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "VMKeyboard v1.0";
            ((System.ComponentModel.ISupportInitialize)(this.Text_Timeout)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Button_Type;
        private System.Windows.Forms.NumericUpDown Text_Timeout;
        private System.Windows.Forms.RichTextBox Text_Main;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}

