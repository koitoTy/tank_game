pub mod net_server{
use std::io;
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


pub fn start_game(){
	
   }
pub fn start_game_handle(){
	let mut i:usize = 0;
	println!("Макс. кол-во игроков:");
	let mut number_player = String::new();

 	//io::stdin().read_line(&mut number_player)
      //	.unwrap();

	
	io::stdin().read_line(&mut number_player)
      	.unwrap();

	let number_player: u32 = number_player.trim().parse().unwrap();
	
	
	/*
			Приняли(1) ->отправили(2) ->наладили отправку через выделенный порт(3)
	*/
		
	let mut addrs:Vec<SocketAddr> = Vec::new();

	println!("Введите IP:PORT сервера:");
	let mut ip_port = String::new();	
	
	io::stdin().read_line(&mut ip_port)
      	.unwrap();
	
	ip_port = slim(ip_port, ' ');
	ip_port = slim(ip_port, '\n');
	ip_port = slim(ip_port, '\r');
	ip_port = slim(ip_port, '\0');
	
	println!("{:?}",ip_port);
	println!("Введите IP:PORT гейм-сервера(+{} будет добавлено):",number_player);
	let mut game_port = String::new();	
	
	io::stdin().read_line(&mut game_port)
      	.unwrap();
	game_port = slim(game_port, ' ');
	game_port = slim(game_port, '\n');
	game_port = slim(game_port, '\r');
	game_port = slim(game_port, '\0');
	let _port = slim_vec(game_port.clone(), ':');// второй элемент - это наш порт
	// а теперь будем прибавлять к порту 
	let _port: u32 = _port[1].trim().parse().unwrap();
	
	
	//let game_port: u32 = game_port.trim().parse().unwrap();
	
	let mut exit_id: Vec<u32> = Vec::new(); // вектор хранящий внутри id тех, кто должен покинуть игру
	
	println!("[Запускаю сервер!]");
	let listener = TcpListener::bind(ip_port.as_str()).unwrap();	
	println!("{:?}", listener);
	let (sender, receiver) = mpsc::channel::<String>();
	//let(sen_, recv_) = mpsc::channel();
	
	for i in 0..number_player {
		//принимаем каждого последовательно
	println!("Принимаю клиента номер:[{}]", i+1);
	match listener.accept(){
		Ok((mut stream, addr)) => { 				
			addrs.push(addr);	
			let sender_clone = mpsc::Sender::clone(&sender);
			let id = addrs.len() - 1;
			thread::spawn(move ||{
				//let id = id;
				/* тут мы передаём все данные */
				thread::spawn(move || { 
						/*а тут читаем данные*/
					let mut buf:[u8; 256] = [0; 256];
					let q:[u8; 8] = [0; 8]; 
					/* 	read	 */
					loop {
						stream.read(&mut buf).is_ok(); // -> bool
						if buf.starts_with(&q) == false { 
							sender_clone.send(String::from_utf8(buf.to_vec()).unwrap()).unwrap(); 
						}
					}
					/*receiver*/
				}); 
				loop{					
					/*
						receiver
							sender
					*/
				
				}	
			});
		},
		Err(e) => {  },
	}}	
	for item in receiver {
		/* если время ожидание истекло и сервер запущен - > говорим адреса*/	
		// if start == true {
			/*send_.send(item).unwrap();
			thread::spawn(move ||{
				for it in recv_{
					
				}
			});*/
		// }
	}
   }
pub fn slim(arg1: String, arg2: char)->String{

	let mut arg_return:String = String::new();
	
	for item in arg1.chars()
	{ if item != arg2 {arg_return.push(item);} }
	arg_return
}

pub fn slim_vec(arg1: String, dementer: char) -> Vec<String>{
	let mut arg_ = arg1;
	arg_.push(dementer);
	let mut ret:Vec<String> = Vec::new();
	let mut st = String::new();
	for item in arg_.chars(){
		if item != dementer { st.push(item); }
		else { ret.push(st); st = String::new(); }
	}
	ret		
}
}