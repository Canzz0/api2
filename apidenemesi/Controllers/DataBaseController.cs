using Microsoft.AspNetCore.Mvc;

public class MyController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public MyController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public IActionResult MyAction()
    {
        using (var connection = _databaseService.GetConnection())
        {
            // Bağlantıyı kullanarak işlemleri gerçekleştirin
        }

        return Ok();
    }
}
