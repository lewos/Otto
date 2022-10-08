namespace Otto.orders.Models
{
    public class QueueTasks
    {
        private readonly Queue<Task<int>> _tasks;
        public QueueTasks()
        {
            _tasks = new Queue<Task<int>>();
        }

        public void Enqueue(Task<int> item)
        {
            _tasks.Enqueue(item);
        }

        public Task<int> Dequeue()
        {
            return _tasks.Dequeue();
        }

        public int Count()
        {
            return _tasks.Count;
        }
    }
}
