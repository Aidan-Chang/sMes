namespace DataArchive.Provider;

public interface IProvider {

    public string Name { get; }

    public Direction Direction { get; }

}

public enum Direction {
    InputOutput = 1,
    Input = 2,
    Output = 3,
}

public class ExecutionResult {

    public bool Success { get; set; } = false;

    public Exception? Exception { get; set; } = null;
}

public class ExecutionResult<T> : ExecutionResult {

    public int Count { get; set; } = 0;

    public IEnumerable<T>? Data { get; set; } = null;

}