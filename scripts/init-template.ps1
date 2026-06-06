param(
    [switch]$Force,
    [switch]$RunTests,
    [switch]$UseDockerEnv
)

$solutionFile = Get-ChildItem -Path "." -Filter "*.slnx" | Select-Object -First 1
if ($null -eq $solutionFile) {
    Write-Error "Không tìm thấy solution file (.slnx) trong thư mục hiện tại. Hãy chạy lại từ thư mục gốc của repo."
    exit 1
}

if ((-not (Test-Path ".env")) -or $Force) {
    if (Test-Path ".env.example") {
        Copy-Item -Path ".env.example" -Destination ".env" -Force:$Force
        Write-Host "Tạo file .env từ .env.example" -ForegroundColor Green
    }
    else {
        Write-Host "Không tìm thấy .env.example. Bỏ qua bước tạo .env." -ForegroundColor Yellow
    }
}
else {
    Write-Host ".env đã tồn tại, giữ nguyên." -ForegroundColor Gray
}

if ($UseDockerEnv) {
    if ((-not (Test-Path ".env.docker")) -or $Force) {
        if (Test-Path ".env.docker.example") {
            Copy-Item -Path ".env.docker.example" -Destination ".env.docker" -Force:$Force
            Write-Host "Tạo file .env.docker từ .env.docker.example" -ForegroundColor Green
        }
        else {
            Write-Host "Không tìm thấy .env.docker.example. Bỏ qua bước tạo .env.docker." -ForegroundColor Yellow
        }
    }
    else {
        Write-Host ".env.docker đã tồn tại, giữ nguyên." -ForegroundColor Gray
    }
}

Write-Host "Khởi tạo template: restore + build" -ForegroundColor Cyan

dotnet restore $solutionFile.Name
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet restore thất bại.";
    exit $LASTEXITCODE
}

dotnet build $solutionFile.Name -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet build thất bại.";
    exit $LASTEXITCODE
}

if ($RunTests) {
    Write-Host "Chạy test..." -ForegroundColor Cyan
    dotnet test $solutionFile.Name -c Release
    if ($LASTEXITCODE -ne 0) {
        Write-Error "dotnet test thất bại.";
        exit $LASTEXITCODE
    }
}

Write-Host "\nTemplate khởi tạo hoàn tất." -ForegroundColor Green
Write-Host "Sử dụng \`dotnet run\` trong thư mục src/YourProject.WebApi để chạy API." -ForegroundColor Cyan
