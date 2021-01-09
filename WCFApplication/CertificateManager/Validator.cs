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
				string kompanija = certificate.SubjectName.Name.Split(',')[2].Substring(3);

				if (!certificate.Issuer.Equals(kompanija))
				{
					throw new Exception("Certificate is not from the valid issuer.");
				}
		}
	}
}