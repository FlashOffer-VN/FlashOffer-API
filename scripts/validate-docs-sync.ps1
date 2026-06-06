# validate-docs-sync.ps1
# Kiá»ƒm tra cÃ¡c thay Ä‘á»•i liÃªn quan Ä‘áº¿n cáº¥u trÃºc/template vÃ  xÃ¡c nháº­n cÃ³ cáº­p nháº­t docs tÆ°Æ¡ng á»©ng.

$changedFiles = @()

if (-not (Test-Path ".git")) {
    Write-Host "KhÃ´ng tÃ¬m tháº¥y thÆ° má»¥c .git. Script nÃ y nÃªn Ä‘Æ°á»£c cháº¡y trong repository git." -ForegroundColor Yellow
    exit 0
}

$staged = git diff --cached --name-only
$unstaged = git diff --name-only
$changedFiles = ($staged + $unstaged) | Sort-Object -Unique

if ($changedFiles.Count -eq 0) {
    Write-Host "KhÃ´ng cÃ³ thay Ä‘á»•i trong working tree." -ForegroundColor Gray
    exit 0
}

$structuralFiles = @(
    'src/FlashOffer.API.Application/DependencyInjection.cs',
    'src/FlashOffer.API.Infrastructure/DependencyInjection.cs',
    'src/FlashOffer.API.WebApi/DependencyInjection.cs',
    'src/FlashOffer.API.Infrastructure/Data/ApplicationDbContext.cs',
    'src/FlashOffer.API.Application/Mappings/MappingProfile.cs',
    'src/FlashOffer.API.WebApi/Controllers/ApiControllerBase.cs',
    'src/FlashOffer.API.WebApi/Middlewares/GlobalExceptionMiddleware.cs',
    'src/FlashOffer.API.WebApi/Filters/ValidationFilter.cs',
    'scripts/rename-project.ps1',
    'scripts/init-template.ps1',
    'README.md',
    'docker-compose.yml',
    'Dockerfile',
    '.env.example',
    '.env.docker.example',
    'docs/FlashOffer.API.Documentation/README.md',
    'docs/FlashOffer.API.Documentation/core/00-ai-rules.md',
    'docs/FlashOffer.API.Documentation/guides/14-project-bootstrap.md',
    'docs/FlashOffer.API.Documentation/Prompts/01-Example-Prompt.md'
)

$docsPaths = @(
    'docs/FlashOffer.API.Documentation/',
    'README.md',
    'docs/FlashOffer.API.Documentation/README.md',
    'docs/FlashOffer.API.Documentation/core/',
    'docs/FlashOffer.API.Documentation/guides/',
    'docs/FlashOffer.API.Documentation/Prompts/'
)

$structuralChanged = $changedFiles | Where-Object {
    $relative = ($_ -replace '\\', '/')
    $structuralFiles | Where-Object { $relative -ieq $_ }
}

$docsChanged = $changedFiles | Where-Object {
    $relative = ($_ -replace '\\', '/')
    $docsPaths | Where-Object { $relative.StartsWith($_) -or $relative -ieq $_ }
}

if ($structuralChanged.Count -gt 0 -and $docsChanged.Count -eq 0) {
    Write-Host "WARNING: CÃ³ thay Ä‘á»•i cáº¥u trÃºc/template nhÆ°ng khÃ´ng tháº¥y cáº­p nháº­t docs." -ForegroundColor Yellow
    Write-Host "Changed structural files:" -ForegroundColor Yellow
    $structuralChanged | ForEach-Object { Write-Host "  - $_" }
    Write-Host "HÃ£y cáº­p nháº­t docs trong docs/FlashOffer.API.Documentation/ hoáº·c README.md vÃ  cháº¡y láº¡i script." -ForegroundColor Yellow
    exit 1
}

Write-Host "Docs sync check passed." -ForegroundColor Green
exit 0
