using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Helper
{
	public class CookieStorage
	{
		private static CookieStorage instance = null;

		private static readonly object instanceLock = new object();
		private Dictionary<string, string> _storage;

		public CookieStorage()
        {
			_storage = new Dictionary<string, string>();
		}

		public static CookieStorage Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new CookieStorage();
					}
					return instance;
				}
			}
		}

		public void Append(string key, string value)
		{
			if (_storage.ContainsKey(key))
			{
				_storage[key] = value;
			}
			else
			{
				_storage.Add(key, value);
			}
		}

		public string Get(string key)
		{
			if (_storage.ContainsKey(key))
			{
				return _storage[key];
			}
			throw new Exception("can't not find this key: "+ key);
		}
		public void Remove(string key)
		{
			if (_storage.ContainsKey(key))
			{
				_storage.Remove(key);
			} else
			{
				throw new Exception("can't not find this key: " + key);
			}
		}

		public bool ContainsKey(string key)
		{
			return _storage.ContainsKey(key);
		}
		public void Clear()
		{
			_storage.Clear();
		}

		public Dictionary<string, string> GetAll()
		{
			return new Dictionary<string, string>(_storage);
		}

	}
}
