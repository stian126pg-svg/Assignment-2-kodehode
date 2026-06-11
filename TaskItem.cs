public class TaskItem
{
    public string Title { get; set; } = "";

    public DateTime DueDate { get; set; }

    public bool Completed { get; set; }

    public bool IsDueSoon =>
        !Completed &&
        DueDate <= DateTime.Now.AddMinutes(30) &&
        DueDate > DateTime.Now;

    public bool IsOverdue =>
        !Completed &&
        DueDate <= DateTime.Now;
}