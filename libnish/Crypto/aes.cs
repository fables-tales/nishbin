// aes.cs created with MonoDevelop
// User: sam at 16:23 17/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Org.BouncyCastle.Crypto.Parameters;
 
namespace libnish.Crypto
{
    
    
    public class LowLevelAes{
        Org.BouncyCastle.Crypto.Engines.AesFastEngine EncHandler = new Org.BouncyCastle.Crypto.Engines.AesFastEngine();
        Org.BouncyCastle.Crypto.Engines.AesFastEngine DecHandler = new Org.BouncyCastle.Crypto.Engines.AesFastEngine();
        
        public LowLevelAes(byte[] key){
            EncHandler.Init(true,new KeyParameter(key));
            DecHandler.Init(false,new KeyParameter(key));
            
        }
        public byte[] Decrypt(byte[] input){
            if (input.Length != 16){
                throw new ArgumentException("bad block length");    
            }
            byte[] output = new byte[16];
            DecHandler.ProcessBlock(input,0,output,0);
            return output;
            
        }
        
        public byte[] Encrypt(byte[] input){
            if (input.Length != 16){
                throw new libnish.Debug.SkynetException("this program has become skynet");
            }
            byte[] output = new byte[16];
            EncHandler.ProcessBlock(input,0,output,0);
            return output;
        }
        
    }
    
    public class aes{
        private LowLevelAes aeshandler;        
        private byte[] EncState = new byte[16];
        private byte[] DecState = new byte[16];
        public aes(byte[] key,byte[]iv){
            if (key.Length == 32){            
                aeshandler = new LowLevelAes(key);
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
        public aes(){
            aeshandler = new LowLevelAes(Math.math.getRandom(256).GetBytes());
            this.EncState = Math.math.getRandom(128).GetBytes();
            this.DecState = new byte[16];
            this.EncState.CopyTo(DecState,0);
            
        }
        private byte[] encrypt(byte[] input){
            if (input.Length != 16){
                throw new ArgumentException("input is not the correct length");
            }
            byte[] output = new byte[16];
            for (int i=0;i<16;i++){
                output[i] = (byte)(input[i] ^ EncState[i]);
            }
            output = aeshandler.Encrypt(output);
            output.CopyTo(EncState,0);
            return output;
        }
        private byte[] decrypt(byte[] input){
            if (input.Length != 16){
                throw new ArgumentException("input is not the correct length");
            }
            byte[] output = new byte[16];
            
            output = aeshandler.Decrypt(output);
            for (int i =0;i<16;i++){
                output[i] = (byte)(output[i]^DecState[i]);
            }
            input.CopyTo(DecState,0);
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
