using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnTP_CSharp
{
     class Program
    {
        static void Main(string[] args)
        {
            utp t = new utp(8080, "127.0.0.1");
            t.send();
        }
    }

    class utp
    {
        private int port { get; set; }
        private string ip { get; set; }
        private data data { get; set; }

        public utp(int port_, string ip_) { port = port_; ip = ip_; data = new data(false, new int[9], 404, "bad connect!"); }

        internal void send()
        {
            int[] buf = new int[9];

            UdpClient socket = new UdpClient();
            IPEndPoint IpPoint = new IPEndPoint(IPAddress.Parse(ip), port);

             try
            {               

                // Преобразуем данные в массив байтов
                byte[] bytes = data.data_;

                // Отправляем данные
                socket.Send(bytes, bytes.Length, IpPoint);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                // Закрыть соединение
                //socket.Close();
            }
        
        }

        internal void recv()
        {

        }
    }
    class data
    {
        public bool connect { get; set; }
        public byte[] data_ { get; set; }
        public int ErrCode { get; set; }
        public string ErrText { get; set; }

        public data(bool con, byte[] data__, int ErrCode_, string ErrText_)
        {
            connect = con; data_ = data__; ErrCode = ErrCode_; ErrText = ErrText_;
        }
    }
}
