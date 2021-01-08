using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CertificateManager
{
    public class Validator : X509CertificateValidator
	{
		public override void Validate(X509Certificate2 certificate)
		{

			

			if (CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name)) != null)
			{
				X509Certificate2 crt = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
					Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

				if (!certificate.Issuer.Equals(crt.Issuer))
				{
					throw new Exception("Certificate is not from the valid issuer.");
				}
            }
            else
            {
				
				Console.WriteLine("Certificate for this user doensnt exist");
            }

			
			
		}
	}
}