using SudokuSolver.Model;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string sud =  @"293040100
                            516230740
                            847156000
                            354002690
                            600415000
                            000900000
                            000394802
                            000600005
                            000521000";
            var sudoku = SudokuFactory.CreateFromString(sud);
            System.Console.WriteLine(sudoku.ToString());

            var solver = new Model.SudokuSolver(sudoku);
            solver.Solve();
            System.Console.WriteLine(solver.ToString());
            System.Console.ReadLine();
        }
    }
}
