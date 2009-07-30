// Main.cs created with MonoDevelop
// User: sam at 22:17 16/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using libnish.Crypto;
using libnish;

namespace encryptortestor
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            AES aes = new AES();
            byte[] someblocks = libnish.Crypto.Math.getRandom(128*34).GetBytes();
            byte[] somemoreblocks = libnish.Crypto.Math.getRandom(128*12).GetBytes();
            byte[] enc1 = aes.Encrypt(someblocks);
            byte[] enc2 = aes.Encrypt(somemoreblocks);
            byte[] dec1 = aes.Decrypt(enc1);
            byte[] dec2 = aes.Decrypt(enc2);
            Console.Out.WriteLine(dec1.Length == someblocks.Length);
            Console.Out.WriteLine(dec2.Length == somemoreblocks.Length);
            Console.ReadLine();
            for (int i =0;i<someblocks.Length;i++){
                if (someblocks[i] != dec1[i]){
                    Console.Out.WriteLine("failmonkey");
                }
                
            }
            for (int i=0;i<somemoreblocks.Length;i++){
                if (somemoreblocks[i] != dec2[i]){
                    Console.Out.WriteLine("failmonkey2");
                }
            }
        }
    }
}