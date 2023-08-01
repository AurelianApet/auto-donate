namespace BetBasketBallChecker
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
            this.label1 = new System.Windows.Forms.Label();
            this.m_cartUrl = new System.Windows.Forms.TextBox();
            this.m_startBtn = new System.Windows.Forms.Button();
            this.m_inputData = new System.Windows.Forms.TextBox();
            this.m_checkResult = new System.Windows.Forms.TextBox();
            this.m_logBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.m_userId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_password = new System.Windows.Forms.TextBox();
            this.m_fileBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_liveCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.m_checkedCount = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.m_totalCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cart URL";
            // 
            // m_cartUrl
            // 
            this.m_cartUrl.Location = new System.Drawing.Point(148, 43);
            this.m_cartUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_cartUrl.Name = "m_cartUrl";
            this.m_cartUrl.Size = new System.Drawing.Size(1215, 22);
            this.m_cartUrl.TabIndex = 1;
            this.m_cartUrl.Text = "https://donate.twr.org/p-241-twr360.aspx";
            // 
            // m_startBtn
            // 
            this.m_startBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_startBtn.Location = new System.Drawing.Point(1193, 89);
            this.m_startBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_startBtn.Name = "m_startBtn";
            this.m_startBtn.Size = new System.Drawing.Size(120, 32);
            this.m_startBtn.TabIndex = 2;
            this.m_startBtn.Text = "Start";
            this.m_startBtn.UseVisualStyleBackColor = true;
            this.m_startBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_inputData
            // 
            this.m_inputData.Location = new System.Drawing.Point(41, 183);
            this.m_inputData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_inputData.Multiline = true;
            this.m_inputData.Name = "m_inputData";
            this.m_inputData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_inputData.Size = new System.Drawing.Size(391, 493);
            this.m_inputData.TabIndex = 3;
            this.m_inputData.Text = "5473700121011490 06/19 250\r\n5204164991561199 04/20 574\r\n5555276267772139 12/18 887\r\n5544417649859624 12/18 23" +
    "8\r\n5544417649859624 12/18 238\r\n5211302185668591 12/18 094\r\n4590613198011049 03/1" +
    "8 642";
            // 
            // m_checkResult
            // 
            this.m_checkResult.Location = new System.Drawing.Point(479, 183);
            this.m_checkResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_checkResult.Multiline = true;
            this.m_checkResult.Name = "m_checkResult";
            this.m_checkResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_checkResult.Size = new System.Drawing.Size(391, 493);
            this.m_checkResult.TabIndex = 4;
            // 
            // m_logBox
            // 
            this.m_logBox.Location = new System.Drawing.Point(917, 183);
            this.m_logBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_logBox.Multiline = true;
            this.m_logBox.Name = "m_logBox";
            this.m_logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_logBox.Size = new System.Drawing.Size(648, 493);
            this.m_logBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 153);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Input Data here";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(609, 153);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Checked Data";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1231, 153);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Log";
            // 
            // m_userId
            // 
            this.m_userId.Location = new System.Drawing.Point(148, 94);
            this.m_userId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_userId.Name = "m_userId";
            this.m_userId.Size = new System.Drawing.Size(300, 22);
            this.m_userId.TabIndex = 9;
            this.m_userId.Text = "joker9393939393@gmail.com";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(56, 101);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "User Id";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(532, 101);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Password";
            // 
            // m_password
            // 
            this.m_password.Location = new System.Drawing.Point(643, 92);
            this.m_password.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_password.Name = "m_password";
            this.m_password.Size = new System.Drawing.Size(313, 22);
            this.m_password.TabIndex = 12;
            this.m_password.Text = "Asdasd123@";
            // 
            // m_fileBtn
            // 
            this.m_fileBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_fileBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.m_fileBtn.Location = new System.Drawing.Point(1028, 89);
            this.m_fileBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_fileBtn.Name = "m_fileBtn";
            this.m_fileBtn.Size = new System.Drawing.Size(136, 32);
            this.m_fileBtn.TabIndex = 13;
            this.m_fileBtn.Text = "Select File";
            this.m_fileBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.m_fileBtn.UseVisualStyleBackColor = true;
            this.m_fileBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panel1.Controls.Add(this.m_liveCount);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.m_checkedCount);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.m_totalCount);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(1, 699);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1667, 46);
            this.panel1.TabIndex = 14;
            // 
            // m_liveCount
            // 
            this.m_liveCount.AutoSize = true;
            this.m_liveCount.Location = new System.Drawing.Point(599, 15);
            this.m_liveCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_liveCount.Name = "m_liveCount";
            this.m_liveCount.Size = new System.Drawing.Size(16, 17);
            this.m_liveCount.TabIndex = 5;
            this.m_liveCount.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(547, 15);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "Live :";
            // 
            // m_checkedCount
            // 
            this.m_checkedCount.AutoSize = true;
            this.m_checkedCount.Location = new System.Drawing.Point(371, 15);
            this.m_checkedCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_checkedCount.Name = "m_checkedCount";
            this.m_checkedCount.Size = new System.Drawing.Size(16, 17);
            this.m_checkedCount.TabIndex = 3;
            this.m_checkedCount.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(288, 15);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "Checked :";
            // 
            // m_totalCount
            // 
            this.m_totalCount.AutoSize = true;
            this.m_totalCount.Location = new System.Drawing.Point(124, 15);
            this.m_totalCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_totalCount.Name = "m_totalCount";
            this.m_totalCount.Size = new System.Drawing.Size(16, 17);
            this.m_totalCount.TabIndex = 1;
            this.m_totalCount.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(67, 15);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Total :";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1341, 87);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 32);
            this.button1.TabIndex = 15;
            this.button1.Text = "End";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1668, 741);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.m_fileBtn);
            this.Controls.Add(this.m_password);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.m_userId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_logBox);
            this.Controls.Add(this.m_checkResult);
            this.Controls.Add(this.m_inputData);
            this.Controls.Add(this.m_startBtn);
            this.Controls.Add(this.m_cartUrl);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.RightToLeftLayout = true;
            this.Text = "BasketBallChecker";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_cartUrl;
        private System.Windows.Forms.Button m_startBtn;
        private System.Windows.Forms.TextBox m_inputData;
        private System.Windows.Forms.TextBox m_checkResult;
        private System.Windows.Forms.TextBox m_logBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_userId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button m_fileBtn;
        private System.Windows.Forms.TextBox m_password;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label m_checkedCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label m_totalCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label m_liveCount;
        private System.Windows.Forms.Button button1;
    }
}

