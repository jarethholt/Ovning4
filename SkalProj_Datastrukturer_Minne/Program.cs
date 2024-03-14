namespace SkalProj_Datastrukturer_Minne;

class Program
{
    /// <summary>Handle the menus for the program.</summary>
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(
                "Please navigate through the menu by inputting the number "
                + "(1, 2, 3, 4, 5, 0) of your choice.\n"
                + "1. Examine a List\n"
                + "2. Examine a Queue\n"
                + "3. Examine a Stack\n"
                + "4. CheckParentheses\n"
                + "5. ReverseText\n"
                + "0. Exit the application"
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
                    CheckParentheses();
                    break;
                case '5':
                    ReverseText();
                    break;
                case '0':
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter some valid input (0, 1, 2, 3, 4, 5)");
                    Console.WriteLine("Press enter to try again.");
                    _ = Console.ReadLine();
                    break;
            }
        }
    }

    /// <summary>Examines the datastructure List.</summary>
    /// <remarks>
    /// Allow the user to manipulate a List of strings. The available actions are,
    /// like the main menu, controlled by the first character of each line:
    ///   '+': Add the rest of the input to the list.
    ///        For example, writing '+Adam' would add "Adam" to the list.
    ///   '-': (Attempt to) remove an item from the list.
    ///        For example, writing '-Adam' would remove "Adam" from the list
    ///        if present and print an error otherwise.
    ///   '0': Exit to the main menu.
    /// Entering any other character (or nothing) as the first character will
    /// print an error message and do nothing.
    /// After each item is added, the Count and Capacity of the list are printed.
    /// </remarks>
    static void ExamineList()
    {
        Console.Clear();
        Console.WriteLine(
            "Examine a List of strings.\n"
            + "To add 'value' to the list, type '+value'.\n"
            + "To remove 'value' from the list, type '-value'.\n"
            + "To exit to the main menu, type '0'.\n"
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
    /* Answers to questions relevant to this code:
     * 2. The list's capacity grows when the current capacity is *exceeded*. For example,
     *    if the capacity is 4, it is increased when trying to add the 5th element.
     * 3. The capacity is doubled each time: 1 -> 2 -> 4 -> 8 -> ...
     * 4. Growing the list requires allocating more space for data. It's expected that
     *    the list will grow, so it's more efficient to allocate plenty of space at once.
     *    The specific capacity increases suggest the list is backed by a binary heap.
     * 5. The capacity is never decreased despite the count falling below any amount of
     *    thresholds. As far as I can tell, List.TrimExcess is the only way to reduce
     *    the capacity of a list.
     * 6. An array is definitely better than a list anytime you know exactly what size
     *    you need beforehand. Otherwise I would say lists are a better choice. Since
     *    C# has List.EnsureCapacity, I don't think there's any amount of knowledge of
     *    the _approximate_ size that would make an array more efficient.
     */


    /// <summary>Examine the datastructure Queue.</summary>
    /// <remarks>
    /// Allow the user to manipulate a Queue of strings. The available actions are,
    /// like the main menu, controlled by the first character of each input line:
    ///   '+': Add the rest of the input to the queue.
    ///        For example, writing '+Adam' would add "Adam" to the queue.
    ///   '-': Pop the item at the front of the queue.
    ///        Gives an error message if the queue is empty.
    ///   '0': Exit to the main menu.
    /// Entering any other character (or nothing) as the first character will
    /// print an error message and do nothing.
    /// After each change the state of the queue is printed.
    /// </remarks>
    static void ExamineQueue()
    {
        Console.Clear();
        Console.WriteLine(
            "Examine a Queue of strings.\n"
            + "To add 'value' to the queue, type '+value'.\n"
            + "To pop an entry from the queue, type '-'.\n"
            + "To exit to the main menu, type '0'.\n"
        );

        Queue<string> queue = new();
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
            // Using Substring should be safe here even for strings of length 1
            string value = readResult[1..];

            switch (choice)
            {
                case '+':
                    queue.Enqueue(value);
                    Console.Write($"Added {value} to the Queue. ");
                    break;
                case '-':
                    if (queue.Count == 0)
                        Console.Write("Cannot pop an item, the Queue is empty. ");
                    else
                    {
                        string popped = queue.Dequeue();
                        Console.Write($"Popped {popped} from the Queue. ");
                    }
                    break;
                case '0':
                    Console.WriteLine("Press enter to return to the main menu.");
                    _ = Console.ReadLine();
                    return;
                default:
                    Console.WriteLine($"Could not parse choice {choice}, try again");
                    continue;
            }

            string queueString = string.Join(", ", [.. queue]);
            Console.WriteLine($"Current Queue: {queueString}");
        }
    }

    /// <summary>Examine the datastructure Stack.</summary>
    /// <remarks>
    /// Allow the user to manipulate a Stack of strings. The available actions are,
    /// like the main menu, controlled by the first character of each input line:
    ///   '+': Add the rest of the input to the stack.
    ///        For example, writing '+Adam' would add "Adam" to the stack.
    ///   '-': Pop the item at the front of the stack.
    ///        Gives an error message if the stack is empty.
    ///   '0': Exit to the main menu.
    /// Entering any other character (or nothing) as the first character will
    /// print an error message and do nothing.
    /// After each change the state of the stack is printed.
    /// </remarks>
    static void ExamineStack()
    {
        Console.Clear();
        Console.WriteLine(
            "Examine a Stack of strings.\n"
            + "To add 'value' to the stack, type '+value'.\n"
            + "To pop an entry from the stack, type '-'.\n"
            + "To exit to the main menu, type '0'.\n"
        );

        Stack<string> stack = new();
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
            // Using Substring should be safe here even for strings of length 1
            string value = readResult[1..];

            switch (choice)
            {
                case '+':
                    stack.Push(value);
                    Console.Write($"Added {value} to the Stack. ");
                    break;
                case '-':
                    if (stack.Count == 0)
                        Console.Write("Cannot pop an item, the Stack is empty. ");
                    else
                    {
                        string popped = stack.Pop();
                        Console.Write($"Popped {popped} from the Stack. ");
                    }
                    break;
                case '0':
                    Console.WriteLine("Press enter to return to the main menu.");
                    _ = Console.ReadLine();
                    return;
                default:
                    Console.WriteLine($"Could not parse choice {choice}, try again");
                    continue;
            }

            string stackString = string.Join(", ", [.. stack]);
            Console.WriteLine($"Current Stack: {stackString}");
        }
    }

    private static readonly Dictionary<char, char> _closerToOpener = new()
    {
        { ')', '(' },
        { ']', '[' },
        { '}', '{' }
    };

    /// <summary>Check if the various enclosers (), [], {} in a string are matched.</summary>
    static void CheckParentheses()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Examine if the enclosers in a string are correct.");
            Console.WriteLine("Please provide a string to check:");
            string? readResult = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(readResult))
            {
                Console.WriteLine("Please enter non-empty text:");
                readResult = Console.ReadLine();
            }

            // Keep a stack of the openers; check each closer against the top of the stack
            bool correct = true;
            Stack<(char,int)> openers = new();
            for (int currPosition = 0; currPosition < readResult.Length; currPosition++)
            {
                char curr = readResult[currPosition];
                if (_closerToOpener.ContainsValue(curr))
                {
                    openers.Push((curr,currPosition));
                    continue;
                }
                if (!_closerToOpener.ContainsKey(curr))
                    continue;

                // Are there any openers left on the stack?
                if (openers.Count == 0)
                {
                    // Draw an arrow pointing out the unmatchable closer
                    string arrowFormat = $"{{0,{currPosition+1}}}";
                    Console.WriteLine(string.Format(arrowFormat, '1'));
                    Console.WriteLine(
                        "This closer (1) is unmatchable."
                    );
                    correct = false;
                    break;
                }

                // Does the closer curr match the most recent opener?
                (char lastOpener, int openerPosition) = openers.Pop();
                if (lastOpener != _closerToOpener[curr])
                {
                    // Draw arrows pointing out the mismatch
                    string arrowFormat
                        = $"{{0,{openerPosition+1}}}{{1,{currPosition - openerPosition}}}";
                    Console.WriteLine(string.Format(arrowFormat, '1', '2'));
                    Console.WriteLine("The closer (2) does not match the most recent opener (1).");
                    correct = false;
                    break;
                }
            }

            if (correct)
                Console.WriteLine("The given text has correctly matched enclosers.");
            else
                Console.WriteLine("The given text does not have correctly matched enclosers.");

            do
            {
                Console.Write("Would you like to input another string [y/n]? ");
                readResult = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(readResult))
                {
                    Console.WriteLine("Please use non-empty input.");
                    continue;
                }

                char answer = readResult.ToLower()[0];
                if (answer == 'n')
                    return;
                else if (answer == 'y')
                    break;
                else
                    Console.WriteLine("Please enter an input starting with 'y' or 'n'.");
            } while (true);
        }
    }

    /// <summary>Take a string from the user and reverse it using a Stack.</summary>
    static void ReverseText()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Reverse text with help of a Stack.");
            Console.WriteLine("Please enter some text to reverse:");
            string? readResult = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(readResult))
            {
                Console.WriteLine("Please enter non-empty text:");
                readResult = Console.ReadLine();
            }

            // Construct a stack, feeding it the characters in readResult
            Stack<char> chars = new(readResult.ToCharArray());
            // Values are added to the "front" of the stack, so ToArray will reverse the characters
            string reversed = new(chars.ToArray());

            Console.WriteLine($"The reversed text: {reversed}");
            Console.WriteLine();

            do
            {
                Console.Write("Would you like to input another string [y/n]? ");
                readResult = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(readResult))
                {
                    Console.WriteLine("Please use non-empty input.");
                    continue;
                }

                char answer = readResult.ToLower()[0];
                if (answer == 'n')
                    return;
                else if (answer == 'y')
                    break;
                else
                    Console.WriteLine("Please enter an input starting with 'y' or 'n'.");
            } while (true);
        }
    }

}

