from dh import *

def make_even_length(string):
	if len(string) % 2 == 1:
		string = "0" + string
		return string
	else:
		return string	
		
class Request:
	def __init__(self):
		self.Dh = Dh()
		self.Dh.gen_g_p()
		self.Dh.gen_a()
		self.Dh.compute_k1()
		
	def send_data(self):
		frame = ""
		frame += make_even_length(str(self.Dh.g)).decode("hex")
		frame += ":"
		frame += make_even_length(str(self.Dh.p)).decode("hex")
		frame += ":"
		frame += make_even_length(str(self.Dh.k1)).decode("hex")
		return frame
		
	def recv_data(self,response):
		values = response.split(":")
		if len(values) == 1:
			self.Dh.k2 = int(values[0].encode("hex"))
			return 0
		else:
			return -1
			
	def compute_key(self):
		self.Dh.get_key()				
		return self.Dh.key
		
class Response:
	def __init__(self):
		self.Dh = Dh()
		
	def recv_data(self,frame):
		values = frame.split(":")
		if len(values) == 3:
			g = int(values[0].encode("hex"))
			p = int(values[1].encode("hex"))
			k2 = int(values[2].encode("hex"))
			self.Dh.g = g
			self.Dh.p = p
			self.Dh.gen_a()
			self.Dh.compute_k1()
			self.Dh.k2 = k2
			self.Dh.get_key()
			return self.Dh.key
		else:
			return -1				
	
	def send_data(self):
		frame = ""
		frame += make_even_length(str(self.Dh.k1)).decode("hex")		
		return frame
		
if __name__ == "__main__":
	rq = Request()
	re = Response()
	data = rq.send_data()
	re.recv_data(data)
	response = re.send_data()
	rq.recv_data(response)
	rq.compute_key()
	print rq.Dh.key
	print re.Dh.key
	#print [data]
	#print [response]
	assert rq.Dh.key == re.Dh.key
