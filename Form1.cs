using BitMEX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;



namespace MeBo
{
    public partial class Form1 : Form
    {
        public string Symbol = "XBTUSD";
        double Price = 0;
        double realliquid = 0;

        BitMEXApi bitmex = new BitMEXApi();
        List<OrderBook> CurrentBook = new List<OrderBook>();
        List<candle> CanDles = new List<candle>();

        bool Running = false;
        string Mode = "Wait";


        public Form1()
        {
            InitializeComponent();
            InitDropDown();
            timercandle.Start();
            // string result = bitmex.GetOrders(Symbol);   // OPEN ORDER CHECK JSON
            //  MessageBox.Show(result);

            //MessageBox.Show(CurrentOrderPrice(Symbol, "Buy").ToString());
            // while(true)
            // {
            text1.Text = "Buy";
            text2.Text = "Sell";



        }

        private void InitDropDown()
        {
            comboBox1.SelectedIndex = 0; // no choosing combo box.
                                         //  comboBox2.SelectedIndex = 0;
        }
        private double CurrentOrderPrice(string Symbol, string Side)
        {
            CurrentBook = bitmex.GetOrderBook(Symbol, 3); // look for number 1 depth
            double SellPrice = CurrentBook.Where(a => a.Side == "Sell").FirstOrDefault().Price;
            double BuyPrice = CurrentBook.Where(a => a.Side == "Buy").FirstOrDefault().Price;
            double sell1 = CurrentBook.Where(a => a.Side == "Sell").ElementAt(2).Price;
           

            double OrderPrice = 0;
            switch (Side)
            {
                case "Buy":
                    OrderPrice = BuyPrice;

                    break;

                case "Sell":
                    OrderPrice = sell1;
                    break;
                    //case "Sell":
                    //   OrderPrice = sell1;
                    //break;





            }
            return OrderPrice;

        }
        private void PnlUpdate()
        {
            label6.Text = bitmex.GetPNL(Symbol,realliquid);

        }


        private void PriceUpdate()
        {
            buylabel.Text = (CurrentOrderPrice(Symbol, "Buy").ToString());
            selllabel.Text = (CurrentOrderPrice(Symbol, "Sell").ToString());
            label3.Text = (CurrentOrderPrice(Symbol, "Sell_1").ToString());
        }
        private void MakeOrder(string Symbol, string Side, int Qty, double Price = 0) // Price ==0 to be reference, to overide later 
        {
            switch (Side)
            {
                case "Buy":
                    if(Price==0)
                    {
                        Price = CurrentOrderPrice(Symbol, Side);
                    }
                    var MarketBuy = bitmex.PostOrderPostOnly(Symbol, Side, Price, Qty);

                    break;

                case "Sell":
                    if(Price==0)
                    {
                        Price = CurrentOrderPrice(Symbol, Side); 
                    }
                    var MarketSell = bitmex.PostOrderPostOnly(Symbol, Side, Price, Qty);
                    break;
            }
        }

        private void MakeFormation(string Symbol, string Side, int Qty, double Price = 0) // Price ==0 to be reference, to overide later 
        {
            //switch (Side)
            // {
            //  case "Buy":
            
            for (int i = 1; i <= 5; i++)
            {
                if (Price == 0)
                {
                    Price = CurrentOrderPrice(Symbol, Side);
                }
                else
                {
                    Price = CurrentOrderPrice(Symbol, Side) + (i*10);
                }
                var MarketBuy = bitmex.PostOrderPostOnly(Symbol, "Sell", Price, Qty);
            }
            //   break;

            for (int i = 1; i <= 5; i++)
            {
                if (Price == 0)
                {
                    Price = CurrentOrderPrice(Symbol, Side);
                }
                else
                {
                    Price = CurrentOrderPrice(Symbol, Side) - (i * 10);
                }
                var MarketBuy = bitmex.PostOrderPostOnly(Symbol, "Buy", Price, Qty);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void buylabel_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void text2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeOrder(Symbol, "Buy", Convert.ToInt32(numericUpDown1.Value));
        }

     


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MakeOrder(Symbol, "Sell", Convert.ToInt32(numericUpDown1.Value));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                bitmex.CancelAllOpenOrders(Symbol);
            }
            else
            MakeFormation(Symbol, "Buy", Convert.ToInt32(numericUpDown1.Value));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

     /*  private void Updatecandle()
        {
            CanDles = bitmex.GetCandle(Symbol, Convert.ToInt32(numericUpDown2.Value), comboBox1.SelectedItem.ToString());

            foreach(candle c in CanDles)
            {
                c.PCC = CanDles.Where(a => a.TimeStamp < c.TimeStamp).Count();

                int MA1Period = Convert.ToInt32(numericUpDown3.Value);
                int MA2Period = Convert.ToInt32(numericUpDown4.Value);

                if(c.PCC > MA1Period)
                {
                    c.MA1 = CanDles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(MA1Period).Average(a => a.Close);

                }
                if(c.PCC > MA2Period)
                {
                    c.MA2 = CanDles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(MA2Period).Average(a => a.Close);

                }



            }



            dataGridView1.DataSource = CanDles;
        }*/


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void timercandle_Tick(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
               // Updatecandle();
                PriceUpdate();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void text1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e) //START
        {
           
              //  Updatecandle();
                PriceUpdate();
                PnlUpdate();
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
