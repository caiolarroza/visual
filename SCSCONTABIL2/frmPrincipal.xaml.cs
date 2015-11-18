using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace SCSCONTABIL2
{
    /// <summary>
    /// Interaction logic for frmPrincipal.xaml
    /// </summary>
    public partial class frmPrincipal : Window
    {
        public frmPrincipal()
        {
            InitializeComponent();
            testar_nivel();
        }

        private void btnCadUsu_Click(object sender, RoutedEventArgs e)
        {
            //Instancia do form frmCadUsu
            frmCadUsu CadUsu = new frmCadUsu();
            //mostra o form frmCadUsu e fecha esse
            CadUsu.Show();
            this.Close();
        }

        private void btnCadFor_Click(object sender, RoutedEventArgs e)
        {
            //Instancia do form frmCadFor
            frmCadFor cadfor = new frmCadFor();
            //mostra o form frmCadFor e fecha esse
            cadfor.Show();
            this.Close();
        }

        private void btnCadPro_Click(object sender, RoutedEventArgs e)
        {
            //Instância da classe frmCadPro
            frmCadPro cadpro = new frmCadPro();
            //mostra o form frmCadPro e fecha esse
            cadpro.Show();
            this.Close();
        }

        private void testar_nivel()
        {
            //abrir conexão
            Conexao conexao = new Conexao();
            conexao.abrir();
            //Instancia da classe frmLogin para pegar a informação do nome do usuario
            frmLogin login = new frmLogin();
            //busca tipo do usuario
            MySqlCommand comandos = new MySqlCommand("select UsuTip from usuario where UsuNom = ?usuario", conexao.con);
            comandos.Parameters.Add(new MySqlParameter("?usuario", login.getUsuario()));
            //É executado e lido o comando.
            MySqlDataReader reader = comandos.ExecuteReader();
            String resultado = null;
            //vai ler o resultado do tipo do usuario
            while (reader.Read())
            {
                resultado = reader["UsuTip"].ToString();
            }
            //Se o usuario estiver nivel abaixo de A ele terá limitações
            if (resultado == "C")
            {
                //bloqueia todos os botões de cadastro
                btnCadUsu.IsEnabled = false;
                btnCadPro.IsEnabled = false;
                btnCadFor.IsEnabled = false;
            }
            else if (resultado == "B")
            {
                //bloqueia o cadastro de usuarios
                btnCadUsu.IsEnabled = false;
            }
            conexao.fechar();
        }

        private void btnConFor_Click(object sender, RoutedEventArgs e)
        {
            /*//Instância da classe frmConFor
            frmConFor confor = new frmConFor();
            //mostra o form frmConFor e fecha esse
            confor.Show();
            this.Close();*/
        }

        private void btnConPro_Click(object sender, RoutedEventArgs e)
        {
            //Instância da classe frmConPro
            frmConProd conpro = new frmConProd();
            //mostra o form frmConPro e fecha esse
            conpro.Show();
            this.Close();
        }
    }
}
