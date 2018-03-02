mod utp{
use std::io;
use std::io::prelude::*;
use std::net::SocketAddr;
use std::net::UdpSocket;
use std::net::{IpAddr, Ipv4Addr};
use std::thread;
use std::sync::mpsc;
use std::fs::File;
use std::time::Duration;
use std::mem;
use std::rc::Rc;

pub struct utp{
	port: u16,
	ip: String,/*порт и ip конечного компа (клиента)*/	
    data: data,
}


pub struct data{
	connect: bool,
	data: [u8; 9],
	err_code: u16, 
	err_text: String,
/* 
0 - non error	
1 - bad message
2 - bad server
404 - bad connect
405 - bad connect or bad message
*/	
}


impl data{
	fn error(&self)->u16{
		self.err_code
	}
	
	fn buf(&self)->[u8; 9]{
		self.data
	}
	fn drop(self){}
}


impl utp{

	fn ping(&self)->bool{
		
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();
				
		
		let data:[u8; 9] = [2,99,0,8,8,8,8,1,101];
		
		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (101)
			

		
		let socket = match UdpSocket::bind("127.0.0.1:8081"){		
			Ok(A) => {A},
			Err(Er) => {return false;},	
	};		
		let a = socket.connect(ip.to_string()+":"+port).is_ok();

		if a == false {
			return false;
        }; 
	
		let a = socket.send(&data).is_ok();		
		thread::sleep(Duration::from_millis(1));
		
		let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
        
		if buf[8] == 101 { return true;} else { return false; } 
		 true
	}
		
	
	
	fn test_connect(&self)->bool{	    
	    
	    let ip: &str = &*self.ip;
	    let port: &str = &*self.port.to_string();
	
          let socket = match UdpSocket::bind("127.0.0.1:8080"){		
			Ok(A) => {A},
			Err(Er) => {return false;},	
	    };		
	
	     socket.connect(ip.to_string()+":"+port).is_ok()	    
	}

	fn clone(&self)->utp{
		utp{ port: self.port, ip: self.ip.to_string(), data: data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()} }
	}

	
	fn recv(&self)->data{    
	    
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();

		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (101)
		let b_q: [u8; 7] = [0; 7];
		
		
		
		
		let socket = match UdpSocket::bind("127.0.0.1:8080"){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: buf, err_code: 2, err_text: "bad server!".to_string()}; 
						UdpSocket::bind("127.0.0.1:8080").unwrap()},	
	};		
	
		let a = socket.connect(ip.to_string()+":"+port).is_ok();

		if a == false {
			return data{connect: false, data: buf, err_code: 404, err_text: "bad connect!".to_string()}; 
		}

	

	let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
		
	if buf[8] == 101 { return data{connect: true, data: buf, err_code: 0, err_text: "all ok".to_string()}; }

	data{connect: false, data: buf, err_code: 1, err_text: "bad message".to_string()}
    }


	fn send(&self, data: [u8; 9])->data{// данные, сколько ждём ответа, количество запросов
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();
		
		let b_q: [u8; 7] = [0; 7];
        
		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (101)
			

		let mut y = false;
		let socket = match UdpSocket::bind("127.0.0.1:8081"){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: [0; 9], err_code: 2, err_text: "bad server!".to_string()}; 
						UdpSocket::bind("127.0.0.1:1000").unwrap()},	
	};		
		let a = socket.connect(ip.to_string()+":"+port).is_ok();

		if a == false {
			return data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()}; 
		}
		
	
		let a = socket.send(&data).is_ok();		
		thread::sleep(Duration::from_millis(1));
        
        let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
        if buf[8] == 101 { y = true;}
        
        if y == false { return data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()}; }
		 data{connect: true, data: buf, err_code: 0, err_text: "all ok".to_string()}		
    }
	fn clear(&self)->utp{
		utp{port: 0, ip: "".to_string(), data: data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()} }
	}
	
}
   pub fn new(port: u16, ip: String)->utp{
		utp{ port: port, ip: ip, data: data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()} }		
	}
   pub fn drop(a: utp){}
}