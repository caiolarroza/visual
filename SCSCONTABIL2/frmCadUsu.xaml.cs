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
    /// Interaction logic for frmCadUsu.xaml
    /// </summary>
    public partial class frmCadUsu : Window
    {
        //codigo de cadastro do novo usuario
        public int codUsuarios = 1;
        //Informações do cadastro
        private static String usuario, senha, tipoUsu;
        //Instancia da classe Conexao
        Conexao conexao = new Conexao();

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            //Instancia do form frmPrincipal
            frmPrincipal principal = new frmPrincipal();
            //mostra o form frmPrincipal e fecha esse
            principal.Show();
            this.Close();
        }

        public frmCadUsu()
        {
            InitializeComponent();
        }

        private void lblStatus_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //centraliza o label conforme o form
            //lblStatus.Left = (this.ClientSize.Width - lblStatus.Size.Width) / 2;
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            //receber dados dos textBox
            usuario = txtUsu.Text;
            senha = txtSen.Text;
            //converter a opção do comboBox para string
            tipoUsu = Convert.ToString(cmbTipo.SelectedItem);
            if (tipoUsu.Equals("") || usuario.Equals("") || senha.Equals(""))
            {   //reinicia os valores                             
                txtUsu.Text = "";
                txtSen.Text = "";
                cmbTipo.SelectedIndex = -1;
                lblStatus.Foreground = Brushes.Red;
                lblStatus.Content = "Preencha todos os campos";
            }
            else
            {
                //metodo verifica se o nome ja está em uso
                verificar_nome();

            }
        }



        private void verificar_nome()
        {
            try
            {
                //abrir conexao BD
                conexao.abrir();
                //Variavel com os comandos de consulta do nome
                MySqlCommand consultaNome = new MySqlCommand("select * from usuario where UsuNom = ?usuario", conexao.con);
                //adiciona parametros ao comando String, evita problemas com SQL Inject
                consultaNome.Parameters.Add(new MySqlParameter("?usuario", usuario));
                //Variavel que executará as leituras
                using (MySqlDataReader readerNome = consultaNome.ExecuteReader())
                {
                    //verificar se o nome ja está em uso
                    if (readerNome.HasRows)
                    {   //se estiver em uso ele avisa o usuário
                        lblStatus.Foreground = Brushes.Red;
                        lblStatus.Content = "Usuário já cadastrado";
                        txtUsu.Text = "";
                        txtSen.Text = "";
                        cmbTipo.SelectedIndex = -1;
                        txtUsu.Focus();
                        conexao.fechar();
                    }
                    else
                    {
                        //fechar reader
                        readerNome.Close();
                        //fechar conexao
                        conexao.fechar();
                        verificar_codigo();
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro: " + erro.Message);
                conexao.fechar();
            }
        }

        private void verificar_codigo()
        {
            try
            {
                //abrir conexao com BD
                conexao.abrir();
                Boolean lugar = false;

                while (lugar == false)
                {
                    //Variavel com os comandos de consulta do codigo
                    MySqlCommand consultaCod = new MySqlCommand("select * from usuario where UsuCod = ?codigo ", conexao.con);
                    //adiciona parametros ao comando String, evita problemas com SQL Inject
                    consultaCod.Parameters.Add(new MySqlParameter("?codigo", codUsuarios));
                    //É executado e lido o comando.
                    using (MySqlDataReader readerCod = consultaCod.ExecuteReader())
                    {
                        //verificar o primeiro lugar vago para cadastrar usuario
                        //verificar se o codigo ja está em uso
                        if (readerCod.HasRows)
                        {   //se estiver em uso procura pelo proximo
                            codUsuarios++;
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
                MessageBox.Show("Erro: " + erro.ToString());
                conexao.fechar();
            }
        }

        private void cadastrar()
        {
            try
            {
                conexao.abrir();
                //Variavel com os comandos de consulta do codigo
                MySqlCommand cadastroUsu = new MySqlCommand("insert into usuario values( ?codigo, ?usuario, ?senha , ?tipo)", conexao.con);
                //adiciona parametros ao comando, evita problemas com SQL Inject
                cadastroUsu.Parameters.Add(new MySqlParameter("?codigo", codUsuarios));
                cadastroUsu.Parameters.Add(new MySqlParameter("?usuario", usuario));
                cadastroUsu.Parameters.Add(new MySqlParameter("?senha", senha));
                cadastroUsu.Parameters.Add(new MySqlParameter("?tipo", tipoUsu));
                //Aqui é executado
                cadastroUsu.ExecuteNonQuery();
                //muda a cor do label para verde e avisa o usuario
                lblStatus.Foreground = Brushes.Green;
                lblStatus.Content = "Usuario cadastrado com sucesso";
                //limpa os campos e foca no txt de Usuario
                txtSen.Text = "";
                txtUsu.Text = "";
                cmbTipo.SelectedIndex = -1;
                txtUsu.Focus();
                conexao.fechar();

            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro: " + erro.ToString());
            }
        }
    }
}
