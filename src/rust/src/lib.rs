#![allow(dead_code)]

pub struct LookBehindPool<T>
{
	capacity: u64,    
	pool: Vec<T>,
}

impl<T> LookBehindPool<T>
{
	fn new(capacity: usize) -> LookBehindPool<T> 
	{ 	
	  let m: Vec<T> = Vec::new();
	   
	  LookBehindPool { capacity: capacity as u64, pool: m}
	}    

	fn item(&self, index: usize) -> &T
	{
	  &self.pool[index]
	}

	fn write(&mut self, value: T)
	{
		if self.pool.len()==self.capacity as usize
		{ 
		  self.pool[0]= value
		}
		else
		{
		  self.pool.push(value)
		}		  
	}
}

#[cfg(test)]
mod tests 
{
	use super::*;

	struct Prices
	{
	  open: u64,
	  high: u64,
	  low: u64,
	  close: u64
	}

	#[test]
	fn it_works() {
		let mut my_thing = LookBehindPool::<Prices>::new(2);
		assert_eq!(2, my_thing.capacity);
		
		my_thing.write(Prices{open: 1, high:2, low:3, close: 4});
		my_thing.write(Prices{open: 1, high:6, low:3, close: 4});
		
		assert_eq!(1, my_thing.item(1).open);
		assert_eq!(6, my_thing.item(1).high);
		assert_eq!(3, my_thing.item(1).low);
		assert_eq!(4, my_thing.item(1).close);
	}
}
