using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyRenamer
{
    public partial class Form1 : Form
    {
        private string _selectedPath; 
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            folderBrowserDialog1.SelectedPath = @"D:\Respite Upload\";
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                button1.Visible = true;
                _selectedPath = folderBrowserDialog1.SelectedPath;

                AddFilesToGrid();
            }
        }

        private void AddFilesToGrid()
        {
            FileInfo[] files = GetAllFiles(_selectedPath);

            foreach (FileInfo file in files)
            {
                dataGridView1.Rows.Add(file, file);

            }
        }

        public static FileInfo[] GetAllFiles(string path)
        {
            try
            {
                DirectoryInfo root = new DirectoryInfo(path);
                return root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }

            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);

            }

            return null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != row.Cells[1].Value)
                {
                    Debug.WriteLine("Renaming {0} to {1}", row.Cells[0].Value, row.Cells[1].Value);
                    string origFile = Path.Combine(_selectedPath, row.Cells[0].Value.ToString());
                    string newFile = Path.Combine(_selectedPath, row.Cells[1].Value.ToString());
                    try
                    {
                        File.Move(origFile, newFile);
                    } catch (IOException ex)
                    {
                        MessageBox.Show("File not found: " + origFile + "\nSystem Error Message: " + ex.Message, "Error Renaming File");
                    } 
                    
                }
                
            }

            dataGridView1.Rows.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
            } else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var filePath in files)
                {
                    Debug.WriteLine(filePath);
                }
            }
        }
    }
}
