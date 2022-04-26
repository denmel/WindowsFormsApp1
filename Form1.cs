using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        Button[] buttons = new Button[16];
        Random rnd = new Random();
        TableLayoutPanelCellPosition space = new TableLayoutPanelCellPosition();

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 16; i++)
            {
                buttons[i] = new Button();
                buttons[i].Dock = DockStyle.Fill;
                buttons[i].Font = new Font("Segoe UI Semibold", 35F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                buttons[i].UseVisualStyleBackColor = true;
                buttons[i].Click += new EventHandler(button1_Click);
                tlp.Controls.Add(buttons[i],i%4,i/4);
            }
        }

        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> n = new List<int>();
            int x;
            do
            {
                for (int i = 0; i < 16; i++)
                {
                    n.Add(i);
                }
                for (int i = 0; i < 16; i++)
                {
                    x = rnd.Next(n.Count);
                    if (n[x] == 0)
                    {
                        (tlp.Controls[i] as Button).Text = "";
                        space.Row = i / 4;
                        space.Column = i % 4;
                    }
                    else (tlp.Controls[i] as Button).Text = n[x].ToString();
                    n.RemoveAt(x);
                }
            } while (!isCorrect());
        }

        private bool isCorrect()
        {
            int s = space.Row+1;
            for (int i = 0; i < 15; i++)
            {
                int c1 = ((tlp.Controls[i] as Button).Text == "" ? 0 : Int32.Parse((tlp.Controls[i] as Button).Text));
                for (int j = i+1; i < 16; i++)
                {
                    int c2 = ((tlp.Controls[j] as Button).Text == "" ? 16 : Int32.Parse((tlp.Controls[j] as Button).Text));
                    if (c1 > c2) s++;
                }
            }
            return s % 2==1 ? false : true;
        }

        private bool canMove(TableLayoutPanelCellPosition position)
        {
            return Math.Abs(position.Row - space.Row) + Math.Abs(position.Column - space.Column) == 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TableLayoutPanelCellPosition position = tlp.GetCellPosition(sender as Control);

            if (canMove(position))
            {
                (tlp.Controls[tlpToInt(space)] as Button).Text = (tlp.Controls[tlpToInt(position)] as Button).Text;
                (tlp.Controls[tlpToInt(position)] as Button).Text = "";
                Swap(ref position, ref space);
                if (checkWin())
                {
                    MessageBox.Show("Win");
                }
            }
        }
        private void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        private int tlpToInt(TableLayoutPanelCellPosition position)
        {
            return position.Row * 4 + position.Column;
        }

        private bool checkWin()
        {
            foreach (Control control in tlp.Controls)
            {
                if (((control as Button).Text==""?0:Int32.Parse((control as Button).Text))!=(tlpToInt(tlp.GetCellPosition(control))+1)%16)
                {
                    return false;
                }                
            }
            return true;
        }
    }
}
