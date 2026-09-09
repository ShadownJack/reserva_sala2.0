namespace ReservaSala
{
    partial class frmReserva
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblNome = new Label();
            lblSala = new Label();
            lblData = new Label();
            lblHorario = new Label();
            lblDuracao = new Label();
            btnReservar = new Button();
            txtNome = new TextBox();
            dtpData = new DateTimePicker();
            cmbSala = new ComboBox();
            cmbHorario = new ComboBox();
            cmbDuracao = new ComboBox();
            btnMostrar = new Button();
            lblReservas = new Label();
            SuspendLayout();
            // 
            // lblNome
            // 
            lblNome.AutoSize = true;
            lblNome.Location = new Point(34, 18);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(43, 15);
            lblNome.TabIndex = 0;
            lblNome.Text = "Nome:";
            // 
            // lblSala
            // 
            lblSala.AutoSize = true;
            lblSala.Location = new Point(46, 59);
            lblSala.Name = "lblSala";
            lblSala.Size = new Size(31, 15);
            lblSala.TabIndex = 1;
            lblSala.Text = "Sala:";
            // 
            // lblData
            // 
            lblData.AutoSize = true;
            lblData.Location = new Point(43, 106);
            lblData.Name = "lblData";
            lblData.Size = new Size(34, 15);
            lblData.TabIndex = 2;
            lblData.Text = "Data:";
            // 
            // lblHorario
            // 
            lblHorario.AutoSize = true;
            lblHorario.Location = new Point(27, 152);
            lblHorario.Name = "lblHorario";
            lblHorario.Size = new Size(50, 15);
            lblHorario.TabIndex = 3;
            lblHorario.Text = "Horário:";
            // 
            // lblDuracao
            // 
            lblDuracao.AutoSize = true;
            lblDuracao.Location = new Point(23, 200);
            lblDuracao.Name = "lblDuracao";
            lblDuracao.Size = new Size(54, 15);
            lblDuracao.TabIndex = 4;
            lblDuracao.Text = "Duração:";
            // 
            // btnReservar
            // 
            btnReservar.Location = new Point(23, 263);
            btnReservar.Name = "btnReservar";
            btnReservar.Size = new Size(251, 23);
            btnReservar.TabIndex = 5;
            btnReservar.Text = "Reservar";
            btnReservar.UseVisualStyleBackColor = true;
            btnReservar.Click += btnReservar_Click;
            // 
            // txtNome
            // 
            txtNome.Location = new Point(101, 18);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(173, 23);
            txtNome.TabIndex = 6;
            // 
            // dtpData
            // 
            dtpData.Checked = false;
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(101, 106);
            dtpData.MaxDate = new DateTime(2026, 8, 31, 0, 0, 0, 0);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(173, 23);
            dtpData.TabIndex = 9;
            dtpData.Value = new DateTime(2026, 8, 31, 0, 0, 0, 0);
            // 
            // cmbSala
            // 
            cmbSala.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSala.FormattingEnabled = true;
            cmbSala.Items.AddRange(new object[] { "Osasco", "Jundiaí", "Iguatu", "Campos do Jordão", "São Caetano", "Santo André", "São Bernardo do Campo" });
            cmbSala.Location = new Point(101, 59);
            cmbSala.Name = "cmbSala";
            cmbSala.Size = new Size(173, 23);
            cmbSala.TabIndex = 10;
            // 
            // cmbHorario
            // 
            cmbHorario.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHorario.FormattingEnabled = true;
            cmbHorario.Items.AddRange(new object[] { "00:00", "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30" });
            cmbHorario.Location = new Point(101, 152);
            cmbHorario.Name = "cmbHorario";
            cmbHorario.Size = new Size(173, 23);
            cmbHorario.TabIndex = 11;
            // 
            // cmbDuracao
            // 
            cmbDuracao.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDuracao.FormattingEnabled = true;
            cmbDuracao.Items.AddRange(new object[] { "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00" });
            cmbDuracao.Location = new Point(101, 200);
            cmbDuracao.Name = "cmbDuracao";
            cmbDuracao.Size = new Size(173, 23);
            cmbDuracao.TabIndex = 12;
            // 
            // btnMostrar
            // 
            btnMostrar.Location = new Point(23, 292);
            btnMostrar.Name = "btnMostrar";
            btnMostrar.Size = new Size(251, 23);
            btnMostrar.TabIndex = 14;
            btnMostrar.Text = "Mostrar reservas";
            btnMostrar.UseVisualStyleBackColor = true;
            btnMostrar.Click += btnMostrar_Click;
            // 
            // lblReservas
            // 
            lblReservas.AutoSize = true;
            lblReservas.Location = new Point(362, 19);
            lblReservas.Name = "lblReservas";
            lblReservas.Size = new Size(0, 15);
            lblReservas.TabIndex = 15;
            // 
            // frmReserva
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblReservas);
            Controls.Add(btnMostrar);
            Controls.Add(cmbDuracao);
            Controls.Add(cmbHorario);
            Controls.Add(cmbSala);
            Controls.Add(dtpData);
            Controls.Add(txtNome);
            Controls.Add(btnReservar);
            Controls.Add(lblDuracao);
            Controls.Add(lblHorario);
            Controls.Add(lblData);
            Controls.Add(lblSala);
            Controls.Add(lblNome);
            Name = "frmReserva";
            Text = "Reserva de Sala";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNome;
        private Label lblSala;
        private Label lblData;
        private Label lblHorario;
        private Label lblDuracao;
        private Button btnReservar;
        private TextBox txtNome;
        private DateTimePicker dtpData;
        private ComboBox cmbSala;
        private ComboBox cmbHorario;
        private ComboBox cmbDuracao;
        private Button btnMostrar;
        private Label lblReservas;
    }
}
