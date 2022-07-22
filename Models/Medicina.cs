using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAMHO.Models
{
    public class Medicina
    {
        [Key]
        public int IdMedicina { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(ResourceType = typeof(RESOURCE), Name = "PrecioMedicina")]
        public double PrecioMedicina { get ; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(ResourceType = typeof(RESOURCE), Name = "IdEstadoMedicina")]
        public int IdEstadoMedicina {get; set;}

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(ResourceType = typeof(RESOURCE), Name = "NombreMedicina")]
        public string NombreMedicina { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(ResourceType = typeof(RESOURCE), Name = "CantidadMedicina")]
        public int CantidadMedicina { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(ResourceType = typeof(RESOURCE), Name = "FechaCaducidad")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime FechaCaducidad { get; set; }

        public DateTime FechaRegistro { get; set; }

    }
}
