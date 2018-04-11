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
//use std::mem;
//use std::rc::Rc;

pub struct utp{
	port: u16,
	ip: String,/*порт и ip конечного компа (клиента)*/	
    
    local_port: u16,
    local_ip: String,
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
3 - error reading

404 - bad connect
405 - bad connect or bad message
*/	
}


impl data{
	pub fn error(&self)->u16{
		self.err_code
	}
	
	pub fn buf(&self)->[u8; 9]{
		self.data
	}
	pub fn drop(self){}
    pub fn clone(&self)->data{ data {connect: self.connect, data: self.data, err_code: self.err_code,
        err_text: self.err_text.clone()} }
}
    
    
impl utp{
    
    pub fn server_mode(mut self, connect_count: usize, ping_con: usize, delete: bool){
        //максимальное количество соединений и колво пакетов после которых выполняем пинг, 
        //удалять ли косяковых клиентов

        //let mut socket = UdpSocket::bind(self.local_ip.clone() + ":" + &*self.local_port.to_string());
        
        let mut connect_count_: usize = 0; // колво соединений        
        let mut ping_con_: usize = 0;      // колво сообщений
        let mut Clients: Vec<utp> = Vec::new();
        //if self.ping() == false {println!("KOCYAK");}
       /* println!("Начинаю пинговать..");
        if self.ping() {
            // что-то тут если удалось
        } else {
            // и тут если не удалось
            println!("пинговка не удалась");
        }*/
        println!("[Запускаю операцию приёма сообщений..]");
        loop {
            if ping_con != 0 { ping_con_ += 1; }
            let data = self.recv();        
            let mut b = false;
            if data.error() == 0 {
                //println!("[Принял сообщение!]");
                for i in 0..Clients.len() {                
                    let (sender, receiv) = mpsc::channel();
                    sender.send((Clients[i].clone(), data.clone())).is_ok();
                    thread::spawn(move||{
                    let (client, data) = match receiv.recv(){
                         Ok((A, B)) => (A, B),
                         Err(Error) => (utp{port: 1234, ip:".0.0.0".to_string(), local_port: 1234, local_ip:".0.0.0".to_string()}, data{connect: false, 
                             data:[0u8; 9], err_code: 405, err_text: "error 'sender' thread".to_string()}),
                        };
                        if client.ip.as_str() != ".0.0.0"{
                            let r_data = client.send(data);
                            if r_data.error() != 0 {
                                    println!("Клиент {} не смог принять сообщение, код ошибки: '{}'", client.ip, r_data.error());
                            }
                        } else {
                            println!("Ошибка на стороне сервера, код ошибки {}", data.error());
                        }
                    });
                    if self.ip == Clients[i].ip.clone() {
                        b = true;
                    }
                    if ping_con_ >= ping_con { 
                        ping_con_ = 0;
                        if Clients[i].ping() == false && delete {
                            println!("[Клиент '{}' упал, отключаю]", Clients[i].ret_addr_port().0);
                            Clients.remove(i);
                        } 
                    }
                }
                if b {} else if connect_count_ < connect_count {
                    connect_count_ += 1;
                    println!("[О, новый клиент! Принял :3]");
                    Clients.push(self.clone()); 
                    println!("[Клиентов всего: {}]",Clients.len());
                }
            } else { println!("Ошибка: {} '{}'\nКлиент: {:?}", data.error(), 
                data.err_text, self.ret_addr_port()); }            
        }
    }
    
    pub fn ret_addr_port(&self)->(String, u16){ (self.ip.clone(), self.port) }
    
    pub fn set_local_ip(&mut self, ip: String){ self.local_ip = ip; }
    
    pub fn ret_local_ip(&self)->String{ self.local_ip.clone() }
    
    pub fn set_local_port(&mut self, port: u16){ self.local_port = port; }
    
    pub fn ret_local_port(&self)->u16{ self.local_port }
    
    pub fn ping(&self)->bool{
		
		let ip: String = self.ip.clone();
		let port: &str = &*self.port.to_string();
				
		
		let data:[u8; 9] = [2,99,0,8,8,8,8,1,101];
		
		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (101)
			

		
		let socket = match UdpSocket::bind(self.local_ip.clone() +":" + &*self.local_port.to_string()){		
			Ok(A) => {A},
			Err(Er) => {
                println!("Неудачный бинд"); return false; 
                UdpSocket::bind(self.local_ip.clone() +":" + &*self.local_port.to_string()).unwrap()
            },	
	    };
        socket.connect(ip.to_string()+":"+port).is_ok()
		//let a = socket.connect(ip.to_string()+":"+port).is_ok();

		/*if a == false {
            println!("Неудачный коннект");
			return false;
            }*/
	    /*for i in 0..3{
            let a = socket.send_to(&data, self.local_ip.clone() + ":" + &*self.local_port.to_string()).is_ok();	
//            thread::sleep(Duration::from_millis(1));
        }
        
        match socket.set_read_timeout(Some(Duration::from_millis(100000))) {
            Ok(a)=>{},
            Err(e)=> {println!("Ошибка: '{:?}'", e); return false;},
        }
        println!("жду..");
        
        match socket.recv_from(&mut buf) {
            Ok(A) => {
                println!("{:?}", buf);
                true
            },
            Err(err) => {*//*println!("[Ошибка при пинговке: {:?} \n(Kлиент: '{}' Порт: '{}')]", err, self.ip.clone(), port);*/ //false},
        //} // чтобы паники небыло	
    }
		
	
	
//	fn test_connect(&self)->bool{	    
//	    
//	    let ip: &str = &*self.ip;
//	    let port: &str = &*self.port.to_string();
//	
//            let socket = match UdpSocket::bind("127.0.0.1:8080"){		
//			Ok(A) => {A},
//			Err(Er) => {return false;},	
//	    };		
//	
//	     socket.connect(ip.to_string()+":"+port).is_ok()	    
//	}

    pub fn clone(&self)->utp{
		utp{ port: self.port, ip: self.ip.to_string(),
            local_port: self.local_port, local_ip: self.local_ip.clone()} 
	}

	
    pub fn recv(&mut self)->data{    
	    
		//let ip: &str = &*self.ip;
		//let port: &str = &*self.port.to_string();

		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (101)
		let b_q: [u8; 7] = [0; 7];




		let socket = match UdpSocket::bind(self.local_ip.clone()+":"+self.local_port.to_string().as_str()){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: buf, err_code: 2, err_text: "bad server, none bind to client!".to_string()}; 
						UdpSocket::bind("127.0.0.1:8080").unwrap()},	
	        };		

        let (number_byte, ip_addr) = match socket.recv_from(&mut buf){
            Ok((A,B)) => (A,B),
            Err(E) => {return data{connect: false, data: buf, err_code: 2, err_text: "bad server, dont read byte!".to_string()}; 
						(90usize, UdpSocket::bind("127.0.0.1:8080").unwrap().local_addr().unwrap())},
        };
        
        self.ip = ip_addr.to_string();
        
		/*let a = socket.connect(self.ip.clone()+":"+self.port.clone().to_string().as_str()).is_ok();

		if a == false {
			return data{connect: false, data: buf, err_code: 404, err_text: "bad connect!".to_string()}; 
		} else*/ if buf[8] == 101 { return data{connect: true, data: buf, err_code: 0, err_text: "all ok".to_string()}; }
        /*
            recv() записывает адрес отправителя в ip 
        */
		//let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
        let s:& [u8; 1] = &[buf[1]; 1];
        for i in 0..3{
            let a = socket.send(s).is_ok();		
//            thread::sleep(Duration::from_millis(1));
        }		
		/*if ok_q == false { return data{connect: false, data: buf, err_code: 3, err_text: "error reading".to_string()}; }
        */		
        data{connect: false, data: buf, err_code: 1, err_text: "bad message".to_string()}
    }


    pub fn send(&self, data: data)->data{// данные, сколько ждём ответа, количество запросов
        
		let ip = self.ip.clone();
		let port: &str = &*self.port.to_string();	
        
        
		//let b_q: [u8; 7] = [0; 7];
        
		let mut buf: [u8; 1] = [0; 1];// mes_type, id_mes, id EvType x y z d , control byte (101)
			

		let mut y = false;
		let socket = match UdpSocket::bind(self.local_ip.clone() + ":" + &*self.local_port.to_string()){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: [0;9], err_code: 2, err_text: "bad server, none connect to client!".to_string()}; 
						UdpSocket::bind("127.0.0.1:1000").unwrap()},	
		};		
		//let a = socket.connect(ip.to_string()+":"+port).is_ok();

//		if a == false {
//			return data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()}; 
//		}
		
	    //for i in 0..3{
            if socket.send_to(&data.buf(), ip.clone() + ":" +port).is_ok(){
                y = true;
            }		
		  //  thread::sleep(Duration::from_millis(1));
        //}
		/*let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
		if ok_q == false { return data{connect: false, data: [0; 9], err_code: 3, err_text: "error reading".to_string()}; } */
		
		// [0] -> id message
        if y {
		  data{connect: true, data: [buf[0]; 9], err_code: 0, err_text: "all ok".to_string()}
        } else {
            data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()}
        }			 		
	}/*
	fn clear(&self)->utp{
		utp{port: 0, ip: "".to_string(), data{connect: false, data: buf, err_code: 2, err_text: "bad server, none connect to client!".to_string()} }
	}*/
	
}
   pub fn new(port: u16, ip: String)->utp{
		utp{ port: port, ip: ip, local_port: port, local_ip: "127.0.0.1".to_string() } 		
	}
   pub fn drop(a: utp){}
}
