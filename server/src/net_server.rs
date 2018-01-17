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
use std::rc::Rc;

struct Connect{
addr: TcpStream,
id: u32,
}
trait con {
fn new(stream: TcpStream, id: u32)->Connect;
}
impl con for Connect{
 fn new(stream: TcpStream, id: u32)->Connect{
	Connect{addr:stream, id:id}
}
}
pub fn start_game(){
	
   }
pub fn start_game_handle(){
	let mut i:usize = 0;
	println!("Макс. кол-во игроков:");
	let mut number_player = String::new();

	let mut wait_operation = String::new();
	
	io::stdin().read_line(&mut number_player)
      	.unwrap();

	let number_player: u32 = number_player.trim().parse().unwrap();
	
	
	println!("Количество итераций для проверки (рекомендую ставить от 15 до 100): ");

	io::stdin().read_line(&mut wait_operation).unwrap();

	let wait_operation: u64 = wait_operation.trim().parse().unwrap();

	/*
			Приняли(1) ->отправили(2)
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
	let (sender, receiver) = mpsc::channel();	
	//let(sen_, recv_) = mpsc::channel();

	let mut Connects:Vec<TcpStream> = Vec::new();
	let mut k = 0;
	for i in 0..number_player {
		//принимаем каждого последовательно
	println!("Принимаю клиента номер:[{}]", i+1);
	match listener.accept(){
		Ok((mut stream, addr)) => { 
			
			addrs.push(addr);	
					
			Connects.push(stream);	
		},
		Err(e) => {  },
	}}			

		let mut Connects_copy:Vec<TcpStream> = Vec::new();
		
		{ let mut i:usize = Connects.len() - 1; loop {
		
		match Connects[i].try_clone() { 
				Ok(mut srm) => { Connects_copy.push(srm); },
				Err(e) => { Connects[i].shutdown(Shutdown::Both); Connects.remove(i); },				
			}
		
		if i != 0{
		i -= 1; } else { break; } 
		}}
		println!("{:?}", Connects);

		for mut item in Connects_copy{ 
			let sender_clone = mpsc::Sender::clone(&sender);
			thread::spawn(move ||{			
			let q:[u8;128] = [0;128];			
			let mut buf:[u8; 256] = [0; 256]; 
			loop { 
					item.read(&mut buf); println!("Принимаем сообщения [{:?}]", item);
					if buf.starts_with(&q) == false { sender_clone.send(buf).unwrap(); }
					item.peer_addr().unwrap();
			}
			});
		}

		for item_ in receiver{ println!("Отправляем сообщение");
			let mut Connects_copy_:Vec<TcpStream> = Vec::new();
			for i in 0..Connects.len(){				
				Connects_copy_.push(Connects[i].try_clone().expect("Клиент упал"));//тут делать проверку и удалять адреса, а если их
															 //совсем нету - паниковать нафиг!
			}

			for mut item in Connects_copy_{ 
				let (sender_, recv_) = mpsc::channel(); sender_.send(item_.clone()).unwrap();
				thread::spawn(move ||{			
					let s = recv_.recv().unwrap();
					item.write(&s);
					println!("{:?}", item.peer_addr());
					item.peer_addr().unwrap();
				});
		}  }
   }

fn send_tcp(mut c:Vec<TcpStream>, m: String){
  for _i in 0..c.len(){
	c[_i].write(&m.clone().into_bytes());
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