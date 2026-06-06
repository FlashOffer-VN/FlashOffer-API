# Logging với Serilog - Ghi log có cấu trúc

## File chính

| File                 | Đường dẫn                                         | Vai trò                          |
|----------------------|---------------------------------------------------|----------------------------------|
| Program.cs           | src/FlashOffer-API.WebApi/Program.cs               | Cấu hình Serilog                 |

## Cấu hình hiện tại trong Program.cs

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

## Output

| Output    | Đường dẫn                                  | Mô tả                                                    |
|-----------|--------------------------------------------|----------------------------------------------------------|
| Console   | Hiển thị trực tiếp                         | Log trong cửa sổ console khi chạy ứng dụng               |
| File      | logs/api-YYYYMMDD.txt                      | Mỗi ngày tạo một file mới                                |

## Cách dùng trong code

### Trong Controller

private readonly ILogger<SampleController> _logger;

public SampleController(ILogger<SampleController> logger)
{
    _logger = logger;
}

[HttpGet]
public async Task<IActionResult> GetAll()
{
    _logger.LogInformation("Getting all products");
    var products = await _repository.GetAllAsync();
    _logger.LogInformation("Retrieved {Count} products", products.Count());
    return Ok(products);
}

### Trong Exception middleware

_logger.LogError(ex, "An unhandled exception occurred");

## Các mức log

| Level       | Method           | Khi nào dùng                                         |
|-------------|------------------|------------------------------------------------------|
| Trace       | LogTrace         | Chi tiết nhất, debug sâu                             |
| Debug       | LogDebug         | Debug trong quá trình phát triển                     |
| Information | LogInformation   | Thông tin hoạt động bình thường                      |
| Warning     | LogWarning       | Cảnh báo, có thể có vấn đề                           |
| Error       | LogError         | Lỗi nhưng ứng dụng vẫn chạy                          |
| Critical    | LogCritical      | Lỗi nghiêm trọng, ứng dụng có thể dừng               |

## Nâng cấp (tùy chọn)

| Nâng cấp        | Package                          | Cấu hình thêm                                                                          |
|-----------------|----------------------------------|----------------------------------------------------------------------------------------|
| Seq             | Serilog.Sinks.Seq                | .WriteTo.Seq("http://localhost:5341")                                                  |
| Elasticsearch   | Serilog.Sinks.ElasticSearch      | .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))) |
| Database        | Serilog.Sinks.MSSqlServer        | .WriteTo.MSSqlServer(connectionString, tableName)                                      |

## Lưu ý

- Log file tự động rotate theo ngày
- KHÔNG log thông tin nhạy cảm (password, token, credit card)
- Sử dụng structured logging: LogInformation("User {UserId} logged in", userId)