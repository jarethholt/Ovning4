/* Answers to the pre-coding exercise questions:
 * 1. The stack works by holding the data and pointers relevant to current execution.
 *    In the context of code execution, the stack holds the methods to be executed in
 *    the order to execute them. Methods are popped off the top of the stack after they
 *    have run; but if they call other methods while running, those are added to the stack
 *    first and have to finish before the current method is considered finished.
 *    The memory heap is a general region of memory to be dynamically (runtime) allocated that
 *    can be accessed globally, not just in the current position of the stack.
 * 2. Value types (or struct) refer to objects that directly contain their data and are
 *    usually immutable. Assignments of new variables to this variable "copy by value",
 *    i.e. read and store the value without any further impact on this variable.
 *    Reference types (or class) refer to objects that do not directly contain their data.
 *    Instead, variables of a reference type point to the corresponding data in the heap.
 *    Assignments for these "copy by reference", i.e. create a pointer to the exact same
 *    data, allowing multiple variables the possibility of changing that data for all others.
 * 3. In ReturnValue, both x and y are the native int value types, so using y = x copies the
 *    value of x to the value of y. Setting y = 4 afterwards cannot change x.
 *    In ReturnValue2, both x and y are now reference types, so using y = x aims the pointer
 *    for the y data to the same place as x's pointer. Then setting y = 4 changes the data
 *    in the pointed location; when x retrieves that data, it gets the value 4.
 */

namespace Ovning4;

class Program
{
    private enum MenuOption
    {
        Exit,
        ExamineList,
        ExamineQueue,
        ExamineStack,
        CheckParentheses,
        ReverseText
    }
    private static readonly int _maxOptionLength =
        Enum.GetValues<MenuOption>().Select(option => $"{option}".Length).Max();
    private static readonly Dictionary<MenuOption, string> _descriptions = new()
    {
        { MenuOption.Exit            , "Exit the application."                  },
        { MenuOption.ExamineList     , "Examine the List type."                 },
        { MenuOption.ExamineQueue    , "Examine the Queue type."                },
        { MenuOption.ExamineStack    , "Examine the Stack type."                },
        { MenuOption.CheckParentheses, "Check a string for matching enclosers." },
        { MenuOption.ReverseText     , "Reverse a piece of text."               }
    };
    private static readonly Dictionary<MenuOption, Action> _actions = new()
    {
        { MenuOption.ExamineList     , ExamineList      },
        { MenuOption.ExamineQueue    , ExamineQueue     },
        { MenuOption.ExamineStack    , ExamineStack     },
        { MenuOption.CheckParentheses, CheckParentheses },
        { MenuOption.ReverseText     , ReverseText      },
    };

    static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("Please choose one of the options below.");
        string format = $"{{0}} : {{1,-{_maxOptionLength}}} : {{2}}";
        foreach (KeyValuePair<MenuOption, string> kvp in _descriptions)
        {
            MenuOption option = kvp.Key;
            string description = kvp.Value;
            Console.WriteLine(format, (int)option, option, description);
        }
    }

    /// <summary>Handle the menus for the program.</summary>
    static void Main()
    {
        while (true)
        {
            DisplayMenu();

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

    /* Answer to exercise 2.1: Write the state of an Ica Queue
     * a. ICA opens with an empty register         : {}
     * b. Kalle gets in line                       : {Kalle}
     * c. Greta gets in line                       : {Kalle, Greta}
     * d. Kalle gets checked out (leaves the queue): {Greta}
     * e. Stine gets in line                       : {Greta, Stine}
     * f. Greta gets checked out                   : {Stine}
     * g. Olle gets in line                        : {Stine, Olle}
     * In this way of writing down the Queue, people are added from the right
     * and removed from the left.
     * 
     * Answer to 2.2: The input to ExamineQueue that would simulate this line is:
     *   +Kalle
     *   +Greta
     *   -
     *   +Stine
     *   -
     *   +Olle
     * 
     * Answer to 3.1: Using a stack removes the most recently-added member.
     * The same input as 2.2 would lead instead to the following sequence of Stacks
     * (using the right side as the top of the stack):
     *   (init) -> {}
     *   +Kalle -> {Kalle}
     *   +Greta -> {Kalle, Greta}
     *   -      -> {Kalle}
     *   +Stine -> {Kalle, Stine}
     *   -      -> {Kalle}
     *   +Olle  -> {Kalle, Olle}
     * Kalle will never get checked out as long as Ica is busy!
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

    /* Answer to 4.1
     * Reading the string from left to right, every time a closer )/]/} is encountered,
     * it must match the *most recent unmatched* opener (/[/{ encountered. If not, you have part
     * of the string like the example '([)]' where the nominally-outer pair () is finished
     * before the inner one [] is. Once you do find a match for an inner pair, you can ignore
     * the opener; the "most recent unmatched" opener is then scrolled back by one.
     * 
     * A stack is the best data structure for this. While reading from left to right, keep
     * a stack of all the openers (/[/{ encountered. When a closer is encountered, it must
     * match the opener on the top of the stack. If not, the string is incorrectly matched
     * and you can stop. If it does match, pop that opener off the stack and continue.
     * If the stack is ever empty when a closer is encountered, it's unmatchable;
     * if the stack is *not* empty after the whole string is parsed, all those openers
     * are unmatchable.
     * 
     * Take the string '([{}]({}))' as an example. Going through each character
     * and looking at the resulting stack (without brackets, for clarity):
     *   Character (  -> Add to stack (
     *   [ -> (, [
     *   { -> (, [, {
     *   } matches {, { gets popped -> (, [
     *   ] matches [ -> (
     *   ( -> (, (
     *   { -> (, (, {
     *   } matches { -> (, (
     *   ) matches ( -> (
     *   ) matches ( -> empty
     */

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
                    Console.WriteLine("This closer (1) is unmatchable.");
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
            // If any openers are left over, they are unmatchable
            if (openers.Count != 0)
            {
                (_, int openerPosition) = openers.Pop();
                string arrowFormat = $"{{0,{openerPosition+1}}}";
                Console.WriteLine(string.Format(arrowFormat, '1'));
                Console.WriteLine("This opener (1) is unmatchable.");
                correct = false;
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

    /* Answer to 5.1
     * RecursiveOdd(1) -> (n == 1) -> return 1.
     * 
     * RecursiveOdd(3) -> (n != 1) -> RecursiveOdd(3-1) + 2
     *   RecursiveOdd(2) -> (n != 1) -> RecursiveOdd(2-1) + 2
     *     RecursiveOdd(1) -> (n == 1) -> return 1
     *   RecursiveOdd(2) -> RecursiveOdd(2-1) + 2 = 1 + 2 = 3
     * RecursiveOdd(3) -> RecursiveOdd(2) + 2 = 3 + 2 = 5.
     * 
     * RecursiveOdd(5) -> RecursiveOdd(5-1) + 2
     *   RecursiveOdd(4) -> RecursiveOdd(4-1) + 2 = 5 + 2 = 7
     * RecursiveOdd(5) -> RecursiveOdd(4) + 2 = 7 + 2 = 9
     * (Skipping past the evaluation of RecursiveOdd(3).)
     */

    static int RecursiveEven(int n) => n == 1 ? 2 : RecursiveEven(n - 1) + 2;

    static int RecursiveFibonacci(int n)
    {
        if (n == 0) return 1;
        else if (n == 1) return 1;
        else return RecursiveFibonacci(n - 1) + RecursiveFibonacci(n - 2);
    }

    /* Answer to 6.1
     * IterativeOdd(1):
     *   result = 1
     *   for (int i = 0; i < 1-1; i++)
     *     i = 0; i !< 1-1 ==> stop
     *   return result = 1
     * 
     * IterativeOdd(3):
     *   result = 1
     *   for (int i = 0; i < 3-1; i++)
     *     i = 0; i < 3-1 ==> result += 2; result = 3
     *     i = 1; i < 3-1 ==> result += 2; result = 5
     *     i = 2; i !< 3-1 ==> stop
     *   return result = 5
     * 
     * IterativeOdd(5):
     *   result = 1
     *   for (int i = 0; i < 5-1; i++)
     *     i = 0; i < 5-1 ==> result += 2; result = 3
     *     i = 1; i < 5-1 ==> result += 2; result = 5
     *     i = 2; i < 5-1 ==> result += 2; result = 7
     *     i = 3; i < 5-1 ==> result += 2; result = 9
     *     i = 4; i !< 5-1 ==> stop
     *   return result = 9
     */

    static int IterativeEven(int n)
    {
        int result = 2;
        for (int i = 0; i < n - 1; i++) result += 2;
        return result;
    }

    static int IterativeFibonacci(int n)
    {
        int result0 = 1;
        int result1 = 1;
        if (n == 0) return result0;
        else if (n == 1) return result1;

        for (int i = 1; i < n; i++)
        {
            int temp = result1;
            result1 += result0;
            result0 = temp;
        }
        return result1;
    }

    /* Answer to final question:
     * In their absolute forms, the iterative versions are more *memory*-friendly.
     * They require only a couple of integers (on the stack, and can be overwritten)
     * and no additional method executions to be added to the stack. The recursive
     * versions add new integers with each method call and add the method execution
     * itself to the stack.
     * However, there are a couple additional things that affect this "memory"-friendliness.
     * First, all of these recursive functions can be tail-call optimized. Since the
     * recursive call is last, it can take the same slot on the stack without having
     * to add a new method call. Second, recursive functions are often cached. The
     * cache takes up space in memory, but can lead to much faster calculation and,
     * in some cases, direct returns with no calculation (and hence memory or CPU
     * overhead) involved.
     */
}

