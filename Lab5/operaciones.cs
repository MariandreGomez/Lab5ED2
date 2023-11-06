using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab5
{
    public class CSVData //clase para leer el archivo csv
    {
        public CSVData() { }

        public CSVData(string v1, string v2)
        {
            operacion = v1;
            JSONData = v2;
        }

        public string operacion { get; set; }
        public string JSONData { get; set; }

    }

    public class Aplicante
    {
        public Aplicante(string nameC, string dpiC, string datebirthC, string adrressC, string reclutador)
        {
            name = nameC;
            dpi = dpiC;
            datebirth = datebirthC;
            address = adrressC;
            recluiter = reclutador;
        }
        public string name { get; set; }
        public string dpi { get; set; }
        public string datebirth { get; set; }
        public string address { get; set; }
        public List<string> companies { get; set; }
        public string recluiter { get; set; }
    }
    public class operaciones
    {
        TreeNode InfoPersona = new TreeNode(); //arbol
        public string clave = "SECRET"; //Clave para transpocision simple 
        public HashSet<Credenciales> credencial = new HashSet<Credenciales>();
        public void CargarDatos()
        {

            string[] CsvLines = System.IO.File.ReadAllLines(@"C:\Users\maria\Desktop\inputslab5\input.csv");

            //Listas auxiliares
            List<CSVData> insert = new List<CSVData>();
            List<CSVData> patch = new List<CSVData>();
            List<CSVData> eliminar = new List<CSVData>();

            for (int i = 0; i < CsvLines.Length; i++)
            {
                string[] rowdata = CsvLines[i].Split(';'); // lee el separador ";" 
                CSVData record = new CSVData(rowdata[0], rowdata[1]); //se inserta en la clase que contiene el jsondata y la operacion

                if (rowdata[0] == "INSERT")
                {
                    insert.Add(record);
                }
                else if (rowdata[0] == "PATCH")
                {
                    patch.Add(record);
                }
                else if (rowdata[0] == "DELETE")
                {
                    eliminar.Add(record);
                }
            }

            //Insertar en arbol 
            foreach (CSVData item in insert)
            {
                Aplicante person = JsonConvert.DeserializeObject<Aplicante>(item.JSONData);
                InfoPersona.Insertar(person);
            }

            //Actualizar informacion 
            string dpi = "";
            foreach (var item in patch)
            {
                Aplicante personaPatch = JsonConvert.DeserializeObject<Aplicante>(item.JSONData);
                dpi = personaPatch.dpi;
                InfoPersona.ActualizarNodo(dpi, personaPatch);
            }

            //Eliminar informacion          
            foreach (var item in eliminar)
            {
                Aplicante personaDelete = JsonConvert.DeserializeObject<Aplicante>(item.JSONData);
                InfoPersona.Eliminar(personaDelete);
            }

            Decifrar();
            

            //Crear Credenciales
            //RecorrerInordenRec(InfoPersona);
            RecorrerInorden();

            //validar Credenciales
            string rec, comp, contra;
            Console.WriteLine("Ingresar reclutador");
            rec = Console.ReadLine();
            Console.WriteLine("Ingresar compañia");
            comp = Console.ReadLine();
            Console.WriteLine("Ingresar contraseña");
            contra = Console.ReadLine();

            string rec1, comp1,contra1;
           

            foreach (Credenciales item in credencial)
            {
                rec1 = item.Recluiter;
                comp1 = item.Company;
                contra1 = Descifrar(item.Password, clave);
                

                if (rec == rec1 )
                {

                    if (comp == comp1)
                    {
                        if (contra == contra1)
                        {
                            Console.WriteLine("Credenciales validas");
                            Console.ReadLine();
                            Environment.Exit(0);
                                          
                        }
                        else
                        {
                            Console.WriteLine("Credenciales No validas");
                            
                        }
                    }
                   

                }
               
            }

        }

        public void RecorrerInorden()
        {
            RecorrerInordenRec(InfoPersona.root);
        }

        public void RecorrerInordenRec(TreeNode nodo)
        {
            if (nodo != null)
            {
                RecorrerInordenRec(nodo.Left);

                foreach (string company in nodo.Data.companies)
                {
                    string reclutador = nodo.Data.recluiter;
                    string mensaje = reclutador + company;
                    string contenidoCifrado = Cifrar(mensaje, clave);
                    
                    credencial.Add(new Credenciales
                    {

                        Company = company,
                        Recluiter = reclutador,
                        Password = contenidoCifrado

                    });
                                      
                }

                RecorrerInordenRec(nodo.Right);

            }
        }

        public void Decifrar()
        {
            string ArchivoCifrado = @"C:\Users\maria\Desktop\inputslab5\inputs";
            string nombre, dpi, DPIBuscar;

            Console.WriteLine("Ingrese DPI de la persona que desea buscar");
            DPIBuscar = Console.ReadLine();

            Aplicante personaEncontrada = InfoPersona.BuscarPorDPI(DPIBuscar);
            if (personaEncontrada != null)
            {
                Console.WriteLine("Persona encontrada(Nombre): " + personaEncontrada.name);
                Console.WriteLine("Persona encontrada(DPI): " + personaEncontrada.dpi);
                Console.WriteLine("Persona encontrada(Direccion): " + personaEncontrada.address);
                Console.WriteLine();

                if (Directory.Exists(ArchivoCifrado))
                {
                    string[] archivosCifrados = Directory.GetFiles(ArchivoCifrado, "REC-*.txt");
                    foreach (var item in archivosCifrados)
                    {
                        // Obtener nombre completo del archivo cifrado
                        nombre = Path.GetFileNameWithoutExtension(item);
                        // Separar el nombre del archivo para obtener el DPI
                        string[] partesNombre = nombre.Split('-');
                        dpi = partesNombre[1];

                        if (dpi == DPIBuscar)
                        {
                            // Obtener el contenido cifrado del archivo
                            string contenidoCifrado = File.ReadAllText(item);
                            // Descifrar el contenido
                            
                            // Mostrar el contenido descifrado
                            Console.WriteLine($"Contenido descifrado de {nombre}:");
                            Console.WriteLine();
                            Console.WriteLine(contenidoCifrado);
                            Console.WriteLine("-------------------------------------");
                        }
                    }
                }

            }
            else
            {
                Console.WriteLine("Persona no encontrada.");
            }
        }

        public string Cifrar(string mensaje, string clave)
        {
            int numRows = (int)Math.Ceiling((double)mensaje.Length / clave.Length);
            char[,] grid = new char[numRows, clave.Length];

            int messageIndex = 0;

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < clave.Length; j++)
                {
                    if (messageIndex < mensaje.Length)
                    {
                        grid[i, j] = mensaje[messageIndex];
                        messageIndex++;
                    }
                    else
                    {
                        grid[i, j] = ' ';
                    }
                }
            }

            int[] order = new int[clave.Length];
            for (int i = 0; i < clave.Length; i++)
            {
                order[i] = i;
            }

            for (int i = 0; i < clave.Length - 1; i++)
            {
                for (int j = i + 1; j < clave.Length; j++)
                {
                    if (clave[i] > clave[j])
                    {
                        char temp = clave[i];
                        clave = clave.Remove(i, 1).Insert(i, clave[j].ToString());
                        clave = clave.Remove(j, 1).Insert(j, temp.ToString());

                        int tempOrder = order[i];
                        order[i] = order[j];
                        order[j] = tempOrder;
                    }
                }
            }

            string mensajeCifrado = "";

            for (int i = 0; i < clave.Length; i++)
            {
                int col = Array.IndexOf(order, i);

                for (int j = 0; j < numRows; j++)
                {
                    mensajeCifrado += grid[j, col];
                }
            }

            return mensajeCifrado;
        }

        public string Descifrar(string mensajeCifrado, string clave)
        {
            int numRows = (int)Math.Ceiling((double)mensajeCifrado.Length / clave.Length);
            char[,] grid = new char[numRows, clave.Length];

            int[] order = new int[clave.Length];
            for (int i = 0; i < clave.Length; i++)
            {
                order[i] = i;
            }

            for (int i = 0; i < clave.Length - 1; i++)
            {
                for (int j = i + 1; j < clave.Length; j++)
                {
                    if (clave[i] > clave[j])
                    {
                        char temp = clave[i];
                        clave = clave.Remove(i, 1).Insert(i, clave[j].ToString());
                        clave = clave.Remove(j, 1).Insert(j, temp.ToString());

                        int tempOrder = order[i];
                        order[i] = order[j];
                        order[j] = tempOrder;
                    }
                }
            }

            int colLength = mensajeCifrado.Length / numRows;
            int index = 0;

            for (int i = 0; i < clave.Length; i++)
            {
                int col = Array.IndexOf(order, i);

                for (int j = 0; j < numRows; j++)
                {
                    grid[j, col] = mensajeCifrado[index];
                    index++;
                }
            }

            string mensajeDescifrado = "";

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < clave.Length; j++)
                {
                    mensajeDescifrado += grid[i, j];
                }
            }

            return mensajeDescifrado.Trim();
        }


    }
}
