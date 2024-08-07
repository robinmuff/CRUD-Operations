using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JSON_CRUD
{
    public class CRUD<O> where O : new()
    {
        /* -- Global Vars -- */
        private ObservableCollection<O> list;
        private readonly CryptAccess cryptAccess;
        public string Filename { get; }
        public bool DoCrypt { get; set; }

        /* -- Contructor -- */
        public CRUD(string filename, CryptAccess cryptAccess = null)
        {
            CheckFilename(filename);
            this.Filename = filename;
            this.cryptAccess = cryptAccess;
            this.DoCrypt = cryptAccess != null;
            this.list = [];

            ReadList();
        }

        /* -- File Operations -- */
        public void ReadList()
        {
            if (File.Exists(Filename))
            {
                FileStream fs = new(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new(fs);
                string fileContent = sr.ReadToEnd();
                sr.Close();

                if ((cryptAccess != null && DoCrypt) || !IsJsonValid(fileContent))
                {
                    byte[] oFileBytes = null;
                    using (FileStream nfs = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        int numBytesToRead = Convert.ToInt32(nfs.Length);
                        oFileBytes = new byte[(numBytesToRead)];
                        nfs.Read(oFileBytes, 0, numBytesToRead);
                    }
                    fileContent = DecryptBytes(oFileBytes);
                }
                Set(JsonConvert.DeserializeObject<List<O>>(fileContent), false);
            }
        }
        private static bool IsJsonValid(string json)
        {
            try
            {
                JsonConvert.DeserializeObject<List<O>>(json);
                return true;
            }
            catch { return false; }
        }
        private void SafeList()
        {
            string fileContent = JsonConvert.SerializeObject(Get());
            if (cryptAccess != null && DoCrypt)
            {
                File.WriteAllBytes(Filename, EncryptBytes(Encoding.ASCII.GetBytes(fileContent)));
            }
            else
            {
                File.WriteAllText(Filename, fileContent);
            }
        }
        private static void CheckFilename(string filename)
        {
            int lastIndex = filename.LastIndexOf('/');

            if (lastIndex != -1 && !Directory.Exists(filename[..lastIndex]))
            {
                throw new System.Exception("Filename isn't valid, check that the directory exist (File doesn't have to exist");
            }
        }

        /* -- List Operations -- */
        public void Set(List<O> list, bool save = true) { this.list = new ObservableCollection<O>(list); if (save) { SafeList(); } }
        public List<O> Get() { return new List<O>(list); }
        public void Add(O item) { list.Add(item); SafeList(); }
        public void AddRange(List<O> items) { foreach (O item in items) { list.Add(item); } SafeList(); }
        public void Clear() { list.Clear(); SafeList(); }
        public void CopyTo(O[] array, int index) { list.CopyTo(array, index); SafeList(); }
        public bool Contains(O item) { return list.Contains(item); }
        public bool ContainsMultiple(List<O> items) { foreach (O item in items) { if (!list.Contains(item)) { return false; } } return true; }
        public int Count() { return list.Count; }
        public bool Equals() { return list.Equals(list); }
        public IEnumerator GetEnumerator() { return list.GetEnumerator(); }
        public override int GetHashCode() { return list.GetHashCode(); }
        public int IndexOf(O item) { return list.IndexOf(item); }
        public List<int> IndexOfMultiple(List<O> items) { List<int> indexList = new List<int>(); foreach (O item in items) { indexList.Add(list.IndexOf(item)); } return indexList; }
        public void Insert(int index, O item) { list.Insert(index, item); SafeList(); }
        public void Move(int oldIndex, int newIndex) { list.Move(oldIndex, newIndex); SafeList(); }
        public void Remove(O item) { list.Remove(item); SafeList(); }
        public void RemoveMultiple(List<O> items) { foreach (O item in items) { list.Remove(item); } SafeList(); }
        public void RemoveAt(int index) { list.RemoveAt(index); SafeList(); }
        public O GetO(int index) { return list[index]; }
        public string ItemToString(O item) { return list[list.IndexOf(item)].ToString(); }
        public override string ToString() { return list.ToString(); }
        public ObservableCollection<O> GetCollection() { return list; }

        /* -- Notifications -- */
        public void AddChangeListener(NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler)
        {
            list.CollectionChanged += notifyCollectionChangedEventHandler;
        }
        public void AddMultipleChangeListener(List<NotifyCollectionChangedEventHandler> notifyCollectionChangedEventHandlers)
        {
            foreach (NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler in notifyCollectionChangedEventHandlers)
            {
                list.CollectionChanged += notifyCollectionChangedEventHandler;
            }
        }

        /* -- Encrypt | Decrypt -- */
        public byte[] EncryptBytes(byte[] inputBytes)
        {
            Aes aes = Aes.Create();

            byte[] salt = Encoding.ASCII.GetBytes(cryptAccess.saltRounds.ToString());
            PasswordDeriveBytes password = new(cryptAccess.password, salt, "SHA1", 2);

            ICryptoTransform Encryptor = aes.CreateEncryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new();
            CryptoStream cryptoStream = new(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            Encoding unicode = Encoding.Unicode;
            byte[] validationCode = unicode.GetBytes("1234567890");

            return [.. CipherBytes, .. validationCode];
        }
        public string DecryptBytes(byte[] encryptedBytes)
        {
            Aes aes = Aes.Create();

            encryptedBytes = encryptedBytes.Take(encryptedBytes.Length - 20).ToArray();

            byte[] salt = Encoding.ASCII.GetBytes(cryptAccess.saltRounds.ToString());
            PasswordDeriveBytes password = new(cryptAccess.password, salt, "SHA1", 2);

            ICryptoTransform Decryptor = aes.CreateDecryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new(encryptedBytes);
            CryptoStream cryptoStream = new(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] plainBytes = new byte[encryptedBytes.Length];

            try
            {
                int DecryptedCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
            }
            catch { }

            memoryStream.Close();
            cryptoStream.Close();

            List<byte> finall = [];
            for (int i = 0; i < plainBytes.Length; i++)
            {
                if (plainBytes[i] != 0)
                {
                    finall.Add(plainBytes[i]);
                }
            }
            byte[] array = [.. finall];
            string azurekey = Encoding.ASCII.GetString(array);
            return azurekey;
        }
    }
}
