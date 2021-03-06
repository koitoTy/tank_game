/*
use std::io::prelude::*;
use std::net::{IpAddr, Ipv4Addr,SocketAddr,TcpListener,TcpStream,Shutdown};
use std::thread;
use std::sync::mpsc;
use std::time::Duration;
*/
extern crate server_main; 

//use server_main::*;
use server_main::net_server;
use server_main::uneversal_tranport_protocol::utp;
use std::io;
use std::thread;
fn main() {
    //net_server::net_server::start_game_handle();
    start_game_on_utp();
}

fn start_game_on_utp(){
    let mut utp = utp::new(8081,"127.0.0.1".to_string());
    // port, ip (ip and port клиента)
    utp.set_local_ip("127.0.0.1".to_string());
    utp.set_local_port(870);
//    устанавливаем ip и port для сервера
    println!("Сервер запущен на протоколе utp, жду соединений..");
    thread::spawn(move||{
        utp.server_mode(10, 1, true);
//        *************************************************************************
//        |первый аргумент - колво соединений, второй: количесво принятых сообщений|
//        |после которых будет пинговка клиента,                                   |
//        |третий: удалять ли непингуемых клиентов                                 |
//        *************************************************************************
//        -------------------------------------------------------------------------------------------
//        |при нуле пинговка (а точнее проверка соединения (метод connect() UdpSocket) не выполняется|
//        -------------------------------------------------------------------------------------------
    });
    let mut gues = String::new();
    io::stdin().read_line(&mut gues).expect("Не удалось запустить сервер..");
}