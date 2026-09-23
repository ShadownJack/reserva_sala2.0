using System;

namespace ReservaSala
{
    class Reserva
    {
        public string Responsavel { get; set; }
        public string Sala { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Horario { get; set; }
        public TimeSpan HorarioFinal { get; set; }
        public int QtdParticipantes { get; set; }
        public string EquipamentosUtilizados { get; set; }

        public Reserva(string responsavel, string sala, DateTime data,
                        TimeSpan horario, TimeSpan horarioFinal,
                        int qtdParticipantes, string equipamentosUtilizados)
        {
            Responsavel = responsavel;
            Sala = sala;
            Data = data;
            Horario = horario;
            HorarioFinal = horarioFinal; // BUG CORRIGIDO: antes, "HorarioFinal = HorarioFinal;" atribuía o
                                         // parâmetro a ele mesmo (mesmo nome da propriedade), então a
                                         // propriedade da classe nunca era realmente preenchida e ficava 00:00:00.
            QtdParticipantes = qtdParticipantes;
            EquipamentosUtilizados = equipamentosUtilizados;
        }
    }
}