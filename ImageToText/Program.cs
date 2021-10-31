using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ImageToText
{
    class Program
    {
        static void Main(string[] args)
        {

            WithColors(args[0]);
   
        }

        static void WithColors(string file)
        {
            Bitmap bmp = new Bitmap(Image.FromFile(file));

            List<string> commands = new List<string>();

            Dictionary<string, string> colorcmds = new Dictionary<string, string>();

            Color c = Color.Black;

            Color active = Color.Transparent;
            string linesize = "";
            string active_id = "";

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    c = bmp.GetPixel(j, i);
                    if (active.R != c.R || active.G != c.G || active.B != c.B)
                    {
                        commands.Add("Console.setBackground(" + active_id + ");");
                        commands.Add("Console.write(\"" + linesize + "\");");
                        linesize = "";
                    }
                    string id = "c" + c.R.ToString() + c.G.ToString() + c.B.ToString();
                    if (!colorcmds.ContainsKey(id))
                    {
                        colorcmds.Add(id, "static Color " + id + " = new Color(" + c.R + "," + c.G + "," + c.B + ");");
                    }
                    active_id = id;
                    active = Color.FromArgb(c.R, c.G, c.B);
                    linesize += " ";
                    
                }
            }

            commands.Add("Console.setBackground(" + active_id + ");");
            commands.Add("Console.write(\"" + linesize + "\");");
            linesize = "";

            File.WriteAllLines("statics.txt", colorcmds.Values);
            File.WriteAllLines("commands.txt", commands);
        }
    
    }
}
