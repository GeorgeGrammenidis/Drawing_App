using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Configuration;

namespace Project3_third_attempt_
{
    public partial class Form1 : Form
    {
        //Creats all the items necessary to draw with the pen://
        public Graphics graphics; //creates graphics//
        public Pen pen = new Pen(Color.Orange, 4); //the default pen size is 4//
        public Point current = new Point(); //these points are necessary for manual drawing//
        public Point old = new Point();
        int time; //used later for timed automatic drawings//
        string TS; //used later to store the values given by the timestamps//
        string provider = ConfigurationManager.AppSettings["settings"]; //used later to connect to database//
        string connectionString = ConfigurationManager.AppSettings["connectionString"];
        //used later for individual timestamp values//
        string year;
        string month;
        string date;
        string hour;
        string minute;
        string second;
        public Form1()
        {
            InitializeComponent();
            graphics = panel1.CreateGraphics(); //creates graphics for the panel//
            //default values for all the shapes://
            richTextBox1.Text = "50";
            richTextBox2.Text = "100";
            richTextBox3.Text = "50";
            richTextBox4.Text = "100";
            richTextBox5.Text = "50";
            richTextBox6.Text = "50";
            richTextBox7.Text = "50";
            richTextBox8.Text = "4";
            //uses timestamp to find the date //
            year = DateTime.Now.Year.ToString("0000");
            month = DateTime.Now.Month.ToString("00");
            date = DateTime.Now.Day.ToString("00");
            TS = "Date:" + date + "/" + month + "/" + year;
            label10.Text = TS; //shows the date on the side//
            //creates a combobox with basic values to save time from entering them manually//
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            comboBox1.Items.Add("6");
            comboBox1.Items.Add("7");
            comboBox1.Items.Add("8");
            comboBox1.Items.Add("9");
            comboBox1.Items.Add("10");
            comboBox1.Items.Add("11");
            comboBox1.Items.Add("12");
        }
        void DB(string s , string t) //name of the shape and time given as parameters to be stored in the table//
        {
            try
            {
                //the program attemps to establish a connection with database//
                DbProviderFactory factory = DbProviderFactories.GetFactory(this.provider);
                using (DbConnection connection = factory.CreateConnection())
                {
                    if (connection == null)
                    {
                        MessageBox.Show("Error: Failed to establish connection with database");
                    }
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    DbCommand command = factory.CreateCommand();
                    if (command == null)
                    {
                        MessageBox.Show("Command Error");
                    }
                    //sql command is used in sql database//
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO dbo.Table (shape , time) VALUES (" + s + "," + t + ")"; //the paremeterers are given within an sql command//
                    command.ExecuteReader();
                }
            }
            catch
            {
                MessageBox.Show("Failed to establish connection with database");
            }
            
            
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        //There are two similar and different methods both for drawing freely and for pasting shapes//
        //The two methods have similar code but one is for clicking the left button and the other is for moving the mouse while its held down//
        //The database process is included ONLY for each mouse left button click, instead for each time the mouse is moved while the button is held down//


        
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            try //the try/catch commands are used because the size of the brush and the shapes are dependent on what the user types in the richtextboxs//
            {
                year = DateTime.Now.Year.ToString("0000");
                month = DateTime.Now.Month.ToString("00");
                date = DateTime.Now.Day.ToString("00");
                hour = DateTime.Now.Hour.ToString("00");
                minute = DateTime.Now.Minute.ToString("00");
                second = DateTime.Now.Second.ToString("00");
                TS = date + "/" + month + "/" + year + "/" + hour + "/" + minute + "/" + second; //includes date and time so it can be stored in the database
                if (radioButton2.Checked == true) //if the user has selected the square radiobutton the program draws a square when the left mouse button is clicked//
                {
                    float square_size = float.Parse(richTextBox1.Text); //square size is manually typed in the corresponding textbox
                    graphics.DrawRectangle(pen, e.Location.X, e.Location.Y, square_size, square_size); //the square is a rectangle where height=width//
                    if (checkBox1.Checked == true) //if the user has checked the checkbox that signals that they want the shapes saved//
                    {
                        DB("square", TS); //saves "
                    }
                }
                else if (radioButton3.Checked == true) //if the user has selected the rectangle radiobutton
                {
                    float width = float.Parse(richTextBox2.Text); //values given by user
                    float height = float.Parse(richTextBox3.Text);
                    graphics.DrawRectangle(pen, e.Location.X, e.Location.Y, width, height); //draws rectangle//
                    if (checkBox1.Checked == true) //same as before
                    {
                        DB("Rectangle", TS);
                    }
                }
                else if (radioButton4.Checked == true)// if the user has selected the ellipse radiobutton 
                {
                    float width = float.Parse(richTextBox4.Text); //values given by user//
                    float height = float.Parse(richTextBox5.Text);
                    graphics.DrawEllipse(pen, e.Location.X, e.Location.Y, width, height);  //draws ellipse//
                    if (checkBox1.Checked == true) //same as before//
                    {
                        DB("Ellipse", TS);
                    }
                }
                else if (radioButton5.Checked == true) //in case the circle radiobutton was selected
                {
                    float radius = float.Parse(richTextBox6.Text); //value given by user//
                    graphics.DrawEllipse(pen, e.Location.X, e.Location.Y, radius, radius); // a circle is an ellipse where high=width//
                    if (checkBox1.Checked == true)
                    {
                        DB("Ellipse", TS);
                    }
                }
                else if (radioButton1.Checked == true) //in case the free draw radiobutton was selected
                {
                    pen.Width = float.Parse(richTextBox8.Text); //value given by user//
                    current = e.Location;
                    graphics.DrawLine(pen, old, current); //draws a small line where the mouse is
                    old = current; //replaces old with current point
                }
                else if (radioButton6.Checked == true) // in case the line radiobutton was selected//
                {
                    float length = float.Parse(richTextBox7.Text);
                    graphics.DrawLine(pen, e.Location.X, e.Location.Y, e.Location.X + length, e.Location.Y); //draws a horizontal line//
                    if (checkBox1.Checked == true)
                    {
                        DB("line", TS);
                    }
                }
            }
            catch //in case the appropriate textbox has no value, negative value, a string or whatever else//
            {
                MessageBox.Show("Please type a valid number in the appropriate box");
            }
           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            old = e.Location; //replaces old point with current location
            
        }


        private void panel1_MouseMove(object sender, MouseEventArgs e) //same as before, but this all happens when the left mouse button is held and the mouse is moving//
        {
            if (MouseButtons.Left == e.Button)
            {
                //same code as before//
                try
                {

                    if (radioButton2.Checked == true)
                    {
                        float square_size = float.Parse(richTextBox1.Text);
                        graphics.DrawRectangle(pen, e.Location.X, e.Location.Y, square_size, square_size);
                    }
                    else if (radioButton3.Checked == true)
                    {
                        float width = float.Parse(richTextBox2.Text);
                        float height = float.Parse(richTextBox3.Text);
                        graphics.DrawRectangle(pen, e.Location.X, e.Location.Y, width, height);
                    }
                    else if (radioButton4.Checked == true)
                    {
                        float width = float.Parse(richTextBox4.Text);
                        float height = float.Parse(richTextBox5.Text);
                        graphics.DrawEllipse(pen, e.Location.X, e.Location.Y, width, height);
                    }
                    else if (radioButton5.Checked == true)
                    {
                        float radius = float.Parse(richTextBox6.Text);
                        graphics.DrawEllipse(pen, e.Location.X, e.Location.Y, radius, radius);
                    }
                    else if (radioButton1.Checked == true)
                    {
                        pen.Width = float.Parse(richTextBox8.Text);
                        current = e.Location;
                        graphics.DrawLine(pen, old, current);
                        old = current;
                    }
                    else if (radioButton6.Checked == true)
                    {
                        float length = float.Parse(richTextBox7.Text);
                        graphics.DrawLine(pen, e.Location.X, e.Location.Y, e.Location.X + length, e.Location.Y);
                    }
                }
                catch
                {
                    MessageBox.Show("Please fill in the box with a valid number");

                }
            }
            label1.Text = e.X.ToString() + " , " + e.Y.ToString();
            
            
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //automatic drawing step by step//
            //each time the timer ticks the time variable increases by one
            if (time== 0) //one step at a time dependent on the time value
            {
                pen.Color = Color.Blue;
                pen.Width = 50;
                graphics.DrawLine(pen, 0, 50, 720, 50);
            }
            else if (time == 1)
            {
                pen.Color = Color.LightGreen;
                pen.Width = 80;
                graphics.DrawLine(pen, 0, 450, 720, 450);
            }
            else if (time == 2)
            {
                pen.Color = Color.DarkGray;
                pen.Width = 10;
                graphics.DrawLine(pen, 80, 400, 600, 400);
            }
            else if (time == 3)
            {
                pen.Color = Color.DarkGray;
                pen.Width = 30;
                graphics.DrawLine(pen, 100, 400, 100, 100);
            }
            else if (time == 4)
            {
                pen.Color = Color.DarkGray;
                pen.Width = 20;
                graphics.DrawLine(pen, 580, 400, 580 , 100);
            }
            else if (time == 5)
            {
                pen.Color = Color.DarkGray;
                pen.Width = 10;
                graphics.DrawLine(pen, 80, 100 , 600, 100);
                richTextBox8.Text = "4";
                pen.Width = 4;
                timer1.Stop();
            }
            time = time + 1; //every time a part is drawn the time variable increases so the next part will be drawn with the next tick//
        }

        private void drawTempleToolStripMenuItem_Click(object sender, EventArgs e) //clicking this button starts the automating drawing//
        {
            panel1.Invalidate();
            time = 0;
            timer1.Start(); //initiates the timed drawing//
        }

        private void drawHouseToolStripMenuItem_Click(object sender, EventArgs e) //same as before but for a different drawing
        {
            panel1.Invalidate(); 
            time = 0;
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e) 
        {
            if (time == 0)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 265, 135, 320, 50);
            }
            else if (time == 1)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 320, 50, 375, 135);
            }
            else if (time == 2)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 265, 135, 375, 135);
            }
            else if (time == 3)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 265, 135, 265, 380);
            }
            else if (time == 4)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 375, 135, 375, 380);
            }
            else if (time == 5)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 300, 300 , 350, 300);
            }
            else if (time == 6)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 300, 300, 300, 380);
            }
            else if (time == 7)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 350, 300, 350, 380);
            }
            else if (time == 8)
            {
                pen.Color = Color.Brown;
                pen.Width = 10;
                graphics.DrawLine(pen, 265, 380, 375 , 380);
                richTextBox8.Text = "4";
                pen.Width = 4;
                timer2.Stop();
            }
            time = time + 1;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (time == 0)
            {
                pen.Color = Color.Yellow;
                pen.Width = 5;
                graphics.DrawLine(pen, 340, 100, 240 , 300);
            }
            else if (time == 1)
            {
                pen.Color = Color.Yellow;
                pen.Width = 5;
                graphics.DrawLine(pen, 240, 300, 440 , 200);
            }
            else if (time == 2)
            {
                pen.Color = Color.Yellow;
                pen.Width = 5;
                graphics.DrawLine(pen, 240, 200 , 440 , 200);
            }
            else if (time == 3)
            {
                pen.Color = Color.Yellow;
                pen.Width = 5;
                graphics.DrawLine(pen, 240, 200, 440, 300);
            }
            else if (time == 4)
            {
                pen.Color = Color.Yellow;
                pen.Width = 5;
                graphics.DrawLine(pen, 340, 100, 440, 300);
                pen.Width = 4;
                timer3.Stop();
            }
            time = time + 1;
        }

        private void drawStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Invalidate();
            time = 0;
            timer3.Start();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e) //clicking this button clears the entire board//
        {
            panel1.Invalidate();
        }

        private void changeColourToolStripMenuItem_Click(object sender, EventArgs e) //clicking this button changes the color of the pen and the shapes//
        {
            ColorDialog color_dialog = new ColorDialog();
            if (color_dialog.ShowDialog() == DialogResult.OK) //presents the user with color options//
            {
                pen.Color = color_dialog.Color;
            }
        }
  
        private void label11_Click(object sender, EventArgs e)
        {

        }

        //the following button generates a line with manually given coordinates that can be of any angle and direction//
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                graphics.DrawLine(pen, float.Parse(richTextBox9.Text), float.Parse(richTextBox10.Text), float.Parse(richTextBox11.Text), float.Parse(richTextBox12.Text));
            }
            catch
            {
                MessageBox.Show("Please type in valid coordinates");
            }
        }

        private void penSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox8.Text = comboBox1.Text; //changes the pen size according to the value of the combobox//
        }
    }
}
