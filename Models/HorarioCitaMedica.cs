namespace SAMHO.Models
{
    public class HorarioCitaMedica
    {
        int idHorarioCitaMedica { get; set; }
        List<int> DiasSemana { get; set; }
        int HInicio { get; set; }
        int MInicio { get; set; }
        int HFin { get; set; }
        int MFin { get; set; }
    }
}
