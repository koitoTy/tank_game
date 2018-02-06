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

//#[derive(Copy)]
struct Connect{
stream: TcpStream,
id: usize,
}

impl Connect{
	fn clone(&self) -> Connect{
		let str = match self.stream.try_clone(){
			Ok(mut str)=>{str}, Err(e)=>{ let t = TcpStream::connect("127.0.0.1:80").unwrap(); t}
		};
	   Connect{stream: str, id: self.id}
  	}
}

/*impl Clone for Connect {
    fn clone(&self) -> Connect { *self }  < -- не работает
}*/

trait con {
fn new(stream: TcpStream, id: usize)->Connect;
}
impl con for Connect{
 fn new(stream: TcpStream, id: usize)->Connect{
	Connect{stream:stream, id:id}
}
}
pub fn start_game(){
	
   }

pub fn start_game_handle(){

    let bue_var:&'static [u8; 3] = b"bue";

	let mut i:usize = 0;
	println!("Макс. кол-во игроков:");
	let mut number_player = String::new();

	let mut wait_operation = String::new();
	
	io::stdin().read_line(&mut number_player)
      	.unwrap();

	let number_player: usize = number_player.trim().parse().unwrap();
	
	
	println!("Количество итераций для проверки (рекомендую ставить от 15 до 100): ");

	io::stdin().read_line(&mut wait_operation).unwrap();

	let wait_operation: u64 = wait_operation.trim().parse().unwrap();

	println!("Время в милисекундах для пропинговки (рекомендуется от 2000 до 5000 +-2)");

	let mut ping_time = String::new();
	io::stdin().read_line(&mut ping_time).unwrap();
	
	let ping_time: u64 = ping_time.trim().parse().unwrap();

	/*
			Приняли(1) ->отправили(2)
	*/
		
	//let mut addrs:Vec<SocketAddr> = Vec::new();

	println!("Введите IP:PORT сервера:");
	let mut ip_port = String::new();	
	
	io::stdin().read_line(&mut ip_port)
      	.unwrap();
	
	ip_port = slim(ip_port, ' ');
	ip_port = slim(ip_port, '\n');
	ip_port = slim(ip_port, '\r');
	ip_port = slim(ip_port, '\0');
	
	println!("{:?}",ip_port);
		
	//let mut exit_id: Vec<u32> = Vec::new(); // вектор хранящий внутри id тех, кто должен покинуть игру


	/*
		Реализовать чтение файлов		
	*/
	
	println!("[Запускаю сервер!]");
	let listener = TcpListener::bind(ip_port.as_str()).unwrap();	
	println!("{:?}", listener);
	let (sender, receiver) = mpsc::channel();	
	//let(sen_, recv_) = mpsc::channel();

	let mut Connects:Vec<Connect> = Vec::new();
	
	for i in 0..number_player {
		//принимаем каждого последовательно
	println!("Принимаю клиента номер:[{}]", i+1);
	match listener.accept(){
		Ok((mut stream, addr)) => { 
			
			//addrs.push(addr);
			thread::sleep(Duration::from_secs(1));
			println!("Отправляю файлы на клиента");
			{
				
				
				
			}	
					
			Connects.push(Connect::new(stream, i));	
		},
		Err(e) => {  },
	}}			

		let mut Connects_copy:Vec<Connect> = Vec::new();
		
		{ let mut i:usize = Connects.len() - 1; loop {
		
		match Connects[i].stream.try_clone() { 
				Ok(mut srm) => { Connects_copy.push(Connect{stream: srm,id: i}); },
				Err(e) => { Connects[i].stream.shutdown(Shutdown::Both); Connects.remove(i); },				
			}
		
		if i != 0{
		i -= 1; } else { break; } 
		}}
		//println!("{:?}", Connects);

		for mut item in Connects_copy{ 
			let sender_clone = mpsc::Sender::clone(&sender);
			//id потока тут юзается 
			thread::spawn(move ||{			
			let q:[u8;128] = [0;128];			
			let mut buf:[u8; 256] = [0; 256]; 
			let mut loop_q: u64 = 0;
			loop { 
					loop_q += 1;
				if loop_q >= wait_operation { 
					let (sen_, recv_) = mpsc::channel();
					match item.stream.write(b"you") { 
					Ok(ok) => {}, Err(e) => {/* Выходим из приёма */ break;}
					, };
					thread::spawn(move ||{
					let mut _kl: u64 = 0; 
					loop{ 						
						thread::sleep(Duration::from_millis(1));
						_kl += 1;		
					sen_.send(false).unwrap();	
						if _kl >= ping_time { break; }						
					} sen_.send(true).unwrap(); });

					let mut buf_q:[u8; 256] = [0; 256]; 
					let mut buf_q_else: [u8; 128] = [0; 128];
	
					let mut recv_val = false;
                    let mut b_false_true = false;
					loop {
						item.stream.read(&mut buf_q);
						if buf_q.starts_with(&buf_q_else) { 
							recv_val = recv_.recv().unwrap(); 
							if recv_val == true { 
								b_false_true = true; break;  } }						
					}
					if b_false_true == true { break;}
				}
					item.stream.read(&mut buf); println!("Принимаем сообщения [{:?}]", item.stream);
					if buf.starts_with(&q) == false { sender_clone.send(buf).unwrap(); }
					item.stream.peer_addr().unwrap();
                
			} 
                
              let received_bue = *bue_var;
			  let received_bue:[u8; 3] = received_bue;
			  let mut r_by:[u8; 256] = [0; 256]; 
			  r_by[0] = received_bue[0]; r_by[1] = received_bue[1]; r_by[2] = received_bue[2];
              //r_by[5] = item.id;
                let y = item.id;
                let mut f: u8 = 0;
                for u in 0..number_player{
                    if y == u {                        
                        break;
                    }
                    f += 1;
                }
                r_by[5] = f;
			  
			sender_clone.send(r_by).unwrap();
			});			
		}

		
		for item_ in receiver{ 
			if item_.starts_with(bue_var){/* Тут удаляем адрес */
			    let j = item_[5] as usize;
                {                    
                    Connects[j].stream.shutdown(Shutdown::Both)
                                        .expect("shutdown call failed");                    
                    Connects.remove(j);
                }
			} else {
			let mut Connects_copy_:Vec<TcpStream> = Vec::new();
		{	for i in 0..Connects.len(){				
				Connects_copy_.push(Connects[i].stream.try_clone().expect("Клиент упал"));//тут делать проверку и удалять адреса, а если их
				
                //совсем нету - НЕ паниковать нафиг!
			} }

		println!("Отправляем сообщение");
		let (_sender_, _receiver_) = mpsc::channel();
	match _sender_.send(Connects_copy_) { Ok(ok)=> {}, Err(e) => {}, } 
        	thread::spawn(move ||{
        		let mut Connects_copy_:Vec<TcpStream> = match _receiver_.recv(){ Ok(ok)=> {ok}, Err(e) => {
			let _l:Vec<TcpStream> = Vec::new(); _l  }, };
                
			for mut item in Connects_copy_{ 									
				item.write(&item_);
				println!("{:?}", item.peer_addr());				
			}
            
        });  
            
            } //else
    }
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