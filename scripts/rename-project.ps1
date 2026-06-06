param(
    [Parameter(Mandatory = $true)]
    [string]$NewProjectName
)

$OldProjectName = "FlashOffer.API"
$RootPath = "."

Write-Host "======================================================================" -ForegroundColor Cyan
Write-Host "ĐỔI TÊN PROJECT TỪ $OldProjectName THÀNH $NewProjectName" -ForegroundColor Yellow
Write-Host "======================================================================" -ForegroundColor Cyan

# 1. Đổi tên thư mục
Write-Host "`n[1] Đổi tên thư mục..." -ForegroundColor Green

$folders = @(
    "src/$OldProjectName.Domain",
    "src/$OldProjectName.Application",
    "src/$OldProjectName.Infrastructure",
    "src/$OldProjectName.WebApi",
    "src/$OldProjectName.Shared",
    "tests/$OldProjectName.UnitTests",
    "tests/$OldProjectName.IntegrationTests",
    "docs/$OldProjectName.Documentation"
)

foreach ($folder in $folders) {
    $newFolder = $folder -replace $OldProjectName, $NewProjectName
    if (Test-Path $folder) {
        Write-Host "  Rename: $folder -> $newFolder" -ForegroundColor Gray
        Rename-Item -Path $folder -NewName (Split-Path $newFolder -Leaf) -ErrorAction SilentlyContinue
    }
}

# 2. Đổi tên file cứng có chứa tên project
Write-Host "`n[2] Đổi tên file chứa tên project..." -ForegroundColor Green
Get-ChildItem -Path $RootPath -Recurse -File | Where-Object { $_.Name -match $OldProjectName } | Sort-Object { $_.FullName.Length } -Descending | ForEach-Object {
    $newName = $_.Name -replace $OldProjectName, $NewProjectName
    if ($newName -ne $_.Name) {
        try {
            Rename-Item -Path $_.FullName -NewName $newName -ErrorAction Stop
            Write-Host "  File Rename: $($_.FullName) -> $newName" -ForegroundColor Gray
        }
        catch {
            Write-Host "  Skip Rename: $($_.FullName)" -ForegroundColor DarkGray
        }
    }
}

# 3. Thay thế nội dung trong tất cả file
Write-Host "`n[3] Thay thế nội dung trong các file..." -ForegroundColor Green

$extensions = @("*.cs", "*.csproj", "*.json", "*.yml", "*.yaml", "*.md", "*.props", "*.example", "*.http", "*.ps1")
$fileCount = 0

Get-ChildItem -Path $RootPath -Recurse -Include $extensions | ForEach-Object {
    try {
        $content = Get-Content $_.FullName -Raw -ErrorAction SilentlyContinue
        if ($content -match $OldProjectName) {
            $newContent = $content -replace $OldProjectName, $NewProjectName
            Set-Content -Path $_.FullName -Value $newContent -NoNewline -Encoding UTF8
            $fileCount++
            Write-Host "  Updated: $($_.FullName)" -ForegroundColor Gray
        }
    }
    catch {
        Write-Host "  Skip: $($_.FullName)" -ForegroundColor DarkGray
    }
}

Write-Host "`n  Đã cập nhật $fileCount file" -ForegroundColor Green

# 4. Xóa bin/obj cũ
Write-Host "`n[4] Dọn dẹp bin/obj cũ..." -ForegroundColor Green
Get-ChildItem -Path $RootPath -Recurse -Directory -Include bin, obj | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
Write-Host "  Đã xóa bin/obj folders" -ForegroundColor Gray

# 5. Build lại
Write-Host "`n[5] Build lại solution..." -ForegroundColor Green
if (Test-Path "$NewProjectName.sln") {
    dotnet restore "$NewProjectName.sln"
    dotnet build "$NewProjectName.sln"
}
else {
    Write-Host "  Không tìm thấy file solution đã đổi tên. Vui lòng build thủ công." -ForegroundColor Yellow
}

Write-Host "`n======================================================================" -ForegroundColor Cyan
Write-Host "HOÀN TẤT! Project đã được đổi tên thành: $NewProjectName" -ForegroundColor Yellow
Write-Host "======================================================================" -ForegroundColor Cyan
