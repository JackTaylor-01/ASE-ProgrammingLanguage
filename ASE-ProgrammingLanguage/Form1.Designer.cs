namespace ASE_ProgrammingLanguage
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
            this.cmdWindow = new System.Windows.Forms.TextBox();
            this.cmdLine = new System.Windows.Forms.TextBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonSyntax = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.drawOutput = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.drawOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdWindow
            // 
            this.cmdWindow.Location = new System.Drawing.Point(12, 34);
            this.cmdWindow.Multiline = true;
            this.cmdWindow.Name = "cmdWindow";
            this.cmdWindow.Size = new System.Drawing.Size(307, 349);
            this.cmdWindow.TabIndex = 0;
            this.cmdWindow.TextChanged += new System.EventHandler(this.cmdWindow_TextChanged);
            // 
            // cmdLine
            // 
            this.cmdLine.Location = new System.Drawing.Point(12, 389);
            this.cmdLine.Name = "cmdLine";
            this.cmdLine.Size = new System.Drawing.Size(307, 20);
            this.cmdLine.TabIndex = 1;
            this.cmdLine.TextChanged += new System.EventHandler(this.cmdLine_TextChanged);
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(12, 415);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 2;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonSyntax
            // 
            this.buttonSyntax.Location = new System.Drawing.Point(93, 415);
            this.buttonSyntax.Name = "buttonSyntax";
            this.buttonSyntax.Size = new System.Drawing.Size(75, 23);
            this.buttonSyntax.TabIndex = 3;
            this.buttonSyntax.Text = "Syntax";
            this.buttonSyntax.UseVisualStyleBackColor = true;
            this.buttonSyntax.Click += new System.EventHandler(this.buttonSyntax_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(12, 5);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 4;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(93, 5);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // drawOutput
            // 
            this.drawOutput.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.drawOutput.Location = new System.Drawing.Point(325, 34);
            this.drawOutput.Name = "drawOutput";
            this.drawOutput.Size = new System.Drawing.Size(451, 375);
            this.drawOutput.TabIndex = 6;
            this.drawOutput.TabStop = false;
            this.drawOutput.Click += new System.EventHandler(this.drawOutput_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.drawOutput);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.buttonSyntax);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.cmdLine);
            this.Controls.Add(this.cmdWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "ASE-Programming Language";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.drawOutput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cmdWindow;
        private System.Windows.Forms.TextBox cmdLine;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonSyntax;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.PictureBox drawOutput;
    }
}

