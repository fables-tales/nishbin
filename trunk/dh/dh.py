from random import randint,randrange
import gmpy
class Dh:
	def __init__(self):
		self.g = None
		self.p = None
		self.a = None
		self.k1 = None
		self.k2 = None
		self.key = None
	def gen_g_p(self):
		self.g = gmpy.next_prime(randint(0,2**256))
		self.p = gmpy.next_prime(randint(0,2**256))
		
	def gen_a(self):
		
		ta = randint(1,2**256)
		while ta > self.p:
			ta = randint(1,2**256)
		self.a = gmpy.next_prime(ta)
		ta = None
	def compute_k1(self):
		self.k1 = pow(self.g,self.a,self.p)
	def get_key(self):
		if self.k1 is not None and self.k2 is not None:
			self.key = pow(self.k2,self.a,self.p)
			
if __name__ == "__main__":
	t1 = Dh()
	t2 = Dh()
	t1.gen_g_p()
	t2.g = t1.g
	t2.p = t1.p
	t1.gen_a()
	t2.gen_a()
	t1.compute_k1()
	t2.compute_k1()
	t1.k2 = t2.k1
	t2.k2 = t1.k1
	t1.get_key()
	t2.get_key()
	print t1.key
	print t2.key
	
