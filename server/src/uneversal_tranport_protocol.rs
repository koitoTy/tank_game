pub mod utp{
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

struct utp{
	port: u16,
	ip: String,
}

enum er{
	r,// result
}

trait new_connection{
	fn new(port: u16, ip: String)->utp;
}

struct data{
	connect: bool,
	data: [u8; 20],
	err_code: u8, // 0 - non error	404 - bad connect
	err_text: String,
}

impl utp{
	fn recv(&self, time: u64, part: u8)->data{
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();

		let mut buf: [u8; 20] = [0; 20];
		let mut b_q: [u8; 15] = [0; 15];
		
		let socket = match UdpSocket::bind(ip.to_string()+":"+port){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: buf, err_code: 404, err_text: "bad connect!".to_string()}; UdpSocket::bind("127.0.0.1:1000").unwrap()},	
	};		
		
	for i in 0..part{

	let ok_q = socket.recv_from(&mut buf).is_ok(); 
		
	if buf.starts_with(&b_q)==false{break;}
		thread::sleep(Duration::from_secs(time));
		if i == part - 1 {return data{connect: false, data: buf, err_code: 404, err_text: "bad connect!".to_string()}; }
	}
	data{connect: true, data: buf, err_code: 0, err_text: "all ok".to_string()} 
  }
}

impl new_connection for utp{
	fn new(port: u16, ip: String)->utp{
		utp{ port: port, ip: ip }		
	}
   }
}