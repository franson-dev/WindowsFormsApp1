using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SerialPort srpArduino;
        public Form1()
        {
            InitializeComponent();
            srpArduino = new SerialPort();
            srpArduino.DataReceived += SrpArduino_DataReceived;
        }

        private void SrpArduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var dados = srpArduino.ReadLine();
            txtReceber.Invoke(new Action(() => { txtReceber.Text += dados + "\n"; }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrPortas.Tick += TmrPortas_Tick;
            tmrPortas.Enabled = true;
        }

        private void TmrPortas_Tick(object sender, EventArgs e)
        {
            var i = 0;
            var qtdeDiferente = false;

            if (cmbPortas.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (var porta in SerialPort.GetPortNames())
                    if (!cmbPortas.Items[i++].Equals(porta))
                    {
                        qtdeDiferente = true;
                        break;
                    }
            }
            else
                qtdeDiferente = true;

            if (!qtdeDiferente)
                return;

            cmbPortas.Items.Clear();

            foreach (var porta in SerialPort.GetPortNames())
                cmbPortas.Items.Add(porta);

            cmbPortas.SelectedIndex = 0;
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (!srpArduino.IsOpen)
            {
                try
                {
                    srpArduino.PortName = cmbPortas.Text;
                    srpArduino.Open();
                    btnConectar.Text = "Desconectar";
                    cmbPortas.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    srpArduino.Close();
                    btnConectar.Text = "Conectar";
                    cmbPortas.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (srpArduino.IsOpen)
                srpArduino.Write(txtEnviar.Text);
        }
    }
}