from time import time
from hashlib import sha256
from random import randint

for i in range(0,256):
	print sha256(str(time())).hexdigest() + sha256(str(randint(0,2**32))).hexdigest()
