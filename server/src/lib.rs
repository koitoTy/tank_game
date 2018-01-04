pub mod net_server;

pub mod digit;

pub mod imput_user{

use std::io;
use std::io::prelude::*;
use std::net::SocketAddr;
use std::net::TcpListener;
use std::net::TcpStream;
use std::thread;
use std::sync::mpsc;
use std::fs::File;

pub fn imput()->usize{
let mut guess = String::new();
println!("Введите число игроков(от 1 до 6)");
println!("Учтите, что комната должна быть полностью заполнена!");
    io::stdin().read_line(&mut guess)
        .expect("Не удалось прочитать строку");
	let mut chars = guess.chars();
	let mut retur:usize = 0;

	match chars.next(){
	Some('1')=> {retur = 1}, 
	Some('2')=> {retur = 2}, 
	Some('3')=> {retur = 3}, 
	Some('4')=> {retur = 4}, 
	Some('5')=> {retur = 5},
	Some('6')=> {retur = 6}, 
	_ => {},
	} retur
    }


pub fn return_text_in_file<'a>(path:& 'a str) -> String{

		let mut file = File::open(path).unwrap();

        	let mut contents = String::new();
        	file.read_to_string(&mut contents).unwrap();
		contents
	}
pub fn return_coordinate_tanks<'a>(path:& 'a str) -> String{
		let mut file = File::open(path).unwrap();

        	let mut contents = String::new();
        	file.read_to_string(&mut contents).unwrap();
		contents		
	}


pub fn imput_waiting()->u64{
let mut guess = String::new();
println!("Введите число секунд ожидания отправки игровых файлов(от 1 до 6)\nЕсли машина игрока медленная - то лучше 6 секунд");
    io::stdin().read_line(&mut guess)
        .expect("Не удалось прочитать строку");
	let mut chars = guess.chars();
	let mut retur:u64 = 0;

	match chars.next(){
	Some('1')=> {retur = 1}, 
	Some('2')=> {retur = 2}, 
	Some('3')=> {retur = 3}, 
	Some('4')=> {retur = 4}, 
	Some('5')=> {retur = 5},
	Some('6')=> {retur = 6}, 
	_ => {},
	} retur
    }
}
pub mod Text {

pub fn trimming(arg1: String, arg2: char)->String{

	let mut arg_return:String = String::new();

	for item in arg1.chars()
	{ if item != arg2 {arg_return.push(item);} }
	arg_return
}

pub fn trimming_vec(arg1:Vec<String>, arg2: char)->Vec<String>{
	let mut arg_return:String = String::new();
	let mut vec_return:Vec<String> = Vec::new();
	for arg in arg1{
	
	for item in arg.chars()
	{ 
		if item != arg2 {arg_return.push(item);} 
	} 
	vec_return.push(arg_return); arg_return = String::new();
	}
	vec_return
}

pub fn split_vec(arg1: String, arg2: char)->Vec<String>{
	let mut arg_ret:Vec<String> = Vec::new();
	let mut str:String = String::new();
	for item in arg1.chars()
	{ if item != arg2 { str.push(item);} else {arg_ret.push(str); str = String::new();} }
	arg_ret
}

}