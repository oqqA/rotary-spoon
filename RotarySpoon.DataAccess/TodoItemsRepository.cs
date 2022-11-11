using System.Text.Json;

namespace RotarySpoon.DataAccess;

public class TodoItemsRepository
{
    private static string DirectoryName = "./todoList/";
    private static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    public static Guid Add(TodoItem newTodoItem)
    {
        var directoryPath = Path.Combine(BaseDirectory, DirectoryName);
        Directory.CreateDirectory(directoryPath);

        var todoItemId = Guid.NewGuid();

        var json = JsonSerializer.Serialize(
            newTodoItem with { Id = todoItemId },
            new JsonSerializerOptions { WriteIndented = true });

        var fileName = $"{todoItemId}.json";
        var filePath = Path.Combine(BaseDirectory, DirectoryName, fileName);


        File.WriteAllText(filePath, json);

        return todoItemId;
    }
    public static TodoItem[] Get()
    {
        var openLoops = new List<TodoItem>();

        var filesPath = Path.Combine(BaseDirectory, DirectoryName);
        if (!Directory.Exists(filesPath))
            return openLoops.ToArray();

        var files = Directory.GetFiles(filesPath);

        foreach (var file in files)
        {
            var json = File.ReadAllText(file);
            var openLoop = JsonSerializer.Deserialize<TodoItem>(json);

            if (openLoop == null)
            {
                throw new Exception("TodoItem cannot be deserializable.");
            }

            openLoops.Add(openLoop);
        }

        return openLoops.ToArray();
    }

    public static bool Update(Guid todoItemId, string newText)
    {
        var fileName = $"{todoItemId}.json";
        var filePath = Path.Combine(BaseDirectory, DirectoryName, fileName);

        if (!File.Exists(filePath))
            return false;

        var json = File.ReadAllText(filePath);
        var todoItem = JsonSerializer.Deserialize<TodoItem>(json);

        if (todoItem == null)
            throw new Exception("TodoItem cannot be deserializable.");

        File.Delete(filePath);

        var newTodoItem = new TodoItem
        {
            Id = todoItem.Id,
            Text = newText,
            CreatedDate = todoItem.CreatedDate
        };

        var newJson = JsonSerializer.Serialize(newTodoItem, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, newJson);

        return true;
    }

    public static bool Close(Guid todoItemId)
    {
        var fileName = $"{todoItemId}.json";
        var filePath = Path.Combine(BaseDirectory, DirectoryName, fileName);

        if (!File.Exists(filePath))
            return false;

        File.Delete(filePath);

        return true;
    }
}