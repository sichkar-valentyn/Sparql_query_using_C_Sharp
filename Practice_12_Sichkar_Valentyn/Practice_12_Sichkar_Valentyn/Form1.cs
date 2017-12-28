// File: Form1.cs
// Description: Sparql query programming using C#
// Environment: Visual Studio 2015, C#
//
// MIT License
// Copyright (c) 2017 Valentyn N Sichkar
// github.com/sichkar-valentyn
//
// Reference to:
//[1] Valentyn N Sichkar. Sparql query programming using C# // GitHub platform [Electronic resource]. URL: https://github.com/sichkar-valentyn/Sparql_query_using_C_Sharp (date of access: XX.XX.XXXX)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace Practice_12_Sichkar_Valentyn
{
    public partial class Form1 : Form
    {

        private const string Query1 = @"
PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
PREFIX ns0: <http://car-assistant.ru/description#>
SELECT ?name
WHERE {
?name a ns0:AccidentFactors.
}";

        private const string Query2 = @"
PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
PREFIX ns0: <http://car-assistant.ru/description#>
SELECT ?name
WHERE {
ns0:BadWeather ns0:ConsistOf ?name.
}";

        private const string Query3 = @"
PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
PREFIX ns0: <http://car-assistant.ru/description#>
SELECT ?name
WHERE {
?name a ns0:DangerousSituations.
}";

        void sparqgSend(string Factors, string description)
        {
            var parser = new Notation3Parser();
            var graph = new Graph();

            //Console.WriteLine("Loading Notation-3 file.");
            richTextBox1.Text += "Loading Notation-3 file:" + Environment.NewLine;
            richTextBox1.Text += "N3_Sichkar_Valentyn.n3" + Environment.NewLine;
            richTextBox1.Text += Environment.NewLine;
            richTextBox1.Text += Environment.NewLine;
            parser.Load(graph, @"n3\N3_Sichkar_Valentyn.n3");

            //Console.WriteLine("Nodes:");
            richTextBox1.Text += "Nodes:" + Environment.NewLine;
            foreach (Triple triple in graph.Triples)
            {
                richTextBox1.Text += GetNodeString(triple.Subject) + GetNodeString(triple.Predicate) + GetNodeString(triple.Object) + Environment.NewLine;
                //Console.WriteLine("{0} {1} {2}", GetNodeString(triple.Subject), GetNodeString(triple.Predicate), GetNodeString(triple.Object));
            }

            //Console.WriteLine();
            richTextBox1.Text += Environment.NewLine;
            //Console.WriteLine();
            richTextBox1.Text += Environment.NewLine;

            SparqlResultSet resultSet = graph.ExecuteQuery(Factors) as SparqlResultSet;
            if (resultSet != null)
            {
                //Console.WriteLine("Querying results for variable 'name':");
                richTextBox1.Text += description + Environment.NewLine;
                for (int i = 0; i < resultSet.Count; i++)
                {
                    SparqlResult result = resultSet[i];
                    //Console.WriteLine("{0}. {1}", i + 1, result["name"]);
                    richTextBox1.Text += Convert.ToString(i + 1) + ". " + Convert.ToString(result["name"]) + Environment.NewLine;
                }
            }
        }

        static string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    if (lio == -1)
                        return s;
                    else
                        return s.Substring(lio + 1);
                case NodeType.Literal:
                    return string.Format("\"{0}\"", s);
                default:
                    return s;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sparqgSend(Query1, "Accident factors are:");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sparqgSend(Query2, "Bad weather might consist of:");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sparqgSend(Query3, "Dangerous situations are:");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
