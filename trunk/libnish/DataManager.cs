using System.Collections.Generic;
using System;

namespace libnish
{
	
	
	public class DataManager
	{
		private List<MetaData> allmetadata;
		public List<MetaData> AllMetaData{
			get{
				return this.allmetadata;
			} set{
				this.allmetadata = value;
			}
		}
		
		
		public DataManager()
		{
			this.allmetadata = new List<MetaData>();
		}
	}
	public class MetaData
	{
		private string uuid;
		public string FileName;
		public string FileDesc;
		
		public string UUID{
			get{
				return this.uuid;
			} set{
				if (Crypto.UUID.verifyuuid(value)){
					this.uuid = value;
				} else{
					throw new ArgumentException("invalid uuid passed");
				}
			}
		}
		
		
		public MetaData(string uuid)
		{
			if (Crypto.UUID.verifyuuid(uuid)){
				this.uuid = uuid;
			} else{
				throw new ArgumentException("invalid uuid passed");
			}
		}
		/// <summary>
		/// Loads metadata from a file on disk
		/// </summary>
		/// <param name="filepath">
		/// A <see cref="System.String"/>
		/// </param>
		public static List<MetaData> LoadMetaDataFromFile(string filepath){
			throw new NotImplementedException();
		}
	}
}
