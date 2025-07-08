using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue items with varying priorities and ensure Dequeue returns the highest priority item.
    // Expected Result: Orange (Pri:3), Apple (Pri:2), Banana (Pri:1)
    // Defect(s) Found:
    // 1. The loop for finding highPriorityIndex in Dequeue iterated only up to `_queue.Count - 1`
    //    instead of `_queue.Count`, thus skipping the last element during the priority comparison.
    // 2. When multiple items had the same highest priority, the original Dequeue method returned
    //    the *last* item found with that priority, not the *first* (FIFO). The comparison
    //    `_queue[index].Priority >= _queue[highPriorityIndex].Priority` needed to be
    //    changed to `_queue[index].Priority > _queue[highPriorityIndex].Priority`
    //    to ensure FIFO for equal priorities, or handle the equality correctly while traversing.
    //    The fix ensures that if priorities are equal, the earlier element (smaller index) is preferred.
    // 3. The `_queue.RemoveAt(highPriorityIndex)` was missing after selecting the value.
    public void TestPriorityQueue_HighestPriorityAndOrder()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Banana", 1);
        priorityQueue.Enqueue("Apple", 2);
        priorityQueue.Enqueue("Orange", 3);

        // Expected: Orange (highest priority)
        Assert.AreEqual("Orange", priorityQueue.Dequeue());
        // Expected: Apple
        Assert.AreEqual("Apple", priorityQueue.Dequeue());
        // Expected: Banana
        Assert.AreEqual("Banana", priorityQueue.Dequeue());

        Assert.AreEqual(0, priorityQueue._queue.Count); // Ensure queue is empty
    }

    [TestMethod]
    // Scenario: Enqueue items with the same highest priority and ensure Dequeue returns the one added first.
    // Expected Result: Apple (Pri:5), Orange (Pri:5), Banana (Pri:1)
    // Defect(s) Found:
    // 1. The original `Dequeue` method would incorrectly return "Orange" first due to the `>=` comparison
    //    and iterating from the beginning, which would favor later elements with equal priority.
    //    The fix ensures that for equal priorities, the item encountered first (closer to the front) is selected.
    // 2. The `_queue.RemoveAt(highPriorityIndex)` was missing after selecting the value.
    public void TestPriorityQueue_SameHighestPriorityFIFO()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Apple", 5);
        priorityQueue.Enqueue("Banana", 1);
        priorityQueue.Enqueue("Orange", 5);
        priorityQueue.Enqueue("Grape", 3);

        // Expected: Apple (first of the highest priority items)
        Assert.AreEqual("Apple", priorityQueue.Dequeue());
        // Expected: Orange (next highest, which is 5 again, and Orange was next in original queue after Apple)
        Assert.AreEqual("Orange", priorityQueue.Dequeue());
        // Expected: Grape (next highest priority)
        Assert.AreEqual("Grape", priorityQueue.Dequeue());
        // Expected: Banana
        Assert.AreEqual("Banana", priorityQueue.Dequeue());

        Assert.AreEqual(0, priorityQueue._queue.Count); // Ensure queue is empty
    }

    [TestMethod]
    // Scenario: Dequeue from an empty PriorityQueue.
    // Expected Result: InvalidOperationException with message "The queue is empty."
    // Defect(s) Found: None. The existing `Dequeue` correctly throws an `InvalidOperationException`.
    public void TestPriorityQueue_EmptyQueueException()
    {
        var priorityQueue = new PriorityQueue();
        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown for an empty queue.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw; // Re-throw Assert.Fail
        }
        catch (Exception e)
        {
            Assert.Fail($"Unexpected exception type: {e.GetType().Name}. Message: {e.Message}");
        }
    }

    [TestMethod]
    // Scenario: Enqueue and Dequeue multiple times, ensuring correct state after operations.
    // Expected Result: The queue behaves as expected after a sequence of operations.
    // Defect(s) Found:
    // 1. The `_queue.RemoveAt(highPriorityIndex)` was missing after selecting the value in `Dequeue`.
    public void TestPriorityQueue_MixedOperations()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 5);
        priorityQueue.Enqueue("C", 2);
        priorityQueue.Enqueue("D", 5); // D and B have same high priority

        Assert.AreEqual("B", priorityQueue.Dequeue()); // B should be first of (B,D) due to FIFO for equal priority
        Assert.AreEqual(3, priorityQueue._queue.Count);

        priorityQueue.Enqueue("E", 10); // Highest priority now
        priorityQueue.Enqueue("F", 0);  // Lowest priority

        Assert.AreEqual("E", priorityQueue.Dequeue());
        Assert.AreEqual(4, priorityQueue._queue.Count);

        Assert.AreEqual("D", priorityQueue.Dequeue()); // D is next highest
        Assert.AreEqual(3, priorityQueue._queue.Count);

        Assert.AreEqual("C", priorityQueue.Dequeue());
        Assert.AreEqual(2, priorityQueue._queue.Count);

        Assert.AreEqual("A", priorityQueue.Dequeue());
        Assert.AreEqual(1, priorityQueue._queue.Count);

        Assert.AreEqual("F", priorityQueue.Dequeue());
        Assert.AreEqual(0, priorityQueue._queue.Count);

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Expected exception for empty queue after all dequeues.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
    }
}