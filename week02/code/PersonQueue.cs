using System.Collections.Generic; // Required for List<T>

/// <summary>
/// A basic implementation of a Queue
/// </summary>
public class PersonQueue
{
    private readonly List<Person> _queue = new();

    public int Length => _queue.Count;

    /// <summary>
    /// Add a person to the queue. New elements are added to the back (end) of the queue.
    /// </summary>
    /// <param name="person">The person to add</param>
    public void Enqueue(Person person)
    {
        // For a FIFO queue, new elements are added to the back.
        _queue.Add(person); // Corrected: Add to the end for FIFO behavior.
    }

    /// <summary>
    /// Removes and returns the person at the front (beginning) of the queue.
    /// </summary>
    /// <returns>The person at the front of the queue.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown if the queue is empty.</exception>
    public Person Dequeue()
    {
        // Check if the queue is empty before attempting to dequeue.
        if (IsEmpty())
        {
            throw new System.InvalidOperationException("Cannot dequeue from an empty queue.");
        }

        // For a FIFO queue, elements are removed from the front.
        var person = _queue[0];
        _queue.RemoveAt(0);
        return person;
    }

  
    public bool IsEmpty()
    {
        return Length == 0;
    }

   
    public override string ToString()
    {
        return $"[{string.Join(", ", _queue)}]";
    }
}