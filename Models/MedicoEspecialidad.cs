namespace Turnos.Models {

    public class MedicoEspecialidad {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public required Medico Medico { get; set; }
        public required Especialidad Especialidad { get; set; }
    }

}

