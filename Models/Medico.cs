using System;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{

    public class Medico {

    [Key]
    public int IdMedico { get; set; }
    public required string Nombre { get; set; } 
    public required string Apellido { get; set; } 
    public required string Direccion { get; set; } 
    public required string Telefono { get; set; } 
    public required string Email { get; set; }

    public DateTime HorarioAtencionDesde { get; set; } 
    public DateTime HorarioAtencionHasta { get; set; }
    public List<MedicoEspecialidad>? MedicoEspecialidad { get; set; } 
    public ICollection<Turno> Turno { get; set; } = [];

    }

}

