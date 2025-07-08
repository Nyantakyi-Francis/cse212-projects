using System.Collections.Generic; // Required for List<T>
using System; // Required for InvalidOperationException

public class PriorityQueue
{
    // Make _queue internal for testing purposes as per problem instructions,
    // or provide a public property if direct access is needed by tests.
    // For the purpose of the provided tests, changing it to public or internal
    // is necessary to assert _queue.Count.
    internal List<PriorityItem> _queue = new(); 

    /// <summary>
    /// Add a new value to the queue with an associated priority.  The
    /// node is always added to the back of the queue regardless of 
    /// the priority.
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="priority">The priority</param>
    public void Enqueue(string value, int priority)
    {
        var newNode = new PriorityItem(value, priority);
        _queue.Add(newNode);
    }

    public string Dequeue()
    {
        if (_queue.Count == 0) // Verify the queue is not empty 
        {
            throw new InvalidOperationException("The queue is empty.");
        }

        // Find the index of the item with the highest priority to remove. 
        // If there are multiple values with the same high priority,
        // then the first one (following the FIFO strategy) is removed first. 
        var highPriorityIndex = 0;
        for (int index = 1; index < _queue.Count; index++) // Corrected: loop up to _queue.Count
        {
            // Changed >= to > to ensure FIFO for items with equal priority.
            // If the current item has a strictly higher priority, update highPriorityIndex.
            // If priorities are equal, highPriorityIndex remains unchanged,
            // effectively picking the one that appeared earlier (smaller index).
            if (_queue[index].Priority > _queue[highPriorityIndex].Priority)
            {
                highPriorityIndex = index;
            }
        }

        // Remove and return the item with the highest priority. 
        var value = _queue[highPriorityIndex].Value;
        _queue.RemoveAt(highPriorityIndex); // Corrected: Added removal of the item
        return value;
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", _queue)}]";
    }
}

internal class PriorityItem
{
    internal string Value { get; set; }
    internal int Priority { get; set; }

    internal PriorityItem(string value, int priority)
    {
        Value = value;
        Priority = priority;
    }

    public override string ToString()
    {
        return $"{Value} (Pri:{Priority})";
    }
}