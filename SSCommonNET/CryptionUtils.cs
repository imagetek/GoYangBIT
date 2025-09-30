using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SSCommonNET
{
	public class CryptionUtils
	{
		public static string ENCRYPT_BY_SHA256(string text)
		{
			return CryptionUtils.ENCRYPT_BY_SHA256(text, Encoding.UTF8);
		}

		public static string ENCRYPT_BY_SHA256(string text, Encoding encoding)
		{
			try
			{
				byte[] hash = new SHA256Managed().ComputeHash(encoding.GetBytes(text));
				StringBuilder stringBuilder = new StringBuilder();
				foreach (byte num in hash)
					stringBuilder.Append(num.ToString("x2"));
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
		}

		public static string ENCRYPT_BY_SHA512(string text)
		{
			return CryptionUtils.ENCRYPT_BY_SHA512(text, Encoding.UTF8);
		}

		public static string ENCRYPT_BY_SHA512(string text, Encoding encoding)
		{
			try
			{
				byte[] hash = new SHA512Managed().ComputeHash(encoding.GetBytes(text));
				StringBuilder stringBuilder = new StringBuilder();
				foreach (byte num in hash)
					stringBuilder.Append(num.ToString("x2"));
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
		}

		public static string ENCRYPT_BY_AES256(string text, string key, Encoding encoding)
		{
			MemoryStream memoryStream = (MemoryStream)null;
			CryptoStream cryptoStream = (CryptoStream)null;
			try
			{
				RijndaelManaged rijndaelManaged = new RijndaelManaged();
				byte[] bytes1 = encoding.GetBytes(text);
				byte[] bytes2 = Encoding.UTF8.GetBytes(key.Length.ToString());
				PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(key, bytes2);
				byte[] bytes3 = passwordDeriveBytes.GetBytes(32);
				byte[] bytes4 = passwordDeriveBytes.GetBytes(16);
				ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes3, bytes4);
				memoryStream = new MemoryStream();
				cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
				cryptoStream.Write(bytes1, 0, bytes1.Length);
				cryptoStream.FlushFinalBlock();
				return Convert.ToBase64String(memoryStream.ToArray());
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
			finally
			{
				cryptoStream?.Close();
				memoryStream?.Close();
			}
		}

		public static string DECRYPT_BY_AES256(string encryptText, string key, Encoding encoding)
		{
			MemoryStream memoryStream = (MemoryStream)null;
			CryptoStream cryptoStream = (CryptoStream)null;
			try
			{
				RijndaelManaged rijndaelManaged = new RijndaelManaged();
				byte[] buffer = Convert.FromBase64String(encryptText);
				byte[] bytes1 = Encoding.UTF8.GetBytes(key.Length.ToString());
				PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(key, bytes1);
				byte[] bytes2 = passwordDeriveBytes.GetBytes(32);
				byte[] bytes3 = passwordDeriveBytes.GetBytes(16);
				ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes2, bytes3);
				memoryStream = new MemoryStream(buffer);
				cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
				byte[] numArray = new byte[buffer.Length];
				int count = cryptoStream.Read(numArray, 0, numArray.Length);
				return encoding.GetString(numArray, 0, count);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
			finally
			{
				cryptoStream?.Close();
				memoryStream?.Close();
			}
		}
	}
}
