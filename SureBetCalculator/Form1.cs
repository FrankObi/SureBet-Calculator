using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Diagnostics;



namespace SureBetCalculator
{
    public partial class Form1 : Form
    {
        XmlWriter writer;

        public Form1()
        {
            InitializeComponent();
        }

        private void calculate_Click(object sender, EventArgs e)
        {
            calc_surebet();
        }

        private void calc_surebet()
        {
            List<BetOdds> BoList = new List<BetOdds>();
            List<BetOdds> BoList2 = new List<BetOdds>();
            List<BetOdds> BoList3 = new List<BetOdds>();
            string[] elename = new string[] {"id","siteid","match",
                "win","draw","lose","windraw","drawlose","winlose",
                "winnodraw","losenodraw","under05","over05","under15",
                "over15","under25","over25","under35","over35","under45",
                "over45","under55","over55","under65","over65","date"};

            try
            {
                OpenFileDialog a = new OpenFileDialog();

                a.Filter = "XML FILE | *.xml";
                a.ShowDialog();
               
                StreamReader sr = new StreamReader(a.FileName);
                XDocument doc = XDocument.Load(sr);
                var data = doc.Descendants("MAC");

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                writer = XmlWriter.Create(a.FileName.Replace(".xml", "_surebets.xml"), settings);

                writer.WriteStartElement("MACLAR");

               
                foreach (XElement d in data)
                {

                    BetOdds temp = new BetOdds(); 

                    temp.id = Int32.Parse(d.Element(elename[0]).Value);
                    //temp.macid = d.Element(elename[1]).Value;
                    temp.siteid = Int32.Parse(d.Element(elename[1]).Value);
                    temp.match = d.Element(elename[2]).Value;
                    temp.win = Convert.ToDouble(d.Element(elename[3]).Value);
                    temp.draw = Convert.ToDouble(d.Element(elename[4]).Value);
                    temp.lose = Convert.ToDouble(d.Element(elename[5]).Value);
                    temp.windraw = Convert.ToDouble(d.Element(elename[6]).Value);
                    temp.drawlose = Convert.ToDouble(d.Element(elename[7]).Value);
                    temp.winlose = Convert.ToDouble(d.Element(elename[8]).Value);
                    temp.winnodraw = Convert.ToDouble(d.Element(elename[9]).Value);
                    temp.losenodraw = Convert.ToDouble(d.Element(elename[10]).Value);
                    temp.under05 = Convert.ToDouble(d.Element(elename[11]).Value);
                    temp.over05 = Convert.ToDouble(d.Element(elename[12]).Value);
                    temp.under15 = Convert.ToDouble(d.Element(elename[13]).Value);
                    temp.over15 = Convert.ToDouble(d.Element(elename[14]).Value);
                    temp.under25 = Convert.ToDouble(d.Element(elename[15]).Value);
                    temp.over25 = Convert.ToDouble(d.Element(elename[16]).Value);
                    temp.under35 = Convert.ToDouble(d.Element(elename[17]).Value);
                    temp.over35 = Convert.ToDouble(d.Element(elename[18]).Value);
                    temp.under45 = Convert.ToDouble(d.Element(elename[19]).Value);
                    temp.over45 = Convert.ToDouble(d.Element(elename[20]).Value);
                    temp.under55 = Convert.ToDouble(d.Element(elename[21]).Value);
                    temp.over55 = Convert.ToDouble(d.Element(elename[22]).Value);
                    temp.under65 = Convert.ToDouble(d.Element(elename[23]).Value);
                    temp.over65 = Convert.ToDouble(d.Element(elename[24]).Value);

                    temp.date = d.Element(elename[25]).Value;
                    BoList.Add(temp);
                }
                

                Parallel.ForEach (BoList, bo =>
                {
                    //bo.checkbaseodds();
                    //if (bo.sbwdl)
                    //{
                    //    writesurebettoxml((BetOdds.BetType)0, bo);
                    //    //MessageBox.Show(null, "0:  \n" + bo.id, "", MessageBoxButtons.OK);
                    //}
                    
                    //var sw = new Stopwatch();
                    //sw.Start();
                    Parallel.ForEach (BoList, bo2 =>
                    {
                        Parallel.For(0, 12, i =>
                        {
                            if (bo.compare_odds(bo2, (BetOdds.BetType)i))
                            {
                                writesurebettoxml((BetOdds.BetType)i, bo, bo2);
                                //MessageBox.Show(null, i.ToString() + ":  \n" + bo.id + "\n" + bo2.id, "", MessageBoxButtons.OK);
                            }
                        });

                        //foreach (BetOdds bo3 in BoList)
                        //{
                        //    //if (ilist2.Exists(x => x == bo3.id))
                        //    //{
                        //    //    continue;
                        //    //}

                        //    for (int i = 12; i < 13; i++)
                        //    {
                        //        if (bo.compare_odds(bo2, (BetOdds.BetType)i, bo3))
                        //        {
                        //            writesurebettoxml((BetOdds.BetType)i, bo, bo2, bo3);
                        //            MessageBox.Show(null, i.ToString() + ":  \n" + bo.id + "\n" + bo2.id + "\n" + bo3.id, "", MessageBoxButtons.OK);
                        //        }
                        //    }
                        //}
                    });
                    //sw.Stop();
                });
                writer.WriteEndElement();
                writer.Close();

            }
            catch (Exception ex)
            {
                //Do Nothing
                MessageBox.Show(null, ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void writesurebettoxml(BetOdds.BetType BT, BetOdds bo, BetOdds bo2 = null, BetOdds bo3 = null)
        {

            string id = "";
            string siteid = "";
            string match = "";
            string surebet = "";
            string date = "";


            if (BT == 0)
            {
                id = bo.id.ToString();
                siteid = bo.siteid.ToString();
                match = bo.match;
                surebet = "WDL";
                date = bo.date;
                goto TF;
            }

            if (bo3 != null)
            {
                switch (BT)
                {
                    case BetOdds.BetType.W_D_L:
                        id = bo.id.ToString() + " | " + bo2.id.ToString() + " | " + bo3.id.ToString();
                        siteid = bo.siteid.ToString()  + " | " + bo2.siteid.ToString() + " | " + bo3.siteid.ToString();
                        match = bo.match + " | " + bo2.match + " | " + bo3.match;
                        surebet = "W | D | L";
                        date = bo.date;
                        break;
                }

                goto TF;
            }

            switch (BT)
            {
                case BetOdds.BetType.D_WL:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString()  + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "D | WL";
                    date = bo.date;
                    break;
                case BetOdds.BetType.L_WD:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString()  + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "L | WD";
                    date = bo.date;
                    break;
                case BetOdds.BetType.WN_LN:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "WN | LN";
                    date = bo.date;
                    break;
                case BetOdds.BetType.W_DL:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "W | DL";
                    date = bo.date;
                    break;
                case BetOdds.BetType.U_O_05:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString()  + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "U | O (05)";
                    date = bo.date;
                    break;
                case BetOdds.BetType.U_O_15:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "U | O (15)";
                    date = bo.date;
                    break;
                case BetOdds.BetType.U_O_25:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "U | O (25)";
                    date = bo.date;
                    break;
                case BetOdds.BetType.U_O_35:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "U | O (35)";
                    date = bo.date;
                    break;
                case BetOdds.BetType.U_O_45:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "U | O (45)";
                    date = bo.date;
                    break;
                case BetOdds.BetType.U_O_55:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "U | O (55)";
                    date = bo.date;
                    break;
                case BetOdds.BetType.U_O_65:
                    id = bo.id.ToString() + " | " + bo2.id.ToString();
                    siteid = bo.siteid.ToString() + " | " + bo2.siteid.ToString();
                    match = bo.match + " | " + bo2.match;
                    surebet = "U | O (65)";
                    date = bo.date;
                    break;
                
            }

        TF:
            
            writer.WriteStartElement("MAC");
                
            writer.WriteStartElement("id");
            writer.WriteString(id);
            writer.WriteEndElement();
            writer.WriteStartElement("siteid");
            writer.WriteString(siteid);
            writer.WriteEndElement();
            writer.WriteStartElement("match");
            writer.WriteString(match);
            writer.WriteEndElement();
            writer.WriteStartElement("surebet");
            writer.WriteString(surebet);
            writer.WriteEndElement();
            writer.WriteStartElement("date");
            writer.WriteString(date);
            writer.WriteEndElement();
                
            writer.WriteEndElement();
            
        }

    }


    public class BetOdds
    {

        public enum BetType
        {
            W_DL = 1,
            L_WD = 2,
            D_WL = 3,
            U_O_05 = 4,
            U_O_15 = 5,
            U_O_25 = 6,
            U_O_35 = 7,
            U_O_45 = 8,
            U_O_55 = 9,
            U_O_65 = 10,
            WN_LN = 11,
            W_D_L = 12,
        }

        public int id = -1;
        public string macid;
        public int siteid = -1;
        public string match;
        public double win = -1;
        public double draw = -1;
        public double lose = -1;
        public double windraw = -1;
        public double winlose = -1;
        public double drawlose = -1;
        public double winnodraw = -1;
        public double losenodraw = -1;
        public double under05 = -1;
        public double over05 = -1;
        public double under15 = -1;
        public double over15 = -1;
        public double under25 = -1;
        public double over25 = -1;
        public double under35 = -1;
        public double over35 = -1;
        public double under45 = -1;
        public double over45 = -1;
        public double under55 = -1;
        public double over55 = -1;
        public double under65 = -1;
        public double over65 = -1;
        public string date;

        public bool sbwdl = false;


        public void checkbaseodds()
        {
            if ((1 / win + 1 / draw + 1 / lose) < 0.81)
            {
                sbwdl = true;
            }
        }

        public bool compare_odds(BetOdds bo, BetType BT, BetOdds bo2 = null)
        {
            bool odds = false;

            if (date == bo.date && (GetSimilarityRatio(match, bo.match) > 81)
                && id != bo.id && siteid != bo.siteid)
            {
                if (BT == BetType.W_DL && bo.drawlose != -1 && win != -1
                    && (1 / win + 1 / bo.drawlose) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.L_WD && bo.windraw != -1 && lose != -1
                    && (1 / lose + 1 / bo.windraw) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.D_WL && bo.winlose != -1 && draw != -1
                    && (1 / draw + 1 / bo.winlose) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.WN_LN && bo.winnodraw != -1 && losenodraw != -1
                && (1 / winnodraw + 1 / bo.losenodraw) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.U_O_05 && bo.over05 != -1 && under05 != -1
                    && (1 / under05 + 1 / bo.over05) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.U_O_15 && bo.over15 != -1 && under15 != -1
                   && (1 / under15 + 1 / bo.over15) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.U_O_25 && bo.over25 != -1 && under25 != -1
                   && (1 / under25 + 1 / bo.over25) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.U_O_35 && bo.over35 != -1 && under35 != -1
                   && (1 / under35 + 1 / bo.over35) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.U_O_45 && bo.over45 != -1 && under45 != -1
                   && (1 / under45 + 1 / bo.over45) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.U_O_55 && bo.over55 != -1 && under55 != -1
                   && (1 / under55 + 1 / bo.over55) < 0.81)
                {
                    odds = true;
                }
                else if (BT == BetType.U_O_65 && bo.over65 != -1 && under65 != -1
                   && (1 / under65 + 1 / bo.over65) < 0.81)
                {
                    odds = true;
                }


                if (bo2 != null && siteid != bo2.siteid && bo.siteid != bo2.siteid && id != bo2.id && bo.id != bo2.id
                    && GetSimilarityRatio(match, bo2.match) > 81 && date == bo2.date)
                {
                    
                    if (BT == BetType.W_D_L && (win != -1 && bo.draw != -1 && bo2.lose != -1) && (1 / win + 1 / bo.draw + 1 / bo2.lose) < 0.81)
                    {
                        odds = true;
                    }
                }
            }
            return odds;
        }

        double GetSimilarityRatio(String FullString1, String FullString2)
        {
            double theResult = 0;
            String[] Splitted1 = FullString1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            String[] Splitted2 = FullString2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (Splitted1.Length < Splitted2.Length)
            {
                String[] Temp = Splitted2;
                Splitted2 = Splitted1;
                Splitted1 = Temp;
            }
            int[,] theScores = new int[Splitted1.Length, Splitted2.Length];//Keep the best scores for each word.0 is the best, 1000 is the starting.
            int[] BestWord = new int[Splitted1.Length];//Index to the best word of Splitted2 for the Splitted1.

            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                for (int loop1 = 0; loop1 < Splitted2.Length; loop1++) theScores[loop, loop1] = 1000;
                BestWord[loop] = -1;
            }
            int WordsMatched = 0;
            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                String String1 = Splitted1[loop];
                for (int loop1 = 0; loop1 < Splitted2.Length; loop1++)
                {
                    String String2 = Splitted2[loop1];
                    int LevenshteinDistance = Compute(String1, String2);
                    theScores[loop, loop1] = LevenshteinDistance;
                    if (BestWord[loop] == -1 || theScores[loop, BestWord[loop]] > LevenshteinDistance) BestWord[loop] = loop1;
                }
            }

            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                if (theScores[loop, BestWord[loop]] == 1000) continue;
                for (int loop1 = loop + 1; loop1 < Splitted1.Length; loop1++)
                {
                    if (theScores[loop1, BestWord[loop1]] == 1000) continue;//the worst score available, so there are no more words left
                    if (BestWord[loop] == BestWord[loop1])//2 words have the same best word
                    {
                        //The first in order has the advantage of keeping the word in equality
                        if (theScores[loop, BestWord[loop]] <= theScores[loop1, BestWord[loop1]])
                        {
                            theScores[loop1, BestWord[loop1]] = 1000;
                            int CurrentBest = -1;
                            int CurrentScore = 1000;
                            for (int loop2 = 0; loop2 < Splitted2.Length; loop2++)
                            {
                                //Find next bestword
                                if (CurrentBest == -1 || CurrentScore > theScores[loop1, loop2])
                                {
                                    CurrentBest = loop2;
                                    CurrentScore = theScores[loop1, loop2];
                                }
                            }
                            BestWord[loop1] = CurrentBest;
                        }
                        else//the latter has a better score
                        {
                            theScores[loop, BestWord[loop]] = 1000;
                            int CurrentBest = -1;
                            int CurrentScore = 1000;
                            for (int loop2 = 0; loop2 < Splitted2.Length; loop2++)
                            {
                                //Find next bestword
                                if (CurrentBest == -1 || CurrentScore > theScores[loop, loop2])
                                {
                                    CurrentBest = loop2;
                                    CurrentScore = theScores[loop, loop2];
                                }
                            }
                            BestWord[loop] = CurrentBest;
                        }

                        loop = -1;
                        break;//recalculate all
                    }
                }
            }
            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                if (theScores[loop, BestWord[loop]] == 1000) theResult += Splitted1[loop].Length;//All words without a score for best word are max failures
                else
                {
                    theResult += theScores[loop, BestWord[loop]];
                    if (theScores[loop, BestWord[loop]] == 0) WordsMatched++;
                }
            }
            int theLength = (FullString1.Replace(" ", "").Length > FullString2.Replace(" ", "").Length) ? FullString1.Replace(" ", "").Length : FullString2.Replace(" ", "").Length;
            if (theResult > theLength) theResult = theLength;
            theResult = (1 - (theResult / theLength)) * 100;
            return theResult;
        }

        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

    }
}
