using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            // Adicionar um novo contato à lista
            Contato novoContato = new Contato
            {
                Nome = txtNome.Text,
                Telefone = txtTelefone.Text,
                Email = txtEmail.Text
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
                        writer.WriteLine($"{contato.Nome},{contato.Telefone},{contato.Email}");
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
            // Abrir a lista de contatos de um ficheiro
            try
            {
                using (StreamReader reader = new StreamReader("contatos.txt"))
                {
                    string linha;
                    while ((linha = reader.ReadLine()) != null)
                    {
                        string[] dados = linha.Split(',');
                        Contato contato = new Contato
                        {
                            Nome = dados[0],
                            Telefone = dados[1],
                            Email = dados[2]
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

            public override string ToString()
            {
                return $"{Nome} - {Telefone} - {Email}";
            }
        }
    }
}
