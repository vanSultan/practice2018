using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace test_06
{
    public static class CTextEditor
    {
        private static string[] WordParser(string s)
        {
            Char del = ' ';

            return s.Split(del);
        }

        private static int SumLenOfStr(string[] str)
        {
            int sum = 0;

            foreach (string s in str)
                sum += s.Length;

            return sum;
        }

        private static string StringAligner(string[] subStrings, int strWidth)
        {
            if (subStrings.Length == 0)
                return null;

            int cnSpace = strWidth - SumLenOfStr(subStrings);
            int addSpace = 0;
            int cnWord = subStrings.Length;

            StringBuilder tmpString = new StringBuilder();

            if (cnSpace < subStrings.Length - 1)
            {
                foreach (string s in subStrings)
                    tmpString.AppendLine(s);

                return tmpString.ToString();
            }

            for (int i = 0; i < subStrings.Length - 1 && cnSpace > 0; i++, cnWord--, cnSpace -= addSpace)
                // cnSpace - количество пробелов, которые осталось вставить между словами
                // addSpace - количество пробелов, которые вставятся на данном этапе итерации
                // cnWord - количество слов, справа от которых еще нет пробелов
            {
                tmpString.Append(subStrings[i]);

                addSpace = cnSpace / (cnWord - 1);

                for (int j = 0; j < addSpace; j++)
                    tmpString.Append(' ');
            }

            tmpString.Append(subStrings.Last());
            tmpString.Append('\n');

            return tmpString.ToString();
        }

        public static string TextAligner(string srcString, int strWidth)
        {
            if (strWidth <= 0)
                return null;

            StringBuilder resString = new StringBuilder();

            string[] subStrings = WordParser(srcString);

            int curLength = 0;
            int cnWord = 0;

            List<String> tmpStrings = new List<String>();

            int i = 0;

            while (i < subStrings.Length)
            {
                if (subStrings[i].Length > strWidth ||
                    (curLength + subStrings[i].Length) + cnWord <= strWidth)
                    // cnWord в данном месте выступает количеством разрывов в строке
                {
                    tmpStrings.Add(subStrings[i]);

                    curLength += subStrings[i].Length;
                    cnWord++;

                    i++;
                }
                else
                {
                    resString.Append(StringAligner(tmpStrings.ToArray(), strWidth));

                    tmpStrings.Clear();
                    curLength = 0;
                    cnWord = 0;
                }
            }

            if (tmpStrings.Capacity > 0)
                resString.Append(StringAligner(tmpStrings.ToArray(), strWidth));

            return resString.ToString();
        }
    }

    public class CMyInput
    {
        public string src;
        public int strWidth;

        public CMyInput()
        {
            Console.Write("Input text: ");

            src = Console.ReadLine();

            Console.Write("Input string width: ");

            strWidth = int.Parse(Console.ReadLine());
        }

        public CMyInput(string filename)
        {
            using (StreamReader fsrc = new StreamReader(filename))
            {
                src = fsrc.ReadLine();
                strWidth = int.Parse(fsrc.ReadLine());
            }
        }
    }

    public class CMyOutput
    {
        public CMyOutput(string dst)
        {
            Console.WriteLine("Edited text:");

            Console.Write(dst);
        }

        public CMyOutput(string filename, string dst)
        {
            using (StreamWriter fdst = new StreamWriter(filename))
            {
                fdst.Write(dst);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CMyInput inp1 = new CMyInput();
            CMyOutput out1 = new CMyOutput(CTextEditor.TextAligner(inp1.src, inp1.strWidth));

            CMyInput inp2 = new CMyInput("testInp.txt");
            CMyOutput out2 = new CMyOutput("testOut.txt", CTextEditor.TextAligner(inp2.src, inp2.strWidth));

            Console.ReadKey();
        }
    }
}
