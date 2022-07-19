namespace SAMHO.Models
{
    public class Sexo
    {
        public int IdSexo { get; set; }
        public string Nsexo { get; set; }

        public static List<Sexo> ListaGeneros()
        { 
            List<Sexo> lista = new List<Sexo>();

            Sexo masculino = new Sexo { IdSexo = 1, Nsexo = "Masculino" };
            Sexo femenino = new Sexo { IdSexo = 2, Nsexo = "Femenino" };

            lista.Add(masculino);
            lista.Add(femenino);

            return lista;

        }
    }
}
