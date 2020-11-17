using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_02_Linear_Equations_Systems
{
    class Matrix
    {
        private List<Equation> equationsList;
        private List<double> answers = new List<double>();

        public Matrix()
        {
            equationsList = new List<Equation>();
        }
        public Matrix(List<Equation> inputList) : this() => equationsList = inputList.ToList();

        public void AddEquation(string userInput)
        {
            equationsList.Add(new Equation(userInput));
        }

        public void ShowEquations()
        {
            int id = 1;
            foreach (Equation eq in equationsList)
            {
                Console.Write($"Equation {id++}: ");
                eq.ShowFullEquation();
            }
        }

        public void CheckMatrix()
        {
            int testNumOfParameters = equationsList[0].Parameters.Count;
            for (int i = 1; i < equationsList.Count; i++)
            {
                if (equationsList[i].Parameters.Count() != testNumOfParameters)
                {
                    Console.WriteLine($"ERROR: Number of parameters in all equations does not match\nFirst one has {testNumOfParameters}, " +
                                                                                $"{i}'s one has {equationsList[i].Parameters.Count()}");
                    throw new Exception("Number of parameters does not match");
                }
            }

            if (testNumOfParameters-1 > equationsList.Count())
            {
                Console.WriteLine("ERROR: Number of equations MUST BE greater or equal to number of parameters");
                throw new Exception("Invalid number of parameters/equations");
            }

            foreach (Equation eq in equationsList)
            {
                var temp = eq.ReturnCopy();
                temp.Parameters[temp.Parameters.Count - 1] = 0;
                if (temp.Parameters.All(param => param == 0))
                {
                    Console.WriteLine("ERROR: At least one of parameters must be non-zero value");
                    throw new Exception(" All parameters are equal to zero");
                }
            }
        }

        public void Solve()
        {
            CheckMatrix();
            CorrectEquationsOrder();

            for (int k =0; k < equationsList.Count; k++) //makes every number in main diagonal = 1, everything under it = 0
            {
                equationsList[k].Divide(equationsList[k].Parameters[k]);
                for (int i = k+1; i < equationsList.Count; i++)
                {
                    var temp = equationsList[k].ReturnCopy();
                    temp.Multiply(equationsList[i].Parameters[k]);
                    equationsList[i].Subtract(temp);
                }
            }

            for (int k = equationsList.Count - 1; k >= 0; k--) // makes everything above main diagonal = 0, so we have identity matrix and answers
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    var temp = equationsList[k].ReturnCopy();
                    temp.Multiply(equationsList[i].Parameters[k]);
                    equationsList[i].Subtract(temp);
                }
            }

            foreach (Equation eq in equationsList)
            {
                answers.Add(eq.Parameters.Last());
            }
        }

        public void ShowAnswers()
        {
            int id = 1;
            foreach (var answer in answers)
            {
                Console.WriteLine($"x{id++} = {answer}");
            }
        }

        private void CorrectEquationsOrder()
        {
            for (int k = 0; k < 2; k++) // not sure if algorithm is perfect, so run it 2 times
            {
                int i = 0;
                while (i < equationsList.Count)
                {

                    if (equationsList[i].Parameters[i] == 0)
                    {
                        if (i == equationsList.Count - 1)
                        {
                            equationsList[i].SwapWith(equationsList[0]);
                        }
                        else
                        {
                            var temp = equationsList[i].ReturnCopy();
                            equationsList.RemoveAt(i);
                            equationsList.Add(temp);
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }
}
