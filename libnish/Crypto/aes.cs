// aes.cs created with MonoDevelop
// User: sam at 16:23 17/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Engines;
 
namespace libnish.Crypto
{
    
    
    public class LowLevelAes{
        AesFastEngine EncHandler = new AesFastEngine();
        AesFastEngine DecHandler = new AesFastEngine();
        
        public LowLevelAes(byte[] key){
            KeyParameter k = new KeyParameter(key);
			this.EncHandler.Init(true,k);
            this.DecHandler.Init(false,k);
            
        }
        public byte[] Decrypt(byte[] input){
            if (input.Length != 16){
                throw new ArgumentException("bad block length");    
            } else {
            	byte[] output = new byte[16];
            	this.DecHandler.ProcessBlock(input,0,output,0);
            	return output;
			}
            
        }
        
        public byte[] Encrypt(byte[] input){
            if (input.Length != 16){
                throw new libnish.Debug.SkynetException("this program has become skynet");
            } else {
            	byte[] output = new byte[16];
            	this.EncHandler.ProcessBlock(input,0,output,0);
            	return output;
			}
        }
        
    }
    
    public class AES{
        private LowLevelAes aeshandler;        
        private byte[] EncState = new byte[16];
        private byte[] DecState = new byte[16];
		
        public AES(byte[] key,byte[]iv){
            if (key.Length == 32){            
                this.aeshandler = new LowLevelAes(key);
            }
            else {
                throw new ArgumentException("key passed is not long enough");
            }
            if (iv.Length == 16){
                this.EncState = iv;
                this.DecState = iv;
            }
            else {
                throw new ArgumentException("key passed is not long enough");                
            }
        }
		
        public AES(){
            this.aeshandler = new LowLevelAes(Math.getRandom(256).GetBytes());
            this.EncState = Math.getRandom(128).GetBytes();
            this.DecState = new byte[16];
            this.EncState.CopyTo(DecState,0);
            
        }
		
        private byte[] encrypt(byte[] input){
            byte[] output = new byte[16];
            for (int i=0;i<16;i++){
                output[i] = (byte)(input[i] ^ this.EncState[i]);
            }
            output = this.aeshandler.Encrypt(output);
            output.CopyTo(this.EncState,0);
            return output;
        }
		
        private byte[] decrypt(byte[] input){
            byte[] output = new byte[16];
            input.CopyTo(output,0);
            output = this.aeshandler.Decrypt(output);
            for (int i =0;i<16;i++){
                output[i] = (byte)(output[i]^this.DecState[i]);
            }
            input.CopyTo(this.DecState,0);
            return output;
        }
		
        public byte[] Encrypt(byte[] input){
            byte[] output = new byte[input.Length];            
            if (input.Length % 16 != 0){
                throw new ArgumentException("input is not the correct length");
            }
            for (int i=0;i<input.Length;i+=16){
                byte[] block = new byte[16];
                Array.Copy(input,i,block,0,16);
                block = this.encrypt(block);
                Array.Copy(block,0,output,i,16);
            }
            return output;
        }
        
		public byte[] Decrypt(byte[] input){
            if (input.Length % 16 != 0){
                throw new ArgumentException("input is not the correct length");
            }
            byte[] output = new byte[input.Length];            
            for (int i=0;i<input.Length;i+=16){
                byte[] block = new byte[16];
                Array.Copy(input,i,block,0,16);
                block = this.decrypt(block);
                Array.Copy(block,0,output,i,16);
            }
            return output;
        }
    }
    
}
