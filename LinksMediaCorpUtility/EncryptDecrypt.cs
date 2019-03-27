namespace LinksMediaCorpUtility
{
    using System;
    using System.Text;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using System.IO;
    using System.Security.Cryptography;
    public class EncryptDecrypt : IDisposable
    {
        #region Global Objects
        StringBuilder objStringBuilder = new StringBuilder();
        private const string _securityKey = "fitcomkit11";
        // LogManager LogManager = new LogManager();
        #endregion
        #region Encript a string
        /// <summary>
        /// function for Encript a string  
        /// </summary>
        /// date : feb 14, 2015
        /// developer: Raghuraj
        ///
        public string EncryptString(string strEncrypted)
        {
            string encryptedPassword = string.Empty;
            try
            {
                objStringBuilder.AppendLine("Begin: Utility-Common-EncryptDecrypt-EncryptString");
                byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
                encryptedPassword = Convert.ToBase64String(b);
                objStringBuilder.AppendLine("End: Utility-Common-EncryptDecrypt-EncryptString");
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Message = ex.ToString();
                logEntry.Title = ex.Message;
                logEntry.Priority = 1;
                logEntry.Severity = System.Diagnostics.TraceEventType.Error;
                Logger.Write(logEntry);
            }
            finally
            {
                LogManager.LogManagerInstance.WriteTraceLog(objStringBuilder);
            }
            //encryptedPassword = EncryptPassword(strEncrypted);
            return encryptedPassword;
        }
        #endregion
        #region Decript a string
        /// <summary>
        /// function for Decript a string  
        /// </summary>
        /// date : feb 14, 2015
        /// developer: Raghuraj
        /// 
        public string DecryptString(string encrString)
        {
            string decryptedPassword = string.Empty;
            try
            {
                objStringBuilder.AppendLine("Begin: Utility-Common-EncryptDecrypt-DecryptString");
                byte[] b = Convert.FromBase64String(encrString);
                decryptedPassword = System.Text.ASCIIEncoding.ASCII.GetString(b);
                objStringBuilder.AppendLine("End: Utility-Common-EncryptDecrypt-DecryptString");
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Message = Convert.ToString(ex);
                logEntry.Title = ex.Message;
                logEntry.Priority = 1;
                logEntry.Severity = System.Diagnostics.TraceEventType.Error;
                Logger.Write(logEntry);
            }
            finally
            {
                LogManager.LogManagerInstance.WriteTraceLog(objStringBuilder);
            }
            return decryptedPassword;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string EncryptPasswordMDF(string input)
        {
            string hash = string.Empty;
            try
            {
                objStringBuilder.AppendLine("Begin: Utility-Common-EncryptPassword using MD5");
                using (MD5 md5Hash = MD5.Create())
                {
                    hash = GetMd5Hash(md5Hash, input);
                    if (VerifyMd5Hash(md5Hash, input, hash))
                    {
                        return hash;
                    }
                    else
                    {
                        objStringBuilder.AppendLine("The hashes are not same.");
                    }
                }

            }
            catch (Exception error)
            {
                LogManager.LogManagerInstance.WriteErrorLog(error);
            }
            finally
            {
                objStringBuilder.AppendLine("End: Utility-Common-EncryptDecrypt-EncryptString");
                LogManager.LogManagerInstance.WriteTraceLog(objStringBuilder);
            }

            return hash;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetMd5Hash(MD5 md5Hash, string input)
        {
            try
            {

                objStringBuilder.AppendLine("Begin: GetMd5Hash");
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();
            }
            catch (Exception error)
            {
                LogManager.LogManagerInstance.WriteErrorLog(error);
                throw;
            }
            finally
            {
                objStringBuilder.AppendLine("End: GetMd5Hash");
                LogManager.LogManagerInstance.WriteTraceLog(objStringBuilder);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            try
            {
                objStringBuilder.AppendLine("Begin: VerifyMd5Hash");
                // Hash the input. 
                string hashOfInput = GetMd5Hash(md5Hash, input);
                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                LogManager.LogManagerInstance.WriteErrorLog(error);
                throw;
            }
            finally
            {
                objStringBuilder.AppendLine("End: VerifyMd5Hash");
                LogManager.LogManagerInstance.WriteTraceLog(objStringBuilder);
            }
        }


        /// <summary>
        /// This method is used to convert the plain text to Encrypted/Un-Readable Text format.
        /// </summary>
        /// <param name="PlainText">Plain Text to Encrypt before transferring over the network.</param>
        /// <returns>Cipher Text</returns>
        public string EncryptPassword(string input)
        {
            string hash = string.Empty;
            MD5CryptoServiceProvider objMD5CryptoService = null;
            TripleDESCryptoServiceProvider objTripleDESCryptoService = null;
            try
            {
                objStringBuilder.AppendLine("Begin: Utility-Common-EncryptPassword using MD5");
                //Getting the bytes of Input String.
                byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(input);

                objMD5CryptoService = new MD5CryptoServiceProvider();

                //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
                byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

                //De-allocatinng the memory after doing the Job.
                objMD5CryptoService.Clear();

                objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

                //Assigning the Security key to the TripleDES Service Provider.
                objTripleDESCryptoService.Key = securityKeyArray;

                //Mode of the Crypto service is Electronic Code Book.
                objTripleDESCryptoService.Mode = CipherMode.ECB;

                //Padding Mode is PKCS7 if there is any extra byte is added.
                objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

                var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

                //Transform the bytes array to resultArray
                byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

                //Releasing the Memory Occupied by TripleDES Service Provider for Encryption.
                objTripleDESCryptoService.Clear();

                //Convert and return the encrypted data/byte into string format.
                hash = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception error)
            {
                LogManager.LogManagerInstance.WriteErrorLog(error);
            }
            finally
            {
                objMD5CryptoService.Dispose();
                objTripleDESCryptoService.Dispose();
                objStringBuilder.AppendLine("End: Utility-Common-EncryptDecrypt-EncryptString");
                LogManager.LogManagerInstance.WriteTraceLog(objStringBuilder);
            }
            return hash;
        }
        #endregion
        #region Dispose
        /// <summary>
        /// function for Dispose objects
        /// </summary>
        /// date : Feb 16, 2015
        /// developer: Raghuraj
        /// 
        private Stream _resource;
        private bool _disposed;
        // The stream passed to the constructor  
        // must be readable and not null.        

        public void Dispose()
        {
            Dispose(true);
            // Use SupressFinalize in case a subclass 
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these  
            // operations, as well as in your methods that use the resource. 
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_resource != null)
                        _resource.Dispose();
                }
                // Indicate that the instance has been disposed.
                _resource = null;
                _disposed = true;
            }
        }
        #endregion
    }
}
