using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_v0._1_alpha
{
    public class Syntax
    {

        public static string Trimming(string arg, char argMinus)
        {
            char[] argChar = arg.ToCharArray();
            string argNew = null;
            for (int i = 0; i < argChar.Length; i++)
            {
                if (argChar[i] != argMinus)
                    argNew = argNew + argChar[i].ToString();
            }

            return argNew;
        }

        public static string Read_text_out_File(string Path_toFile) //при ошибке вернётся текст с сообщением об этой ошибкой
        {
            string argReturn = null;
            try
            {
                try
                {
                    FileStream file = new FileStream(Path_toFile + ".txt", FileMode.Open, FileAccess.Read); //открываем файл только на чтение
                    StreamReader reader = new StreamReader(file); // создаем реадер(считыватель потоков) и связываем его с файловым потоком 
                    argReturn = reader.ReadToEnd();//читаем все данные с потока и пишем в переменную
                    reader.Close(); //закрываем поток    

                }
                catch
                {
                    FileStream fil1e = new FileStream(Path_toFile, FileMode.Open, FileAccess.Read); //открываем файл только на чтение
                    StreamReader reade1r = new StreamReader(fil1e); // создаем реадер(считыватель потоков) и связываем его с файловым потоком 
                    argReturn = reade1r.ReadToEnd();//читаем все данные с потока и пишем в переменную
                    reade1r.Close(); //закрываем поток  
                }
            }
            catch (Exception ex) { string exString = ex.Message + " in " + ex.TargetSite; return exString; }
            return argReturn;
        }
    }
}
