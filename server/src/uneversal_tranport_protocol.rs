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
	fn error(&self)->u16{
		self.err_code
	}
	
	fn buf(&self)->[u8; 9]{
		self.data
	}
	fn drop(self){}
    fn clone(&self)->data{ data {connect: self.connect, data: self.data, err_code: self.err_code,
        err_text: self.err_text.clone()} }
}
    
    
impl utp{
    
    pub fn server_mode(mut self, connect_count: usize, ping_con: usize){
        //максимальное количество соединений и колво пакетов после которых выполняем пинг

        //let mut socket = UdpSocket::bind(self.local_ip.clone() + ":" + &*self.local_port.to_string());
        
        let mut connect_count_: usize = 0;
        
        let mut ping_con_: usize = 0;
        
        let mut Clients: Vec<utp> = Vec::new();
         
        //let (sender, receiv) = mpsc::channel();
        
        println!("[Ожидаю пакетов..]");
        loop {
            let data = self.recv();
            
            
            //ping_con_ += 1;            
            
            let mut b = false;
            if data.error() == 0 {
                println!("[Принял сообщение!]");
                for i in 0..Clients.len() {                
                    let (sender, receiv) = mpsc::channel();
                    sender.send((Clients[i].clone(), data.clone())).is_ok();
                    thread::spawn(move||{
                    let (client, data) = match receiv.recv(){
                         Ok((A, B)) => (A, B),
                         Err(Error) => (utp{port: 1234, ip:".0.0.0".to_string(), local_port: 1234, local_ip:".0.0.0".to_string()}, data{connect: false, 
                             data:[0u8; 9], err_code: 405, err_text: "dont send client-message on sender thread".to_string()}),
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
                }
                if b {} else if connect_count_ < connect_count {
                    connect_count_ += 1;
                    println!("[О, новый клиент! Принял :3]");
                    Clients.push(self.clone()); 
                }
            } else {println!("Ошибка: {} '{}'", data.error(), data.err_text);}            
        }
    }
    
    fn get_addr_port(&self)->(String, u16){ (self.ip.clone(), self.port) }
    
    pub fn set_local_ip(&mut self, ip: String){ self.local_ip = ip; }
    
    fn ret_local_ip(&self)->String{ self.local_ip.clone() }
    
    pub fn set_local_port(&mut self, port: u16){ self.local_port = port; }
    
    fn ret_local_port(&self)->u16{ self.local_port }
    
	fn ping(&self)->bool{
		
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();
				
		
		let data:[u8; 9] = [2,99,0,8,8,8,8,1,101];
		
		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (101)
			

		
		let socket = match UdpSocket::bind(self.local_ip.clone() + &*self.local_port.to_string()){		
			Ok(A) => {A},
			Err(Er) => {return false;},	
	        };		
		let a = socket.connect(ip.to_string()+":"+port).is_ok();

		if a == false {
			return false;
                }
	    for i in 0..3{
            let a = socket.send(&data).is_ok();		
            thread::sleep(Duration::from_millis(1));
        }
        
        socket.set_read_timeout(Some(Duration::from_secs(10))).unwrap();
        
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
		utp{ port: self.port, ip: self.ip.to_string(),
            local_port: self.local_port, local_ip: self.local_ip.clone()} 
	}

	
	fn recv(&mut self)->data{    
	    
		//let ip: &str = &*self.ip;
		//let port: &str = &*self.port.to_string();

		let mut buf: [u8; 9] = [0; 9];// mes_type, id_mes, id EvType x y z d , control byte (101)
		let b_q: [u8; 7] = [0; 7];




		let socket = match UdpSocket::bind(self.local_ip.clone()+":"+self.port.to_string().as_str()){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: buf, err_code: 2, err_text: "bad server, none connect to client!".to_string()}; 
						UdpSocket::bind("127.0.0.1:8080").unwrap()},	
	        };		

        let (number_byte, ip_addr) = match socket.recv_from(&mut buf){
            Ok((A,B)) => (A,B),
            Err(E) => {return data{connect: false, data: buf, err_code: 2, err_text: "bad server, dont read byte!".to_string()}; 
						(90usize, UdpSocket::bind("127.0.0.1:8080").unwrap().local_addr().unwrap())},
        };
        
        self.ip = ip_addr.to_string();
        
		let a = socket.connect(self.ip.clone()+":"+self.port.clone().to_string().as_str()).is_ok();

		if a == false {
			return data{connect: false, data: buf, err_code: 404, err_text: "bad connect!".to_string()}; 
		}

        /*
            recv() надо переделать, чтобы он возвращал адрес (автоматом записывал
                его в ip) 
        */

		//let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
        let s:& [u8; 1] = &[buf[1]; 1];
        for i in 0..3{
            let a = socket.send(s).is_ok();		
            thread::sleep(Duration::from_millis(1));
        }
		
		/*if ok_q == false { return data{connect: false, data: buf, err_code: 3, err_text: "error reading".to_string()}; }
        */
		if buf[8] == 101 { return data{connect: true, data: buf, err_code: 0, err_text: "all ok".to_string()}; }

        data{connect: false, data: buf, err_code: 1, err_text: "bad message".to_string()}
    }


	fn send(&self, data: data)->data{// данные, сколько ждём ответа, количество запросов
        
		let ip: &str = &*self.ip;
		let port: &str = &*self.port.to_string();	
        
        
		//let b_q: [u8; 7] = [0; 7];
        
		let mut buf: [u8; 1] = [0; 1];// mes_type, id_mes, id EvType x y z d , control byte (101)
			

		let mut y = false;
		let socket = match UdpSocket::bind("127.0.0.1:8081"){		
			Ok(A) => {A},
			Err(Er) => {return data{connect: false, data: [0;9], err_code: 2, err_text: "bad server, none connect to client!".to_string()}; 
						UdpSocket::bind("127.0.0.1:1000").unwrap()},	
		};		
		let a = socket.connect(ip.to_string()+":"+port).is_ok();

		if a == false {
			return data{connect: false, data: [0; 9], err_code: 404, err_text: "bad connect!".to_string()}; 
		}
		
	    for i in 0..3{
            let a = socket.send(&data.buf()).is_ok();		
		    thread::sleep(Duration::from_millis(1));
        }
		let ok_q = socket.recv(&mut buf).is_ok(); // чтобы паники небыло
		if ok_q == false { return data{connect: false, data: [0; 9], err_code: 3, err_text: "error reading".to_string()}; }
		
		// [0] -> id message		
		data{connect: true, data: [buf[0]; 9], err_code: 0, err_text: "all ok".to_string()}			 		
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
