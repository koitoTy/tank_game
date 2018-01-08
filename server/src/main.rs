use std::io::prelude::*;
use std::net::{IpAddr, Ipv4Addr,SocketAddr,TcpListener,TcpStream,Shutdown};
use std::thread;
use std::sync::mpsc;
use std::time::Duration;

extern crate server_main; 

use server_main::*;

fn main() {
 println!("[Считываю необходимую информацию с текстовиков]"); 
 let mut addrs_string:Vec<String> = Vec::new(); 
 let map:String = imput_user :: return_text_in_file("map.txt");
 let tank_plase:Vec<String> = Text :: trimming_vec(Text :: split_vec ( imput_user :: return_text_in_file("tankplase.txt") , ';'), ';' );
 let mut if_i = true;

 println!("[Всё было успешно считано!] \n{:?}", tank_plase);
	//читаем из файла tankstart.txt в формате UTF8 и через сплит делим танки
/*
	
	тут я доделаю таймер до начала игры

*/
 

 let socket = SocketAddr::new(IpAddr::V4(Ipv4Addr::new(127, 0, 0, 1)), 1010);//сравниваем в сокетах (чтоб повторения небыло)
 let (sender, receiver) = mpsc::channel();
thread::spawn(move|| { 

	let mut port = 8090;
	let listener = TcpListener::bind("127.0.0.1:8080").unwrap();	
	println!("Наш сервер запущен: {:?}", listener.local_addr());
	
	let mut i:usize = 0; //выдаём ID танку

	let s = mpsc::Sender::clone(&sender);	
	loop{
	
	let sen = mpsc::Sender::clone(&s);

	match listener.accept() {

      Ok((stream, addr)) => {		
	port += 1;
	println!("Приняли соединение: [{:?}]: [{}]", addr, i);
	let tanks = tank_plase.clone();
	let (send_i, i_rec) = mpsc::channel();
	if i == 0 {
		send_i.send((map.clone(), tank_plase[0].clone(),i.clone())).unwrap();
	} else { 
		
		let mut message:String = String::new();
		
		let mut k:usize = 0;
		
		for item in tanks { 
			message.insert_str(k, item.as_str());	
			message.push(';');
			if k == i { break; } else { k = message.len(); }
		 }
		println!("{}: {:?}", message, addr);
		send_i.send((map.clone(), message ,i.clone())).unwrap();
		//fn insert_str(&mut self, idx: usize, string: &str)
	}

	thread::spawn(move||{	

	

	let (map_, tank_plase_, id) = i_rec.recv().unwrap();

	sen.send((addr, "0".to_string())).unwrap();

      {
	let c_text:String = format!("{}M{}M{}M{}", map_, tank_plase_, id, port);
	println!("{}", c_text);
	let vector:Vec<String> = vec![addr.to_string()];
	if send_tcp(c_text, vector, 1000) {  } else if i == 0 { if_i = false; } else { i -= 1; }
      }

	let mut buf:[u8;256] = [0;256];
	let mut stream = stream;
	let _s = SocketAddr::new(IpAddr::V4(Ipv4Addr::new(127, 0, 0, 1)), 1010);

	let q = b"movetank";
	let q1 = b"createshot";

	let send_ = b"sender";

	let exit = b"bue";
	let listen = TcpListener::bind(SocketAddr::from(([127,0,0,1], port))).unwrap();

	match listen.accept() {

	Ok((streams, addr)) => {
	let mut streams = streams;
	println!("Клиент: {:?} : [{}] назначен на порт {}",addr, i, port);
	loop{
		
	streams.read(&mut buf).unwrap();	

	if buf.starts_with(send_) { println!("socket {:?} delete", addr); sen.send((addr, "delete".to_string())).unwrap(); buf = [0; 256]; }

      if buf.starts_with(q)||buf.starts_with(q1) {
		println!("Клиент {:?} что-то нам сказал", addr);
	let mut v:Vec<u8> = Vec::new();
		for i in 0..buf.len(){ v.push(buf[i]);}
 
	let message:String = String::from_utf8(v).unwrap();
	
	
	
	
	sen.send((_s, message)).unwrap();	
	
		//(адрес:127.0.0.1:1010, сообщение)
	} else if buf.starts_with(exit) { 
	stream.shutdown(Shutdown::Both).expect("shutdown call failed");
	break;
      }
	}}, 
	
Err(e) => { println!("С клиентом что-то не то, либо ошибка у нас, либо проблемы в пинге: [{:?}]", e); } ,}
	
	println!("Клиент номер: [{}] попрощался с нами", id);
	});  if if_i { i += 1; } else { if_i == true; }
	},
	Err(e) => { println!("С клиентом что-то не то, либо ошибка у нас, либо проблемы в пинге: [{:?}]", e);}, }
	
 }});


	for receiv in receiver{

	

	if receiv.0 != socket {
	addrs_string.push(receiv.0.to_string());}	//адекватно работает [v4(127.0.0.1,7845)]:["127.0.0.1:7845"] или т.п.
	println!("Содержимое адресов: {:?}", addrs_string);	//временно
	
	if receiv.1 != "0".to_string()&&receiv.1 != "delete".to_string() { 
	let clone_addrs = addrs_string.clone();
	thread::spawn(move||{ 
		send_tcp(receiv.1, clone_addrs, 1);
	});
	} else if receiv.1 == "delete".to_string() { 
		
		let mut kl:Vec<usize> = Vec::new(); 		
		for i in 0..addrs_string.len(){ 	
			if receiv.0.to_string() == addrs_string[i] { kl.push(i); }
		}
			let mut lengh:usize = kl.len() - 1;
		loop {
			addrs_string.remove(kl[lengh]);
			if lengh == 0 { break;} 
			lengh -= 1;			
		}			
      }
 }
}


fn send_tcp(message:String, addrs:Vec<String>, intr_y: u64)-> bool{	
let mut b = true;	
for i in addrs{	
let str = message.clone();
let (sender, recve) = mpsc::channel();
let (send, rev) = mpsc::channel();
send.send(i).unwrap(); sender.send(str).unwrap();
thread::sleep(Duration::from_millis(intr_y));
thread::spawn(move ||{ 
let stream: String = rev.recv().unwrap();
let message = recve.recv().unwrap();
println!("Передаю данные {}", stream.clone());
println!("Мы говорим клиентам: {}", message);//пока что будет, в финальной реализации этот println! уйдёт
	let mut srm = match TcpStream::connect(stream.as_str()){ 
Ok(mut srm) => {
if srm.write(message.as_bytes()).is_ok() == false { b = false; panic!("Кто-то отключился");}; }, 
Err(e) => { b = false; panic!("Кто-то отключился"); },};	
});	
	}	/*  Всё работает как надо!   */
	b
}
