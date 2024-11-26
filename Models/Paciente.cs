using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{

    public class Paciente
    {

        [Key]
        public int IdPaciente { get; set;}
        public required string Nombre { get; set;}
        public required string Apellido { get; set;}
        public required string Direccion {get; set;}
        public required string Telefono {get; set;}
        public required string Email {get; set;}
        public ICollection<Turno> Turno { get; set; } = [];

    }

}

