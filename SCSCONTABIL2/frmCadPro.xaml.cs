using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using MySql.Data.MySqlClient;

namespace SCSCONTABIL2
{
    /// <summary>
    /// Interaction logic for frmCadPro.xaml
    /// </summary>
    public partial class frmCadPro : Window
    {
        //instancia da classe conexao
        Conexao conexao = new Conexao();
        //codigo do fornecedor no bd
        static private int fornecedor;
        //codigo que será cadastrado o produto
        static private int cod = 1;
        //preco do produto
        
        public frmCadPro()
        {
            InitializeComponent();
            txtPreco.Text = "0,00";
            txtNomePro.MaxLength = 39;
            txtPreco.MaxLength = 20;
            txtQtd.MaxLength = 9;
            txtCnpj.Focus();
        }

        private void btnCad_Click(object sender, RoutedEventArgs e)
        {
            //verifica se todos os campos foram preenchidos
            if (txtCnpj.semFormato().Equals("") || txtRazao.Text.Equals("") ||
                txtNome.Text.Equals("") || txtIes.semFormato().Equals("") ||
                txtImu.semFormato().Equals("") || txtNomePro.Text.Equals("") ||
                txtPreco.Text.Equals(""))
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Preencha todos os campos com *";
            }
            else
            {//dps de validados os dados vai procurar um codigo disponivel para cadastrar
                consultar_codigo();
            }
        }

        private void lblStatus_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //centraliza o label
            //lblStatus.Left = (this.ClientSize.Width - lblStatus.Size.Width) / 2;
        }

        private void btnVol_Click(object sender, RoutedEventArgs e)
        {
            //instância da classe frmPrincipal
            frmPrincipal frmpri = new frmPrincipal();
            //Fecha esse form e abre o frmPrincipal
            frmpri.Show();
            this.Close();
        }

        private void btnFor_Click(object sender, RoutedEventArgs e)
        {
            //valida o campo do cnpj e manda para pesquisa
            if (txtCnpj.semFormato().Equals(""))
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Digite o CNPJ";
            }
            else
            {
                busca_for();
            }
        }

        private void txtPreco_TextChanged(object sender, TextChangedEventArgs e)
        {
            //chama o metodo que formatara o valor
            Moeda(ref txtPreco);
        }

        private void txtPreco_KeyDown(object sender, KeyEventArgs e)
        {//bloqueia a digitação de valores diferentes de numeros no textbox
            e.Handled = !recebeNumero(e.Key);
        }

        

        private void txtQtd_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !recebeNumero(e.Key);
        }


        private bool recebeNumero(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
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

        private void busca_for()
        {//pesquisa o fornecedor pelo CNPJ
            try
            {

                //le o comando e adiciona os parametros
                MySqlCommand buscaFor = new MySqlCommand("select * from fornecedor where ForCnp = ?cnpj", conexao.con);
                buscaFor.Parameters.Add(new MySqlParameter("?cnpj", txtCnpj.semFormato()));
                //abre a conexao com o bd
                conexao.abrir();
                using (MySqlDataReader leitor = buscaFor.ExecuteReader())
                {
                    //se a pesquisa retornar dados, eles serão apresentados ao usuario
                    if (leitor.HasRows)
                    {
                        while (leitor.Read())
                        {
                            txtRazao.Text = leitor["ForRaz"].ToString();
                            txtNome.Text = leitor["ForNom"].ToString();
                            txtIes.Text = leitor["ForIes"].ToString();
                            txtImu.Text = leitor["ForImu"].ToString();
                            fornecedor = Convert.ToInt32(leitor["ForCod"]);
                            txtNomePro.Focus();
                            lblStatus.Content = "";
                        }
                        conexao.fechar();
                    }
                    else
                    {
                        //se não for retornado nenhum dado a msg abaixo será exibida
                        lblStatus.Foreground = Brushes.Red;
                        lblStatus.Content = "CNPJ não encontrado";
                        txtCnpj.Text = "";
                        leitor.Close();
                        conexao.fechar();
                        txtCnpj.Focus();
                    }
                }

            }
            catch (Exception erro)
            {
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = erro.Message;
                conexao.fechar();
            }
        }

        private void consultar_codigo()
        {
            try
            {
                Boolean lugar = false;

                while (lugar == false)
                {

                    //Variavel com os comandos de consulta do codigo
                    MySqlCommand consultaCod = new MySqlCommand("select * from produto where ProCod = ?codigo", conexao.con);
                    //adiciona parametros ao comando String, evita problemas com SQL Inject
                    consultaCod.Parameters.Add(new MySqlParameter("?codigo", cod));
                    //abrir conexao com BD
                    conexao.abrir();
                    using (MySqlDataReader readerCod = consultaCod.ExecuteReader())
                    {
                        //verificar o primeiro lugar vago para cadastrar fornecedor
                        //verificar se o codigo ja está em uso
                        if (readerCod.HasRows)
                        {   //se estiver em uso procura pelo proximo
                            cod++;
                            conexao.fechar();
                        }
                        else
                        {
                            //fechar reader
                            readerCod.Close();
                            //fechar conexao
                            conexao.fechar();
                            lugar = true;
                            cadastrar();
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                //fechar conexao
                conexao.fechar();
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = erro.Message;
            }
        }

        private void cadastrar()
        {//ira cadastrar o produto
            String preco = txtPreco.Text.Replace(".", "").Replace(",", ".");
            DateTime data = DateTime.Now;
            try
            {   //abre a conexao com o bd
                conexao.abrir();
                //comando do bd
                MySqlCommand cadastroPro = new MySqlCommand("insert into produto values (?codigo, ?nome, ?fornecedor, ?preco, ?data, ?qtd)", conexao.con);
                cadastroPro.Parameters.Add(new MySqlParameter("?codigo", cod));
                cadastroPro.Parameters.Add(new MySqlParameter("?nome", txtNomePro.Text));
                cadastroPro.Parameters.Add(new MySqlParameter("?fornecedor", fornecedor));
                cadastroPro.Parameters.Add(new MySqlParameter("?preco", preco));
                cadastroPro.Parameters.Add(new MySqlParameter("?data", data));
                cadastroPro.Parameters.Add(new MySqlParameter("?qtd", txtQtd.Text));
                //executa o comando        
                cadastroPro.ExecuteNonQuery();
                conexao.fechar();
                lblStatus.Foreground = Brushes.Green;
                lblStatus.Content = "Produto cadastrado com sucesso!";
                limpar();
                txtCnpj.Focus();
            }
            catch (MySqlException erro)
            {
                //caso de erro na quantidade de digitos do preço
                lblStatus.Foreground = Brushes.Red;
                Xceed.Wpf.Toolkit.MessageBox.Show(preco);
                lblStatus.Content = erro.Message;
                conexao.fechar();
            }
            catch (Exception erro)
            {
                //caso de erro no cadastro
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = erro.Message;
                conexao.fechar();
            }
        }
        private void limpar()
        {
            //limpa o form
            txtCnpj.Text = "";
            txtRazao.Text = "";
            txtNome.Text = "";
            txtIes.Text = "";
            txtImu.Text = "";
            txtNomePro.Text = "";
            txtPreco.Text = "";
            txtQtd.Text = "";
        }
    }
}
