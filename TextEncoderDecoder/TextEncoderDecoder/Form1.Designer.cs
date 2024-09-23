namespace TextEncoderDecoder
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnEncode = new System.Windows.Forms.Button();
            this.btnDecode = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDecodedOutput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBinaryOutput = new System.Windows.Forms.TextBox();
            this.btnBinaryEncode = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            resources.ApplyResources(this.txtInput, "txtInput");
            this.txtInput.Name = "txtInput";
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // btnEncode
            // 
            this.btnEncode.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.btnEncode, "btnEncode");
            this.btnEncode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.UseVisualStyleBackColor = false;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // btnDecode
            // 
            this.btnDecode.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.btnDecode, "btnDecode");
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.UseVisualStyleBackColor = false;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // txtOutput
            // 
            resources.ApplyResources(this.txtOutput, "txtOutput");
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.TextChanged += new System.EventHandler(this.txtOutput_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtDecodedOutput
            // 
            resources.ApplyResources(this.txtDecodedOutput, "txtDecodedOutput");
            this.txtDecodedOutput.Name = "txtDecodedOutput";
            this.txtDecodedOutput.ReadOnly = true;
            this.txtDecodedOutput.TextChanged += new System.EventHandler(this.txtDecodedOutput_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtBinaryOutput
            // 
            resources.ApplyResources(this.txtBinaryOutput, "txtBinaryOutput");
            this.txtBinaryOutput.Name = "txtBinaryOutput";
            this.txtBinaryOutput.ReadOnly = true;
            this.txtBinaryOutput.TextChanged += new System.EventHandler(this.txtBinaryOutput_TextChanged);
            // 
            // btnBinaryEncode
            // 
            this.btnBinaryEncode.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.btnBinaryEncode, "btnBinaryEncode");
            this.btnBinaryEncode.Name = "btnBinaryEncode";
            this.btnBinaryEncode.UseVisualStyleBackColor = false;
            this.btnBinaryEncode.Click += new System.EventHandler(this.btnBinaryEncode_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBinaryEncode);
            this.Controls.Add(this.txtBinaryOutput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDecodedOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtInput);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnEncode;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDecodedOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBinaryOutput;
        private System.Windows.Forms.Button btnBinaryEncode;
        private System.Windows.Forms.Label label4;
    }
}
