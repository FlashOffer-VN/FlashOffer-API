param(
    [switch]$Force,
    [switch]$RunTests,
    [switch]$UseDockerEnv
)

$solutionFile = Get-ChildItem -Path "." -Filter "*.slnx" | Select-Object -First 1
if ($null -eq $solutionFile) {
    Write-Error "Khong tim thay solution file (.slnx) trong thu muc hien tai."
    exit 1
}

if ((-not (Test-Path ".env")) -or $Force) {
    if (Test-Path ".env.example") {
        Copy-Item -Path ".env.example" -Destination ".env" -Force:$Force
        Write-Host "Tao file .env tu .env.example" -ForegroundColor Green
    }
    else {
        Write-Host "Khong tim thay .env.example. Bo qua." -ForegroundColor Yellow
    }
}
else {
    Write-Host ".env da ton tai, giu nguyen." -ForegroundColor Gray
}

if ($UseDockerEnv) {
    if ((-not (Test-Path ".env.docker")) -or $Force) {
        if (Test-Path ".env.docker.example") {
            Copy-Item -Path ".env.docker.example" -Destination ".env.docker" -Force:$Force
            Write-Host "Tao file .env.docker tu .env.docker.example" -ForegroundColor Green
        }
        else {
            Write-Host "Khong tim thay .env.docker.example. Bo qua." -ForegroundColor Yellow
        }
    }
    else {
        Write-Host ".env.docker da ton tai, giu nguyen." -ForegroundColor Gray
    }
}

Write-Host "Khoi tao template: restore + build" -ForegroundColor Cyan

dotnet restore $solutionFile.Name
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet restore that bai."
    exit $LASTEXITCODE
}

dotnet build $solutionFile.Name -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet build that bai."
    exit $LASTEXITCODE
}

if ($RunTests) {
    Write-Host "Chay test..." -ForegroundColor Cyan
    dotnet test $solutionFile.Name -c Release
    if ($LASTEXITCODE -ne 0) {
        Write-Error "dotnet test that bai."
        exit $LASTEXITCODE
    }
}

Write-Host "Khoi tao template hoan tat." -ForegroundColor Green
Write-Host "Su dung 'dotnet run' trong thu muc src/YourProject.WebApi de chay API." -ForegroundColor Cyan