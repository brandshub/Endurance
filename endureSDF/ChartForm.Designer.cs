namespace endureSDF
{
    partial class ChartForm
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
            this.canvas = new System.Windows.Forms.PictureBox();
            this.monthes_LB = new System.Windows.Forms.ListBox();
            this.avgSpeed_RB = new System.Windows.Forms.RadioButton();
            this.distance_RB = new System.Windows.Forms.RadioButton();
            this.speed_RB = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dots_CB = new System.Windows.Forms.CheckBox();
            this.avg_CB = new System.Windows.Forms.CheckBox();
            this.routes_LB = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.Location = new System.Drawing.Point(12, 12);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(666, 583);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // monthes_LB
            // 
            this.monthes_LB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.monthes_LB.FormattingEnabled = true;
            this.monthes_LB.Location = new System.Drawing.Point(699, 35);
            this.monthes_LB.Name = "monthes_LB";
            this.monthes_LB.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.monthes_LB.Size = new System.Drawing.Size(142, 238);
            this.monthes_LB.TabIndex = 1;
            this.monthes_LB.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // avgSpeed_RB
            // 
            this.avgSpeed_RB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.avgSpeed_RB.AutoSize = true;
            this.avgSpeed_RB.Checked = true;
            this.avgSpeed_RB.Location = new System.Drawing.Point(699, 505);
            this.avgSpeed_RB.Name = "avgSpeed_RB";
            this.avgSpeed_RB.Size = new System.Drawing.Size(122, 17);
            this.avgSpeed_RB.TabIndex = 2;
            this.avgSpeed_RB.TabStop = true;
            this.avgSpeed_RB.Text = "Середня швидкість";
            this.avgSpeed_RB.UseVisualStyleBackColor = true;
            this.avgSpeed_RB.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // distance_RB
            // 
            this.distance_RB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.distance_RB.AutoSize = true;
            this.distance_RB.Location = new System.Drawing.Point(699, 535);
            this.distance_RB.Name = "distance_RB";
            this.distance_RB.Size = new System.Drawing.Size(109, 17);
            this.distance_RB.TabIndex = 3;
            this.distance_RB.Text = "Пройдений шлях";
            this.distance_RB.UseVisualStyleBackColor = true;
            this.distance_RB.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // speed_RB
            // 
            this.speed_RB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.speed_RB.AutoSize = true;
            this.speed_RB.Location = new System.Drawing.Point(699, 520);
            this.speed_RB.Name = "speed_RB";
            this.speed_RB.Size = new System.Drawing.Size(77, 17);
            this.speed_RB.TabIndex = 4;
            this.speed_RB.TabStop = true;
            this.speed_RB.Text = "Швидкість";
            this.speed_RB.UseVisualStyleBackColor = true;
            this.speed_RB.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(696, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Місяці";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(696, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Маршрути";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(697, 484);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Режим";
            // 
            // dots_CB
            // 
            this.dots_CB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dots_CB.AutoSize = true;
            this.dots_CB.Checked = true;
            this.dots_CB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dots_CB.Location = new System.Drawing.Point(700, 561);
            this.dots_CB.Name = "dots_CB";
            this.dots_CB.Size = new System.Drawing.Size(74, 17);
            this.dots_CB.TabIndex = 8;
            this.dots_CB.Text = "Крапочки";
            this.dots_CB.UseVisualStyleBackColor = true;
            this.dots_CB.CheckedChanged += new System.EventHandler(this.PbInvalidation);
            // 
            // avg_CB
            // 
            this.avg_CB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.avg_CB.AutoSize = true;
            this.avg_CB.Location = new System.Drawing.Point(700, 578);
            this.avg_CB.Name = "avg_CB";
            this.avg_CB.Size = new System.Drawing.Size(69, 17);
            this.avg_CB.TabIndex = 9;
            this.avg_CB.Text = "Середнє";
            this.avg_CB.UseVisualStyleBackColor = true;
            this.avg_CB.Visible = false;
            this.avg_CB.CheckedChanged += new System.EventHandler(this.PbInvalidation);
            // 
            // routes_LB
            // 
            this.routes_LB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.routes_LB.FormattingEnabled = true;
            this.routes_LB.Location = new System.Drawing.Point(699, 318);
            this.routes_LB.Name = "routes_LB";
            this.routes_LB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.routes_LB.Size = new System.Drawing.Size(142, 160);
            this.routes_LB.TabIndex = 10;
            this.routes_LB.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 607);
            this.Controls.Add(this.routes_LB);
            this.Controls.Add(this.avg_CB);
            this.Controls.Add(this.dots_CB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.speed_RB);
            this.Controls.Add(this.distance_RB);
            this.Controls.Add(this.avgSpeed_RB);
            this.Controls.Add(this.monthes_LB);
            this.Controls.Add(this.canvas);
            this.MinimumSize = new System.Drawing.Size(862, 634);
            this.Name = "ChartForm";
            this.ShowInTaskbar = false;
            this.Text = "ChartForm";
            this.SizeChanged += new System.EventHandler(this.PbInvalidation);
            this.ResizeEnd += new System.EventHandler(this.PbInvalidation);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.ListBox monthes_LB;
        private System.Windows.Forms.RadioButton avgSpeed_RB;
        private System.Windows.Forms.RadioButton distance_RB;
        private System.Windows.Forms.RadioButton speed_RB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox dots_CB;
        private System.Windows.Forms.CheckBox avg_CB;
        private System.Windows.Forms.ListBox routes_LB;
    }
}