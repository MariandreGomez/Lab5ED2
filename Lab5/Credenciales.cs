using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public class Credenciales
    {
        private string recluiter;
        private string company;
        private string password;

        public string Recluiter
        {
            get { return recluiter; }
            set { recluiter = value; }
        }

        // Propiedad pública para obtener y establecer el valor de "company"
        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        // Propiedad pública para obtener y establecer el valor de "password"
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
