namespace scorecard
{
    partial class scorecard_form
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
            System.Windows.Forms.Button button;
            this.customer_select_label = new System.Windows.Forms.Label();
            this.customer_select_combobox = new System.Windows.Forms.ComboBox();
            this.button_label = new System.Windows.Forms.Label();
            this.output = new System.Windows.Forms.RichTextBox();
            button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button
            // 
            button.Location = new System.Drawing.Point(18, 103);
            button.Margin = new System.Windows.Forms.Padding(4);
            button.Name = "button";
            button.Size = new System.Drawing.Size(143, 28);
            button.TabIndex = 3;
            button.Text = "Get Scorecard";
            button.UseVisualStyleBackColor = true;
            button.Click += new System.EventHandler(this.button_Click);
            // 
            // customer_select_label
            // 
            this.customer_select_label.AutoSize = true;
            this.customer_select_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.customer_select_label.Location = new System.Drawing.Point(18, 18);
            this.customer_select_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.customer_select_label.Name = "customer_select_label";
            this.customer_select_label.Size = new System.Drawing.Size(143, 16);
            this.customer_select_label.TabIndex = 0;
            this.customer_select_label.Text = "Choose a Customer";
            // 
            // customer_select_combobox
            // 
            this.customer_select_combobox.FormattingEnabled = true;
            this.customer_select_combobox.Location = new System.Drawing.Point(18, 38);
            this.customer_select_combobox.Margin = new System.Windows.Forms.Padding(4);
            this.customer_select_combobox.Name = "customer_select_combobox";
            this.customer_select_combobox.Size = new System.Drawing.Size(252, 24);
            this.customer_select_combobox.TabIndex = 1;
            // 
            // button_label
            // 
            this.button_label.AutoSize = true;
            this.button_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.button_label.Location = new System.Drawing.Point(18, 80);
            this.button_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.button_label.Name = "button_label";
            this.button_label.Size = new System.Drawing.Size(114, 16);
            this.button_label.TabIndex = 2;
            this.button_label.Text = "Click the Button";
            // 
            // output
            // 
            this.output.Location = new System.Drawing.Point(18, 152);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(520, 78);
            this.output.TabIndex = 4;
            this.output.Text = "";
            // 
            // scorecard_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 248);
            this.Controls.Add(this.output);
            this.Controls.Add(button);
            this.Controls.Add(this.button_label);
            this.Controls.Add(this.customer_select_combobox);
            this.Controls.Add(this.customer_select_label);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "scorecard_form";
            this.Text = "Scorecard Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label customer_select_label;
        private System.Windows.Forms.ComboBox customer_select_combobox;
        private System.Windows.Forms.Label button_label;
        private System.Windows.Forms.RichTextBox output;
    }
}

