# Manual Deployment to gh-pages
# Run this if GitHub Actions fails

Write-Host ""
Write-Host "🎯 Blazing Darts - Manual GitHub Pages Deployment" -ForegroundColor Cyan
Write-Host "===================================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Build
Write-Host "📦 Building project..." -ForegroundColor Yellow
dotnet publish BlazingDartsForFriends/BlazingDartsForFriends.csproj -c Release -o ./publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build successful!" -ForegroundColor Green
Write-Host ""

# Step 2: Save current branch
$currentBranch = git branch --show-current
Write-Host "📌 Current branch: $currentBranch" -ForegroundColor Cyan
Write-Host ""

# Step 3: Switch to gh-pages
Write-Host "🔄 Switching to gh-pages branch..." -ForegroundColor Yellow

# Check if gh-pages exists remotely
$ghPagesExists = git ls-remote --heads origin gh-pages

if ($ghPagesExists) {
    Write-Host "   gh-pages branch exists, checking out..." -ForegroundColor Gray
    git fetch origin gh-pages
    git checkout gh-pages

    # Remove old files but keep .git
    Write-Host "   Cleaning old files..." -ForegroundColor Gray
    Get-ChildItem -Exclude .git,.gitignore | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "   Creating new gh-pages branch..." -ForegroundColor Gray
    git checkout --orphan gh-pages
    git rm -rf . 2>$null
}

Write-Host "✅ On gh-pages branch" -ForegroundColor Green
Write-Host ""

# Step 4: Copy files
Write-Host "📁 Copying build files..." -ForegroundColor Yellow
Copy-Item -Path "publish/wwwroot/*" -Destination . -Recurse -Force

# Step 5: Verify critical files
Write-Host ""
Write-Host "🔍 Verifying deployment files..." -ForegroundColor Yellow

$criticalFiles = @("index.html", "404.html", ".nojekyll", "IndexedDbAccessor.js")
$allGood = $true

foreach ($file in $criticalFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file MISSING!" -ForegroundColor Red
        $allGood = $false
    }
}

if (-not $allGood) {
    Write-Host ""
    Write-Host "⚠️  Some critical files are missing!" -ForegroundColor Yellow
    Write-Host "   Continuing anyway..." -ForegroundColor Gray
}

Write-Host ""

# Step 6: Add and commit
Write-Host "💾 Committing changes..." -ForegroundColor Yellow
git add .

$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
git commit -m "Deploy to GitHub Pages - $timestamp"

if ($LASTEXITCODE -ne 0) {
    Write-Host "⚠️  No changes to commit" -ForegroundColor Yellow
} else {
    Write-Host "✅ Changes committed" -ForegroundColor Green
}

Write-Host ""

# Step 7: Push
Write-Host "📤 Pushing to GitHub..." -ForegroundColor Yellow
git push origin gh-pages --force

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Deployment successful!" -ForegroundColor Green
    Write-Host ""
    Write-Host "🌐 Your site will be available at:" -ForegroundColor Cyan
    Write-Host "   https://xxc0br4xx.github.io/BlazingDartsForFriends/" -ForegroundColor White
    Write-Host ""
    Write-Host "⏱️  Changes may take 1-2 minutes to appear." -ForegroundColor Yellow
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "❌ Push failed!" -ForegroundColor Red
    Write-Host "   Check your GitHub permissions and try again." -ForegroundColor Yellow
    Write-Host ""
}

# Step 8: Return to original branch
Write-Host "🔄 Returning to $currentBranch branch..." -ForegroundColor Yellow
git checkout $currentBranch

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Back on $currentBranch" -ForegroundColor Green
} else {
    Write-Host "⚠️  Could not return to $currentBranch" -ForegroundColor Yellow
}

Write-Host ""

# Step 9: Clean up
Write-Host "🧹 Cleaning up..." -ForegroundColor Yellow
Remove-Item -Path "publish" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "✅ Cleanup complete" -ForegroundColor Green
Write-Host ""
Write-Host "🎯 Done! Check your site in a few minutes." -ForegroundColor Green
Write-Host ""
