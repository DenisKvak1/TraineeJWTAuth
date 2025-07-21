namespace TraineeJWTAuth.Server.Responces;

public class ApiResponse
{
    public object? Data { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
}