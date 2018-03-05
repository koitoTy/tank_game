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

    class utp {
        private int port { get; set; }
        private string ip{ get; set; }
        private data data { get; set; }
        public utp(int port_, string ip_) { port = port_; ip = ip_; data = new data(false, new int[9], 404, "bad connect!"); }       

        internal void send(){
            
        }
        
        internal void recv(){
            
        }
    }
    class data {
        private bool connect { get; set; }
        private int[] data_ { get; set; }
        private int ErrCode { get; set; }
        private string ErrText { get; set; }

        public data(bool con, int[] data__, int ErrCode_, string ErrText_) {
            connect = con; data_ = data__; ErrCode = ErrCode_; ErrText = ErrText_;
        }        
    }    
}
