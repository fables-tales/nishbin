// Main.cs created with MonoDevelop
// User: sam at 22:17 16/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using libnish.Crypto;

namespace encryptortestor
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            byte[] blocks = libnish.Crypto.Math.math.getRandom(256).GetBytes();
            byte[] moreblocks = libnish.Crypto.Math.math.getRandom(128).GetBytes();
            aes aes = new aes();
            byte[] enc1 = aes.encrypt(blocks);
            byte[] enc2 = aes.encrypt(moreblocks);
            byte[] dec1 = aes.decrypt(enc1);
            byte[] dec2 = aes.decrypt(enc2);
            bool equal = true;
            for(int i=0;i<blocks.Length;i++){
                if (blocks[i] != dec1[i]){
                    equal = false;
                    break;
                }
            }
            if (equal){
                Console.Out.WriteLine("first dataset decrypted");
                
            } else {
                Console.Out.WriteLine("first dataset didn't decrypt");
            }
            equal = true;
            for (int i =0;i<moreblocks.Length;i++){
                if (moreblocks[i] == dec2[i]){
                    equal = false;
                    break;
                }
            }
            if (equal){
                Console.Out.WriteLine("second dataset decrypted");
            } else {
                Console.Out.WriteLine("second dataset didn't decrypt");
            }
            Console.Out.WriteLine("now testing non-simultaneous decryption");
            aes aes2 = new aes();                        
            byte[] blocks2 = libnish.Crypto.Math.math.getRandom(256*3).GetBytes();
            byte[] enc = aes2.encrypt(blocks2);
            byte[] encblock1 = new byte[16];
            byte[] decblock1 = new byte[16];
            Array.Copy(enc,0,encblock1,0,16);
            decblock1 = aes2.decrypt(encblock1);
            equal = true;
            for (int i=0;i<16;i++){
                if (decblock1[i] != blocks2[i]){
                    equal = false;
                    Console.Out.WriteLine("failbacon");
                    break;
                    
                }
            }    
            Array.Copy(enc,16,encblock1,0,16);
            decblock1 = aes2.decrypt(encblock1);
            equal = true;
            for (int i=0;i<16;i++){
                if (decblock1[i] != blocks2[i+16]){
                    equal = false;
                    Console.Out.WriteLine("failbacon2");
                    break;
                    
                }
            }
            
        }
    }
}