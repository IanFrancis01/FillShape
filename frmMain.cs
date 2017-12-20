using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ambro.Drawing;

namespace FillShape
{
    public partial class frmMain : Form
    {
        //form variables
        private Grid grid;
        private char[,] shape;
        private Color fillColor;
        int MapRows = 0;
        int MapColumns = 0;

        public frmMain()
        {
            InitializeComponent();

            //initialize grid
            grid = new Grid(15, 40, 0, 30, 20);

            //default fill color to blue
            fillColor = Color.Blue;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //draw grid          
            grid.DrawGrid(e.Graphics);

        }

        private void frmMain_MouseClick(object sender, MouseEventArgs e)
        {
            //get cell clicked
            int col = e.Y / grid.getCellSize();
            int row = (e.X - 30) / grid.getCellSize();
            
            //fill shape if within shape or fill outside
            FillShape(row, col);
            //set up the grid cell components to display colour accordingly
            ConfigureGrid();
            //refesh to call OnPaint 
            this.Refresh();

        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            LoadFile();
        }


        private void LoadFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();


            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(fileDialog.OpenFile());

                string line = sr.ReadLine();
                int cols = int.Parse(line);
                line = sr.ReadLine();
                int rows = int.Parse(line);
                shape = new char[rows, cols];

                MapRows = rows;
                MapColumns = cols;

                for (int r = 0; r < rows; r++)
                {
                    line = sr.ReadLine();
                    for (int c = 0; c < cols; c++)
                    {
                        shape[r, c] = line[c];
                    }
                }

                ConfigureGrid();
            }

            this.Refresh();
        }

        private void ConfigureGrid()
        {
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    //periods are dots
                    if (shape[r, c] == '.')
                        grid.getCell(r, c).setBackColor(Color.White);
                        //edges are T's
                    else if (shape[r, c] == 'T')
                        grid.getCell(r, c).setBackColor(Color.Black);
                        //New color is F
                    else if (shape[r, c] == 'F')
                        grid.getCell(r, c).setBackColor(fillColor);
                }
            }
        }

        private void mnuFileColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                fillColor = cd.Color;
            }
        }

        public void FillShape(int r, int c)
        {
            //check if out of bounds
            if (r < 0 || c < 0 || r >= MapRows || c >= MapColumns) return;

            //check if at the edge of the shape
            if (grid.getCell(r,c).getBackColor() == Color.Black) return;

            //if you are here, you are still within the bounds of the shape
            if (grid.getCell(r, c).getBackColor() == Color.White)
            {
                grid.getCell(r, c).setBackColor(fillColor);

                //move in all directions to find the endpoint.

                //move down
                FillShape(r + 1, c);

                //move up
                FillShape(r - 1, c);

                //move right
                FillShape(r, c + 1);

                //move right
                FillShape(r, c - 1);
            }

            //if you reach down here, you have filled the shape.
            return;

        }
    }
}
