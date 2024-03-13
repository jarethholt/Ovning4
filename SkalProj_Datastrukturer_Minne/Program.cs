namespace SkalProj_Datastrukturer_Minne;

class Program
{
    /// <summary>
    /// Handle the menus for the program.
    /// </summary>
    static void Main()
    {
        while (true)
        {
            Console.WriteLine(
                "Please navigate through the menu by inputting the number"
                + "\n(1, 2, 3 ,4, 0) of your choice."
                + "\n1. Examine a List"
                + "\n2. Examine a Queue"
                + "\n3. Examine a Stack"
                + "\n4. CheckParenthesis"
                + "\n0. Exit the application"
            );

            // Create the character input to be used with the switch-case below.
            // Read the first character of any input.
            // If no input is given, ask the user for some.
            char input = ' ';
            try
            {
                input = Console.ReadLine()![0];
            }
            catch (IndexOutOfRangeException)
            {
                Console.Clear();
                Console.WriteLine("Please enter some input!");
            }

            switch (input)
            {
                case '1':
                    ExamineList();
                    break;
                case '2':
                    ExamineQueue();
                    break;
                case '3':
                    ExamineStack();
                    break;
                case '4':
                    CheckParanthesis();
                    break;
                /*
                 * Extend the menu to include the recursive 
                 * and iterative exercises.
                 */
                case '0':
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter some valid input (0, 1, 2, 3, 4)");
                    break;
            }
        }
    }

    /// <summary>
    /// Examines the datastructure List.
    /// </summary>
    /// <remarks>
    /// Allow the user to manipulate a List of strings. The available actions are,
    /// like the main menu, controlled by the first character of each line:
    ///     '+': Add the rest of the input to the list.
    ///          For example, writing '+Adam' would add "Adam" to the list.
    ///     '-': (Attempt to) remove an item from the list.
    ///          For example, writing '-Adam' would remove "Adam" from the list
    ///          if present and print an error otherwise.
    ///     '0': Exit to the main menu.
    /// After each item is added, the Count and Capacity of the list are printed.
    /// </remarks>
    static void ExamineList()
    {

        //List<string> theList = new List<string>();
        //string input = Console.ReadLine();
        //char nav = input[0];
        //string value = input.substring(1);

        //switch(nav){...}
    }

    /// <summary>
    /// Examine the datastructure Queue.
    /// </summary>
    static void ExamineQueue()
    {
        /*
         * Loop this method untill the user inputs something to exit to main menue.
         * Create a switch with cases to enqueue items or dequeue items
         * Make sure to look at the queue after Enqueueing and Dequeueing to see how it behaves
         */
    }

    /// <summary>
    /// Examine the datastructure Stack.
    /// </summary>
    static void ExamineStack()
    {
        /*
         * Loop this method until the user inputs something to exit to main menue.
         * Create a switch with cases to push or pop items
         * Make sure to look at the stack after pushing and and poping to see how it behaves
         */
    }

    /// <summary>
    /// Check if the various brackets (), [], {} in a string are matched.
    /// </summary>
    static void CheckParanthesis()
    {
        /*
         * Use this method to check if the paranthesis in a string is Correct or incorrect.
         * Example of correct: (()), {}, [({})],  List<int> list = new List<int>() { 1, 2, 3, 4 };
         * Example of incorrect: (()]), [), {[()}],  List<int> list = new List<int>() { 1, 2, 3, 4 );
         */

    }

}

