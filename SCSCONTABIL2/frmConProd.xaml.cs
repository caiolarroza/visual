using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Media;
// Primeiro de tudo, vou te apresentar um facilitador de coisas... chamado ReSharper...

namespace SCSCONTABIL2
{
    /// <summary>
    /// Interaction logic for frmConProd.xaml
    /// </summary>
    public partial class frmConProd : Window
    {
        Conexao conexao = new Conexao();
        public frmConProd()
        {
            InitializeComponent();
            atualizaDataGrid();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtCod_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtIes_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtImu_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAlt_Click(object sender, RoutedEventArgs e)
        {
            if (txtRazao.Text.Equals("") || txtNomePro.Text.Equals("") || txtPreco.Text.Equals("") ||
                txtQtd.Text.Equals(""))
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Preencha todos os campos com *";
            }
            else
            {
                atualizarDados();
            }
        }

        private void txtPreco_TextChanged(object sender, TextChangedEventArgs e)
        {
            //chama o metodo que formatara o valor
            Moeda(ref txtPreco);
        }

        private void txtPreco_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //bloqueia a digitação de valores diferentes de numeros, backspace, e ponto no textbox
            if (!char.IsDigit((char)e.Key) && (char)e.Key != (char)8 && (char)e.Key != (char)44)
            {
                e.Handled = true;
            }

            //limita somente a 1 ponto (para separar os centavos) no textbox
            if ((char)e.Key == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void dataGrid_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //variavel de codigo do fornecedor
            int forne = 0;
            //pega a linha selecionada no datagrid
            int linha = dataGrid.SelectedIndex;
            //pega o codigo do produto
            int codigo = int.Parse(dataGrid.SelectedCells[linha].Column.GetCellContent("Codigo").ToString());
            //informações do produto
            MySqlCommand buscaProd = new MySqlCommand("select * from produto where ProCod = ?codigo", conexao.con);
            buscaProd.Parameters.Add(new MySqlParameter("?codigo", codigo));


            //abrir conexao
            conexao.abrir();
            //ler as informações do produto
            using (MySqlDataReader leitor = buscaProd.ExecuteReader())
            {
                while (leitor.Read())
                {
                    txtNomePro.Text = leitor["ProNom"].ToString();
                    txtData.Text = leitor["ProDat"].ToString();
                    txtPreco.Text = leitor["ProPco"].ToString();
                    txtQtd.Text = leitor["ProQtd"].ToString();
                    forne = int.Parse(leitor["ProFor"].ToString());
                    txtCod.Text = codigo.ToString();
                }

            }

            //informações do fornecedor
            MySqlCommand buscaFor = new MySqlCommand("select * from fornecedor where ForCod = ?codigo", conexao.con);
            buscaFor.Parameters.Add(new MySqlParameter("?codigo", forne));

            using (MySqlDataReader leitor = buscaFor.ExecuteReader())
            {
                while (leitor.Read())
                {
                    txtRazao.Text = leitor["ForRaz"].ToString();
                    txtNome.Text = leitor["ForNom"].ToString();
                    txtImu.Text = leitor["ForImu"].ToString();
                    txtIes.Text = leitor["ForIes"].ToString();
                    txtCnpj.Text = leitor["ForCnp"].ToString();
                }
            }
            conexao.fechar();
        }

        private void btnVol_Click(object sender, EventArgs e)
        {
            //instância da classe frmPrincipal
            frmPrincipal frmpri = new frmPrincipal();
            //Fecha esse form e abre o frmPrincipal
            frmpri.Show();
            this.Close();
        }

        private void btnBus_Click(object sender, RoutedEventArgs e)
        {
            String busca = txtBusca.Text;
            //buscar os produtos conforme o nome que o usuario digitar
            MySqlCommand buscaProd = new MySqlCommand("select ProCod, ProNom from produto where ProNom like '%' ?nome '%'", conexao.con);
            buscaProd.Parameters.Add(new MySqlParameter("?nome", busca));
            //limpa o datagrid
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            conexao.abrir();
            using (MySqlDataReader leitor = buscaProd.ExecuteReader())
            {
                while (leitor.Read())
                {
                    //adiciona dados ao dataGrid
                    dataGrid.Items.Add(leitor["ProCod"].ToString());
                    dataGrid.Items.Add(leitor["ProNom"].ToString());
                }
            }
            txtBusca.Text = "";
            conexao.fechar();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (txtRazao.Text.Equals("") || txtNomePro.Text.Equals("") || txtPreco.Text.Equals("") ||
                txtQtd.Text.Equals(""))
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Preencha todos os campos com *";
            }
            else
            {
                deletarDados();
            }
        }

        private void txtQtd_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //só permite numeros e o backspace no textbox
            if (!char.IsDigit((char)e.Key) && (char)e.Key != (char)8)
            {
                e.Handled = true;
            }
        }

        private void limpar()
        {

            txtRazao.Text = "";
            txtBusca.Text = "";
            txtNome.Text = "";
            txtIes.Text = "";
            txtImu.Text = "";
            txtCod.Text = "";
            txtNomePro.Text = "";
            txtPreco.Text = "";
            txtData.Text = "";
            txtQtd.Text = "";
        }

        private void Moeda(ref TextBox txt)
        {
            //faz uma mascara para digitar o preco
            String n = "";
            Double v = 0;
            try
            {
                n = txt.Text.Replace(",", "").Replace(".", "");
                if (n.Equals(""))
                {
                    n = "";
                }
                n = n.PadLeft(3, '0');
                if ((n.Length > 3) && (n.Substring(0, 1) == "0"))
                {
                    n = n.Substring(1, n.Length - 1);
                }
                v = Convert.ToDouble(n) / 100;
                txt.Text = string.Format("{0:N}", v);
                txt.SelectionStart = txt.Text.Length;
            }
            catch (Exception e)
            {

            }
        }


        private void deletarDados()
        {
            try
            {
                //confirma se o usuario vai excluir o produto
                MessageBoxResult op = Xceed.Wpf.Toolkit.MessageBox.Show("Tem certeza que deseja excluir o produto: " + txtNomePro.Text + "?", "Excluir",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (op.ToString().ToUpper() == "YES")
                {
                    MySqlCommand deletar = new MySqlCommand("delete from produto where ProCod = ?codigo", conexao.con);
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
            }
        }


        private void atualizarDados()
        {
            try
            {

                String preco = txtPreco.Text.Replace(".", "").Replace(",", ".");
                MySqlCommand alterar = new MySqlCommand("update produto set ProNom = ?nome, ProPco = ?preco, ProQtd = ?qtd " +
                    "where ProCod = ?codigo", conexao.con);
                alterar.Parameters.Add(new MySqlParameter("?nome", txtNomePro.Text));
                alterar.Parameters.Add(new MySqlParameter("?preco", preco));
                alterar.Parameters.Add(new MySqlParameter("?qtd", txtQtd.Text));
                alterar.Parameters.Add(new MySqlParameter("?codigo", txtCod.Text));
                conexao.abrir();
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

            }
        }


        private void atualizaDataGrid()
        {
            //carrega o datagrid com todos os produtos   
            conexao.abrir();
            //limpa o datagrid
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            MySqlCommand datagrid = new MySqlCommand("select ProCod, ProNom from produto", conexao.con);
            using (MySqlDataReader leitor = datagrid.ExecuteReader())
            {
                while (leitor.Read())
                {
                    //adiciona os campos do BD ao datagrid
                    dataGrid.Items.Add(leitor["ProCod"].ToString());
                    dataGrid.Items.Add(leitor["ProNom"].ToString());
                }
            }
            conexao.fechar();
        }

        private void lblStatus_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }

        private void lblStatus_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //centraliza o label
            lblStatus.Width = (this.RenderSize.Width- lblStatus.RenderSize.Width) / 5;
        }

        private void btnVol_Click_1(object sender, RoutedEventArgs e)
        {
            //Instancia do form frmPrincipal
            frmPrincipal principal = new frmPrincipal();
            //mostra o form frmPrincipal e fecha esse
            principal.Show();
            this.Close();
        }
    }
    
}
