using CertificateManager;
using CryptographyManager;
using DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Replikator
{
    public class ReplikatorService : IReplikator
    {
        public static string kljuc = string.Empty;
        
        public void Archive(byte[] data, byte[] signature)
        {
            string clienName = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
            //clienName += "S_sign";
            clienName = clienName.Substring(0, 3) + "S";
            X509Certificate2 certificate2 = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clienName);
            if (DigitalSignature.Verify(data, signature, certificate2))
            {
                if (!Directory.Exists("Archive"))   //ako ne postoji archive folder, napravi ga
                {
                    Directory.CreateDirectory("Archive");
                }
                string archiveFile = String.Format("{0} {1}.txt", "archive", DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss"));  //formira string za arhivni fajl

                byte[] desifrovano = CryptographyManager.EncryptDecryptManager.DecrypthFile(data, "aca.txt", kljuc); //'aca.txt' je nebitan parametar jer u DecrypthFile metodi je zakomentarisan deo upisa u taj fajl

                string putanja = Directory.GetCurrentDirectory() + "\\Archive\\" + archiveFile;

                StreamWriter Writer = new StreamWriter(putanja, false);
                string aca = ASCIIEncoding.ASCII.GetString(desifrovano);
                aca = aca.Trim('\0');   //Zbog Paddinga ZEROS kod kriptografije ovde ce se pojaviti NULL (\0) karakteri i bice upisani u fajl
                Writer.Write(aca);      // ASCIENCODING.ASCII.GETSTRING() ovo za sad najbolje radi ali ispusje smece na pocetku (2 karaktera)    
                                        //Writer.Write(UnicodeEncoding.BigEndianUnicode.GetString(desifrovano)); ovo nije ispisuje nesto sacuvaj me boze
                                        // Writer.Write(ASCIIEncoding.BigEndianUnicode.GetString(desifrovano)); isto kao ovo iznad njega
                                        // Writer.Write(UnicodeEncoding.ASCII.GetString(desifrovano)); // ispusje smece samo na pocetku (2 karaktera)
                                        //Writer.Write(UTF8Encoding.UTF32.GetString(desifrovano)); ovaj UTF32 nije, a za UTF8 i UTF7 ispisuje ista kao za ASCII.GetString()
                                        //Writer.Write(Encoding.UTF8.GetString(desifrovano));
                Writer.Flush();
                Writer.Close();

                Console.WriteLine("Arhiviranje uradjenoooo");
            }
            else
            {
                Console.WriteLine("Za arhiviranje digitalni potpis nije validan");
            }
            
        }

        public void SendData(byte[] data, byte[] signature)
        {
            string clienName = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
            //clienName += "S_sign";
            clienName = clienName.Substring(0, 3) + "S";

            X509Certificate2 certificate2 = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clienName);

            if (DigitalSignature.Verify(data, signature, certificate2))
            {
                byte[] desifrovano = CryptographyManager.EncryptDecryptManager.DecrypthFile(data, "aca.txt",kljuc);

                StreamWriter Writer = new StreamWriter("replicirano.txt", false);
                string aca = ASCIIEncoding.ASCII.GetString(desifrovano);
                aca = aca.Trim('\0'); //Zbog Paddinga ZEROS kod kriptografije ovde ce se pojaviti NULL (\0) karakteri i bice upisani u fajl

                Writer.Write(aca);      // ASCIENCODING.ASCII.GETSTRING() ovo za sad najbolje radi ali ispusje smece na pocetku (2 karaktera)    
                                        //Writer.Write(UnicodeEncoding.BigEndianUnicode.GetString(desifrovano)); ovo nije ispisuje nesto sacuvaj me boze
                                        // Writer.Write(ASCIIEncoding.BigEndianUnicode.GetString(desifrovano)); isto kao ovo iznad njega
                                        // Writer.Write(UnicodeEncoding.ASCII.GetString(desifrovano)); // ispusje smece samo na pocetku (2 karaktera)
                                        //Writer.Write(UTF8Encoding.UTF32.GetString(desifrovano)); ovaj UTF32 nije, a za UTF8 i UTF7 ispisuje ista kao za ASCII.GetString()
                                        //Writer.Write(Encoding.UTF8.GetString(desifrovano));
                Writer.Flush();
                Writer.Close();
                Console.WriteLine("Replikacija uspesna!");
            }
            else
            {
                Console.WriteLine("Digitalni potpis nije validan");
            }
            
        }

        public void SendKey(byte[] key)
        {
            string clienName = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
            X509Certificate2 certificate2 = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clienName);
           // Console.WriteLine(ASCIIEncoding.ASCII.GetString(key));
            //Console.WriteLine("****************************************");
            string desifrovani_kljuc = KeyManager.Decrypt(key, certificate2);
            kljuc = desifrovani_kljuc;
            //Console.WriteLine(desifrovani_kljuc);
        }
    }
}
