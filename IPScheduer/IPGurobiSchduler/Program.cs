using System;
using Gurobi;
namespace IPGurobiSchduler
{

    /* Copyright 2019, Gurobi Optimization, LLC */

    /* This example reads a MIP model from a file, solves it and
       prints the objective values from all feasible solutions
       generated while solving the MIP. Then it creates the fixed
       model and solves that model. */

    /* Copyright 2019, Gurobi Optimization, LLC */

    /* This example formulates and solves the following simple MIP model:

         maximize    x +   y + 2 z
         subject to  x + 2 y + 3 z <= 4
                     x +   y       >= 1
                     x, y, z binary
    */



    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            TryGurobi();
        }

        public static void TryGurobi()
        {
            try
            {

                // Create an empty environment, set options and start
                GRBEnv env = new GRBEnv(true);
                env.Set("LogFile", "mip1.log");
                env.Start();

                // Create empty model
                GRBModel model = new GRBModel(env);

                // Create variables
                GRBVar x = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "x");
                GRBVar y = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "y");
                GRBVar z = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "z");

                // Set objective: maximize x + y + 2 z
                model.SetObjective(x + y + 2 * z, GRB.MAXIMIZE);

                // Add constraint: x + 2 y + 3 z <= 4
                model.AddConstr(x + 2 * y + 3 * z <= 4.0, "c0");

                // Add constraint: x + y >= 1
                model.AddConstr(x + y >= 1.0, "c1");

                // Optimize model
                model.Optimize();

                Console.WriteLine(x.VarName + " " + x.X);
                Console.WriteLine(y.VarName + " " + y.X);
                Console.WriteLine(z.VarName + " " + z.X);

                Console.WriteLine("Obj: " + model.ObjVal);

                // Dispose of model and env
                model.Dispose();
                env.Dispose();

            }
            catch (GRBException e)
            {
                Console.WriteLine("Error code: " + e.ErrorCode + ". " + e.Message);
            }
        }

    }
}
