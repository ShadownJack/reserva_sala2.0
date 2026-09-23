using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ReservaSala
{
    public partial class frmReserva : Form
    {
        private static readonly Dictionary<string, int> CapacidadeSalas = new Dictionary<string, int>
        {
            { "Osasco", 10 },
            { "Jundiaí", 10 },
            { "Iguatu", 10 },
            { "Campos do Jordão", 10 },
            { "São Caetano", 10 },
            { "Santo André", 10 },
            { "São Bernardo do Campo", 10 },
        };

        public frmReserva()
        {
            InitializeComponent();
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            List<string> erros = ValidarCampos();
            if (erros.Count > 0)
            {
                MessageBox.Show(
                    "Não foi possível criar a reserva:" + Environment.NewLine + Environment.NewLine +
                    "- " + string.Join(Environment.NewLine + "- ", erros),
                    "Campos inválidos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Reserva reserva = CriarReserva();

            // RF06 / CA06 / CA07 - conflito de horário na mesma sala/data
            if (VerificarConflito(reserva))
            {
                MessageBox.Show(
                    "Já existe uma reserva para esta sala nesse período. Escolha outro horário, data ou sala.",
                    "Conflito de horário",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!SalvarReserva(reserva))
                return; // erro de E/S já foi exibido dentro de SalvarReserva

            MessageBox.Show("Reserva realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparFormulario();
        }

        /// <summary>
        /// Valida campos obrigatórios e regras de negócio de horário/capacidade:
        /// RF03/CA02, RF04/CA03, CA04, RF05/CA05, RF07/CA08.
        /// Retorna a lista de mensagens de erro; lista vazia significa que está tudo válido.
        /// </summary>
        private List<string> ValidarCampos()
        {
            var erros = new List<string>();

            // RF03 / CA02 - campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtNome.Text))
                erros.Add("Nome do responsável é obrigatório.");

            if (string.IsNullOrWhiteSpace(cmbSala.Text))
                erros.Add("Sala é obrigatória.");

            if (string.IsNullOrWhiteSpace(cmbHorario.Text))
                erros.Add("Horário inicial é obrigatório.");

            if (string.IsNullOrWhiteSpace(cmbHorarioFinal.Text))
                erros.Add("Horário final é obrigatório.");

            if (numParticipantes.Value <= 0)
                erros.Add("Quantidade de participantes é obrigatória.");

            // Observação: dtpData não entra nessa checagem. Sem ShowCheckBox habilitado,
            // um DateTimePicker sempre tem um valor selecionado — não existe "campo data vazio" possível.

            bool horarioInicialValido = TimeSpan.TryParse(cmbHorario.Text, out TimeSpan inicio);
            bool horarioFinalValido = TimeSpan.TryParse(cmbHorarioFinal.Text, out TimeSpan fim);

            if (!string.IsNullOrWhiteSpace(cmbHorario.Text) && !horarioInicialValido)
                erros.Add("Horário inicial inválido.");

            if (!string.IsNullOrWhiteSpace(cmbHorarioFinal.Text) && !horarioFinalValido)
                erros.Add("Horário final inválido.");

            if (horarioInicialValido && horarioFinalValido)
            {
                // CA04 - início deve ser estritamente anterior ao final
                if (inicio >= fim)
                {
                    erros.Add("O horário inicial deve ser anterior ao horário final.");
                }
                else
                {
                    // RF07 / CA08 - duração máxima de 4 horas (validação real, não depende apenas do combo)
                    TimeSpan duracao = fim - inicio;
                    if (duracao > TimeSpan.FromHours(4))
                        erros.Add("A reserva não pode ter duração superior a 4 horas.");
                }

                // RF04 / CA03 - antecedência mínima de 1 hora para reservas na data atual
                if (dtpData.Value.Date == DateTime.Today)
                {
                    DateTime inicioCompleto = dtpData.Value.Date + inicio;
                    if (inicioCompleto < DateTime.Now.AddHours(1))
                        erros.Add("Para reservas hoje, o horário inicial deve ser pelo menos 1 hora à frente do horário atual.");
                }
            }

            // RF05 / CA05 - capacidade máxima da sala
            if (!string.IsNullOrWhiteSpace(cmbSala.Text))
            {
                int capacidade = ObterCapacidadeSala(cmbSala.Text);
                if (numParticipantes.Value > capacidade)
                    erros.Add($"A sala \"{cmbSala.Text}\" comporta no máximo {capacidade} participante(s).");
            }

            return erros;
        }

        private int ObterCapacidadeSala(string sala)
        {
            if (CapacidadeSalas.TryGetValue(sala, out int capacidade))
                return capacidade;

            // Sala não cadastrada no dicionário: usa o limite do próprio NumericUpDown como fallback
            return (int)numParticipantes.Maximum;
        }

        private Reserva CriarReserva()
        {
            // ValidarCampos() já garantiu que os horários são parseáveis antes de chegar aqui;
            // o fallback abaixo é só uma proteção extra contra uso indevido futuro do método.
            if (!TimeSpan.TryParse(cmbHorario.Text, out TimeSpan horarioInicial))
                horarioInicial = TimeSpan.Zero;

            if (!TimeSpan.TryParse(cmbHorarioFinal.Text, out TimeSpan horarioFinalReserva))
                horarioFinalReserva = TimeSpan.Zero;

            int qtdParticipantes = (int)numParticipantes.Value;
            string equipamentosSelecionados = string.Join(", ", clbItens.CheckedItems.Cast<string>());

            return new Reserva(
                txtNome.Text.Trim(),
                cmbSala.Text,
                dtpData.Value.Date,
                horarioInicial,
                horarioFinalReserva,
                qtdParticipantes,
                equipamentosSelecionados
            );
        }

        private bool SalvarReserva(Reserva reserva)
        {
            try
            {
                string linha =
                    $"{reserva.Responsavel};" +
                    $"{reserva.Sala};" +
                    $"{reserva.Data:dd/MM/yyyy};" +
                    $"{reserva.Horario};" +
                    $"{reserva.HorarioFinal};" +
                    $"{reserva.QtdParticipantes};" +
                    $"{reserva.EquipamentosUtilizados}";

                File.AppendAllText("reservas.txt", linha + Environment.NewLine);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Não foi possível salvar a reserva no arquivo:{Environment.NewLine}{ex.Message}",
                    "Erro ao salvar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        private List<Reserva> CarregarReservas()
        {
            var reservas = new List<Reserva>();

            try
            {
                if (!File.Exists("reservas.txt")) return reservas;

                foreach (var linha in File.ReadAllLines("reservas.txt"))
                {
                    if (string.IsNullOrWhiteSpace(linha)) continue;

                    var d = linha.Split(';');
                    if (d.Length < 7) continue; // linha corrompida/incompleta: ignora com segurança

                    if (DateTime.TryParseExact(d[2], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data) &&
                        TimeSpan.TryParse(d[3], out TimeSpan inicio) &&
                        TimeSpan.TryParse(d[4], out TimeSpan fim) &&
                        int.TryParse(d[5], out int qtd))
                    {
                        reservas.Add(new Reserva(d[0], d[1], data, inicio, fim, qtd, d[6]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Não foi possível ler o arquivo de reservas:{Environment.NewLine}{ex.Message}",
                    "Erro ao carregar reservas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return reservas;
        }

        private bool VerificarConflito(Reserva novaReserva)
        {
            List<Reserva> reservas = CarregarReservas();

            TimeSpan inicioNova = novaReserva.Horario;
            TimeSpan fimNova = novaReserva.HorarioFinal;

            foreach (Reserva reserva in reservas)
            {
                if (reserva.Sala != novaReserva.Sala || reserva.Data != novaReserva.Data)
                    continue; // sala ou data diferente: não há como haver conflito

                TimeSpan inicioExistente = reserva.Horario;
                TimeSpan fimExistente = reserva.HorarioFinal;

                // Os intervalos só se cruzam se um começar antes do outro terminar
                // (CA06/CA07: uma reserva começando exatamente no fim da outra é permitida)
                if (inicioNova < fimExistente && inicioExistente < fimNova)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            var reservas = CarregarReservas();

            if (reservas.Count == 0)
            {
                lblReservas.Text = "Nenhuma reserva cadastrada até o momento.";
                return;
            }

            // RF08 / CA09 - exibe todas as informações cadastradas, incluindo participantes e equipamentos
            var linhas = reservas.Select(r =>
                $"{r.Responsavel} reservou a sala {r.Sala} em {r.Data:dd/MM/yyyy} " +
                $"das {r.Horario:hh\\:mm} às {r.HorarioFinal:hh\\:mm} " +
                $"para {r.QtdParticipantes} participante(s) " +
                $"— Equipamentos: {(string.IsNullOrWhiteSpace(r.EquipamentosUtilizados) ? "Nenhum" : r.EquipamentosUtilizados)}");

            lblReservas.Text =
                $"Quantidade de reservas já efetuadas: {reservas.Count} reserva(s)\n\n\n" +
                string.Join(Environment.NewLine, linhas);
        }

        // NOTE: método já existia no código original, mas nunca era chamado em lugar nenhum.
        // Não o utilizei: "só letras" não é uma regra pedida pelos RF/CA (RF03 só exige que o
        // nome não esteja vazio). Ativar essa checagem seria inventar validação fora do escopo
        // definido no documento. Fica aqui disponível caso você decida habilitar depois.
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

        private void LimparFormulario()
        {
            txtNome.Clear();
            cmbSala.SelectedIndex = -1;
            cmbHorario.SelectedIndex = -1;
            cmbHorarioFinal.Items.Clear();
            cmbHorarioFinal.Text = string.Empty;
            numParticipantes.Value = numParticipantes.Minimum;
            for (int i = 0; i < clbItens.Items.Count; i++)
                clbItens.SetItemChecked(i, false);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void frmReserva_Load(object sender, EventArgs e)
        {
        }

        private void cmbHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbHorarioFinal.Items.Clear();
            cmbHorarioFinal.Text = "";

            if (!TimeSpan.TryParse(cmbHorario.Text, out TimeSpan inicio) || string.IsNullOrWhiteSpace(cmbSala.Text))
                return;

            DateTime dataSelecionada = dtpData.Value.Date;
            string salaSelecionada = cmbSala.Text;

            // Teto padrão: máximo de 4 horas à frente (RF07/CA08)
            TimeSpan tetoMaximo = inicio.Add(TimeSpan.FromHours(4));

            var reservasExistentes = CarregarReservas()
                .Where(r => r.Sala == salaSelecionada && r.Data.Date == dataSelecionada)
                .ToList();

            // Se já existir reserva começando depois do nosso início, o teto vira o início dela (CA06/CA07)
            var proximaReserva = reservasExistentes
                .Where(r => r.Horario > inicio)
                .OrderBy(r => r.Horario)
                .FirstOrDefault();

            if (proximaReserva != null && proximaReserva.Horario < tetoMaximo)
            {
                tetoMaximo = proximaReserva.Horario;
            }

            TimeSpan passo = TimeSpan.FromMinutes(30);
            TimeSpan opcao = inicio.Add(passo);

            while (opcao <= tetoMaximo)
            {
                cmbHorarioFinal.Items.Add(opcao.ToString(@"hh\:mm"));
                opcao = opcao.Add(passo);
            }
        }

        private void cmbSala_DropDown(object sender, EventArgs e)
        {
        }

        private void cmbHorario_DropDown(object sender, EventArgs e)
        {
        }

        private void cmbHorarioFinal_DropDown(object sender, EventArgs e)
        {
        }
    }
}