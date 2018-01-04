pub mod net_server{

use std::io::prelude::*;
use std::net::SocketAddr;
use std::net::TcpListener;
use std::net::TcpStream;
use std::net::UdpSocket;
use std::net::Shutdown;
use std::net::{IpAddr, Ipv4Addr};
use std::thread;
use std::sync::mpsc;
use std::fs::File;
use std::time::Duration;
use std::mem;

pub fn send(message:String, addr:SocketAddr){
	let mut stream = TcpStream::connect(addr).unwrap();
	stream.write(message.as_bytes()).unwrap();	
   }
pub fn send_udp(message:String, addr: Vec<SocketAddr>, addrs:& 'static str){
	let mut socket = UdpSocket::bind(addrs).expect("");
	for i in 0..addr.len(){
	socket.send_to(message.as_bytes(), addr[i]).expect("");
     }
   }
pub fn read_udp(addrs: &'static str)-> (String, SocketAddr){
	let mut buf:[u8;1024] = [0;1024];	
	let mut socket = UdpSocket::bind(addrs).expect("");
	let (n, src_addr) = socket.recv_from(&mut buf).expect("");
	drop(n);
	let mut v:Vec<u8> = Vec::new();
		for i in 0..buf.len(){ v.push(buf[i]);}
	(String::from_utf8(v).unwrap(), src_addr)
}

pub fn read_tcp(mut stream: TcpStream)-> String{
	
	let mut buf:[u8;1024] = [0; 1024]; println!("{:?}", stream);
	stream.read(&mut buf);
	let mut v:Vec<u8> = Vec::new();
		for i in 0..buf.len(){ v.push(buf[i]);}
 
	String::from_utf8(v).unwrap()	
}

pub fn send_tcp(message:String, addrs:Vec<String>, addr: &'static str, intr_y: u64){		
for i in addrs{	
let str = message.clone();
let (sender, recve) = mpsc::channel();
let (send, rev) = mpsc::channel();
send.send(i).unwrap(); sender.send(str).unwrap();
thread::sleep(Duration::from_millis(intr_y));
thread::spawn(move ||{ 
let mut stream: String = rev.recv().unwrap();
let mut message = recve.recv().unwrap();
println!("Мы говорим клиентам: {}", message);//пока что будет, в финальной реализации этот println! уйдёт
	let mut srm = TcpStream::connect(stream.as_str()).unwrap();
	srm.write(message.as_bytes());
});	
	}	/*  ПРОВЕСТИ ТЕСТ ЭТОЙ ФУНКЦИИ ГДЕ ФОР СЧИТАЕТ ОТ 1 ДО 70   */
}

pub fn accept_socet_array<'a>(i: usize, addr:& 'a str)-> Vec<SocketAddr>{

let listener = TcpListener::bind(addr).unwrap();

let mut addr:Vec<SocketAddr> = Vec::new();//тут у нас все пришедшие к нам адреса
loop{
    
        if addr.len() < i {

		match listener.accept(){
			Ok((_socket, addrs))=> addr.push(addrs),
			Err(e) => {},
			_=>{},
		}
			println!("\n->Игрок под номером {} подключён!<-", addr.len());
	  } else { break; }
     }	addr
   }

pub fn start_game<'a>(addr: Vec<SocketAddr>, server:& 'static str, ping: & 'a str, min_ping: u64, wait_download_to_ms: u64){	
	println!("{:?}",addr);
	let mut start = Socket_to_string(&addr);
	let (sender, receiver) = mpsc::channel();
	let addr_ = &addr.clone();	
	for i in 0..addr.len(){ 	
	println!("Игрок {} ожидает", addr[i]);
	thread::sleep(Duration::from_millis(wait_download_to_ms));
	send("0".to_string(),addr[i]);	
	}	

		let send = mpsc::Sender::clone(&sender);
		
thread::spawn(move ||{ 	
	let listener = TcpListener::bind(server).unwrap();
	println!("Забиндились и ждём..{:?}", listener);
	let s = mpsc::Sender::clone(&send);	

	loop{
	
	let sen = mpsc::Sender::clone(&s);

	match listener.accept() {
      Ok((stream, addr)) => {

	println!("{:?}", stream);	

	thread::spawn(move||{
	let mut stream = stream;
	println!("Начинаю приём");
	loop{ 

	let mut buf:[u8;256] = [0; 256]; 
	//let qu:[u8; 256] = [0; 256];

	//println!("{:?}", stream);

	stream.read(&mut buf);

	let q = b"movetank";
	let q1 = b"createshot";

      if buf.starts_with(q)||buf.starts_with(q1) {

	let mut v:Vec<u8> = Vec::new();
		for i in 0..buf.len(){ v.push(buf[i]);}
 
	let message:String = String::from_utf8(v).unwrap();	
	println!("К нам пришло: {}", message);
	 sen.send(message).unwrap(); 
	}

	
	}});},

    	Err(e) => println!("couldn't get client: {:?}", e),
	}
	}
	});
	
	for r in receiver{ 
		let (sender_, receiver_) = mpsc::channel();
		let (send_,rec_) = mpsc::channel(); send_.send(start.clone()).unwrap();
		sender_.send(r).unwrap();
		thread::spawn(move ||{
		let s = receiver_.recv().unwrap();
		let f = rec_.recv().unwrap();
		send_tcp(s, f, server, min_ping);});
	}
  }

fn Socket_to_string(Addr: &Vec<SocketAddr>)->Vec<String>{
	//Addr = Addr.to_vec();
	let mut vec_return:Vec<String> = Vec::new();
	for i in 0..Addr.len(){
	vec_return.push(Addr[i].to_string());
	} vec_return
   }
}