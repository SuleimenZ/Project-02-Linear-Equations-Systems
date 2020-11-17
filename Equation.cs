using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_02_Linear_Equations_Systems
{
    class Equation
    {
        public string InputString;
        public List<double> Parameters = new List<double>(); //Actually contains parameters AND right-hand side constant
        public List<bool> ParameterSigns = new List<bool>();

        public Equation(string inputString) 
        { 
            InputString = inputString; 

            if(CheckEquation(inputString))
            {
                CreateLists();
            }
            else
            {
                Console.WriteLine("There are incorrect characters in your input");
            }
        }

        public bool CheckEquation(string equation)
        {
            return equation.Replace("-", "").Replace(" ", "").All(char.IsDigit);
        }

        public void ShowFullEquation()
        {
            string result = "";
            int idOfUnknown = 1;
            string sign = ParameterSigns.First() ? "" : "-";

            result += $"{sign}{Parameters.First().ToString().Replace("-", "")}*x{idOfUnknown++} ";

            for (int i = 1; i < Parameters.Count() - 1; i++)
            {
                sign = ParameterSigns[i] ? "+" : "-";
                result += $"{sign} {Parameters[i].ToString().Replace("-", "")}*x{idOfUnknown++} ";
            }

            sign = ParameterSigns.Last() ? "" : "-";
            result += $"= {sign}{Parameters.Last().ToString().Replace("-", "")}";

            Console.WriteLine(result);
        }

        public void Divide(double divider)
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                Parameters[i] /= divider;
            }
        }
        public void Multiply(double multiplier)
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                Parameters[i] *= multiplier;
            }
        }
        public void Sum(Equation eq)
        {
            for (int i = 0; i < eq.Parameters.Count; i++)
            {
                Parameters[i] = eq.Parameters[i];
            }
        }
        public void Subtract(Equation eq) // does not change the values 
        {
            for (int i = 0; i < eq.Parameters.Count; i++)
            {
                Parameters[i] -= eq.Parameters[i];
            }
        }
        
        public void SwapWith(Equation eq)
        {
            List<double> tempParameters = new List<double>();
            tempParameters.AddRange(eq.Parameters);

            eq.Parameters.Clear();
            eq.Parameters.AddRange(Parameters);

            Parameters.Clear();
            Parameters.AddRange(tempParameters);
        }
        public Equation ReturnCopy()
        {
            var temp = new Equation("");
            temp.Parameters.AddRange(Parameters);
            temp.ParameterSigns.AddRange(ParameterSigns);
            return temp;
        }
        public double GetParameter(int id)
        {
            return Parameters[id];
        }
        public int GetNumOfParameters()
        {
            return Parameters.Count();
        }
        public List<double> GetParametersList()
        {
            return Parameters;
        }


        private void CreateLists()
        {
            if(string.IsNullOrWhiteSpace(InputString))
            {
                return;
            }

            Parameters = InputString.Split(' ').Select(Double.Parse).ToList();

            foreach (int parameter in Parameters)
            {
                if(parameter >= 0)
                {
                    ParameterSigns.Add(true);
                }
                else
                {
                    ParameterSigns.Add(false);
                }
            }
        }
    }
}
