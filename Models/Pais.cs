using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;

namespace SAMHO.Models
{
    public class Pais
    {
        [Key]
        public int IdPais { get; set; }
        [Required]
        public string NombrePais { get; set; }
        
        public string CodigoPais { get; set; }

        public static List<Pais> ListaPaises()
        {
            List<Pais> listaPais = new List<Pais>();
            try
            {
                Pais pais = new Pais() { IdPais = -1, NombrePais = "Selecciona Pais" };
                listaPais.Add(pais);                
                
                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PaisesListar", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //stored procedure regresa la lista completa de paises al pasar parametro 0
                        cmd.Parameters.Add("@IdPais", SqlDbType.Int).Value = 0;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        SqlDataReader idr = cmd.ExecuteReader();

                        if (idr.HasRows)
                        {
                            while (idr.Read())
                            {
                                listaPais.Add(new Pais
                                {
                                    IdPais = Convert.ToInt32(idr["IDPAIS"]),
                                    NombrePais = Convert.ToString(idr["NOMBREPAIS"]),
                                    CodigoPais = Convert.ToString(idr["CODIGOPAIS"])
                                });
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                //TODO Error code

            }
            return listaPais;
        }

        private static object GetConnection()
        {
            throw new NotImplementedException();
        }
    }
}
