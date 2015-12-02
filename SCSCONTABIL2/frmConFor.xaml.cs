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
    /// Interaction logic for frmConFor.xaml
    /// </summary>
    public partial class frmConFor : Window
    {
        Conexao conexao = new Conexao();
        public frmConFor()
        {
            InitializeComponent();
            atualizaDataGrid();
            testar_nivel();
        }

        private void limpar()
        {

            txtRazao.Text = "";
            txtBusca.Text = "";
            txtNome.Text = "";
            txtIes.Text = "";
            txtImu.Text = "";
            txtCod.Text = "";
            txtCnpj.Text = "";
            txtDdd.Text = "";
            txtNumTel.Text = "";
            txtCep.Text = "";
            txtNumEnd.Text = "";
            txtCom.Text = "";
            txtEnd.Text = "";
            txtEst.Text = "";
            txtMun.Text = "";
            txtBai.Text = "";
          
        }

        private void btnBus_Click(object sender, RoutedEventArgs e)
        {
            //lista da classe abstrata Fornecedor que receberá os produtos
            var lista = new List<Fornecedor>();
            String busca = txtBusca.Text;
            //buscar os fornecedores conforme o cnpj que o usuario digitar
            MySqlCommand buscaForne = new MySqlCommand("select * from fornecedor where ForCnp like '%' ?nome '%'", conexao.con);
            buscaForne.Parameters.Add(new MySqlParameter("?nome", busca));
            //limpar o datagrid
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            //abrir BD
            conexao.abrir();
            //ler as informações do banco de dados
            using (MySqlDataReader leitor = buscaForne.ExecuteReader())
            {
                while (leitor.Read())
                {
                    //classe abstrata para dados de produtos
                    Fornecedor fornecedor = new Fornecedor();
                    //info do BD
                    fornecedor.ForCod = Convert.ToInt32(leitor["ForCod"]);
                    fornecedor.ForNom = leitor["ForNom"].ToString();
                    fornecedor.ForRaz = leitor["ForRaz"].ToString();
                    fornecedor.ForCnp = leitor["ForCnp"].ToString();
                    //adiciona as variaveis a uma lista
                    lista.Add(fornecedor);
                }
            }
            //adiciona a lista ao dataGrid
            dataGrid.ItemsSource = lista;
            txtBusca.Text = "";
            conexao.fechar();
        }

        private void btnAlt_Click(object sender, RoutedEventArgs e)
        {
            if (txtRazao.Text.Equals("") || txtNome.Text.Equals("") || txtCnpj.Text.Equals("") || txtCod.Text.Equals(""))
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

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (txtRazao.Text.Equals("") || txtNome.Text.Equals("") || txtCnpj.Text.Equals("") || txtCod.Text.Equals(""))
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Preencha todos os campos com *";
            }
            else
            {
                deletarDados();

            }
        }

        private void deletarDados()
        {
            try
            {
                //confirma se o usuario vai excluir o fornecedor
                MessageBoxResult op = Xceed.Wpf.Toolkit.MessageBox.Show("Tem certeza que deseja excluir o fornecedor: " + txtNome.Text + "?", "Excluir",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (op.ToString().ToUpper() == "YES")
                {
                    MySqlCommand deletar = new MySqlCommand("delete from fornecedor where ForCod = ?codigo", conexao.con);
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
            var lista = new List<Fornecedor>();

            //limpar o datagrid
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();

            //abre BD
            conexao.abrir();

            MySqlCommand datagrid = new MySqlCommand("select * from fornecedor", conexao.con);
            using (MySqlDataReader leitor = datagrid.ExecuteReader())
            {
                while (leitor.Read())
                {
                    //classe abstrata para dados de produtos
                    Fornecedor fornecedor = new Fornecedor();
                    fornecedor.ForCod = Convert.ToInt32(leitor["ForCod"]);
                    fornecedor.ForRaz = leitor["ForRaz"].ToString();
                    fornecedor.ForNom = leitor["ForNom"].ToString();
                    fornecedor.ForCnp = leitor["ForCnp"].ToString();
                    //adiciona as variaveis a uma lista
                    lista.Add(fornecedor);

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
                MySqlCommand alterar = new MySqlCommand("update fornecedor set ForRaz = ?razao, ForNom = ?nome,ForCnp = ?cnpj, ForImu = ?imu, ForIes = ?ies where ForCod = ?codigo", conexao.con);
                alterar.Parameters.Add(new MySqlParameter("?razao", txtRazao.Text));
                alterar.Parameters.Add(new MySqlParameter("?nome", txtNome.Text));
                alterar.Parameters.Add(new MySqlParameter("?cnpj", txtCnpj.semFormato()));
                alterar.Parameters.Add(new MySqlParameter("?imu", txtImu.semFormato()));
                alterar.Parameters.Add(new MySqlParameter("?ies", txtIes.semFormato()));
                alterar.Parameters.Add(new MySqlParameter("?codigo", txtCod.Text));

                MySqlCommand alterarEnd = new MySqlCommand("update endereco set EndCep = ?cep, EndEnd = ?rua, EndNum = ?num, EndCom = ?compl, EndBai = ?bairro, EndMun = ?mun, EndEst = ?est" +
                     " where EndCod = ?cod", conexao.con);
                alterarEnd.Parameters.Add(new MySqlParameter("?cep", txtCep.semFormato()));
                alterarEnd.Parameters.Add(new MySqlParameter("?rua", txtEnd.Text));
                alterarEnd.Parameters.Add(new MySqlParameter("?num", txtNumEnd.Text));
                alterarEnd.Parameters.Add(new MySqlParameter("?compl", txtCom.Text));
                alterarEnd.Parameters.Add(new MySqlParameter("?bairro", txtBai.Text));
                alterarEnd.Parameters.Add(new MySqlParameter("?mun", txtMun.Text));
                alterarEnd.Parameters.Add(new MySqlParameter("?est", txtEst.Text));
                alterarEnd.Parameters.Add(new MySqlParameter("?cod", txtCod.Text));

                MySqlCommand alterarTel = new MySqlCommand("update telefone set TelDdd = ?ddd, TelNum = ?num where TelCod = ?cod2", conexao.con);
                alterarTel.Parameters.Add(new MySqlParameter("?ddd", txtDdd.semFormato()));
                alterarTel.Parameters.Add(new MySqlParameter("?num", txtNumTel.semFormato()));
                alterarTel.Parameters.Add(new MySqlParameter("?cod2", txtCod.Text));

                
                alterar.ExecuteNonQuery();
                alterarEnd.ExecuteNonQuery();
                alterarTel.ExecuteNonQuery();
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
            //variavel de codigo do fornecedor
            int forne = 0;
            int end = 0;
            int tel = 0;
            //pega a linha selecionada no datagrid
            Fornecedor dados = (Fornecedor)(dataGrid.SelectedItem);
            //pega o codigo do fornecedor
            int codigo = dados.ForCod;
            //informações do fornecedor
            MySqlCommand buscaFor = new MySqlCommand("select * from fornecedor where ForCod = ?codigo", conexao.con);
            buscaFor.Parameters.Add(new MySqlParameter("?codigo", codigo));


            //abrir conexao
            conexao.abrir();

            //informações do fornecedor
            using (MySqlDataReader leitor = buscaFor.ExecuteReader())
            {
                while (leitor.Read())
                {
                    txtRazao.Text = leitor["ForRaz"].ToString();
                    txtNome.Text = leitor["ForNom"].ToString();
                    txtImu.Text = leitor["ForImu"].ToString();
                    txtIes.Text = leitor["ForIes"].ToString();
                    txtCnpj.Text = leitor["ForCnp"].ToString();
                    end = int.Parse(leitor["ForEnd"].ToString());
                    tel = int.Parse(leitor["ForTel"].ToString());
                }
                leitor.Close();
            }

            //informações de telefone

            MySqlCommand buscaTel = new MySqlCommand("select * from telefone where TelCod = ?codigo", conexao.con);
            buscaTel.Parameters.Add(new MySqlParameter("?codigo", tel));

            using (MySqlDataReader leitor = buscaTel.ExecuteReader())
            {
                while (leitor.Read())
                {
                    txtDdd.Text = leitor["TelDdd"].ToString();
                    txtNumTel.Text = leitor["TelNum"].ToString();
                }
                leitor.Close();
            }

            //informações de endereço

            MySqlCommand buscaEnde = new MySqlCommand("select * from endereco where EndCod = ?codigo", conexao.con);
            buscaEnde.Parameters.Add(new MySqlParameter("?codigo", end));

            using (MySqlDataReader leitor = buscaEnde.ExecuteReader())
            {
                while (leitor.Read())
                {
                    txtCod.Text = leitor["EndCod"].ToString();
                    txtCep.Text = leitor["EndCep"].ToString();
                    txtEnd.Text = leitor["EndEnd"].ToString();
                    txtNumEnd.Text = leitor["EndNum"].ToString();
                    txtCom.Text = leitor["EndCom"].ToString();
                    txtBai.Text = leitor["EndBai"].ToString();
                    txtMun.Text = leitor["EndMun"].ToString();
                    txtEst.Text = leitor["EndEst"].ToString();
                }
                leitor.Close();
            }
            conexao.fechar();
        }

        
    }
    }

