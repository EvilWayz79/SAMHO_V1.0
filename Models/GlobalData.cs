namespace SAMHO.Models
{
    public static class GlobalData
    {
        public static Dictionary<string, string> APLICACION = new Dictionary<string, string>();
        public static List<string> ROLELIST= new List<string>();
        public static string ESPECIALIDAD = "ESPECIALIDAD";

        /// <summary>
        /// EXITO=0, FALLO =1, EXCEPCION =2, NOUNICO=3
        /// </summary>
        public enum TRANSACTRESULTS
        {
            EXITO, 
            FALLO, 
            EXCEPCION, 
            NOUNICO 
        };

        public enum DIASDELASEMANA
        {   
            LUNES,MARTES,MIERCOLES,JUEVES,VIERNES,SABADO,DOMINGO
        }

        public enum SEXO
        { 
            SELECCIONAR,
            MASCULINO,
            FEMENINO        
        }

        public enum TIPOIDENTIFICACION
        { 
            SELECCIONAR,
            CEDULA,
            PASAPORTE
        }



    }
}
