from hashlib import sha256,md5
from Crypto.Cipher import AES
from Crypto.Util.number import getRandomNumber

def create_key_for_n_chunks(n):
	key = ""
	for i in range(0,(256/8)*n):
		key += chr(getRandomNumber(7))
	return key
def create_split_key_and_iv_for_n_chunks(n):
	key = create_key_for_n_chunks(n)
	iv = create_key_for_n_chunks(n)
	subkeys = []
	subivs = []
	for i in range(0,len(key)/(256/8)):
		subkeys.append(key[i*(256/8):(i+1)*(256/8)])
		subivs.append(iv[i*(256/8):(i+1)*(256/8)])
	return (subkeys,subivs)
def recover_key_and_create_aes_object(split_key,split_iv):
	recovered_key = ""
	recovered_iv = ""
	for i in split_key:
		recovered_key += i
	for i in split_iv:
		recovered_iv += i	
	actual_key = sha256(recovered_key).digest()
	actual_iv = md5(recovered_iv).digest()
	aes = AES.new(actual_key,AES.MODE_CBC,actual_iv)
	return aes

#print recover_key_and_create_aes_object(["d","b"],["c","a"])
key,iv= create_split_key_and_iv_for_n_chunks(16)
