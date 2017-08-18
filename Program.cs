using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace MacToIP
{
    class Program
    {
       
        static void Main(string[] args)
        {
           
            string patternIP = @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b";
            string patternMac = @"([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})";
            string ipAddress = string.Empty;
            string MacAddress = string.Empty;
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "arp.exe";
            p.StartInfo.Arguments = "-a";
            p.Start();

            string output = p.StandardOutput.ReadToEnd();

            
            p.WaitForExit();

            File.WriteAllText("temp.txt", output);

            string[] lines = File.ReadAllLines("temp.txt");

            Regex rgx = new Regex(patternIP,RegexOptions.None);
            Regex rgxMac = new Regex(patternMac, RegexOptions.None);

            DataTable dt = new DataTable();
            dt.Columns.Add("ipAddress");
            dt.Columns.Add("MacAddress");
            for (int i = 3; i < lines.Length; i++)
            {
                Match match = rgx.Match(lines[i]);
                if(match.Success)
                {
                    Console.WriteLine(match.Value);
                    ipAddress = match.Value;
                }
                Match matchMac = rgxMac.Match(lines[i]);
                if(matchMac.Success)
                {
                    Console.WriteLine(matchMac.Value);
                    MacAddress = matchMac.Value;
                }
                if(lines[i] == "")
                {
                    return;
                }
                dt.Rows.Add(ipAddress, MacAddress);
            }
            File.Delete("temp.txt");

        }
    }
}
