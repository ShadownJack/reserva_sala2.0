using System.Globalization;

namespace ReservaSala
{
    public partial class frmReserva : Form
    {
        public frmReserva()
        {
            InitializeComponent();
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(cmbSala.Text) ||
                string.IsNullOrWhiteSpace(cmbHorario.Text) ||
                string.IsNullOrWhiteSpace(cmbDuracao.Text)
                )
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                return;
            }
            else if (!ValidarLetras(txtNome.Text))
            {
                MessageBox.Show("Não é permitido digitar números no campo nome");
                return;
            }
            Reserva reserva = CriarReserva();
            if (VerificarConflito(reserva))
            {
                MessageBox.Show("A sala já está reservada neste horário.");
                return;
            }
            SalvarReserva(reserva);
            MessageBox.Show("Reserva realizada com sucesso!");
        }

        private Reserva CriarReserva()
        {
            TimeSpan horario = TimeSpan.Parse(cmbHorario.Text);
            TimeSpan duracao = TimeSpan.Parse(cmbDuracao.Text);

            return new Reserva(
                txtNome.Text,
                cmbSala.Text,
                dtpData.Value.Date,
                horario,
                duracao
            );
        }

        private void SalvarReserva(Reserva reserva)
        {
            string linha =
                $"{reserva.Responsavel};" +
                $"{reserva.Sala};" +
                $"{reserva.Data:dd/MM/yyyy};" +
                $"{reserva.Horario};" +
                $"{reserva.Duracao:hh\\:mm\\:ss}";

            File.AppendAllText("reservas.txt", linha + Environment.NewLine);
        }

        private List<Reserva> CarregarReservas()
        {
            var reservas = new List<Reserva>();
            if (!File.Exists("reservas.txt")) return reservas;

            foreach (var linha in File.ReadAllLines("reservas.txt"))
            {
                var dados = linha.Split(';');
                if (dados.Length < 5) continue; // ou log

                if (!DateTime.TryParseExact(dados[2], "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out DateTime data))
                    continue; // ou log

                // horário: tenta TimeSpan
                if (!TimeSpan.TryParseExact(dados[3], @"hh\:mm\:ss", CultureInfo.InvariantCulture,
                                            out TimeSpan horario))
                {
                    if (!TimeSpan.TryParse(dados[3], out horario)) continue; // ou log
                }

                TimeSpan duracao;
                if (!TimeSpan.TryParseExact(dados[4], @"hh\:mm\:ss", CultureInfo.InvariantCulture, out duracao))
                {
                    // fallback: se for um DateTime completo, extrai o TimeOfDay
                    if (DateTime.TryParseExact(dados[4], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out DateTime dtDur))
                    {
                        duracao = dtDur.TimeOfDay;
                    }
                    else if (!TimeSpan.TryParse(dados[4], out duracao))
                    {
                        continue; // formato desconhecido -> ignora linha
                    }
                }

                reservas.Add(new Reserva(dados[0], dados[1], data, horario, duracao));
            }

            return reservas;
        }

        private bool VerificarConflito(Reserva novaReserva)
        {
            List<Reserva> reservas = CarregarReservas();

            TimeSpan inicioNova = novaReserva.Horario;
            TimeSpan fimNova = novaReserva.Horario + novaReserva.Duracao;

            foreach (Reserva reserva in reservas)
            {
                if (reserva.Sala != novaReserva.Sala || reserva.Data != novaReserva.Data)
                    continue; // sala ou data diferente, não precisa nem comparar horário

                TimeSpan inicioExistente = reserva.Horario;
                TimeSpan fimExistente = reserva.Horario + reserva.Duracao;

                // checa se os intervalos se cruzam
                if (inicioNova < fimExistente && inicioExistente < fimNova)
                {
                    return true; // tem conflito
                }
            }

            return false;
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            var reservas = CarregarReservas();
            lblReservas.Text = $"Quantidade de reservas já efetuadas: {reservas.Count.ToString()} reserva(s)\n\n\n" +
                string.Join(Environment.NewLine, reservas.Select(r => $"{r.Responsavel} reservou a sala {r.Sala} em {r.Data:dd/MM/yyyy} às {r.Horario} por {r.Duracao:hh\\:mm\\:ss}"));
        }

        private bool ValidarLetras(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}