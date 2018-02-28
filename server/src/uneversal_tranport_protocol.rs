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
	time: u64,
	part: u8,
}


pub struct data{
	connect: bool,
	data: [u8; 9],
	err_code: u8, 
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
	fn error(&self)->u8{
		self.err_code
	}
	
	fn buf(&self)->[u8; 9]{
		self.data
	}
	fn drop(self){
		// уничтожаем
	}
}


impl utp{

	fn ping(&self)->bool{
		
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();
		let wait_time = self.time;
	      let part = self.part;
		
		
		let data:[u8; 9] = [2,99,0,8,8,8,8,1,1010];
		
		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (1010)
			

		let mut y = false;
		let socket = match UdpSocket::bind("127.0.0.1:8081"){		
			Ok(A) => {A},
			Err(Er) => {return false;},	
	};		
		let a = socket.connect(ip.to_string()+":"+port).is_ok();

		if a == false {
			return false;}; 
	for i in 0..part{
		let a = socket.send(&data).is_ok();		
		thread::sleep(Duration::from_millis(1));
		if y == false {
		 	let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
				if buf[8] == 1010 { y = true;}
		}	
	}	if y == false { return false; } 
		 true
	}
		
	
	
	fn test_connect(&self)->bool{
	    let time = self.time;
	    let part = self.part;
	    
	    let ip: &str = &*self.ip;
	    let port: &str = &*self.port.to_string();
	
          let socket = match UdpSocket::bind("127.0.0.1:8080"){		
			Ok(A) => {A},
			Err(Er) => {return false;},	
	    };		
	
	    let a = socket.connect(ip.to_string()+":"+port).is_ok();
	    a
	}

	fn clone(&self)->utp{
		utp{ port: self.port, ip: self.ip.to_string(), time: self.time, part: self.part }
	}


	fn time(&mut self, t: u64){
        	self.time = t;
    	}
	
	fn recv(&self)->data{
	
	    let time = self.time;
	    let part = self.part;
	    
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();

		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (1010)
		let mut b_q: [u8; 7] = [0; 7];
		
		
		
		
		let socket = match UdpSocket::bind("127.0.0.1:8080"){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: buf, err_code: 2, err_text: "bad server!".to_string()}; 
						UdpSocket::bind("127.0.0.1:8080").unwrap()},	
	};		
	
		let a = socket.connect(ip.to_string()+":"+port).is_ok();

		if a == false {
			return data{connect: false, data: buf, err_code: 404, err_text: "bad connect!".to_string()}; 
		}

	for i in 0..part{

	let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
		
	if buf.starts_with(&b_q)==false{break;}
		thread::sleep(Duration::from_millis(time));
		if i == part - 1 {return data{connect: false, data: buf, err_code: 405, err_text: "bad connect or no message".to_string()}; }
	}

	if buf[8] == 1010 { return data{connect: true, data: buf, err_code: 0, err_text: "all ok".to_string()}; }

	data{connect: false, data: buf, err_code: 1, err_text: "bad message".to_string()}
    }


	fn send(&self, data: [u8; 9])->data{// данные, сколько ждём ответа, количество запросов
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();
		let wait_time = self.time;
	    let part = self.part;
		
		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (1010)
			

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
		
	for i in 0..part{
		let a = socket.send(&data).is_ok();		
		thread::sleep(Duration::from_millis(1));
		if y == false {
		 	let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
				if buf[8] == 1010 { y = true;}
		}	
	}	if y == false { return data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()}; }
		 data{connect: true, data: buf, err_code: 0, err_text: "all ok".to_string()}		
    }
	fn clear(&self)->utp{
		utp{port: 0, ip: "".to_string(), time: 1, part: 10}
	}
	
/*
struct utp{
	port: u16,
	ip: String,
}
*/
}
   pub fn new(port: u16, ip: String)->utp{
		utp{ port: port, ip: ip, time: 1, part: 10 }		
	}
   pub fn drop(a: utp){}
}
