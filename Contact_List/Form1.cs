using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contact_List
{
    public partial class Form1 : Form
    {
        private List<Contato> contatos = new List<Contato>();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdicionar_Click(object sender, EventArgs e) 
        {
            /// Verificar se os campos obrigatórios (nome, telefone e email) estão preenchidos
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtTelefone.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios (Nome, Telefone e Email).", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar se o telefone possui exatamente 9 dígitos
            if (!string.IsNullOrEmpty(txtTelefone.Text) && !Regex.IsMatch(txtTelefone.Text, @"^\d{9}$"))
            {
                MessageBox.Show("Por favor, insira um número de telefone com 9 dígitos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar se o email possui uma estrutura válida
            if (!string.IsNullOrEmpty(txtEmail.Text) && !Regex.IsMatch(txtEmail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Por favor, insira um endereço de email válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Adicionar um novo contato à lista
            Contato novoContato = new Contato
            {
                Nome = txtNome.Text,
                Telefone = txtTelefone.Text,
                Email = txtEmail.Text,
                DDD = string.IsNullOrEmpty(txtDDD.Text) ? 0 : int.Parse(txtDDD.Text),
                Pronome = txtPronome.Text,
                Morada = txtMorada.Text,
                
            };
            contatos.Add(novoContato);

            // Atualizar a ListBox com o novo contato
            listBoxContatos.Items.Add(novoContato);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Salvar a lista de contatos em um arquivo
            try
            {
                using (StreamWriter writer = new StreamWriter("contatos.txt"))
                {
                    foreach (Contato contato in contatos)
                    {
                        // Formatando a linha para incluir todos os detalhes do contato
                        string linha = $"{contato.Nome},{contato.Telefone},{contato.Email},{contato.DDD},{contato.Pronome},{contato.Morada}";
                        writer.WriteLine(linha);
                    }
                }
                MessageBox.Show("Contatos salvos com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar contatos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAbrirLista_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader("contatos.txt"))
                {
                    string linha;
                    while ((linha = reader.ReadLine()) != null)
                    {
                        string[] dados = linha.Split(',');

                        // Verificar se a linha possui todos os campos necessários
                        if (dados.Length < 3)
                        {
                            MessageBox.Show("Erro ao carregar contatos: formato de linha inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        Contato contato = new Contato
                        {
                            Nome = dados[0],
                            Telefone = dados[1],
                            Email = dados[2],
                            DDD = dados.Length > 3 ? (int.TryParse(dados[3], out int ddd) ? ddd : 0) : 0,
                            Pronome = dados.Length > 4 ? dados[4] : "",
                            Morada = dados.Length > 5 ? dados[5] : "",
                            
                        };

                        contatos.Add(contato);
                        listBoxContatos.Items.Add(contato);
                    }
                }
                MessageBox.Show("Contatos carregados com sucesso!");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Arquivo de contatos não encontrado. A lista de contatos está vazia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar contatos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public class Contato
        {
            public string Nome { get; set; }
            public string Telefone { get; set; }
            public string Email { get; set; }
            public int DDD { get; set; }
            public string Pronome { get; set; }
            public string Morada { get; set; }

            public override string ToString()
            {
                return $"{Nome} - {Telefone} - {Email} - {DDD} - {Pronome} - {Morada} - ";
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (listBoxContatos.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um contato para editar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Contato contatoSelecionado = (Contato)listBoxContatos.SelectedItem;
            // Exibir os detalhes do contato selecionado em controles de entrada (por exemplo, TextBox)
            txtNome.Text = contatoSelecionado.Nome;
            txtTelefone.Text = contatoSelecionado.Telefone;
            txtEmail.Text = contatoSelecionado.Email;
            txtDDD.Text = contatoSelecionado.DDD.ToString();
            txtPronome.Text = contatoSelecionado.Pronome;
            txtMorada.Text = contatoSelecionado.Morada;
            

            // Remover o contato da lista
            contatos.Remove(contatoSelecionado);
            listBoxContatos.Items.Remove(contatoSelecionado);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (listBoxContatos.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um contato para excluir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Contato contatoSelecionado = (Contato)listBoxContatos.SelectedItem;
            // Exibir uma mensagem de confirmação antes de excluir o contato
            DialogResult resultado = MessageBox.Show($"Tem certeza que deseja excluir o contato {contatoSelecionado.Nome}?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                contatos.Remove(contatoSelecionado);
                listBoxContatos.Items.Remove(contatoSelecionado);
                MessageBox.Show("Contato excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLicenca_Click(object sender, EventArgs e)
        {
            // Exibir mensagem da licença
            MessageBox.Show("Criador: Gabriel Carvalho (11ºE)\nVersão: 1.0\nCopyright © 2024\nTodos os direitos reservados.",
                            "Licença",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}
