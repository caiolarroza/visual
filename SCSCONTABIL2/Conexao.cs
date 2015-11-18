using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace SCSCONTABIL2
{
    class Conexao
    {
        public MySqlConnection con = new MySqlConnection("SERVER=localhost;DATABASE=scs;UID=root;PASSWORD=vertrigo;");

        public void abrir()
        {
            try
            {
                con.Open();
            }
            catch (Exception erro)
            {
                Console.WriteLine(erro.ToString());
                fechar();
            }
        }

        public void fechar()
        {
            try
            {
                con.Close();
            }
            catch (Exception erro)
            {

            }
        }
    }
}
