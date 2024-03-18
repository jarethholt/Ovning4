using System.Collections;

namespace Ovning4;

public class PushPop
{
    // Interface to make a class look like a stack (push and pop methods)
    internal interface IPushPop<T> : IReadOnlyCollection<T>
    {
        public string BaseName { get; }
        public void Push(T item);
        public T? Pop(T item);
    }

    // Inner loop method to update a StackLike object
    private static bool UpdatePushPop<TCollector>(
        TCollector pushPop,
        Action<TCollector> callback
    ) where TCollector : IPushPop<string>
    {
        bool again = true;
        string? readResult = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(readResult))
        {
            Console.WriteLine("Empty input, try again.");
            return again;
        }
        char choice = readResult[0];
        // Using Substring should be safe here even for strings of length 1
        string value = readResult[1..];

        switch (choice)
        {
            case '+':
                pushPop.Push(value);
                Console.Write($"Added {value} to the {pushPop.BaseName}. ");
                break;
            case '-':
                if (pushPop.Count == 0)
                    Console.Write($"Cannot pop an item, the {pushPop.BaseName} is empty. ");
                else
                {
                    string? popped = pushPop.Pop(value);
                    if (popped is null)
                        Console.Write($"{value} is not in the {pushPop.BaseName}. ");
                    else
                        Console.Write($"Popped {popped} from the {pushPop.BaseName}. ");
                }
                break;
            case '0':
                Console.WriteLine("Press enter to return to the main menu.");
                _ = Console.ReadLine();
                again = false;
                return again;
            default:
                Console.WriteLine($"Could not parse choice {choice}, try again.");
                return again;
        }

        callback(pushPop);
        return again;
    }

    // Main menu method to examine an IPushPop.
    private static void ExaminePushPop<TCollector>(
        TCollector pushPop,
        Action<TCollector> callback
    ) where TCollector : IPushPop<string>
    {
        Console.Clear();
        Console.WriteLine(
            $"Examine a {pushPop.BaseName} of strings.\n"
            + $"To add 'value' to the {pushPop.BaseName}, type '+value'.\n"
            + $"To pop an entry from the {pushPop.BaseName}, type '-value'.\n"
            + "To exit to the main menu, type '0'.\n"
        );

        bool again;
        do
        {
            again = UpdatePushPop(pushPop, callback);
        } while (again);
    }

    // Make a List into a PushPop
    private class ListToPushPopAdapter<T> : IPushPop<T>
    {
        private readonly List<T> _list = [];
        public string BaseName => "List";
        public int Count => _list.Count;
        public int Capacity => _list.Capacity;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public T? Pop(T item)
        {
            bool success = _list.Remove(item);
            return success ? item : default;
        }

        public void Push(T item) => _list.Add(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    // Callback function to present state of the List
    private static readonly Action<ListToPushPopAdapter<string>> LCallback = new(pushPop =>
    {
        Console.WriteLine($"Count: {pushPop.Count}; Capacity: {pushPop.Capacity}");
    });

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
    public static void ExamineList()
    {
        ListToPushPopAdapter<string> pushPop = new();
        ExaminePushPop(pushPop, LCallback);
    }

    // Adapter to make a Queue look like a PushPop
    private class QueueToPushPopAdapter<T> : IPushPop<T>
    {
        private readonly Queue<T> _queue = new();
        public string BaseName => "Queue";
        public int Count => _queue.Count;

        public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

        public T? Pop() => _queue.Dequeue();
        public T? Pop(T item) => this.Pop();

        public void Push(T item) => _queue.Enqueue(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    private static readonly Action<IPushPop<string>> QSCallback = new(pushPop =>
    {
        string pushPopString = string.Join(", ", [.. pushPop]);
        Console.WriteLine($"Current {pushPop.BaseName}: {pushPopString}");
    });

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
    public static void ExamineQueue()
    {
        QueueToPushPopAdapter<string> pushPop = new();
        ExaminePushPop(pushPop, QSCallback);
    }

    // Adapter to make a Stack look like a Stack
    private class StackToPushPopAdapter<T> : IPushPop<T>
    {
        private readonly Stack<T> _stack = new();
        public string BaseName => "Stack";

        public int Count => _stack.Count;

        public IEnumerator<T> GetEnumerator() => _stack.GetEnumerator();

        public T Pop() => _stack.Pop();
        public T? Pop(T item) => this.Pop();

        public void Push(T item) => _stack.Push(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
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
    public static void ExamineStack()
    {
        StackToPushPopAdapter<string> pushPop = new();
        ExaminePushPop(pushPop, QSCallback);
    }
}
