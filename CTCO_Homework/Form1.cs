using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTCO_Homework
{
    public partial class Form1 : Form
    {
        public class Payment
        {
            public string Name { get; set; }
            public string Service { get; set; }
            public int Price { get; set; }
        }

        public class PersonTotal
        {
            public string Name { get; set; }
            public int Cost { get; set; }
        }


        List<Payment> Pay = new List<Payment>();
        List<PersonTotal> Tot = new List<PersonTotal>();

        int total = 0;
        int average = 0;

        public Form1()
        {
            InitializeComponent();
        }





        private void button1_Click(object sender, EventArgs e)
        {
            //Person count
            int skaititajs = 0;
            for (int i = 0; i < Tot.Count; i++)
            {
                if (textBox1.Text == Tot[i].Name)
                {
                    Tot[i].Cost += Convert.ToInt32(textBox3.Text);
                    skaititajs++;
                }
            }
            if (skaititajs == 0)
                Tot.Add(new PersonTotal { Name = textBox1.Text, Cost = Convert.ToInt32(textBox3.Text) });
                Pay.Add(new Payment { Name = textBox1.Text, Service = textBox2.Text, Price = Convert.ToInt32(textBox3.Text) });
            //videjais average
            total += Convert.ToInt32(textBox3.Text);
            label6.Text = "TOTAL($): " + Convert.ToString(total);
            average = (total / Tot.Count);
            label7.Text = "AVERAGE($): " + Convert.ToString(average) + " per person";


            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Pay;
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = Tot;
        }
        static void transactions(int[] bilance)
        {
            int nmax = bilance.Length; // masīva garums
            int n, i, m;
            for (n = 0; n < nmax; n++) // cikls pāru meklēšanai pēc masīva elementu skaita (katrā solī nonullējas viena vai divas vērtības ) 
                                       // ja viss sadalīts visi masīva elementi ir palikuši pa nullēm un cikls pārtrauksies ar break
            {
                int minval = 0, minabsval = 0, minvalid = -1;
                for (i = 0; i < nmax; i++)  // meklē pa masīvu minimālo absolūto vērtību ,kura garantēti dzēsīsies ar pretējās zīmes summu , nulles masīvā ignorē
                {
                    m = bilance[i];
                    if (m != 0) // nulles masīvā ignorē
                    {
                        if (m < 0)
                        {
                            m = -m; // nomaina zīmi uz plus
                        }
                        if (m < minabsval | minvalid < 0)   //  pirmo atrasto nenulles vērtību arī uzskata par minabsval un meklē tālāk
                        {
                            minabsval = m;
                            minvalid = i;  // ja masīvā ir tikai nulles vertība paliek minapsvalid = -1
                        }
                    }
                }
                if (minvalid < 0)  // pārtrauc n ciklu vairāk minimālā absolūtā vērtība nav atrasta
                {
                    break;
                }
                // velreiz meklē pa masīvu lielāko vērtību ar pretējo zīmi atrastajai ignorējot nulles un izdara pārskaitījumu mazāko vērtību dzēšot (persona nonullējas)
                // ja atrod teeši sakrītošu pretējas zīmes vērtību - abas atrastās vērtības nonullē ( divas personas nonullējas ar vienu pārskaitījumu) 
                minval = bilance[minvalid];  // atrastā minimālā vērtība  (ar zīmi)
                int maxval = 0, maxvalid = -1;
                for (i = 0; i < nmax; i++)  // meklē lielāko vērtību ar pretējo zīmi atrastajai ignorējot nulles
                {
                    m = bilance[i];
                    if ((minval + m) == 0) // ļoti labs gadījums vērības kompensējas tālāk vairs nemeklē ( divas personas nonullējas ar vienu pārskaitījumu) 
                    {
                        maxval = m;
                        maxvalid = i;
                        break;
                    }
                    if (minval > 0 & m < 0)  // minimālā abs vērtība ir pozitīva masīvā meklēs maksimāli negatīvāko
                    {
                        if (m < maxval)   // atrastā ir negatīvāka un nav nulle
                        {
                            maxval = m;
                            maxvalid = i;
                        }
                    }
                    if (minval < 0 & m > 0)  // minimālā abs vērtība ir negatīva masīvā meklēs maksimāli pozitīvāko
                    {
                        if (m > maxval)   // atrastā ir pozitīvāka un nav nulle
                        {
                            maxval = m;
                            maxvalid = i;
                        }
                    }
                }
                // ja viss ir ok  maxvalid nav - 1  un ir atrastas minval um maxval ar pretējām zīmēm minvalid un maxvalid satur masīva elementu numurus (personas id)
                int parsksumma = 0;
                if (maxvalid >= 0)  // izveido pārskaitījumus un masīvā pārrēķina atrastos elementus
                {
                    parsksumma = Math.Abs(minval);
                    if ((minval + maxval) == 0)  // divas personas nonullējas ar vienu pārskaitījumu 
                    {
                        if (minval < 0) { izvads(minvalid, maxvalid, parsksumma); }  // maxval ir pozitīva
                        if (minval > 0) { izvads(maxvalid, minvalid, parsksumma); }  // minval ir pozitīva
                        bilance[minvalid] = 0;
                        bilance[maxvalid] = 0;
                        minval = 0;   // novērš tālāku if izpildi
                    }
                    // tālāk nonullējas tikai persona ar minval vērtību un izvadās pārskaitījums . maxval vērtībai atņemas pārskaitītā summa
                    if (minval < 0)
                    {
                        izvads(minvalid, maxvalid, parsksumma);   // maxval ir pozitīva
                        bilance[minvalid] = 0;
                        bilance[maxvalid] = bilance[maxvalid] - parsksumma;
                    }
                    if (minval > 0)
                    {
                        izvads(maxvalid, minvalid, parsksumma);   // minval ir pozitīva
                        bilance[minvalid] = 0;
                        bilance[maxvalid] = bilance[maxvalid] + parsksumma;
                    }
                    for (int z = 0; z < bilance.Length; z++)
                    {
                        System.Console.Write("{0},", bilance[z]);
                    }

                }
            }
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            int i;
            int[] arr = new int[Tot.Count];
            for (i = 0; i <= Tot.Count; i++)
            {
                arr[i] = average - Tot[i].Cost;
            }
            
         
            //int[] bilance = { 900, 100, 99, 98, 1,-2,-202,-297,-299,-300,-301 }; // optimāli 9 transakcijas
            // int[] bilance = { 9, 8, -3, -6, -8 } ;  // opt 3
            // int[] bilance = { 9, 8, 2, -4, -5, -10 };     // optimums 4
            //int[] bilance = { -8, -4, 5, 7 };
            // int[] bilance ={10-1 10-9 7-5 7-2 7-1}
            //  person id       0   1   2   3  4   5  6
            int[] bilance = arr;
                //{ -9, -5, -2, -1, 7, 10, 0 };
            //int[] bilance = { -12, -7, 0, 2, 8, 9 };
            // kopējai masīva summai vajadzētu būt nullei lai sadalītu visas summas ,
            // bilance izdotās summas nobīde no vidējās izdotās summas. plus ir izdevis vairāk par vidējo mīnus - mazāk
            // pāskaitījumus izdara no mīnus summas uz plus

            int ksum = 0;
            for (int z = 0; z < bilance.Length; z++)
            {
                ksum = ksum + bilance[z];
                System.Console.Write("{0},", bilance[z]);
                //label8.Text = Convert.ToString(bilance[z]);
            }
            Console.WriteLine(" Sākuma summa: {0}", ksum);
            transactions(bilance);
            Console.WriteLine();
            ksum = 0;
            for (int z = 0; z < bilance.Length; z++)
            {
                ksum = ksum + bilance[z];
                System.Console.Write("{0},", bilance[z]);
            }
            Console.WriteLine(" Beigu summa: {0}", ksum);
            Console.ReadLine(); // pauze un programmas beigas
        }

        static void izvads(int persid_from, int presid_to, int transumma)
        {
            System.Console.WriteLine();
            Console.Write("Parskaitijums no: {0} uz {1}  Summa: {2} ($)", persid_from, presid_to, transumma);            
        }
    }
}
