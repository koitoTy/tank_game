pub mod digit{

pub fn is_digit(c:char)->bool{
 let mut ret = true;
 match c{
	'0'=>{},
	'1'=>{},
	'2'=>{},
	'3'=>{},
	'4'=>{},
	'5'=>{},
	'6'=>{},
	'7'=>{},
	'8'=>{},
	'9'=>{},
	_ =>{ret = false},
	} ret
    }

pub fn string_to_u64<'a>(word_: & 'a str)->u64{	
	let mut word:String = word_.to_string();
	let mut chars = word.chars();
	let mut ch:Vec<char> = Vec::new();
	let mut i = word.len();
	let mut retur:u64 = 0;
	let mut s:u64 = 0;
	
	let mut k = 0;
	loop{	
		match chars.next(){
	
	Some(A)=>{ch.push(A);}, _=> {break;},
	
		}
	
        }	
	 let mut first = true;
	loop{ 
		if i == 0 { break;} else{i -= 1; s = return_u64(ch[i]);
	if first == false{ for _ in 0..k{s = s*10;} retur +=s; } else {retur +=s; first = false; } k+=1;
	} 	
      } retur
    }   
pub fn return_u64(u:char)->u64{
match u{	
	'0'=>{0},
	'1'=>{1},
	'2'=>{2},
	'3'=>{3},
	'4'=>{4},
	'5'=>{5},
	'6'=>{6},
	'7'=>{7},
	'8'=>{8},
	'9'=>{9},
	_ =>{0},
   }
 }
}