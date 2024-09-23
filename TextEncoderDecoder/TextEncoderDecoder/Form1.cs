using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TextEncoderDecoder
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<char, string> encodeTable = new Dictionary<char, string>
        {
            {'а', "e0"}, {'б', "e1"}, {'в', "e2"}, {'г', "e3"}, {'д', "e4"}, {'е', "e5"}, {'ё', "b6"}, {'ж', "e7"}, {'з', "e8"}, {'и', "e9"},
            {'й', "ea"}, {'к', "eb"}, {'л', "ec"}, {'м', "ed"}, {'н', "ee"}, {'о', "ef"}, {'п', "f0"}, {'р', "f1"}, {'с', "f2"}, {'т', "f3"},
            {'у', "f4"}, {'ф', "f5"}, {'х', "f6"}, {'ц', "f7"}, {'ч', "f8"}, {'ш', "f9"}, {'щ', "fa"}, {'ъ', "fb"}, {'ы', "fc"}, {'ь', "fd"},
            {'э', "fe"}, {'ю', "ff"}, {'я', "ff"}, {' ', "a0"}, {',', "82"},
        };

        private readonly Dictionary<string, char> decodeTable = new Dictionary<string, char>
        {
            {"e0", 'а'}, {"e1", 'б'}, {"e2", 'в'}, {"e3", 'г'}, {"e4", 'д'}, {"e5", 'е'}, {"b6", 'ё'}, {"e7", 'ж'}, {"e8", 'з'}, {"e9", 'и'},
            {"ea", 'й'}, {"eb", 'к'}, {"ec", 'л'}, {"ed", 'м'}, {"ee", 'н'}, {"ef", 'о'}, {"f0", 'п'}, {"f1", 'р'}, {"f2", 'с'}, {"f3", 'т'},
            {"f4", 'у'}, {"f5", 'ф'}, {"f6", 'х'}, {"f7", 'ц'}, {"f8", 'ч'}, {"f9", 'ш'}, {"fa", 'щ'}, {"fb", 'ъ'}, {"fc", 'ы'}, {"fd", 'ь'},
            {"fe", 'э'}, {"ff", 'я'}, {"a0", ' '}, {"82", ','}
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            string inputText = txtInput.Text;
            StringBuilder encodedString = new StringBuilder();

            foreach (char ch in inputText)
            {
                if (encodeTable.ContainsKey(ch))
                {
                    encodedString.Append(encodeTable[ch] + " ");
                }
                else
                {
                    MessageBox.Show($"Символ '{ch}' не найден в таблице кодировки.");
                    return;
                }
            }

            txtOutput.Text = encodedString.ToString().Trim();
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            try
            {
                string encodedText = txtOutput.Text;
                if (string.IsNullOrEmpty(encodedText))
                {
                    MessageBox.Show("Неверный формат текста для декодирования.");
                    return;
                }

                string[] hexBlocks = encodedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder decodedString = new StringBuilder();

                foreach (string hexBlock in hexBlocks)
                {
                    if (decodeTable.ContainsKey(hexBlock))
                    {
                        decodedString.Append(decodeTable[hexBlock]);
                    }
                    else
                    {
                        MessageBox.Show($"Код '{hexBlock}' не найден в таблице декодирования.");
                        return;
                    }
                }

                txtDecodedOutput.Text = decodedString.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при декодировании: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDecodedOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBinaryEncode_Click(object sender, EventArgs e)
        {
            string outputText = txtOutput.Text;
            StringBuilder binaryString = new StringBuilder();
            foreach (char ch in outputText)
            {
                string binaryChar = Convert.ToString(ch, 2).PadLeft(8, '0');
                binaryString.Append(binaryChar + " "); 
            }
            txtBinaryOutput.Text = binaryString.ToString().Trim();
        }


        private void txtBinaryOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
