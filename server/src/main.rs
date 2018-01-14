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

fn main() {
net_server::net_server::start_game_handle();
}