namespace Overseas
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
            this.chooseFileButton = new System.Windows.Forms.Button();
            this.chosenFileLable = new System.Windows.Forms.Label();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.mainDataGrid = new System.Windows.Forms.DataGridView();
            this.detailsGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.countryLabel = new System.Windows.Forms.Label();
            this.postalCodeLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.statusDescriptionLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.scanDateStringLabel = new System.Windows.Forms.Label();
            this.ScanTimeStringLabel = new System.Windows.Forms.Label();
            this.statusNumberLabel = new System.Windows.Forms.Label();
            this.dateTimeFileCreatedLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // chooseFileButton
            // 
            this.chooseFileButton.Location = new System.Drawing.Point(15, 460);
            this.chooseFileButton.Name = "chooseFileButton";
            this.chooseFileButton.Size = new System.Drawing.Size(75, 45);
            this.chooseFileButton.TabIndex = 0;
            this.chooseFileButton.Text = "Odaberi datoteku";
            this.chooseFileButton.UseVisualStyleBackColor = true;
            this.chooseFileButton.Click += new System.EventHandler(this.ChooseFileButton_Click);
            // 
            // chosenFileLable
            // 
            this.chosenFileLable.AutoSize = true;
            this.chosenFileLable.Location = new System.Drawing.Point(12, 508);
            this.chosenFileLable.Name = "chosenFileLable";
            this.chosenFileLable.Size = new System.Drawing.Size(118, 13);
            this.chosenFileLable.TabIndex = 1;
            this.chosenFileLable.Text = "Nije odabrana datoteka";
            // 
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(116, 460);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(119, 45);
            this.loadFileButton.TabIndex = 2;
            this.loadFileButton.Text = "Učitaj nove podatke iz datoteke";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.LoadFileButton_Click);
            // 
            // mainDataGrid
            // 
            this.mainDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mainDataGrid.Location = new System.Drawing.Point(15, 12);
            this.mainDataGrid.Name = "mainDataGrid";
            this.mainDataGrid.Size = new System.Drawing.Size(514, 381);
            this.mainDataGrid.TabIndex = 3;
            this.mainDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MainDataGrid_CellClick);
            // 
            // detailsGrid
            // 
            this.detailsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.detailsGrid.Location = new System.Drawing.Point(608, 177);
            this.detailsGrid.Name = "detailsGrid";
            this.detailsGrid.Size = new System.Drawing.Size(825, 328);
            this.detailsGrid.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(996, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Primatelj:";
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(1154, 40);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(35, 13);
            this.countryLabel.TabIndex = 6;
            this.countryLabel.Text = "label1";
            // 
            // postalCodeLabel
            // 
            this.postalCodeLabel.AutoSize = true;
            this.postalCodeLabel.Location = new System.Drawing.Point(1154, 63);
            this.postalCodeLabel.Name = "postalCodeLabel";
            this.postalCodeLabel.Size = new System.Drawing.Size(35, 13);
            this.postalCodeLabel.TabIndex = 6;
            this.postalCodeLabel.Text = "label1";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(1154, 87);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(35, 13);
            this.cityLabel.TabIndex = 6;
            this.cityLabel.Text = "label1";
            // 
            // statusDescriptionLabel
            // 
            this.statusDescriptionLabel.AutoSize = true;
            this.statusDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusDescriptionLabel.Location = new System.Drawing.Point(603, 54);
            this.statusDescriptionLabel.MaximumSize = new System.Drawing.Size(350, 30);
            this.statusDescriptionLabel.MinimumSize = new System.Drawing.Size(350, 30);
            this.statusDescriptionLabel.Name = "statusDescriptionLabel";
            this.statusDescriptionLabel.Size = new System.Drawing.Size(350, 30);
            this.statusDescriptionLabel.TabIndex = 6;
            this.statusDescriptionLabel.Text = "label1";
            this.statusDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(605, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Trenutno stanje:";
            // 
            // scanDateStringLabel
            // 
            this.scanDateStringLabel.AutoSize = true;
            this.scanDateStringLabel.Location = new System.Drawing.Point(605, 86);
            this.scanDateStringLabel.Name = "scanDateStringLabel";
            this.scanDateStringLabel.Size = new System.Drawing.Size(35, 13);
            this.scanDateStringLabel.TabIndex = 6;
            this.scanDateStringLabel.Text = "label1";
            // 
            // ScanTimeStringLabel
            // 
            this.ScanTimeStringLabel.AutoSize = true;
            this.ScanTimeStringLabel.Location = new System.Drawing.Point(678, 86);
            this.ScanTimeStringLabel.Name = "ScanTimeStringLabel";
            this.ScanTimeStringLabel.Size = new System.Drawing.Size(35, 13);
            this.ScanTimeStringLabel.TabIndex = 6;
            this.ScanTimeStringLabel.Text = "label1";
            // 
            // statusNumberLabel
            // 
            this.statusNumberLabel.AutoSize = true;
            this.statusNumberLabel.Location = new System.Drawing.Point(736, 86);
            this.statusNumberLabel.Name = "statusNumberLabel";
            this.statusNumberLabel.Size = new System.Drawing.Size(35, 13);
            this.statusNumberLabel.TabIndex = 6;
            this.statusNumberLabel.Text = "label1";
            // 
            // dateTimeFileCreatedLabel
            // 
            this.dateTimeFileCreatedLabel.AutoSize = true;
            this.dateTimeFileCreatedLabel.Location = new System.Drawing.Point(12, 405);
            this.dateTimeFileCreatedLabel.Name = "dateTimeFileCreatedLabel";
            this.dateTimeFileCreatedLabel.Size = new System.Drawing.Size(35, 13);
            this.dateTimeFileCreatedLabel.TabIndex = 7;
            this.dateTimeFileCreatedLabel.Text = "label2";
            this.dateTimeFileCreatedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1070, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Država:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1070, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Poštanski broj:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1070, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Grad:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1464, 540);
            this.Controls.Add(this.dateTimeFileCreatedLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.postalCodeLabel);
            this.Controls.Add(this.countryLabel);
            this.Controls.Add(this.statusNumberLabel);
            this.Controls.Add(this.ScanTimeStringLabel);
            this.Controls.Add(this.scanDateStringLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.statusDescriptionLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.detailsGrid);
            this.Controls.Add(this.mainDataGrid);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.chosenFileLable);
            this.Controls.Add(this.chooseFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1480, 579);
            this.MinimumSize = new System.Drawing.Size(1480, 579);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Overseas - praćenje pošiljaka";
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button chooseFileButton;
        private System.Windows.Forms.Label chosenFileLable;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.DataGridView mainDataGrid;
        private System.Windows.Forms.DataGridView detailsGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label postalCodeLabel;
        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.Label statusDescriptionLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label scanDateStringLabel;
        private System.Windows.Forms.Label ScanTimeStringLabel;
        private System.Windows.Forms.Label statusNumberLabel;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.Label dateTimeFileCreatedLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

