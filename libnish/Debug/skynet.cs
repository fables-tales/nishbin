// skynet.cs created with MonoDevelop
// User: sam at 21:32 10/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace libnish.Debug
{
    
    
    public class SkynetException:Exception
    {
        
        public SkynetException()
        {
        }
        public SkynetException(string message):base (message){
            
        }
    }
}
