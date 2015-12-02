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
using Xceed.Wpf.Toolkit;
using MySql.Data.MySqlClient;
using System.Data;


namespace SCSCONTABIL2
{
    /// <summary>
    /// Interaction logic for frmConUsu.xaml
    /// </summary>
    public partial class frmConUsu : Window
    {
        Conexao conexao = new Conexao();
        public frmConUsu()
        {
            InitializeComponent();
            cmbTipo.Items.Add("A");
            cmbTipo.Items.Add("B");
            cmbTipo.Items.Add("C");
            atualizaDataGrid();
            testar_nivel();
        }

        private void limpar()
        {

            txtBusca.Text = "";
            txtUsu.Text = "";
            txtCod.Text = "";
            txtSen.Text = "";
            cmbTipo.Text = "";

        }

        private void btnBus_Click(object sender, RoutedEventArgs e)
        {
            //lista da classe abstrata Usuario que receberá os usuarios
            var lista = new List<Usuario>();
            String busca = txtBusca.Text;
            //buscar os usuarios conforme o nome que o usuario digitar
            MySqlCommand buscaUsu = new MySqlCommand("select * from usuario where UsuNom like '%' ?nome '%'", conexao.con);
            buscaUsu.Parameters.Add(new MySqlParameter("?nome", busca));
            //limpar o datagrid
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            //abrir BD
            conexao.abrir();
            //ler as informações do banco de dados
            using (MySqlDataReader leitor = buscaUsu.ExecuteReader())
            {
                while (leitor.Read())
                {
                    //classe abstrata para dados de produtos
                    Usuario usuario = new Usuario();
                    //info do BD
                    usuario.UsuCod = Convert.ToInt32(leitor["UsuCod"]);
                    usuario.UsuNom = leitor["UsuNom"].ToString();
                    usuario.UsuTip = Convert.ToChar(leitor["UsuTip"]);
                    //adiciona as variaveis a uma lista
                    lista.Add(usuario);
                }
            }
            //adiciona a lista ao dataGrid
            dataGrid.ItemsSource = lista;
            txtBusca.Text = "";
            conexao.fechar();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (txtCod.Text.Equals("") || txtUsu.Text.Equals("") || txtSen.Text.Equals(""))
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Preencha todos os campos com *";
            }
            else
            {
                deletarDados();

            }

        }

        private void btnAlt_Click(object sender, RoutedEventArgs e)
        {
            if (txtCod.Text.Equals("") || txtUsu.Text.Equals("") || txtSen.Text.Equals(""))
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Preencha todos os campos com *";
            }
            else
            {
                atualizarDados();

            }
        }

        private void btnVol_Click(object sender, RoutedEventArgs e)
        {
            //instância da classe frmPrincipal
            frmPrincipal frmpri = new frmPrincipal();
            //Fecha esse form e abre o frmPrincipal
            frmpri.Show();
            this.Close();
        }

        private void deletarDados()
        {
            try
            {
                //confirma se o usuario vai excluir o usuario
                MessageBoxResult op = Xceed.Wpf.Toolkit.MessageBox.Show("Tem certeza que deseja excluir o usuário: " + txtUsu.Text + "?", "Excluir",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (op.ToString().ToUpper() == "YES")
                {
                    MySqlCommand deletar = new MySqlCommand("delete from usuario where UsuCod = ?codigo", conexao.con);
                    deletar.Parameters.Add(new MySqlParameter("?codigo", txtCod.Text));
                    conexao.abrir();
                    deletar.ExecuteNonQuery();
                    conexao.fechar();
                    lblStatus.Foreground = Brushes.Green;
                    lblStatus.Content = "Deletado com sucesso";
                    limpar();
                    atualizaDataGrid();
                    txtBusca.Focus();
                }
            }
            catch (Exception erro)
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = erro.Message;
                Xceed.Wpf.Toolkit.MessageBox.Show(erro.ToString());
            }
        }

        private void atualizaDataGrid()
        {
            //lista que sera adicionada ao datagrid
            var lista = new List<Usuario>();

            //limpar o datagrid
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();

            //abre BD
            conexao.abrir();

            MySqlCommand datagrid = new MySqlCommand("select * from usuario", conexao.con);
            using (MySqlDataReader leitor = datagrid.ExecuteReader())
            {
                while (leitor.Read())
                {
                    //classe abstrata para dados de usuario
                    Usuario usuario = new Usuario();
                    usuario.UsuCod = Convert.ToInt32(leitor["UsuCod"]);
                    usuario.UsuNom = leitor["UsuNom"].ToString();
                    usuario.UsuTip = Convert.ToChar(leitor["UsuTip"]);
                    //adiciona as variaveis a uma lista
                    lista.Add(usuario);

                }
                leitor.Close();

            }
            //adiciona a lista ao datagrid
            dataGrid.ItemsSource = lista;

            conexao.fechar();
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
                btnAlt.IsEnabled = false;
                btnDel.IsEnabled = false;

            }

            conexao.fechar();
        }

        private void atualizarDados()
        {

            try
            {
                conexao.abrir();
                MySqlCommand alterar = new MySqlCommand("update usuario set UsuNom = ?nome, UsuSen = ?senha, UsuTip = ?tipo where UsuCod = ?codigo", conexao.con);
                alterar.Parameters.Add(new MySqlParameter("?nome", txtUsu.Text));
                alterar.Parameters.Add(new MySqlParameter("?senha", txtSen.Text));
                alterar.Parameters.Add(new MySqlParameter("?tipo", cmbTipo.SelectedItem));
                alterar.Parameters.Add(new MySqlParameter("?codigo", txtCod.Text));

                alterar.ExecuteNonQuery();
                conexao.fechar();
                lblStatus.Foreground = Brushes.Green;
                lblStatus.Content = "Alterado com sucesso";
                limpar();
                atualizaDataGrid();
                txtBusca.Focus();
            }
            catch (Exception erro)
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = erro.Message;
                Xceed.Wpf.Toolkit.MessageBox.Show(erro.ToString());
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            {
                //pega a linha selecionada no datagrid
                Usuario dados = (Usuario)(dataGrid.SelectedItem);
                //pega o codigo do usuario
                int codigo = dados.UsuCod;
                //informações do usuario
                MySqlCommand buscaUsu = new MySqlCommand("select * from usuario where UsuCod = ?codigo", conexao.con);
                buscaUsu.Parameters.Add(new MySqlParameter("?codigo", codigo));


                //abrir conexao
                conexao.abrir();

                //informações do fornecedor
                using (MySqlDataReader leitor = buscaUsu.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        txtCod.Text = leitor["UsuCod"].ToString();
                        cmbTipo.SelectedItem = leitor["UsuTip"].ToString();
                        txtUsu.Text = leitor["UsuNom"].ToString();
                        txtSen.Text = leitor["UsuSen"].ToString();
                    }
                    leitor.Close();
                }
                conexao.fechar();
            }
        }

      
    }
}
