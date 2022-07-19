using System.ComponentModel.DataAnnotations;

namespace SAMHO.Models
{
    public class HorarioTrabajo
    {
        [Key]
        public int IdHorarioTrabajo { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "SeleccionaDia"), MaxLength(100)]
        public string? DiasSemana { get; set; }        
        public int HInicio { get; set; }        
        public int MInicio { get; set; }        
        public int HFin { get; set; }        
        public int MFin { get; set; }        

        [RegularExpression(@"^[1-9\D]{1,3}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "SeleccionarEstado")]
        public int IdEstado { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(20)]
        public string NombreHorario { get; set; }

        public static string HoraFormat(int hora, int minuto)
        {
            return string.Concat(hora.ToString().Length < 2 ? string.Concat("0", hora.ToString()) : hora.ToString(),
                ":",
                minuto.ToString().Length < 2 ? string.Concat("0", minuto.ToString()) : minuto.ToString());
        }
    }
}
