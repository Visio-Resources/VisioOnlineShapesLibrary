namespace VisioAddin.Ui
{
    partial class FrmContributeStencil
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnContribute = new System.Windows.Forms.Button();
            this.lbStencils = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(632, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnContribute
            // 
            this.btnContribute.Enabled = false;
            this.btnContribute.Location = new System.Drawing.Point(713, 415);
            this.btnContribute.Name = "btnContribute";
            this.btnContribute.Size = new System.Drawing.Size(75, 23);
            this.btnContribute.TabIndex = 2;
            this.btnContribute.Text = "Contribute";
            this.btnContribute.UseVisualStyleBackColor = true;
            this.btnContribute.Click += new System.EventHandler(this.btnContribute_Click);
            // 
            // lbStencils
            // 
            this.lbStencils.FormattingEnabled = true;
            this.lbStencils.ItemHeight = 16;
            this.lbStencils.Location = new System.Drawing.Point(12, 12);
            this.lbStencils.Name = "lbStencils";
            this.lbStencils.Size = new System.Drawing.Size(776, 340);
            this.lbStencils.TabIndex = 4;
            this.lbStencils.SelectedIndexChanged += new System.EventHandler(this.lbStencils_SelectedIndexChanged);
            // 
            // FrmContributeStencil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbStencils);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContribute);
            this.Name = "FrmContributeStencil";
            this.Text = "Contribute Stencil";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnContribute;
        private System.Windows.Forms.ListBox lbStencils;
    }
}