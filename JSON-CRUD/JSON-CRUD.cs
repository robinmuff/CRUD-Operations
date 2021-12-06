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
        private FileSystemWatcher fileSystemWatcher;
        private ObservableCollection<O> list;
        private CryptAccess cryptAccess;
        public string filename { get; }

        /* -- Contructor -- */
        public CRUD(string filename, CryptAccess cryptAccess = null)
        {
            checkFilename(filename);
            this.filename = filename;

            this.fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            this.fileSystemWatcher.Path = "/";
            this.fileSystemWatcher.Changed += updater;
            this.fileSystemWatcher.Renamed += updater;
            this.fileSystemWatcher.Deleted += updater;
            this.fileSystemWatcher.Created += updater;
            this.fileSystemWatcher.EnableRaisingEvents = true;

            this.cryptAccess = cryptAccess;

            list = new ObservableCollection<O>();

            readList();
        }

        /* -- File Operations -- */
        private void readList()
        {
            if (File.Exists(filename))
            {
                string fileContent = File.ReadAllText(filename);
                if (cryptAccess != null) { fileContent = DecryptBytes(File.ReadAllBytes(filename)); }
                Set(JsonConvert.DeserializeObject<List<O>>(fileContent));
            }
        }
        private void safeList()
        {
            this.fileSystemWatcher.EnableRaisingEvents = false;
            string fileContent = JsonConvert.SerializeObject(Get());
            if (cryptAccess != null)
            {
                File.WriteAllBytes(filename, EncryptBytes(Encoding.ASCII.GetBytes(fileContent)));
            }
            else
            {
                File.WriteAllText(filename, fileContent);
            }
            this.fileSystemWatcher.EnableRaisingEvents = true;
        }
        private void checkFilename(string filename)
        {
            int lastIndex = filename.LastIndexOf('/');

            if (lastIndex != -1 && !Directory.Exists(filename.Substring(0, lastIndex)))
            {
                throw new System.Exception("Filename isn't valid, check that the directory exist (File doesn't have to exist");
            }
        }

        /* -- List Operations -- */
        public void Set(List<O> list) { this.list = new ObservableCollection<O>(list); safeList(); }
        public List<O> Get() { return new List<O>(list); }
        public void Add(O item) { list.Add(item); safeList(); }
        public void AddRange(List<O> items) { foreach (O item in items) { list.Add(item); } safeList(); }
        public void Clear() { list.Clear(); safeList(); }
        public void CopyTo(O[] array, int index) { list.CopyTo(array, index); safeList(); }
        public bool Contains(O item) { return list.Contains(item); }
        public bool ContainsMultiple(List<O> items) { foreach (O item in items) { if (!list.Contains(item)) { return false; } } return true; }
        public int Count() { return list.Count; }
        public bool Equals() { return list.Equals(list); }
        public IEnumerator GetEnumerator() { return list.GetEnumerator(); }
        public override int GetHashCode() { return list.GetHashCode(); }
        public int IndexOf(O item) { return list.IndexOf(item); }
        public List<int> IndexOfMultiple(List<O> items) { List<int> indexList = new List<int>(); foreach (O item in items) { indexList.Add(list.IndexOf(item)); } return indexList; }
        public void Insert(int index, O item) { list.Insert(index, item); safeList(); }
        public void Move(int oldIndex, int newIndex) { list.Move(oldIndex, newIndex); safeList(); }
        public void Remove(O item) { list.Remove(item); safeList(); }
        public void RemoveMultiple(List<O> items) { foreach (O item in items) { list.Remove(item); } safeList(); }
        public void RemoveAt(int index) { list.RemoveAt(index); safeList(); }
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
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            RijndaelCipher.Mode = CipherMode.CBC;
            byte[] salt = Encoding.ASCII.GetBytes(cryptAccess.saltRounds.ToString());
            PasswordDeriveBytes password = new PasswordDeriveBytes(cryptAccess.password, salt, "SHA1", 2);

            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            Encoding unicode = Encoding.Unicode;
            byte[] validationCode = unicode.GetBytes("1234567890");

            return CipherBytes.Concat(validationCode).ToArray();
        }
        public string DecryptBytes(byte[] encryptedBytes)
        {
            encryptedBytes = encryptedBytes.Take(encryptedBytes.Count() - 20).ToArray();

            string azurekey = "";
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            RijndaelCipher.Mode = CipherMode.CBC;
            byte[] salt = Encoding.ASCII.GetBytes(cryptAccess.saltRounds.ToString());
            PasswordDeriveBytes password = new PasswordDeriveBytes(cryptAccess.password, salt, "SHA1", 2);

            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream(encryptedBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] plainBytes = new byte[encryptedBytes.Length];

            try
            {
                int DecryptedCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
            }
            catch { }

            memoryStream.Close();
            cryptoStream.Close();

            List<byte> finall = new List<byte>();
            for (int i = 0; i < plainBytes.Length; i++)
            {
                if (plainBytes[i] != 0)
                {
                    finall.Add(plainBytes[i]);
                }
            }
            byte[] array = finall.ToArray();
            azurekey = Encoding.ASCII.GetString(array);
            return azurekey;
        }

        /* -- FileSystemWatcher -- */
        private void updater(object sender, FileSystemEventArgs e)
        {
            readList();
        }
    }
}
