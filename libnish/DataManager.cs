using System.Collections.Generic;
using System;

namespace libnish
{
	
	
	public class DataManager
	{
		List<MetaData> AllMetaData;
		
		public DataManager()
		{
		}
	}
	public class MetaData
	{
		private string uuid;
		private string filename;
		private string filedesc;
		
		public MetaData(string uuid)
		{
			this.uuid = uuid;
		}
		/// <summary>
		/// Loads metadata from a file on disk
		/// </summary>
		/// <param name="filepath">
		/// A <see cref="System.String"/>
		/// </param>
		public static MetaData LoadMetaDataFromFile(string filepath){
			throw new NotImplementedException();
		}
	}
}
