public class IdGenerator
{
    private static int _lastId = 0;

    public static int GenerateId()
    {
        // Incrementa o último ID e retorna o novo ID
        return System.Threading.Interlocked.Increment(ref _lastId);
    }
}