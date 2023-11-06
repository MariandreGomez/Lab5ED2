using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public class TreeNode
    {
        public Aplicante Data { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeNode(Aplicante data)
        {
            Data = data;
            Left = null;
            Right = null;
        }

        public TreeNode()
        {
        }

        public TreeNode root; // Raíz del árbol

        //Insertar Elementos
        public void Insertar(Aplicante persona)
        {
            root = InsertarRecursivamente(root, persona);
        }

        private TreeNode InsertarRecursivamente(TreeNode raiz, Aplicante persona)
        {
            if (raiz == null)
            {
                raiz = new TreeNode(persona);
                return raiz;
            }

            if (string.Compare(persona.dpi, raiz.Data.dpi) < 0)
            {
                raiz.Left = InsertarRecursivamente(raiz.Left, persona);
            }
            else if (string.Compare(persona.dpi, raiz.Data.dpi) > 0)
            {
                raiz.Right = InsertarRecursivamente(raiz.Right, persona);
            }

            return raiz;
        }


        //Actualizar 
        public void ActualizarNodo(string dpi, Aplicante nuevaInformacion)
        {
            root = ActualizarRecursivamente(root, dpi, nuevaInformacion);
        }

        private TreeNode ActualizarRecursivamente(TreeNode nodoActual, string dpi, Aplicante nuevaInformacion)
        {
            if (nodoActual == null)
            {
                // El nodo no se encuentra en el árbol, no se realiza ninguna actualización.
                return null;
            }

            int comparacion = string.Compare(dpi, nodoActual.Data.dpi);

            if (comparacion < 0)
            {
                // La clave está en el subárbol izquierdo.
                nodoActual.Left = ActualizarRecursivamente(nodoActual.Left, dpi, nuevaInformacion);
            }
            else if (comparacion > 0)
            {
                // La clave está en el subárbol derecho.
                nodoActual.Right = ActualizarRecursivamente(nodoActual.Right, dpi, nuevaInformacion);
            }
            else
            {
                // Se encontró el nodo con la clave DPI y se actualiza la información.
                nodoActual.Data = nuevaInformacion;
            }

            return nodoActual;
        }


        //Eliminar 
        public void Eliminar(Aplicante persona)
        {
            root = EliminarRecursivamente(root, persona);
        }

        private TreeNode EliminarRecursivamente(TreeNode nodo, Aplicante persona)
        {
            if (nodo == null)
            {
                return nodo;
            }

            // Si el Dpi de la persona es menor que el Dpi del nodo actual, buscar en el subárbol izquierdo
            if (string.Compare(persona.dpi, nodo.Data.dpi) < 0)
            {
                nodo.Left = EliminarRecursivamente(nodo.Left, persona);
            }
            // Si el Dpi de la persona es mayor que el Dpi del nodo actual, buscar en el subárbol derecho
            else if (string.Compare(persona.dpi, nodo.Data.dpi) > 0)
            {
                nodo.Right = EliminarRecursivamente(nodo.Right, persona);
            }
            // Si encontramos el nodo que contiene la persona, procedemos a eliminarlo
            else
            {
                // Caso 1: El nodo tiene 0 o 1 hijo
                if (nodo.Left == null)
                {
                    return nodo.Right;
                }
                else if (nodo.Right == null)
                {
                    return nodo.Left;
                }

                // Caso 2: El nodo tiene 2 hijos
                // Encontrar el sucesor inorden (el nodo más pequeño en el subárbol derecho)
                nodo.Data = EncontrarSucesorInorden(nodo.Right);
                // Eliminar el sucesor inorden
                nodo.Right = EliminarRecursivamente(nodo.Right, nodo.Data);
            }

            return nodo;
        }

        private Aplicante EncontrarSucesorInorden(TreeNode nodo)
        {
            Aplicante sucesor = nodo.Data;
            while (nodo.Left != null)
            {
                sucesor = nodo.Left.Data;
                nodo = nodo.Left;
            }
            return sucesor;
        }

        private Aplicante FindMinValue(TreeNode node)
        {
            Aplicante minValue = node.Data;
            while (node.Left != null)
            {
                minValue = node.Left.Data;
                node = node.Left;
            }
            return minValue;
        }

        //Busqueda
        public Aplicante BuscarPorDPI(string dpi)
        {
            return BuscarPorDPIRecursivo(root, dpi);
        }

        private Aplicante BuscarPorDPIRecursivo(TreeNode nodoActual, string dpi)
        {
            if (nodoActual == null)
            {
                return null; // El nodo actual es nulo, lo que significa que la persona no se encontró en el árbol.
            }

            int comparacion = string.Compare(dpi, nodoActual.Data.dpi);

            if (comparacion == 0)
            {
                return nodoActual.Data; // Encontramos la persona con el DPI deseado.
            }
            else if (comparacion < 0)
            {
                return BuscarPorDPIRecursivo(nodoActual.Left, dpi); // Buscar en el subárbol izquierdo.
            }
            else
            {
                return BuscarPorDPIRecursivo(nodoActual.Right, dpi); // Buscar en el subárbol derecho.
            }
        }


        //contar
        public int ContarElementos()
        {
            return ContarElementosRec(root);
        }

        private int ContarElementosRec(TreeNode node)
        {
            if (node == null)
            {
                return 0;
            }

            // Suma 1 para el nodo actual y luego recursivamente suma los elementos de los subárboles izquierdo y derecho.
            return 1 + ContarElementosRec(node.Left) + ContarElementosRec(node.Right);
        }
        
        //mostrar
        public void MostrarArbol()
        {
            MostrarArbolRec(root);
        }

        private void MostrarArbolRec(TreeNode node)
        {
            if (node != null)
            {
                MostrarArbolRec(node.Left);
                Console.WriteLine($"Nombre: {node.Data.name}");
                Console.WriteLine($"DPI: {node.Data.dpi}");
                Console.WriteLine($"Fecha de Nacimiento: {node.Data.datebirth}");
                Console.WriteLine($"Dirección: {node.Data.address}");
                Console.WriteLine($"Reclutadir: {node.Data.recluiter}");
                Console.WriteLine("Compañías:");
                foreach (var compañia in node.Data.companies)
                {
                    Console.WriteLine($"- {compañia}");
                }
                Console.WriteLine("-------------------------------");
                MostrarArbolRec(node.Right);
            }
        }
    }
}
