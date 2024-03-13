﻿namespace SkalProj_Datastrukturer_Minne;

class Program
{
    /// <summary>
    /// Handle the menus for the program.
    /// </summary>
    static void Main()
    {
        while (true)
        {
            Console.Clear();
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
    /// Entering any other character (or nothing) as the first character will
    /// print an error message and do nothing.
    /// After each item is added, the Count and Capacity of the list are printed.
    /// </remarks>
    static void ExamineList()
    {
        Console.Clear();
        Console.WriteLine(
            "Examine a List of strings.\n"
            + "To add a 'value' to the list, type '+value'.\n"
            + "To remove 'value' from the list, type '-value'.\n"
            + "To exit to the main menu, type '0'.\n\n"
        );

        List<string> list = [];
        string? readResult;

        while (true)
        {
            readResult = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(readResult))
            {
                Console.WriteLine("Empty input, try again.");
                continue;
            }
            char choice = readResult[0];
            string value = readResult[1..];

            switch (choice)
            {
                case '+':
                    list.Add(value);
                    Console.Write($"Added {value}. ");
                    break;
                case '-':
                    bool success = list.Remove(value);
                    if (success)
                        Console.Write($"Removed {value}. ");
                    else
                        Console.Write($"{value} is not in the list. ");
                    break;
                case '0':
                    Console.WriteLine("Press enter to return to the main menu.");
                    _ = Console.ReadLine();
                    return;
                default:
                    Console.WriteLine($"Could not parse choice {choice}, try again");
                    continue;
            }

            Console.WriteLine($"Count: {list.Count}; Capacity: {list.Capacity}");
        }
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

