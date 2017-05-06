namespace Maze_Pres
{
    partial class FormMaze
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
            this.components = new System.ComponentModel.Container();
            this.timerGameTick = new System.Windows.Forms.Timer(this.components);
            this.timerMazeWinner = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerGameTick
            // 
            this.timerGameTick.Enabled = true;
            this.timerGameTick.Interval = 1000;
            this.timerGameTick.Tick += new System.EventHandler(this.timerGameTick_Tick);
            // 
            // timerMazeWinner
            // 
            this.timerMazeWinner.Interval = 5000;
            this.timerMazeWinner.Tick += new System.EventHandler(this.timerMazeWinner_Tick);
            // 
            // FormMaze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 612);
            this.Name = "FormMaze";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMaze_Load);
            this.Shown += new System.EventHandler(this.FormMaze_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMaze_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMaze_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormMaze_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerGameTick;
        private System.Windows.Forms.Timer timerMazeWinner;
    }
}

